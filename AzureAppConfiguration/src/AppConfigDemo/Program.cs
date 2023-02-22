using System.Reflection;

using Azure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = new ConfigurationBuilder();
var appName = Assembly.GetEntryAssembly()
    ?.GetName()
    .Name ?? throw new ApplicationException("Could not read assembly name.");
var connectionString = Environment.GetEnvironmentVariable("APPCONFIG_CONNECTIONSTRING") ?? throw new ApplicationException("No connection string defined.");
var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? throw new ApplicationException("No environment defined.");
builder.AddAzureAppConfiguration(
    options =>
    {
        options.Connect(connectionString)
            //.ConfigureKeyVault(kvc => kvc.SetCredential(new DefaultAzureCredential()))
            //.Select($"{appName}:*", environment);
            .Select(KeyFilter.Any, environment);
    });
var config = builder.Build();
//var configModel = new ConfigModel();
//config.Bind("Complex", configModel);
Console.WriteLine(config["DemoValue"]);
//Console.WriteLine(config["OtherSample"]);
//Console.WriteLine(config["Secret"]);

internal class ConfigModel
{
    #region properties

    public string? Foo { get; set; }

    public int Bar { get; set; }

    #endregion
}