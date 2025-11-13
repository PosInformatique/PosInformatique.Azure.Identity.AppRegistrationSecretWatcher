//-----------------------------------------------------------------------
// <copyright file="EmailContactTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailContactTest
    {
        [Fact]
        public void Constructor()
        {
            var emailAddress = EmailAddress.Parse("user@domain.com");

            var contact = new EmailContact(emailAddress, "The display name");

            contact.Email.Should().BeSameAs(emailAddress);
            contact.DisplayName.Should().Be("The display name");
        }
    }
}