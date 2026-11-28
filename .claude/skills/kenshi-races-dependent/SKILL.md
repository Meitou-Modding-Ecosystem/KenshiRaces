---
name: kenshi-races-dependent
description: Use when making a Meitou code mod depend on Kenshi Races, using the Leoma, Crevan, Crabfolk, Tiefling, Batfolk, Wolfkin or Owlkind races, the Skeleton subraces beepbot, Kat-Lon and chibot, adding a subrace (e.g. a hermit crab), playing its animation library (wave, dance, throw...), or fitting a new rigged .glb body as a race.
---

# Build on Kenshi Races

Kenshi Races (`kenshi.races`) owns the fitted races and the body-fitting tool. Your mod refers to its races by id or adds its own fitted races
with its helper; it never copies its records or files. Read `docs/depending-on-kenshi-races.md` first, and `docs/race-import.md` for a new body.

## Hard rules

- One framework: reference `KenshiRaces` with `Private="false"` (the framework lock does); never list `KenshiRaces.dll` in your `assemblies`.
- Records only in your data phase (`IModDataPhase.EditData`); Kenshi Races loads above your mod in `mods.cfg`.
- Fitted files: unique stem `<modfolder lower case>_<race>`, in your mod's `items/races`, committed; never commit the source `.glb` or any
  game file. The import needs the game install (skeleton, human body, part map), so it runs locally or in a cloud session with the game data,
  never in CI.

## Steps

1. **Manifest**: `"dependencies": [{ "id": "kenshi.races", "version": ">=0.10.0 <0.11.0" }]` (match the current `KenshiRaces/meitou.json`
   minor), `gameVersion` floor at least 0.23.0.
2. **Project**: add a `kenshi.races` entry to `meitou.lock.json` from the release notes of `kenshi-races-v<version>`, and move `engine/` to its
   engine commit.
3. **Use a race**: link a subrace RACE (`Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`, `SpidercrabRace`, `GhostcrabRace`, `TieflingRace`, `BatfolkRace`, `WolfkinRace`, `BeepbotRace`, `KatlonRace`, `ChibotRace`, `OwlkindRace`) in your CHARACTER's `race` list; set
   `female chance` for leoma, crevan, batfolk and wolfkin, which have both genders.
4. **New subrace**: per body `dotnet run --project <KenshiRaces>/tools/RaceImport -c Release -- <model.glb> <Kenshi> <Mod>/items/races <stem>
   [male|female] [slim] [bust=female] [neck=<share>] [wings] [headless] [ears] [palms] [rigid] [attach=<part.glb>]`; check the log line (scale near 10 to 13 for a human-sized model, hideable vertices
   above 0, human weights on hundreds of vertices (none with `rigid`, for mechanical bodies whose parts must not stretch), a tail's vertices on the pelvis for a tailed body). Then `RaceRecords.Add(c, new Subrace(...))` and `RaceRecords.JoinGroup(c, Races.CrabfolkGroup, id)`
   (or `AddGroup` for a race of your own). Give it `Traits:` (`RaceTraits`: `Model` (`RaceModel.Skeleton` for a robot, e.g. `Races.BeepbotTraits`), aptitudes, `Health`, gear limits, `Fists`, `DamageTaken`), e.g.
   `Races.CrabfolkTraits` for a crab. A `headless` body (human head, face sliders, hair) is `RaceBody(..., HumanHead: true)`, plus `SkinTone: true` when imported `skintone` (one Skin Tone slider tints head and body); `wings` binds
   arm-wing membranes between arm and flank; `attach=<part.glb>` merges a static part (ears, back wings, horns) placed by
   `tools/RaceImport/parts/<part>.json` (race-import.md "Attachments"). Tell the user the race mod goes above every other animation mod
   (Husbandry...) in the mod list (animations.md "Mod order").
5. **Animations**: pick a clip from `docs/animations.md`, get its technique with `AnimationRecords.Technique(c.Data, RaceAnimations.X)` in
   `Initialize`, and answer `c.AddCombatMove` with `new CombatMove(technique, progress)` (progress 0 to 1, your own state, thread-safe).
6. **Docs**: list the mod's races in its own docs; a change to `RaceBody`, `Subrace`, `RaceTraits`, `RaceCharacter`, `RaceGroup`, `RaceRecords`, `Races`, `RaceAnimation(s)` or `AnimationRecords` updates
   `docs/depending-on-kenshi-races.md`, `docs/kenshi-races.md` and this skill, and bumps Kenshi Races' minor version.
7. **Check**: `dotnet build`, `dotnet test`; in game F10 **Kenshi Races: races** and `spawn <name>` (name subraces and characters one lowercase word, e.g. `hermitcrab`); a new game's editor lists the subrace.
