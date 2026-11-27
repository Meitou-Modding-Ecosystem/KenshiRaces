# Kenshi Races

[Français](translations/fr/README.md) · [Deutsch](translations/de/README.md) · [Español](translations/es/README.md) · [Italiano](translations/it/README.md) · [Português do Brasil](translations/pt-BR/README.md) · [Русский](translations/ru/README.md) · [日本語](translations/ja/README.md) · [한국어](translations/ko/README.md) · [简体中文](translations/zh-CN/README.md)

New races for [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) on the game's human skeletons, playable in the character
editor: Leoma and Crevan (male and female), the Crabfolk (crabman, crabqueen, spidercrab, ghostcrab), the Tiefling, the Batfolk and the Wolfkin (male and female, with human faces), the beepbot and the Kat-Lon (skeletal robots, subraces of the game's Skeleton), the Owlkind (male and female), an animation library of 188 clips for every human-skeleton race, and the framework
other mods add their own races and subraces with. GPL-3.0-or-later.

- The races, their records and testing them in game: [docs/kenshi-races.md](docs/kenshi-races.md).
- Using these races or adding your own from another mod: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (agent skill
  `.claude/skills/kenshi-races-dependent`).
- The animation library (188 clips for every race on the human skeletons) and how it was made: [docs/animations.md](docs/animations.md).
- Fitting a rigged `.glb` body (Meshy, Hi3D "3ds Max Biped") onto the game's skeleton: [docs/race-import.md](docs/race-import.md).
- The whole character pipeline, `.glb` to playable race (hands, chest, clothing, sliders), and how it compares with the Motion Lab: [docs/character-pipeline.md](docs/character-pipeline.md).

- Build and run locally with Visual Studio, VS Code or the terminal (this repo's engine or the installed launcher, deployed to the game): [docs/play.md](docs/play.md).
- Setting up the repos on your machine: [docs/local-development.md](docs/local-development.md).
- Instructing agents on a change that spans repos: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- Agent rules for the engine pin, versions, CI and releases: [CLAUDE.md](CLAUDE.md) (same as `AGENTS.md`); step-by-step procedures:
  `.claude/skills/meitou-engine-release`.
- Checking a built mod against the newest Meitou release (CI does it on every PR): [docs/refcheck.md](docs/refcheck.md).
- Publishing the mod to the Steam Workshop for the first time, then on every release: [docs/steam-workshop.md](docs/steam-workshop.md).
- Turning `.glb` models (Meshy, Hi3D, Blender) into buildable structures, no FCS needed: [docs/structures.md](docs/structures.md);
  getting a generated model's polycount, size and textures right first: [docs/model-prep.md](docs/model-prep.md).

## Numbers

Task types (`AddGoalType`, `AddJobType`) and dialogue codes (`AddDialogueCondition`, `AddDialogueEffect`) are shared by every loaded mod
([mod number blocks](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)).

Block 5000-5099.

| Number | Pool | Use |
| --- | --- | --- |
| 5000 | Dialogue condition | No introduction from Marlo is on its way (value 1), or one is (value 0) |
| 5000 | Dialogue effect | Marlo sends for the race character whose record number is the value |

## Layout

- `engine/`: MeitouClient as a git submodule, pinned to the engine commit the mod builds against. `git pull` does not move it: after
  every pull run `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`
  ([keep clones current](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`: the mod's project and `meitou.json`; `KenshiRaces/items/races/`: the fitted bodies (committed import output).
  `KenshiRaces.Tests/`: its tests (synthetic data, no game install needed).
  `<Mod>.Data/` (optional): a generator that writes `<Mod>.mod` from the installed game. `<Mod>/Structures/` (optional): `.glb` models
  imported as buildable structures on deploy ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`: the framework releases the mods build against
  ([framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)); the Framework update workflow moves it
  to new releases and merges when CI passes ([framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`: one-click build, deploy and run.
- `tools/`: `Update-Local.ps1`, the local pull and engine-pin check ([keep clones current](docs/local-development.md#keep-clones-current));
  `Switch-Engine.ps1` and `Use-ForkEngine.ps1`, local engine switching ([play every mod on the fork](docs/local-development.md#play-every-mod-on-the-fork));
  `refcheck.sh` and `RefCheck/`, the engine-reference check ([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`, the model import ([docs/structures.md](docs/structures.md)); `RaceImport/`, the body fitting
  ([docs/race-import.md](docs/race-import.md)); `model-info.py` and `prepare-model.py`,
  model checks and texture shrinking ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` imports the engine's, so mods build with the game's settings (warnings are errors).
- `Directory.Build.rsp` stamps the engine references as the oldest Meitou release the mods support, equal to each `gameVersion` floor: the
  game refuses a mod referencing a newer engine stamp, and the submodule builds `1.0.0.0`
  ([versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). The Meitou compatibility workflow checks the
  mods daily against new Meitou releases ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

## Branches

PRs into `beta`; CI (build and tests) must pass. `beta` is promoted to `main` once tested in play, and production goes through a PR to
`release`. Each merge into `release` tags it `<slug>-v<version>` and publishes the mod's zip (`.github/workflows/release.yml`) unless that
tag exists; `tools\Tag-Release.ps1` is the manual fallback.

- New-repository branch protections and permanent beta/main/release reviews: [docs/branch-policy.md](docs/branch-policy.md). Templates copy source; GutenStructure applies settings through reconciliation.
