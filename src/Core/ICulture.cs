//-----------------------------------------------------------------------
// <copyright file="ICulture.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    using System.Globalization;

    public interface ICulture
    {
        CultureInfo Current { get; }
    }
}