namespace codingfreaks.ApiConversion.Services.Swagger.Controllers
{
    using Asp.Versioning;

    using Logic.Interfaces;
    using Logic.Models;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Provides endpoints for weather data.
    /// </summary>
    [ApiController]
    //[ApiVersion(1.0)]
    //[ApiVersion(2.0)]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        #region member vars

        private readonly IWeatherService _weatherService;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// </summary>
        /// <param name="weatherService"></param>
        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        #endregion

        #region methods

        /// <summary>
        /// Retrieves weather forecast data.
        /// </summary>
        /// <returns>The weather data.</returns>
        /// <response code="200">Valid weather data.</response>
        /// <response code="404">No weather data was found.</response>
        /// <response code="500">A server error occurred.</response>
        [HttpGet]
        public async ValueTask<ActionResult<WeatherForecast[]>> Get(string location)
        {
            var result = await _weatherService.GetForecastsAsync(location);
            return Ok(result);
        }

        #endregion
    }
}