namespace codingfreaks.ApiConversion.Services.OpenApi.Extensions
{
    using Asp.Versioning.ApiExplorer;

    using Logic.Models;

    using Microsoft.Identity.Web;

    /// <summary>
    /// Provides extensions for <see cref="WebApplication" />.
    /// </summary>
    internal static class WebApplicationExtensions
    {
        #region methods

        /// <summary>
        /// Adds Swagger UI to the <paramref name="app" />.
        /// </summary>
        /// <param name="app">The app before it runs.</param>
        /// <param name="configurationOptions">The configurationOptions for Swagger.</param>
        /// <param name="identityOptions"></param>
        public static void UseSwaggerUiInternal(
            this WebApplication app,
            OpenApiConfigurationOptions configurationOptions,
            MicrosoftIdentityOptions identityOptions)
        {
            app.UseSwaggerUI(
                opt =>
                {
                    if (configurationOptions.ApiVersions.Any())
                    {
                        var apiProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
                        if (apiProvider != null)
                        {
                            foreach (var description in apiProvider.ApiVersionDescriptions.OrderByDescending(
                                         a => a.ApiVersion))
                            {
                                opt.SwaggerEndpoint(
                                    $"/openapi/{description.GroupName}.json",
                                    description.GroupName.ToLowerInvariant());
                            }
                        }
                        opt.OAuthClientId(identityOptions.ClientId);
                        opt.EnablePersistAuthorization();
                        opt.OAuthUsePkce();
                    }
                });
        }

        #endregion
    }
}