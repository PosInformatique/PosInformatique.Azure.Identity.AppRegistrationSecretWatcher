//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckParameters.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public class AppRegistrationSecretCheckParameters
    {
        public AppRegistrationSecretCheckParameters(DateTime expirationDateLimit)
        {
            Guard.IsUtc(expirationDateLimit, nameof(expirationDateLimit));

            this.ExpirationDateLimit = expirationDateLimit;
            this.TenantIds = new Collection<string>();
        }

        public Collection<string> TenantIds { get; }

        public DateTime ExpirationDateLimit { get; }
    }
}