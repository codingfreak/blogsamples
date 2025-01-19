namespace codingfreaks.ApiConversion.Services.OpenApi.Extensions
{
    using Asp.Versioning.ApiExplorer;

    using Logic.Models;

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
        /// <param name="options">The options for OpenAPI.</param>
        public static void UseSwaggerUiInternal(this WebApplication app, OpenApiConfigurationOptions options)
        {
            app.UseSwaggerUI(
                opt =>
                {
                    var identityOptions = app.Configuration.GetIdentityOptions();
                    if (options.ApiVersions.Any())
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
                    }
                    // OAuth
                    opt.OAuthClientId(identityOptions.ClientId);
                    opt.EnablePersistAuthorization();
                    if (identityOptions.UsePkce)
                    {
                        opt.OAuthUsePkce();
                    }
                });
        }

        #endregion
    }
}