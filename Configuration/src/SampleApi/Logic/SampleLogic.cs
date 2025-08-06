namespace SampleApi.Logic
{
    using Microsoft.Extensions.Options;

    using Models;

    public class SampleLogic
    {
        #region member vars

        private MyAppOptions _appOptions;

        #endregion

        #region constructors and destructors

        public SampleLogic(IOptionsSnapshot<MyAppOptions> appOptions)
        {
            _appOptions = appOptions.Value;
        }

        #endregion
    }
}