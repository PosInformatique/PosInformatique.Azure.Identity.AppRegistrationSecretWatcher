//-----------------------------------------------------------------------
// <copyright file="AppRegistrationSecretStatus.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Azure.Identity.AppRegistrationSecretWatcher
{
    public enum AppRegistrationSecretStatus
    {
        Valid,

        ExpiringSoon,

        Expired,
    }
}