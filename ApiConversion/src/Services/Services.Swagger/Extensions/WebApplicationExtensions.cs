namespace codingfreaks.ApiConversion.Services.Swagger.Extensions
{
    using Asp.Versioning.ApiExplorer;

    using Logic.Models;

    internal static class WebApplicationExtensions
    {
        #region methods

        public static void UseSwaggerUiInternal(this WebApplication app, SwaggerOptions options)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                opt =>
                {
                    opt.DocumentTitle = "My API";
                    if (options.Versions.Any())
                    {
                        var apiProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                        foreach (var description in apiProvider.ApiVersionDescriptions.OrderByDescending(a => a.ApiVersion))
                        {
                            opt.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    }
                    // OAuth
                    opt.OAuthClientId(options.AuthClientId);
                    opt.OAuthScopes(options.AuthScopes.ToArray());
                    opt.OAuthScopeSeparator(" ");
                    opt.EnablePersistAuthorization();
                    opt.OAuthUsePkce();
                });
        }

        #endregion
    }
}