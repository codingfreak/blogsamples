namespace codingfreaks.PoshBicepLint.Models
{
    /// <summary>
    /// Represents the result of the linting operation of 1 file.
    /// </summary>
    public class LinterResult
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="fileUri">The full URI of the parsed file.</param>
        /// <param name="results">The warnings or errors which occured, if any.</param>
        public LinterResult(string fileUri, string[] results)
        {
            FileUri = fileUri;
            Results = results;
        }

        #endregion

        #region properties

        /// <summary>
        /// The full URI of the parsed file.
        /// </summary>
        public string FileUri { get; }

        /// <summary>
        /// The warnings or errors which occured, if any.
        /// </summary>
        public string[] Results { get; }

        /// <summary>
        /// Indicates if <see cref="Results" /> has any entry -> meaning warnings or errors.
        /// </summary>
        public bool Succeeded => !Results.Any();

        #endregion
    }
}