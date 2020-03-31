namespace ApiVersioningSample
{
    using System;
    using System.Linq;

    using Filters;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Provides the implementation of the startup logic called by <see cref="Program"/>.
    /// </summary>
    public class Startup
    {
        #region methods

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The app builder.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                });
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "codingfreaks API v1.0");
                    c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "codingfreaks API v1.1");
                    c.SwaggerEndpoint("/swagger/v2.0/swagger.json", "codingfreaks API v2.0");
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection to be extended.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1.0",
                        new OpenApiInfo
                        {
                            Title = "codingfreaks API",
                            Version = "v1.0"
                        });
                    c.SwaggerDoc(
                        "v1.1",
                        new OpenApiInfo
                        {
                            Title = "codingfreaks API",
                            Version = "v1.1"
                        });
                    c.SwaggerDoc(
                        "v2.0",
                        new OpenApiInfo
                        {
                            Title = "codingfreaks API",
                            Version = "v2.0"
                        });
                    // configure filters
                    c.OperationFilter<RemoveVersionParameterFilter>();
                    c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                    // Take API versioning attributes from ASP.NET into account
                    c.DocInclusionPredicate(
                        (version, desc) =>
                        {
                            var versionAttributes = desc.CustomAttributes().OfType<ApiVersionAttribute>()
                                .SelectMany(attr => attr.Versions);
                            var mappingAttributes = desc.CustomAttributes().OfType<MapToApiVersionAttribute>()
                                .SelectMany(attr => attr.Versions).ToArray();
                            return versionAttributes.Any(v => $"v{v}" == version)
                                   && (!mappingAttributes.Any() || mappingAttributes.Any(v => $"v{v}" == version));
                        });
                    c.EnableAnnotations();
                    //c.IncludeXmlComments(
                    //    $"{AppDomain.CurrentDomain.BaseDirectory}/ApiVersioningSample.xml");
                });
        }

        #endregion
    }
}