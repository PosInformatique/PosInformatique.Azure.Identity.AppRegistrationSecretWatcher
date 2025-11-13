//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultTenant.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public class AppRegistrationSecretCheckResultTenant
    {
        public AppRegistrationSecretCheckResultTenant(string id, string displayName, IReadOnlyCollection<AppRegistrationSecretCheckResultApplication> applications)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.Applications = new ReadOnlyCollection<AppRegistrationSecretCheckResultApplication>(applications.ToArray());
        }

        public string Id { get; }

        public string DisplayName { get; }

        public ReadOnlyCollection<AppRegistrationSecretCheckResultApplication> Applications { get; }
    }
}