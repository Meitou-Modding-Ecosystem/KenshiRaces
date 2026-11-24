<a id="depending-on-kenshi-races"></a>
# A seconda di Kenshi Race

Come utilizza un codice Kenshi Le razze e le animazioni di Races, aggiungono subrassi a loro o aggiungono le proprie razze (`kenshi.races`). Regole di caricamento: motore [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Un agente abilità va attraverso gli stessi passaggi: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Riferendolo al tempo di costruzione, non inviarlo mai

In un repo realizzato da [Il modello](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)e aggiungi Kenshi Race' rilascio zip a `meitou.lock.json`La costruzione lo scarica, lo verifica e lo riferisce, e Play lo distribuisce. Ogni valore viene dalla nota di rilascio (`kenshi-races-v<version>`):

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

- Il tuo `engine/` il pin si muove a quel motore composta (GutenStructure
[Il framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Mai la lista `KenshiRaces.dll` Nel vostro `assemblies`Il carico `kenshi.races` Servono i tipi.

<a id="2-declare-it-in-meitoujson"></a>
## 2. dichiarare in `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Prima di 1.0 una versione minore può rompere il APIPer questo motivo, si può trovare la gamma al prossimo minore.
- Kenshi Le corse devono stare al di sopra del vostro mod `data/mods.cfg` (L'arresto del lanciatore lo fa), quindi i suoi record esistono quando la fase dei dati è in corso.

<a id="3-use-its-races"></a>
## 3. Utilizzare le sue corse

Si riferisce agli id in `Races` dai tuoi record, ad esempio nella tua fase dati:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs ( per i sottotitoli ) `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (Sì, è un’esperienza di ricorso e di ricorso): `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (la corsa dell’editore) `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` E’ la parola “giungiamo”.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`e) e `Races.Groups` Descrivi loro. Il tavolo:
[Kenshi-races.md “Razzi”](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Aggiungi un subraso o una corsa

1. Fatto ogni corpo con il [La corsa di importazione](race-import.md) da un KenshiRaces clone, nel tuo mod `items/races` Con la tua voce
(`mymod_<character>`; `female` Come il rig per un corpo femminile), e compiere i file. Aggiungi `!/<Mod>/items/races/*.mesh` e `!/<Mod>/items/races/*.dds` per `.gitignore`.
2. Descriverlo e aggiungere i suoi registri nella fase dati del mod (l'implementazione dell'ingresso `IModDataPhase`):

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

- Un subraso con un `Male` e a `Female` Il corpo riceve la pistola di genere dell'editore; con uno è single e ogni personaggio lo indossa.
- `RaceRecords.Add(c, subrace, talk)`: `talk` è a DIALOGUE per i personaggi, `null` per nessuno.
- Una corsa per te: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` Aggiungi il RACE_GROUP e ogni subrasione.
- I soggetti sono `playable` con Kenshi I limiti dell'editore di Races (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` Inserisci i registri che aggiungi
Il tuo (`c.Add`Quando si vuole cambiare più campi dopo.
- I ID sono tuoi: i registri del tuo `.mod` Il nome del file.

<a id="6-play-its-animations"></a>
## 6. Gioca le sue animazioni

Ogni clip di [animations.md](animations.md) è collegato a ogni corsa sui scheletoni umani del gioco (una subrassi aggiunta con `RaceRecords` Anche in questo caso, un mod lo gioca solo. Un modo di combattimento è solo animazione, chiesto ogni volta che il gioco non gioca nulla di se stesso (motore) docs/modding.md“Il combattimento si muove”:

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` è tuo: un prezzo economico, sicuro, risposta leggibile dal tuo stato, il progresso del clip da 0 a 1 (secondi / `clip.Length`).
- I clip giocano in posizione e si muovono avanti; un clip più breve di quello che si muove, man mano che non si muove.
- Le animazioni richiedono solo la dipendenza (sezione 1 e 2), nessuna registrazione della vostra.

<a id="5-check-it"></a>
## 5. Verifica il

`dotnet build`Il tuo test, poi nel gioco F10 **Kenshi Rassaggi: Rassaggi** La tua corsa con la sua messa, e `spawn <name>` Lo spazza. Nome di subratto e CHARACTERs Una parola inferiore, ecc. `ghostcrab`, quindi la console li prende senza citazioni; l'editore di un nuovo gioco elenca il tuo subraso.
