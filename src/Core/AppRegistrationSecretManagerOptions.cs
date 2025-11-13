//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManagerOptions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    using PosInformatique.Foundations.EmailAddresses;

    public class AppRegistrationSecretManagerOptions
    {
        public AppRegistrationSecretManagerOptions()
        {
            this.EmailRecipients = new Collection<EmailAddress>();
        }

        public EmailAddress EmailSender { get; set; } = default!;

        public Collection<EmailAddress> EmailRecipients { get; }
    }
}