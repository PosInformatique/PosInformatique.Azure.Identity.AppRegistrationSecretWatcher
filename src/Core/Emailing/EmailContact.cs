//-----------------------------------------------------------------------
// <copyright file="EmailContact.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailContact
    {
        public EmailContact(EmailAddress email, string displayName)
        {
            this.Email = email;
            this.DisplayName = displayName;
        }

        public EmailAddress Email { get; }

        public string DisplayName { get; }
    }
}