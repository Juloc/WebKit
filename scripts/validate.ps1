$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$artifactPath = Join-Path $repositoryRoot 'artifacts'
$temporaryRoot = Join-Path ([System.IO.Path]::GetTempPath()) "webkit-validation-$PID"
$previousLocation = Get-Location
$previousCliHome = $env:DOTNET_CLI_HOME
$previousAspNetEnvironment = $env:ASPNETCORE_ENVIRONMENT

function Invoke-Dotnet {
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [string[]] $Arguments
    )

    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet $($Arguments -join ' ') failed with exit code $LASTEXITCODE."
    }
}

function Assert-True {
    param(
        [Parameter(Mandatory = $true)] [bool] $Condition,
        [Parameter(Mandatory = $true)] [string] $Message
    )

    if (-not $Condition) {
        throw $Message
    }
}

function Assert-PackageEntry {
    param(
        [Parameter(Mandatory = $true)] $Archive,
        [Parameter(Mandatory = $true)] [string] $Pattern,
        [Parameter(Mandatory = $true)] [string] $PackageName
    )

    $entry = $Archive.Entries | Where-Object { $_.FullName -like $Pattern } | Select-Object -First 1
    Assert-True ($null -ne $entry) "Package $PackageName does not contain an entry matching $Pattern."
}

try {
    Set-Location $repositoryRoot
    New-Item -ItemType Directory -Path $artifactPath -Force | Out-Null
    Remove-Item -LiteralPath $temporaryRoot -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType Directory -Path $temporaryRoot | Out-Null
    $env:DOTNET_CLI_HOME = Join-Path $temporaryRoot 'dotnet-home'
    New-Item -ItemType Directory -Path $env:DOTNET_CLI_HOME | Out-Null

    $projects = @(
        'src/WebKit.Core/WebKit.Core.csproj',
        'src/WebKit.Design/WebKit.Design.csproj',
        'src/WebKit.Web/WebKit.Web.csproj',
        'src/WebKit.UI/WebKit.UI.csproj',
        'samples/WebKit.ExampleApp/WebKit.ExampleApp.csproj',
        'tests/WebKit.Tests/WebKit.Tests.csproj'
    )

    foreach ($project in $projects) {
        Invoke-Dotnet @('restore', $project, '-p:RestoreDisableParallel=true', '-m:1')
    }

    Invoke-Dotnet @('build', 'WebKit.sln', '-c', 'Release', '--no-restore', '-m:1')
    Invoke-Dotnet @('run', '--project', 'tests/WebKit.Tests/WebKit.Tests.csproj', '-c', 'Release', '--no-build', '--no-restore')

    Remove-Item -LiteralPath (Join-Path $artifactPath '*.nupkg') -Force -ErrorAction SilentlyContinue
    foreach ($project in @('src/WebKit.Core/WebKit.Core.csproj', 'src/WebKit.Design/WebKit.Design.csproj', 'src/WebKit.Web/WebKit.Web.csproj', 'src/WebKit.UI/WebKit.UI.csproj')) {
        Invoke-Dotnet @('pack', $project, '-c', 'Release', '--no-restore', '-m:1', '-o', $artifactPath)
    }

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $packages = @(Get-ChildItem -LiteralPath $artifactPath -Filter '*.nupkg' -File)
    Assert-True ($packages.Count -eq 4) "Expected four WebKit packages, got $($packages.Count)."
    foreach ($package in $packages) {
        $archive = [System.IO.Compression.ZipFile]::OpenRead($package.FullName)
        try {
            Assert-PackageEntry $archive '*.nuspec' $package.Name
            Assert-PackageEntry $archive 'lib/net10.0/*.dll' $package.Name
            if ($package.BaseName -like '*Design*') {
                Assert-PackageEntry $archive 'staticwebassets/*webkit-design.css' $package.Name
            }
            if ($package.BaseName -like '*UI*') {
                Assert-PackageEntry $archive 'staticwebassets/*webkit-ui.css' $package.Name
                Assert-PackageEntry $archive 'staticwebassets/*webkit-ui.js' $package.Name
            }
        }
        finally {
            $archive.Dispose()
        }
    }

    $designCssPath = Join-Path $repositoryRoot 'src/WebKit.Design/wwwroot/css/webkit-design.css'
    $uiCssPath = Join-Path $repositoryRoot 'src/WebKit.UI/wwwroot/css/webkit-ui.css'
    $uiJsPath = Join-Path $repositoryRoot 'src/WebKit.UI/wwwroot/js/webkit-ui.js'
    $designCss = [System.IO.File]::ReadAllText($designCssPath)
    $uiCss = [System.IO.File]::ReadAllText($uiCssPath)
    $uiJs = [System.IO.File]::ReadAllText($uiJsPath)
    $dialogPartial = [System.IO.File]::ReadAllText((Join-Path $repositoryRoot 'src/WebKit.UI/Pages/Shared/_ConfirmDialog.cshtml'))
    $toastPartial = [System.IO.File]::ReadAllText((Join-Path $repositoryRoot 'src/WebKit.UI/Pages/Shared/_Toasts.cshtml'))
    Assert-True ($designCss.Contains(':focus-visible') -and $designCss.Contains('prefers-reduced-motion') -and $designCss.Contains('@media (max-width')) 'Design accessibility/responsive contract audit failed.'
    Assert-True ($dialogPartial.Contains('AntiForgeryToken') -and $dialogPartial.Contains('aria-labelledby')) 'Dialog security/accessibility contract audit failed.'
    Assert-True ($toastPartial.Contains('aria-live') -and $toastPartial.Contains('role=')) 'Toast live-region contract audit failed.'
    Assert-True ($designCss.Contains('.wk-button') -and $designCss.Contains('.wk-form') -and $designCss.Contains('.wk-dialog') -and $designCss.Contains('.wk-toast') -and $uiCss.Contains('.wk-brand')) 'Shared UI primitive contract audit failed.'
    Assert-True ($uiJs.Length -lt 20000 -and -not $uiJs.Contains('fetch(')) 'Global UI JavaScript performance contract audit failed.'
    Write-Output 'Accessibility/responsive/performance contract audit passed.'

    $templateDirectories = @(
        'templates/webkit-app',
        'templates/webkit-feature',
        'templates/webkit-page',
        'templates/webkit-list-page',
        'templates/webkit-detail-page',
        'templates/webkit-form-page',
        'templates/webkit-settings-page',
        'templates/webkit-admin-page'
    )
    foreach ($templateDirectory in $templateDirectories) {
        Invoke-Dotnet @('new', 'install', (Join-Path $repositoryRoot $templateDirectory), '--force')
    }

    $externalApp = Join-Path $temporaryRoot 'ExternalApp'
    Invoke-Dotnet @('new', 'webkit-app', '-n', 'ExternalApp', '-o', $externalApp, '--force')
    $externalProject = Join-Path $externalApp 'ExternalApp.csproj'
    Invoke-Dotnet @('restore', $externalProject, '--source', $artifactPath, '--ignore-failed-sources', '-p:RestoreDisableParallel=true', '-m:1')
    $externalProjectText = [System.IO.File]::ReadAllText($externalProject)
    Assert-True (-not $externalProjectText.Contains('<ProjectReference')) 'External app contains an unexpected source project reference.'
    foreach ($packageId in @('Juloc.WebKit.Web', 'Juloc.WebKit.UI', 'Juloc.WebKit.Design')) {
        $packageReference = 'PackageReference Include="' + $packageId + '"'
        Assert-True ($externalProjectText.Contains($packageReference)) "External app does not reference package $packageId."
    }
    Invoke-Dotnet @('build', $externalProject, '--no-restore', '-m:1')

    foreach ($templateInvocation in @(
        @('webkit-feature', '-n', 'Billing', '-o', (Join-Path $externalApp 'Features/Billing'), '--force'),
        @('webkit-page', '-n', 'Reports', '-o', (Join-Path $externalApp 'Pages/Reports'), '--force'),
        @('webkit-list-page', '-n', 'Providers', '-o', (Join-Path $externalApp 'Pages/Providers'), '--force'),
        @('webkit-detail-page', '-n', 'ProviderDetails', '-o', (Join-Path $externalApp 'Pages/ProviderDetails'), '--force'),
        @('webkit-form-page', '-n', 'ProviderEdit', '-o', (Join-Path $externalApp 'Pages/ProviderEdit'), '--force'),
        @('webkit-settings-page', '-n', 'Preferences', '-o', (Join-Path $externalApp 'Pages/Preferences'), '--force'),
        @('webkit-admin-page', '-n', 'Settings', '-o', (Join-Path $externalApp 'Pages/Settings'), '--force')
    )) {
        $arguments = @('new') + $templateInvocation
        Invoke-Dotnet $arguments
        Invoke-Dotnet @('build', $externalProject, '--no-restore', '-m:1')
    }

    $expectedFiles = @(
        'Features/Billing/BillingFeature.cs',
        'Pages/Reports/Reports.cshtml',
        'Pages/Providers/Providers.cshtml',
        'Pages/ProviderDetails/ProviderDetails.cshtml',
        'Pages/ProviderEdit/ProviderEdit.cshtml',
        'Pages/Preferences/Preferences.cshtml',
        'Pages/Settings/Settings.cshtml'
    )
    foreach ($relativePath in $expectedFiles) {
        Assert-True (Test-Path -LiteralPath (Join-Path $externalApp $relativePath)) "Template did not generate $relativePath."
    }

    $url = 'http://127.0.0.1:5137'
    $env:ASPNETCORE_ENVIRONMENT = 'Development'
    $stdout = Join-Path $temporaryRoot 'external.stdout.log'
    $stderr = Join-Path $temporaryRoot 'external.stderr.log'
    $process = Start-Process -FilePath 'dotnet' -ArgumentList @('run', '--project', $externalProject, '--no-build', '--no-restore', '--urls', $url) -WorkingDirectory $externalApp -RedirectStandardOutput $stdout -RedirectStandardError $stderr -PassThru
    try {
        $ready = $false
        for ($attempt = 0; $attempt -lt 40; $attempt++) {
            Start-Sleep -Milliseconds 500
            if ($process.HasExited) {
                throw "External app exited early. $([System.IO.File]::ReadAllText($stderr))"
            }

            try {
                $response = Invoke-WebRequest -Uri "$url/" -UseBasicParsing -TimeoutSec 2
                if ($response.StatusCode -eq 200) {
                    $ready = $true
                    break
                }
            }
            catch {
                # The process may still be starting.
            }
        }

        Assert-True $ready 'External app did not become ready.'
        $css = Invoke-WebRequest -Uri "$url/_content/Juloc.WebKit.Design/css/webkit-design.css" -UseBasicParsing
        $js = Invoke-WebRequest -Uri "$url/_content/Juloc.WebKit.UI/js/webkit-ui.js" -UseBasicParsing
        Assert-True ($css.StatusCode -eq 200 -and $css.Content.Contains('--wk-color-surface')) 'Design static asset smoke failed.'
        Assert-True ($js.StatusCode -eq 200 -and $js.Content.Contains('theme-preset')) 'UI static asset smoke failed.'
        Write-Output 'External package/template/HTTP/static-asset smoke passed.'
    }
    finally {
        if (-not $process.HasExited) {
            Stop-Process -Id $process.Id -Force
        }
    }

    $sampleUrl = 'http://127.0.0.1:5138'
    $sampleProject = Join-Path $repositoryRoot 'samples/WebKit.ExampleApp/WebKit.ExampleApp.csproj'
    $sampleStdout = Join-Path $temporaryRoot 'sample.stdout.log'
    $sampleStderr = Join-Path $temporaryRoot 'sample.stderr.log'
    $sampleProcess = Start-Process -FilePath 'dotnet' -ArgumentList @('run', '--project', $sampleProject, '-c', 'Release', '--no-build', '--no-restore', '--urls', $sampleUrl) -WorkingDirectory $repositoryRoot -RedirectStandardOutput $sampleStdout -RedirectStandardError $sampleStderr -PassThru
    try {
        $sampleReady = $false
        for ($attempt = 0; $attempt -lt 40; $attempt++) {
            Start-Sleep -Milliseconds 500
            if ($sampleProcess.HasExited) {
                throw "Example app exited early. $([System.IO.File]::ReadAllText($sampleStderr))"
            }

            try {
                $response = Invoke-WebRequest -Uri "$sampleUrl/" -UseBasicParsing -TimeoutSec 2
                if ($response.StatusCode -eq 200) {
                    $sampleReady = $true
                    break
                }
            }
            catch {
                # The process may still be starting.
            }
        }

        Assert-True $sampleReady 'Example app did not become ready.'
        $sampleCss = Invoke-WebRequest -Uri "$sampleUrl/_content/Juloc.WebKit.Design/css/webkit-design.css" -UseBasicParsing
        Assert-True ($sampleCss.StatusCode -eq 200 -and $sampleCss.Content.Contains('--wk-color-surface')) 'Example app static asset smoke failed.'
        Write-Output 'Example app HTTP/static-asset smoke passed.'
    }
    finally {
        if (-not $sampleProcess.HasExited) {
            Stop-Process -Id $sampleProcess.Id -Force
        }
    }

    git diff --check
    if ($LASTEXITCODE -ne 0) {
        throw 'git diff --check failed.'
    }

    Write-Output 'WebKit validation completed successfully.'
}
finally {
    Set-Location $previousLocation
    if ($null -eq $previousCliHome) {
        Remove-Item Env:DOTNET_CLI_HOME -ErrorAction SilentlyContinue
    }
    else {
        $env:DOTNET_CLI_HOME = $previousCliHome
    }
    if ($null -eq $previousAspNetEnvironment) {
        Remove-Item Env:ASPNETCORE_ENVIRONMENT -ErrorAction SilentlyContinue
    }
    else {
        $env:ASPNETCORE_ENVIRONMENT = $previousAspNetEnvironment
    }
    if (Test-Path -LiteralPath $temporaryRoot) {
        Remove-Item -LiteralPath $temporaryRoot -Recurse -Force -ErrorAction SilentlyContinue
    }
}
