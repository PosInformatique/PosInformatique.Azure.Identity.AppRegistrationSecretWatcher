//-----------------------------------------------------------------------
// <copyright file="EmailTemplates.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing
{
    using System.Diagnostics.CodeAnalysis;
    using PosInformatique.Foundations.Emailing;
    using PosInformatique.Foundations.Emailing.Templates.Razor;

    [ExcludeFromCodeCoverage]
    public static class EmailTemplates
    {
        public static EmailTemplateIdentifier<AppRegistrationSecretCheckResult> ReportIdentifier { get; } = EmailTemplateIdentifier<AppRegistrationSecretCheckResult>.Create();

        public static EmailTemplate<AppRegistrationSecretCheckResult> Report { get; } = RazorEmailTemplate<AppRegistrationSecretCheckResult>.Create<ReportEmailTemplateSubject, ReportEmailTemplateBody>();
    }
}