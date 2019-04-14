[CmdletBinding()]
Param(
    #[Parameter(Mandatory=$true)]
	[string]$SolutionFolder
)

$ScriptDir = Split-Path $MyInvocation.MyCommand.Path -Parent

$LicenseDependencyGeneration = $true
$LoadMetadata = $true
$IgnoreCLITools = $false
$IgnorePackagesConfig = $false
$PrintResult = $true

if ([string]::IsNullOrEmpty($SolutionFolder))
{
	$SolutionFolder = $ScriptDir
}

if (!(Test-Path $SolutionFolder))
{
	Write-Output "Specified folder was not found"
	exit -1
}

#$arguments = @()
$arguments = @("--solution-folder", "$SolutionFolder")

if ($LicenseDependencyGeneration)
{
	$arguments += "--generate-license"
}

if ($LoadMetadata)
{
	$arguments += "--load-metadata"
}

if ($IgnoreCLITools)
{
	$arguments += "--ignore-cli-tools"
}

if ($IgnorePackagesConfig)
{
	$arguments += "--load-packages-config"
}

if ($PrintResult)
{
	$arguments += "--print-results"
}

#dotnet run --project NuGetHelper.Tool --configuration Debug --no-build --no-restore -- "$arguments"

Start-Process "dotnet" -NoNewWindow -Wait -ArgumentList "NuGetHelper.Tool.dll", "$arguments"

exit 0