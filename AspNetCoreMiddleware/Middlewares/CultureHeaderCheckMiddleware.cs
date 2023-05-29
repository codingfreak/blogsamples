namespace AspNetCoreMiddleware.Middlewares
{
    using System.Globalization;

    using Attributes;

    using Microsoft.AspNetCore.Http.Features;

    public class CultureHeaderCheckMiddleware : IMiddleware
    {
        #region explicit interfaces

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var endpointFeature = context.Features.Get<IEndpointFeature>()
                ?.Endpoint;
            var attribute = endpointFeature?.Metadata.GetMetadata<CheckCultureAttribute>();
            if (attribute == null)
            {
                // there is no attribute for culture check present in the current context -> proceed
                await next(context);
                return;
            }
            var culture = context.Request.Headers["Culture"].FirstOrDefault();
            if (culture == null)
            {
                // no culture header present
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Culture header is missing");
                return;
            }
            try
            {
                var clientCulture = new CultureInfo(culture);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid culture provided");
                return;
            }
            await next(context);
        }

        #endregion
    }
}