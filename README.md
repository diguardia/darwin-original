# Darwin Original

Simulador evolutivo en 2D donde especies con pequeños cerebros neuronales compiten por sobrevivir. La solución se divide en:

- `DarwinDll/`: motor de simulación (biblioteca .NET 2.0/3.5).
- `DarwinXNA/`: cliente gráfico y de audio basado en XNA 2.0 para Windows que referencia el motor.

## Requisitos
- Visual Studio 2008/2010 con .NET Framework 3.5.
- Microsoft XNA Game Studio 2.0 (Runtime + Content Pipeline).

## Ejecutar rápido
1) Abrir `DarwinXNA/DarwinXNA.sln`.
2) Restaurar XNA 2.0 si el IDE lo solicita.
3) Compilar en `Debug|x86` y ejecutar `DarwinXNA`.

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

## Árbol del repositorio
- `DarwinDll/` motor y lógica evolutiva.
- `DarwinXNA/` juego y assets (imágenes, fuentes, sonido).
- `.gitignore` configurado para artefactos de Visual Studio/XNA.
