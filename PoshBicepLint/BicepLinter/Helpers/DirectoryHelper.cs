namespace codingfreaks.PoshBicepLint.Helpers
{
    /// <summary>
    /// Provides helper methods for directories.
    /// </summary>
    internal static class DirectoryHelper
    {
        #region methods

        /// <summary>
        /// Retrieves all files matching the given <paramref name="pattern" /> by searching recursively in the given
        /// <paramref name="path" />.
        /// </summary>
        /// <param name="path">The path where to search for files.</param>
        /// <param name="pattern">The pattern for the files.</param>
        /// <returns>All URIs of the matched files.</returns>
        public static string[] GetFilesRecursive(string path, string pattern)
        {
            var files = Directory.GetFiles(path, pattern)
                .ToList();
            foreach (var dir in Directory.GetDirectories(path))
            {
                files.AddRange(GetFilesRecursive(dir, pattern));
            }
            return files.ToArray();
        }

        #endregion
    }
}