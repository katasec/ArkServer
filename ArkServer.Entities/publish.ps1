
$csproj = [xml]([string](Get-Content ArkServer.Entities.csproj))
$version = $csproj.Project.PropertyGroup.Version

dotnet build -c release
$packageName = (Get-ChildItem ".\bin\release\ArkServer.Entities.$version.nupkg" | Select-Object FullName).FullName
dotnet nuget push "$packageName" --api-key "$env:NUGET_API_KEY" --source https://api.nuget.org/v3/index.json