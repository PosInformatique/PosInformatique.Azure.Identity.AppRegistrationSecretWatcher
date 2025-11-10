//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretCheckResultApplicationSecretTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    public class AppRegistrationSecretCheckResultApplicationSecretTest
    {
        [Fact]
        public void Constructor()
        {
            var secret = new AppRegistrationSecretCheckResultApplicationSecret("The display name", new DateTime(2025, 1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            secret.DisplayName.Should().Be("The display name");
            secret.EndDate.Should().Be(new DateTime(2025, 1, 2, 3, 4, 5, 6)).And.BeIn(DateTimeKind.Utc);
            secret.Status.Should().Be(AppRegistrationSecretStatus.Valid);
        }

        [Fact]
        public void Status_ValueChanged()
        {
            var secret = new AppRegistrationSecretCheckResultApplicationSecret(default, default);

            secret.Status = AppRegistrationSecretStatus.ExpiringSoon;

            secret.Status.Should().Be(AppRegistrationSecretStatus.ExpiringSoon);
        }
    }
}