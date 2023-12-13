# Usage: Build-Docs.ps1 [--serve]

[CmdLetBinding(PositionalBinding = $false)]
param (
    [Parameter(
        HelpMessage = "Start a web server to view generated documentation"
    )]
    [switch] $Snapshot
)

& docfx Documentation/docfx.json --serve:$Serve