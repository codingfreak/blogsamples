namespace codingfreaks.ApiConversion.Services.Swagger.Extensions
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
        /// <param name="options">The options for Swagger.</param>
        public static void UseSwaggerUiInternal(this WebApplication app, SwaggerOptions options)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                opt =>
                {
                    var identityOptions = app.Configuration.GetIdentityOptions();
                    opt.DocumentTitle = options.ApiName;
                    if (options.ApiVersions.Any())
                    {
                        var apiProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
                        if (apiProvider != null)
                        {
                            foreach (var description in apiProvider.ApiVersionDescriptions.OrderByDescending(
                                         a => a.ApiVersion))
                            {
                                opt.SwaggerEndpoint(
                                    $"/swagger/{description.GroupName}/swagger.json",
                                    description.GroupName.ToLowerInvariant());
                            }
                        }
                    }
                    // OAuth
                    opt.OAuthClientId(identityOptions.ClientId);
                    opt.OAuthScopes(identityOptions.Scope.ToArray());
                    opt.OAuthScopeSeparator(" ");
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