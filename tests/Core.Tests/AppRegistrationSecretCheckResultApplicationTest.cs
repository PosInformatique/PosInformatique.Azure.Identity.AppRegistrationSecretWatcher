//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultApplicationTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    public class AppRegistrationSecretCheckResultApplicationTest
    {
        [Fact]
        public void Constructor()
        {
            var secrets = new[]
            {
                new AppRegistrationSecretCheckResultApplicationSecret(default, default, default),
                new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.ExpiringSoon },
            };

            var application = new AppRegistrationSecretCheckResultApplication("The ID", "The display name", secrets);

            application.DisplayName.Should().Be("The display name");
            application.HasExpiredSecrets.Should().BeFalse();
            application.Id.Should().Be("The ID");
            application.Secrets.Should().Equal(secrets);
        }

        [Fact]
        public void HasExpiredSecrets()
        {
            var secrets = new[]
            {
                new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.Expired },
                new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.ExpiringSoon },
            };

            var application = new AppRegistrationSecretCheckResultApplication("The ID", "The display name", secrets);

            application.DisplayName.Should().Be("The display name");
            application.HasExpiredSecrets.Should().BeTrue();
            application.Id.Should().Be("The ID");
            application.Secrets.Should().Equal(secrets);
        }
    }
}