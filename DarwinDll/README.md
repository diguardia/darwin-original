# DarwinDll – Motor de simulación

Biblioteca de clases que modela el ecosistema y la evolución de las especies.

## Rol en la solución
- Expone el modelo (`Universo`, `Terreno`, `Especie`, `SerVivo`/`Animal`/`Planta`) y la lógica de simulación (`Tick`).
- Implementa cerebros neuronales simples para individuos depredadores (`CerebroNeuronal`, `Hemisferio`, `Neurona`) y cerebros reactivos por vista (`AnimalVistaReaccion`).
- Gestiona serialización XML completa del universo para guardar/cargar partidas.

## Componentes clave
- `Universo`: orquesta el ciclo, mantiene tiempo, velocidad y regeneración del terreno; crea especies por defecto (Planta, Recolector herbívoro, Predador carnívoro, Neurodator con red neuronal, Fuego como peligro ambiental).
- `Terreno`: superficie de 900x768 con pozos/obstáculos; dibuja el fondo.
- `Especie`: define dieta (`TipoComida`), clase de los individuos y población mínima; mantiene colas de nacimiento y contadores de eficiencia.
- `SerVivo` y derivados:
  - `Planta`: recurso para herbívoros.
  - `AnimalVistaReaccion`: comportamiento guiado por percepción sencilla.
  - `AnimalNeuronal`: (proyecto original) controla acciones a partir de una red feed-forward.
- Acciones (`Accion*`), movimiento (`Vector`, `Punto`), colisiones (`Rectangulo`, `Pared`, `Pozo`), logging (`Log`), y eventos (`EventList<T>`).

## Flujo de simulación
1) `Universo.InicializarPorDefault()` crea el terreno, pozos y colas de individuos iniciales.
2) Cada `Tick()`:
   - Avanza `velocidad` veces el motor interno.
   - Ejecuta `tick()` de cada especie (movimiento, percepción, consumo, reproducción, muerte).
   - Reaparece población si cae por debajo del mínimo y puede heredar del individuo más eficiente.
   - Incrementa el contador de tiempo y opcionalmente regenera el terreno.

## Serialización
- `Universo.Serialize()` prepara las especies y las escribe en XML; `Deserialize` recompone referencias (pozos, especies, individuos) al cargar.
- `Guardar(fileName)` / `Cargar(fileName)` permiten persistir partidas desde cualquier host de la librería.

## Uso básico (código)
```csharp
var u = new DarwinDLL.Universo();
u.InicializarPorDefault();
for (int i = 0; i < 1000; i++) u.Tick();   // avanza la simulación
u.Guardar(\"save.xml\");                   // persistir
```

## Dependencias
- .NET Framework 3.5; namespaces estándar (`System`, `System.Drawing`, `System.Xml.Serialization`).
- Sin recursos gráficos propios; el renderizado lo provee el cliente (XNA u otro).
