function Get-AccessToken {
    [CmdLetBinding()]
    param (
        [string]
        $TenantId = '',
        [string]
        $ResourceUrl = '',
        [switch]
        $AsSecureString
    )
    if ($ResourceUrl.Length -gt 0) {
        $token = Get-AzAccessToken -ResourceUrl $ResourceUrl -WarningAction SilentlyContinue -AsSecureString
    }
    else {
        $token = Get-AzAccessToken -WarningAction SilentlyContinue -AsSecureString
    }
    if (!$accessToken) {

    }
    if ($AsSecureString.IsPresent) {
        # return the secure token as it is
        return $token.Token
    }
    $accessToken = ConvertFrom-SecureString -SecureString $token.Token -AsPlainText
    return $accessToken
}

function Get-Constants {
    # Parameter help description
    [CmdletBinding()]
    param (
    )
    process {
        return @{
            AzureDevOps  = @{
                BaseUrl    = "https://dev.azure.com"
                TokenUrl   = "499b84ac-1321-427f-aa17-267ca6975798"
                ApiVersion = "7.1"
            }
            VisualStudio = @{
                BaseUrl    = "https://app.vssps.visualstudio.com"
                TokenUrl   = "499b84ac-1321-427f-aa17-267ca6975798"
                ApiVersion = "7.1"
            }
        }
    }
}

function Invoke-AzureDevOpsRestApi {
    # Parameter help description
    [CmdletBinding()]
    param (
        [string]
        $OrganizationName = '',
        [string]
        $ProjectName = '',
        [string]
        $RelativeUrl,
        [string]
        $ContentType = '',
        [string]
        [ValidateSet('GET', 'POST', 'PUT', 'DELETE', 'PATCH')]
        $Method,
        [object]
        $Body = $null,
        [string]
        [ValidateSet('VisualStudio', 'AzureDevOps')]
        $ApiType = 'AzureDevOps',
        [Hashtable]
        $AdditionalParameters
    )
    process {
        $constants = Get-Constants
        $api = $ApiType -eq 'VisualStudio' ? $constants.VisualStudio : $constants.AzureDevOps
        $baseUrl = $api.BaseUrl
        $apiVersion = $api.ApiVersion
        $uri = "$baseUrl/"
        if ($OrganizationName.Length -gt 0) {
            $uri += "$OrganizationName/"
        }
        if ($ProjectName.Length -gt 0) {
            $uri += "$ProjectName/"
        }
        $uri += "_apis/$($RelativeUrl)?api-version=$apiVersion"
        if ($null -ne $AdditionalParameters -and $AdditionalParameters.Count -gt 0) {
            foreach ($key in $AdditionalParameters.Keys) {
                $uri += "&$($key)=$($AdditionalParameters[$key])"
            }
        }
        $accessToken = Get-AccessToken -ResourceUrl $api.TokenUrl
        $headers = @{
            Accept        = "application/json"
            Authorization = "Bearer $accessToken"
        }
        try {
            Write-Verbose $uri
            $response = Invoke-RestMethod -Method $Method -Uri $uri -Headers $headers -Body $Body -ContentType $ContentType
            return $response
        }
        catch {
            Write-Host $_
            return $null
        }
    }
}

$tenantId = ''
$servicePrincipalClientId = ''
$serviceConnectionName = ''
$subscriptionName = ''
$subscriptionId = ''
$orgName = ''
$projectName = ''

# get the project id
$projectResponse = Invoke-AzureDevOpsRestApi -OrganizationName $orgName `
    -RelativeUrl 'projects' `
    -Method GET

$project = $projectResponse.value | Where-Object { $_.name -eq $projectName }[0]
$projectId = $project.id
# get the object id of the SP
$servicePrincipalObjectId = (Get-AzADApplication -ApplicationId $servicePrincipalClientId).Id
# generate the service connection
$body = @{
    name                             = $serviceConnectionName
    type                             = 'AzureRM'
    url                              = 'https://management.azure.com/'
    isReady                          = $true
    data                             = @{
        creationMode     = 'Manual'
        environment      = 'AzureCloud'
        scopeLevel       = 'Subscription'
        subscriptionName = $subscriptionName
        subscriptionId   = $subscriptionId
    }
    serviceEndpointProjectReferences = @(
        @{
            name             = $serviceConnectionName
            projectReference = @{
                id   = $projectId
                name = $projectName
            }
        }
    )
    authorization                    = @{
        scheme     = 'WorkloadIdentityFederation'
        parameters = @{
            tenantId           = $tenantId
            servicePrincipalId = $servicePrincipalObjectId
        }
    }
}
$json = $body | ConvertTo-Json -Depth 10
$scResult = Invoke-AzureDevOpsRestApi -OrganizationName $orgName `
    -ProjectName $projectName `
    -RelativeUrl "serviceendpoint/endpoints" `
    -Method POST `
    -ContentType 'application/json' `
    -Body $json
$issuer = $scResult.authorization.parameters.workloadIdentityFederationIssuer
$subject = $scResult.authorization.parameters.workloadIdentityFederationSubject
# create federation on existing service principal
New-AzADAppFederatedCredential -ApplicationObjectId $servicePrincipalObjectId `
    -Audience 'api://AzureADTokenExchange' `
    -Name "devops-$orgName-$projectName" `
    -Issuer $issuer `
    -Subject $subject

