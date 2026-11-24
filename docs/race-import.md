# Race import: fitting a body onto the game's skeleton

`tools/RaceImport` turns a rigged `.glb` body (Meshy, Hi3D: the "3ds Max Biped" rig, joints `Bip001 ...`) into a body the game animates with
its own human animations: a skinned Ogre mesh linked to `male_skeleton.skeleton` or `female_skeleton.skeleton`, its DDS textures and a part
map. It reads the rig's skeleton, human body and part map from the game install, so it runs on a machine with Kenshi, never in CI; its output is committed.

## Run it

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim] [bust=male|female] [neck=<share>]
```

- `<Kenshi folder>`: the game folder (`data/` inside it), e.g. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- The stem names the files and must be unique across mods (the game finds files by bare name): `<modfolder lower case>_<race>`.
- `male` / `female`: the rig, default male. A female body is fitted to `female_skeleton` and measured against `human_female.mesh` and
  `FemaleMask_Default.png` (female animations and voice in game, the editor's bust sliders); a male one to `male_skeleton`, `human_male.mesh`
  and `MaleMask_Default.png`.
- `slim`: 0 to 1, default 1 ([Slimming](#slimming)); lower it for a race meant to be bulkier than its clothes.
- `bust=female`: raise the bust to the female human's height on a male fit (a single-gender woman such as the Crab Queen); default the rig's
  own human ([Bust](#bust)).
- `neck=<share>`: the neck's length as a share of the fitted one, default 1 (the Crabman uses 0.7).
- Writes `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (only when the model has a normal map) and `<stem>_parts.png`; the log line gives the
  vertex count, scale, bounds and how many vertices fall in hideable parts.
- Keep the `.glb` outside the repo (tens of MB); commit the four outputs. `.gitignore` lets `*/items/races/*.mesh` and `*.dds` through.
- A dependent mod runs it from a KenshiRaces clone with its own output folder (`<Mod>/items/races`) and stem.

## What the model needs

- One skinned mesh whose skin has the Biped joints `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`, both `Clavicle`, `UpperArm`,
  `Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`, and `mixamorig:HeadTop_End` above the head. Extra joints (fingers, tails, helpers)
  follow their nearest Biped ancestor. **Verified** on Leoma, Crevan, Crabman and the Crab Queen (Meshy exports).
- A base colour texture; a metallic-roughness and a normal texture are used when present.
- Any rest pose: T-pose and A-pose both fit (the game's bind pose has the arms down).

## How the fit works

1. Every vertex is skinned into the rest pose the file shows (inverse bind times joint rest transform).
2. Scale: the game's head bone height over the model's head joint above its lowest vertex.
3. Each spine, arm and leg bone is turned so it points from its joint to its child as the game's does, and stretched to the game's length
   (0.6 to 1.6 times).
4. Pelvis, spines, neck and clavicles are fitted as one rigid piece (least squares onto the game's neck, shoulder and hip joints), so the chest
   keeps its shape; per-bone fits shear it where spine and clavicle weights blend. The pelvis and spine joints are left out of that fit: the
   game's pelvis is level with its hips and a Biped export's sits above them, which dropped the chest about a unit.
5. The head lands half way from where the torso carries it to the game's head joint, so its mesh sits below its bone; with `neck` below 1 it
   then moves that share of the way from the carried neck joint. The game's neck rises
   about a unit above its shoulders and these models' barely does: placing the head on the joint stretched their necks to about three times
   their length (**Observed** on the male Leoma, Crabman and the Crab Queen: fitted neck 2.4 units, the models' own 0.8, the game's 1.4).
6. Feet keep upright and are squashed so the soles land on the ground (paws and crab feet are taller than the game's foot).
7. Each vertex is the weighted sum of its joints' fits; its weights go to the game bones its joints follow, the heaviest four kept (then
   [Human weights](#human-weights) replaces those of the torso).

**Verified**: the C# tool's meshes match the original Python prototype's (positions within 1e-5 units, identical bone weights) on Crabman.

## Slimming

The game's clothing is modelled around the rig's human body, so a body bulkier than it shows through. Around each torso and limb bone (pelvis,
spines, clavicles, upper and lower arms and legs) the tool measures the human body's furthest extent per sixth of the bone and per twelfth of a
turn around it; a fitted vertex beyond that extent plus 0.15 units (1.5 cm of cloth) keeps a fifth of its excess, its correction blended by its
bone weights. The correction fades out between 1.5 and 2.5 times the limit (beyond: an appendage such as a claw, left alone), is zero for a
vertex outside the bone's slices (a tail hanging below the pelvis), and is smoothed over the surface (12 rounds of neighbour averaging, uv
seams welded) so neighbouring vertices move alike. **Observed** (male Leoma): 39% of vertices move, by 1.3 cm on average and 9.5 cm at most.

## Skin conform

Slimming only reaches the human's widest extent per slice, so legs, hips and a bulky waist still poked through trousers, skirts, coats and
boots. After slimming, each vertex carried by the pelvis, spines, bust bones, thighs, calves, feet or toes is pulled onto the human body's
skin itself (the nearest point on the triangles round its six nearest human vertices; the human's skinned triangles only): a vertex outside
the skin by more than 0.05 units (5 mm of cloth) moves by its excess, fully up to 1.0 units out and fading to nothing at 1.6 (beyond: a
claw, a tail, left alone), scaled by its weight on those bones, so the head, arms and hands stay. A vertex below the pelvis joint within 0.6
units of the middle line stays: its nearest skin is an inner thigh (**Observed**, male Crevan: pulling there turned the crotch feathers inside
out). Tails and the vertices near them are spared as in slimming. Three passes, each smoothed (three rounds, uv seams welded) and the next
taking up what smoothing undid.

**Observed** (share of a region's vertices over 0.15 units outside the human skin, before → after): Leoma male legs 45 → 13%, feet 30 → 0%;
Leoma female legs 38 → 0%; Crevan male hips 25 → 2%, legs 38 → 3%; Crevan female legs 60 → 15%, hips 64 → 36%; Crab Queen hips 19 → 1%.
What stays outside is tails, the manes and neck plumage, and the claws.

## Bust

Every body's bust (a man's chest) is raised to its rig's human's, or the female human's with `bust=female` (**Observed**: the Leoma and
Crevan women sat theirs 2.0 and 1.75 units lower, the male Leoma 1.0, the Crab Queen 1.5 below the female human's, under the clothing's chest
and the part map's bust). The bust is the middle of the quarter-unit slice, between `Spine1` and the shoulders and 0.3 to 1.8 units beside
the middle line, whose frontmost vertex is furthest forward; its top is the bottom of the first slice above whose front is back halfway to
the collar's. The front of the torso from 0.8 below the bust to its top (at least 0.2 above) rises by the whole difference, so the bust keeps
its shape. Below, the rise falls off by a smoothstep over twice the lift (at least 1.5 units); above, almost linearly up to 0.4 above the
clavicles' joint (at least 1.4 times the lift), so the upper chest is squashed evenly and the bust's top slopes down from the collar. The rise
is scaled by the vertex's torso-bone weight and a smoothstep from the spine's depth (0) to the bust's (1), so the back, arms and legs stay.
**Observed**: a narrow Gaussian folded the chest above a 2-unit lift and stretched the bust into a cone; a smoothstep fade reaching into the
neck (whose vertices ride the head) left a ledge above the bust. Then slimming runs.

## Human weights

The editor's body sliders scale the game's bones (engine docs/animation.md "Body shape sliders"): Chest scales `Spine2`, Stomach and Waist
`Spine1`, Neck `Neck`, the bust sliders the female rig's `L Boob` and `R Boob`. **Observed** (all four Meshy Leoma and Crevan bodies): the
models' weights leave `Spine2` and `Neck` empty, carrying the chest on `Spine1` and the clavicles and the neck on the head, so Stomach and
Waist dragged the chest and Chest did nothing; the Leoma tails are bound to a thigh or a toe. On the surface with uv seams
welded:

1. Tails (before slimming): a vertex carried by a leg bone, behind depth -0.6 and over 0.8 units from every human vertex, goes to `Bip01 Pelvis`; so do its
   neighbours carried by the same bone, behind -0.6 and over 0.3 units from the human (the tail's root). Slimming leaves the tail alone and
   fades out within 1.5 units of it, so the back is not pulled in around the tail's root (**Observed**, male Leoma: a notch above the tail).
2. After the skin conform, each vertex carried by the pelvis, spines, neck, clavicles, upper arms, thighs or the bust bones, or by the head below the game's head
   joint, blends to the human's weights there (the six nearest human torso, neck, head, upper-limb vertices' weights by inverse distance):
   fully within 0.5 units of the human, not at all beyond 1.2. A limb vertex takes only its own limb's weights and the torso's next to it
   (an arm: `Spine1`, `Spine2`, `Neck`; a leg: `Pelvis`, `Spine`), a torso vertex the torso's and the limbs' upper bones', never a forearm's
   or calf's. **Observed** (Crab Queen, arms hanging by her hips): taking the nearest human weights of any bone gave the arms pelvis and
   thigh weights and the hips forearm weights, so walking stretched webs between arms, hips and legs.
3. The changed vertices and two rings around them are smoothed: four rounds, each halfway to the neighbours' mean.

The bones (`L Boob` 28, `R Boob` 29) and the female rig's other handles differ from the male's (`R Clavicle` 23 against 25; the male has
`Bip01 Jaw`), so meshes are always written against their own rig's handles.

## Part map

The race's `part map male`/`female` PNG marks which body part each vertex belongs to; worn items' `hide parts` stop drawing those parts
(engine docs/characters.md "Hidden body parts"). The tool gives each fitted vertex the part of the nearest human body vertex carried by
the same bone, read from the rig's part map at that vertex's uv, and writes it at the fitted vertex's own uv (1024 × 1024,
other texels transparent). Each human part is first grown by 0.8 units (a vertex without a part takes that of the nearest marked vertex
within that distance), since a bulkier body clips over a wider band.

**Observed** (`MaleMask_Default.png`, 256², sampled at `human_male.mesh` uvs as the game samples them): 642 of 15,504 vertices have a part:
red the upper chest and shoulder tops, white bands round the upper arms, green the belly front. `hide parts` in the base data (rebirth.mod):
Drifter's and Padded Leather Jacket 1, Sleeveless Longcoat and Dustcoat 2, Longcoat and Dustcoat 3, Ninja Gi and Rags 4, robes 5, plate
jackets, skin suits and Crab Armour 7; Holy Servant Rags, Monk Pants and hoods 0. Items with 0 hide nothing; slimming covers those.
**Observed** (`FemaleMask_Default.png` at `human_female.mesh` uvs): red the bust (1,102 vertices), white the upper-arm bands (560), green the
belly (250).

## Textures

- `_d`: base colour, alpha = gloss = (255 - roughness) / 2 (roughness 200 without a map); 2048², BC3 with mips.
- `_n`: the game's character layout, X in alpha and Y in green, Y flipped from glTF's +Y up; red and blue 0.
