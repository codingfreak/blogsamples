namespace codingfreaks.ApiConversion.Logic.Models
{
    /// <summary>
    /// Represents weather forecast data for 1 location.
    /// </summary>
    public class WeatherForecast
    {
        #region properties

        /// <summary>
        /// The date of the forecast.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// The temperature in degrees Celsius.
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// The temperature in degrees Fahrenheit.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// A summary text.
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// The location for which this forecasts was generated.
        /// </summary>
        public string? Location { get; set; }

        #endregion
    }
}