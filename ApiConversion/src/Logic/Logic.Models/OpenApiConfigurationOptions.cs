namespace codingfreaks.ApiConversion.Logic.Models
{
    using Asp.Versioning;

    /// <summary>
    /// Is used to represent config options for OpenApi.
    /// </summary>
    public class OpenApiConfigurationOptions
    {
        #region properties

        /// <summary>
        /// The name of the API to be displayed.
        /// </summary>
        public string ApiName { get; set; } = null!;

        /// <summary>
        /// Some text describing the purpose of the API.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// A list of major API version numbers supported by this API.
        /// </summary>
        public int[] Versions { get; set; } = null!;

        /// <summary>
        /// The <see cref="Versions" /> converted to the explicit OpenAPI version type.
        /// </summary>
        public ApiVersion[] ApiVersions =>
            Versions.Select(v => new ApiVersion(v, 0))
                .ToArray();

        /// <summary>
        /// The name of the company delivering this API.
        /// </summary>
        public string Contact { get; set; } = null!;

        #endregion
    }
}