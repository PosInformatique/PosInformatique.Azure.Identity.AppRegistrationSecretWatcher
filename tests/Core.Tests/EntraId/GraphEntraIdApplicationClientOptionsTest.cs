//-----------------------------------------------------------------------
// <copyright file="GraphEntraIdApplicationClientOptionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.EntraId.Tests
{
    public class GraphEntraIdApplicationClientOptionsTest
    {
        [Fact]
        public void Constructor()
        {
            var options = new GraphEntraIdApplicationClientOptions();

            options.ClientId.Should().BeNull();
            options.ClientSecret.Should().BeNull();
        }

        [Fact]
        public void ClientId_ValueChanged()
        {
            var options = new GraphEntraIdApplicationClientOptions();

            options.ClientId = "The client ID";

            options.ClientId.Should().Be("The client ID");
        }

        [Fact]
        public void ClientSecret_ValueChanged()
        {
            var options = new GraphEntraIdApplicationClientOptions();

            options.ClientSecret = "The client secret";

            options.ClientSecret.Should().Be("The client secret");
        }
    }
}