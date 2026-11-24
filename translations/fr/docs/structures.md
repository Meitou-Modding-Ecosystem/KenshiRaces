<a id="structures-from-3d-models"></a>
# Structures à partir de modèles 3D

Mettez un modèle `.glb` ( Meshy , Hi3D , un export Blender ) dans le dossier `Structures` de votre mod et la construction de jeu le transforme en un bâtiment que vous pouvez placer depuis le menu de construction : maillage Ogre, textures compressées, collision et les enregistrements dans `<Mod>.mod` . Pas de FCS , pas de Blender , aucune installation de jeu nécessaire pour l'importation elle-même.

<a id="quick-start"></a>
## Démarrage rapide

1. Mettez le modèle dans `<Mod>/Structures/` , par ex. `MyMod/Structures/watch-tower.glb` .
2. Facultatif : à côté, `watch-tower.json` avec n'importe quel [paramètres](#settings) , par ex.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Créez et déployez comme d'habitude ([play.md](play.md) ) : Visual Studio F5, VS Code F5 ou `dotnet run --project Play` . Le journal de construction montre une ligne `Structure ...` par modèle.
4. Dans le jeu : menu construction, catégorie `EXTERIOR` (ou celle que vous avez définie). Avec le `"research": "start"` par défaut, il est connu dès le début d'un jeu, ainsi que dans les sauvegardes existantes ( Meitou calcule les déverrouillages à partir de la recherche terminée, qui inclut toujours `_Default Start` ).
5. Validez le modèle, ses `.json` et `Structures/ids.json` (écrits lors de la première importation). Les fichiers générés sont une sortie de build et ne sont pas validés.

Sans jeu : `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>` .

<a id="settings"></a>
## Paramètres

Tout est facultatif ( `<model>.json` , commentaires autorisés ; un nom inconnu est une erreur donc les fautes de frappe ne passent pas inaperçues).

| Paramètre | Défaut | Signification |
| --- | --- | --- |
| `name` | nom de fichier, `-` / `_` sous forme d'espaces | nom dans le menu de construction |
| `description` | `""` | construire le texte du menu |
| `height` | propre taille du modèle | hauteur finie en mètres ; le modèle est mis à l'échelle uniformément.  Les modèles AI sont disponibles dans des tailles arbitraires, alors définissez ceci |
| `sink` | `0` | mètres enfoncés dans le sol (l'empêche de flotter sur les pentes) |
| `yaw` | `0` | degrés tournés autour de l'axe vertical |
| `category` | `EXTERIOR` | catégorie du menu de construction ( `building category` ) : celle du jeu ( `BUILDINGS` , `CAMPING` , `FARMING` , ...) ou un nouveau nom |
| `group` | `""` | `building group` : les bâtiments qui en partagent une sont des variantes d'une même entrée |
| `materials` | `5` | Les matériaux de construction, ça coûte |
| `buildSpeed` | `2` | `build speed mult` (les tentes du jeu 5, les maisons 1 à 2) |
| `research` | `start` | `start` : connu depuis le début ; `none` : rien ne le déverrouille (vos propres enregistrements le font) ; ou un identifiant de chaîne RESEARCH , par ex. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor` , `indoor` ou `both` |
| `createsTown` | `true` | le placer en dehors d'une ville démarre un avant-poste |
| `collision` | `mesh` | `mesh` (une copie simplifiée du modèle), `box` (son cadre de délimitation) ou `none` (le parcourir) |
| `collisionTriangles` | `2000` | la plupart des triangles conservés par la collision `mesh` (12 à 50 000) |
| `textureSize` | `2048` | plus grand côté de texture en pixels (une puissance de deux, au maximum 8192) |

<a id="item-models"></a>
## Modèles d'articles

Mettez plutôt le modèle d'un objet dans `<Mod>/ItemModels/` ( `Structures` en ferait un bâtiment). L'importation écrit son maillage et ses textures à côté des structures et d'un enregistrement MATERIAL_SPECS_CLOTHING ; l'enregistrement ITEM de votre mod les pointe alors. Le journal de construction affiche une ligne `Item ...` par modèle avec les noms à utiliser.

1. `MyMod/ItemModels/lantern.glb` , en option `lantern.json` : `{ "size": 0.3 }` .
2. Dans votre fiche article (générateur de données ou phase de données), définissez :

| Champ | Valeur |
   | --- | --- |
| `mesh` et `ground mesh` (noms de fichiers) | `<mod>_<model>.mesh` , par ex. `mymod_lantern.mesh` | | `material` (référence) | `<950000 + 100 × block>-<Mod>.mod` , bloc de `ItemModels/ids.json` , par ex. `950000-MyMod.mod` |

Recherchez d'abord le matériel ( `GameDatabase.Find` ) et conservez l'apparence du remplaçant lorsqu'il manque : un zip publié et des tests qui lisent le jeu sans le mod déployé ne l'ont pas.
3. Validez le modèle, ses `.json` et `ItemModels/ids.json` .

| Paramètre | Défaut | Signification |
| --- | --- | --- |
| `name` | nom de fichier, `-` / `_` sous forme d'espaces | le nom de l'enregistrement matériel |
| `size` | propre taille du modèle | côté le plus long en mètres ; le modèle est mis à l'échelle uniformément |
| `yaw` | `0` | degrés tournés autour de l'axe vertical |
| `textureSize` | `1024` | plus grand côté de texture en pixels (une puissance de deux, au maximum 8192) |

- Un matériau par modèle d'article (le ITEM en nomme un `material` ); le modèle se tient sur son point le plus bas, centré, comme le font les maillages d'objets du jeu.
- Le jeu dessine un objet au sol, dans une main et dans une icône auto avec son `mesh` ( `ground mesh` premier au sol) et le MATERIAL_SPECS_CLOTHING ses noms `material` (moteur `docs/formats/runtime-materials.md` "Callers", `WorldObjectLayout.ItemMesh` ).
- L'icône d'inventaire est tirée du modèle, sauf si l'enregistrement définit `auto icon image` false et son propre `icon` .

<a id="getting-a-good-model"></a>
## Obtenir un bon modèle

- Demandez à Meshy ou Hi3D un modèle texturé PBR et téléchargez-le sous le nom GLB . Quelques dizaines de milliers de triangles suffisent ; le journal signale un modèle supérieur à 150 000. Couper le polycount, choisir la taille et réduire les textures : [model-prep.md](model-prep.md) .
- Le recto du modèle fait face +Z dans le fichier (convention de glTF) ; utilisez `yaw` pour le faire tourner.
- Chaque matériau devient sa propre pièce. Les générateurs en donnent généralement un ; au plus 49.
- Le modèle se tient sur son point le plus bas, centré.

<a id="what-it-writes"></a>
## Ce qu'il écrit

Dans le dossier du mod déployé ( `<Kenshi>/mods/<Mod>/` ) :

- `structures/<mod>_<model>.mesh` (un par matériau, `_1` , `_2` , ... quand plusieurs) : Ogre `[MeshSerializer_v1.8]` , position, normal, UV et tangente avec main en w, la disposition des maillages de construction du jeu.
- `structures/<mod>_<model>_d.dds` , `_n.dds` , `_m.dds` : diffus (BC3), normal (BC1, BC3 à la découpe), métallité (BC1), avec mips.
- `structures/<mod>_<model>_col.xml` : la collision.
- Pour un modèle d'article : `structures/<mod>_<model>.mesh` et ses textures, pas de collision.
- Les enregistrements ont été fusionnés dans `<Mod>.mod` à côté de tout ce qu'il contient (les enregistrements d'un générateur de données sont conservés ; ceux d'une importation antérieure sont remplacés) : un BUILDING , un BUILDING_PART et un MATERIAL_SPEC par matériau, et le déverrouillage (le `enable buildings` de la recherche gagne le bâtiment) ; un MATERIAL_SPECS_CLOTHING par modèle d'article.

Les noms de fichiers commencent par le nom du mod car le jeu recherche les ressources par nom de fichier brut dans chaque mod.

Les identifiants d'enregistrement sont `<n>-<Mod>.mod` avec n à partir de 900 000, un bloc de 100 par modèle (bâtiment +0, pièces +1, matériaux +50) ; les modèles d'articles comptent à partir de 950 000 (matériau +0) dans `ItemModels/ids.json` . `Structures/ids.json` conserve le bloc de chaque modèle afin que les identifiants ne changent jamais : une sauvegarde nomme un bâtiment placé par l'identifiant de son enregistrement. Ne modifiez ou ne supprimez jamais un bloc ; supprimer une figurine supprime son bâtiment des jeux qui l'ont placé.

<a id="facts-the-import-relies-on"></a>
## Faits sur lesquels repose l’importation

Vérifié avec les fichiers du jeu (données Kenshi 1.0.68) et avec les lecteurs du moteur :

- **Unités** : l'unité mondiale est 0,1 m, Y vers le haut ; glTF mesure des mètres, Y vers le haut, donc l'échelle est de 10 (les fichiers de collision du jeu utilisent la gravité 98,1).
- **Matériaux** : l'apparence d'une pièce de bâtiment est son MATERIAL_SPEC ( `texture map` , `normal map` , `metalness map` , `material type` ), et non le nom du matériau du maillage (moteur `docs/formats/runtime-materials.md` ). Le shader de construction du jeu ( `data/materials/deferred/objects.hlsl` ) lit RGB diffus comme couleur et A comme brillant (fois `specular mult` ), RGB normal comme normale de l'espace tangent et A comme masque de découpe ( `material type` 1, `alpha threshold` ), métallisation de R. L'importation écrit gloss = 1 - rugosité et métallicité glTF à partir du canal bleu de glTF ; glTF MASK et BLEND deviennent une découpe.
- **Tangentes** : la binormale du shader est croisée (tangente, normale) × w, et sur les maillages du jeu, elle pointe le long de V croissant (**Vérifié** : `prisoner_pole.mesh` 86 sur 86 triangles, `Camp_tent1.mesh` 1858 sur 1865). Les cartes normales glTF pointent en vert vers le haut de l'image (diminuant V), de sorte que l'importation fait tourner le canal vert et définit le chemin du jeu.
- **Collision** : le `xml collision` d'une pièce est un fichier PhysX NxuStream2 ; un fichier minimal écrit à la main se charge (le `Camp_tent1_COL.xml` du jeu est une boîte). PhysX est Z en haut et le jeu mappe (x, y, z) sur (x, z, -y), donc l'importation écrit les points de la pièce sous la forme (x, -z, y). Le maillage triangulaire se présente sous forme de points et de triangles simples ; le jeu le cuisine au chargement, Meitou le lit tel quel (moteur `docs/formats/collision.md` ).
- **Champs** : BUILDING et BUILDING_PART contiennent l'ensemble complet de terrains de la tente de camping 1 du jeu ( `18026-small_changes_otto.mod` ), donc rien ne tombe par défaut ; pas de `functionality` , c'est donc un bâtiment simple.
- **Déverrouillage** : un bâtiment est dans le menu de construction une fois qu'une recherche terminée le répertorie dans `enable buildings` ; chaque jeu commence avec `_Default Start` ( `5359-gamedata.base` ) terminé. **Vérifié** avec les moteurs `GameDatabase` , `ResearchGate` et `BuildingBuildData` sur le jeu de base plus un mod importé : les structures sont déverrouillées et coûtent les matériaux de construction comme défini.

<a id="not-covered-yet"></a>
## Pas encore couvert

Intérieurs, portes, fonctions (rangement, lits, artisanat) et pièces tournantes.
