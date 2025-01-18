namespace codingfreaks.ApiConversion.Services.OpenApi.Extensions
{
    using Logic.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Identity.Web;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        #region methods

        /// <summary>
        /// Adds API versionning to the DI.
        /// </summary>
        /// <param name="services">The DI container.</param>
        /// <param name="configurationOptions">The Swagger configurationOptions.</param>
        /// <returns>The configured DI container.</returns>
        public static IServiceCollection AddApiVersioningInternal(
            this IServiceCollection services,
            OpenApiConfigurationOptions configurationOptions)
        {
            services.AddApiVersioning(
                    config =>
                    {
                        // Specify the default API Version
                        config.DefaultApiVersion = configurationOptions.ApiVersions!.Max()!;
                        // If the client hasn't specified the API version in the request, use the default API version number
                        config.AssumeDefaultVersionWhenUnspecified = true;
                        config.ReportApiVersions = true;
                    })
                .AddApiExplorer(
                    apiExplorerOptions =>
                    {
                        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                        // note: the specified format code will format the version as "'v'major[.minor][-status]"
                        apiExplorerOptions.GroupNameFormat = "'v'VVV";
                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        apiExplorerOptions.SubstituteApiVersionInUrl = true;
                    });
            return services;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configOptions"></param>
        /// <param name="identityOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenApiInternal(
            this IServiceCollection services,
            OpenApiConfigurationOptions configOptions,
            MicrosoftIdentityOptions identityOptions)
        {
            foreach (var apiVersion in configOptions.ApiVersions)
            {
                services.AddOpenApi($"v{apiVersion.MajorVersion}",
                    options =>
                    {
                        // add security definition and reference to the document
                        var scheme = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                AuthorizationCode = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri(
                                        $"{identityOptions.Instance}/{identityOptions.TenantId}/oauth2/v2.0/authorize"),
                                    TokenUrl = new Uri(
                                        $"{identityOptions.Instance}/{identityOptions.TenantId}/oauth2/v2.0/token"),
                                    Scopes = identityOptions.Scope.ToDictionary(s => s, s => s)
                                }
                            }
                        };
                        var referenceScheme = new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = nameof(SecuritySchemeType.OAuth2),
                                Type = ReferenceType.SecurityScheme
                            }
                        };
                        // add document transformer
                        options.AddDocumentTransformer(
                            (doc, ctx, _) =>
                            {
                                doc.Components ??= new OpenApiComponents();
                                doc.Components.SecuritySchemes.Add(nameof(SecuritySchemeType.OAuth2), scheme);
                                var contact = new OpenApiContact
                                {
                                    Name = configOptions.Contact
                                };
                                doc.Info.Contact = contact;
                                return Task.CompletedTask;
                            });
                        // add operation transformer
                        options.AddOperationTransformer(
                            (op, ctx, _) =>
                            {
                                var hasAnonymous = ctx.Description.ActionDescriptor.EndpointMetadata
                                    .OfType<IAllowAnonymous>()
                                    .Any();
                                var needsRequirementByAttribute = !hasAnonymous && ctx.Description.ActionDescriptor.EndpointMetadata
                                    .OfType<IAuthorizeData>()
                                    .Any();
                                if (needsRequirementByAttribute)
                                {
                                    var requirements = new List<OpenApiSecurityRequirement>
                                    {
                                        new()
                                        {
                                            [referenceScheme] = Array.Empty<string>()
                                        }
                                    };
                                    op.Security = requirements;
                                }
                                return Task.CompletedTask;
                            });
                    });
            }
            return services;
        }

        #endregion
    }
}