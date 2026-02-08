# Script para detectar imágenes problemáticas en MonoGame

Write-Host "=== Detector de Imágenes Problemáticas para MonoGame ===" -ForegroundColor Cyan
Write-Host ""

$imageFolder = "imagenes"

if (-not (Test-Path $imageFolder)) {
    Write-Host "ERROR: No se encuentra la carpeta '$imageFolder'" -ForegroundColor Red
    exit 1
}

Write-Host "Analizando carpeta: $imageFolder" -ForegroundColor Green
Write-Host ""

# Buscar archivos DDS (NO soportados)
$ddsFiles = Get-ChildItem -Path $imageFolder -Filter "*.dds" -Recurse
if ($ddsFiles.Count -gt 0) {
    Write-Host "??  ARCHIVOS DDS ENCONTRADOS (NO SOPORTADOS):" -ForegroundColor Red
    foreach ($file in $ddsFiles) {
        $size = [math]::Round($file.Length / 1KB, 2)
        Write-Host "  ? $($file.Name) - ${size}KB - $($file.FullName)" -ForegroundColor Red
    }
    Write-Host ""
}

# Buscar archivos TGA (pueden no ser soportados)
$tgaFiles = Get-ChildItem -Path $imageFolder -Filter "*.tga" -Recurse
if ($tgaFiles.Count -gt 0) {
    Write-Host "??  ARCHIVOS TGA ENCONTRADOS (PUEDE NO SER SOPORTADO):" -ForegroundColor Yellow
    foreach ($file in $tgaFiles) {
        $size = [math]::Round($file.Length / 1KB, 2)
        Write-Host "  ??  $($file.Name) - ${size}KB - $($file.FullName)" -ForegroundColor Yellow
    }
    Write-Host ""
}

# Buscar archivos soportados
$supportedExtensions = @("*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif")
$supportedFiles = @()
foreach ($ext in $supportedExtensions) {
    $supportedFiles += Get-ChildItem -Path $imageFolder -Filter $ext -Recurse
}

if ($supportedFiles.Count -gt 0) {
    Write-Host "? ARCHIVOS SOPORTADOS:" -ForegroundColor Green
    $totalSize = 0
    foreach ($file in $supportedFiles) {
        $size = [math]::Round($file.Length / 1KB, 2)
        $totalSize += $file.Length
        $ext = $file.Extension.ToUpper()
        Write-Host "  ? $($file.Name) - ${size}KB ($ext)" -ForegroundColor Gray
    }
    $totalSizeMB = [math]::Round($totalSize / 1MB, 2)
    Write-Host ""
    Write-Host "Total: $($supportedFiles.Count) archivos soportados (${totalSizeMB}MB)" -ForegroundColor Green
}

# Resumen
Write-Host ""
Write-Host "=== RESUMEN ===" -ForegroundColor Cyan
Write-Host "Archivos DDS (NO soportados): $($ddsFiles.Count)" -ForegroundColor $(if ($ddsFiles.Count -gt 0) { "Red" } else { "Green" })
Write-Host "Archivos TGA (riesgo): $($tgaFiles.Count)" -ForegroundColor $(if ($tgaFiles.Count -gt 0) { "Yellow" } else { "Green" })
Write-Host "Archivos soportados: $($supportedFiles.Count)" -ForegroundColor Green

# Recomendaciones
Write-Host ""
if ($ddsFiles.Count -gt 0 -or $tgaFiles.Count -gt 0) {
    Write-Host "=== ACCIÓN REQUERIDA ===" -ForegroundColor Red
    Write-Host ""
    
    if ($ddsFiles.Count -gt 0) {
        Write-Host "Para convertir archivos DDS a PNG:" -ForegroundColor Yellow
        Write-Host "  1. Ejecuta: .\ConvertDDStoPNG.ps1" -ForegroundColor White
        Write-Host "  2. O convierte online: https://www.aconvert.com/image/dds-to-png/" -ForegroundColor White
        Write-Host ""
    }
    
    if ($tgaFiles.Count -gt 0) {
        Write-Host "Para convertir archivos TGA a PNG:" -ForegroundColor Yellow
        Write-Host "  1. Usa GIMP: https://www.gimp.org/" -ForegroundColor White
        Write-Host "  2. O usa ImageMagick:" -ForegroundColor White
        foreach ($file in $tgaFiles) {
            $pngName = [IO.Path]::ChangeExtension($file.Name, ".png")
            Write-Host "     magick `"$($file.FullName)`" `"$([IO.Path]::Combine([IO.Path]::GetDirectoryName($file.FullName), $pngName))`"" -ForegroundColor Gray
        }
    }
} else {
    Write-Host "? Todas las imágenes usan formatos soportados" -ForegroundColor Green
}

Write-Host ""
Write-Host "=== AYUDA ===" -ForegroundColor Cyan
Write-Host "Formatos SOPORTADOS por MonoGame:" -ForegroundColor White
Write-Host "  ? PNG  - Recomendado (soporta transparencia)" -ForegroundColor Green
Write-Host "  ? JPG  - Bueno para fotos (sin transparencia)" -ForegroundColor Green
Write-Host "  ? BMP  - Soportado (archivos grandes)" -ForegroundColor Green
Write-Host "  ? GIF  - Soportado (animación no soportada)" -ForegroundColor Green
Write-Host ""
Write-Host "Formatos NO SOPORTADOS:" -ForegroundColor White
Write-Host "  ? DDS  - DirectDraw Surface (formato XNA)" -ForegroundColor Red
Write-Host "  ? TGA  - Targa (puede no funcionar)" -ForegroundColor Red
