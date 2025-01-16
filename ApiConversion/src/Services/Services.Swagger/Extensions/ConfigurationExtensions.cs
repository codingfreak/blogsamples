namespace codingfreaks.ApiConversion.Services.Swagger.Extensions
{
    using Logic.Models;

    using Microsoft.Identity.Web;

    public static class ConfigurationExtensions
    {
        #region methods

        public static MicrosoftIdentityOptions GetIdentityOptions(this IConfiguration configuration)
        {
            return configuration.GetSection("AzureAd")
                .Get<MicrosoftIdentityOptions>() ?? throw new ApplicationException("Could not map identity options.");
        }

        public static SwaggerOptions GetSwaggerOptions(this IConfiguration configuration)
        {
            return configuration.GetSection("Swagger")
                .Get<SwaggerOptions>() ?? throw new ApplicationException("Could not map Swagger options.");
        }

        #endregion
    }
}