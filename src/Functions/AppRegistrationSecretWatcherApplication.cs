//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretWatcherApplication.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Functions
{
    using System.Globalization;
    using global::Azure.Identity;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Graph;
    using PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing;
    using PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId;
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.Emailing;
    using PosInformatique.Foundations.Emailing.Graph;

    public static class AppRegistrationSecretWatcherApplication
    {
        public static async Task Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            string? appSecretWatcherClientId;
            string? appSecretWatcherClientSecret;

            // Check the APP_SECRET_WATCHER_CLIENT_ID and  APP_SECRET_WATCHER_CLIENT_SECRET
            if (!string.IsNullOrWhiteSpace(builder.Configuration["APP_SECRET_WATCHER_CLIENT_ID"]))
            {
                if (string.IsNullOrWhiteSpace(builder.Configuration["APP_SECRET_WATCHER_CLIENT_SECRET"]))
                {
                    throw new InvalidOperationException("No client secret has been specified for the app registrations watcher (Missing setting: APP_SECRET_WATCHER_CLIENT_SECRET).");
                }

                appSecretWatcherClientId = builder.Configuration["APP_SECRET_WATCHER_CLIENT_ID"];
                appSecretWatcherClientSecret = builder.Configuration["APP_SECRET_WATCHER_CLIENT_SECRET"];
            }
            else
            {
                appSecretWatcherClientId = null;
                appSecretWatcherClientSecret = null;
            }

            // Check the APP_SECRET_WATCHER_TENANT_IDS
            if (string.IsNullOrWhiteSpace(builder.Configuration["APP_SECRET_WATCHER_TENANT_IDS"]))
            {
                throw new InvalidOperationException("No tenant ids has been specified for the app registrations watcher (Missing setting: APP_SECRET_WATCHER_TENANT_IDS).");
            }

            var tenantIds = builder.Configuration["APP_SECRET_WATCHER_TENANT_IDS"]!.Split(';', StringSplitOptions.RemoveEmptyEntries);

            // Check APP_SECRET_WATCHER_SENDER_EMAIL
            if (string.IsNullOrWhiteSpace(builder.Configuration["APP_SECRET_WATCHER_SENDER_EMAIL"]))
            {
                throw new InvalidOperationException("No email sender has been specified for the app registrations watcher (Missing setting: APP_SECRET_WATCHER_SENDER_EMAIL).");
            }

            if (!EmailAddress.TryParse(builder.Configuration["APP_SECRET_WATCHER_SENDER_EMAIL"], out var emailSender))
            {
                throw new InvalidOperationException("The email sender specified for the app registrations watcher is invalid (Invalid setting: APP_SECRET_WATCHER_SENDER_EMAIL).");
            }

            // Check APP_SECRET_WATCHER_RECIPIENTS_EMAIL
            if (string.IsNullOrWhiteSpace(builder.Configuration["APP_SECRET_WATCHER_RECIPIENTS_EMAIL"]))
            {
                throw new InvalidOperationException("No recipient emails address has been specified for the app registrations watcher (Missing setting: APP_SECRET_WATCHER_RECIPIENTS_EMAIL).");
            }

            var recipientEmailAddresses = builder.Configuration["APP_SECRET_WATCHER_RECIPIENTS_EMAIL"]!.Split(';', StringSplitOptions.RemoveEmptyEntries);

            // Check APP_SECRET_WATCHER_EXPIRATION_THRESHOLD
            var expirationThreshold = TimeSpan.Zero;

            if (!string.IsNullOrWhiteSpace(builder.Configuration["APP_SECRET_WATCHER_EXPIRATION_THRESHOLD"]))
            {
                if (!TimeSpan.TryParse(builder.Configuration["APP_SECRET_WATCHER_EXPIRATION_THRESHOLD"], CultureInfo.InvariantCulture, out var expirationThresholdParsed))
                {
                    throw new InvalidOperationException("The expiration threshold specified for the app registrations watcher is invalid (Invalid setting: APP_SECRET_WATCHER_EXPIRATION_THRESHOLD).");
                }

                expirationThreshold = expirationThresholdParsed;
            }

            builder.ConfigureFunctionsWebApplication();

            // Infrastructure
            builder.Services.AddSingleton(TimeProvider.System);

            // Add Application Insights
            builder.Services
                .AddApplicationInsightsTelemetryWorkerService()
                .ConfigureFunctionsApplicationInsights();

            // Emailing
            builder.Services.AddSingleton<IEmailGenerator, ScribanEmailGenerator>();
            builder.Services.AddSingleton<IEmailProvider, GraphEmailProvider>();

            // Graph API
            builder.Services.AddSingleton<IEntraIdClient, GraphEntraIdClient>();
            builder.Services.AddSingleton<IGraphServiceClientFactory, GraphServiceClientFactory>();
            builder.Services.AddSingleton(sp =>
            {
                return new GraphServiceClient(new DefaultAzureCredential());
            });

            builder.Services.Configure<GraphEntraIdClientOptions>(opt =>
            {
                opt.ClientId = appSecretWatcherClientId;
                opt.ClientSecret = appSecretWatcherClientSecret;
            });

            // App registrations secret manager
            builder.Services.AddSingleton<IAppRegistrationSecretManager, AppRegistrationSecretManager>();

            builder.Services.Configure<AppRegistrationSecretManagerOptions>(opt =>
            {
                opt.EmailSender = emailSender;

                foreach (var recipientEmailAddress in recipientEmailAddresses)
                {
                    opt.EmailRecipients.Add(recipientEmailAddress);
                }
            });

            // Function
            builder.Services.Configure<AppRegistrationSecretWatcherFunctionsOptions>(opt =>
            {
                foreach (var tenantId in tenantIds)
                {
                    opt.TenantIds.Add(tenantId);
                }

                opt.ExpirationThreshold = expirationThreshold;
            });

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}