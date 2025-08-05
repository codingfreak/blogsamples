namespace SampleApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        #region member vars

        private readonly MyAppOptions _appOptions;

        private readonly IConfiguration _configuration;

        private readonly IHostEnvironment _environment;

        private Logic _myLogic;

        #endregion

        #region constructors and destructors

        public SampleController(
            IConfiguration configuration,
            IOptions<MyAppOptions> appOptions,
            Logic myLogic,
            IHostEnvironment environment)
        {
            _configuration = configuration;
            _myLogic = myLogic;
            _environment = environment;
            _appOptions = appOptions.Value;
        }

        #endregion

        #region methods

        [HttpGet]
        public IActionResult Get()
        {
            var result = new
            {
                Environment = _environment.EnvironmentName,
                DbConnectionString = _configuration.GetConnectionString("MyDb"),
                LoggingSection = _configuration.GetSection("Logging:LogLevel"),
                MyConfig = _appOptions
            };
            return Ok(result);
        }

        #endregion
    }
}