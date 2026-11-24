<a id="depending-on-kenshi-races"></a>
# Abhängig von Kenshi Races

Wie ein Code-Mod die Rassen und Animationen von Kenshi Races verwendet, ihnen Unterrassen hinzufügt oder seine eigenen Rassen hinzufügt ( `kenshi.races` ). Laderegeln: engine [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md) . Ein Agenten-Skill durchläuft die gleichen Schritte: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md) .

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Referenzieren Sie es zum Zeitpunkt der Erstellung, versenden Sie es niemals

Fügen Sie in einem Repo aus [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate) , Kenshi Races ' Release-Zip zu `meitou.lock.json` hinzu; Der Build lädt ihn herunter, prüft und referenziert ihn und Play stellt ihn bereit. Jeder Wert stammt aus den Versionshinweisen ( `kenshi-races-v<version>` ):

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

- Ihr `engine/` -Pin wird zu diesem Engine-Commit verschoben ( GutenStructure [Framework-Sperre](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ).
- Listen Sie `KenshiRaces.dll` niemals in Ihrem eigenen `assemblies` auf; Der geladene `kenshi.races` bedient die Typen.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Deklarieren Sie es in `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Vor 1.0 kann eine Nebenversion den API beschädigen, also begrenzen Sie den Bereich bei der nächsten Nebenversion.
- Kenshi Races muss sich über Ihrem Mod in `data/mods.cfg` befinden (das macht der Arrange-Befehl des Launchers), damit seine Datensätze vorhanden sind, wenn Ihre Datenphase ausgeführt wird.

<a id="3-use-its-races"></a>
## 3. Benutze seine Rassen

Beziehen Sie sich auf die IDs in `Races` aus Ihren Unterlagen, z. B. in Ihrer Datenphase:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RENNEN (Unterrassen): `Races.LeomaRace` , `CrevanRace` , `CrabmanRace` , `CrabQueenRace` .
- CHARAKTER (ohne Ausrüstung, rekrutierbar): `LeomaMaleCharacter` , `LeomaFemaleCharacter` , `CrevanMaleCharacter` , `CrevanFemaleCharacter` , `CrabmanCharacter` , `CrabQueenCharacter` .
- RACE_GROUPs (die Rassen des Herausgebers): `LeomaGroup` , `CrevanGroup` , `CrabfolkGroup` . `Races.Recruit` ist das „Kommen Sie mit“. sprechen.
- `Races.Leoma` , `Crevan` , `Crabman` , `CrabQueen` ( `Subrace` ) und `Races.Groups` beschreiben sie. Tabelle: [kenshi-races.md „Rennen“](kenshi-races.md#races) .

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Fügen Sie eine Unterrasse oder eine Rasse hinzu

1. Passen Sie jeden Körper mit dem [race import](race-import.md) von einem KenshiRaces-Klon, in den `items/races` Ihres Mods mit Ihrem eigenen Vorbau an ( `mymod_<character>` ; `female` als Rig für einen weiblichen Körper) und übertragen Sie die Dateien. Fügen Sie `!/<Mod>/items/races/*.mesh` und `!/<Mod>/items/races/*.dds` zu `.gitignore` hinzu.
2. Beschreiben Sie es und fügen Sie seine Datensätze in der Datenphase Ihres Mods hinzu (der Eintrag implementiert `IModDataPhase` ):

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

- Eine Unterrasse mit einem `Male` - und einem `Female` -Körper erhält den Geschlechtspfeil des Herausgebers; Bei einem handelt es sich um ein geschlechtsspezifisches Modell, und jeder Charakter trägt es.
- `RaceRecords.Add(c, subrace, talk)` : `talk` ist eine DIALOGUE -ID für die Charaktere, `null` für keine.
- Eine eigene Rasse: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` fügt die RACE_GROUP und jede Unterrasse hinzu.
- Subraces sind `playable` mit Kenshi Races ' Editorgrenzen ( `RaceRecords.EditorLimits` ); `FillRace` / `FillCharacter` füllen Datensätze aus, die Sie selbst hinzufügen ( `c.Add` ), wenn Sie danach weitere Felder ändern möchten.
- Die IDs gehören Ihnen: Aufzeichnungen Ihres eigenen `.mod` -Dateinamens.

<a id="6-play-its-animations"></a>
## 6. Spielen Sie die Animationen ab

Jeder Clip von [animations.md](animations.md) ist mit jeder Rasse auf den menschlichen Skeletten des Spiels verknüpft (eine Unterrasse, die auch mit `RaceRecords` hinzugefügt wurde), sodass ein Mod ihn nur abspielt. Ein Mod-Kampfzug ist nur eine Animation und wird immer dann abgefragt, wenn das Spiel nichts Eigenes abspielt (Engine docs/modding.md, „Kampfbewegungen“):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` gehört Ihnen: eine günstige, threadsichere, schreibgeschützte Antwort aus Ihrem eigenen Status, der Fortschritt des Clips 0 bis 1 (Sekunden / `clip.Length` ).
- Die Clips bleiben an Ort und Stelle und zeigen nach vorne; Ein Clip, der kürzer als Ihr Zug ist, behält seinen letzten Frame bei, es sei denn, Sie brechen den Fortschritt ab.
- Animationen benötigen nur die Abhängigkeit (Abschnitte 1 und 2), keine eigenen Datensätze.

<a id="5-check-it"></a>
## 5. Überprüfen Sie es

`dotnet build` , Ihre Tests, dann im Spiel F10 ** Kenshi Races : Rennen** listet Ihr Rennen mit seinem Netz auf und `spawn <name>` erzeugt es. Benennen Sie Unterrassen und ZEICHEN mit einem Wort in Kleinbuchstaben, z. B. `ghostcrab` , sodass die Konsole sie ohne Anführungszeichen akzeptiert; Der Editor eines neuen Spiels listet Ihre Unterrasse auf.
