//-----------------------------------------------------------------------
// <copyright file="ScribanEmailGenerator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing
{
    using Scriban;
    using Scriban.Runtime;

    public class ScribanEmailGenerator : IEmailGenerator
    {
        public async Task<string> GenerateAsync(AppRegistrationSecretCheckResult result, CancellationToken cancellationToken = default)
        {
            using var htmlTemplateStream = typeof(ScribanEmailGenerator).Assembly.GetManifestResourceStream("PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing.EmailTemplate.html")!;
            using var reader = new StreamReader(htmlTemplateStream);

            var htmlTemplate = await reader.ReadToEndAsync(cancellationToken);

            var scribanTemplate = Template.Parse(htmlTemplate);

            var scriptObject = new ScriptObject
            {
                { nameof(result.Tenants), result.Tenants },
            };

            var context = new TemplateContext()
            {
                MemberRenamer = r => r.Name,
                MemberFilter = null,
            };

            context.PushGlobal(scriptObject);

            return await scribanTemplate.RenderAsync(context);
        }
    }
}