<a id="kenshi-races"></a>
# Kenshi Races

Nuevas razas para [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) en los esqueletos humanos del juego, jugables en el editor de personajes: Leoma y Crevan (masculino y femenino) y Crabfolk (hombre cangrejo, reina cangrejo), una biblioteca de animación de 59 clips para cada raza de esqueleto humano y el marco con el que otros mods agregan sus propias razas y subrazas. GPL -3.0 o posterior.

- Las carreras, sus récords y sus pruebas en el juego: [docs/kenshi-races.md](docs/kenshi-races.md) .
- Usando estas carreras o agregando las tuyas desde otro mod: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (habilidad del agente `.claude/skills/kenshi-races-dependent` ).
- La biblioteca de animaciones (59 clips para cada raza en los esqueletos humanos) y cómo se hizo: [docs/animations.md](docs/animations.md) .
- Colocar un cuerpo `.glb` manipulado ( Meshy , Hi3D "3ds Max Biped") en el esqueleto del juego: [docs/race-import.md](docs/race-import.md) .

- Compile y ejecute localmente con Visual Studio , VS Code o la terminal (el motor de este repositorio o el iniciador instalado, implementado en el juego): [docs/play.md](docs/play.md) .
- Configurando los repositorios en su máquina: [docs/local-development.md](docs/local-development.md) .
- Instruir a los agentes sobre un cambio que abarca repositorios: [docs/agents-across-repos.md](docs/agents-across-repos.md) .
- Reglas del agente para el pin del motor, versiones, CI y lanzamientos: [ CLAUDE .md](../../CLAUDE.md) (igual que `AGENTS.md` ); Procedimientos paso a paso: `.claude/skills/meitou-engine-release` .
- Comparando un mod construido con la versión más reciente de Meitou ( CI lo hace en cada PR ): [docs/refcheck.md](docs/refcheck.md) .
- Publicar el mod en Steam Workshop por primera vez y luego en cada lanzamiento: [docs/steam-workshop.md](docs/steam-workshop.md) .
- Convirtiendo los modelos `.glb` ( Meshy , Hi3D , Blender ) en estructuras construibles, no se necesita FCS : [docs/structures.md](docs/structures.md) ; obtener primero el número de polímeros, el tamaño y las texturas de un modelo generado: [docs/model-prep.md](docs/model-prep.md) .

<a id="numbers"></a>
## Números

Los tipos de tareas ( `AddGoalType` , `AddJobType` ) y códigos de diálogo ( `AddDialogueCondition` , `AddDialogueEffect` ) son compartidos por cada mod cargado ([bloques de números de mod](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) ). Kenshi Races afirma no tener ninguno.

| Número | Piscina | Usar |
| --- | --- | --- |

<a id="layout"></a>
## Disposición

- `engine/` : MeitouClient como un submódulo de git, anclado al motor que confirma las compilaciones del mod. `git pull` no lo mueve: después de cada extracción, ejecute `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([mantenga los clones actualizados](docs/local-development.md#keep-clones-current) ).
- `KenshiRaces/` : el proyecto del mod y `meitou.json` ; `KenshiRaces/items/races/` : las carrocerías montadas (producción de importación comprometida). `KenshiRaces.Tests/` : sus pruebas (datos sintéticos, no es necesaria la instalación del juego). `<Mod>.Data/` (opcional): un generador que escribe `<Mod>.mod` desde el juego instalado. `<Mod>/Structures/` (opcional): modelos `.glb` importados como estructuras construibles en la implementación ([docs/structures.md](docs/structures.md) ).
- `meitou.lock.json` : el marco libera las modificaciones compiladas ([bloqueo del marco](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ); el flujo de trabajo de actualización del marco lo mueve a nuevas versiones y se fusiona cuando pasa CI ([actualización del marco](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ).
- `Play/` : compilación, implementación y ejecución con un solo clic.
- `tools/` : `Update-Local.ps1` , la verificación local de extracción y pasador del motor ([mantener los clones actualizados](docs/local-development.md#keep-clones-current) ); `refcheck.sh` y `RefCheck/` , la verificación de referencia del motor ([docs/refcheck.md](docs/refcheck.md) ); `StructureImport/` , la importación del modelo ([docs/structures.md](docs/structures.md) ); `RaceImport/` , el ajuste de la carrocería ([docs/race-import.md](docs/race-import.md) ); `model-info.py` y `prepare-model.py` , comprobaciones de modelos y reducción de texturas ([docs/model-prep.md](docs/model-prep.md) ).
- `Directory.Build.props` importa el motor, por lo que los mods se crean con la configuración del juego (las advertencias son errores).
- `Directory.Build.rsp` marca las referencias del motor como la versión más antigua de Meitou que admite mods, igual a cada piso de `gameVersion` : el juego rechaza un mod que hace referencia a un sello de motor más nuevo y el submódulo compila `1.0.0.0` ([versionado](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ). El flujo de trabajo de compatibilidad de Meitou compara las modificaciones diariamente con las nuevas versiones de Meitou ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ).

<a id="branches"></a>
## Sucursales

PR en `beta` ;  Debe aprobarse CI (compilación y pruebas). `beta` pasa a `main` una vez probado en juego, y la producción pasa de PR a `release` . Cada fusión en `release` lo etiqueta como `<slug>-v<version>` y publica el zip del mod ( `.github/workflows/release.yml` ) a menos que esa etiqueta exista; `tools\Tag-Release.ps1` es el recurso manual.

- Protecciones de sucursales de nuevos repositorios y revisiones permanentes de beta/principal/versión: [docs/branch-policy.md](docs/branch-policy.md) . Fuente de copia de plantillas; GutenStructure aplica la configuración mediante conciliación.
