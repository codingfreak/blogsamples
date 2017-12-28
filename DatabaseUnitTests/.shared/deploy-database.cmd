cd ..\packages
nuget.exe install Microsoft.Data.Tools.Msbuild -ExcludeVersion -Source https://api.nuget.org/v3/index.json
cd Microsoft.Data.Tools.Msbuild*\lib\net46
sqlpackage.exe /a:publish /Profile:..\..\..\..\Data\Data.Database\TestSystemWithSeeding.publish.xml /TargetDatabaseName:unittest-%COMPUTERNAME% /Sourcefile:..\..\..\..\Data\Data.Database\bin\Output\Data.Database.dacpac
