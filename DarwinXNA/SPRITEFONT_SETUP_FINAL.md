# Configuración Final de SpriteFont - INSTRUCCIONES

## ? Todo está configurado correctamente

Los cambios están hechos. Sigue estos pasos para ejecutar:

### Paso 1: Compilar el contenido (solo una vez)

```powershell
# Navega a la carpeta Content
cd Content

# Compila el .spritefont a .xnb
mgcb .\Content.mgcb /platform:DesktopGL

# Verifica que se creó Franklin.xnb
ls bin\DesktopGL\
```

Deberías ver: `Franklin.xnb` en `Content\bin\DesktopGL\`

### Paso 2: Compilar y ejecutar el proyecto

```powershell
# Vuelve a la carpeta del proyecto
cd ..

# Compila
dotnet build

# Ejecuta
dotnet run
```

## ?? Cuando cambies la fuente

Si modificas `Content/Franklin.spritefont` (cambiar tamaño, fuente, etc.):

```powershell
# 1. Recompila el contenido
cd Content
mgcb .\Content.mgcb /platform:DesktopGL

# 2. Recompila el proyecto
cd ..
dotnet build
```

## ?? Estructura de Archivos

```
DarwinXNA/
??? Content/
?   ??? Franklin.spritefont         ? Definición de fuente
?   ??? Content.mgcb                ? Configuración del Content Pipeline
?   ??? bin/
?       ??? DesktopGL/
?           ??? Franklin.xnb        ? Archivo compilado (se copia automáticamente)
?
??? bin/
?   ??? Debug/
?       ??? net10.0-windows7.0/
?           ??? Content/
?               ??? bin/
?                   ??? DesktopGL/
?                       ??? Franklin.xnb  ? Archivo en el directorio de salida
?
??? Game1.cs                        ? Configurado con Content.RootDirectory
??? PantallaXNA.cs                  ? Usa SpriteFont
??? DarwinXNA.csproj               ? Copia automáticamente los .xnb
```

## ?? Configuración Actual

### Game1.cs
```csharp
Content.RootDirectory = "Content/bin/DesktopGL";
```

### DarwinXNA.csproj
```xml
<Content Include="Content\bin\DesktopGL\*.xnb">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  <Link>Content\bin\DesktopGL\%(Filename)%(Extension)</Link>
</Content>
```

### PantallaXNA.cs
```csharp
textWriter = content.Load<SpriteFont>("Franklin");
```

## ?? Importante

- **No uses** `MonoGame.Content.Builder.Task` - tiene problemas con .NET 10
- **Compila manualmente** el contenido con `mgcb` cuando cambies fonts
- El proyecto **copia automáticamente** los .xnb compilados al output

## ?? Solución de Problemas

### Error: "Franklin.xnb not found"

1. Verifica que compilaste el contenido:
   ```powershell
   cd Content
   mgcb .\Content.mgcb /platform:DesktopGL
   ```

2. Verifica que existe el archivo:
   ```powershell
   Test-Path Content\bin\DesktopGL\Franklin.xnb
   ```

3. Si no existe, hay un error en `Franklin.spritefont` - revisa el output de `mgcb`

### Error al compilar contenido

```powershell
# Ver errores detallados
mgcb .\Content.mgcb /platform:DesktopGL
```

Errores comunes:
- **Fuente no encontrada**: Cambia `<FontName>` a una fuente instalada (Arial, Verdana)
- **XML mal formado**: Verifica que no hay comentarios HTML mal puestos
- **Caracteres inválidos**: Verifica `<CharacterRegions>`

### Fuente no instalada

Si ves: `Cannot find font 'Franklin Gothic Medium'`

Edita `Content/Franklin.spritefont`:
```xml
<FontName>Arial</FontName>
```

Luego recompila:
```powershell
cd Content
mgcb .\Content.mgcb /platform:DesktopGL
cd ..
dotnet build
```

## ? Verificación Final

```powershell
# 1. Compilar contenido
cd Content
mgcb .\Content.mgcb /platform:DesktopGL

# 2. Verificar que se creó
ls bin\DesktopGL\Franklin.xnb

# 3. Volver y compilar
cd ..
dotnet build

# 4. Ejecutar
dotnet run
```

Si todo está bien, el juego debería iniciar y mostrar texto con SpriteFont. ??
