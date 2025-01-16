namespace codingfreaks.ApiConversion.Logic.Models
{
    using Asp.Versioning;

    public class SwaggerOptions
    {
        #region properties

        public string ApiName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int[] Versions { get; set; } = null!;

        public ApiVersion[] ApiVersions =>
            Versions.Select(v => new ApiVersion(v, 0))
                .ToArray();

        public string Contact { get; set; } = null!;

        #endregion
    }
}