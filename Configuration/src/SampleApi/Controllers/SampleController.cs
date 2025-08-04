namespace SampleApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {

        private IConfiguration _configuration;

        private MyAppOptions _appOptions;

        private IHostEnvironment _environment;

        private Logic _myLogic;

        public SampleController(IConfiguration configuration, IOptions<MyAppOptions> appOptions, Logic myLogic, IHostEnvironment environment)
        {
            _configuration = configuration;
            _myLogic = myLogic;
            _environment = environment;
            _appOptions = appOptions.Value;
        }

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
    }
}
