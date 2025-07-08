# Konfiguráció
$resourceGroup = "MyResourceGroup"
$sqlServerName = "patientsqlserver"
$ruleName = "TemporaryAccess"
$durationMinutes = 60

$myIp = (Invoke-RestMethod -Uri "http://ipinfo.io/json").ip
Write-Host "Aktuális IP-cím: $myIp"

Write-Host "Tűzfal szabály hozzáadása..."
az sql server firewall-rule create `
  --resource-group $resourceGroup `
  --server $sqlServerName `
  --name $ruleName `
  --start-ip-address $myIp `
  --end-ip-address $myIp | Out-Null

Write-Host "IP-cím engedélyezve $durationMinutes percre."

Start-Sleep -Seconds ($durationMinutes * 60)

Write-Host "Tűzfal szabály törlése..."
az sql server firewall-rule delete `
  --resource-group $resourceGroup `
  --server $sqlServerName `
  --name $ruleName | Out-Null

Write-Host "Ideiglenes hozzáférés visszavonva."
