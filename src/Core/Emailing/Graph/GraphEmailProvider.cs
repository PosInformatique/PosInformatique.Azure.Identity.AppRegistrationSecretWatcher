//-----------------------------------------------------------------------
// <copyright file="GraphEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Graph
{
    using Microsoft.Graph;
    using Microsoft.Graph.Me.SendMail;
    using Microsoft.Graph.Models;

    public sealed class GraphEmailProvider : IEmailProvider
    {
        private readonly GraphServiceClient serviceClient;

        public GraphEmailProvider(GraphServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            var graphMessage = new Message()
            {
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = message.HtmlContent,
                },
                From = new Recipient()
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = message.From.Email.ToString(),
                        Name = message.From.DisplayName,
                    },
                },
                Subject = message.Subject,
                ToRecipients = new List<Recipient>
                {
                    new()
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = message.To.Email.ToString(),
                            Name = message.To.DisplayName,
                        },
                    },
                },
            };

            var body = new SendMailPostRequestBody()
            {
                Message = graphMessage,
                SaveToSentItems = false,
            };

            await this.serviceClient.Me.SendMail.PostAsync(body, cancellationToken: cancellationToken);
        }
    }
}