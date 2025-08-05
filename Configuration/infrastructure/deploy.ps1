[CmdLetBinding()]
param (
    [string]
    $Location = "westeurope",
    [switch]
    $WhatIf
)
$ctx = Get-AzContext
if ($null -eq $ctx) {
    throw 'No Az context found. Consider running Connect-AzAccount first!'
}
$dateSuffix = Get-Date -Format "yyyy-dd-MM-HH-mm"
$deployName = "codingfreaks-sample-$dateSuffix"
New-AzDeployment -Name $deployName -Location $Location -TemplateFile ./main.bicep -TemplateParameterFile ./params.bicepparam -WhatIf:$WhatIf -WhatIfResultFormat ResourceIdOnly