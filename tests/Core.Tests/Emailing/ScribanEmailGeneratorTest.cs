//-----------------------------------------------------------------------
// <copyright file="ScribanEmailGeneratorTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Emailing.Tests
{
    public class ScribanEmailGeneratorTest
    {
        [Fact]
        public async Task GenerateAsync()
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
                            ])
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
            ]);

            var generator = new ScribanEmailGenerator();

            var content = await generator.GenerateAsync(checkResult, CancellationToken.None);

            await Verify(content);
        }
    }
}