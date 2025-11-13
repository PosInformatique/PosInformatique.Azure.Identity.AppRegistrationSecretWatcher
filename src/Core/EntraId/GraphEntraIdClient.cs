//-----------------------------------------------------------------------
// <copyright file="GraphEntraIdClient.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId
{
    public class GraphEntraIdClient : IEntraIdClient
    {
        private readonly IGraphServiceClientFactory graphServiceClientFactory;

        public GraphEntraIdClient(IGraphServiceClientFactory graphServiceClientFactory)
        {
            this.graphServiceClientFactory = graphServiceClientFactory;
        }

        public async Task<EntraIdTenant> GetApplicationsAsync(string tenantId, CancellationToken cancellationToken = default)
        {
            using var graphClient = this.graphServiceClientFactory.Create(tenantId);

            var tenant = await graphClient.Organization[tenantId].GetAsync(
                request =>
                {
                    request.QueryParameters.Select = ["id", "displayName"];
                },
                cancellationToken);

            if (tenant is null)
            {
                throw new InvalidOperationException($"Unable to retrieve the tenant '{tenantId}'.");
            }

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

            var entraIdApplications = applications.Value
                .Select(app => new EntraIdApplication(app.AppId!, app.DisplayName!, app.PasswordCredentials!.Select(pc => new EntraIdApplicationPasswordCredential(pc.DisplayName!, pc.EndDateTime!.Value.UtcDateTime)).ToArray()))
                .ToArray();

            return new EntraIdTenant(tenant.Id!, tenant.DisplayName!, entraIdApplications);
        }
    }
}