# Race import: fitting a body onto the game's skeleton

`tools/RaceImport` turns a rigged `.glb` body (Meshy, Hi3D: the "3ds Max Biped" rig, joints `Bip001 ...`) into a body the game animates with
its own human animations: a skinned Ogre mesh linked to `male_skeleton.skeleton`, its DDS textures and a part map. It reads the skeleton, the
human body and the human part map from the game install, so it runs on a machine with Kenshi, never in CI; its output is committed.

## Run it

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<race> [slim]
```

- `<Kenshi folder>`: the game folder (`data/` inside it), e.g. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- The stem names the files and must be unique across mods (the game finds files by bare name): `<modfolder lower case>_<race>`.
- `slim`: 0 to 1, default 1 ([Slimming](#slimming)); lower it for a race meant to be bulkier than its clothes.
- Writes `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (only when the model has a normal map) and `<stem>_parts.png`; the log line gives the
  vertex count, scale, bounds and how many vertices fall in hideable parts.
- Keep the `.glb` outside the repo (tens of MB); commit the four outputs. `.gitignore` lets `*/items/races/*.mesh` and `*.dds` through.
- A dependent mod runs it from a KenshiRaces clone with its own output folder (`<Mod>/items/races`) and stem.

## What the model needs

- One skinned mesh whose skin has the Biped joints `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`, both `Clavicle`, `UpperArm`,
  `Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`, and `mixamorig:HeadTop_End` above the head. Extra joints (fingers, tails, helpers)
  follow their nearest Biped ancestor. **Verified** on Leonid, Crabman and the Crab Queen (Meshy exports).
- A base colour texture; a metallic-roughness and a normal texture are used when present.
- Any rest pose: T-pose and A-pose both fit (the game's bind pose has the arms down).

## How the fit works

1. Every vertex is skinned into the rest pose the file shows (inverse bind times joint rest transform).
2. Scale: the game's head bone height over the model's head joint above its lowest vertex.
3. Each spine, arm and leg bone is turned so it points from its joint to its child as the game's does, and stretched to the game's length
   (0.6 to 1.6 times).
4. Pelvis, spines, neck and clavicles are fitted as one rigid piece (least squares onto the game's torso, shoulder and hip joints), so the chest
   keeps its shape; per-bone fits shear it where spine and clavicle weights blend.
5. Feet keep upright and are squashed so the soles land on the ground (paws and crab feet are taller than the game's foot).
6. Each vertex is the weighted sum of its joints' fits; its weights go to the game bones its joints follow, the heaviest four kept.

**Verified**: the C# tool's meshes match the original Python prototype's (positions within 1e-5 units, identical bone weights) on Crabman.

## Slimming

The game's clothing is modelled around `human_male.mesh`, so a body bulkier than it shows through. Around each torso and limb bone (pelvis,
spines, clavicles, upper and lower arms and legs) the tool measures the human body's furthest extent per sixth of the bone and per twelfth of a
turn around it; a fitted vertex beyond that extent plus 0.15 units (1.5 cm of cloth) keeps a fifth of its excess, its correction blended by its
bone weights. The correction fades out between 1.5 and 2.5 times the limit (beyond: an appendage such as a claw, left alone), is zero for a
vertex outside the bone's slices (a tail hanging below the pelvis), and is smoothed over the surface (12 rounds of neighbour averaging, uv
seams welded) so neighbouring vertices move alike. **Observed** (Leonid): 39% of vertices move, by 1.3 cm on average and 9.5 cm at most.

## Part map

The race's `part map male`/`female` PNG marks which body part each vertex belongs to; worn items' `hide parts` stop drawing those parts
(engine docs/characters.md "Hidden body parts"). The tool gives each fitted vertex the part of the nearest `human_male.mesh` vertex carried by
the same bone, read from the game's `MaleMask_Default.png` at that vertex's uv, and writes it at the fitted vertex's own uv (1024 × 1024,
other texels transparent). Each human part is first grown by 0.8 units (a vertex without a part takes that of the nearest marked vertex
within that distance), since a bulkier body clips over a wider band.

**Observed** (`MaleMask_Default.png`, 256², sampled at `human_male.mesh` uvs as the game samples them): 642 of 15,504 vertices have a part:
red the upper chest and shoulder tops, white bands round the upper arms, green the belly front. `hide parts` in the base data (rebirth.mod):
Drifter's and Padded Leather Jacket 1, Sleeveless Longcoat and Dustcoat 2, Longcoat and Dustcoat 3, Ninja Gi and Rags 4, robes 5, plate
jackets, skin suits and Crab Armour 7; Holy Servant Rags, Monk Pants and hoods 0. Items with 0 hide nothing; slimming covers those.

## Textures

- `_d`: base colour, alpha = gloss = (255 - roughness) / 2 (roughness 200 without a map); 2048², BC3 with mips.
- `_n`: the game's character layout, X in alpha and Y in green, Y flipped from glTF's +Y up; red and blue 0.
