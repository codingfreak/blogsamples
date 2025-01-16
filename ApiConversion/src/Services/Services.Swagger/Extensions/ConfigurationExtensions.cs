namespace codingfreaks.ApiConversion.Services.Swagger.Extensions
{
    using Logic.Models;

    using Microsoft.Identity.Web;

    /// <summary>
    /// Provides extension methods for the type <see cref="IConfiguration" />.
    /// </summary>
    public static class ConfigurationExtensions
    {
        #region methods

        /// <summary>
        /// Allows logic to access the MS identity options from the config if DI is not available yet.
        /// </summary>
        /// <param name="configuration">The configuration accessor.</param>
        /// <returns>The configured identity options.</returns>
        /// <exception cref="ApplicationException">Is thrown if no options could be found or mapped.</exception>
        public static MicrosoftIdentityOptions GetIdentityOptions(this IConfiguration configuration)
        {
            return configuration.GetSection("AzureAd")
                .Get<MicrosoftIdentityOptions>() ?? throw new ApplicationException("Could not map identity options.");
        }

        /// <summary>
        /// Allows logic to access the Swagger options from the config if DI is not available yet.
        /// </summary>
        /// <param name="configuration">The configuration accessor.</param>
        /// <returns>The configured Swagger options.</returns>
        /// <exception cref="ApplicationException">Is thrown if no options could be found or mapped.</exception>
        public static SwaggerOptions GetSwaggerOptions(this IConfiguration configuration)
        {
            return configuration.GetSection("Swagger")
                .Get<SwaggerOptions>() ?? throw new ApplicationException("Could not map Swagger options.");
        }

        #endregion
    }
}