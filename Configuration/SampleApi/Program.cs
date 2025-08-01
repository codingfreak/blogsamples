
namespace SampleApi
{
    using Scalar.AspNetCore;

    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddTransient<Logic>();
            builder.Services.AddOptions<MyAppOptions>()
                .Bind(builder.Configuration.GetSection(MyAppOptions.ConfigKey));
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
