//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResult.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public class AppRegistrationSecretCheckResult
    {
        public AppRegistrationSecretCheckResult(IReadOnlyList<AppRegistrationSecretCheckResultTenant> tenants)
        {
            this.Tenants = new ReadOnlyCollection<AppRegistrationSecretCheckResultTenant>(tenants.ToArray());
        }

        public ReadOnlyCollection<AppRegistrationSecretCheckResultTenant> Tenants { get; }
    }
}