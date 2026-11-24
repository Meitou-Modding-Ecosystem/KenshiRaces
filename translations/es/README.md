# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · [Deutsch](../de/README.md) · Español · [Italiano](../it/README.md) · [Português do Brasil](../pt-BR/README.md) · [Русский](../ru/README.md) · [日本語](../ja/README.md) · [한국어](../ko/README.md) · [简体中文](../zh-CN/README.md)

Kenshi Races añade tres razas jugables al editor de personajes de [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient). Los Leoma y los Crevan tienen cuerpos masculinos y femeninos. Los Crabfolk incluyen dos variantes, `crabman` y `crabqueen`. Todas utilizan los esqueletos humanos del juego. El mod también ofrece 59 animaciones para las razas que comparten esos esqueletos y un framework con el que otros mods pueden añadir sus propias razas y subrazas. GPL-3.0-or-later.

## Guías

- [Razas, registros y pruebas en el juego](docs/kenshi-races.md)
- [Usar el framework en otro mod y añadir subrazas](docs/depending-on-kenshi-races.md)
- [Biblioteca de 59 animaciones y proceso de creación](docs/animations.md)
- [Adaptar un cuerpo GLB al esqueleto del juego](docs/race-import.md)
- [Compilar y jugar con Visual Studio, VS Code o la terminal](docs/play.md)
- [Configurar y actualizar los repositorios locales](docs/local-development.md)
- [Coordinar cambios entre repositorios](docs/agents-across-repos.md)
- [Comprobar la compatibilidad con nuevas versiones de Meitou](docs/refcheck.md)
- [Publicar en Steam Workshop](docs/steam-workshop.md)
- [Importar modelos GLB como edificios sin FCS](docs/structures.md)
- [Preparar la geometría, las dimensiones y las texturas](docs/model-prep.md)
- [Protección de ramas y requisitos de revisión](docs/branch-policy.md)
- [Reglas del repositorio y procedimientos para agentes](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## Identificadores compartidos

Los mods cargados comparten la numeración de los tipos de tarea y los códigos de diálogo. Kenshi Races no reserva ninguno. (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## Estructura del repositorio

- `engine/` — Submódulo de MeitouClient fijado al commit usado para compilar. Actualízalo con el siguiente comando después de cada pull. [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — Proyecto y manifiesto del mod. Los cuerpos adaptados están en el repositorio. Las pruebas utilizan datos sintéticos y no requieren instalar el juego.
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — Los proyectos opcionales generan datos a partir del juego instalado o importan modelos GLB como edificios al desplegar. [StructureImport](docs/structures.md)
- `meitou.lock.json` — Fija las versiones de los frameworks. El flujo de actualización incorpora nuevas versiones y fusiona el cambio cuando pasa la CI. [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — Compilar, desplegar y ejecutar con un clic.
- `tools/` — Herramientas de sincronización, comprobación de referencias del motor, importación de edificios, adaptación de cuerpos y preparación de texturas. [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — Importa la configuración del motor. Las advertencias del compilador se tratan como errores.
- `Directory.Build.rsp` — Fija las referencias a la versión más antigua del motor admitida. El juego rechaza un mod con referencias a un motor más reciente. El submódulo usa una versión de ensamblado por defecto. (`gameVersion`, `1.0.0.0`).  La compatibilidad se comprueba a diario con las nuevas versiones de Meitou. [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## Ramas

Dirige las PR a beta. La compilación y las pruebas deben pasar. Tras probar el mod en el juego, se puede promover beta a main. La producción se publica mediante una PR a release. Cada fusión en release crea la etiqueta de versión y publica el ZIP si la etiqueta aún no existe. El script de etiquetado sirve como alternativa manual. (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
