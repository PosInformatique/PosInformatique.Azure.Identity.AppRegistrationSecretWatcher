//-----------------------------------------------------------------------
// <copyright file="EntraIdTenantTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId.Tests
{
    public class EntraIdTenantTest
    {
        [Fact]
        public void Constructor()
        {
            var applications = new[]
            {
                new EntraIdApplication(default, default, []),
                new EntraIdApplication(default, default, []),
            };

            var tenant = new EntraIdTenant("The id", "The display name", applications);

            tenant.Applications.Should().Equal(applications);
            tenant.DisplayName.Should().Be("The display name");
            tenant.Id.Should().Be("The id");
        }
    }
}