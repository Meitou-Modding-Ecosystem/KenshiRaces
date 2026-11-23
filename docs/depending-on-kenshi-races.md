# Depending on Kenshi Races

How a code mod uses Kenshi Races' races or adds its own fitted races (`kenshi.races`). Loader rules: engine
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
  "tag": "kenshi-races-v0.1.0",
  "asset": "KenshiRaces-0.1.0.zip",
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
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.1.0 <0.2.0" }]
```

- Before 1.0 a minor version may break the API, so cap the range at the next minor.
- Kenshi Races must sit above your mod in `data/mods.cfg` (the launcher's Arrange does it), so its records exist when your data phase runs.

## 3. Use its races

Refer to the ids in `Races` from your records, e.g. in your data phase:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leonidguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeonidMaleRace, 100, 0, 0)];
}
```

`Races.LeonidMaleRace`, `LeonidFemaleRace`, `CrabmanRace`, `CrabQueenRace`, `CorvidMaleRace`, `CorvidFemaleRace` (RACE), `...Character` (a kitless recruitable CHARACTER), `Races.Recruit` (the "Join
us." talk). Table: [kenshi-races.md "Races"](kenshi-races.md#races).

## 4. Add your own race

1. Fit the body with the [race import](race-import.md) from a KenshiRaces clone, into your mod's `items/races` with your own stem
   (`mymod_<race>`), and commit the four files. Add `!/<Mod>/items/races/*.mesh` and `!/<Mod>/items/races/*.dds` to `.gitignore`.
2. Describe it and add its records in your mod's data phase (the entry implements `IModDataPhase`):

```csharp
static readonly RaceBody Naga = new("Naga", "30-MyMod.mod", "31-MyMod.mod", "MyMod", "mymod_naga", NormalMap: true);

public void EditData(ModDataContext c) => RaceRecords.Add(c, Naga);              // the recruit talk by default
```

- `RaceRecords.Add(c, body, talk)`: `talk` is a DIALOGUE id for the character, `null` for none. `CharacterId` null adds only the RACE.
- `FillRace` / `FillCharacter` fill records you add yourself (`c.Add`) when you want to change more fields after.
- The ids are yours: records of your own `.mod` file name.

## 5. Check it

`dotnet build`, your tests, then in game F10 **Kenshi Races: races** lists your race with its mesh, and `spawn "<name>"` spawns it.
