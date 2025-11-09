//-----------------------------------------------------------------------
// <copyright file="EmailMessageTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    public class EmailMessageTest
    {
        [Fact]
        public void Constructor()
        {
            var from = new EmailContact(default, default);
            var to = new EmailContact(default, default);

            var emailMessage = new EmailMessage(
                from,
                to,
                "The subject",
                "HTML content");

            emailMessage.From.Should().Be(from);
            emailMessage.HtmlContent.Should().Be("HTML content");
            emailMessage.Subject.Should().Be("The subject");
            emailMessage.To.Should().Be(to);
        }
    }
}
