<a id="depending-on-kenshi-races"></a>
# En fonction de Kenshi Les races

Comment un code mod utilise Kenshi Les races et animations de races, ajoutent des subraces à eux ou ajoutent leurs propres races (`kenshi.races`). Règles de chargement : moteur [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Un agent va par les mêmes étapes : [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Rappelez-vous le temps de construction, ne jamais le déposer

Dans une récolte faite de [Mémorandum](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)et ajouter Kenshi Résultats pour Zip `meitou.lock.json`; le bâtiment télécharge, vérifie et fait référence, et le jeu le déploie. Chaque valeur est dérivée des notes de l'édition (`kenshi-races-v<version>`):

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

- Votre `engine/` La pin se déplace vers ce moteur (GutenStructure)
[Le framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Jamais la liste `KenshiRaces.dll` Dans votre propre `assemblies`Le chargé `kenshi.races` Ils servent les types.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Déclarer dans `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Avant 1.0, une version plus petite peut rompre APIPour cela, il est possible de couper la gamme à la prochaine petite.
- Kenshi Les races doivent être au-dessus de votre mode `data/mods.cfg` (l'arrange du lanceur le fait), donc ses enregistrements existent lorsque votre phase de données se déroule.

<a id="3-use-its-races"></a>
## 3. Utilisez vos races

Il s’agit des ids en `Races` de vos enregistrements, par exemple dans la phase de vos données :

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs ( les sous-traits sont : `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (Pour le recrutement et la récupération) `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (Les cours de l’éditeur) `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` C’est le mot « Join us » qui parle.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) et `Races.Groups` Décrivez les La table :
[Kenshi-races.md « Races »](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Ajoutez une subrace ou une race

1. Chaque corps est adapté à [Les races importées](race-import.md) d'un KenshiRaces clone, dans votre mode `items/races` Avec votre propre voix
(`mymod_<character>`; `female` comme le rig pour un corps féminin) et communiquer les fichiers. Ajouter `!/<Mod>/items/races/*.mesh` et `!/<Mod>/items/races/*.dds` à `.gitignore`.
2. Décrivez-le et ajoutez ses enregistrements dans la phase de données de votre mode (les implémentations d'entrée `IModDataPhase`):

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

- Une subrasion avec un `Male` et a `Female` Le corps reçoit la pile de sexe de l'éditeur ; avec un, il est de sexe unique et chaque personnage la porte.
- `RaceRecords.Add(c, subrace, talk)`: `talk` est un DIALOGUE pour les personnages, `null` Pour personne.
- Une course de votre propre : `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` Ajoutez le RACE_GROUP et chaque subrasion.
- Les sujets sont `playable` Avec Kenshi Les limites d’éditeur (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` Remplir les dossiers que vous ajoutez
Pour vous (`c.Add`Lorsque vous voulez changer de champs après.
- Les ids sont les vôtres : les records de vos propres `.mod` Nom du fichier.

<a id="6-play-its-animations"></a>
## 6. Jouer ses animations

Chaque clip de [animations.md](animations.md) est lié à chaque course sur les skeletons humains du jeu (une subrace ajoutée avec `RaceRecords` Mais aussi, un mod ne joue qu’à ça. Un mouvement de combat de mode n'est qu'animation, demandée à chaque fois que le jeu ne joue rien de lui-même (moteur) docs/modding.md« Le combat se déplace » :

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` est le vôtre: une réponse bon marché, sécurisée par le fil, la réponse de votre propre état, le progrès du clip 0 à 1 (seconds / `clip.Length`).
- Les clips jouent en place et en face; un clip plus court que votre mouvement maintient son dernier cadre, à moins que vous puissiez faire progresser.
- Les animations n’ont besoin que de la dépendance (sections 1 et 2), pas d’enregistrement de votre propre.

<a id="5-check-it"></a>
## 5. Vérifier ça

`dotnet build`Les tests, puis en jeu F10 **Kenshi Étiquette : races** Vous avez la course avec sa messe, et `spawn <name>` Il le dégage. Nom de la subrasion et CHARACTERs Un mot plus bas, par exemple. `ghostcrab`Ainsi, la console les prend sans cotes ; l'éditeur d'un nouveau jeu enregistre votre sous-race.
