using System;
using System.Linq;

namespace ApiVersioningSample.Filters
{
    using System;
    using System.Linq;

    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Is used by Swashbuckle to remove the parameter named 'version' from the routes.
    /// </summary>
    public class RemoveVersionParameterFilter : IOperationFilter
    {
        #region explicit interfaces

        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }

        #endregion
    }
}