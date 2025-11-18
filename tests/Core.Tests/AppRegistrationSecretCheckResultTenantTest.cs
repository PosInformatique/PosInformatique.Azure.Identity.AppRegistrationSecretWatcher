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
                new AppRegistrationSecretCheckResultApplication(
                    default,
                    default,
                    [
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.Valid },
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.ExpiringSoon },
                    ]),
                new AppRegistrationSecretCheckResultApplication(
                    default,
                    default,
                    [
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.ExpiringSoon },
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.Valid },
                    ]),
            };

            var tenant = new AppRegistrationSecretCheckResultTenant("The ID", "The display name", applications);

            tenant.Applications.Should().Equal(applications);
            tenant.DisplayName.Should().Be("The display name");
            tenant.HasExpiredSecrets.Should().BeFalse();
            tenant.Id.Should().Be("The ID");
        }

        [Fact]
        public void HasExpiredSecrets()
        {
            var applications = new[]
            {
                new AppRegistrationSecretCheckResultApplication(
                    default,
                    default,
                    [
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.Expired },
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.ExpiringSoon },
                    ]),
                new AppRegistrationSecretCheckResultApplication(
                    default,
                    default,
                    [
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.ExpiringSoon },
                        new AppRegistrationSecretCheckResultApplicationSecret(default, default, default) { Status = AppRegistrationSecretStatus.Valid },
                    ]),
            };

            var tenant = new AppRegistrationSecretCheckResultTenant("The ID", "The display name", applications);

            tenant.Applications.Should().Equal(applications);
            tenant.DisplayName.Should().Be("The display name");
            tenant.HasExpiredSecrets.Should().BeTrue();
            tenant.Id.Should().Be("The ID");
        }
    }
}