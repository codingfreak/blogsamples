using codingfreaks.ApiConversion.Logic.Interfaces;
using codingfreaks.ApiConversion.Logic.WeatherMock;
using codingfreaks.ApiConversion.Services.OpenApi.Extensions;

using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
var openApiOptions = builder.Configuration.GetOpenApiOptions();
var identityOptions = builder.Configuration.GetIdentityOptions();
builder.Services.AddScoped<IWeatherService, MockWeatherService>();
builder.Services.AddOpenApiInternal(openApiOptions, identityOptions);
builder.Services.AddApiVersioningInternal(openApiOptions);
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUiInternal(openApiOptions);
app.Run();