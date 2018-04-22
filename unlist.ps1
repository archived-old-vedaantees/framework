$PackageId = "WorkNotes.Metadata.Models"
$ApiKey = "oy2nbipr5yctq72t64tq6ou3m2fwl5fuxiaaovbys3r2l4"

$json = Invoke-WebRequest -Uri "https://api.nuget.org/v3-flatcontainer/$PackageId/index.json" | ConvertFrom-Json

foreach($version in $json.versions)
{
  Write-Host "Unlisting $PackageId, Ver $version"
  Invoke-Expression "nuget.exe delete $PackageId $version $ApiKey -source https://api.nuget.org/v3/index.json -NonInteractive"
}