namespace codingfreaks.ApiConversion.Logic.Interfaces
{
    using System.Collections.ObjectModel;

    using Models;

    /// <summary>
    /// Must be implemented by services which are able to deliver weather data.
    /// </summary>
    public interface IWeatherService
    {
        #region methods

        /// <summary>
        /// Retrieves a collection of weather forecasts for the given <paramref name="location" />.
        /// </summary>
        /// <param name="location">The name of the location for which to deliver forecasts.</param>
        /// <param name="days">The amount of days for which to deliver forecasts.</param>
        /// <returns>The forecasts.</returns>
        ValueTask<ReadOnlyCollection<WeatherForecast>> GetForecastsAsync(string location, int days = 5);

        #endregion
    }
}