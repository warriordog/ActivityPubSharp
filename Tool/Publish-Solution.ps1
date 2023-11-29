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
        HelpMessage = "Uploads a 'snapshot' build, which includes a timestamp suffix"
    )]
    [switch] $Snapshot
);

$packageNames =
@(
    "ActivityPub.Client",
    "ActivityPub.Common",
    "ActivityPub.Extension.Mastodon",
    "ActivityPub.Server",
    "ActivityPub.Server.AspNetCore",
    "ActivityPub.Types"
);

Write-Output "## [Publish-Solution] Publish entire solution";
Write-Output "";
& $PSScriptRoot\Publish-Package `
    -NugetSource $NugetSource `
    -NugetKey $NugetKey `
    -BuildConfig $BuildConfig `
    -Snapshot:$Snapshot `
    $packageNames;