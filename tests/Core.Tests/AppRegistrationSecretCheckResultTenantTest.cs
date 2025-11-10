//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultTenantTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    public class AppRegistrationSecretCheckResultTenantTest
    {
        [Fact]
        public void Constructor()
        {
            var applications = new[]
            {
                new AppRegistrationSecretCheckResultApplication(default, []),
                new AppRegistrationSecretCheckResultApplication(default, []),
            };

            var tenant = new AppRegistrationSecretCheckResultTenant("The ID", "The display name", applications);

            tenant.Applications.Should().Equal(applications);
            tenant.DisplayName.Should().Be("The display name");
            tenant.Id.Should().Be("The ID");
        }
    }
}