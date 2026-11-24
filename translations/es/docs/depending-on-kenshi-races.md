<a id="depending-on-kenshi-races"></a>
# Dependiendo de Kenshi Las carreras

Cómo usa un código Kenshi Las razas y animaciones de Races, añade subraces a ellos o añade sus propias razas (`kenshi.races`). Reglas de carga: motor [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Una habilidad agente pasa por los mismos pasos: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Referirlo en el tiempo de construcción, nunca enviarlo

En un repo hecho de [El templo](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)y añade Kenshi Races' release zip para `meitou.lock.json`La construcción descarga, verifica y hace referencia a ella, y el juego lo despliega. Todos los valores provienen de las notas de la publicación (`kenshi-races-v<version>`):

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

- tuyo `engine/` El pin se mueve a ese motor (GutenStructure)
[El bloqueo marco](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Nunca lista `KenshiRaces.dll` En tu propia `assemblies`El cargado `kenshi.races` sirven a los tipos.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Declararlo en `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Antes de 1.0 una versión menor puede romper el APIPor lo tanto, captura el rango en el siguiente menor.
- Kenshi Las carreras deben sentarse por encima de su modo `data/mods.cfg` (El arranjo del lanzador lo hace), por lo que sus registros existen cuando su fase de datos se lleva a cabo.

<a id="3-use-its-races"></a>
## 3. Utilice sus carreras

En el caso de los IDs en `Races` de sus registros, por ejemplo, en su fase de datos:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs (en las subespecies) `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (Sin embargo, es muy importante que los trabajadores sean reclutados) `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (La raza del editor) `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` Es el “Junto a nosotros” hablar.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) y `Races.Groups` Describe los La mesa:
[Kenshi-races.md «Races»](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Añadir una subraza o una carrera

1. Se ajusta a cada cuerpo con [Las carreras de importación](race-import.md) desde un clon de KenshiRaces, en su mod `items/races` Con tu propia voz
(`mymod_<character>`; `female` como el riego para un cuerpo femenino), y se compromete a los archivos. Añadir `!/<Mod>/items/races/*.mesh` y `!/<Mod>/items/races/*.dds` para `.gitignore`.
2. Describe y ajuste sus registros en la fase de datos de su modo (las implementaciones de entrada `IModDataPhase`):

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

- Un subrace con una `Male` y a `Female` El cuerpo recibe la bala de género del editor; con uno es un solo género y cada personaje la lleva.
- `RaceRecords.Add(c, subrace, talk)`: `talk` Es una DIALOGUE para los personajes, `null` por ninguno.
- Una carrera de tu propia: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` Añade el RACE_GROUP y cada subraza.
- Las subespecies son `playable` con Kenshi Los límites de los editores (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` Encuentran los registros que añaden
En el caso de sí mismo (`c.Add`Cuando desea cambiar más campos después.
- Los IDs son tus: registros de tus propios `.mod` Nombre de archivo.

<a id="6-play-its-animations"></a>
## 6. Juega sus animaciones

Cada clip de [animations.md](animations.md) está vinculado a cada carrera en los esqueletos humanos del juego (una subraza añadida con `RaceRecords` También), así que un mod sólo lo juega. Un movimiento de combate de modo es solo animación, preguntado cada vez que el juego no juega nada de su propio (motor) docs/modding.md“El combate se mueve”):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` es suyo: un barato, seguro de la línea, respuesta de lectura de su propio estado, el progreso del clip de 0 a 1 (segundos / `clip.Length`).
- Los clips juegan en el lugar y se enfrentan hacia adelante; un clipe más corto que su movimiento mantiene su último marco, a menos que se vaya el progreso.
- Las animaciones sólo necesitan la dependencia (secciones 1 y 2), sin registros de su propio.

<a id="5-check-it"></a>
## 5. Verificarlo

`dotnet build`En el caso de los juegos, en el caso de F10 **Kenshi Categoría: Race** En el caso de que se trate de una raza con su mezcla, y `spawn <name>` Lo espiona. El nombre de los subraces y CHARACTERs Una palabra de menor tamaño, por ejemplo. `ghostcrab`, por lo que la consola los toma sin citas; el editor de un nuevo juego lista su subraza.
