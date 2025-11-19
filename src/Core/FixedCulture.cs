//-----------------------------------------------------------------------
// <copyright file="FixedCulture.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    using System.Globalization;

    public class FixedCulture : ICulture
    {
        private readonly CultureInfo current;

        public FixedCulture(string name)
        {
            this.current = CultureInfo.GetCultureInfo(name);
        }

        public CultureInfo Current => this.current;
    }
}