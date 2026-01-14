[CmdletBinding()]
param (
    [switch]
    $WhatIf
)

New-AzDeployment -Name 'deploy-cf-sample' `
    -Location 'westeurope' `
    -TemplateFile ./main.bicep `
    -TemplateParameterFile ./demo.bicepparam `
    -WhatIf:$WhatIf `
    -WhatIfResultFormat ResourceIdOnly