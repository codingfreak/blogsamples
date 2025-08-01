namespace SampleApi
{
    public class MyAppOptions
    {
        public static readonly string ConfigKey = "MyAppSettings";

        #region properties

        public TimeSpan Timeout { get; set; }

        public int B { get; set; }

        public int C { get; set; }

        #endregion
    }

}