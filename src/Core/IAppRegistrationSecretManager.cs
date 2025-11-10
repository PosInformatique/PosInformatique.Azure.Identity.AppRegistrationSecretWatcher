//-----------------------------------------------------------------------
// <copyright file="IAppRegistrationSecretManager.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public interface IAppRegistrationSecretManager
    {
        Task<AppRegistrationSecretCheckResult> CheckAsync(AppRegistrationSecretCheckParameters parameters, CancellationToken cancellationToken = default);
    }
}
