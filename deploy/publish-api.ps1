# =====================================================================
# PUBLICAR API → NUBE (runasp.net)
# Genera el publish en ./publish para subirlo por FTP / panel de control
# de runasp.net.
#
# Uso:  powershell -ExecutionPolicy Bypass -File publish-api.ps1
# =====================================================================

$ErrorActionPreference = 'Stop'

Set-Location (Join-Path $PSScriptRoot '..\ERP.Api')

# Publicar en Release
Write-Host "Publicando API (Release)..." -ForegroundColor Cyan
dotnet publish ERP.Api -c Release -o "$PSScriptRoot\publish"

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al publicar." -ForegroundColor Red
    exit 1
}

$out = Join-Path $PSScriptRoot 'publish'

# Copiar el .env de producción al publish (para que el host lo encuentre
# sin importar si ASPNETCORE_ENVIRONMENT viene configurado o no)
if (Test-Path 'ERP.Api\.env.production') {
    Copy-Item 'ERP.Api\.env.production' (Join-Path $out '.env') -Force
    Copy-Item 'ERP.Api\.env.production' (Join-Path $out '.env.production') -Force
    Write-Host "✅ .env.production copiado al publish." -ForegroundColor Green
} else {
    Write-Host "⚠️  No se encontró .env.production. Revisa la configuración." -ForegroundColor Yellow
}

Write-Host "" -ForegroundColor Green
Write-Host "✅ Publicación lista en: $out" -ForegroundColor Green
Write-Host "" -ForegroundColor Green
Write-Host "SIGUIENTES PASOS (en runasp.net):" -ForegroundColor Yellow
Write-Host "  1. Sube TODO el contenido de $out por FTP o ZIP al sitio." -ForegroundColor Yellow
Write-Host "  2. Confirma que ASPNETCORE_ENVIRONMENT=Production en el host." -ForegroundColor Yellow
Write-Host "  3. La API quedará en: https://t3xn1ca.runasp.net" -ForegroundColor Yellow
Write-Host "     Swagger: https://t3xn1ca.runasp.net/swagger" -ForegroundColor Yellow
