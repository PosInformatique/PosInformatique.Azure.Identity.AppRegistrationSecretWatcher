//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManagerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Core.Tests
{
    using Microsoft.Extensions.Options;
    using PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing;
    using PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId;
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.Emailing;

    public class AppRegistrationSecretManagerTest
    {
        [Fact]
        public async Task CheckAsync()
        {
            var now = new DateTime(2025, 6, 15, 1, 2, 3, 4, 5, DateTimeKind.Utc);

            var cancellationToken = new CancellationTokenSource().Token;

            AppRegistrationSecretCheckResult expectedResult = null;

            var entraIdClient = new Mock<IEntraIdClient>(MockBehavior.Strict);
            entraIdClient.Setup(c => c.GetApplicationsAsync("Tenant 1", cancellationToken))
                .ReturnsAsync(
                    new EntraIdTenant(
                        "The tenant ID 1",
                        "The tenant display name 1",
                        [
                            new EntraIdApplication(
                                "1-1",
                                "App 1-1",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 1-1-1", now.AddDays(60)),
                                    new EntraIdApplicationPasswordCredential("Secret 1-1-2", now.AddDays(10)),
                                ]),
                            new EntraIdApplication(
                                "1-2",
                                "App 1-2",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 1-2-1", now.AddDays(30)),
                                    new EntraIdApplicationPasswordCredential("Secret 1-2-2", now.AddDays(120)),
                                ])
                        ]));
            entraIdClient.Setup(c => c.GetApplicationsAsync("Tenant 2", cancellationToken))
                .ReturnsAsync(
                    new EntraIdTenant(
                        "The tenant ID 2",
                        "The tenant display name 2",
                        [
                            new EntraIdApplication(
                                "2-1",
                                "App 2-1",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 2-1-1", now.AddDays(100)),
                                ]),
                            new EntraIdApplication(
                                "2-2",
                                "App 2-2",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 2-2-1", now.AddDays(300)),
                                ])
                        ]));

            var email = new Email<AppRegistrationSecretCheckResult>(EmailTemplates.Report);

            var emailManager = new Mock<IEmailManager>(MockBehavior.Strict);
            emailManager.Setup(em => em.Create(EmailTemplates.ReportIdentifier))
                .Returns(email);

            emailManager.Setup(g => g.SendAsync(It.IsAny<Email<AppRegistrationSecretCheckResult>>(), cancellationToken))
                .Callback((Email<AppRegistrationSecretCheckResult> e, CancellationToken _) =>
                {
                    e.Should().BeSameAs(email);

                    expectedResult = email.Recipients[0].Model;

                    e.Importance.Should().Be(EmailImportance.Normal);

                    e.Recipients.Should().HaveCount(2);

                    e.Recipients[0].Address.Should().Be(EmailAddress.Parse("email1@domain.com"));
                    e.Recipients[0].DisplayName.Should().BeEmpty();

                    e.Recipients[1].Address.Should().Be(EmailAddress.Parse("email2@domain.com"));
                    e.Recipients[1].DisplayName.Should().BeEmpty();
                    e.Recipients[1].Model.Should().BeSameAs(expectedResult);
                })
                .Returns(Task.CompletedTask);

            var timeProvider = new Mock<TimeProvider>(MockBehavior.Strict);
            timeProvider.Setup(tp => tp.GetUtcNow())
                .Returns(now);
            timeProvider.Setup(tp => tp.LocalTimeZone)
                .Returns(TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila"));

            var options = Options.Create(new AppRegistrationSecretManagerOptions()
            {
                EmailRecipients =
                {
                    EmailAddress.Parse("email1@domain.com"),
                    EmailAddress.Parse("email2@domain.com"),
                },
            });

            var parameters = new AppRegistrationSecretCheckParameters()
            {
                ExpirationThreshold = TimeSpan.FromDays(5),
                TenantIds =
                {
                    "Tenant 1",
                    "Tenant 2",
                },
            };

            var manager = new AppRegistrationSecretManager(entraIdClient.Object, emailManager.Object, timeProvider.Object, options);

            var result = await manager.CheckAsync(parameters, cancellationToken);

            result.Should().BeSameAs(expectedResult);

            result.DateTime.Should().Be(now).And.BeIn(DateTimeKind.Utc);

            result.Tenants.Should().HaveCount(2);

            result.Tenants[0].DisplayName.Should().Be("The tenant display name 1");
            result.Tenants[0].Id.Should().Be("The tenant ID 1");
            result.Tenants[0].Applications.Should().HaveCount(2);
            result.Tenants[0].Applications[0].DisplayName.Should().Be("App 1-1");
            result.Tenants[0].Applications[0].Id.Should().Be("1-1");
            result.Tenants[0].Applications[0].Secrets.Should().HaveCount(2);
            result.Tenants[0].Applications[0].Secrets[0].DaysBeforeExpiration.Should().Be(60);
            result.Tenants[0].Applications[0].Secrets[0].DisplayName.Should().Be("Secret 1-1-1");
            result.Tenants[0].Applications[0].Secrets[0].EndDate.Should().Be(now.AddDays(60).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[0].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.Valid);
            result.Tenants[0].Applications[0].Secrets[1].DaysBeforeExpiration.Should().Be(10);
            result.Tenants[0].Applications[0].Secrets[1].DisplayName.Should().Be("Secret 1-1-2");
            result.Tenants[0].Applications[0].Secrets[1].EndDate.Should().Be(now.AddDays(10).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[0].Secrets[1].Status.Should().Be(AppRegistrationSecretStatus.Valid);
            result.Tenants[0].Applications[1].DisplayName.Should().Be("App 1-2");
            result.Tenants[0].Applications[1].Id.Should().Be("1-2");
            result.Tenants[0].Applications[1].Secrets.Should().HaveCount(2);
            result.Tenants[0].Applications[1].Secrets[0].DaysBeforeExpiration.Should().Be(30);
            result.Tenants[0].Applications[1].Secrets[0].DisplayName.Should().Be("Secret 1-2-1");
            result.Tenants[0].Applications[1].Secrets[0].EndDate.Should().Be(now.AddDays(30).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[1].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.Valid);
            result.Tenants[0].Applications[1].Secrets[1].DaysBeforeExpiration.Should().Be(120);
            result.Tenants[0].Applications[1].Secrets[1].DisplayName.Should().Be("Secret 1-2-2");
            result.Tenants[0].Applications[1].Secrets[1].EndDate.Should().Be(now.AddDays(120).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[1].Secrets[1].Status.Should().Be(AppRegistrationSecretStatus.Valid);

            result.Tenants[1].DisplayName.Should().Be("The tenant display name 2");
            result.Tenants[1].Id.Should().Be("The tenant ID 2");
            result.Tenants[1].Applications.Should().HaveCount(2);
            result.Tenants[1].Applications[0].DisplayName.Should().Be("App 2-1");
            result.Tenants[1].Applications[0].Id.Should().Be("2-1");
            result.Tenants[1].Applications[0].Secrets.Should().HaveCount(1);
            result.Tenants[1].Applications[0].Secrets[0].DaysBeforeExpiration.Should().Be(100);
            result.Tenants[1].Applications[0].Secrets[0].DisplayName.Should().Be("Secret 2-1-1");
            result.Tenants[1].Applications[0].Secrets[0].EndDate.Should().Be(now.AddDays(100).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[1].Applications[0].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.Valid);
            result.Tenants[1].Applications[1].DisplayName.Should().Be("App 2-2");
            result.Tenants[1].Applications[1].Id.Should().Be("2-2");
            result.Tenants[1].Applications[1].Secrets.Should().HaveCount(1);
            result.Tenants[1].Applications[1].Secrets[0].DaysBeforeExpiration.Should().Be(300);
            result.Tenants[1].Applications[1].Secrets[0].DisplayName.Should().Be("Secret 2-2-1");
            result.Tenants[1].Applications[1].Secrets[0].EndDate.Should().Be(now.AddDays(300).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[1].Applications[1].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.Valid);

            emailManager.VerifyAll();
            entraIdClient.VerifyAll();
            timeProvider.VerifyAll();
        }

        [Fact]
        public async Task CheckAsync_WithImportance()
        {
            var now = new DateTime(2025, 6, 15, 1, 2, 3, 4, 5, DateTimeKind.Utc);

            var cancellationToken = new CancellationTokenSource().Token;

            AppRegistrationSecretCheckResult expectedResult = null;

            var entraIdClient = new Mock<IEntraIdClient>(MockBehavior.Strict);
            entraIdClient.Setup(c => c.GetApplicationsAsync("Tenant 1", cancellationToken))
                .ReturnsAsync(
                    new EntraIdTenant(
                        "The tenant ID 1",
                        "The tenant display name 1",
                        [
                            new EntraIdApplication(
                                "1-1",
                                "App 1-1",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 1-1-1", now.AddDays(60)),
                                    new EntraIdApplicationPasswordCredential("Secret 1-1-2", now.AddDays(10)),
                                ]),
                            new EntraIdApplication(
                                "1-2",
                                "App 1-2",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 1-2-1", now.AddDays(30)),
                                    new EntraIdApplicationPasswordCredential("Secret 1-2-2", now.AddDays(120)),
                                ])
                        ]));
            entraIdClient.Setup(c => c.GetApplicationsAsync("Tenant 2", cancellationToken))
                .ReturnsAsync(
                    new EntraIdTenant(
                        "The tenant ID 2",
                        "The tenant display name 2",
                        [
                            new EntraIdApplication(
                                "2-1",
                                "App 2-1",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 2-1-1", now.AddDays(-100)),
                                ]),
                            new EntraIdApplication(
                                "2-2",
                                "App 2-2",
                                [
                                    new EntraIdApplicationPasswordCredential("Secret 2-2-1", now.AddDays(300)),
                                ])
                        ]));

            var email = new Email<AppRegistrationSecretCheckResult>(EmailTemplates.Report);

            var emailManager = new Mock<IEmailManager>(MockBehavior.Strict);
            emailManager.Setup(em => em.Create(EmailTemplates.ReportIdentifier))
                .Returns(email);

            emailManager.Setup(g => g.SendAsync(It.IsAny<Email<AppRegistrationSecretCheckResult>>(), cancellationToken))
                .Callback((Email<AppRegistrationSecretCheckResult> e, CancellationToken _) =>
                {
                    e.Should().BeSameAs(email);

                    expectedResult = email.Recipients[0].Model;

                    e.Importance.Should().Be(EmailImportance.High);

                    e.Recipients.Should().HaveCount(2);

                    e.Recipients[0].Address.Should().Be(EmailAddress.Parse("email1@domain.com"));
                    e.Recipients[0].DisplayName.Should().BeEmpty();

                    e.Recipients[1].Address.Should().Be(EmailAddress.Parse("email2@domain.com"));
                    e.Recipients[1].DisplayName.Should().BeEmpty();
                    e.Recipients[1].Model.Should().BeSameAs(expectedResult);
                })
                .Returns(Task.CompletedTask);

            var timeProvider = new Mock<TimeProvider>(MockBehavior.Strict);
            timeProvider.Setup(tp => tp.GetUtcNow())
                .Returns(now);
            timeProvider.Setup(tp => tp.LocalTimeZone)
                .Returns(TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila"));

            var options = Options.Create(new AppRegistrationSecretManagerOptions()
            {
                EmailRecipients =
                {
                    EmailAddress.Parse("email1@domain.com"),
                    EmailAddress.Parse("email2@domain.com"),
                },
            });

            var parameters = new AppRegistrationSecretCheckParameters()
            {
                ExpirationThreshold = TimeSpan.FromDays(30),
                TenantIds =
                {
                    "Tenant 1",
                    "Tenant 2",
                },
            };

            var manager = new AppRegistrationSecretManager(entraIdClient.Object, emailManager.Object, timeProvider.Object, options);

            var result = await manager.CheckAsync(parameters, cancellationToken);

            result.Should().BeSameAs(expectedResult);

            result.DateTime.Should().Be(now).And.BeIn(DateTimeKind.Utc);

            result.Tenants.Should().HaveCount(2);

            result.Tenants[0].DisplayName.Should().Be("The tenant display name 1");
            result.Tenants[0].Id.Should().Be("The tenant ID 1");
            result.Tenants[0].Applications.Should().HaveCount(2);
            result.Tenants[0].Applications[0].DisplayName.Should().Be("App 1-1");
            result.Tenants[0].Applications[0].Id.Should().Be("1-1");
            result.Tenants[0].Applications[0].Secrets.Should().HaveCount(2);
            result.Tenants[0].Applications[0].Secrets[0].DaysBeforeExpiration.Should().Be(60);
            result.Tenants[0].Applications[0].Secrets[0].DisplayName.Should().Be("Secret 1-1-1");
            result.Tenants[0].Applications[0].Secrets[0].EndDate.Should().Be(now.AddDays(60).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[0].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.Valid);
            result.Tenants[0].Applications[0].Secrets[1].DaysBeforeExpiration.Should().Be(10);
            result.Tenants[0].Applications[0].Secrets[1].DisplayName.Should().Be("Secret 1-1-2");
            result.Tenants[0].Applications[0].Secrets[1].EndDate.Should().Be(now.AddDays(10).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[0].Secrets[1].Status.Should().Be(AppRegistrationSecretStatus.ExpiringSoon);
            result.Tenants[0].Applications[1].DisplayName.Should().Be("App 1-2");
            result.Tenants[0].Applications[1].Id.Should().Be("1-2");
            result.Tenants[0].Applications[1].Secrets.Should().HaveCount(2);
            result.Tenants[0].Applications[1].Secrets[0].DaysBeforeExpiration.Should().Be(30);
            result.Tenants[0].Applications[1].Secrets[0].DisplayName.Should().Be("Secret 1-2-1");
            result.Tenants[0].Applications[1].Secrets[0].EndDate.Should().Be(now.AddDays(30).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[1].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.ExpiringSoon);
            result.Tenants[0].Applications[1].Secrets[1].DaysBeforeExpiration.Should().Be(120);
            result.Tenants[0].Applications[1].Secrets[1].DisplayName.Should().Be("Secret 1-2-2");
            result.Tenants[0].Applications[1].Secrets[1].EndDate.Should().Be(now.AddDays(120).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[0].Applications[1].Secrets[1].Status.Should().Be(AppRegistrationSecretStatus.Valid);

            result.Tenants[1].DisplayName.Should().Be("The tenant display name 2");
            result.Tenants[1].Id.Should().Be("The tenant ID 2");
            result.Tenants[1].Applications.Should().HaveCount(2);
            result.Tenants[1].Applications[0].DisplayName.Should().Be("App 2-1");
            result.Tenants[1].Applications[0].Id.Should().Be("2-1");
            result.Tenants[1].Applications[0].Secrets.Should().HaveCount(1);
            result.Tenants[1].Applications[0].Secrets[0].DaysBeforeExpiration.Should().Be(-100);
            result.Tenants[1].Applications[0].Secrets[0].DisplayName.Should().Be("Secret 2-1-1");
            result.Tenants[1].Applications[0].Secrets[0].EndDate.Should().Be(now.AddDays(-100).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[1].Applications[0].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.Expired);
            result.Tenants[1].Applications[1].DisplayName.Should().Be("App 2-2");
            result.Tenants[1].Applications[1].Id.Should().Be("2-2");
            result.Tenants[1].Applications[1].Secrets.Should().HaveCount(1);
            result.Tenants[1].Applications[1].Secrets[0].DaysBeforeExpiration.Should().Be(300);
            result.Tenants[1].Applications[1].Secrets[0].DisplayName.Should().Be("Secret 2-2-1");
            result.Tenants[1].Applications[1].Secrets[0].EndDate.Should().Be(now.AddDays(300).AddHours(8)).And.BeIn(DateTimeKind.Local);
            result.Tenants[1].Applications[1].Secrets[0].Status.Should().Be(AppRegistrationSecretStatus.Valid);

            emailManager.VerifyAll();
            entraIdClient.VerifyAll();
            timeProvider.VerifyAll();
        }
    }
}