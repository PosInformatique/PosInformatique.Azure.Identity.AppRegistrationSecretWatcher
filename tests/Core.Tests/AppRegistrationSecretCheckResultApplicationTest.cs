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
                new AppRegistrationSecretCheckResultApplicationSecret(default, default),
                new AppRegistrationSecretCheckResultApplicationSecret(default, default),
            };

            var application = new AppRegistrationSecretCheckResultApplication("The display name", secrets);

            application.DisplayName.Should().Be("The display name");
            application.Secrets.Should().Equal(secrets);
        }
    }
}
