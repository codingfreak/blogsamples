namespace SampleApi
{
    using Azure.Identity;

    using Logic;

    using Microsoft.Extensions.Configuration.AzureAppConfiguration;

    using Models;

    using Scalar.AspNetCore;

    public class Program
    {
        #region methods

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            if (!builder.Environment.IsDevelopment())
            {
                // we probably run in Azure
                //UseKeyVault(builder);
                UseAzureAppConfig(builder);
            }
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<SampleLogic>();
            builder.Services.AddOptions<MyAppOptions>()
                .Bind(builder.Configuration.GetSection(MyAppOptions.ConfigKey));
            builder.Services.AddAzureAppConfiguration();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseAzureAppConfiguration();
            app.Run();
        }

        private static void UseAzureAppConfig(WebApplicationBuilder builder)
        {
            builder.Configuration.AddAzureAppConfiguration(appConfigOptions =>
            {
                appConfigOptions.Connect(
                    new Uri(
                        builder.Configuration["AppConfiguration:Endpoint"]
                        ?? throw new ApplicationException("App config not configured.")),
                    new ManagedIdentityCredential());
                appConfigOptions.Select(KeyFilter.Any);
                appConfigOptions.Select(KeyFilter.Any, $"Environment:{builder.Environment.EnvironmentName}");
                appConfigOptions.ConfigureRefresh(refreshOpt =>
                {
                    refreshOpt.Register("Sentinel", true)
                        .SetRefreshInterval(TimeSpan.FromSeconds(30));
                });
            });
        }

        private static void UseKeyVault(WebApplicationBuilder builder)
        {
            builder.Configuration.AddAzureKeyVault(
                new Uri(
                    builder.Configuration["AppConfiguration:KeyVaultUri"]
                    ?? throw new ApplicationException("Key vault not configured.")),
                new ManagedIdentityCredential());
        }

        #endregion
    }
}