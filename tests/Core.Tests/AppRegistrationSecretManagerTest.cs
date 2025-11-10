//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretManagerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Core.Tests
{
    using System.Threading.Tasks;
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

            var sender = new EmailContact(default, default);

            var entraIdClient = new Mock<IEntraIdApplicationClient>(MockBehavior.Strict);
            entraIdClient.Setup(c => c.GetAsync("Tenant 1", cancellationToken))
                .ReturnsAsync(
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
                ]);
            entraIdClient.Setup(c => c.GetAsync("Tenant 2", cancellationToken))
                .ReturnsAsync(
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
                ]);

            var emailProvider = new Mock<IEmailProvider>(MockBehavior.Strict);
            emailProvider.Setup(ep => ep.SendAsync(It.Is<EmailMessage>(m => m.To.Email.ToString() == "email1@domain.com"), cancellationToken))
                .Callback((EmailMessage m, CancellationToken _) =>
                {
                    m.HtmlContent.Should().Be("The content");
                    m.From.Should().BeSameAs(sender);
                    m.Subject.Should().Be("Reminder: App Registration secrets expiring soon - [07/02/2020]");
                    m.To.DisplayName.Should().Be("Email 1");
                })
                .Returns(Task.CompletedTask);
            emailProvider.Setup(ep => ep.SendAsync(It.Is<EmailMessage>(m => m.To.Email.ToString() == "email2@domain.com"), cancellationToken))
                .Callback((EmailMessage m, CancellationToken _) =>
                {
                    m.HtmlContent.Should().Be("The content");
                    m.From.Should().BeSameAs(sender);
                    m.Subject.Should().Be("Reminder: App Registration secrets expiring soon - [07/02/2020]");
                    m.To.DisplayName.Should().Be("Email 2");
                })
                .Returns(Task.CompletedTask);

            var emailGenerator = new Mock<IEmailGenerator>(MockBehavior.Strict);
            emailGenerator.Setup(g => g.Generate(It.IsAny<AppRegistrationSecretCheckResult>()))
                .Callback((AppRegistrationSecretCheckResult r) =>
                {
                    r.Tenants.Should().HaveCount(2);

                    r.Tenants[0].Id.Should().Be("Tenant 1");
                    r.Tenants[0].Applications.Should().HaveCount(2);
                    r.Tenants[0].Applications[0].DisplayName.Should().Be("App 1-1");
                    r.Tenants[0].Applications[0].Secrets.Should().HaveCount(2);
                    r.Tenants[0].Applications[0].Secrets[0].DisplayName.Should().Be("Secret 1-1-1");
                    r.Tenants[0].Applications[0].Secrets[0].EndDate.Should().Be(now.AddDays(60).AddHours(8)).And.BeIn(DateTimeKind.Local);
                    r.Tenants[0].Applications[0].Secrets[0].Expired.Should().BeFalse();
                    r.Tenants[0].Applications[0].Secrets[1].DisplayName.Should().Be("Secret 1-1-2");
                    r.Tenants[0].Applications[0].Secrets[1].EndDate.Should().Be(now.AddDays(10).AddHours(8)).And.BeIn(DateTimeKind.Local);
                    r.Tenants[0].Applications[0].Secrets[1].Expired.Should().BeTrue();
                    r.Tenants[0].Applications[1].DisplayName.Should().Be("App 1-2");
                    r.Tenants[0].Applications[1].Secrets.Should().HaveCount(2);
                    r.Tenants[0].Applications[1].Secrets[0].DisplayName.Should().Be("Secret 1-2-1");
                    r.Tenants[0].Applications[1].Secrets[0].EndDate.Should().Be(now.AddDays(30).AddHours(8)).And.BeIn(DateTimeKind.Local);
                    r.Tenants[0].Applications[1].Secrets[0].Expired.Should().BeTrue();
                    r.Tenants[0].Applications[1].Secrets[1].DisplayName.Should().Be("Secret 1-2-2");
                    r.Tenants[0].Applications[1].Secrets[1].EndDate.Should().Be(now.AddDays(120).AddHours(8)).And.BeIn(DateTimeKind.Local);
                    r.Tenants[0].Applications[1].Secrets[1].Expired.Should().BeFalse();

                    r.Tenants[1].Id.Should().Be("Tenant 2");
                    r.Tenants[1].Applications.Should().HaveCount(2);
                    r.Tenants[1].Applications[0].DisplayName.Should().Be("App 2-1");
                    r.Tenants[1].Applications[0].Secrets.Should().HaveCount(1);
                    r.Tenants[1].Applications[0].Secrets[0].DisplayName.Should().Be("Secret 2-1-1");
                    r.Tenants[1].Applications[0].Secrets[0].EndDate.Should().Be(now.AddDays(100).AddHours(8)).And.BeIn(DateTimeKind.Local);
                    r.Tenants[1].Applications[0].Secrets[0].Expired.Should().BeFalse();
                    r.Tenants[1].Applications[1].DisplayName.Should().Be("App 2-2");
                    r.Tenants[1].Applications[1].Secrets.Should().HaveCount(1);
                    r.Tenants[1].Applications[1].Secrets[0].DisplayName.Should().Be("Secret 2-2-1");
                    r.Tenants[1].Applications[1].Secrets[0].EndDate.Should().Be(now.AddDays(300).AddHours(8)).And.BeIn(DateTimeKind.Local);
                    r.Tenants[1].Applications[1].Secrets[0].Expired.Should().BeFalse();

                    expectedResult = r;
                })
                .Returns("The content");

            var timeProvider = new Mock<TimeProvider>(MockBehavior.Strict);
            timeProvider.Setup(tp => tp.GetUtcNow())
                .Returns(new DateTimeOffset(2020, 2, 7, 8, 9, 15, 10, 4, TimeSpan.Zero));
            timeProvider.Setup(tp => tp.LocalTimeZone)
                .Returns(TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila"));

            var options = Options.Create(new AppRegistrationSecretManagerOptions()
            {
                EmailRecipients =
                {
                    new EmailContact(EmailAddress.Parse("email1@domain.com"), "Email 1"),
                    new EmailContact(EmailAddress.Parse("email2@domain.com"), "Email 2"),
                },
                EmailSender = sender,
            });

            var parameters = new AppRegistrationSecretCheckParameters(now.AddDays(30))
            {
                TenantIds =
                {
                    "Tenant 1",
                    "Tenant 2",
                },
            };

            var manager = new AppRegistrationSecretManager(entraIdClient.Object, emailProvider.Object, emailGenerator.Object, timeProvider.Object, options);

            var result = await manager.CheckAsync(parameters, cancellationToken);

            result.Should().BeSameAs(expectedResult);

            emailGenerator.VerifyAll();
            emailProvider.VerifyAll();
            entraIdClient.VerifyAll();
            timeProvider.VerifyAll();
        }
    }
}