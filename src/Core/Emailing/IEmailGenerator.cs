//-----------------------------------------------------------------------
// <copyright file="IEmailGenerator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing
{
    public interface IEmailGenerator
    {
        Task<string> GenerateAsync(AppRegistrationSecretCheckResult result, CancellationToken cancellationToken = default);
    }
}