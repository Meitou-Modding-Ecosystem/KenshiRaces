<a id="structures-from-3d-models"></a>
# Structures de modèles 3D

Mettez à `.glb` Le modèle (Meshy, Hi3D, un Blender exporté) dans votre mode `Structures` Le dossier et le jeu le transformeront en un bâtiment que vous pouvez placer du menu : Mise à jour, textures comprimées, collision et enregistrements `<Mod>.mod`. Non pas FCSAucun Blender, aucune installation nécessaire pour l'importation elle-même.

<a id="quick-start"></a>
## Début rapide

1. Mettez le modèle dans `<Mod>/Structures/`et g. `MyMod/Structures/watch-tower.glb`.
2. Optionnel : en plus de cela, `watch-tower.json` Avec n'importe quel [Settings](#settings)et g.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Il s’agit de la construction et de la mise en œuvre comme d’habitude ([play.md](play.md)• Visual Studio F5, VS Code F5, ou `dotnet run --project Play`. Le logon montre un
`Structure ...` Lignes par modèle.
4. Dans le jeu: construire le menu, catégorie `EXTERIOR` (ou celui que vous avez mis en place) Avec le défaut `"research": "start"` Il est connu depuis le début d’une
jeu, et dans les sauvegardes existantes aussi (Meitou travaille sur les clés de la recherche terminée, qui comprend toujours `_Default Start`).
5. Il s’agit du modèle, son `.json` et `Structures/ids.json` (décrit sur la première mise en œuvre). Les fichiers générés sont des outils et ne sont
engagée .

Sans jouer : `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## Les paramètres

Tout le monde (`<model>.json`, les commentaires sont autorisés ; un nom inconnu est une erreur, de sorte que les types ne sont pas ignorés).

| Settings | Défaut | Signification |
| --- | --- | --- |
| `name` | Le nom du fichier, `-`/`_` comme espaces | Nom dans le menu de construction |
| `description` | `""` | Créer un menu texte |
| `height` | La taille du modèle | hauteur terminée en mètres; le modèle est équivalent à elle. Les modèles d'IA sont à des tailles arbitraires, donc définissez ce |
| `sink` | `0` | des mètres tombent dans le sol (le garde de flotter sur les pistes) |
| `yaw` | `0` | Les degrés tournent autour de l'axe verticale |
| `category` | `EXTERIOR` | La catégorie de menu (`building category`Le jeu est (`BUILDINGS`, `CAMPING`, `FARMING`... ou un nouveau nom |
| `group` | `""` | `building group`: les bâtiments partage un sont des variantes d'une entrée |
| `materials` | `5` | Le matériel qui coûte |
| `buildSpeed` | `2` | `build speed mult` (les tentes du jeu 5, les maisons 1 à 2) |
| `research` | `start` | `start`Connu depuis le début ; `none`Il n’y a rien qui le déclenche (votre propre enregistrement fait); ou un RESEARCH C’est un code d’identité, etc. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` ou `both` |
| `createsTown` | `true` | L'emplacement à l'extérieur d'une ville commence par un outpost |
| `collision` | `mesh` | `mesh` (une copie simplifiée du modèle) `box` (c’est une boîte de bord) ou `none` (Voyez à travers ça) |
| `collisionTriangles` | `2000` | La plupart des triangles `mesh` La collision se poursuit (12 à 50 000) |
| `textureSize` | `2048` | plus grand côté de texture en pixels (une puissance de deux, au maximum 8192) |

<a id="item-models"></a>
## Modèles d'objets

Placez le modèle d'un objet `<Mod>/ItemModels/` Au lieu de cela (`Structures` Il en ferait un bâtiment). L'importation écrit sa messe et ses textures à côté des structures et une MATERIAL_SPECS_CLOTHING La rédaction : votre mod ITEM Les notes les montrent ensuite. Le logon montre un `Item ...` Le modèle avec les noms à utiliser.

1. `MyMod/ItemModels/lantern.glb`Optionnellement `lantern.json`: `{ "size": 0.3 }`.
2. Dans votre fichier d'objets (Générateur de données ou phase de données), définissez :

La valeur du champ
   | --- | --- |
| `mesh` et `ground mesh` (Précédent) Les `<mod>_<model>.mesh`et g. `mymod_lantern.mesh` | | `material` (Référence) Laïa `<950000 + 100 × block>-<Mod>.mod`Le bloc de `ItemModels/ids.json`et g. `950000-MyMod.mod` |

Regardez le matériau d’abord (`GameDatabase.Find`Et gardez le regard de l'instant quand il manque: un zip libéré, et les tests qui lisent le jeu sans le mode déployé, ne l'ont pas.
3. Il s’agit du modèle, son `.json` et `ItemModels/ids.json`.

| Settings | Défaut | Signification |
| --- | --- | --- |
| `name` | Le nom du fichier, `-`/`_` comme espaces | Le nom de l’enregistrement |
| `size` | La taille du modèle | le côté le plus long en mètres; le modèle est équivalent à celui-ci |
| `yaw` | `0` | Les degrés tournent autour de l'axe verticale |
| `textureSize` | `1024` | plus grand côté de texture en pixels (une puissance de deux, au maximum 8192) |

- Un matériau par modèle (le ITEM Nom 1 `material`Le modèle se trouve sur son point le plus bas, centré, comme le jeu des pièces.
- Le jeu attire un élément sur le sol, dans une main et dans une icône automatique avec son `mesh` (`ground mesh` d’abord sur le sol et le
MATERIAL_SPECS_CLOTHING Leur `material` Le nom du moteur `docs/formats/runtime-materials.md` Les « appels », `WorldObjectLayout.ItemMesh`).
- L'icône d'inventaire est extraite du modèle, sauf si les enregistrements `auto icon image` Fausse et sa propre `icon`.

<a id="getting-a-good-model"></a>
## Obtenir un bon modèle

- Demandez Meshy ou Hi3D Pour A PBR- Modèle textile et téléchargez-le comme GLB. Quelques dizaines de milliers de triangles sont riches; le log flamme un modèle
Plus de 150 000. Coupez le polyconte, choisissez la taille et raccourcez les textures : [model-prep.md](model-prep.md).
- Le tableau de l'écran est enregistré sur le tableau + Z (glTFConvention ; utilisation et `yaw` Pour le tourner.
- Chaque matériau devient sa propre partie. Les générateurs d'électricité donnent généralement un, au maximum 49.
- Le modèle est sur son point le plus bas, le centre.

<a id="what-it-writes"></a>
## Ce qu’il écrit

Dans le modèle de mise en œuvre (`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` (une pour un matériau, `_1`, `_2`... quand il y a plusieurs : Ogre `[MeshSerializer_v1.8]`position, normale, UV et
tangent avec l'habileté en w, la disposition des mesquilles de construction du jeu.
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: diffuse (BC3), normale (BC1, BC3 lors de la coupe), métallesse (BC1), avec des mips.
- `structures/<mod>_<model>_col.xml`La collision .
- Pour un modèle d’objet : `structures/<mod>_<model>.mesh` et ses textures, pas de collision.
- Les enregistrements sont intégrés `<Mod>.mod` en plus de tout ce qui y sera placé (les enregistrements d'un générateur de données sont conservés; les importations antérieures sont remplacées):
BUILDING, un BUILDING_PART et MATERIAL_SPEC de l’environnement et de l’écart (la recherche de `enable buildings` Le bâtiment ; un MATERIAL_SPECS_CLOTHING par modèle.

Les noms de fichiers commencent par le nom du mod parce que le jeu apparaît par le nom de fichier sur chaque mod.

Les IDs sont `<n>-<Mod>.mod` avec n de 900 000 à 10 000, un bloc de 10 000 par modèle (construction +0, pièces +1, matériaux +50); les modèles d'objets comptent de 9 500 000 (materiaux +0) en `ItemModels/ids.json`. `Structures/ids.json` Il maintient le bloc de chaque modèle de sorte que les identifiants ne changent jamais : un sauvetage nomme un bâtiment placé par l'identifiant de son enregistrement. Ne jamais modifier ou supprimer un bloc ; en supprimant un modèle, il est retiré de ses jeux qui l’ont placé.

<a id="facts-the-import-relies-on"></a>
## Les importations dépendent de

Les fichiers du jeu sont contrôlés (Kenshi 1.0.68 données) et avec les lecteurs du moteur:

- **Unités**: l'unité mondiale est de 0,1 m, Y en haut; glTF est de mètres, Y en haut, donc l'échelle est 10 (les fichiers de collision du jeu utilisent la gravité 98.1).
- **Matériaux**La vue d’un bâtiment est la MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`Non, pas la messe
Nom du matériel (moteur) `docs/formats/runtime-materials.md`). Le jeu d'écriture de l'écran (`data/materials/deferred/objects.hlsl`Lire diffuse RGB comme couleur et A comme gloss (times `specular mult`• Normalement RGB Le cas échéant, c’est le cas de la masque (`material type` 1, `alpha threshold`Le métal de R. L'importation écrit gloss = 1 - glTF La rougeur et le métal de glTFle canal bleu ; glTF MASK et BLEND Devenir un cut-out.
- **Les Tangent**: la forme de l'étoile binaire est croisée(tangent, normale) × w, et sur les mesures du jeu il marque le long de l'augmentation de V (**Vérifié**:
`prisoner_pole.mesh` 86 sur 86 triangles `Camp_tent1.mesh` 1858 et 1865. glTF Les cartes normales mettent l'image en vert (dégradant V), donc l'importation tourne le canal vert et met en place la voie du jeu.
- **Collision**: une partie de `xml collision` c'est un fichier PhysX NxuStream2 ; un minimum de chargement de fichier écrit à la main (le jeu `Camp_tent1_COL.xml` Il est
une boîte) PhysX est Z up et les cartes de jeu (x, y, z) à (x, z, -y), donc l'importation écrit les points de la partie comme (x, -z, y). Le triangle entre comme des points et des triangles ; le jeu le fait, Meitou le lit comme il est. `docs/formats/collision.md`).
- **Les champs**: BUILDING et BUILDING_PART Le jeu est enregistré dans le cadre de l'équipe de camping (`18026-small_changes_otto.mod`Et donc rien
Il s’agit d’un défaut ; non `functionality`Il s’agit donc d’un bâtiment plat.
- **Déblocage**: un bâtiment est dans le menu de construction une fois qu'une recherche terminée le liste dans `enable buildings`Chaque jeu commence par
`_Default Start` (`5359-gamedata.base`et terminé. **Vérifié** Avec le moteur `GameDatabase`, `ResearchGate` et `BuildingBuildData` sur le jeu de base et un mode importé : les structures sont déverrouillées et coûte les matériaux de construction comme indiqué.

<a id="not-covered-yet"></a>
## Pas encore couvert

Interiors, portes, fonctions (entrepôt, lits, dessin) et pièces tournées.
