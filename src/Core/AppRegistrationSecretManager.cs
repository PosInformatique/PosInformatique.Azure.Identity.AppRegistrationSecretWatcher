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
        private readonly IEntraIdClient entraIdApplicationClient;

        private readonly IEmailProvider emailProvider;

        private readonly IEmailGenerator emailGenerator;

        private readonly AppRegistrationSecretManagerOptions options;

        private readonly TimeProvider timeProvider;

        public AppRegistrationSecretManager(IEntraIdClient entraIdApplicationClient, IEmailProvider emailProvider, IEmailGenerator emailGenerator, TimeProvider timeProvider, IOptions<AppRegistrationSecretManagerOptions> options)
        {
            this.entraIdApplicationClient = entraIdApplicationClient;
            this.emailProvider = emailProvider;
            this.emailGenerator = emailGenerator;
            this.timeProvider = timeProvider;
            this.options = options.Value;
        }

        public async Task<AppRegistrationSecretCheckResult> CheckAsync(AppRegistrationSecretCheckParameters parameters, CancellationToken cancellationToken = default)
        {
            var now = this.timeProvider.GetUtcNow().UtcDateTime;

            // Gets the applications
            var applicationsByTenant = await this.GetApplicationsByTenantAsync(parameters, cancellationToken);

            // Check the result
            var result = this.BuildResult(applicationsByTenant, now, parameters.ExpirationThreshold);

            // Generate the e-mail
            var emailContent = await this.emailGenerator.GenerateAsync(result, cancellationToken);

            // Send e-mail
            await this.SendEmailAsync(emailContent, now, cancellationToken);

            return result;
        }

        private async Task<IReadOnlyList<EntraIdTenant>> GetApplicationsByTenantAsync(AppRegistrationSecretCheckParameters parameters, CancellationToken cancellationToken)
        {
            var tenantTasks = new List<Task<EntraIdTenant>>(parameters.TenantIds.Count);

            foreach (var tenantId in parameters.TenantIds)
            {
                tenantTasks.Add(this.entraIdApplicationClient.GetApplicationsAsync(tenantId, cancellationToken));
            }

            await Task.WhenAll(tenantTasks);

            return tenantTasks.Select(t => t.Result).ToArray();
        }

        private AppRegistrationSecretCheckResult BuildResult(IReadOnlyList<EntraIdTenant> tenants, DateTime now, TimeSpan expirationThreshold)
        {
            var tenantsResult = new List<AppRegistrationSecretCheckResultTenant>(tenants.Count);

            foreach (var tenant in tenants)
            {
                var tenantResult = new AppRegistrationSecretCheckResultTenant(
                    tenant.Id,
                    tenant.DisplayName,
                    tenant.Applications
                        .Select(app => this.Build(app, now, expirationThreshold))
                        .OrderBy(app => app.DisplayName)
                        .ToArray());

                tenantsResult.Add(tenantResult);
            }

            return new AppRegistrationSecretCheckResult(tenantsResult);
        }

        private AppRegistrationSecretCheckResultApplication Build(EntraIdApplication application, DateTime now, TimeSpan expirationThreshold)
        {
            var secrets = application.PasswordCredentials
                .OrderBy(pc => pc.DisplayName)
                .Select(pc => this.Build(pc, now, expirationThreshold))
                .ToArray();

            return new AppRegistrationSecretCheckResultApplication(application.Id, application.DisplayName, secrets);
        }

        private AppRegistrationSecretCheckResultApplicationSecret Build(EntraIdApplicationPasswordCredential passwordCredential, DateTime now, TimeSpan expirationThreshold)
        {
            var localEndDateTime = TimeZoneInfo.ConvertTimeFromUtc(passwordCredential.EndDateTime, this.timeProvider.LocalTimeZone);
            localEndDateTime = DateTime.SpecifyKind(localEndDateTime, DateTimeKind.Local);

            var secret = new AppRegistrationSecretCheckResultApplicationSecret(passwordCredential.DisplayName, localEndDateTime, (passwordCredential.EndDateTime - now).Days);

            if (passwordCredential.EndDateTime < now)
            {
                secret.Status = AppRegistrationSecretStatus.Expired;
            }
            else if (passwordCredential.EndDateTime <= now + expirationThreshold)
            {
                secret.Status = AppRegistrationSecretStatus.ExpiringSoon;
            }

            return secret;
        }

        private async Task SendEmailAsync(string emailContent, DateTime now, CancellationToken cancellationToken)
        {
            var todayLocal = TimeZoneInfo.ConvertTimeFromUtc(now, this.timeProvider.LocalTimeZone);

            foreach (var recipient in this.options.EmailRecipients)
            {
                var message = new EmailMessage(
                    new EmailContact(this.options.EmailSender, string.Empty),
                    new EmailContact(recipient, string.Empty),
                    $"Reminder: App Registration secrets expiring soon - [{todayLocal:d}]",
                    emailContent);

                await this.emailProvider.SendAsync(message, cancellationToken);
            }
        }
    }
}