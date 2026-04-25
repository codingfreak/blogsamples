namespace codingfreaks.PoshBicepLint
{
    public class LinterResult
    {
        #region constructors and destructors

        public LinterResult(string filename, string[] results)
        {
            Filename = filename;
            Results = results;
        }

        #endregion

        #region properties

        public string Filename { get; }

        public string[] Results { get; }

        public bool Succeeded => !Results.Any();

        #endregion
    }
}