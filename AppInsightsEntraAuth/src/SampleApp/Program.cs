using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var otel = builder.Services.AddOpenTelemetry();
if (!builder.Environment.IsDevelopment())
{
    // we are running in Azure!
    otel.UseAzureMonitor(options =>
    {
        options.Credential = new ManagedIdentityCredential(new ManagedIdentityCredentialOptions());
    });
}
else
{
    // TODO USe Aspire dashboard as OTEL endpoint
    // otel.UseAzureMonitor(options =>
    // {
    //     options.Credential = new ManagedIdentityCredential(new ManagedIdentityCredentialOptions());
    // });
}
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();