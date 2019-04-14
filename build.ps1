$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

dotnet publish --configuration Release --output "$PSScriptRoot/build" --verbosity normal --self-contained false
Copy-Item "$PSScriptRoot/scripts/run-tool.ps1" -Destination "$PSScriptRoot/build/run.ps1"
Copy-Item "$PSScriptRoot/scripts/run-tool.sh" -Destination "$PSScriptRoot/build/run.sh"

exit 0
