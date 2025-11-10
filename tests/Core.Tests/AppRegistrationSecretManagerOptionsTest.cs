//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManagerOptionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    using PosInformatique.Foundations.Emailing;

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

            var sender = new EmailContact(default, default);

            options.EmailSender = sender;

            options.EmailSender.Should().Be(sender);
        }
    }
}