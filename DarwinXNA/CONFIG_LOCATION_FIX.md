# Ubicación del config.json - Solución

## ?? El Problema

Estabas editando `config.json` en la carpeta del proyecto:
```
DarwinXNA/config.json  ? Editabas aquí
```

Pero el juego se ejecuta desde:
```
DarwinXNA/bin/Debug/net10.0-windows7.0/  ? El juego busca aquí
```

Por eso los cambios no se aplicaban.

## ? La Solución

Se implementaron dos soluciones:

### 1. Copia Automática al Compilar

El archivo `.csproj` ahora tiene un target que copia `config.json` al directorio de salida **solo si no existe**:

```xml
<Target Name="CopyConfigIfNotExists" AfterTargets="Build">
  <Copy SourceFiles="config.json" 
        DestinationFiles="$(OutputPath)config.json" 
        Condition="!Exists('$(OutputPath)config.json')" />
</Target>
```

Esto significa:
- Primera vez: Se copia automáticamente
- Siguientes compilaciones: **NO sobrescribe** (preserva tu configuración)

### 2. Limpieza de Comentarios en JSON

El parser de JSON ahora elimina las líneas con comentarios `//` antes de parsear, así funciona correctamente.

## ?? Dónde Editar config.json

Tienes **dos opciones**:

### Opción A: Editar en el directorio de salida (Recomendado)
```
DarwinXNA/bin/Debug/net10.0-windows7.0/config.json
```
? **Ventaja**: Los cambios se aplican inmediatamente
? **Ventaja**: No se sobrescribe al compilar

### Opción B: Editar en el proyecto y copiar
```
DarwinXNA/config.json
```
Luego ejecuta:
```powershell
Copy-Item config.json bin\Debug\net10.0-windows7.0\config.json -Force
dotnet run
```

## ?? Flujo Recomendado

```powershell
# 1. Edita config.json en el directorio de salida
notepad bin\Debug\net10.0-windows7.0\config.json

# 2. Ejecuta el juego
dotnet run

# Los cambios se aplican inmediatamente
```

## ?? Estructura de Archivos

```
DarwinXNA/
??? config.json              ? Plantilla (se copia primera vez)
??? config.json.example      ? Ejemplo documentado
??? bin/
    ??? Debug/
        ??? net10.0-windows7.0/
            ??? config.json  ? EL ARCHIVO QUE USA EL JUEGO ?
            ??? DarwinXNA.exe
            ??? log.txt
            ??? save.xml
```

## ?? Tu Configuración Actual

Ya copié tu configuración (1600x900) al directorio correcto:

```json
{
  "screenWidth": 1600,
  "screenHeight": 900,
  "vsync": true,
  "defaultSpeed": 1
}
```

**Resultado esperado**:
- Ventana: 1600 x 900
- Terreno: 1476 x 900 (1600 - 124 = 1476)

## ?? Verificación

Para verificar qué configuración está usando el juego:

```powershell
# Ver el archivo que usa el juego
Get-Content bin\Debug\net10.0-windows7.0\config.json

# Ver el log después de ejecutar
.\ViewLogs.ps1 | Select-String "Configuración"
```

Deberías ver en el log:
```
[INFO] Configuración cargada: 1600x900
[INFO] Dimensiones calculadas - Terreno: 1476x900, Panel: 124px
```

## ?? Tips

1. **No edites el del proyecto** después de la primera vez - no se copia automáticamente para no sobrescribir tu config personalizado

2. **Si quieres resetear** a valores por defecto:
   ```powershell
   Remove-Item bin\Debug\net10.0-windows7.0\config.json
   dotnet run  # Se crea uno nuevo
   ```

3. **Para debugging**: Siempre revisa `log.txt` para ver qué configuración se cargó:
   ```powershell
   .\ViewLogs.ps1 | Select-String "Configuración"
   ```

## ?? Nota Importante

El `config.json` con comentarios `//` no es JSON estándar, pero el código ahora los limpia antes de parsear, así que funciona correctamente.

Si prefieres JSON puro sin comentarios:
```json
{
  "screenWidth": 1600,
  "screenHeight": 900,
  "fullscreen": false,
  "vsync": true,
  "defaultSpeed": 1
}
```

## ? Resumen

**Problema resuelto:**
- ? `config.json` se copia automáticamente al compilar (primera vez)
- ? No se sobrescribe en compilaciones posteriores
- ? Parser limpia comentarios antes de leer
- ? Tu configuración 1600x900 ya está en el lugar correcto

**Ahora ejecuta `dotnet run` y debería usar 1600x900!** ??
