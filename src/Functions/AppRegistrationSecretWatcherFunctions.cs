//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretWatcherFunctions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Functions
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Options;

    public class AppRegistrationSecretWatcherFunctions
    {
        private readonly IAppRegistrationSecretManager manager;

        private readonly AppRegistrationSecretWatcherFunctionsOptions options;

        public AppRegistrationSecretWatcherFunctions(IAppRegistrationSecretManager manager, IOptions<AppRegistrationSecretWatcherFunctionsOptions> options)
        {
            this.manager = manager;
            this.options = options.Value;
        }

        [Function("WatchAppSecrets")]
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public async Task WatchAppSecretsAsync([TimerTrigger("%APP_SECRET_WATCHER_FREQUENCY%")] TimerInfo _, CancellationToken cancellationToken)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            var parameters = new AppRegistrationSecretCheckParameters()
            {
                ExpirationThreshold = this.options.ExpirationThreshold,
            };

            foreach (var tenantId in this.options.TenantIds)
            {
                parameters.TenantIds.Add(tenantId);
            }

            await this.manager.CheckAsync(parameters, cancellationToken);
        }
    }
}