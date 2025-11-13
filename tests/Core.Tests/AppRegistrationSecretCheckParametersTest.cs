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
            var parameters = new AppRegistrationSecretCheckParameters();

            parameters.ExpirationThreshold.Should().Be(TimeSpan.Zero);
            parameters.TenantIds.Should().BeEmpty();
        }

        [Fact]
        public void ExpirationThreshold_ValueChanged()
        {
            var parameters = new AppRegistrationSecretCheckParameters();

            parameters.ExpirationThreshold = TimeSpan.FromDays(4);

            parameters.ExpirationThreshold.Should().Be(TimeSpan.FromDays(4));
        }
    }
}