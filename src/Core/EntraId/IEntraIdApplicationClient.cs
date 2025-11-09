//-----------------------------------------------------------------------
// <copyright file="IEntraIdApplicationClient.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public interface IEntraIdApplicationClient
    {
        Task<IReadOnlyList<EntraIdApplication>> GetAsync(string tenantId, CancellationToken cancellationToken = default);
    }
}
