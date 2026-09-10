param (
    [string]$BuildFile = "Celestite/Build.cs",
    [string]$CsprojFile = "Celestite.Desktop/Celestite.Desktop.csproj",
    [switch]$NoAot,
    [switch]$UpdateWorkloads
)

try {
    if (-not (Test-Path $BuildFile)) {
        Write-Error "build.cs not found at $BuildFile"
        exit 1
    }

    $buildContent = Get-Content -Path $BuildFile -Raw

    if ($buildContent -match 'Version\s*=\s*"([^"]+)"') {
        $version = $matches[1]
        Write-Host "Version: $version"
    } else {
        Write-Error "Failed to find Version in $BuildFile"
        exit 1
    }

    $year = (Get-Date).Year
    $copyright = "Copyright © $year Kengxxiao"

    if (-not (Test-Path $CsprojFile)) {
        Write-Error ".csproj not found at $CsprojFile"
        exit 1
    }

    $csprojContent = Get-Content -Path $CsprojFile -Raw
    $updatedContent = $csprojContent `
        -replace '<Version>[^<]+</Version>', "<Version>$version</Version>" `
        -replace '<FileVersion>[^<]+</FileVersion>', "<FileVersion>$version</FileVersion>" `
        -replace '<InformationalVersion>[^<]+</InformationalVersion>', "<InformationalVersion>$version</InformationalVersion>" `
        -replace '<Copyright>[^<]+</Copyright>', "<Copyright>$copyright</Copyright>"

    Set-Content -Path $CsprojFile -Value $updatedContent -Encoding Default
    Write-Host "Updated $CsprojFile with Version: $version, InformationalVersion: $version, Copyright: $copyright"
}
catch {
    Write-Error "Error updating .csproj: $_"
    exit 1
}

if ($UpdateWorkloads) {
    Write-Host "Updating .NET workloads..."
    dotnet workload update
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to update .NET workloads."
        exit $LASTEXITCODE
    }
}

# Determine whether NativeAOT can be used
$hasMsvcLinker = $false
if (-not $NoAot) {
    if (Get-Command link.exe -ErrorAction SilentlyContinue) {
        $hasMsvcLinker = $true
    } else {
        $vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
        if (Test-Path $vswhere) {
            $vsPath = & $vswhere -latest -products * -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath
            if ($vsPath) {
                $hasMsvcLinker = $true
            }
        }
    }
}

# Publish Celestite.Desktop project
Write-Host "Publishing Celestite.Desktop for Windows..."
if ($hasMsvcLinker) {
    Write-Host "MSVC linker detected. Publishing with NativeAOT..."
    dotnet publish Celestite.Desktop/Celestite.Desktop.csproj `
        -p:StripSymbols=true `
        -p:Configuration=Release `
        -p:SelfContained=true `
        -p:PublishTrimmed=true `
        -p:PublishAot=true `
        -p:RuntimeIdentifier=win-x64 `
        -p:DebugType=None `
        -p:DebugSymbols=false `
        --output ./Release/Windows
} else {
    Write-Host "MSVC linker not found or -NoAot specified. Publishing Self-Contained SingleFile executable..."
    dotnet publish Celestite.Desktop/Celestite.Desktop.csproj `
        -p:Configuration=Release `
        -p:SelfContained=true `
        -p:PublishSingleFile=true `
        -p:PublishAot=false `
        -p:RuntimeIdentifier=win-x64 `
        -p:DebugType=None `
        -p:DebugSymbols=false `
        --output ./Release/Windows
}

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to publish Celestite.Desktop."
    exit $LASTEXITCODE
}

Write-Host "Publish completed successfully. Output is in ./Release/Windows"