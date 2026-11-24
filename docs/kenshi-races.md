# Kenshi Races

Three new races for the character editor, each with subraces, on the game's human skeletons (`kenshi.races`, no dependencies): every record
added at load, the bodies fitted by the [race import](race-import.md). It also ships an [animation library](animations.md) of 88 clips for
every race on those skeletons. Mods use the races and clips, add subraces or add their own races:
[depending-on-kenshi-races.md](depending-on-kenshi-races.md).

## Races

The editor lists each RACE_GROUP as a race and its RACEs as subraces (engine docs/characters.md "Character editor").

| Race (RACE_GROUP) | Subrace (RACE) | Genders | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | male, female | leomamale `11-`, leomafemale `17-` |
| Crevan `31-` | crevan `18-` | male, female | crevanmale `19-`, crevanfemale `21-` |
| Crabfolk `32-` | crabman `12-` | single | crabman `13-` |
| | crabqueen `14-` | single | crabqueen `15-` |

Ids end in `KenshiRaces.mod`. Subrace and CHARACTER names are one lowercase word, so the console's `spawn` takes them without quotes. Each body's
files are `KenshiRaces/items/races/kenshiraces_<character>*` (normal maps on Leoma and Crevan). Male bodies are fitted to `male_skeleton`,
female ones to `female_skeleton`; the crab bodies are male fits, so a crab queen is single gender with the male animations and voice. Source
models (Meshy exports, slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman (`neck=0.7`) and crabqueen (`bust=female`) "... - 3ds
Max Biped.glb", the others `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

## Records

Added in the data phase from the loaded game's own records (`RaceRecords`), so `KenshiRaces.mod` ships empty and CI needs no game:

- RACE_GROUP: the race's name and its subraces' RACEs, in the editor's order.
- RACE (subrace): a copy of Greenlander (`17-gamedata.quack`: stats, animations, voices) with each gender's mesh, colour texture, normal map
  and part map (a single-gender subrace wears its one body for both); `single gender` unless it has both bodies; no heads or hair (a race
  listing no head draws none; the body has its own); `morph num` 0; `no shirts` (shirts and pants paint the human body's uv layout); body mask
  and strength normal maps cleared; `playable` with `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTER: a copy of Drifter (`2205-gamedata.base`) of the subrace and gender (`female chance` 100 or 0), no kit, money or bounty, with the
  recruit talk.
- Engine: a CHARACTER's own `mesh` field overrides its race's body mesh.
- WEAPON `50-` "FISTS- crab": the Crabfolk's fists ([Traits](#traits)).

## Traits

Each subrace's `RaceTraits` replace the Greenlander's (`RaceRecords.FillRace`). Aptitudes are the RACE's `stats good` (x1.2 starting value and
XP) and `stats bad` (x0.8), `strength` 1.2 for strength, which the game lays over the lists (engine docs/game/character-stats.md "Races");
"all weapons but ranged" are Katanas, Sabres, Hackers, Heavy Weapons, Blunt and Polearms; precision shooting is stat 36. Part health is each
`combat anatomy` entry's value 1, the part's base hit points (the Greenlander's 100).

| Subrace | +20% | -20% | Head / chest / stomach / arms / legs | Gear |
| --- | --- | --- | --- | --- |
| leoma | strength, sabres, athletics, weapon smith, labouring | armour smith, farming, polearms, heavy weapons, robotics | 100 / 120 / 120 / 120 / 120 | all |
| crevan | perception, stealth, lockpicking, thieving, assassination, robotics, dodge | the weapons above, farming, labouring | 140 / 100 / 100 / 90 / 90 | `no shoes`; head gear only named `Ashland...` (the Ashland Hat, the Ashlander Stormgoggles) |
| crabman, crabqueen | martial arts, strength, toughness, melee attack and defence, swimming, labouring, engineering | dodge, turrets, crossbows, precision shooting, stealth, lockpicking, thieving, assassination, field medic, weapon, armour and crossbow smith | 100 / 200 / 200 / 250 / 120 | `no hats`; no footwear named `Sandals`; no weapons or crossbows |

- Gear limits beyond the RACE's slot flags are a wear rule (engine docs/modding.md "Wear rules", `RaceRules`) read from the RACE's own
  fields `kenshi races hats only` and `kenshi races no footwear` ('|'-separated names) and `kenshi races no weapons`, so a dependent's subrace
  with traits gets them. They refuse equipping, buying into a slot and a loaded save's worn gear; a generated character's weapons are not
  checked (Kenshi Races' characters spawn with none).
- Crabfolk fists: a copy of the skeletons' `FISTS- robo` (cut x1.2, bleed 0.2, robot damage 1; the people's `FISTS`: 1, 0.05, 0.6) with cut
  x1.3 and blunt x1.1, named by the RACE's `fists lv1` and `fists lv100`.
- Crabfolk shell: every blow landing on them (melee, bolt, execution) is x0.8 before armour and toughness (a hit filter, engine docs/modding.md
  "Hits before they land"; RACE field `kenshi races damage taken`). The game's RACE has no damage resistance: skeletons only take less from
  weapons whose `robot damage mult` is below 1 (**Verified**, base data: 40 of 54 WEAPONs 1, `FISTS` and 5 more 0.6, 6 above 1).

## Character editor

The races are offered at character creation like the game's own. `KenshiRaces/items/races/kenshiraces_editor.xml` keeps only the sliders that
move bones (engine docs/animation.md, the bone scales): Face holds Head size, Head shape and the neck sliders (no face poses: the fitted heads
have none); Body holds Height, Frame, Posture, Shoulder set, Neck position, Leg length, Shoulders, Arm bulk, Hands, Feet and Skin Tone.
Chest, Stomach, Waist, Hips, Legs bulk, Legs shape and the bust sliders are left out: they scale the spine, pelvis, thigh and bust bones
unevenly, which warped the fitted chests, busts and the small of the Leoma's back even with human weights (**Observed** in game, 2026-11-23).
Ranges are the human ones (`data/editor/editor_data_human.xml`).

## Recruiting

DIALOGUE `1-KenshiRaces.mod`: the character says "You look like you could use another pair of hands.", the player answers "Join us." and
action 18 (join squad) adds the character to the player's squad.

## Testing in game

1. Enable Kenshi Races in the launcher (or `data/mods.cfg`).
2. Console: `spawn leomafemale` (or any CHARACTER in the table) spawns at the cursor; talk to recruit, or F10 debug Recruit.
3. F10 **Kenshi Races: races** lists every fitted race loaded (this mod's and dependents').
4. New game: the editor's race arrows reach Leoma, Crevan and Crabfolk.

## Clothing

Armour shares the body's skeleton, so it moves with these bodies, but it is modelled around the human body. The part map hides the skin
under items with `hide parts` (coats, robes, jackets); slimming pulls in what is bulkier than a human elsewhere
([race import](race-import.md#slimming)). Shapes far from human (shells, claws) still poke through tight armour.
