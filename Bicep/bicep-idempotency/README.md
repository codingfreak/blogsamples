# BICEP Idempotency broken

## Video

<div align="left">
      <a href="https://youtu.be/MM5r7HCeiwk">
         <img src="https://img.youtube.com/vi/MM5r7HCeiwk/0.jpg" style="width:50%;">
      </a>
</div>

## Summary

This folder contains the sample to my [Youtube Video](https://youtu.be/MM5r7HCeiwk) about broken idempotency in Bicep. It shows an example which you can deploy 2 times in a row without changing anything on the files. The first deployment will fail while the second will succeed. This directly contradicts the claim from the [Bicep documentation](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview?tabs=bicep):

> **Repeatable results**: Repeatedly deploy your infrastructure throughout the development lifecycle and have confidence your resources are deployed in a consistent manner. Bicep files are idempotent, which means you can deploy the same file many times and get the same resource types in the same state. ...

## Try this out

1. Open a PowerShell command line in this directory.
2. Be sure to execute `Connect-AzAccount` properly and that `Get-AzContext` shows you the correct subscription id.
3. Exectute `New-azdeployment -Name bicep-demo -Location 'West Europe' -TemplateFile .\main.bicep -TemplateParameterFile .\main.bicepparam -WhatIf` to see the "simulation".
4. Execute `New-azdeployment -Name bicep-demo -Location 'West Europe' -TemplateFile .\main.bicep -TemplateParameterFile .\main.bicepparam` now -> you should some Bad Request errors.
5. Re-execute the last command which should result in a succeeded deployment -> that breaks idempotency!
