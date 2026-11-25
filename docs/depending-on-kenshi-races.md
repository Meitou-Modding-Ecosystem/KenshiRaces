# Depending on Kenshi Races

How a code mod uses Kenshi Races' races and animations, adds subraces to them or adds its own races (`kenshi.races`). Loader rules: engine
[modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). An agent skill
walks through the same steps: [.claude/skills/kenshi-races-dependent](../.claude/skills/kenshi-races-dependent/SKILL.md).

## 1. Reference it at build time, never ship it

In a repo made from [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate), add Kenshi Races' release zip to
`meitou.lock.json`; the build downloads, checks and references it, and Play deploys it. Every value comes from the release's notes
(`kenshi-races-v<version>`):

```json
{
  "id": "kenshi.races",
  "repo": "Meitou-Modding-Ecosystem/KenshiRaces",
  "tag": "kenshi-races-v0.10.0",
  "asset": "KenshiRaces-0.10.0.zip",
  "sha256": "<Zip SHA-256 from the notes>",
  "folder": "KenshiRaces",
  "engine": "<Submodule engine commit from the notes>"
}
```

- Your `engine/` pin moves to that engine commit (GutenStructure
  [framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Never list `KenshiRaces.dll` in your own `assemblies`; the loaded `kenshi.races` serves the types.

## 2. Declare it in `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.23.0",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.10.0 <0.11.0" }]
```

- Before 1.0 a minor version may break the API, so cap the range at the next minor.
- Kenshi Races must sit above your mod in `data/mods.cfg` (the launcher's Arrange does it), so its records exist when your data phase runs.

## 3. Use its races

Refer to the ids in `Races` from your records, e.g. in your data phase:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs (subraces): `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`, `SpidercrabRace`, `GhostcrabRace`, `TieflingRace`, `BatfolkRace`, `WolfkinRace`, `BeepbotRace`, `OwlkindRace`.
- CHARACTERs (kitless, recruitable): `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
  `CrabmanCharacter`, `CrabQueenCharacter`, `SpidercrabCharacter`, `GhostcrabCharacter`, `TieflingMaleCharacter`, `TieflingFemaleCharacter`, `BatfolkMaleCharacter`, `BatfolkFemaleCharacter`,
  `WolfkinMaleCharacter`, `WolfkinFemaleCharacter`, `BeepbotCharacter`, `OwlkindCharacter`.
- RACE_GROUPs (the editor's races): `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`, `TieflingGroup`, `BatfolkGroup`, `WolfkinGroup`, `BeepbotGroup`, `OwlkindGroup`. `Races.Recruit` is the "Join us." talk.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen`, `Spidercrab`, `Ghostcrab`, `Tiefling`, `Batfolk`, `Wolfkin`, `Beepbot`, `Owlkind` (`Subrace`) and `Races.Groups` describe them. Table:
  [kenshi-races.md "Races"](kenshi-races.md#races).

## 4. Add a subrace or a race

1. Fit each body with the [race import](race-import.md) from a KenshiRaces clone, into your mod's `items/races` with your own stem
   (`mymod_<character>`; `female` as the rig for a female body), and commit the files. Add `!/<Mod>/items/races/*.mesh` and
   `!/<Mod>/items/races/*.dds` to `.gitignore`.
2. Describe it and add its records in your mod's data phase (the entry implements `IModDataPhase`):

```csharp
static readonly Subrace HermitCrab = new("hermitcrab", "30-MyMod.mod",
    Male: new RaceBody("MyMod", "mymod_hermitcrab", NormalMap: true), Female: null,
    Characters: [new RaceCharacter("hermitcrab", "31-MyMod.mod", Female: false)]);

public void EditData(ModDataContext c)
{
    RaceRecords.Add(c, HermitCrab);                                      // RACE + CHARACTERs, the recruit talk by default
    RaceRecords.JoinGroup(c, Races.CrabfolkGroup, HermitCrab.RaceId);  // the editor lists it under Crabfolk
}
```

- A subrace with a `Male` and a `Female` body gets the editor's gender arrow; with one it is single gender and every character wears it.
- A body imported `headless` (it wears the human head, [race import](race-import.md#headless)) is `new RaceBody(..., HumanHead: true)`: its
  subrace keeps the Greenlander's heads, hair and random faces and gets the face editor limits (`RaceRecords.FaceEditorLimits`: human face
  sliders, chest, bust).
- A body imported `skintone` as well ([race import](race-import.md#skin-tone)) is `new RaceBody(..., HumanHead: true, SkinTone: true)`: its
  `<stem>_mask.dds` becomes the RACE's `body mask` and the subrace gets `RaceRecords.TintedEditorLimits`, so one Skin Tone slider tints head
  and body alike.
- `RaceRecords.Add(c, subrace, talk)`: `talk` is a DIALOGUE id for the characters, `null` for none.
- A race of your own: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` adds the RACE_GROUP and every subrace.
- Subraces are `playable` with Kenshi Races' editor limits (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` fill records you add
  yourself (`c.Add`) when you want to change more fields after.
- The ids are yours: records of your own `.mod` file name.
- Traits: `new Subrace(..., Traits: new RaceTraits(good, bad) { Health = ..., NoHats, NoShoes, HatsOnly, NoFootwear, NoWeapons, Fists,
  DamageTaken })` sets aptitudes (x1.2 / x0.8), part hit points, gear limits, fists and the share of each blow that lands
  ([kenshi-races.md](kenshi-races.md#traits)); a hermit crab can reuse `Races.CrabfolkTraits`. Without traits a subrace keeps the Greenlander's.

## 6. Play its animations

Every clip of [animations.md](animations.md) is linked to every race on the game's human skeletons (a subrace added with `RaceRecords` too),
so a mod only plays it. A mod combat move is animation only, asked whenever the game plays nothing of its own (engine docs/modding.md,
"Combat moves"):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` is yours: a cheap, thread-safe, read-only answer from your own state, the clip's progress 0 to 1 (seconds / `clip.Length`).
- Clips play in place and face forward; a clip shorter than your move holds its last frame unless you wrap the progress.
- Animations need only the dependency (sections 1 and 2), no records of your own.

## 5. Check it

`dotnet build`, your tests, then in game F10 **Kenshi Races: races** lists your race with its mesh, and `spawn <name>` spawns it. Name subraces and CHARACTERs one lowercase word, e.g. `hermitcrab`, so the console takes them without quotes; a new game's editor lists your subrace.
