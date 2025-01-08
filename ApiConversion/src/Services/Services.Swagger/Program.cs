using System.Reflection;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using codingfreaks.ApiConversion.Logic.Interfaces;
using codingfreaks.ApiConversion.Logic.WeatherMock;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var apiName = "My API";
var clientId = "b3c9c391-6981-44e5-a2d7-8db105561655";
var instance = "https://login.microsoftonline.com";
var tenant = "devdeer.com";
var scopes = new[] { "api://b3c9c391-6981-44e5-a2d7-8db105561655/full" };
var versions = new[] { new ApiVersion(1, 0), new ApiVersion(2, 0) };
// Add services to the container.
builder.Services.AddScoped<IWeatherService, MockWeatherService>();
builder.Services.AddControllers();
builder.Services.AddApiVersioning(
        config =>
        {
            // Specify the default API Version
            config.DefaultApiVersion = versions!.Max()!;
            // If the client hasn't specified the API version in the request, use the default API version number
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        })
    .AddApiExplorer(
        versionOptions =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            versionOptions.GroupNameFormat = "'v'VVV";
            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            versionOptions.SubstituteApiVersionInUrl = true;
        });
builder.Services.AddSwaggerGen(
    opt =>
    {
        var provider = builder?.Services?.BuildServiceProvider()
            .GetService<IApiVersionDescriptionProvider>();
        if (provider != null)
        {
            foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(v => v.ApiVersion.MajorVersion)
                         .ThenByDescending(v => v.ApiVersion.MinorVersion))
            {
                var info = new OpenApiInfo
                {
                    Title = apiName,
                    Version = description.ApiVersion.ToString(),
                    Description = "Just a sample",
                    Contact = new OpenApiContact
                    {
                        Name = "codingfreaks"
                    }
                };
                opt.SwaggerDoc(description.GroupName, info);
            }
        }
        opt.AddSecurityDefinition(
            nameof(SecuritySchemeType.OAuth2),
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{instance}/{tenant}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"{instance}/{tenant}/oauth2/v2.0/token"),
                        Scopes = scopes.ToDictionary(s => s, s => s)
                    }
                }
            });
        opt.AddSecurityRequirement(
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
                    scopes.ToArray()
                }
            });
        opt.CustomOperationIds(
            e =>
                $"{e.ActionDescriptor.RouteValues["controller"]?.ToLowerInvariant() ?? "unkown"}-{e.ActionDescriptor.RouteValues["action"]?.ToLowerInvariant() ?? "unkown"}-{e.HttpMethod?.ToLowerInvariant() ?? "unkown"}");
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
        {
            return;
        }
        var basePath = Path.GetDirectoryName(assembly.Location);
        var fileName = assembly.GetName()
            .Name + ".xml";
        var xmlCommentsFile = Path.Combine(basePath, fileName);
        if (!File.Exists(xmlCommentsFile))
        {
            return;
        }
        opt.IncludeXmlComments(xmlCommentsFile);
    });
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(
    opt =>
    {
        opt.DocumentTitle = "My API";
        if (versions.Any())
        {
            var apiProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in apiProvider.ApiVersionDescriptions.OrderByDescending(a => a.ApiVersion))
            {
                opt.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        }
        // OAuth
        opt.OAuthClientId(clientId);
        opt.OAuthScopes(scopes.ToArray());
        opt.OAuthScopeSeparator(" ");
        opt.EnablePersistAuthorization();
        opt.OAuthUsePkce();
    });
app.Run();