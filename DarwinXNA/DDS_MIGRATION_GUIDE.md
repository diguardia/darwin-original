# Migración de XNA a MonoGame - Problema con archivos DDS

## El Problema

**MonoGame no soporta archivos DDS** (DirectDraw Surface), que eran el formato estándar en XNA 2.0-4.0. 

Cuando intentas ejecutar el juego, obtienes este error:
```
System.InvalidOperationException: This image format is not supported
 ---> System.InvalidOperationException: unknown image type
```

## La Solución

Debes convertir todos los archivos `.dds` a formatos soportados por MonoGame (PNG, JPG, BMP, etc.). Se recomienda **PNG** porque mantiene la transparencia.

### Método 1: Script Automático (Recomendado)

#### Requisitos previos:
Instala **ImageMagick**:
- Windows: https://imagemagick.org/script/download.php
  - Descarga `ImageMagick-X.X.X-Q16-x64-dll.exe`
  - Durante la instalación, marca "Install legacy utilities (e.g. convert)"
  
#### Ejecutar conversión:
```powershell
cd DarwinXNA
.\ConvertDDStoPNG.ps1
```

El script automáticamente:
1. Encuentra todos los archivos `.dds` en la carpeta `Fonts/`
2. Los convierte a `.png`
3. El código ya está preparado para usar los archivos PNG

### Método 2: Conversión Online (Sin instalaciones)

1. **Ve a**: https://www.aconvert.com/image/dds-to-png/
2. **Sube** los archivos `.dds` desde `DarwinXNA/Fonts/`
3. **Descarga** los archivos PNG convertidos
4. **Guárdalos** en `DarwinXNA/Fonts/` con el mismo nombre:
   - `franklin.dds` ? `franklin.png`

### Método 3: Usando GIMP (Editor gratuito)

1. **Descarga GIMP**: https://www.gimp.org/downloads/
2. **Instala** y abre GIMP
3. **Abre** el archivo `DarwinXNA/Fonts/franklin.dds`
4. **Exporta** como PNG:
   - File ? Export As...
   - Nombre: `franklin.png`
   - Tipo: PNG image
5. **Guarda** en la misma carpeta `Fonts/`

### Método 4: Usando Paint.NET (Windows)

1. **Descarga Paint.NET**: https://www.getpaint.net/download.html
2. **Instala** el plugin DDS: https://forums.getpaint.net/topic/111731-dds-filetype-plus/
3. **Abre** `franklin.dds`
4. **Guarda como** PNG

## Archivos a Convertir

Archivos DDS actuales en el proyecto:
```
DarwinXNA/Fonts/franklin.dds ? franklin.png
```

## ¿Qué hace el código actualizado?

El código en `XNAFont.cs` ahora:

1. **Intenta cargar PNG primero**: `franklin.png`
2. **Si no existe PNG**: Verifica si existe el archivo DDS
3. **Si existe DDS**: Lanza una excepción clara indicando que debe convertirse
4. **Si no existe ninguno**: Error de archivo no encontrado

```csharp
// El código busca automáticamente el archivo PNG
string pngPath = System.IO.Path.ChangeExtension(strDDS, ".png");
if (System.IO.File.Exists(pngPath))
{
    ret.texture = Texture2D.FromFile(device, pngPath);
}
```

## Verificar que funciona

Después de convertir:

1. **Verifica** que existe `DarwinXNA/Fonts/franklin.png`
2. **Ejecuta** el juego:
   ```bash
   dotnet run --project DarwinXNA/DarwinXNA.csproj
   ```
3. **Si hay problemas**: Revisa `log.txt` para más detalles

## ¿Por qué MonoGame no soporta DDS?

- **DDS** es un formato propietario de DirectX/Microsoft
- **MonoGame** es multiplataforma y usa **StbImageSharp** para cargar imágenes
- **StbImageSharp** solo soporta formatos estándar: PNG, JPG, BMP, TGA, GIF, PSD, HDR, PIC

## Otras alternativas (Avanzado)

Si tienes muchos archivos DDS o quieres usar el Content Pipeline de MonoGame:

### Usar SpriteFont de MonoGame
1. Crea un archivo `.spritefont` en el Content Pipeline
2. Reemplaza `XNAFont` por `SpriteFont` de MonoGame
3. Ventaja: Mejor rendimiento y más opciones de fuentes

```csharp
// En lugar de XNAFont custom
SpriteFont font = Content.Load<SpriteFont>("franklin");
spriteBatch.DrawString(font, "Hello", position, Color.White);
```

## Recursos Adicionales

- **Documentación MonoGame**: https://docs.monogame.net/articles/content/using_mgcb_editor.html
- **Guía migración XNA a MonoGame**: https://docs.monogame.net/articles/migrate_xna.html
- **ImageMagick CLI**: https://imagemagick.org/script/convert.php

## Solución de Problemas

### "El script no se puede ejecutar"
```powershell
# Habilitar ejecución de scripts en PowerShell
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
```

### "No se encuentra ImageMagick"
- Reinicia PowerShell después de instalar ImageMagick
- Verifica la instalación: `magick --version`

### "El PNG convertido se ve mal"
- Prueba otro método de conversión
- Verifica que el DDS original sea válido
- Asegúrate de que mantiene el canal alpha (transparencia)
