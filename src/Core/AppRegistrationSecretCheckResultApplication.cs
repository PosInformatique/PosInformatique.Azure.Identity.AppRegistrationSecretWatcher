//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultApplication.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public class AppRegistrationSecretCheckResultApplication
    {
        public AppRegistrationSecretCheckResultApplication(string id, string displayName, IReadOnlyList<AppRegistrationSecretCheckResultApplicationSecret> secrets)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.Secrets = new ReadOnlyCollection<AppRegistrationSecretCheckResultApplicationSecret>(secrets.ToArray());
        }

        public string Id { get; }

        public string DisplayName { get; }

        public ReadOnlyCollection<AppRegistrationSecretCheckResultApplicationSecret> Secrets { get; }
    }
}