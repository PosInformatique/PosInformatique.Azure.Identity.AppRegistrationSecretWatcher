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
                new AppRegistrationSecretCheckResultTenant(
                    default,
                    default,
                    [
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
                    ]),
                new AppRegistrationSecretCheckResultTenant(
                    default,
                    default,
                    [
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
                    ]),
            };

            var dateTime = new DateTime(2024, 1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc);

            var tenant = new AppRegistrationSecretCheckResult(tenants, dateTime);

            tenant.DateTime.Should().Be(dateTime);
            tenant.HasExpiredSecrets.Should().BeFalse();
            tenant.Tenants.Should().Equal(tenants);
        }

        [Fact]
        public void HasExpiredSecrets()
        {
            var tenants = new[]
            {
                new AppRegistrationSecretCheckResultTenant(
                    default,
                    default,
                    [
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
                    ]),
                new AppRegistrationSecretCheckResultTenant(
                    default,
                    default,
                    [
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
                    ]),
            };

            var dateTime = new DateTime(2024, 1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc);

            var tenant = new AppRegistrationSecretCheckResult(tenants, dateTime);

            tenant.DateTime.Should().Be(dateTime);
            tenant.HasExpiredSecrets.Should().BeTrue();
            tenant.Tenants.Should().Equal(tenants);
        }
    }
}