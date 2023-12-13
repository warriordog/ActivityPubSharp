# Usage: update version in all csproj files, then run this from the repository root.

[CmdLetBinding(PositionalBinding = $false)]
param (
    [Parameter(
        HelpMessage = "URL of a NuGet source repository. Defaults to nuget.org."
    )]
    [string] $NugetSource = "https://api.nuget.org/v3/index.json",
    
    [Parameter(
        Mandatory,
        HelpMessage = "API key for the NuGet repository."
    )]
    [string] $NugetKey,
    
    [Parameter(
        HelpMessage = "Name of the build configuration to run. Defaults to Release."
    )]
    [string] $BuildConfig = "Release",
    
    [Parameter(
        Mandatory,
        Position = 0,
        ValueFromRemainingArguments,
        HelpMessage = "Names of packages (projects) to upload, separated by commas or spaces."
    )]
    [string[]] $PackageNames,

    [Parameter(
        HelpMessage = "Uploads a 'snapshot' build, which includes a timestamp suffix"
    )]
    [switch] $Snapshot
);

$versionSuffix = "";
if ($Snapshot)
{
    $timestamp = Get-Date -Format "yyyy-MM-dd.HH-mm-ss.fff";
    $versionSuffix = "snapshot.$timestamp";
}

# Workaround for https://github.com/dotnet/docs/issues/12304
Write-Output "## [publish-package] Purge old NuGet builds";
foreach ($packageName in $PackageNames)
{
    $buildDir = "Source\$packageName\bin\$buildConfig";
    if (Test-Path -Path $buildDir)
    {
        $oldNugets = Get-ChildItem -Path $buildDir -Include "*.nupkg" -Recurse;
        Remove-Item $oldNugets;
    }
}

Write-Output "";
Write-Output "## [publish-package] Build and pack selected packages";
& dotnet restore /p:VersionSuffix=$versionSuffix
& dotnet clean --configuration $BuildConfig;
& dotnet build --configuration $BuildConfig --version-suffix $versionSuffix;
& dotnet pack --configuration $BuildConfig --version-suffix $versionSuffix;

foreach ($packageName in $PackageNames)
{
    $packagePath = "Source\$packageName\bin\$buildConfig\*.nupkg";

    Write-Output "";
    Write-Output "## [publish-package] Push $packageName";
    & dotnet nuget push $packagePath --api-key $NugetKey --source $NugetSource;
}