<a id="depending-on-kenshi-races"></a>
# Abhängig von Kenshi Rennen

Wie ein Code-Mod verwendet Kenshi Races' Races und Animationen, fügt ihnen Subrazen hinzu oder fügt ihre eigenen Rassen hinzu (`kenshi.races`). Ladenregelungen: Motoren [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Ein Agent-Knowledge geht durch die gleichen Schritte: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Referieren Sie es auf die Zeit, niemals senden

In einem Repo, der von [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate), hinzufügen Kenshi Races' Release Zip zu `meitou.lock.json`; die Build Downloads, überprüft und bezieht sich auf sie, und Play stellt sie auf. Jede Werte stammen aus den Notizen der Veröffentlichung (`kenshi-races-v<version>`):

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

- Ihre `engine/` Pin bewegt sich zu diesem Motor Komplett (GutenStructure
[Framework Locker](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- niemals aufgeführt `KenshiRaces.dll` In deiner eigenen `assemblies`Die geladenen `kenshi.races` Wir dienen den Typen.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Erklären Sie es in `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Vor 1.0 kann eine kleinere Version APISo zieht man das Range auf das nächste Minor.
- Kenshi Rassen müssen über Ihren Mod `data/mods.cfg` (Der Start-Up-System tut es), so dass seine Aufzeichnungen existieren, wenn Ihre Datenphase läuft.

<a id="3-use-its-races"></a>
## 3. Nutzen Sie Ihre Rennen

Die IDs in `Races` aus Ihren Aufzeichnungen, z. B. in Ihrer Datenphase:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs ( und die Unterschriften ) `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (Sie sind unabhängig von der Rekrutierung) `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (Die Rennen des Redaktors) `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` Es ist das „Gehen Sie uns“ zu reden.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) und `Races.Groups` Beschreiben Sie sie. Das Tisch:
[Kenshi-races.md „Races“](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Eine Subrace oder eine Race hinzufügen

1. Jeder Körper mit dem [Rennen Import](race-import.md) von einem KenshiRaces-Klone, in Ihren Mod `items/races` Mit deiner eigenen Stimme
(`mymod_<character>`; `female` wie die Reife für ein weibliches Körper), und verpflichten sich die Dateien. hinzufügen `!/<Mod>/items/races/*.mesh` und `!/<Mod>/items/races/*.dds` zu `.gitignore`.
2. Beschreiben Sie es und fügen Sie seine Aufzeichnungen in die Datenphase Ihres Mods hinzu (die Eintrittsimplementierungen `IModDataPhase`):

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

- Eine Subrace mit einer `Male` und a `Female` Der Körper bekommt die Geschlechterstreifen des Redaktors; mit einem ist es einzig Geschlechter und jeder Charakter trägt es.
- `RaceRecords.Add(c, subrace, talk)`: `talk` ist A DIALOGUE für die Charaktere, `null` Für keiner.
- Ein Rennen von dir selbst: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` Er fügt die RACE_GROUP Und jede Subrasion.
- Die Substanzen sind `playable` mit Kenshi Veröffentlichungsgrenze von Races (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` Füllen Sie die Aufzeichnungen, die Sie hinzufügen
Sie selbst (`c.Add`Wenn Sie nach mehr Feldern ändern möchten.
- Die IDs sind deine: Aufzeichnungen von deinen eigenen `.mod` Datei Name

<a id="6-play-its-animations"></a>
## 6. Spielen Sie Ihre Animationen

Jedes Clip von [animations.md](animations.md) ist mit jedem Rennen auf den menschlichen Skeletons des Spiels verbunden (eine Subrace mit `RaceRecords` Auch), so dass ein Mod es nur spielt. Ein Mod-Kampfbewegung ist nur Animation, gefragt jedes Mal, wenn das Spiel nichts von sich selbst spielt (Motor) docs/modding.md„Kombat bewegt sich“):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` ist dein: ein billiges, thread-sicheres, lesbares Antworten aus deinem eigenen Zustand, die Progression des Klips 0 zu 1 (Sekunden / Sekunden) `clip.Length`).
- Die Clips spielen voran; ein kürzerer als Ihre Bewegung hält ihren letzten Rahmen, es sei denn, Sie haben den Fortschritt.
- Animationen benötigen nur die Abhängigkeit (Sektionen 1 und 2), keine eigenen Aufzeichnungen.

<a id="5-check-it"></a>
## 5. Überprüfen Sie es

`dotnet build`, Ihre Tests, dann im Spiel F10 **Kenshi Rennen: Rennen** Sie werden mit ihrer Messe aufgeführt und `spawn <name>` Spannen Sie es. Name Subraces und CHARACTERs Ein weiteres Wort, zum Beispiel `ghostcrab`So nimmt die Konsole sie ohne Zitate; ein neues Spiel-Editor erfasst Ihre Subrace.
