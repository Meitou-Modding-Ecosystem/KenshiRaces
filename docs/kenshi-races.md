# Kenshi Races

Five new races for the character editor, each with subraces, on the game's human skeletons (`kenshi.races`, no dependencies): every record
added at load, the bodies fitted by the [race import](race-import.md). It also ships an [animation library](animations.md) of 112 clips for
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
| | spidercrab `22-` | single | spidercrab `23-` |
| | ghostcrab `24-` | single | ghostcrab `25-` |
| Tiefling `33-` | tiefling `26-` | single | tiefling `27-` |
| Batfolk `34-` | batfolk `28-` | male, female | batfolkmale `29-`, batfolkfemale `35-` |
| Wolfkin `39-` | wolfkin `36-` | male, female | wolfkinmale `37-`, wolfkinfemale `38-` |

Ids end in `KenshiRaces.mod`. Subrace and CHARACTER names are one lowercase word, so the console's `spawn` takes them without quotes. Each body's
files are `KenshiRaces/items/races/kenshiraces_<character>*` (normal maps on Leoma, Crevan, the spider crab, the ghost crab, the Batfolk man and the Wolfkin). Male bodies are fitted to `male_skeleton`,
female ones to `female_skeleton`; the crab and tiefling bodies are male fits, so the crab queen and the tiefling (both women, `bust=female`)
are single gender with the male animations and voice (the game and Meitou make a single-gender race's characters male). The tiefling's tail is
carried by `Spine`. The Batfolk wear the human head (imported `wings headless`, [race import](race-import.md#headless)): the
Greenlander's heads, hair and face sliders on their own bodies, their arm-wing membranes spanning arm and flank, opening as the arms rise. The Wolfkin wear it too, with their own wolf
ears on the skull (imported `ears`, [race import](race-import.md#ears)). Source
models (Meshy exports, slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman (`neck=0.7`) and crabqueen (`bust=female`) "... - 3ds
Max Biped.glb", the others `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`, `spidercrab.glb`, `ghostcrab.glb` and
`tiefling-female.glb` (`bust=female`), `batman-rigged.glb` (male) and `batgirl-rigged.glb` (female), both `wings headless`, and `wolfman-rigged.glb` (male, a Mixamo rig)
and `wolfwoman-rigged.glb` (female), both `ears palms`.

## Records

Added in the data phase from the loaded game's own records (`RaceRecords`), so `KenshiRaces.mod` ships empty and CI needs no game:

- RACE_GROUP: the race's name and its subraces' RACEs, in the editor's order.
- RACE (subrace): a copy of Greenlander (`17-gamedata.quack`: stats, animations, voices) with each gender's mesh, colour texture, normal map
  and part map (a single-gender subrace wears its one body for both); `single gender` unless it has both bodies; no heads, hair or random faces
  (`morph num` 0; a race listing no head draws none; the body has its own) unless its bodies wear the human head (`RaceBody.HumanHead`),
  which keep the Greenlander's; `no shirts` (shirts and pants paint the human body's uv layout); body mask
  and strength normal maps cleared; `playable` with `editor limits` [`kenshiraces_editor.xml`](#character-editor)
  (`kenshiraces_editor_faces.xml` with the human head).
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
| crabman, crabqueen, spidercrab, ghostcrab | martial arts, strength, toughness, melee attack and defence, swimming, labouring, engineering | dodge, turrets, crossbows, precision shooting, stealth, lockpicking, thieving, assassination, field medic, weapon, armour and crossbow smith | 100 / 200 / 200 / 250 / 120 | `no hats`; no footwear named `Sandals`; no weapons or crossbows |
| tiefling, batfolk, wolfkin | (the Greenlander's) | (the Greenlander's) | 100 / 100 / 100 / 100 / 100 | all |

- The spider and ghost crabs share the Crabfolk traits until their own are settled; the tiefling, the Batfolk and the Wolfkin have none yet.
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

Races wearing the human head use `kenshiraces_editor_faces.xml`: the same plus the face sliders of the human head's poses (each pose -100 to
100), Chest, and for women Breast size, height and spacing. Their torsos are pulled onto the human's skin and carry its weights, which the
chest and bust sliders were made for; whether they stay clean in game is not yet checked.

## Recruiting

DIALOGUE `1-KenshiRaces.mod`: the character says "You look like you could use another pair of hands.", the player answers "Join us." and
action 18 (join squad) adds the character to the player's squad.

## Getting the races

Kenshi Races stays a framework: one NPC sells introductions, and the debug menu spawns the races. Code in `KenshiRaces/Introductions/`.

- **Marlo the Far-Walker** (CHARACTER `200-`, squad `201-`, talk `202-`): a Drifter Greenlander in Armoured Rags (`2307-clothes_v1.mod`) and
  an Ashland Hat (`55393-rebirth.mod`), of the Drifters (`51707-rebirth.mod`), in the bar of Black Scratch (TOWN `55656-rebirth.mod`; the bar
  is the home of its "Adventurer Waystation Bar" residents, `44856-rebirth.mod`, else its first bar). Once the town's zone is loaded the mod
  adds his platoon at the bar's first general node (else bar node), holds it in its name (saved, so he is added once) and walks him back
  whenever he stands idle away from it.
- **Introduction**: he tells of his travels beyond the maps and the friends he made there. "Introduce me to one of your friends." leads to a
  race, then a character; each character choice needs 10,000 cats (DC 2) and no introduction on its way (condition 5000), pays him (DA 8) and
  orders it (effect 5000, value = the character's record number). Three game days later the friend's platoon (squad `300 + number`, the
  character alone, Drifters) is added 300 to 500 units from the player's first outpost and sent there; with no outpost the player is told
  once and it waits until there is one. Loaded, it walks to the outpost's centre and offers to join with the [recruit talk](#recruiting).
  The introduction ends when the friend joins, dies or leaves the world; until then Marlo refuses another. Messages tell the player each step.
- **F10 Kenshi Races: spawn race**: the field takes a character's or subrace's name from the [table](#races) (`crabman`, `leomafemale`,
  `leoma female`) and `join` (default: into the selected character's squad, else the first player squad) or `hostile` (alone, as a Dust
  Bandit, `200-gamedata.base`, held where it stands); it spawns at the camera target.
- The pending introduction is saved in the mod's save data (system `introductions`).

## Testing in game

1. Enable Kenshi Races in the launcher (or `data/mods.cfg`).
2. F10 **Kenshi Races: spawn race** with `leomafemale` (or `crabman hostile`) spawns at the camera target; the console's `spawn leomafemale`
   also works (talk to recruit).
3. F10 **Kenshi Races: races** lists every fitted race loaded (this mod's and dependents').
4. New game: the editor's race arrows reach Leoma, Crevan and Crabfolk.
5. Black Scratch's bar: Marlo sells an introduction for 10,000 cats; it arrives at your outpost three game days later.

## Clothing

Armour shares the body's skeleton, so it moves with these bodies, but it is modelled around the human body. The part map hides the skin
under items with `hide parts` (coats, robes, jackets); slimming and the skin conform pull the torso, hips, legs and feet onto the human's skin
([race import](race-import.md#slimming), [skin conform](race-import.md#skin-conform)). Shapes far from human (tails, manes, shells, claws) still
poke through tight armour.
