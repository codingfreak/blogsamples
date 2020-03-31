namespace ApiVersioningSample
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Provides the entry point of the app.
    /// </summary>
    public class Program
    {
        #region methods

        /// <summary>
        /// Retrieves the configured host builder using the logic in <see cref="Startup"/>.
        /// </summary>
        /// <param name="args">Optional command line arguments.</param>
        /// <returns>The confgured host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        /// <summary>
        /// The main entry point of the app.
        /// </summary>
        /// <param name="args">Optional command line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion
    }
}