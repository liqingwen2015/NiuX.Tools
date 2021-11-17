using JetBrains.Annotations;
using NiuX;

namespace System.IO
{
    public static class NiuXDirectoryInfoExtensions
    {
        public static void CreateIfNotExists(this DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        public static bool IsSubDirectoryOf([NotNull] this DirectoryInfo parentDirectory,
            [NotNull] DirectoryInfo childDirectory)
        {
            Checker.NotNull(parentDirectory, nameof(parentDirectory));
            Checker.NotNull(childDirectory, nameof(childDirectory));

            if (parentDirectory.FullName == childDirectory.FullName)
            {
                return true;
            }

            var parentOfChild = childDirectory.Parent;
            if (parentOfChild == null)
            {
                return false;
            }

            return IsSubDirectoryOf(parentDirectory, parentOfChild);
        }

    }
}
