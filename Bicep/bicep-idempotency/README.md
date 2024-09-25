# BICEP Idempotency broken

## Video

<div align="left">
      <a href="https://youtu.be/MM5r7HCeiwk">
         <img src="https://img.youtube.com/vi/MM5r7HCeiwk/0.jpg" style="width:50%;">
      </a>
</div>

## Summary

This

## Try this out

1. Open a PowerShell command line in this directory.
2. Be sure to execute `Connect-AzAccount` properly and that `Get-AzContext` shows you the correct subscription id.
3. Exectute `New-azdeployment -Name bicep-demo -Location 'West Europe' -TemplateFile .\main.bicep -TemplateParameterFile .\main.bicepparam -WhatIf` to see the "simulation".
4. Execute `New-azdeployment -Name bicep-demo -Location 'West Europe' -TemplateFile .\main.bicep -TemplateParameterFile .\main.bicepparam` now -> you should some Bad Request errors.
5. Re-execute the last command which should result in a succeeded deployment -> that breaks idempotency!
