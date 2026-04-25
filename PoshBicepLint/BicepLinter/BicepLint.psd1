@{
    # Module identity
    ModuleVersion     = '1.0.0'
    GUID              = 'A4D1CA0B-6F9A-4DF0-BDE4-D2C5991FB578' 
    Author            = 'codingfreaks'
    Description       = 'Bicep linting cmdlets'

    # The compiled assembly
    RootModule        = 'BicepLint.dll'

    # Formatting and type files
    FormatsToProcess  = @('BicepLint.Format.ps1xml')
    TypesToProcess    = @('BicepLint.Types.ps1xml')

    # Only export what you intend to be public
    CmdletsToExport   = @('Start-BicepLint')
    FunctionsToExport = @()
    AliasesToExport   = @()

    # Minimum PowerShell version
    PowerShellVersion = '7.0'
}