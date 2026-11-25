<a id="depending-on-kenshi-races"></a>
# Usar el framework en otro mod y añadir subrazas

Cómo un mod de código utiliza las carreras y animaciones de Kenshi Races, les agrega subrazas o agrega sus propias carreras (`kenshi.races`). Reglas del cargador: motor [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Una habilidad de agente sigue los mismos pasos: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Consúltelo en el momento de la construcción, nunca lo envíe

En un repositorio creado a partir de [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate), agregue el zip de lanzamiento de Kenshi Races a `meitou.lock.json` ; la compilación la descarga, la verifica y hace referencia a ella, y Play la implementa. Cada valor proviene de las notas de la versión (`kenshi-races-v<version>`):

```json
{
  "id": "kenshi.races",
  "repo": "Meitou-Modding-Ecosystem/KenshiRaces",
  "tag": "kenshi-races-v0.4.0",
  "asset": "KenshiRaces-0.4.0.zip",
  "sha256": "<Zip SHA-256 from the notes>",
  "folder": "KenshiRaces",
  "engine": "<Submodule engine commit from the notes>"
}
```

- Su pin `engine/` se mueve a la confirmación de ese motor (GutenStructure [bloqueo de marco](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Nunca incluya `KenshiRaces.dll` en su propio `assemblies` ; el `kenshi.races` cargado sirve para los tipos.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Declararlo en `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.23.0",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Antes de la 1.0, una versión menor puede dañar el API, así que limite el rango en la siguiente menor.
- Kenshi Races debe ubicarse encima de su mod en `data/mods.cfg` (el Arrange del lanzador lo hace), por lo que sus registros existen cuando se ejecuta su fase de datos.

<a id="3-use-its-races"></a>
## 3. Usa sus carreras

Consulte los identificadores en `Races` de sus registros, p. en su fase de datos:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RAZAs (subrazas): `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- PERSONAJES (sin kit, reclutables): `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`, `CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (las carreras del editor): `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` es "Únase a nosotros". hablar.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) y `Races.Groups` los describen. Tabla: [kenshi-races.md "Carreras"](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Agrega una subraza o una raza

1. Coloque cada cuerpo con la [carrera importada](race-import.md) de un clon de KenshiRaces, en el `items/races` de su mod con su propio vástago (`mymod_<character>` ; `female` como plataforma para un cuerpo femenino) y confirme los archivos. Agregue `!/<Mod>/items/races/*.mesh` y `!/<Mod>/items/races/*.dds` a `.gitignore`.
2. Descríbelo y agrega sus registros en la fase de datos de tu mod (la entrada implementa `IModDataPhase`):

```csharp
static readonly Subrace GhostCrab = new("ghostcrab", "30-MyMod.mod",
    Male: new RaceBody("MyMod", "mymod_ghostcrab", NormalMap: true), Female: null,
    Characters: [new RaceCharacter("ghostcrab", "31-MyMod.mod", Female: false)]);

public void EditData(ModDataContext c)
{
    RaceRecords.Add(c, GhostCrab);                                       // RACE + CHARACTERs, the recruit talk by default
    RaceRecords.JoinGroup(c, Races.CrabfolkGroup, GhostCrab.RaceId);    // the editor lists it under Crabfolk
}
```

- Una subraza con un cuerpo `Male` y `Female` obtiene la flecha de género del editor; con uno es de un solo género y todos los personajes lo usan.
- `RaceRecords.Add(c, subrace, talk)` : `talk` es una identificación DIALOGUE para los personajes, `null` para ninguno.
- Una carrera propia: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` suma la RACE_GROUP y todas las subrazas.
- Las subrazas son `playable` con límites del editor Kenshi Races (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` completa los registros que agrega usted mismo (`c.Add`) cuando desea cambiar más campos después.
- Los identificadores son suyos: registros de su propio nombre de archivo `.mod`.

<a id="6-play-its-animations"></a>
## 6. Reproduce sus animaciones.

Cada clip de [animations.md](animations.md) está vinculado a cada raza en los esqueletos humanos del juego (una subraza agregada con `RaceRecords` también), por lo que un mod solo la reproduce. Un movimiento de combate mod es solo animación y se solicita cada vez que el juego no reproduce nada propio (engine docs/modding.md, "Movimientos de combate"):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` es tuyo: una respuesta económica, segura para subprocesos y de solo lectura de tu propio estado, el progreso del clip de 0 a 1 (segundos/ `clip.Length`).
- Los clips se reproducen en su lugar y miran hacia adelante; un clip más corto que su movimiento retiene su último fotograma a menos que ajuste el progreso.
- Las animaciones solo necesitan la dependencia (secciones 1 y 2), no registros propios.

<a id="5-check-it"></a>
## 5. Compruébalo

`dotnet build`, tus pruebas, luego en el juego F10 **Kenshi Races : carreras**enumera tu carrera con su malla y `spawn <name>` la genera. Nombra las subrazas y los CARACTERES con una palabra en minúscula, p. `ghostcrab`, por lo que la consola los toma sin comillas; El editor de un nuevo juego enumera tu subraza.

[English](../../../docs/depending-on-kenshi-races.md) · [Kenshi Races](../README.md)
