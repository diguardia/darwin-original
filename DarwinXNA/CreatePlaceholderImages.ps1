# Script para crear imágenes PNG de reemplazo temporales
# Mientras conviertes los DDS originales

Add-Type -AssemblyName System.Drawing

Write-Host "=== Creando imágenes PNG de reemplazo ===" -ForegroundColor Cyan
Write-Host ""

$imagenes = @(
    @{ Nombre = "Neurodator"; Tamaño = 16; Color = [System.Drawing.Color]::Red },
    @{ Nombre = "eficiente"; Tamaño = 16; Color = [System.Drawing.Color]::Green },
    @{ Nombre = "fuego1"; Tamaño = 8; Color = [System.Drawing.Color]::Orange },
    @{ Nombre = "fuego2"; Tamaño = 8; Color = [System.Drawing.Color]::Yellow },
    @{ Nombre = "seguidor"; Tamaño = 32; Color = [System.Drawing.Color]::Blue }
)

$created = 0
$folder = "imagenes"

foreach ($img in $imagenes) {
    $pngPath = Join-Path $folder "$($img.Nombre).png"
    
    # Solo crear si no existe
    if (Test-Path $pngPath) {
        Write-Host "  ? Ya existe: $($img.Nombre).png" -ForegroundColor Gray
        continue
    }
    
    try {
        $size = $img.Tamaño
        $bitmap = New-Object System.Drawing.Bitmap($size, $size)
        $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
        
        # Fondo transparente
        $graphics.Clear([System.Drawing.Color]::Transparent)
        
        # Dibujar círculo de color
        $brush = New-Object System.Drawing.SolidBrush($img.Color)
        $graphics.FillEllipse($brush, 0, 0, $size, $size)
        
        # Guardar
        $bitmap.Save($pngPath, [System.Drawing.Imaging.ImageFormat]::Png)
        
        Write-Host "  ? Creado: $($img.Nombre).png (${size}x${size}, $($img.Color.Name))" -ForegroundColor Green
        $created++
        
        # Limpiar
        $graphics.Dispose()
        $bitmap.Dispose()
        $brush.Dispose()
    }
    catch {
        Write-Host "  ? Error al crear $($img.Nombre).png: $_" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== RESULTADO ===" -ForegroundColor Cyan
Write-Host "Imágenes de reemplazo creadas: $created" -ForegroundColor Green

if ($created -gt 0) {
    Write-Host ""
    Write-Host "??  ESTAS SON IMÁGENES TEMPORALES ??" -ForegroundColor Yellow
    Write-Host "Son círculos de colores simples para que el juego funcione." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Para obtener las imágenes originales:" -ForegroundColor Cyan
    Write-Host "  1. Busca los archivos .dds originales" -ForegroundColor White
    Write-Host "  2. Usa GIMP para abrirlos y exportarlos como PNG" -ForegroundColor White
    Write-Host "  3. O usa un conversor online especializado en DDS de juegos" -ForegroundColor White
    Write-Host ""
    Write-Host "Ahora puedes ejecutar el juego con: dotnet run" -ForegroundColor Green
}
