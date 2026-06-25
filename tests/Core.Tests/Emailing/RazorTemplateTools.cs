//-----------------------------------------------------------------------
// <copyright file="RazorTemplateTools.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.JSInterop;
    using PosInformatique.Foundations.Text.Templating;
    using PosInformatique.Foundations.Text.Templating.Razor;

    public static class RazorTemplateTools
    {
        public static void DisplayHtmlPage(string content, string testName)
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            var temporaryFolder = Path.Combine(UnitTestsFolders.TemporaryRoot, "Emailing Template Tests");
            var temporaryFile = Path.Combine(temporaryFolder, $"{testName}.html");

            if (!Directory.Exists(temporaryFolder))
            {
                Directory.CreateDirectory(temporaryFolder);
            }

            File.WriteAllText(temporaryFile, content);

            Process.Start(new ProcessStartInfo
            {
                FileName = temporaryFile,
                UseShellExecute = true,
            });
        }

        public static async Task<string> RenderAsync<TComponent>(object model, IServiceCollection services)
            where TComponent : ComponentBase
        {
#pragma warning disable PosInfoMoq1000 // VerifyAll() method should be called when instantiate a Mock<T> instances
            var jsRuntime = new Mock<IJSRuntime>(MockBehavior.Strict);
#pragma warning restore PosInfoMoq1000 // VerifyAll() method should be called when instantiate a Mock<T> instances

            services.AddLogging();
            services.AddSingleton(jsRuntime.Object);
            services.AddRazorTextTemplating();

            var serviceProvider = services.BuildServiceProvider();

            var context = new Mock<ITextTemplateRenderContext>(MockBehavior.Strict);
            context.Setup(c => c.ServiceProvider)
                .Returns(serviceProvider);

            var template = new RazorTextTemplate<object>(typeof(TComponent));

            var output = new StringWriter();

            await template.RenderAsync(model, output, context.Object);

            context.VerifyAll();

            return output.ToString();
        }
    }
}