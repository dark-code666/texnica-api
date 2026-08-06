# =====================================================================
# ACTUALIZAR BD EN LA NUBE (agrega tablas nuevas)
# ---------------------------------------------------------------------
# Tu BD en la nube ya tiene las tablas antiguas pero su
# _EFMigrationsHistory está VACÍO. Por eso NO usamos `dotnet ef update`
# (intentaría recrear tablas existentes y fallaría).
#
# Este script aplica SOLO las migraciones nuevas (Mill Production,
# Mill Test, Fabric Shipment, Lot, Catálogos + columnas de auditoría)
# mediante el SQL incremental: Scripts\MigrateNewTables.sql
#
# Uso: powershell -ExecutionPolicy Bypass -File deploy-db-prod-migrate.ps1
# =====================================================================

$ErrorActionPreference = 'Stop'

# Credenciales de producción (coinciden con ERP.Api/.env.production)
$server = $env:DB_SERVER ?? '3.129.157.12'
$db     = $env:DB_NAME ?? 'POSDevelop'
$user   = $env:DB_USER ?? 'won'
$pass   = $env:DB_PASSWORD ?? 'W@nc0nexion'

$sqlNew = Join-Path $PSScriptRoot '..\ERP.Api\Scripts\MigrateNewTables.sql'
$sqlBackfill = Join-Path $PSScriptRoot '..\ERP.Api\Scripts\BackfillMigrationHistory.sql'
if (-not (Test-Path $sqlNew)) {
    Write-Host "ERROR: No existe $sqlNew" -ForegroundColor Red
    exit 1
}

Write-Host "Paso 1/2 — Respaldo del historial de migraciones..." -ForegroundColor Cyan
if (Test-Path $sqlBackfill) {
    sqlcmd -S $server -d $db -U $user -P $pass -i $sqlBackfill -b
    if ($LASTEXITCODE -ne 0) { Write-Host "❌ Error en respaldo de historial." -ForegroundColor Red; exit 1 }
}

Write-Host "Paso 2/2 — Aplicando tablas nuevas en: $server / $db" -ForegroundColor Cyan
Write-Host "Tablas a crear: MillProductions, MillTests, FabricShipments, Lots, CatalogValues" -ForegroundColor Cyan
sqlcmd -S $server -d $db -U $user -P $pass -i $sqlNew -b

if ($LASTEXITCODE -eq 0) {
    Write-Host "" -ForegroundColor Green
    Write-Host "✅ Tablas nuevas creadas correctamente." -ForegroundColor Green
    Write-Host "Verifica:" -ForegroundColor Green
    Write-Host "  SELECT name FROM sys.tables WHERE name IN ('MillProductions','MillTests','FabricShipments','Lots','CatalogValues')" -ForegroundColor Green
} else {
    Write-Host "❌ Hubo un error al aplicar. Revisa conectividad/credenciales." -ForegroundColor Red
    exit 1
}
