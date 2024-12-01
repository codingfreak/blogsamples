# Checks all used resource providers in /infrastructure/modules or updates.
# Exit code 0 means that no updates where found.
# Exit code 1 means that at least 1 update was found.
# If switch $AllowPrerelease is provided the script will always look for the newest version even if it is a preview.

[CmdLetBinding()]
param (
    [switch]
    $AllowPrerelease
)
$files = Get-ChildItem -Path . -Filter *.bicep -Recurse
$filesLength = ($files | Measure-Object).Count
$pathLength = $PSScriptRoot.Length
Write-Host "Found $filesLength Bicep modules. Starting dependency check..."
$regexPattern = "'([^']+)@([\w-]+)'"
$providers = @()
$i = 0
$updates = 0
foreach ($file in $files) {
    $i++
    $complete = [Math]::Round($i * 100 / $filesLength)
    $relativeFile = $file.FullName.Substring($pathLength + 1)
    Write-Progress -Activity 'Dependency check' -CurrentOperation $file -Status "($i of $filesLength -> $complete%) $relativeFile" -PercentComplete $complete
    $content = Get-Content -Raw $file
    $hits = [regex]::Matches($content, $regexPattern)
    foreach ($match in $hits) {
        $provider = $match.Groups[1].Value
        $version = $match.Groups[2].Value
        $firstSlash = $provider.IndexOf('/')
        $ns = $provider.Substring(0, $firstSlash)
        $type = $provider.Substring($firstSlash + 1)
        $typeInfo = Get-AzResourceProvider -ProviderNamespace $ns | Select-Object -ExpandProperty ResourceTypes | Where-Object ResourceTypeName -eq $type
        $versions = $typeInfo.ApiVersions
        if ($null -eq $versions) {
            continue
        }
        if (!$version.ToLower().Contains('preview') -and !$AllowPrerelease.IsPresent) {
            # remove preview versions
            $versions = $versions | Where-Object { $_.ToLower().Contains('preview') -eq $false }
        }
        if ($versions) {
            $currentPos = $versions.IndexOf($version)
            $updates += $currentPos -gt 0 ? 1 : 0
            $providers += New-Object PSObject -Property ([ordered]@{
                    'Module'      = $relativeFile
                    'Provider'    = $provider
                    'Local'       = $version
                    'Newest'      = $versions[0]
                    'NeedsUpdate' = $currentPos -gt 0
                })
        }
    }
}
Write-Host "Found $($updates) available upgrades:"
$providers | Format-Table
exit $updates -gt 0 ? 1 : 0