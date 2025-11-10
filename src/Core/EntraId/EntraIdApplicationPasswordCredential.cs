//-----------------------------------------------------------------------
// <copyright file="EntraIdApplicationPasswordCredential.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public class EntraIdApplicationPasswordCredential
    {
        public EntraIdApplicationPasswordCredential(string displayName, DateTime endDateTime)
        {
            Guard.IsUtc(endDateTime, nameof(endDateTime));

            this.DisplayName = displayName;
            this.EndDateTime = endDateTime;
        }

        public string DisplayName { get; }

        public DateTime EndDateTime { get; }
    }
}
