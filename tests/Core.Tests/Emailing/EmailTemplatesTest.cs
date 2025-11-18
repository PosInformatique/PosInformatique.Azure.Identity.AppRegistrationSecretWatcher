//-----------------------------------------------------------------------
// <copyright file="EmailTemplatesTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing.Tests
{
    using Microsoft.Extensions.DependencyInjection;

    public class EmailTemplatesTest
    {
        [Fact]
        public async Task ReportEmailTemplateBody_Render()
        {
            var checkResult = new AppRegistrationSecretCheckResult(
            [
                new AppRegistrationSecretCheckResultTenant(
                    "Id 1",
                    "The tenant 1",
                    [
                        new AppRegistrationSecretCheckResultApplication(
                            "Id 1-1",
                            "The app 1-1",
                            [
                                new AppRegistrationSecretCheckResultApplicationSecret("Secret 1-1-1", new DateTime(2025, 1, 1), -10) { Status = AppRegistrationSecretStatus.Expired },
                                new AppRegistrationSecretCheckResultApplicationSecret("Secret 1-1-2", new DateTime(2025, 2, 2), 20) { Status = AppRegistrationSecretStatus.Valid },
                            ]),
                        new AppRegistrationSecretCheckResultApplication(
                            "Id 1-2",
                            "The app 1-2",
                            [
                                new AppRegistrationSecretCheckResultApplicationSecret("Secret 1-2-1", new DateTime(2025, 3, 3), 30) { Status = AppRegistrationSecretStatus.Valid },
                                new AppRegistrationSecretCheckResultApplicationSecret("Secret 1-2-2", new DateTime(2025, 4, 4), 40) { Status = AppRegistrationSecretStatus.ExpiringSoon },
                            ]),
                        new AppRegistrationSecretCheckResultApplication(
                            "Id 1-3",
                            "The app 1-3",
                            [])
                    ]),
                new AppRegistrationSecretCheckResultTenant(
                    "Id 2",
                    "The tenant 2",
                    [
                        new AppRegistrationSecretCheckResultApplication(
                            "Id 2-1",
                            "The app 2-1",
                            [
                                new AppRegistrationSecretCheckResultApplicationSecret("Secret 2-1-1", new DateTime(2025, 5, 5), 50) { Status = AppRegistrationSecretStatus.Valid },
                            ]),
                        new AppRegistrationSecretCheckResultApplication(
                            "Id 2-2",
                            "The app 2-2",
                            [
                                new AppRegistrationSecretCheckResultApplicationSecret("Secret 2-1-1", new DateTime(2025, 6, 6), 60) { Status = AppRegistrationSecretStatus.Valid },
                            ])
                    ]),
                new AppRegistrationSecretCheckResultTenant(
                    "Id 3",
                    "The tenant 3",
                    []),
            ],
            new DateTime(2025, 1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            var serviceCollection = new ServiceCollection();

            var content = await RazorTemplateTools.RenderAsync<ReportEmailTemplateBody>(checkResult, serviceCollection);

            await Verify(content, "html");
        }

        [Fact]
        public async Task ReportEmailTemplateSubject_Render()
        {
            var checkResult = new AppRegistrationSecretCheckResult(
                [],
                new DateTime(2025, 1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            var serviceCollection = new ServiceCollection();

            var content = await RazorTemplateTools.RenderAsync<ReportEmailTemplateSubject>(checkResult, serviceCollection);

            content.Should().Be("Reminder: App Registration secrets expiring soon - [02/01/2025]");
        }
    }
}