using codingfreaks.ApiConversion.Logic.Interfaces;
using codingfreaks.ApiConversion.Logic.Models;
using codingfreaks.ApiConversion.Logic.WeatherMock;
using codingfreaks.ApiConversion.Services.Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);
var options = SwaggerOptions.GenerateDemo();
// Add services to the container.
builder.Services.AddScoped<IWeatherService, MockWeatherService>();
builder.Services.AddControllers();
//builder.Services.AddApiVersioningInternal(options);
builder.Services.AddSwaggerGenInternal(builder, options);
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUiInternal(options);
app.Run();