nuget install devdeer.Templates.Bicep -Source nuget.org
if (Test-Path -Path .\modules) {
	rm .\modules -Recurse
}
mv .\devdeer.Templates.Bicep*\modules . -Force

if (Test-Path -Path .\components) {
	rm .\components -Recurse
}
mv .\devdeer.Templates.Bicep*\components . -Force
rm .\devdeer.Templates.Bicep* -Recurse
