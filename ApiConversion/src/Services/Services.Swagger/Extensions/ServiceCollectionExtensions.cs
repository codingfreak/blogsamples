namespace codingfreaks.ApiConversion.Services.Swagger.Extensions
{
    using System.Reflection;
    using System.Xml.Linq;

    using Asp.Versioning.ApiExplorer;

    using Logic.Models;

    using Microsoft.Identity.Web;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

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
        /// <param name="options">The Swagger options.</param>
        /// <returns>The configured DI container.</returns>
        public static IServiceCollection AddApiVersioningInternal(
            this IServiceCollection services,
            SwaggerOptions options)
        {
            services.AddApiVersioning(
                    config =>
                    {
                        // Specify the default API Version
                        config.DefaultApiVersion = options.ApiVersions!.Max()!;
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
        /// Adds Swagger generation to the DI.
        /// </summary>
        /// <param name="services">The DI container.</param>
        /// <param name="builder">The web-app-builder.</param>
        /// <param name="options">The Swagger options.</param>
        /// <param name="identityOptions">The options for MS identity.</param>
        /// <returns>The configured DI container.</returns>
        public static IServiceCollection AddSwaggerGenInternal(
            this IServiceCollection services,
            WebApplicationBuilder builder,
            SwaggerOptions options,
            MicrosoftIdentityOptions identityOptions)
        {
            services.AddSwaggerGen(
                opt =>
                {
                    opt.CustomOperationIds(e =>
                    {
                        var controller = e.ActionDescriptor.RouteValues["controller"]
                            ?.ToLowerInvariant() ?? "unknown";
                        var method = e.HttpMethod?.ToLowerInvariant() ?? "unknown";
                        var action = e.ActionDescriptor.RouteValues["action"]
                            ?.ToLowerInvariant().Replace(method, string.Empty) ?? "unknown";
                        return $"{controller}-{action}-{method}";
                    });
                    opt.UseVersioning(options, builder);
                    opt.UseXmlFile();
                    opt.UseOauth(options, identityOptions);
                });
            return services;
        }

        /// <summary>
        /// Reads in all XML documentation files in the current folder and merges all <member />-tags
        /// into the <paramref name="xmlCommentsFile" /> <members />-section.
        /// </summary>
        /// <param name="basePath">The directory path in which to search for the files.</param>
        /// <param name="xmlCommentsFile">The name of the target file.</param>
        /// <returns>The full path of the resulting file.</returns>
        private static string JoinXmlComments(string basePath, string xmlCommentsFile)
        {
            try
            {
                var allFiles = new DirectoryInfo(basePath).GetFiles("*.xml");
                var target = XDocument.Load(xmlCommentsFile);
                var targetElement = target.Elements("doc")
                    .Elements("members")
                    .Single();
                foreach (var file in allFiles)
                {
                    if (file.Name == xmlCommentsFile)
                    {
                        // this is our target file
                        continue;
                    }
                    var source = XDocument.Load(file.FullName);
                    var allMembers = source.Elements("doc")
                        .Elements("members")
                        .Elements();
                    // make sure to not write any duplicates
                    targetElement.Add(
                        allMembers.Where(
                            m => !targetElement.Elements()
                                .Any(e => XNode.DeepEquals(m, e))));
                }
                var resultFile = Path.Combine(basePath, "joined.xml");
                target.Save(resultFile);
                return resultFile;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not join XML comments.", ex);
            }
        }

        private static void UseOauth(
            this SwaggerGenOptions genOptions,
            SwaggerOptions options,
            MicrosoftIdentityOptions identityOptions)
        {
            genOptions.AddSecurityDefinition(
                nameof(SecuritySchemeType.OAuth2),
                new OpenApiSecurityScheme
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
                });
            genOptions.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = nameof(SecuritySchemeType.OAuth2)
                            }
                        },
                        identityOptions.Scope.ToArray()
                    }
                });
        }

        private static void UseVersioning(
            this SwaggerGenOptions genOptions,
            SwaggerOptions options,
            WebApplicationBuilder builder)
        {
            var provider = builder?.Services?.BuildServiceProvider()
                .GetService<IApiVersionDescriptionProvider>();
            if (provider != null)
            {
                foreach (var description in provider.ApiVersionDescriptions
                             .OrderByDescending(v => v.ApiVersion.MajorVersion)
                             .ThenByDescending(v => v.ApiVersion.MinorVersion))
                {
                    var info = new OpenApiInfo
                    {
                        Title = options.ApiName,
                        Version = description.ApiVersion.ToString(),
                        Description = options.Description,
                        Contact = new OpenApiContact
                        {
                            Name = options.Contact
                        }
                    };
                    genOptions.SwaggerDoc(description.GroupName, info);
                }
            }
        }

        /// <summary>
        /// Is adding XML doc support to the <paramref name="genOptions" /> if possible.
        /// </summary>
        /// <param name="genOptions">The Swagger generation options.</param>
        private static void UseXmlFile(this SwaggerGenOptions genOptions)
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                return;
            }
            var basePath = Path.GetDirectoryName(assembly.Location) ?? string.Empty;
            var assemblyFile = Path.Combine(
                basePath,
                assembly.GetName()
                    .Name + ".xml");
            var file = JoinXmlComments(basePath, assemblyFile);
            genOptions.IncludeXmlComments(file);
        }

        #endregion
    }
}