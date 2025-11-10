//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManager.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    using Microsoft.Extensions.Options;
    using PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing;
    using PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId;
    using PosInformatique.Foundations.Emailing;

    public class AppRegistrationSecretManager : IAppRegistrationSecretManager
    {
        private readonly IEntraIdApplicationClient entraIdApplicationClient;

        private readonly IEmailProvider emailProvider;

        private readonly IEmailGenerator emailGenerator;

        private readonly AppRegistrationSecretManagerOptions options;

        private readonly TimeProvider timeProvider;

        public AppRegistrationSecretManager(IEntraIdApplicationClient entraIdApplicationClient, IEmailProvider emailProvider, IEmailGenerator emailGenerator, TimeProvider timeProvider, IOptions<AppRegistrationSecretManagerOptions> options)
        {
            this.entraIdApplicationClient = entraIdApplicationClient;
            this.emailProvider = emailProvider;
            this.emailGenerator = emailGenerator;
            this.timeProvider = timeProvider;
            this.options = options.Value;
        }

        public async Task<AppRegistrationSecretCheckResult> CheckAsync(AppRegistrationSecretCheckParameters parameters, CancellationToken cancellationToken = default)
        {
            // Gets the applications
            var applicationsByTenant = await this.GetApplicationsByTenantAsync(parameters, cancellationToken);

            // Check the result
            var result = this.BuildResult(applicationsByTenant, parameters.ExpirationDateLimit);

            // Generate the e-mail
            var emailContent = this.emailGenerator.Generate(result);

            // Send e-mail
            await this.SendEmailAsync(emailContent, cancellationToken);

            return result;
        }

        private async Task<IReadOnlyList<TenantApplications>> GetApplicationsByTenantAsync(AppRegistrationSecretCheckParameters parameters, CancellationToken cancellationToken)
        {
            var applicationsTask = new List<Task<IReadOnlyList<EntraIdApplication>>>(parameters.TenantIds.Count);

            foreach (var tenantId in parameters.TenantIds)
            {
                applicationsTask.Add(this.entraIdApplicationClient.GetAsync(tenantId, cancellationToken));
            }

            await Task.WhenAll(applicationsTask);

            var result = new List<TenantApplications>(parameters.TenantIds.Count);

            for (int i = 0; i < parameters.TenantIds.Count; i++)
            {
                result.Add(new TenantApplications(parameters.TenantIds[i], applicationsTask[i].Result));
            }

            return result;
        }

        private AppRegistrationSecretCheckResult BuildResult(IReadOnlyList<TenantApplications> tenants, DateTime expirationDateLimit)
        {
            var tenantsResult = new List<AppRegistrationSecretCheckResultTenant>(tenants.Count);

            foreach (var tenant in tenants)
            {
                var tenantResult = new AppRegistrationSecretCheckResultTenant(
                    tenant.Id,
                    tenant.Applications
                        .Select(app => this.Build(app, expirationDateLimit))
                        .OrderBy(app => app.DisplayName)
                        .ToArray());

                tenantsResult.Add(tenantResult);
            }

            return new AppRegistrationSecretCheckResult(tenantsResult);
        }

        private AppRegistrationSecretCheckResultApplication Build(EntraIdApplication application, DateTime expirationDateLimit)
        {
            var secrets = application.PasswordCredentials
                .OrderBy(pc => pc.DisplayName)
                .Select(pc => this.Build(pc, expirationDateLimit))
                .ToArray();

            return new AppRegistrationSecretCheckResultApplication(application.DisplayName, secrets);
        }

        private AppRegistrationSecretCheckResultApplicationSecret Build(EntraIdApplicationPasswordCredential passwordCredential, DateTime expirationDateLimit)
        {
            var localEndDateTime = TimeZoneInfo.ConvertTimeFromUtc(passwordCredential.EndDateTime, this.timeProvider.LocalTimeZone);
            localEndDateTime = DateTime.SpecifyKind(localEndDateTime, DateTimeKind.Local);

            var secret = new AppRegistrationSecretCheckResultApplicationSecret(passwordCredential.DisplayName, localEndDateTime);

            if (passwordCredential.EndDateTime <= expirationDateLimit)
            {
                secret.Expired = true;
            }

            return secret;
        }

        private async Task SendEmailAsync(string emailContent, CancellationToken cancellationToken)
        {
            var todayLocal = this.timeProvider.GetLocalNow();

            foreach (var recipient in this.options.EmailRecipients)
            {
                var message = new EmailMessage(this.options.EmailSender, recipient, $"Reminder: App Registration secrets expiring soon - [{todayLocal:d}]", emailContent);

                await this.emailProvider.SendAsync(message, cancellationToken);
            }
        }

        private sealed class TenantApplications
        {
            public TenantApplications(string id, IReadOnlyList<EntraIdApplication> applications)
            {
                this.Applications = applications;
                this.Id = id;
            }

            public string Id { get; }

            public IReadOnlyList<EntraIdApplication> Applications { get; }
        }
    }
}