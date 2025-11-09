//-----------------------------------------------------------------------
// <copyright file="EntraIdApplicationTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId.Tests
{
    public class EntraIdApplicationTest
    {
        [Fact]
        public void Constructor()
        {
            var passwordCredentials = new[]
            {
                new EntraIdApplicationPasswordCredential(default, default),
                new EntraIdApplicationPasswordCredential(default, default),
            };

            var application = new EntraIdApplication("The id", "The display name", passwordCredentials);

            application.DisplayName.Should().Be("The display name");
            application.Id.Should().Be("The id");
            application.PasswordCredentials.Should().Equal(passwordCredentials);
        }
    }
}
