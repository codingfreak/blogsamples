namespace codingfreaks.ApiConversion.Logic.WeatherMock
{
    using System.Collections.ObjectModel;

    using Interfaces;

    using Models;

    /// <summary>
    /// A weather services which generates mock data.
    /// </summary>
    public class MockWeatherService : IWeatherService
    {
        #region constants

        private static readonly string[] Summaries =
            ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public ValueTask<ReadOnlyCollection<WeatherForecast>> GetForecastsAsync(string location, int days = 5)
        {
            var result = Enumerable.Range(1, days)
                .Select(
                    index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                        Location = location
                    })
                .ToList()
                .AsReadOnly();
            return ValueTask.FromResult(result);
        }

        #endregion
    }
}