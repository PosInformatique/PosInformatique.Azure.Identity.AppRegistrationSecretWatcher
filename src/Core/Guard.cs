//-----------------------------------------------------------------------
// <copyright file="Guard.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique
{
    using System.Diagnostics;

    internal static class Guard
    {
        [DebuggerNonUserCode]
        public static void IsUtc(DateTime dateTime, string paramName)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The argument must be an UTC date time.", paramName);
            }
        }
    }
}
