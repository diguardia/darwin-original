# DarwinXNA – Cliente XNA 2.0

Frontend Windows construido con Microsoft XNA Game Studio 2.0. Consume el motor `DarwinDll` y dibuja/sonoriza la simulación.

## Estructura
- `DarwinXNA.csproj`: ejecutable `WinExe` dirigido a `x86`, referencia `DarwinDll`.
- `Content/`: proyecto de contenido anidado (`Content.contentproj`) con sprites y sonidos.
- `imagenes/`, `Fonts/`, `Win/`: assets para texturas, tipografías y bancos de sonido.

## Controles en tiempo de ejecución
- `N`: crear un nuevo universo con especies y pozos por defecto.
- `S`: guardar el estado actual a `save.xml`.
- `R`: recargar el estado serializado en memoria.
- `F`: alternar pantalla completa.
- Click izquierdo: mover el `Seguidor` para inspeccionar otro punto del mapa.
- `Esc`: salir.

## Ciclo del juego
- `Game1.Update`: delega en `Universo.Tick()`, mueve el seguidor y gestiona atajos de guardado/recarga.
- `Game1.Draw`: limpia el backbuffer, pinta el terreno, dibuja cada especie, panel lateral con estadísticas (tiempo, timers, conteo y eficiencia por especie) y reproduce la cola de sonidos.
- Ventanas auxiliares (`Ventana` y `VentanaSeleccion`) muestran detalles y texto con `XNAFont`.

## Compilación y ejecución
1) Instalar Microsoft XNA Game Studio 2.0 y runtime.
2) Abrir `DarwinXNA.sln` en Visual Studio 2008/2010.
3) Seleccionar configuración `Debug|x86`.
4) Compilar y ejecutar. Los assets se copian desde `Content` y `imagenes`.

## Notas
- El proyecto se diseñó para resolución 1024x768; puede ajustarse vía `GraphicsDeviceManager` en `Game1`.
- Los sonidos están referenciados pero comentados; requiere reinstalar los bancos de audio (`Win/*.xgs`, `.xwb`, `.xsb`) para habilitarlos.
