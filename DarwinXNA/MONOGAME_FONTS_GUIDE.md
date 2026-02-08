# Guía de Fuentes en MonoGame

## ?? El Problema con XNA Fonts

En XNA 2.0-4.0, era común usar fuentes bitmap custom (archivos `.dds` + `.xml`). MonoGame **NO soporta este sistema**.

### ? Lo que NO funciona:
```csharp
// Sistema antiguo de XNA con archivos .dds
XNAFont font = XNAFont.LoadFont(device, "font.xml", "font.dds", spriteBatch);
```

### ? Lo que SÍ funciona en MonoGame:
```csharp
// Sistema moderno con Content Pipeline
SpriteFont font = Content.Load<SpriteFont>("FontName");
spriteBatch.DrawString(font, "Texto", position, Color.White);
```

## ?? Solución Implementada: SpriteFont

### Paso 1: Archivo `.spritefont`

Archivo: `Content/Franklin.spritefont`

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent xmlns:Graphics="Microsoft.Xna.Framework.Content.Pipeline.Graphics">
  <Asset Type="Graphics:FontDescription">
    <FontName>Franklin Gothic Medium</FontName>
    <Size>14</Size>
    <Spacing>0</Spacing>
    <UseKerning>true</UseKerning>
    <Style>Regular</Style>
    
    <!-- Caracteres ASCII básicos -->
    <CharacterRegions>
      <CharacterRegion>
        <Start>&#32;</Start>   <!-- Espacio -->
        <End>&#126;</End>      <!-- ~ -->
      </CharacterRegion>
      <!-- Caracteres españoles: á, é, í, ó, ú, ñ, etc. -->
      <CharacterRegion>
        <Start>&#160;</Start>
        <End>&#255;</End>
      </CharacterRegion>
    </CharacterRegions>
  </Asset>
</XnaContent>
```

**Configuración:**
- **FontName**: Nombre de una fuente instalada en Windows (Franklin Gothic Medium, Arial, etc.)
- **Size**: Tamaño en puntos
- **CharacterRegions**: Qué caracteres incluir (importante para español: ñ, á, etc.)

### Paso 2: Agregar al Content Pipeline

El archivo `Content/Content.mgcb` debe incluir:

```
#begin Franklin.spritefont
/importer:FontDescriptionImporter
/processor:FontDescriptionProcessor
/processorParam:PremultiplyAlpha=True
/processorParam:TextureFormat=Compressed
/build:Franklin.spritefont
```

Esto se puede hacer:
- **Manualmente**: Editar `Content.mgcb`
- **Con MGCB Editor**: Abrir `Content.mgcb` con el editor visual
- **Ya está hecho** en este proyecto

### Paso 3: Código Actualizado

#### PantallaXNA.cs

**Antes:**
```csharp
private XNAFont textWriter;

public PantallaXNA(GraphicsDeviceManager _graphics) 
{
    var fontXml = Path.Combine(baseDir, "Fonts", "franklin.dds.xml");
    var fontTex = Path.Combine(baseDir, "Fonts", "franklin.dds");
    textWriter = XNAFont.LoadFont(graphics.GraphicsDevice, fontXml, fontTex, ForegroundBatch);
}

public void EscribirTexto(int x, int y, string texto, Color color)
{
    textWriter.OutputText(texto, x, y, color);
}
```

**Ahora:**
```csharp
private SpriteFont textWriter;

public PantallaXNA(GraphicsDeviceManager _graphics, ContentManager content) 
{
    textWriter = content.Load<SpriteFont>("Franklin");
}

public void EscribirTexto(int x, int y, string texto, Color color)
{
    ForegroundBatch.DrawString(textWriter, texto, new Vector2(x, y), color);
}
```

#### Game1.cs

**Antes:**
```csharp
pantalla = new PantallaXNA(graphics);
```

**Ahora:**
```csharp
pantalla = new PantallaXNA(graphics, Content);
```

## ?? Cómo Funciona

1. **Build time**: El Content Pipeline convierte el `.spritefont` ? `.xnb` (binario)
2. **Runtime**: `Content.Load<SpriteFont>()` carga el `.xnb`
3. **Render**: `DrawString()` dibuja el texto usando la fuente

## ?? Ventajas de SpriteFont

| Característica | XNAFont (antiguo) | SpriteFont (moderno) |
|----------------|-------------------|----------------------|
| Formato | .dds + .xml | .spritefont ? .xnb |
| Soporte MonoGame | ? No | ? Sí |
| Escalado | ? Pixelado | ? Suavizado |
| Kerning | ?? Manual | ? Automático |
| Fuentes del sistema | ? No | ? Sí |
| Caracteres especiales | ?? Limitado | ? Configurable |
| Performance | ?? Regular | ?? Optimizado |

## ?? Personalización de SpriteFont

### Cambiar fuente:
```xml
<FontName>Arial</FontName>
<!-- O cualquier fuente instalada en tu sistema -->
```

### Cambiar tamaño:
```xml
<Size>20</Size>
<!-- Más grande para títulos, más pequeño para detalles -->
```

### Agregar más caracteres:
```xml
<CharacterRegions>
  <!-- ASCII básico -->
  <CharacterRegion>
    <Start>&#32;</Start>
    <End>&#126;</End>
  </CharacterRegion>
  
  <!-- Español -->
  <CharacterRegion>
    <Start>&#160;</Start>
    <End>&#255;</End>
  </CharacterRegion>
  
  <!-- Símbolos adicionales (opcional) -->
  <CharacterRegion>
    <Start>&#8364;</Start> <!-- € -->
    <End>&#8364;</End>
  </CharacterRegion>
</CharacterRegions>
```

### Negrita o cursiva:
```xml
<Style>Bold</Style>
<!-- O: Regular, Italic, Bold Italic -->
```

## ?? Solución de Problemas

### Error: "Cannot find font 'Franklin Gothic Medium'"

**Causa**: La fuente no está instalada en tu sistema.

**Solución**:
1. Cambia a una fuente común:
   ```xml
   <FontName>Arial</FontName>
   ```
2. O instala la fuente en Windows

### Error: "Character '€' is not available"

**Causa**: El carácter no está en `CharacterRegions`.

**Solución**: Agregar el rango de caracteres al `.spritefont`

### El texto se ve pixelado

**Causa**: Tamaño de fuente muy pequeño o escalado.

**Solución**:
```xml
<Size>16</Size>  <!-- Tamaño más grande -->
```

### Error al compilar el Content

**Causa**: El archivo `.spritefont` tiene errores XML o la fuente no existe.

**Solución**: 
1. Verifica que el XML esté bien formado
2. Verifica que la fuente existe: `fc-list` en Linux o revisar Fonts en Windows

## ?? Archivos Modificados en Este Proyecto

1. ? **`Content/Franklin.spritefont`** (nuevo) - Definición de fuente
2. ? **`Content/Content.mgcb`** - Agregada configuración de fuente
3. ? **`PantallaXNA.cs`** - Cambiado a usar `SpriteFont`
4. ? **`Game1.cs`** - Pasar `Content` al constructor de `PantallaXNA`

## ??? Archivos Obsoletos

Estos archivos ya NO se usan:
- ? `Fonts/franklin.dds` - Bitmap antiguo
- ? `Fonts/franklin.dds.xml` - Descriptor antiguo
- ? `XNAFont.cs` - Clase custom (ya no se usa)

Puedes eliminarlos o mantenerlos por referencia.

## ?? Para Agregar Más Fuentes

1. **Crear nuevo `.spritefont`**:
   ```bash
   cp Content/Franklin.spritefont Content/TituloFont.spritefont
   ```

2. **Editar** `TituloFont.spritefont`:
   ```xml
   <FontName>Impact</FontName>
   <Size>24</Size>
   <Style>Bold</Style>
   ```

3. **Agregar a Content.mgcb**:
   ```
   #begin TituloFont.spritefont
   /importer:FontDescriptionImporter
   /processor:FontDescriptionProcessor
   /build:TituloFont.spritefont
   ```

4. **Usar en código**:
   ```csharp
   SpriteFont tituloFont = Content.Load<SpriteFont>("TituloFont");
   spriteBatch.DrawString(tituloFont, "TÍTULO", position, Color.Yellow);
   ```

## ?? Best Practices

1. **Usa fuentes del sistema** comunes (Arial, Verdana, etc.) para compatibilidad
2. **Define solo los caracteres necesarios** para reducir tamaño del .xnb
3. **Crea fuentes separadas** para diferentes tamaños en lugar de escalar
4. **Incluye caracteres españoles** si tu app los usa (ñ, á, é, etc.)
5. **Cache las fuentes** - no las cargues en cada frame

## ?? Referencias

- [MonoGame SpriteFont Documentation](https://docs.monogame.net/articles/getting_started/4_adding_content.html)
- [Content Pipeline Tool](https://docs.monogame.net/articles/tools/mgcb.html)
- [Unicode Character Codes](https://www.unicode.org/charts/)

## ? Verificación

Después de estos cambios:

```powershell
dotnet build
dotnet run --project DarwinXNA.csproj
```

El texto debería mostrarse correctamente usando SpriteFont de MonoGame. ??
