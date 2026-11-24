<a id="depending-on-kenshi-races"></a>
# A seconda di Kenshi Races

Come un mod del codice utilizza le razze e le animazioni di Kenshi Races , aggiunge loro sottorazze o aggiunge le proprie razze ( `kenshi.races` ). Regole del caricatore: motore [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md) . Un'abilità dell'agente segue gli stessi passaggi: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md) .

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Fai riferimento al momento della creazione, non spedirlo mai

In un repository creato da [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate) , aggiungi Kenshi Races ' release zip a `meitou.lock.json` ; la build la scarica, la controlla e la fa riferimento e Play la distribuisce. Ogni valore proviene dalle note di rilascio ( `kenshi-races-v<version>` ):

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

- Il tuo pin `engine/` si sposta su quel commit del motore ( GutenStructure [blocco del framework](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ).
- Non elencare mai `KenshiRaces.dll` nel tuo `assemblies` ; lo `kenshi.races` caricato serve i tipi.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Dichiararlo in `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Prima della 1.0 una versione minore potrebbe interrompere API , quindi limitare l'intervallo al minore successivo.
- Kenshi Races deve trovarsi sopra il tuo mod in `data/mods.cfg` (lo fa Arrange del launcher), quindi i suoi record esistono quando viene eseguita la fase dati.

<a id="3-use-its-races"></a>
## 3. Usa le sue razze

Fai riferimento agli ID in `Races` dai tuoi record, ad es. nella fase dati:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RAZZE (sottorazze): `Races.LeomaRace` , `CrevanRace` , `CrabmanRace` , `CrabQueenRace` .
- PERSONAGGI (senza kit, reclutabili): `LeomaMaleCharacter` , `LeomaFemaleCharacter` , `CrevanMaleCharacter` , `CrevanFemaleCharacter` , `CrabmanCharacter` , `CrabQueenCharacter` .
- RACE_GROUPs (le gare dell'editore): `LeomaGroup` , `CrevanGroup` , `CrabfolkGroup` . `Races.Recruit` è il "Unisciti a noi". parlare.
- `Races.Leoma` , `Crevan` , `Crabman` , `CrabQueen` ( `Subrace` ) e `Races.Groups` li descrivono. Tabella: [kenshi-races.md "Razze"](kenshi-races.md#races) .

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Aggiungi una sottorazza o una razza

1. Adatta ogni corpo con [race import](race-import.md) da un clone KenshiRaces, nello `items/races` del tuo mod con la tua radice ( `mymod_<character>` ; `female` come rig per un corpo femminile) e committa i file. Aggiungi `!/<Mod>/items/races/*.mesh` e `!/<Mod>/items/races/*.dds` a `.gitignore` .
2. Descrivilo e aggiungi i suoi record nella fase dati della tua mod (la voce implementa `IModDataPhase` ):

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

- Una sottorazza con un corpo `Male` e `Female` ottiene la freccia di genere dell'editore; con uno è dello stesso sesso e ogni personaggio lo indossa.
- `RaceRecords.Add(c, subrace, talk)` : `talk` è un ID DIALOGUE per i personaggi, `null` per nessuno.
- Una gara tutta tua: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` aggiunge RACE_GROUP e tutte le sottorazze.
- Le sottorazze sono `playable` con i limiti dell'editor Kenshi Races ( `RaceRecords.EditorLimits` ); `FillRace` / `FillCharacter` compila i record che aggiungi tu stesso ( `c.Add` ) quando desideri modificare più campi in seguito.
- Gli ID sono tuoi: record del tuo nome file `.mod` .

<a id="6-play-its-animations"></a>
## 6. Riproduci le sue animazioni

Ogni clip di [animations.md](animations.md) è collegata a ogni razza sugli scheletri umani del gioco (una sottorazza aggiunta anche con `RaceRecords` ), quindi un mod lo riproduce solo. Una mossa di combattimento mod è solo un'animazione, e viene richiesta ogni volta che il gioco non riproduce nulla di proprio (engine docs/modding.md, "Mosse di combattimento"):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` è tuo: una risposta economica, thread-safe e di sola lettura dal tuo stato, l'avanzamento della clip da 0 a 1 (secondi / `clip.Length` ).
- Le clip giocano sul posto e sono rivolte in avanti; una clip più corta del tuo spostamento mantiene l'ultimo fotogramma a meno che non riavvolgi l'avanzamento.
- Le animazioni necessitano solo della dipendenza (sezioni 1 e 2), nessun record personale.

<a id="5-check-it"></a>
## 5. Controllalo

`dotnet build` , i tuoi test, poi nel gioco F10 ** Kenshi Races : gare** elenca la tua razza con la sua mesh e `spawn <name>` la genera. Nomina le sottorazze e i CARATTERI con una parola minuscola, ad es. `ghostcrab` , quindi la console li prende senza virgolette; l'editor di un nuovo gioco elenca la tua sottorazza.
