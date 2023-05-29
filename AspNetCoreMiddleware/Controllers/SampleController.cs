namespace AspNetCoreMiddleware.Controllers
{
    using System.Globalization;

    using Attributes;

    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    //[CheckCulture]
    public class SampleController : ControllerBase
    {
        #region methods

        [HttpGet]
        public ActionResult<string?> Get()
        {
            var culture = Request.Headers["Culture"].FirstOrDefault();
            if (!string.IsNullOrEmpty(culture))
            {
                return Ok(new CultureInfo(culture).DisplayName);
            }
            return BadRequest("Could not parse culture");
        }

        #endregion

    }
}