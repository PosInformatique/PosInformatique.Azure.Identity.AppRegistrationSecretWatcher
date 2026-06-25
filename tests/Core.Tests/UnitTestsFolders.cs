//-----------------------------------------------------------------------
// <copyright file="UnitTestsFolders.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique
{
    internal static class UnitTestsFolders
    {
        static UnitTestsFolders()
        {
            SolutionRoot = GetSolutionRootPath();
            TemporaryRoot = Path.Combine(SolutionRoot, "tmp");
        }

        public static string TemporaryRoot { get; }

        private static string SolutionRoot { get; }

        private static string GetSolutionRootPath()
        {
            var directory = new DirectoryInfo("./");

            while (directory is not null && !directory.EnumerateFiles("PosInformatique.Azure.Identity.AppRegistrationSecretWatcher.slnx").Any())
            {
                directory = directory.Parent;
            }

            if (directory is null)
            {
                throw new DirectoryNotFoundException("Unable to find the PosInformatique.Azure.Identity.AppRegistrationSecretWatcher solution root path.");
            }

            return directory.FullName;
        }
    }
}