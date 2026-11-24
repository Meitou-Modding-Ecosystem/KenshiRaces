<a id="kenshi-races"></a>
# Kenshi Las carreras

Las nuevas carreras para [MeitouCliente](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) en los esqueletos humanos del juego, jugable en el editor de personajes: Leoma y Crevan (mujer y mujer) y el Crabfolk (crabman, crabqueen), una biblioteca de 59 clips para cada raza humana, y el marco de otros modos añade sus propias razas y subrasas. GPL0 0 o más tarde.

- Las carreras, sus registros y probarlos en el juego: [docs/kenshi-races.md](docs/kenshi-races.md).
- Usar estas carreras o añadir tu propio de otro modo: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (Capacidad de agente
`.claude/skills/kenshi-races-dependent`).
- La biblioteca de animación (clips de 59 para cada raza en los esqueletos humanos) y cómo se hizo: [docs/animations.md](docs/animations.md).
- Fijando una rigida `.glb` El cuerpo (Meshy, Hi3D "3ds Max Biped") en el esqueleto del juego: [docs/race-import.md](docs/race-import.md).

- Construye y ejecuta localmente con Visual Studio, VS Code o el terminal (el motor de este repo o el lanzador instalado, desplazado al juego): [docs/play.md](docs/play.md).
- Instalar el reposo en tu máquina: [docs/local-development.md](docs/local-development.md).
- Instrucción de agentes sobre un cambio que descansa: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- Las reglas del agente para el pin, las versiones, los lanzamientos: [CLAUDE.md](../../CLAUDE.md) (También como `AGENTS.md`• Procedimientos de paso a paso:
`.claude/skills/meitou-engine-release`.
- Verificar un modo construido contra el último lanzamiento de Meitou (CI lo hace en cada PR): [docs/refcheck.md](docs/refcheck.md).
- Publicación del modo de trabajo de Steam por primera vez, luego en cada edición: [docs/steam-workshop.md](docs/steam-workshop.md).
- La vuelta `.glb` Los modelos (Meshy, Hi3D, Blender) en estructuras constructivas, no FCS Necesidad de: [docs/structures.md](docs/structures.md);
Obtener el polincuente, el tamaño y las texturas de un modelo generado primero: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## Números

Tipos de tareas (`AddGoalType`, `AddJobType`y los códigos de diálogo (`AddDialogueCondition`, `AddDialogueEffect`En el caso de que se trate de una de las más importantes ([El número de bloques](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi Las razas no reclaman nada.

| Número | Piscina | Uso |
| --- | --- | --- |

<a id="layout"></a>
## Layout

- `engine/`: MeitouClient Como un submodulo de git, encendido al motor se compromete a la construcción de modos en contra. `git pull` No se mueve: después de
Todos los pull run `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([Mantener los clones actuales](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`• El proyecto de modos y `meitou.json`; `KenshiRaces/items/races/`• Los organismos adheridos (productos de importación adheridos).
`KenshiRaces.Tests/`: sus pruebas (dados sintéticos, no se necesita instalación de juego). `<Mod>.Data/` (opcional): un generador que escribe `<Mod>.mod` del juego instalado. `<Mod>/Structures/` (de manera opcional) `.glb` Los modelos importados como estructuras constructivas en el desplegamiento ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`• El marco libera los mods construidos contra
([El bloqueo marco](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)El flujo de trabajo de actualización del Framework está moviéndolo a nuevas publicaciones y se fusionan cuando el CI pasa ([Actualización del marco](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`Un clic para construir, implementar y ejecutar.
- `tools/`: `Update-Local.ps1`, el control local de la pulsación y el motor-pin ([Mantener los clones actuales](docs/local-development.md#keep-clones-current));
`refcheck.sh` y `RefCheck/`, el motor-referencia de verificación ([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`El modelo de importación ([docs/structures.md](docs/structures.md)); `RaceImport/`El cuerpo de la mujer ([docs/race-import.md](docs/race-import.md)); `model-info.py` y `prepare-model.py`, controles de modelo y textura recortando ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` importa el motor, por lo que los modos se construyen con las configuraciones del juego (las advertencias son errores).
- `Directory.Build.rsp` empieza las referencias de los motores, así que el Meitou más antiguo libera el soporte de mods, igual a cada `gameVersion` El piso: el
El juego rechaza un modo que se refiere a una nueva marca de motor, y el submodulo construye `1.0.0.0` ([Versión](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). El flujo de trabajo de la compatibilidad de Meitou veo verifica los mods diariamente contra las nuevas publicaciones de Meitou ([Meitou Compo](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## Las ramas

PRS en el `beta`CI (construcción y pruebas) debe pasar. `beta` Se promueve para `main` Una vez probado en el juego, y la producción va a través de un PR `release`. Cada uno se une a `release` Etiquetas El `<slug>-v<version>` y publica el mod's zip (`.github/workflows/release.yml`a) a menos que exista esa etiqueta; `tools\Tag-Release.ps1` Es el manual fallback.

- Nuevas protecciones de ramo y permanentes beta/main/release Las Revisións: [docs/branch-policy.md](docs/branch-policy.md). Templates copia fuente; GutenStructure aplica las configuraciones a través de la reconciliación.
