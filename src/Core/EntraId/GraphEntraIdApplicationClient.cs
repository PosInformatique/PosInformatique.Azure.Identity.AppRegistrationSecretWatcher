//-----------------------------------------------------------------------
// <copyright file="GraphEntraIdApplicationClient.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public class GraphEntraIdApplicationClient : IEntraIdApplicationClient
    {
        private readonly IGraphServiceClientFactory graphServiceClientFactory;

        public GraphEntraIdApplicationClient(IGraphServiceClientFactory graphServiceClientFactory)
        {
            this.graphServiceClientFactory = graphServiceClientFactory;
        }

        public async Task<IReadOnlyList<EntraIdApplication>> GetAsync(string tenantId, CancellationToken cancellationToken = default)
        {
            using var graphClient = this.graphServiceClientFactory.Create(tenantId);

            var applications = await graphClient.Applications.GetAsync(
                request =>
                {
                    request.QueryParameters.Select = ["appId", "displayName", "passwordCredentials"];
                },
                cancellationToken);

            if (applications is null || applications.Value is null)
            {
                throw new InvalidOperationException($"Unable to retrieve the list of the app registrations for the tenant '{tenantId}'.");
            }

            return applications.Value
                .Select(app => new EntraIdApplication(app.AppId!, app.DisplayName!, app.PasswordCredentials!.Select(pc => new EntraIdApplicationPasswordCredential(pc.DisplayName!, pc.EndDateTime!.Value)).ToArray()))
                .ToArray();
        }
    }
}
