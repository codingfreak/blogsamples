[CmdletBinding()]
param (
	[Parameter()]	
	[string]	
	[ValidateSet("dev", "prod")]
	$Stage
)

$sourceFile = "settings.$Stage.json"
Copy-Item -Path $sourceFile -Destination "settings.json" -Force

New-AzResourceGroupDeployment -Name deploy `
	-ResourceGroupName 'rg-sample-test' `
	-TemplateFile .\main.bicep `
	-TemplateParameterFile .\parameters.json
	#-Verbose