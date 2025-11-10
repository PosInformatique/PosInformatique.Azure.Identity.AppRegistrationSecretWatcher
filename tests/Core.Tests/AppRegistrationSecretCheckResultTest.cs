//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Core.Tests
{
    public class AppRegistrationSecretCheckResultTest
    {
        [Fact]
        public void Constructor()
        {
            var tenants = new[]
            {
                new AppRegistrationSecretCheckResultTenant(default, []),
                new AppRegistrationSecretCheckResultTenant(default, []),
            };

            var tenant = new AppRegistrationSecretCheckResult(tenants);

            tenant.Tenants.Should().Equal(tenants);
        }
    }
}