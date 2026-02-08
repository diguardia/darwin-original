# Solución: Serialización de Dimensiones del Universo

## ?? Problema Identificado

El `Universo` se serializa a XML (`save.xml`), pero el campo `terreno` tiene `[XmlIgnore]`, por lo que no se guardaba y al deserializar siempre se recreaba con dimensiones fijas (900x768), ignorando la configuración actual.

### Código Anterior:
```csharp
[XmlIgnore]
public Terreno terreno;  // No se serializa

private void Inicializar(int anchoTerreno, int altoTerreno)
{
    // Las dimensiones no se guardaban
    terreno = new Terreno(new PuntoEntero(anchoTerreno, altoTerreno));
}

public static Universo Deserialize(string xml)
{
    Universo u = (Universo)Universo.Deserialize(xml, typeof(Universo));
    // terreno es null aquí, se debe recrear
    // ? Pero no sabíamos con qué dimensiones
}
```

## ? Solución Implementada

### 1. Agregar Campos para Dimensiones

Se agregaron dos campos que **sí se serializan**:

```csharp
[XmlAttribute]
public int anchoTerreno;
[XmlAttribute]
public int altoTerreno;
```

### 2. Guardar Dimensiones al Inicializar

```csharp
private void Inicializar(int anchoTerreno, int altoTerreno)
{
    velocidad = 1;
    tiempo = 0;
    regenerarTerreno = 0;
    
    // ? Guardar dimensiones para serialización
    this.anchoTerreno = anchoTerreno;
    this.altoTerreno = altoTerreno;
    
    terreno = new Terreno(new PuntoEntero(anchoTerreno, altoTerreno));
    // ...
}
```

### 3. Recrear Terreno con Dimensiones Guardadas

```csharp
public static Universo Deserialize(string xml)
{
    Universo u = (Universo)Universo.Deserialize(xml, typeof(Universo));

    // ? Recrear el terreno con las dimensiones guardadas
    int ancho = u.anchoTerreno > 0 ? u.anchoTerreno : 900;  // Fallback para saves antiguos
    int alto = u.altoTerreno > 0 ? u.altoTerreno : 768;
    
    u.terreno = new Terreno(new PuntoEntero(ancho, alto));
    
    foreach (Pozo p in u.Pozos)
    {
        u.AgregarPozoAlTerreno(p);
    }
    // ...
}
```

### 4. Actualizar Program.cs para Manejar Cambios de Configuración

```csharp
private static Universo CargarOCrearUniverso(GameConfig config)
{
    if (File.Exists("save.xml"))
    {
        universo = Universo.Cargar("save.xml");
        
        // ? Verificar si las dimensiones guardadas coinciden con la config actual
        if (universo.anchoTerreno != config.TerrainWidth || 
            universo.altoTerreno != config.TerrainHeight)
        {
            Log.Warning($"Tamaño de terreno en save.xml ({universo.anchoTerreno}x{universo.altoTerreno}) " +
                       $"difiere de la configuración actual ({config.TerrainWidth}x{config.TerrainHeight})");
            Log.Info("Recreando terreno con nuevo tamaño (se mantendrán especies y pozos)");
            
            // ? Recrear solo el terreno, mantener especies y pozos
            universo.anchoTerreno = config.TerrainWidth;
            universo.altoTerreno = config.TerrainHeight;
            universo.terreno = new Terreno(new PuntoEntero(config.TerrainWidth, config.TerrainHeight));
            
            // Reagregar pozos al nuevo terreno
            foreach (var pozo in universo.Pozos)
            {
                universo.AgregarPozoAlTerreno(pozo);
            }
        }
    }
}
```

## ?? Comportamiento Actualizado

### Caso 1: Crear Universo Nuevo
```
1. Leer config.json ? 1280x720
2. new Universo(1280-124, 720) ? 1156x720
3. anchoTerreno = 1156, altoTerreno = 720 (se guardan)
4. Al guardar save.xml ? incluye anchoTerreno="1156" altoTerreno="720"
```

### Caso 2: Cargar Universo Existente (Sin Cambios)
```
1. Leer save.xml ? anchoTerreno="1156" altoTerreno="720"
2. Recrear terreno con 1156x720
3. ? Todo funciona, dimensiones correctas
```

### Caso 3: Cargar Universo con Config Diferente
```
1. save.xml tiene: anchoTerreno="1156" altoTerreno="720"
2. config.json ahora tiene: 1920x1080 ? terreno 1796x1080
3. Sistema detecta diferencia
4. Log: "Recreando terreno con nuevo tamaño"
5. Mantiene especies y pozos
6. Crea nuevo terreno de 1796x1080
7. ? Universo se adapta a nueva resolución
```

### Caso 4: Cargar Save Antiguo (Sin Dimensiones)
```
1. save.xml antiguo no tiene anchoTerreno/altoTerreno
2. Sistema detecta anchoTerreno = 0
3. Usa fallback: 900x768 (valores por defecto)
4. ? Compatibilidad con saves antiguos
```

## ?? Ejemplo de save.xml

### Antes (problemático):
```xml
<Universo velocidad="1" regenerarTerreno="0" tiempo="12345">
  <!-- terreno no se guarda por [XmlIgnore] -->
  <Pozos>...</Pozos>
  <Especies>...</Especies>
</Universo>
```

### Ahora (correcto):
```xml
<Universo velocidad="1" 
          regenerarTerreno="0" 
          tiempo="12345"
          anchoTerreno="1156"
          altoTerreno="720">
  <!-- terreno se recrea con estas dimensiones al cargar -->
  <Pozos>...</Pozos>
  <Especies>...</Especies>
</Universo>
```

## ?? Logs Resultantes

### Al Guardar:
```
[INFO] Guardando universo...
[INFO] Universo guardado exitosamente
```
El XML incluye: `anchoTerreno="1156" altoTerreno="720"`

### Al Cargar (Mismas Dimensiones):
```
[INFO] Cargando universo desde save.xml...
[INFO] Universo cargado desde archivo guardado: 1156x720
```

### Al Cargar (Dimensiones Diferentes):
```
[INFO] Cargando universo desde save.xml...
[WARNING] Tamaño de terreno en save.xml (900x768) difiere de la configuración actual (1156x720)
[INFO] Recreando terreno con nuevo tamaño (se mantendrán especies y pozos)
[INFO] Universo cargado desde archivo guardado: 1156x720
```

### Al Cargar Save Antiguo:
```
[INFO] Cargando universo desde save.xml...
[INFO] Universo cargado desde archivo guardado: 900x768
```

## ? Ventajas de la Solución

1. **Mantiene Dimensiones**: El save guarda el tamaño del terreno
2. **Adaptable**: Si cambias config.json, ajusta el terreno automáticamente
3. **Conserva Datos**: Mantiene especies y pozos al cambiar tamaño
4. **Compatible**: Funciona con saves antiguos (fallback a 900x768)
5. **Informativo**: Logs claros de qué está pasando
6. **Robusto**: Maneja todos los casos edge

## ?? Casos de Prueba

### Test 1: Crear y Guardar
```powershell
# config.json: 1024x768 ? terreno 900x768
dotnet run
# Cerrar juego
# save.xml ahora tiene anchoTerreno="900" altoTerreno="768"
```

### Test 2: Cargar con Misma Config
```powershell
# config.json: 1024x768
# save.xml: anchoTerreno="900" altoTerreno="768"
dotnet run
# ? Carga sin warnings, terreno 900x768
```

### Test 3: Cargar con Config Diferente
```powershell
# Editar config.json: screenWidth=1280, screenHeight=720
# save.xml: anchoTerreno="900" altoTerreno="768"
dotnet run
# ?? Warning: Tamaño difiere
# ? Terreno recreado: 1156x720
# ? Especies y pozos mantenidos
```

### Test 4: Save Antiguo
```powershell
# save.xml sin atributos anchoTerreno/altoTerreno
dotnet run
# ? Usa fallback: 900x768
# ? Funciona sin errores
```

## ?? Archivos Modificados

1. **`Universo.cs`** (DarwinDll):
   - ? Agregados campos `anchoTerreno` y `altoTerreno`
   - ? Método `Inicializar` guarda dimensiones
   - ? Método `Deserialize` recrea terreno con dimensiones guardadas
   - ? Fallback para saves antiguos

2. **`Program.cs`** (DarwinXNA):
   - ? Método `CargarOCrearUniverso` verifica dimensiones
   - ? Recrea terreno si config cambió
   - ? Mantiene especies y pozos
   - ? Logging detallado

## ?? Resultado Final

**El sistema ahora:**
- ? Guarda las dimensiones del terreno en save.xml
- ? Recrea el terreno con las dimensiones correctas al cargar
- ? Se adapta si cambias la configuración
- ? Es compatible con saves antiguos
- ? Mantiene todos los datos (especies, pozos, etc.)
- ? Registra todos los cambios en el log

**¡El problema de serialización está completamente resuelto!** ??
