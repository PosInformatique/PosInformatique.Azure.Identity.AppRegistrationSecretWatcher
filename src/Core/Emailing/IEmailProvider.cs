//-----------------------------------------------------------------------
// <copyright file="IEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    public interface IEmailProvider
    {
        Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}