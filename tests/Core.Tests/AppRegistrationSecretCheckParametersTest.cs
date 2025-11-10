//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckParametersTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    public class AppRegistrationSecretCheckParametersTest
    {
        [Fact]
        public void Constructor()
        {
            var parameters = new AppRegistrationSecretCheckParameters(new DateTime(2025, 1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc));

            parameters.ExpirationDateLimit.Should().Be(new DateTime(2025, 1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc));
            parameters.TenantIds.Should().BeEmpty();
        }
    }
}
