namespace codingfreaks.samples.AppInsightsEntraAuth.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        #region constructors and destructors

        public TestController(ILogger<TestController> logger)
        {
            Logger = logger;
        }

        #endregion

        #region methods

        [HttpGet]
        public IActionResult Get()
        {
            Logger.LogInformation("Get was called.");
            var result = new
            {
                Timestamp = DateTimeOffset.Now,
                Message = "Hello",
                AppInsights = new
                {
                    ConnectionString =
                        Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING") ?? "-",
                    AuthenticationString =
                        Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_AUTHENTICATION_STRING") ?? "-"
                }
            };
            return Ok(result);
        }

        #endregion

        #region properties

        private ILogger<TestController> Logger { get; }

        #endregion
    }
}