# =====================================================================
# DEPLOY BD → NUBE (crear todas las tablas)
# Aplica el script SQL generado por EF Migrations a la BD de producción.
#
# Requisitos: SQLCMD instalado (ya lo tienes).
# Uso:  powershell -ExecutionPolicy Bypass -File deploy-db-prod.ps1
# =====================================================================

$ErrorActionPreference = 'Stop'

# Credenciales de producción (coinciden con ERP.Api/.env.production)
$server = $env:DB_SERVER ?? '3.129.157.12'
$db     = $env:DB_NAME ?? 'POSDevelop'
$user   = $env:DB_USER ?? 'won'
$pass   = $env:DB_PASSWORD ?? 'W@nc0nexion'

$sqlFile = Join-Path $PSScriptRoot '..\ERP.Api\Scripts\FullDatabase.sql'
if (-not (Test-Path $sqlFile)) {
    Write-Host "ERROR: No existe $sqlFile" -ForegroundColor Red
    exit 1
}

Write-Host "==============================================" -ForegroundColor Cyan
Write-Host "  CREANDO TABLAS EN: $server / $db"
Write-Host "==============================================" -ForegroundColor Cyan

sqlcmd -S $server -d $db -U $user -P $pass -i $sqlFile -b

if ($LASTEXITCODE -eq 0) {
    Write-Host "" -ForegroundColor Green
    Write-Host "✅ Base de datos creada/actualizada correctamente." -ForegroundColor Green
    Write-Host "Verifica: SELECT name FROM sys.tables ORDER BY name" -ForegroundColor Green
} else {
    Write-Host "❌ Hubo un error al aplicar el script." -ForegroundColor Red
    Write-Host "Revisa que el servidor '$server' sea accesible y las credenciales sean correctas." -ForegroundColor Yellow
    exit 1
}
