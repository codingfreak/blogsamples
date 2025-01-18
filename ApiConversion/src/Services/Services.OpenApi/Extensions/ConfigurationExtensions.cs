namespace codingfreaks.ApiConversion.Services.OpenApi.Extensions
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
        /// Allows logic to access the OpenAPI options from the config if DI is not available yet.
        /// </summary>
        /// <param name="configuration">The configuration accessor.</param>
        /// <returns>The configured OpenAPI options.</returns>
        /// <exception cref="ApplicationException">Is thrown if no options could be found or mapped.</exception>
        public static OpenApiConfigurationOptions GetOpenApiOptions(this IConfiguration configuration)
        {
            return configuration.GetSection("OpenApi")
                .Get<OpenApiConfigurationOptions>() ?? throw new ApplicationException("Could not map OpenAPI options.");
        }

        #endregion
    }
}