using codingfreaks.ApiConversion.Logic.Interfaces;
using codingfreaks.ApiConversion.Logic.WeatherMock;
using codingfreaks.ApiConversion.Services.OpenApi.Extensions;

using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
var openApiOptions = builder.Configuration.GetOpenApiOptions();
var identityOptions = builder.Configuration.GetIdentityOptions();
builder.Services.AddScoped<IWeatherService, MockWeatherService>();
builder.Services.AddControllers();
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddApiVersioningInternal(openApiOptions);
builder.Services.AddOpenApiInternal(openApiOptions, identityOptions);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUiInternal(openApiOptions, identityOptions);
app.Run();