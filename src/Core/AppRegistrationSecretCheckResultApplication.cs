//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultApplication.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public class AppRegistrationSecretCheckResultApplication
    {
        public AppRegistrationSecretCheckResultApplication(string displayName, IReadOnlyList<AppRegistrationSecretCheckResultApplicationSecret> secrets)
        {
            this.DisplayName = displayName;
            this.Secrets = new ReadOnlyCollection<AppRegistrationSecretCheckResultApplicationSecret>(secrets.ToArray());
        }

        public string DisplayName { get; }

        public ReadOnlyCollection<AppRegistrationSecretCheckResultApplicationSecret> Secrets { get; }
    }
}
