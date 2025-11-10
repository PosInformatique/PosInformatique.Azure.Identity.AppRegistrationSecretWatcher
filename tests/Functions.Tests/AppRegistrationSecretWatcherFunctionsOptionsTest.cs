//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretWatcherFunctionsOptionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Functions.Tests
{
    public class AppRegistrationSecretWatcherFunctionsOptionsTest
    {
        [Fact]
        public void Constructor()
        {
            var options = new AppRegistrationSecretWatcherFunctionsOptions();

            options.ExpirationThreshold.Should().Be(TimeSpan.Zero);
            options.TenantIds.Should().BeEmpty();
        }

        [Fact]
        public void ExpirationThreshold_ValueChanged()
        {
            var options = new AppRegistrationSecretWatcherFunctionsOptions();

            options.ExpirationThreshold = TimeSpan.FromDays(4);

            options.ExpirationThreshold.Should().Be(TimeSpan.FromDays(4));
        }
    }
}