[CmdletBinding()]
param (		
	[switch]
	$WhatIf
)

$tenantId = "18ca94d4-b294-485e-b973-27ef77addb3e"
$subscriptionId = "4407cff4-9b51-48b2-a256-c199b73b145b"
$templateFile = "main.bicep"
$parameterFile = "parameters.json"
$location = "West Europe"

# ensure AZ context
$currentContext = Get-AzContext
if ($currentContext.Subscription.Id -ne $subscriptionId -or $currentContext.Tenant.Id -ne $tenantId) {
	# current Az context is wrong
	Set-AzContext -Tenant $tenantId -Subscription $subscriptionId
}
else {
	# current Az context is fine
	Write-Host "Reusing subscription context for $subscriptionId."
}
if (!$?) {
	# something went wrong on of the commands before
	throw 'Could not set subscription scope. Maybe running Connect-AzAccount can fix the problem?'
}

$dateSuffix = Get-Date -Format "yyyy-dd-MM-HH-mm"
$deploymentName = "deploy-$dateSuffix"

New-AzDeployment `
	-Name $deploymentName `
	-Location $location `
	-TemplateFile $templateFile `
	-TemplateParameterFile $parameterFile `
	-DeploymentDebugLogLevel None `
	-WhatIf:$WhatIf