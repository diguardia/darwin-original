# Mejoras de Logging para Carga de Imágenes

## ?? Problema Resuelto

**Antes**: Cuando una imagen fallaba, solo veías:
```
EXCEPCIÓN: InvalidOperationException
MENSAJE: This image format is not supported
```

**No sabías**:
- ¿Qué imagen estaba intentando cargar?
- ¿Qué archivo encontró?
- ¿Qué formato tenía?

## ? Solución Implementada

### Logging Detallado en `LoadTexture`

Ahora el log muestra TODO el proceso:

#### 1. Al intentar cargar:
```
[2026-02-08 17:30:45.123] [DEBUG  ] Intentando cargar imagen: 'Neurodator'
```

#### 2. Al encontrar archivo:
```
[2026-02-08 17:30:45.145] [INFO   ] Cargando imagen: Neurodator.dds (formato: .dds)
[2026-02-08 17:30:45.146] [WARNING] Formato '.dds' puede no ser compatible con MonoGame. Convierte 'Neurodator.dds' a PNG.
```

#### 3. Si falla:
```
[2026-02-08 17:30:45.234] [ERROR  ] ERROR al cargar imagen 'Neurodator.dds' (clave: 'Neurodator')
Ruta completa: C:\...\imagenes\Neurodator.dds
Formato: .dds
Tamaño archivo: 184 bytes

?? FORMATO DDS NO SOPORTADO ??
MonoGame no puede cargar archivos .dds
SOLUCIÓN: Convierte 'Neurodator.dds' a PNG:
  1. Ejecuta: .\ConvertDDStoPNG.ps1
  2. O usa: https://www.aconvert.com/image/dds-to-png/
```

#### 4. Si carga exitosamente:
```
[2026-02-08 17:30:45.456] [DEBUG  ] Imagen 'casa.jpg' cargada exitosamente (256x256)
[2026-02-08 17:30:45.457] [DEBUG  ] Sprite 'casa' agregado al cache
```

## ?? Información Capturada

Para cada imagen, el log ahora muestra:

1. **Clave solicitada**: Qué imagen pidió el código
2. **Archivo encontrado**: Qué archivo específico se encontró
3. **Formato**: Extensión del archivo (.dds, .png, etc.)
4. **Tamaño**: Tamaño del archivo en bytes
5. **Dimensiones**: Ancho x Alto (si carga exitosamente)
6. **Advertencias**: Si el formato puede ser problemático
7. **Solución**: Instrucciones claras si falla

## ?? Niveles de Log Usados

- **DEBUG**: Intentos de carga y éxitos (útil para debugging)
- **INFO**: Archivos cargados exitosamente
- **WARNING**: Formatos que pueden causar problemas (.dds, .tga)
- **ERROR**: Fallos de carga con información completa

## ??? Scripts de Ayuda

### CheckImageFormats.ps1
Analiza la carpeta `imagenes` y reporta:
- ? Archivos soportados (PNG, JPG, BMP, GIF)
- ?? Archivos riesgosos (TGA)
- ? Archivos no soportados (DDS)

```powershell
.\CheckImageFormats.ps1
```

### ConvertDDStoPNG.ps1
Convierte automáticamente todos los archivos DDS a PNG:

```powershell
.\ConvertDDStoPNG.ps1
```

## ?? Ejemplo de Sesión de Log

```
[2026-02-08 17:30:42.123] [INFO   ] Iniciando aplicación Darwin XNA
[2026-02-08 17:30:42.234] [DEBUG  ] Intentando cargar imagen: 'Punto'
[2026-02-08 17:30:42.235] [INFO   ] Cargando imagen: punto.bmp (formato: .bmp)
[2026-02-08 17:30:42.256] [DEBUG  ] Imagen 'punto.bmp' cargada exitosamente (1x1)
[2026-02-08 17:30:42.257] [DEBUG  ] Sprite 'Punto' agregado al cache
[2026-02-08 17:30:42.345] [DEBUG  ] Intentando cargar imagen: 'Neurodator'
[2026-02-08 17:30:42.346] [INFO   ] Cargando imagen: Neurodator.dds (formato: .dds)
[2026-02-08 17:30:42.347] [WARNING] Formato '.dds' puede no ser compatible con MonoGame. Convierte 'Neurodator.dds' a PNG.
[2026-02-08 17:30:42.456] [ERROR  ] ERROR al cargar imagen 'Neurodator.dds' (clave: 'Neurodator')
Ruta completa: C:\Users\cguar\repos\darwin-original\DarwinXNA\imagenes\Neurodator.dds
Formato: .dds
Tamaño archivo: 184 bytes

?? FORMATO DDS NO SOPORTADO ??
MonoGame no puede cargar archivos .dds
SOLUCIÓN: Convierte 'Neurodator.dds' a PNG:
  1. Ejecuta: .\ConvertDDStoPNG.ps1
  2. O usa: https://www.aconvert.com/image/dds-to-png/
```

## ?? Beneficios

1. **Diagnóstico rápido**: Sabes inmediatamente qué imagen está fallando
2. **Formato claro**: El error incluye ruta completa y solución
3. **Advertencias preventivas**: Te avisa si un formato puede ser problemático
4. **Contexto completo**: Clave solicitada + archivo encontrado
5. **Scripts automatizados**: Herramientas para detectar y convertir

## ?? Flujo de Trabajo Recomendado

### Primera Ejecución:
```powershell
# 1. Verificar qué imágenes hay
.\CheckImageFormats.ps1

# 2. Convertir las problemáticas
.\ConvertDDStoPNG.ps1

# 3. Ejecutar el juego
dotnet run

# 4. Si hay errores, revisar log
.\ViewLogs.ps1 -Level ERROR
```

### Si hay un error nuevo:
```powershell
# Ver logs con filtro de errores
.\ViewLogs.ps1 -Level ERROR

# El log te dirá exactamente:
# - Qué imagen falló
# - Qué formato tiene
# - Cómo solucionarlo
```

## ?? Formatos de Imagen

### ? Soportados por MonoGame:
- **PNG** - Recomendado (transparencia, sin pérdida)
- **JPG** - Bueno para fotos (sin transparencia)
- **BMP** - Soportado (archivos grandes)
- **GIF** - Soportado (sin animación)

### ? NO Soportados:
- **DDS** - DirectDraw Surface (formato antiguo de XNA)
- **TGA** - Targa (puede no funcionar)

## ?? Archivos Modificados

1. **`PantallaXNA.cs`**:
   - `LoadTexture()` - Logging completo de carga
   - `CargarImagen()` - Manejo de excepciones mejorado

2. **`CheckImageFormats.ps1`** (nuevo):
   - Detecta formatos problemáticos
   - Reporta estadísticas

3. **`ConvertDDStoPNG.ps1`** (mejorado):
   - Busca recursivamente
   - Reporta progreso
   - Muestra resumen

## ? Resultado

**Ya no más "unknown image type" sin contexto!**

Ahora cuando hay un error, sabes:
- ? Qué imagen
- ? Qué archivo
- ? Qué formato
- ? Cómo solucionarlo

?? **Debugging simplificado!**
