//-----------------------------------------------------------------------
// <copyright file="IEntraIdClient.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public interface IEntraIdClient
    {
        Task<EntraIdTenant> GetApplicationsAsync(string tenantId, CancellationToken cancellationToken = default);
    }
}