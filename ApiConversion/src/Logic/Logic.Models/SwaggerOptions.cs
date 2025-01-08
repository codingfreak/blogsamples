namespace codingfreaks.ApiConversion.Logic.Models
{
    using Asp.Versioning;

    public class SwaggerOptions
    {
        #region constructors and destructors

        private SwaggerOptions(string apiName, string description, string authClientId, string authInstance, string tenant, string[] authScopes, ApiVersion[] versions)
        {
            ApiName = apiName;
            AuthClientId = authClientId;
            AuthInstance = authInstance;
            AuthScopes = authScopes;
            Versions = versions;
            AuthTenant = tenant;
            Description = description;
        }

        #endregion

        #region methods

        public static SwaggerOptions GenerateDemo()
        {
            return new SwaggerOptions(
                "My API",
                "Just a sample for Youtube",
                "b3c9c391-6981-44e5-a2d7-8db105561655",
                "https://login.microsoftonline.com",
                "devdeer.com",
                [
                    "api://b3c9c391-6981-44e5-a2d7-8db105561655/full"
                ],
                [new ApiVersion(1, 0), new ApiVersion(2, 0)]);
        }

        #endregion

        #region properties

        public string ApiName { get; }

        public string Description { get; } = null!;

        public string AuthTenant { get; } = null!;

        public string AuthClientId { get; }

        public string AuthInstance { get; }

        public string[] AuthScopes { get; }

        public ApiVersion[] Versions { get; }

        #endregion
    }
}