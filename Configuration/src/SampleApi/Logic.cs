namespace SampleApi
{
    using Microsoft.Extensions.Options;

    public class Logic
    {
        #region member vars

        private MyAppOptions _appOptions;

        #endregion

        #region constructors and destructors

        public Logic(IOptionsSnapshot<MyAppOptions> appOptions)
        {
            _appOptions = appOptions.Value;
        }

        #endregion
    }
}