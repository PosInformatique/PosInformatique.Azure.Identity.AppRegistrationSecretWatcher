//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManagerOptionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    public class AppRegistrationSecretManagerOptionsTest
    {
        [Fact]
        public void Constructor()
        {
            var options = new AppRegistrationSecretManagerOptions();

            options.EmailRecipients.Should().BeEmpty();
        }
    }
}