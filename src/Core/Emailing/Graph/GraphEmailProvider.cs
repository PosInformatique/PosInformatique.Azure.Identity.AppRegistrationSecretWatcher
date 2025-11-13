//-----------------------------------------------------------------------
// <copyright file="GraphEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Graph
{
    using Microsoft.Graph;
    using Microsoft.Graph.Models;
    using Microsoft.Graph.Users.Item.SendMail;

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

            await this.serviceClient.Users[message.From.Email.ToString()].SendMail.PostAsync(body, cancellationToken: cancellationToken);
        }
    }
}