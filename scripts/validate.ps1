$ErrorActionPreference = 'Stop'

$repositoryRoot = Split-Path -Parent $PSScriptRoot
Push-Location $repositoryRoot
try {
    $projects = @(
        'src/WebKit.Core/WebKit.Core.csproj',
        'src/WebKit.Design/WebKit.Design.csproj',
        'src/WebKit.Web/WebKit.Web.csproj',
        'src/WebKit.UI/WebKit.UI.csproj',
        'samples/WebKit.ExampleApp/WebKit.ExampleApp.csproj',
        'tests/WebKit.Tests/WebKit.Tests.csproj'
    )

    foreach ($project in $projects) {
        dotnet restore $project
        if ($LASTEXITCODE -ne 0) {
            throw "Restore failed for $project."
        }
    }

    dotnet build WebKit.sln -m:1 --no-restore
    if ($LASTEXITCODE -ne 0) {
        throw 'Solution build failed.'
    }

    dotnet run --project tests/WebKit.Tests/WebKit.Tests.csproj --no-build --no-restore
    if ($LASTEXITCODE -ne 0) {
        throw 'Test runner failed.'
    }
}
finally {
    Pop-Location
}
