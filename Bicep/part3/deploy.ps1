# Be sure that Az Posh context is set correctly
New-AzDeployment -Name "blog-sample" `
	-TemplateFile "$PSScriptRoot\main.bicep" `
	-Location "westeurope"
