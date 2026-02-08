# Script para convertir archivos DDS a PNG
# Requiere ImageMagick instalado: https://imagemagick.org/script/download.php
# O usa https://www.aconvert.com/image/dds-to-png/ online

Write-Host "=== Conversión de archivos DDS a PNG para MonoGame ===" -ForegroundColor Cyan
Write-Host ""

$fontsDir = "imagenes"
$ddsFiles = Get-ChildItem -Path $fontsDir -Filter "*.dds" -Recurse

if ($ddsFiles.Count -eq 0) {
    Write-Host "✓ No se encontraron archivos .dds en la carpeta $fontsDir" -ForegroundColor Green
    exit 0
}

Write-Host "Archivos DDS encontrados:" -ForegroundColor Yellow
$ddsFiles | ForEach-Object { Write-Host "  - $($_.Name)" -ForegroundColor Yellow }
Write-Host ""

# Verificar si ImageMagick está instalado
$hasMagick = Get-Command magick -ErrorAction SilentlyContinue
$hasConvert = Get-Command convert -ErrorAction SilentlyContinue

if ($hasMagick -or $hasConvert) {
    $cmd = if ($hasMagick) { "magick" } else { "convert" }
    
    Write-Host "ImageMagick detectado. Convirtiendo automáticamente..." -ForegroundColor Green
    Write-Host ""
    
    $converted = 0
    $failed = 0
    
    foreach ($file in $ddsFiles) {
        $outputPath = [IO.Path]::ChangeExtension($file.FullName, ".png")
        $relPath = $file.FullName.Replace((Get-Location).Path + "\", "")
        
        Write-Host "Convirtiendo: $relPath" -ForegroundColor Cyan
        
        try {
            & $cmd $file.FullName $outputPath 2>&1 | Out-Null
            if (Test-Path $outputPath) {
                Write-Host "  ✓ Convertido: $([IO.Path]::GetFileName($outputPath))" -ForegroundColor Green
                $converted++
            } else {
                Write-Host "  ✗ No se creó el archivo de salida" -ForegroundColor Red
                $failed++
            }
        } catch {
            Write-Host "  ✗ Error al convertir: $_" -ForegroundColor Red
            $failed++
        }
    }
    
    Write-Host ""
    Write-Host "=== RESULTADO ===" -ForegroundColor Cyan
    Write-Host "Convertidos exitosamente: $converted" -ForegroundColor Green
    if ($failed -gt 0) {
        Write-Host "Fallidos: $failed" -ForegroundColor Red
    }
    
    if ($converted -gt 0) {
        Write-Host ""
        Write-Host "✓ Ahora puedes ejecutar el juego con 'dotnet run'" -ForegroundColor Green
    }
} else {
    Write-Host "ImageMagick no está instalado." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Opciones para convertir archivos DDS a PNG:" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1. Instalar ImageMagick:" -ForegroundColor White
    Write-Host "   - Descargar de: https://imagemagick.org/script/download.php" -ForegroundColor Gray
    Write-Host "   - Después ejecutar este script nuevamente" -ForegroundColor Gray
    Write-Host ""
    Write-Host "2. Convertir online:" -ForegroundColor White
    Write-Host "   - Ir a: https://www.aconvert.com/image/dds-to-png/" -ForegroundColor Gray
    Write-Host "   - Subir los archivos .dds de la carpeta Fonts/" -ForegroundColor Gray
    Write-Host "   - Descargar los PNG y guardarlos en Fonts/" -ForegroundColor Gray
    Write-Host ""
    Write-Host "3. Usar GIMP (editor de imágenes gratuito):" -ForegroundColor White
    Write-Host "   - Descargar de: https://www.gimp.org/downloads/" -ForegroundColor Gray
    Write-Host "   - Abrir franklin.dds" -ForegroundColor Gray
    Write-Host "   - Exportar como franklin.png" -ForegroundColor Gray
    Write-Host ""
    
    Write-Host "Archivos a convertir:" -ForegroundColor Yellow
    foreach ($file in $ddsFiles) {
        $relPath = $file.FullName.Replace((Get-Location).Path + "\", "")
        Write-Host "  $relPath" -ForegroundColor Gray
    }
}

Write-Host ""
Write-Host "Después de convertir, el código cargará automáticamente los archivos PNG" -ForegroundColor Cyan
