//-----------------------------------------------------------------------
// <copyright file="EntraIdApplicationPasswordCredentialTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId.Tests
{
    public class EntraIdApplicationPasswordCredentialTest
    {
        [Fact]
        public void Constructor()
        {
            var credential = new EntraIdApplicationPasswordCredential("The display name", new DateTime(2021, 1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            credential.DisplayName.Should().Be("The display name");
            credential.EndDateTime.Should().Be(new DateTime(2021, 1, 2, 3, 4, 5, 6)).And.BeIn(DateTimeKind.Utc);
        }
    }
}