//-----------------------------------------------------------------------
// <copyright file="IGraphServiceClientFactory.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    using Microsoft.Graph;

    public interface IGraphServiceClientFactory
    {
        GraphServiceClient Create(string tenantId);
    }
}