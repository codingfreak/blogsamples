namespace codingfreaks.ApiConversion.Services.OpenApi.Extensions
{
    using Logic.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Identity.Web;
    using Microsoft.OpenApi.Models;

    using System.Reflection;
    using System.Xml;

    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        #region methods


        /// <summary>
        /// Retrieves all controller-types from the entry-assembly.
        /// </summary>
        private static readonly Lazy<Type[]> ControllerTypes = new(
            () =>
            {
                return Assembly.GetEntryAssembly()
                    ?.GetTypes()
                    ?.Where(
                        t => t is { IsPublic: true, IsAbstract: false } && typeof(ControllerBase).IsAssignableFrom(t))
                    ?.ToArray() ?? [];
            });

        /// <summary>
        /// Tries to retrieve a key-value-list by reading the documentation file in. The key would be the value of
        /// the attribute "name" of all the member tags and the value the summary in it.
        /// </summary>
        private static readonly Lazy<Dictionary<string, string>> XmlSummaryInfo = new(
            () =>
            {
                var result = new Dictionary<string, string>();
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    var xmlFileName = assembly.GetName()
                        .Name + ".xml";
                    var xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFileName);
                    if (File.Exists(xmlFile))
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(xmlFile);
                        var nodes = xmlDoc.SelectNodes("doc/members/member");
                        if (nodes != null)
                        {
                            foreach (XmlNode node in nodes)
                            {
                                if (node is not { Attributes: not null, FirstChild.InnerText: not null })
                                {
                                    continue;
                                }
                                var att = node.Attributes["name"];
                                if (att != null)
                                {
                                    result.Add(
                                        att.Value,
                                        node.FirstChild.InnerText.Replace("\r", string.Empty)
                                            .Replace("\n", string.Empty)
                                            .Trim());
                                }
                            }
                        }
                    }
                }
                return result;
            });

        /// <summary>
        /// Adds API versionning to the DI.
        /// </summary>
        /// <param name="services">The DI container.</param>
        /// <param name="options">The Swagger options.</param>
        /// <returns>The configured DI container.</returns>
        public static IServiceCollection AddApiVersioningInternal(
            this IServiceCollection services,
            OpenApiConfigurationOptions options)
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
        /// Adds and configures OpenAPI services to the DI.
        /// </summary>
        /// <param name="services">The DI container.</param>
        /// <param name="configOptions">The OpenAPI options from the config.</param>
        /// <param name="identityOptions">The options for the MS identity config.</param>
        /// <returns>The configured DI container.</returns>
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
                        var referenceScheme = new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = nameof(SecuritySchemeType.OAuth2),
                                Type = ReferenceType.SecurityScheme
                            }
                        };
                        var requirements = new List<OpenApiSecurityRequirement>
                        {
                            new()
                            {
                                [referenceScheme] = Array.Empty<string>()
                            }
                        };
                        options.AddDocumentTransformer(
                            (doc, ctx, _) =>
                            {
                                doc.Components ??= new OpenApiComponents();
                                // meta data
                                doc.Info.Title = configOptions.ApiName;
                                doc.Info.Description = configOptions.Description;
                                doc.Info.Contact = new OpenApiContact
                                {
                                    Name = configOptions.Contact
                                };
                                // auth stuff
                                var oauthScheme = new OpenApiSecurityScheme
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
                                doc.Components.SecuritySchemes.Add(nameof(SecuritySchemeType.OAuth2), oauthScheme);
                                return Task.CompletedTask;
                            });
                        options.AddOperationTransformer(
                            (op, ctx, _) =>
                            {
                                // operation id
                                if (!ctx.Description.ActionDescriptor.EndpointMetadata.OfType<EndpointNameAttribute>()
                                        .Any())
                                {
                                    // We need to generate the operation id because no EndpointNameAttribute was found on this endpoint.
                                    var httpMethodName = ctx.Description.HttpMethod?.ToLower() ?? "unknown";
                                    var controllerName = ctx.Description.ActionDescriptor.RouteValues["controller"]
                                        ?.ToLower() ?? "-";
                                    var actionName = ctx.Description.ActionDescriptor.RouteValues["action"]
                                        ?.ToLower() ?? "-";
                                    actionName = actionName.Replace(httpMethodName, string.Empty);
                                    op.OperationId = $"{controllerName}-{actionName}-{httpMethodName}";
                                }
                                // endpoint auth requirement
                                var allowsAnonymous = ctx.Description.ActionDescriptor.EndpointMetadata
                                    .OfType<IAllowAnonymous>()
                                    .Any();
                                var needsRequirement = !allowsAnonymous && ctx.Description.ActionDescriptor.EndpointMetadata
                                    .OfType<IAuthorizeData>()
                                    .Any();
                                if (needsRequirement)
                                {
                                    op.Security = requirements;
                                }
                                // take care about XML documentation
                                if (!ctx.Description.ActionDescriptor.EndpointMetadata
                                        .OfType<EndpointDescriptionAttribute>()
                                        .Any())
                                {
                                    // Lets generate the description automatically because no EndpointDescriptionAttribute was found on this endpoint.
                                    if (ctx.Description.ActionDescriptor is ControllerActionDescriptor
                                        controllerActionDescriptor)
                                    {
                                        var type = ControllerTypes.Value.FirstOrDefault(
                                            t => t.Name == $"{controllerActionDescriptor.ControllerName}Controller");
                                        if (type != null)
                                        {
                                            var method = type.GetMethod(controllerActionDescriptor.ActionName);
                                            if (method == null)
                                            {
                                                method = type.GetMethod(
                                                    controllerActionDescriptor.ActionName + "Async");
                                            }
                                            if (method != null)
                                            {
                                                var elementName = $"M:{type.Namespace}.{type.Name}.{method.Name}";
                                                var parameters = method.GetParameters();
                                                if (parameters.Any())
                                                {
                                                    elementName += "(";
                                                    foreach (var parameter in parameters)
                                                    {
                                                        elementName += parameter.ParameterType.FullName + ",";
                                                    }
                                                    elementName = elementName.TrimEnd(',');
                                                    elementName += ")";
                                                }
                                                if (XmlSummaryInfo.Value.TryGetValue(elementName, out var desc))
                                                {
                                                    op.Description = desc;
                                                    if (!ctx.Description.ActionDescriptor.EndpointMetadata
                                                            .OfType<EndpointSummaryAttribute>()
                                                            .Any())
                                                    {
                                                        // Lets also set the summary to the value.
                                                        op.Summary = op.Description;
                                                    }
                                                }
                                            }
                                        }
                                    }
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