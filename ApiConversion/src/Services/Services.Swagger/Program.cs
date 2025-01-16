using codingfreaks.ApiConversion.Logic.Interfaces;
using codingfreaks.ApiConversion.Logic.WeatherMock;
using codingfreaks.ApiConversion.Services.Swagger.Extensions;

using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var swaggerOptions = builder.Configuration.GetSwaggerOptions();
builder.Services.AddScoped<IWeatherService, MockWeatherService>();
builder.Services.AddControllers();
builder.Services.AddApiVersioningInternal(swaggerOptions);
builder.Services.AddSwaggerGenInternal(builder, swaggerOptions, builder.Configuration.GetIdentityOptions());
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUiInternal(swaggerOptions);
app.Run();