# Script para visualizar los logs de Darwin
# Uso: .\ViewLogs.ps1 [-Follow] [-Lines 50] [-Level ERROR]

param(
    [switch]$Follow,
    [int]$Lines = 50,
    [ValidateSet("ALL", "DEBUG", "INFO", "WARNING", "ERROR", "FATAL")]
    [string]$Level = "ALL"
)

$logFiles = @(
    "log.txt",
    "bin\Debug\net10.0\log.txt",
    "DarwinXNA\bin\Debug\net10.0\log.txt",
    "..\log.txt"
)

$logFound = $false

foreach ($logPath in $logFiles) {
    if (Test-Path $logPath) {
        Write-Host "=== Log encontrado en: $logPath ===" -ForegroundColor Green
        Write-Host ""
        
        if ($Level -ne "ALL") {
            Write-Host "Filtrando por nivel: $Level" -ForegroundColor Yellow
            Write-Host ""
        }
        
        if ($Follow) {
            Write-Host "Siguiendo log en tiempo real (Ctrl+C para salir)..." -ForegroundColor Yellow
            if ($Level -eq "ALL") {
                Get-Content $logPath -Wait
            } else {
                Get-Content $logPath -Wait | Where-Object { $_ -match "\[$Level\s*\]" }
            }
        } else {
            $content = Get-Content $logPath
            
            if ($Level -ne "ALL") {
                $content = $content | Where-Object { $_ -match "\[$Level\s*\]" }
            }
            
            $totalLines = $content.Count
            
            if ($totalLines -gt $Lines) {
                Write-Host "Mostrando últimas $Lines líneas de $totalLines totales:" -ForegroundColor Cyan
                $content | Select-Object -Last $Lines
            } else {
                Write-Host "Contenido completo del log ($totalLines líneas):" -ForegroundColor Cyan
                $content
            }
            
            Write-Host ""
            Write-Host "=== Resumen ===" -ForegroundColor Cyan
            $errors = ($content | Select-String "\[ERROR\s*\]").Count
            $fatals = ($content | Select-String "\[FATAL\s*\]").Count
            $warnings = ($content | Select-String "\[WARNING\s*\]").Count
            $infos = ($content | Select-String "\[INFO\s*\]").Count
            
            Write-Host "Errores fatales: $fatals" -ForegroundColor $(if ($fatals -gt 0) { "Red" } else { "Gray" })
            Write-Host "Errores: $errors" -ForegroundColor $(if ($errors -gt 0) { "Red" } else { "Gray" })
            Write-Host "Advertencias: $warnings" -ForegroundColor $(if ($warnings -gt 0) { "Yellow" } else { "Gray" })
            Write-Host "Info: $infos" -ForegroundColor Gray
        }
        
        $logFound = $true
        break
    }
}

if (-not $logFound) {
    Write-Host "No se encontró ningún archivo log.txt" -ForegroundColor Red
    Write-Host "El log se crea cuando el programa se ejecuta y hay errores." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Ubicaciones buscadas:" -ForegroundColor Cyan
    foreach ($path in $logFiles) {
        Write-Host "  - $path"
    }
    Write-Host ""
    Write-Host "Ejecuta primero el programa para generar el log:" -ForegroundColor Yellow
    Write-Host "  dotnet run --project DarwinXNA.csproj" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Ejemplos de uso:" -ForegroundColor Cyan
Write-Host "  .\ViewLogs.ps1                    # Ver últimas 50 líneas"
Write-Host "  .\ViewLogs.ps1 -Lines 100         # Ver últimas 100 líneas"
Write-Host "  .\ViewLogs.ps1 -Follow            # Seguir en tiempo real"
Write-Host "  .\ViewLogs.ps1 -Level ERROR       # Solo errores"
Write-Host "  .\ViewLogs.ps1 -Level FATAL       # Solo fatales"
Write-Host "  .\ViewLogs.ps1 -Level WARNING     # Solo advertencias"

