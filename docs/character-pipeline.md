# Character pipeline: from a `.glb` to a playable race

The order of work that turns a character `.glb` (Meshy, Hi3D, Blender) into a race the game animates, dresses and shapes with its editor
sliders. What each step does inside the tool: [race-import.md](race-import.md). The races made this way: [kenshi-races.md](kenshi-races.md#races).

Folders, as PowerShell variables (set them once; every command below uses them):

```powershell
$Repo      = "C:\Users\<you>\source\repos\KenshiRaces"            # this repo (or a dependent mod's KenshiRaces clone)
$Kenshi    = "C:\Program Files (x86)\Steam\steamapps\common\Kenshi"
$MotionLab = "C:\Users\<you>\source\repos\PZAKenshiMotionLab"     # only for animations (step 8)
$Models    = "C:\Users\<you>\Models"                              # the source .glb files, outside the repo
```

**Work in `$Repo`:**

```powershell
cd $Repo
```

## 1. Get a model the import reads

- Meshy: export a humanoid with the **3ds Max Biped** skeleton template (joints `Bip001 ...`). A Mixamo rig (`mixamorig:Hips`, ...) also
  works. The required joints: [race-import.md "What the model needs"](race-import.md#what-the-model-needs).
- One skinned mesh, one material, a base colour texture. A normal map and a metallic-roughness map are used when present.
- Any rest pose (T-pose, A-pose).
- Inspect it:

  ```powershell
  python tools\model-info.py "$Models\owlkindmale-rigged.glb"
  ```

## 2. A high-poly or unrigged model: bake and transfer weights

Only when the model is far over budget (the beepbot's was 1M vertices, unrigged) and a rigged low-poly of the same model exists (Meshy rigs
the low-poly). In Blender (or Blender as Python, `pip install bpy`):

1. Import both `.glb` files. Bring the low-poly rig into the high-poly's frame (the beepbot's rig: scale 1.711, lift 0.85 m).
2. Decimate the high-poly (Collapse) to about 40,000 triangles.
3. Bake a tangent-space normal map from the original high-poly onto the decimated mesh (selected to active, cage extrusion 0.01). Flatten
   texels whose baked normal has Z below 0.3 (misses on cloth flaps).
4. Data Transfer modifier on the decimated mesh: vertex groups from the rigged low-poly, Nearest Face Interpolated. Apply it, then Limit
   Total 4 on the vertex groups.
5. Parent it to the low-poly's armature and export `.glb` (Apply Modifiers on).

## 3. Choose the import options

| The model | Option |
| --- | --- |
| A woman with a man and woman in the subrace | `female` (fits `female_skeleton`, female animations, bust bones) |
| A single-gender woman (forced onto the male rig) | `male bust=female` |
| Palms face forward or down in the rest pose (twisted hands in game) | `palms` |
| Claws or pincers instead of hands | no `palms` |
| A stretched neck in the first import | `neck=0.7` (lower for shorter) |
| Meant to look bulkier than its clothes | a number from 0 to 1, e.g. `0.5` (slimming strength, default 1) |
| Should wear the human head, its hair and face sliders | `headless`; `ears` keeps the model's ears; `nohair` drops the model's hair |
| Should take the editor's Skin Tone slider | `skintone` (with the human head) |
| Wing membranes under the arms | `wings` |
| Separate ears, horns or folded wings | `attach=<part>.glb` with a spec in `tools/RaceImport/parts/` |
| A mechanical body (open ribs, springs, joints) | `rigid` |

## 4. Run the import

```powershell
dotnet run --project tools\RaceImport -c Release -- "$Models\owlkindmale-rigged.glb" $Kenshi KenshiRaces\items\races kenshiraces_owlkindmale male palms
```

- A dependent mod: its own output folder (`<Mod>\items\races`) and stem (`<modfolder lower case>_<character>`).
- Writes `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (with a normal map), `<stem>_parts.png`, and `<stem>_mask.dds` with `skintone`.
- The log line gives the vertex count, scale, bounds, part-map vertices, hand turns and thumb joints (`palms`), membrane vertices (`wings`)
  and face poses (`headless`). A bad model exits non-zero.

## 5. What the import fixes without an option

| Problem seen in game | Fix in the import | Details |
| --- | --- | --- |
| Misshapen chest: sheared where spine and clavicle weights blend | Pelvis, spines, neck and clavicles fitted as one rigid piece | [How the fit works](race-import.md#how-the-fit-works), step 4 |
| Bust or chest sits below the clothing's chest | Bust raised to the rig's human's (or the female human's with `bust=female`), its shape kept | [Bust](race-import.md#bust) |
| Folded chest above the bust, cone-shaped bust, ledge under the collar | Smoothstep fade below, even squash up to 0.4 above the clavicles | [Bust](race-import.md#bust) |
| Twisted hands (palms forward or down) | `palms`: each hand turned about its forearm, palm to thigh; the forearm takes half the turn | [Hands](race-import.md#hands) |
| Thumbs left behind when the hand turns or moves | `palms`: a helper joint in the forearm's last quarter (Meshy's thumb) follows the hand | [Hands](race-import.md#hands) |
| Talons, claws and fingers bend apart from the hand | Joints without a game bone follow their nearest Biped ancestor (the hand: one rigid shape) | [What the model needs](race-import.md#what-the-model-needs) |
| Body shows through coats, sleeves, trousers, boots | Slimming to the human's extent, then skin conform onto the human's skin | [Slimming](race-import.md#slimming), [Skin conform](race-import.md#skin-conform) |
| Skin drawn through coats and jackets | Part map from the human's, by nearest human vertex on the same bone | [Part map](race-import.md#part-map) |
| Holes at the elbows under a jacket | Part map grows only within a bone, so hanging forearms don't take the hip band | [Part map](race-import.md#part-map) |
| Chest and Neck sliders do nothing, Stomach and Waist drag the chest | Human weights copied onto the torso, neck and upper limbs (Meshy leaves `Spine2` and `Neck` empty) | [Human weights](race-import.md#human-weights) |
| Webs between arms, hips and legs when walking | A limb vertex takes only its own limb's weights and the torso's next to it | [Human weights](race-import.md#human-weights) |
| Tail swings with a leg | A tail bound to a leg moves to `Bip01 Pelvis`; slimming spares it | [Human weights](race-import.md#human-weights) |
| Paws or crab feet below the ground | Feet kept upright and squashed onto the ground | [How the fit works](race-import.md#how-the-fit-works), step 6 |

## 6. Check the poses

Check each body before committing:

- In the idle: palms to the thighs, thumbs on the hands, no fold above the bust, no notch above a tail.
- In a swing that crosses the chest (`kr_charged_slash`) and in the walk: no web between arms and hips, the chest keeps its shape.
- Without the game window (the cloud): skin `<stem>.mesh` with clips from `KenshiRaces\items\animations\kenshiraces_anims_<gender>.skeleton`
  using the engine's `OgreMeshReader` and `OgreSkeletonReader`, write OBJ and render it (Blender).
- In game ([kenshi-races.md "Testing in game"](kenshi-races.md#testing-in-game)): F10 **Kenshi Races: spawn race**, then dress the character
  in a Drifter's Jacket (`hide parts` 1), a Longcoat (3) and Monk Pants (0), and move the editor's sliders.

## 7. Make it a race

1. Add the subrace in `KenshiRaces\Races.cs` (a dependent mod: its own data phase,
   [depending-on-kenshi-races.md "Add a subrace or a race"](depending-on-kenshi-races.md#4-add-a-subrace-or-a-race)):
   `RaceBody(..., NormalMap: true)` with a `_n.dds`, `HumanHead: true` for `headless`, `SkinTone: true` for `skintone`; traits and
   `RaceModel.Skeleton` for a robot.
2. Editor sliders, by how the body was imported ([kenshi-races.md "Character editor"](kenshi-races.md#character-editor)):

   | Body | Editor limits | Sliders |
   | --- | --- | --- |
   | Own head | `kenshiraces_editor.xml` | Bone sliders only; Chest, Stomach, Waist, Hips, Legs and bust sliders left out |
   | `headless` | `kenshiraces_editor_faces.xml` | Those plus the human face sliders, Chest and the bust sliders |
   | `headless skintone` | `kenshiraces_editor_tinted.xml` | Those plus Skin Tone |

3. Add the body to the tables in [kenshi-races.md](kenshi-races.md#races) with its source model and options.
4. Commit the import's files with the code, and bump the version in `KenshiRaces\meitou.json`.

## 8. Its animations

Clips in the source `.glb` don't come through the import: the body plays the game's human animations. To add them to the shared library
(`kr_<clip>`, playable by every human-skeleton race), follow [animations.md "Making them"](animations.md#making-them) with `$MotionLab`.

## 9. Export the fitted body (optional)

To use a fitted body outside the game as `.glb` or `.fbx`: load `<stem>.mesh` with the game's skeleton through the Motion Lab
(`motionlab.ogre.load.load_mesh`, then `motionlab.gltf.write_glb`), convert the `.dds` textures to PNG, and in Blender add a material with
them packed and export GLB and FBX (Path Mode Copy, embedded textures, no leaf bones).

## The Motion Lab and the race import

| | Race import (`tools/RaceImport`) | [PZA's Kenshi Motion Lab](https://github.com/Meitou-Modding-Ecosystem/PZAKenshiMotionLab) |
| --- | --- | --- |
| Skeleton | The game's `male_skeleton` / `female_skeleton`; no `.skeleton` written | The model's own (a `Bip001` rig kept, others auto-rigged from the shape); writes a new `.skeleton` |
| Animations | The game's human animations, combat moves and the `kr_` library | Its own baked clips (idle, walk, run, knockdowns, rolls, jumps, `ref_` retargets); no combat moves |
| Clothing | Slimming, skin conform, part map | None |
| Editor sliders | Human weights on the torso | None |
| Hands, chest, neck | `palms`, bust lift, rigid torso fit, `neck` | Arms lowered from a T-pose for its clips |
| Weights | 4 per vertex | 3 per vertex |
| Output | `.mesh`, `_d`/`_n` `.dds`, part map; records from code | `.mesh`, `.skeleton`, `_d`/`_n`/`_m` `.dds`, `.asset.json`; no records |
| Runs on | .NET 11, the game install | Python and numpy, or the Blender add-on; no game install needed for its own rigs |

The race import uses the Motion Lab for the animation library (step 8) and the body export (step 9).
