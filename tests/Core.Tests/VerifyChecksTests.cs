//-----------------------------------------------------------------------
// <copyright file="VerifyChecksTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Core.Tests
{
    public class VerifyChecksTests
    {
        [Fact]
        public Task Run() => VerifyChecks.Run();
    }
}