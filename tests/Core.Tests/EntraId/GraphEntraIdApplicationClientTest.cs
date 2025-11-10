//-----------------------------------------------------------------------
// <copyright file="GraphEntraIdApplicationClientTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId.Tests
{
    using Microsoft.Graph;
    using Microsoft.Graph.Models;
    using Microsoft.Kiota.Abstractions;
    using Microsoft.Kiota.Abstractions.Serialization;

    public class GraphEntraIdApplicationClientTest
    {
        [Fact]
        public async Task GetAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var requestAdapter = new Mock<IRequestAdapter>(MockBehavior.Strict);
            requestAdapter.Setup(r => r.BaseUrl)
                .Returns("http://base/url");
            requestAdapter.Setup(r => r.EnableBackingStore(null));
            requestAdapter.Setup(r => r.SendAsync(It.IsAny<RequestInformation>(), It.IsAny<ParsableFactory<ApplicationCollectionResponse>>(), It.IsNotNull<Dictionary<string, ParsableFactory<IParsable>>>(), cancellationToken))
                .Callback((RequestInformation requestInfo, ParsableFactory<ApplicationCollectionResponse> factory, Dictionary<string, ParsableFactory<IParsable>> _, CancellationToken _) =>
                {
                    requestInfo.HttpMethod.Should().Be(Method.GET);
                    requestInfo.URI.Should().Be("http://base/url/applications?%24select=appId,displayName,passwordCredentials");

                    requestInfo.QueryParameters.Should().HaveCount(1);
                    requestInfo.QueryParameters["%24select"].As<string[]>().Should().Equal("appId", "displayName", "passwordCredentials");

                    requestInfo.Content.Should().BeSameAs(Stream.Null);
                })
                .ReturnsAsync(new ApplicationCollectionResponse()
                {
                    Value = new List<Application>
                    {
                        new Application()
                        {
                            AppId = "App Id 1",
                            DisplayName = "Display name 1",
                            PasswordCredentials = new List<PasswordCredential>
                            {
                                new PasswordCredential()
                                {
                                    DisplayName = "Password 1",
                                    EndDateTime = new DateTimeOffset(2025, 1, 1, 2, 1, 1, 1, 1, TimeSpan.FromHours(1)),
                                },
                                new PasswordCredential()
                                {
                                    DisplayName = "Password 2",
                                    EndDateTime = new DateTimeOffset(2025, 2, 2, 4, 2, 2, 2, 2, TimeSpan.FromHours(2)),
                                },
                            },
                        },
                        new Application()
                        {
                            AppId = "App Id 2",
                            DisplayName = "Display name 2",
                            PasswordCredentials = new List<PasswordCredential>
                            {
                            },
                        },
                    },
                });

            var graphServiceClient = new Mock<GraphServiceClient>(MockBehavior.Strict, requestAdapter.Object, null);

            var graphServiceClientFactory = new Mock<IGraphServiceClientFactory>(MockBehavior.Strict);
            graphServiceClientFactory.Setup(gf => gf.Create("The tenant Id"))
                .Returns(graphServiceClient.Object);

            var client = new GraphEntraIdApplicationClient(graphServiceClientFactory.Object);

            var result = await client.GetAsync("The tenant Id", cancellationToken);

            result.Should().HaveCount(2);

            result[0].DisplayName.Should().Be("Display name 1");
            result[0].Id.Should().Be("App Id 1");
            result[0].PasswordCredentials.Should().HaveCount(2);
            result[0].PasswordCredentials[0].DisplayName.Should().Be("Password 1");
            result[0].PasswordCredentials[0].EndDateTime.Should().Be(new DateTime(2025, 1, 1, 1, 1, 1, 1, 1)).And.BeIn(DateTimeKind.Utc);
            result[0].PasswordCredentials[1].DisplayName.Should().Be("Password 2");
            result[0].PasswordCredentials[1].EndDateTime.Should().Be(new DateTime(2025, 2, 2, 2, 2, 2, 2, 2)).And.BeIn(DateTimeKind.Utc);

            result[1].DisplayName.Should().Be("Display name 2");
            result[1].Id.Should().Be("App Id 2");
            result[1].PasswordCredentials.Should().BeEmpty();

            graphServiceClient.VerifyAll();
            graphServiceClientFactory.VerifyAll();
            requestAdapter.VerifyAll();
        }

        [Fact]
        public async Task GetAsync_ApplicationNull()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var requestAdapter = new Mock<IRequestAdapter>(MockBehavior.Strict);
            requestAdapter.Setup(r => r.BaseUrl)
                .Returns("http://base/url");
            requestAdapter.Setup(r => r.EnableBackingStore(null));
            requestAdapter.Setup(r => r.SendAsync(It.IsAny<RequestInformation>(), It.IsAny<ParsableFactory<ApplicationCollectionResponse>>(), It.IsNotNull<Dictionary<string, ParsableFactory<IParsable>>>(), cancellationToken))
                .Callback((RequestInformation requestInfo, ParsableFactory<ApplicationCollectionResponse> factory, Dictionary<string, ParsableFactory<IParsable>> _, CancellationToken _) =>
                {
                    requestInfo.HttpMethod.Should().Be(Method.GET);
                    requestInfo.URI.Should().Be("http://base/url/applications?%24select=appId,displayName,passwordCredentials");

                    requestInfo.QueryParameters.Should().HaveCount(1);
                    requestInfo.QueryParameters["%24select"].As<string[]>().Should().Equal("appId", "displayName", "passwordCredentials");

                    requestInfo.Content.Should().BeSameAs(Stream.Null);
                })
                .ReturnsAsync(new ApplicationCollectionResponse()
                {
                    Value = null,
                });

            var graphServiceClient = new Mock<GraphServiceClient>(MockBehavior.Strict, requestAdapter.Object, null);

            var graphServiceClientFactory = new Mock<IGraphServiceClientFactory>(MockBehavior.Strict);
            graphServiceClientFactory.Setup(gf => gf.Create("The tenant Id"))
                .Returns(graphServiceClient.Object);

            var client = new GraphEntraIdApplicationClient(graphServiceClientFactory.Object);

            await client.Invoking(c => c.GetAsync("The tenant Id", cancellationToken))
                .Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage("Unable to retrieve the list of the app registrations for the tenant 'The tenant Id'.");

            graphServiceClient.VerifyAll();
            graphServiceClientFactory.VerifyAll();
            requestAdapter.VerifyAll();
        }

        [Fact]
        public async Task GetAsync_ResponseValueNull()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var requestAdapter = new Mock<IRequestAdapter>(MockBehavior.Strict);
            requestAdapter.Setup(r => r.BaseUrl)
                .Returns("http://base/url");
            requestAdapter.Setup(r => r.EnableBackingStore(null));
            requestAdapter.Setup(r => r.SendAsync(It.IsAny<RequestInformation>(), It.IsAny<ParsableFactory<ApplicationCollectionResponse>>(), It.IsNotNull<Dictionary<string, ParsableFactory<IParsable>>>(), cancellationToken))
                .Callback((RequestInformation requestInfo, ParsableFactory<ApplicationCollectionResponse> factory, Dictionary<string, ParsableFactory<IParsable>> _, CancellationToken _) =>
                {
                    requestInfo.HttpMethod.Should().Be(Method.GET);
                    requestInfo.URI.Should().Be("http://base/url/applications?%24select=appId,displayName,passwordCredentials");

                    requestInfo.QueryParameters.Should().HaveCount(1);
                    requestInfo.QueryParameters["%24select"].As<string[]>().Should().Equal("appId", "displayName", "passwordCredentials");

                    requestInfo.Content.Should().BeSameAs(Stream.Null);
                })
                .ReturnsAsync((ApplicationCollectionResponse)null);

            var graphServiceClient = new Mock<GraphServiceClient>(MockBehavior.Strict, requestAdapter.Object, null);

            var graphServiceClientFactory = new Mock<IGraphServiceClientFactory>(MockBehavior.Strict);
            graphServiceClientFactory.Setup(gf => gf.Create("The tenant Id"))
                .Returns(graphServiceClient.Object);

            var client = new GraphEntraIdApplicationClient(graphServiceClientFactory.Object);

            await client.Invoking(c => c.GetAsync("The tenant Id", cancellationToken))
                .Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage("Unable to retrieve the list of the app registrations for the tenant 'The tenant Id'.");

            graphServiceClient.VerifyAll();
            graphServiceClientFactory.VerifyAll();
            requestAdapter.VerifyAll();
        }
    }
}