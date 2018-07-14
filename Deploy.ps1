$scriptPath = Split-Path -Parent $PSCommandPath
$deployPath = Join-Path -Path $scriptPath -ChildPath ".deploy"
$packagesPath = Join-Path -Path $scriptPath -ChildPath ".deploy\packages"

Write-Host "Clearing deploy folder" | Remove-Item -Recurse -Force $deployPath

Write-Host "Testing" | dotnet test test/Unmanaged.Tests/Unmanaged.Tests.csproj -c Release

Write-Host "Deploying nuget packages" | dotnet pack -o $packagesPath