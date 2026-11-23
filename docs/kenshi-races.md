# Kenshi Races

New playable-looking races on the game's human skeleton (`kenshi.races`, no dependencies): each a RACE and a CHARACTER added at load, its body
fitted by the [race import](race-import.md). Mods use them or add their own: [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

## Races

| Race | RACE | CHARACTER | Files (`KenshiRaces/items/races/`) | Normal map |
| --- | --- | --- | --- | --- |
| Leonid | `10-KenshiRaces.mod` | `11-KenshiRaces.mod` | `kenshiraces_leonid*` | yes |
| Crabman | `12-KenshiRaces.mod` | `13-KenshiRaces.mod` | `kenshiraces_crabman*` | no |
| Crab Queen | `14-KenshiRaces.mod` | `15-KenshiRaces.mod` | `kenshiraces_crabqueen*` | no |

Source models: Leonid "Leonid Bare - 3ds Max Biped.glb" (slim 1), Crabman and Crab Queen "... - 3ds Max Biped.glb" (slim 1), Meshy exports.

## Records

Added in the data phase from the loaded game's own records (`RaceRecords`), so `KenshiRaces.mod` ships empty and CI needs no game:

- RACE: a copy of Greenlander (`17-gamedata.quack`: stats, animations, voices, limits) with the body's mesh, colour texture, normal map and part
  map for both genders; single gender; no heads or hair (a race listing no head draws none; the body has its own); `morph num` 0; `no shirts`
  (shirts and pants paint the human body's uv layout); not `playable` at character creation; body mask and strength normal maps cleared.
- CHARACTER: a copy of Drifter (`2205-gamedata.base`) of the race, always male (the bodies are fitted to `male_skeleton`), no kit, money or
  bounty, with the recruit talk.
- Engine: a CHARACTER's own `mesh` field overrides its race's body mesh.

## Recruiting

DIALOGUE `1-KenshiRaces.mod`: the character says "You look like you could use another pair of hands.", the player answers "Join us." and
action 18 (join squad) adds the character to the player's squad.

## Testing in game

1. Enable Kenshi Races in the launcher (or `data/mods.cfg`).
2. Console: `spawn "Leonid"` (or `"Crabman"`, `"Crab Queen"`) spawns at the cursor; talk to recruit, or F10 debug Recruit.
3. F10 **Kenshi Races: races** lists every fitted race loaded (this mod's and dependents').

## Clothing

Armour shares the body's skeleton, so it moves with these bodies, but it is modelled around the human body. The part map hides the skin
under items with `hide parts` (coats, robes, jackets); slimming pulls in what is bulkier than a human elsewhere
([race import](race-import.md#slimming)). Shapes far from human (shells, claws) still poke through tight armour.
