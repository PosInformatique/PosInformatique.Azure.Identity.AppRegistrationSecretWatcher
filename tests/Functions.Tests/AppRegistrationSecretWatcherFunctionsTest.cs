//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretWatcherFunctionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Functions.Tests
{
    using Microsoft.Extensions.Options;

    public class AppRegistrationSecretWatcherFunctionsTest
    {
        [Fact]
        public async Task WatchAppSecretsAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var manager = new Mock<IAppRegistrationSecretManager>(MockBehavior.Strict);
            manager.Setup(m => m.CheckAsync(It.IsAny<AppRegistrationSecretCheckParameters>(), cancellationToken))
                .Callback((AppRegistrationSecretCheckParameters p, CancellationToken _) =>
                {
                    p.ExpirationThreshold.Should().Be(TimeSpan.FromDays(2));
                })
                .ReturnsAsync((AppRegistrationSecretCheckResult)null);

            var options = Options.Create(new AppRegistrationSecretWatcherFunctionsOptions()
            {
                ExpirationThreshold = TimeSpan.FromDays(2),
                TenantIds =
                {
                    "Tenant 1",
                    "Tenant 2",
                },
            });

            var functions = new AppRegistrationSecretWatcherFunctions(manager.Object, options);

            await functions.WatchAppSecretsAsync(null, cancellationToken);

            manager.VerifyAll();
        }
    }
}