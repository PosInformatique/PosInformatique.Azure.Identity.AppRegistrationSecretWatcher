//-----------------------------------------------------------------------
// <copyright file="FixedCultureTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.Tests
{
    using System.Globalization;

    public class FixedCultureTest
    {
        [Fact]
        public void Constructor()
        {
            var culture = new FixedCulture("fr");

            culture.Current.Should().BeSameAs(CultureInfo.GetCultureInfo("fr"));
        }
    }
}