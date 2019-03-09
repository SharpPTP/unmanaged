$scriptPath = Split-Path -Parent $PSCommandPath
$deployPath = Join-Path -Path $scriptPath -ChildPath ".deploy"

Write-Host "Clearing deploy folder: $deployPath"
Remove-Item $deployPath -Recurse -Force

Write-Host "Testing unmanaged"
dotnet test test/Unmanaged.Tests/Unmanaged.Tests.csproj -c Release

Write-Host "Testing unmanaged MSTests"
dotnet test test/Unmanaged.MSTest.Tests/Unmanaged.MSTest.Tests.csproj -c Release

Write-Host "Deploying packages"
dotnet pack -o $deployPath