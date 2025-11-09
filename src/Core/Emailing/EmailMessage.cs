//-----------------------------------------------------------------------
// <copyright file="EmailMessage.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    public sealed class EmailMessage
    {
        public EmailMessage(EmailContact from, EmailContact to, string subject, string htmlContent)
        {
            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.HtmlContent = htmlContent;
        }

        public EmailContact From { get; }

        public EmailContact To { get; }

        public string Subject { get; }

        public string HtmlContent { get; }
    }
}
