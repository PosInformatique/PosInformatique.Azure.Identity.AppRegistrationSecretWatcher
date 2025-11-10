//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultApplicationSecret.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public class AppRegistrationSecretCheckResultApplicationSecret
    {
        public AppRegistrationSecretCheckResultApplicationSecret(string displayName, DateTime endDate, int daysBeforeExpiration)
        {
            this.DisplayName = displayName;
            this.EndDate = endDate;
            this.DaysBeforeExpiration = daysBeforeExpiration;
        }

        public string DisplayName { get; }

        public DateTime EndDate { get; }

        public AppRegistrationSecretStatus Status { get; set; }

        public int DaysBeforeExpiration { get; }
    }
}