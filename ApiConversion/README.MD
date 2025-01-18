# OpenAPI with .NET 8 and/or .NET 9

## Summary

This represents the sources for the video series on .NET 9 updates regarding OpenAPI generation.

![Part 1](https://youtu.be/m4qAZifm42o)

## Using the sample

After cloning this to your local machine you need

1) Create an app registration in Entra ID:
    - Redirect URI: `https://localhost:44301/swagger/oauth2-redirect.html`
    - Expose an API with the scope "full".
    - Add API permissions to:
        - Microsoft Graph: `User.Read`.
        - Your Application: `full`
        - Grant admin constent on those permissions

   (The reason you need to consent to yourself is that the Swagger UI will be like a client "outside" calling your Swagger.)

2) Execute the following commands in the directory where the sln file is resided:

    ```powershell
    # OpenAPI API
    dotnet user-secrets set AzureAd:TenantId GUID_OF_YOUR_ENTRA_TENANT --project src/Services/Services.OpenApi
    dotnet user-secrets set AzureAd:Scope:0 APP_REGISTRATION_SCOPE --project src/Services/Services.OpenApi
    dotnet user-secrets set AzureAd:ClientID CLIENT_ID --project src/Services/Services.OpenApi
    # Swagger API
    dotnet user-secrets set AzureAd:TenantId GUID_OF_YOUR_ENTRA_TENANT --project src/Services/Services.Swagger
    dotnet user-secrets set AzureAd:Scope:0 APP_REGISTRATION_SCOPE --project src/Services/Services.Swagger
    dotnet user-secrets set AzureAd:ClientID CLIENT_ID --project src/Services/Services.Swagger
    ```

    The APP_REGISTRATION_SCOPE can be obtained in Azure Portal on the app registration by going to `API Permissions` and clicking on the formerly created `full` scope. In the new window you can copy it out from the first entry.