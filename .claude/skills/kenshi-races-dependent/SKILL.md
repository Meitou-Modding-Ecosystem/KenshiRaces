---
name: kenshi-races-dependent
description: Use when making a Meitou code mod depend on Kenshi Races, using the Leonid, Crabman or Crab Queen races, or fitting a new rigged .glb body as a race.
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

1. **Manifest**: `"dependencies": [{ "id": "kenshi.races", "version": ">=0.1.0 <0.2.0" }]` (match the current `KenshiRaces/meitou.json`
   minor), `gameVersion` floor at least 0.22.2 (data phase).
2. **Project**: add a `kenshi.races` entry to `meitou.lock.json` from the release notes of `kenshi-races-v<version>`, and move `engine/` to its
   engine commit.
3. **Use a race**: link `Races.LeonidRace` (or `CrabmanRace`, `CrabQueenRace`) in your CHARACTER's `race` list.
4. **New race**: `dotnet run --project <KenshiRaces>/tools/RaceImport -c Release -- <model.glb> <Kenshi> <Mod>/items/races <stem> [slim]`; check
   the log line (scale near 10 to 13 for a human-sized model, hideable vertices above 0). Then `RaceRecords.Add(c, new RaceBody(...))`.
5. **Docs**: list the mod's races in its own docs; a change to `RaceBody`, `RaceRecords` or `Races` updates
   `docs/depending-on-kenshi-races.md`, `docs/kenshi-races.md` and this skill, and bumps Kenshi Races' minor version.
6. **Check**: `dotnet build`, `dotnet test`; in game F10 **Kenshi Races: races** and `spawn "<name>"`.
