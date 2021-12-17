# Be sure that Az Posh context is set correctly
New-AzResourceGroupDeployment -Name "blog-sample" `
	-ResourceGroupName 'rg-sample' `
	-TemplateFile "$PSScriptRoot\main.bicep" `
	-TemplateParameterFile "$PSScriptRoot\parameters.test.json" `
	-Location "westeurope"
