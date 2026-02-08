# MonoGame y el Problema de la Consola

## ?? El Problema

Cuando migras de una aplicación de consola o de XNA, podrías encontrar este error:

```
System.InvalidOperationException: Cannot read keys when either application 
does not have a console or when console input has been redirected. 
Try Console.Read.
```

## ?? ¿Por qué pasa esto?

MonoGame genera **aplicaciones de ventana** (WinExe), no aplicaciones de consola:

```xml
<!-- En el .csproj -->
<OutputType>WinExe</OutputType>
```

Esto significa:
- ? No hay consola visible
- ? `Console.ReadKey()` falla
- ? `Console.WriteLine()` no se ve
- ? `Console.Read()` falla

## ? La Solución

### Antes (? No funciona en MonoGame):
```csharp
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
    Console.WriteLine("Presiona una tecla para salir...");
    Console.ReadKey();  // ? FALLA: InvalidOperationException
}
```

### Después (? Funciona):
```csharp
catch (Exception ex)
{
    // 1. Registrar en log
    Log.Exception(ex, "Error en la aplicación");
    
    // 2. Mostrar al usuario
    MessageBox.Show(
        $"Error: {ex.Message}\n\nRevisa log.txt para más detalles.",
        "Error de Aplicación",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    );
}
```

## ?? Alternativas para Mostrar Información

### Para errores críticos:
```csharp
using System.Windows.Forms;

MessageBox.Show("Mensaje", "Título", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### Para logs de depuración:
```csharp
// Escribir en archivo
Log.Debug("Variable x = " + x);
Log.Info("Proceso completado");

// Luego revisar con:
.\ViewLogs.ps1
```

### Para debugging durante desarrollo:
```csharp
// Usar Output Window de Visual Studio
System.Diagnostics.Debug.WriteLine("Debug info: " + value);
```

### Para aplicación con consola (solo desarrollo):
```csharp
// En .csproj, cambiar temporalmente:
<OutputType>Exe</OutputType>  <!-- En lugar de WinExe -->

// ¡Recuerda revertir a WinExe para release!
```

## ?? Proyecto Darwin - Solución Implementada

En este proyecto ya está todo corregido:

### 1. **Log System** (DarwinDll\Log.cs)
```csharp
// No usa Console - escribe solo a archivo
Log.Info("Mensaje");
Log.Error("Error");
Log.Exception(ex, "Contexto");
```

### 2. **Error Handling** (Program.cs)
```csharp
try 
{
    // ... código
}
catch (Exception ex)
{
    Log.Exception(ex, "Error no controlado");
    MessageBox.Show(/* mensaje amigable */);
    // NO usa Console.ReadKey()
}
```

### 3. **Ver logs**
```powershell
# Script que busca y muestra log.txt
.\ViewLogs.ps1
.\ViewLogs.ps1 -Level ERROR
```

## ?? Comparación

| Método | Consola App | MonoGame (WinExe) |
|--------|-------------|-------------------|
| `Console.WriteLine()` | ? Funciona | ? No se ve |
| `Console.ReadKey()` | ? Funciona | ? Falla |
| `MessageBox.Show()` | ?? Funciona pero raro | ? Ideal |
| `Log.escribir()` | ? Funciona | ? Funciona |
| `Debug.WriteLine()` | ? Visual Studio | ? Visual Studio |

## ?? Best Practices para MonoGame

1. **Logs en archivo** ? Para diagnóstico completo
2. **MessageBox** ? Para errores críticos que el usuario debe ver
3. **Debug.WriteLine()** ? Para debugging durante desarrollo
4. **NO usar Console** ? Nunca en aplicaciones MonoGame finales

## ?? Tips

### Durante desarrollo:
```csharp
// En Debug builds, puedes usar:
#if DEBUG
    System.Diagnostics.Debug.WriteLine("Info de debug");
#endif
```

### Para release:
```csharp
// Siempre usar logs en archivo
Log.Info("Aplicación iniciada");

// Y MessageBox para errores críticos
MessageBox.Show("Error crítico", "Error");
```

### Para debugging intenso:
```csharp
// Puedes crear una pequeña consola visual en el juego
SpriteBatch.DrawString(font, debugInfo, position, Color.White);
```

## ?? Referencias

- [MonoGame Documentation](https://docs.monogame.net/)
- [WinExe vs Exe OutputType](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#outputtype)
- [Console in Windows Forms](https://stackoverflow.com/questions/4362111)

## ? Verificación

Si ves este error, significa que el código está intentando usar la consola:
```
InvalidOperationException: Cannot read keys when either application 
does not have a console...
```

**Solución**: Reemplaza todos los `Console.ReadKey()` y `Console.WriteLine()` críticos con `MessageBox.Show()` y `Log.escribir()`.

¡Este proyecto ya tiene la solución implementada! ??
