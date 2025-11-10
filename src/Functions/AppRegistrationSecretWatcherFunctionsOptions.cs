//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretWatcherFunctionsOptions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Functions
{
    public class AppRegistrationSecretWatcherFunctionsOptions
    {
        public AppRegistrationSecretWatcherFunctionsOptions()
        {
            this.TenantIds = new Collection<string>();
        }

        public Collection<string> TenantIds { get; }

        public TimeSpan ExpirationThreshold { get; set; }
    }
}