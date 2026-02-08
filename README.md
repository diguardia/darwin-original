# Darwin Original

Simulador evolutivo en 2D donde especies con pequeños cerebros neuronales compiten por sobrevivir. Migrado desde XNA 2.0 a **MonoGame 3.8 DesktopGL** sobre **.NET 6 (windows)**.

## Estructura
- `DarwinDll/`: motor de simulación (biblioteca net6.0-windows).
- `DarwinXNA/`: cliente gráfico basado en MonoGame DesktopGL que referencia el motor.

## Requisitos
- SDK de .NET 6 (o superior compatible con MonoGame 3.8).
- Paquete NuGet `MonoGame.Framework.DesktopGL` (se restaura con `dotnet restore`).
- Windows con soporte para OpenGL/SDL que trae MonoGame.

## Ejecutar rápido
```bash
dotnet restore
dotnet build DarwinXNA/DarwinXNA.csproj
dotnet run --project DarwinXNA/DarwinXNA.csproj
```
El run abrirá la ventana del simulador; `Esc` para salir.

**Si el programa termina inmediatamente**, revisa la sección [Diagnóstico y Logs](#diagnóstico-y-logs) más abajo.

**Si ves error de formato de imagen**, consulta [DarwinXNA/DDS_MIGRATION_GUIDE.md](DarwinXNA/DDS_MIGRATION_GUIDE.md).

## Mecánica principal
- El `Universo` mantiene el `Terreno`, pozos/obstáculos y una lista de `Especies`.
- Cada especie define la clase de sus individuos (plantas, recolectores, depredadores, depredadores neuronales, fuego) y su dieta.
- En cada `Tick()`:
  - Se actualiza cada especie (movimiento, consumo, reproducción, muerte).
  - Se regeneran recursos y se mantiene una población mínima.
  - Se guarda/recarga el estado según atajos del cliente (teclas `S`/`R`).
- El motor serializa/deserializa el universo para persistir partidas (`save.xml`).

## Control básico en el cliente XNA
- `N`: reinicia un universo con valores por defecto.
- `S`: guarda el estado.
- `R`: recarga desde el estado serializado en memoria.
- `F`: alterna pantalla completa.
- Click izquierdo: reposiciona el seguidor/visor.

## Diagnóstico y Logs

### Leer los logs
Si el programa termina inesperadamente o hay errores, consulta el archivo de logs:

- **Ubicación**: `log.txt` en el directorio donde se ejecuta el proyecto (normalmente `DarwinXNA/bin/Debug/net10.0/` o la carpeta raíz si ejecutas con `dotnet run`).
- **Formato**: Logs estructurados con timestamp, nivel y mensaje:
  ```
  [2026-02-08 16:30:45.123] [INFO   ] Iniciando aplicación Darwin XNA
  [2026-02-08 16:30:45.456] [ERROR  ] Error al cargar textura
  ```
- **Niveles**: DEBUG, INFO, WARNING, ERROR, FATAL
- **Contenido**: Registra excepciones, errores y mensajes de depuración.
- **Cómo revisar**:
  ```bash
  # Usando el script de ayuda (Windows PowerShell)
  .\DarwinXNA\ViewLogs.ps1           # Ver últimas 50 líneas
  .\DarwinXNA\ViewLogs.ps1 -Lines 100  # Ver últimas 100 líneas
  .\DarwinXNA\ViewLogs.ps1 -Follow     # Seguir en tiempo real
  
  # O manualmente
  cat log.txt
  
  # Filtrar solo errores
  cat log.txt | grep ERROR
  
  # Seguir el log en tiempo real (Linux/Mac)
  tail -f log.txt
  
  # En Windows PowerShell
  Get-Content log.txt -Tail 50
  Get-Content log.txt -Wait  # para seguir en tiempo real
  Get-Content log.txt | Select-String "ERROR"  # solo errores
  ```

### Problemas comunes
- **El juego se cierra inmediatamente**: Revisa `log.txt` para ver si hay excepciones de archivos faltantes (imágenes, fuentes) o errores de inicialización de MonoGame.
- **Error "This image format is not supported"**: MonoGame no soporta archivos `.dds`. Para imágenes, ejecuta `DarwinXNA/ConvertDDStoPNG.ps1`. Para fuentes, usa SpriteFont (ya implementado). Ver [README de DarwinXNA](DarwinXNA/README.md#fuentes-en-monogame).
- **Error de OpenGL/SDL**: Asegúrate de tener controladores gráficos actualizados y que MonoGame DesktopGL esté correctamente instalado.
- **Archivo save.xml corrupto**: Elimina `save.xml` para forzar la creación de un universo nuevo.
- **No se genera log.txt**: Verifica permisos de escritura en el directorio de ejecución.

## Árbol del repositorio
- `DarwinDll/` motor y lógica evolutiva.
- `DarwinXNA/` juego y assets (imágenes, fuentes, sonido).
- `.gitignore` configurado para artefactos de .NET/MonoGame.
- `log.txt` archivo de logs generado en tiempo de ejecución (no versionado).
