//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManagerOptionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class AppRegistrationSecretManagerOptionsTest
    {
        [Fact]
        public void Constructor()
        {
            var options = new AppRegistrationSecretManagerOptions();

            options.EmailSender.Should().BeNull();
            options.EmailRecipients.Should().BeEmpty();
        }

        [Fact]
        public void EmailSender_ValueChanged()
        {
            var options = new AppRegistrationSecretManagerOptions();

            var sender = EmailAddress.Parse("email@domain.com");

            options.EmailSender = sender;

            options.EmailSender.Should().Be(sender);
        }
    }
}