namespace SampleApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {

        private IConfiguration _configuration;

        public SampleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = new
            {
                DbConnection = _configuration["ConnectionStrings:MyDatabase"],
                StorageConnection = _configuration.GetConnectionString("MyStorage")
            };
            return Ok(result);
        }
    }
}
