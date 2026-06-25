//-----------------------------------------------------------------------
// <copyright file="GraphServiceClientFactory.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    using global::Azure.Core;
    using global::Azure.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.Graph;

    public class GraphServiceClientFactory : IGraphServiceClientFactory
    {
        private readonly GraphEntraIdClientOptions options;

        public GraphServiceClientFactory(IOptions<GraphEntraIdClientOptions> options)
        {
            this.options = options.Value;
        }

        public GraphServiceClient Create(string tenantId)
        {
            TokenCredential credential;

            if (this.options.ClientId is null)
            {
                credential = new ManagedIdentityCredential(new ManagedIdentityCredentialOptions());
            }
            else
            {
                credential = new ClientSecretCredential(tenantId, this.options.ClientId, this.options.ClientSecret);
            }

            return new GraphServiceClient(credential);
        }
    }
}