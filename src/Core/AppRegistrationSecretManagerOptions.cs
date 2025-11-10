//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManagerOptions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    using PosInformatique.Foundations.Emailing;

    public class AppRegistrationSecretManagerOptions
    {
        public AppRegistrationSecretManagerOptions()
        {
            this.EmailRecipients = new Collection<EmailContact>();
        }

        public EmailContact EmailSender { get; set; } = default!;

        public Collection<EmailContact> EmailRecipients { get; }
    }
}
