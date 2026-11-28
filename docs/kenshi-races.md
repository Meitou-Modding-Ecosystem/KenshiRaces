# Kenshi Races

Seven new races for the character editor, each with subraces, and three new subraces of the game's Skeleton (the beepbot, the Kat-Lon and the chibot), on the game's human skeletons (`kenshi.races`, no dependencies): every record
added at load, the bodies fitted by the [race import](race-import.md). It also ships an [animation library](animations.md) of 190 clips for
every race on those skeletons. Mods use the races and clips, add subraces or add their own races:
[depending-on-kenshi-races.md](depending-on-kenshi-races.md).

## Races

The editor lists each RACE_GROUP as a race and its RACEs as subraces (engine docs/characters.md "Character editor"). The beepbot, the Kat-Lon and the
chibot join the game's own Skeleton RACE_GROUP (an edit adding them to its list), so the editor shows them beside the Skeleton's own subraces.

| Race (RACE_GROUP) | Subrace (RACE) | Genders | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | male, female | leomamale `11-`, leomafemale `17-` |
| Crevan `31-` | crevan `18-` | male, female | crevanmale `19-`, crevanfemale `21-` |
| Crabfolk `32-` | crabman `12-` | single | crabman `13-` |
| | crabqueen `14-` | single | crabqueen `15-` |
| | spidercrab `22-` | single | spidercrab `23-` |
| | ghostcrab `24-` | single | ghostcrab `25-` |
| Tiefling `33-` | tiefling `26-` | male, female | tieflingmale `42-`, tieflingfemale `27-` |
| Batfolk `34-` | batfolk `28-` | male, female | batfolkmale `29-`, batfolkfemale `35-` |
| Wolfkin `39-` | wolfkin `36-` | male, female | wolfkinmale `37-`, wolfkinfemale `38-` |
| Skeleton (the game's, `42903-rebirth.mod`) | beepbot `16-` | single | beepbot `20-` |
| | katlon `47-` | single | katlon `48-` |
| | chibot `49-` | single | chibot `51-` |
| Owlkind `45-` | owlkind `43-` | male, female | owlkindmale `46-`, owlkindfemale `44-` |

Ids end in `KenshiRaces.mod`. Subrace and CHARACTER names are one lowercase word, so the console's `spawn` takes them without quotes. Each body's
files are `KenshiRaces/items/races/kenshiraces_<character>*` (normal maps on Leoma, Crevan, the spider crab, the ghost crab, the tiefling man, the Batfolk man, the Wolfkin, the beepbot, the Kat-Lon and the chibot). Male bodies are fitted to `male_skeleton`,
female ones to `female_skeleton`; the crab bodies are male fits, so the crab queen (a woman, `bust=female`) is single gender with the male
animations and voice (the game and Meitou make a single-gender race's characters male). The tieflings wear the human head without their own
hair and their skin takes the editor's skin tone with it (imported `nohair skintone`, [race import](race-import.md#skin-tone)); the woman's model has no skin under its long hair, so the
human's skin closes her upper back ([hair holes](race-import.md#headless)); ram horns ride the head (`attach=horns.glb`, [attachments](race-import.md#attachments)). The Batfolk wear the human head (imported `wings headless`, [race import](race-import.md#headless)): the
Greenlander's heads, hair and face sliders on their own bodies, their arm-wing membranes spanning arm and flank, opening as the arms rise. The Wolfkin wear it too; both races wear
wolf ears on the skull, and the Batfolk folded bat wings on their backs ([attachments](race-import.md#attachments)). The beepbot, a skeletal robot, is a male fit imported `rigid`
([race import](race-import.md#rigid)): its ribs, springs and joints keep their shape and its own weights. The Kat-Lon, a skeletal robot with a broad
swept-back helmet head, and the chibot, a slim one with a woman's figure and looped braids (a male fit, single gender like
every Skeleton subrace), are imported the same way. The Owlkind keep their own beaked,
feathered heads and talons; imported `palms` ([hands](race-import.md#hands)), the man's palms, which face forward in his model, hang to the
thigh, and both models' thumbs, which Meshy hangs off the forearm, follow the hands. Source
models (Meshy exports, slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman (`neck=0.7`) and crabqueen (`bust=female`) "... - 3ds
Max Biped.glb", the others `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`, `spidercrab.glb`, `ghostcrab.glb` and
`tiefling-female.glb` (female, `hairback skintone attach=horns.glb`) and `tieflingmale-rigged.glb` (male, `nohair palms relief=0.4 attach=horns.glb`: its palms faced forward, its muscles too defined), `batman-rigged.glb` (male) and `batgirl-rigged.glb` (female), both `wings headless attach=wolfears.glb attach=batwings.glb`, and
`wolfman-rigged.glb` (male, a Mixamo rig) and `wolfwoman-rigged.glb` (female), both `headless palms attach=wolfears.glb`; owlkindmale from `owlkindmale-rigged.glb` (`male palms`) and owlkindfemale from `owlkindfemale-rigged.glb` (`female palms`); beepbot from `beepbot-highpoly.glb` (1M vertices, unrigged), decimated to 40,000
triangles with its normals baked, carrying the weights of the Meshy rig of the low-poly `beepbot-rigged.glb` (nearest face, same frame after
that rig's scale 1.711 and lift 0.85 m), `rigid`; katlon from `kittycatlon-rigged.glb` (3,550 vertices, Meshy rig) `male rigid`; chibot from `chibot-rigged.glb`
(3,443 vertices, Meshy rig, unrigged twin `chibot.glb`) `male rigid`.

## Records

Added in the data phase from the loaded game's own records (`RaceRecords`), so `KenshiRaces.mod` ships empty and CI needs no game:

- RACE_GROUP: the race's name and its subraces' RACEs, in the editor's order.
- RACE (subrace): a copy of Greenlander (`17-gamedata.quack`: stats, animations, voices) with each gender's mesh, colour texture, normal map
  and part map (a single-gender subrace wears its one body for both); `single gender` unless it has both bodies; no heads, hair or random faces
  (`morph num` 0; a race listing no head draws none; the body has its own) unless its bodies wear the human head (`RaceBody.HumanHead`),
  which keep the Greenlander's; `no shirts` (shirts and pants paint the human body's uv layout); `body mask` the body's skin-tone mask
  (`RaceBody.SkinTone`) or cleared; strength normal maps cleared; `playable` with `editor limits` [`kenshiraces_editor.xml`](#character-editor)
  (`kenshiraces_editor_faces.xml` with the human head, `kenshiraces_editor_tinted.xml` with a skin tone).
- CHARACTER: a copy of Drifter (`2205-gamedata.base`) of the subrace and gender (`female chance` 100 or 0), no kit, money or bounty, with the
  recruit talk.
- Engine: a CHARACTER's own `mesh` field overrides its race's body mesh.
- WEAPON `50-` "FISTS- crab": the Crabfolk's fists ([Traits](#traits)).

## Traits

Each subrace's `RaceTraits` replace those of the game RACE it is copied from (`RaceTraits.Model`, `RaceRecords.FillRace`): the Greenlander,
or the Skeleton (`17946-stick_people.mod`) for a skeleton subrace. Empty aptitude lists keep the model's, as do unset part health and fists. Aptitudes are the RACE's `stats good` (x1.2 starting value and
XP) and `stats bad` (x0.8), `strength` 1.2 for strength, which the game lays over the lists (engine docs/game/character-stats.md "Races");
"all weapons but ranged" are Katanas, Sabres, Hackers, Heavy Weapons, Blunt and Polearms; precision shooting is stat 36. Part health is each
`combat anatomy` entry's value 1, the part's base hit points (the Greenlander's 100).

| Subrace | +20% | -20% | Head / chest / stomach / arms / legs | Gear |
| --- | --- | --- | --- | --- |
| leoma | strength, sabres, athletics, weapon smith, labouring | armour smith, farming, polearms, heavy weapons, robotics | 100 / 120 / 120 / 120 / 120 | all |
| crevan | perception, stealth, lockpicking, thieving, assassination, robotics, dodge | the weapons above, farming, labouring | 140 / 100 / 100 / 90 / 90 | `no shoes`; head gear only named `Ashland...` (the Ashland Hat, the Ashlander Stormgoggles) |
| crabman, crabqueen, spidercrab, ghostcrab | martial arts, strength, toughness, melee attack and defence, swimming, labouring, engineering | dodge, turrets, crossbows, precision shooting, stealth, lockpicking, thieving, assassination, field medic, weapon, armour and crossbow smith | 100 / 200 / 200 / 250 / 120 | `no hats`; no footwear named `Sandals`; no weapons or crossbows |
| beepbot, katlon, chibot (Skeleton model) | heavy weapons, turrets, robotics (the Skeleton's) | thieving, stealth, dodge (the Skeleton's) | 200 / 200 / 200 / 200 / 200 (the Skeleton's) | all hats; `no shoes` |
| tiefling, batfolk, wolfkin, owlkind | (the Greenlander's) | (the Greenlander's) | 100 / 100 / 100 / 100 / 100 | all |

- The spider and ghost crabs share the Crabfolk traits until their own are settled; the tiefling, the Batfolk, the Wolfkin and the Owlkind have none yet.
- The beepbot, the Kat-Lon and the chibot keep the rest of the Skeleton's RACE (base data, `Meitou.Tools record Skeleton`): `is robot`, `hunger rate` 0, `bleed rate`
  0.1, `heal stat` 6 (repaired with Robotics), `heal rate` 2, weather immunity, `FISTS- robo`, no swimming, `combat move speed mult` 0.9,
  `vision range mult` 1.25, and no limb replacements or severed limbs; its own body, no head and the animation library are set over them.
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

Skin-toned races (the tieflings) use `kenshiraces_editor_tinted.xml`: the faces file with a Skin Tone slider of 11 colours, the human tones
(white, 251/219/219, 201/190/180, 137/119/102), then crimson, purple (the middle, so the editor's default), rose, plum, indigo, blue-grey and
ash. A colour multiplies the Greenlander head's bare skin (204, 143, 112), which the body's skin is repainted to, so each is the wanted skin
over that, at most 255 a channel (blue cannot rise above 112). Head and body take the same colour.

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
  Bandit, `200-gamedata.base`, held where it stands); it spawns at the camera target. A joining character puts on a Standard (quality 40)
  Dustcoat (`3061-gamedata.base`) to show the clothing's fit, and the character editor opens on it as a recruit's edit (race and gender kept).
- The pending introduction is saved in the mod's save data (system `introductions`).

## Testing in game

1. Enable Kenshi Races in the launcher (or `data/mods.cfg`).
2. F10 **Kenshi Races: spawn race** with `leomafemale` spawns her at the camera target in a Dustcoat and opens the editor on her (`crabman
   hostile` spawns a hostile one); the console's `spawn leomafemale` also works (talk to recruit).
3. F10 **Kenshi Races: races** lists every fitted race loaded (this mod's and dependents').
4. New game: the editor's race arrows reach Leoma, Crevan and Crabfolk.
5. Black Scratch's bar: Marlo sells an introduction for 10,000 cats; it arrives at your outpost three game days later.

## Clothing

Armour shares the body's skeleton, so it moves with these bodies, but it is modelled around the human body. The part map hides the skin
under items with `hide parts` (coats, robes, jackets); slimming and the skin conform pull the torso, hips, legs, feet, arms and the front of the crotch onto the human's skin
([race import](race-import.md#slimming), [skin conform](race-import.md#skin-conform)). Shapes far from human (tails, manes, shells, claws) and the
hands still poke through tight armour and gloves.
