#dotnet build -c release
$packageName = (gci .\bin\release\ArkServer.Entities.0.0.2.nupkg | select FullName).FullName
dotnet nuget push "$packageName" --api-key "$env:NUGET_API_KEY" --source https://api.nuget.org/v3/index.json