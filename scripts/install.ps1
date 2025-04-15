Write-Host "=== Initialisation de l’environnement ==="

# ANGULAR CLI
if (-not (Get-Command ng -ErrorAction SilentlyContinue)) {
    Write-Host "Installation d'Angular CLI 17..."
    npm install -g @angular/cli@17
} else {
    $version = ng version | Select-String -Pattern "Angular CLI:" | ForEach-Object { $_.ToString().Trim() }
    Write-Host "Angular CLI déjà installé : $version"
}

# .NET SDK
$dotnetVersion = dotnet --version
if ($dotnetVersion -like "8.*") {
    Write-Host ".NET SDK 8 déjà installé : $dotnetVersion"
} else {
    Write-Host "Installation de .NET SDK 8..."
    Invoke-WebRequest -Uri https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.ps1 -OutFile dotnet-install.ps1
    powershell -ExecutionPolicy Bypass -File .\dotnet-install.ps1 -Version 8.0.100
}

Write-Host ""
Write-Host "=== Environnement prêt ==="
