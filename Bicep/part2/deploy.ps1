# New-AzResourceGroup -Name rg-sample-test `
# 	-Location westeurope `
# 	-Tag @{ "purpose" = "test" }
New-AzResourceGroupDeployment -Name deploy `
	-ResourceGroupName 'rg-sample-test' `
	-TemplateFile .\main.bicep `
	-TemplateParameterFile .\parameters.json `
	-Verbose	