//-----------------------------------------------------------------------
// <copyright file="EntraIdTenant.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public class EntraIdTenant
    {
        public EntraIdTenant(string id, string displayName, IReadOnlyList<EntraIdApplication> applications)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.Applications = new ReadOnlyCollection<EntraIdApplication>(applications.ToArray());
        }

        public string Id { get; }

        public string DisplayName { get; }

        public ReadOnlyCollection<EntraIdApplication> Applications { get; }
    }
}