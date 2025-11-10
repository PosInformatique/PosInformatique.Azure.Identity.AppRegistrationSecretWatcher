//-----------------------------------------------------------------------
// <copyright file="EntraIdApplication.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public class EntraIdApplication
    {
        public EntraIdApplication(string id, string displayName, IReadOnlyList<EntraIdApplicationPasswordCredential> passwordCredentials)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.PasswordCredentials = new ReadOnlyCollection<EntraIdApplicationPasswordCredential>(passwordCredentials.ToArray());
        }

        public string Id { get; }

        public string DisplayName { get; }

        public ReadOnlyCollection<EntraIdApplicationPasswordCredential> PasswordCredentials { get; }
    }
}