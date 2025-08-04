namespace SampleApi
{
    using Microsoft.Extensions.Options;

    public class Logic
    {

        private MyAppOptions _appOptions;

        public Logic(IOptionsSnapshot<MyAppOptions> appOptions)
        {
            _appOptions = appOptions.Value;
        }
    }
}
