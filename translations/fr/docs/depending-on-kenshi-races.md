<a id="depending-on-kenshi-races"></a>
# Utiliser le framework dans un autre mod et ajouter des sous-races

Comment un mod de code utilise les races et les animations de Kenshi Races, leur ajoute des sous-races ou ajoute ses propres races (`kenshi.races`). Règles du chargeur : moteur [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Une compétence d'agent suit les mêmes étapes : [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Référencez-le au moment de la construction, ne l'expédiez jamais

Dans un dépôt créé à partir de [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate), ajoutez le zip de version de Kenshi Races à `meitou.lock.json` ; la build la télécharge, la vérifie et la référence, et Play la déploie. Chaque valeur provient des notes de version (`kenshi-races-v<version>`) :

```json
{
  "id": "kenshi.races",
  "repo": "Meitou-Modding-Ecosystem/KenshiRaces",
  "tag": "kenshi-races-v0.4.0",
  "asset": "KenshiRaces-0.4.0.zip",
  "sha256": "<Zip SHA-256 from the notes>",
  "folder": "KenshiRaces",
  "engine": "<Submodule engine commit from the notes>"
}
```

- Votre broche `engine/` se déplace vers cette validation de moteur (GutenStructure [framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Ne répertoriez jamais `KenshiRaces.dll` dans votre propre `assemblies` ; le `kenshi.races` chargé sert les types.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Déclarez-le dans `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.23.0",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Avant la version 1.0, une version mineure peut casser le API, limitez donc la plage à la version mineure suivante.
- Kenshi Races doit se trouver au-dessus de votre mod dans `data/mods.cfg` (l'Arrangement du lanceur le fait), de sorte que ses enregistrements existent lorsque votre phase de données s'exécute.

<a id="3-use-its-races"></a>
## 3. Utilisez ses courses

Reportez-vous aux identifiants dans `Races` à partir de vos enregistrements, par ex. dans votre phase de données :

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- COURSES (sous-races) : `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- PERSONNAGES (sans kit, recrutables) : `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`, `CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (les courses de l'éditeur) : `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` est le « Rejoignez-nous ». parler.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) et `Races.Groups` les décrivent. Tableau : [kenshi-races.md "Courses"](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Ajouter une sous-race ou une race

1. Installez chaque corps avec le [race import](race-import.md) à partir d'un clone de KenshiRaces, dans le `items/races` de votre mod avec votre propre tige (`mymod_<character>` ; `female` comme rig pour un corps féminin) et validez les fichiers. Ajoutez `!/<Mod>/items/races/*.mesh` et `!/<Mod>/items/races/*.dds` à `.gitignore`.
2. Décrivez-le et ajoutez ses enregistrements dans la phase de données de votre mod (l'entrée implémente `IModDataPhase`) :

```csharp
static readonly Subrace GhostCrab = new("ghostcrab", "30-MyMod.mod",
    Male: new RaceBody("MyMod", "mymod_ghostcrab", NormalMap: true), Female: null,
    Characters: [new RaceCharacter("ghostcrab", "31-MyMod.mod", Female: false)]);

public void EditData(ModDataContext c)
{
    RaceRecords.Add(c, GhostCrab);                                       // RACE + CHARACTERs, the recruit talk by default
    RaceRecords.JoinGroup(c, Races.CrabfolkGroup, GhostCrab.RaceId);    // the editor lists it under Crabfolk
}
```

- Une sous-race avec un corps `Male` et un corps `Female` reçoit la flèche de genre de l'éditeur ; avec un, il est unisexe et chaque personnage le porte.
- `RaceRecords.Add(c, subrace, talk)` : `talk` est un identifiant DIALOGUE pour les personnages, `null` pour aucun.
- Une course à vous : `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` ajoute le RACE_GROUP et toutes les sous-races.
- Les sous-races sont `playable` avec les limites de l'éditeur Kenshi Races (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` remplissez les enregistrements que vous ajoutez vous-même (`c.Add`) lorsque vous souhaitez modifier d'autres champs par la suite.
- Les identifiants sont les vôtres : enregistrements de votre propre nom de fichier `.mod`.

<a id="6-play-its-animations"></a>
## 6. Jouez ses animations

Chaque clip de [animations.md](animations.md) est lié à chaque race des squelettes humains du jeu (une sous-race ajoutée avec `RaceRecords` également), donc un mod ne le joue que. Un mouvement de combat de mod est uniquement une animation, demandé chaque fois que le jeu ne joue rien de propre (engine docs/modding.md, "Combat move") :

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` est à vous : une réponse bon marché, thread-safe et en lecture seule provenant de votre propre état, la progression du clip de 0 à 1 (secondes / `clip.Length`).
- Les clips jouent sur place et sont tournés vers l'avant ; un clip plus court que votre mouvement conserve sa dernière image à moins que vous n'enveloppiez la progression.
- Les animations n'ont besoin que de la dépendance (sections 1 et 2), aucun enregistrement de votre part.

<a id="5-check-it"></a>
## 5. Vérifiez-le

`dotnet build`, vos tests, puis dans le jeu F10 **Kenshi Races : courses**liste votre race avec son maillage, et `spawn <name>` la fait apparaître. Nommez les sous-races et les CARACTÈRES avec un mot minuscule, par ex. `ghostcrab`, donc la console les prend sans guillemets ; l'éditeur d'un nouveau jeu répertorie votre sous-race.

[English](../../../docs/depending-on-kenshi-races.md) · [Kenshi Races](../README.md)
