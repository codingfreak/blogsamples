namespace SampleApi.Models
{
    public class MyAppOptions
    {
        #region constants

        public static readonly string ConfigKey = "MyAppSettings";

        #endregion

        #region properties

        public TimeSpan Timeout { get; set; }

        public int B { get; set; }

        public int C { get; set; }

        #endregion
    }
}