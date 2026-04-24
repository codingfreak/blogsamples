using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddOpenTelemetry()
    .UseAzureMonitor(options =>
    {
        if (builder.Environment.IsProduction())
        {
            options.Credential = new ManagedIdentityCredential(new ManagedIdentityCredentialOptions());
        }
        else
        {
            // DON'T DO THIS BECAUSE IT ONLY WORKS IN VS!!!
            //options.Credential = new DefaultAzureCredential();
        }
    });
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();