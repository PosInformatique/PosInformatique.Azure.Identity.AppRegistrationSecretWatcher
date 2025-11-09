//-----------------------------------------------------------------------
// <copyright file="GraphEntraIdApplicationClientOptions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public class GraphEntraIdApplicationClientOptions
    {
        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }
    }
}
