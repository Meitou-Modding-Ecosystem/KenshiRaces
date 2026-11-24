<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Importation de course: mettre un corps sur le squelette du jeu

`tools/RaceImport` Devenir une rigidité `.glb` Le corps (Meshy, Hi3D: le "3ds Max Biped" rig, joints `Bip001 ...`) dans un corps, le jeu s'animera avec ses propres animations humaines : une messe d'Ogre en cuir liée à `male_skeleton.skeleton` ou `female_skeleton.skeleton`Et ses DDS Les textures et une partie de la carte. Il lit le squelette, le corps humain et une partie du jeu, donc il fonctionne sur une machine avec KenshiJamais dans le CI; sa production est engagée.

<a id="run-it"></a>
## Faire courir

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>`La carte de jeu (`data/` à l’intérieur, par exemple. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- Les stam nomment les fichiers et doivent être uniques à travers les mods (le jeu trouve les fichières par un seul nom) : `<modfolder lower case>_<race>`.
- `male` / `female`Le rig, le maître défaut. Un corps féminin est mis à `female_skeleton` Mesurée contre `human_female.mesh` et
`FemaleMask_Default.png` (animations féminines et la voix dans le jeu, le bust de l'éditeur); un homme à `male_skeleton`, `human_male.mesh` et `MaleMask_Default.png`.
- `slim`0 à 1 (defaut 1 à 1)[La faiblesse](#slimming)• La baisse pour une course qui signifiait être plus bulgare que ses vêtements.
- Écrits `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (Seulement lorsque le modèle a une carte normale) et `<stem>_parts.png`La ligne de log donne la
Le nombre de verts, l'échelle, les limites et le nombre de vertices tombent dans les parties cachées.
- Gardez le `.glb` Au-delà de la répo (tens de MB); faites les quatre sorties. `.gitignore` Laissez `*/items/races/*.mesh` et `*.dds` à travers.
- Un mod dépendant va de la clone KenshiRaces avec sa propre dossier de sortie (`<Mod>/items/races`et le vote.

<a id="what-the-model-needs"></a>
## Ce que le modèle a besoin

- Un masque en cuir dont la peau a les articulations Biped `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`Les deux `Clavicle`, `UpperArm`,
`Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`et `mixamorig:HeadTop_End` Au-dessus de la tête Les articulations supplémentaires (de doigts, des épaules, des aides) suivent leur ancêtre le plus proche. **Vérifié** sur Leoma, CrevanLe Crab et la Reine Crab (Meshy les exportations).
- Une texture de couleur de base; une rigidité métallique et une texture normale sont utilisés lorsqu'ils sont présents.
- T-Pose et A-Pose s'adaptent à la fois (la pose de l'engagement du jeu a les bras en bas).

<a id="how-the-fit-works"></a>
## Comment fonctionne le fit

1. Chaque vertex est éclipsé dans le reste de la pose des fichiers (à l'inverse, les temps de liaison joint repos transformation).
2. Scale : la hauteur de l'os de la tête au-dessus de l'articulation de la tête du modèle au-dessu de son vertex le plus bas.
3. Chaque dos, bras et jambes sont tournées afin qu'ils pointent de leur joint à leur enfant comme le jeu fait, et étendus à la longueur du jeu.
6 à 6 fois.
4. Les pelvis, les épaules, le cou et les clavicles sont équipés comme un seul morceau rigide (le plus petit squares sur le cou, l'épaule et les articulations du jeu), de sorte que le sein
Il maintient sa forme; per-bone s’adapte à l’éclair où l’épine et le clavicle pesent mélangé. Les articulations de la peluche sont laissées hors de cette forme : la peluche du jeu est à l'échelle de ses couches et un siège de l'exportation au-dessus de celles-ci, qui a baissé le sein autour d'une unité.
5. La tête s'étend à moitié de là où le torse la porte à l'articulation de la tête, de sorte que sa mère est assise sous son os. Le cou du jeu s’élève
Il s’agit d’une unité de piètre de piètres au-dessus de ses épaules et de pièter les piètres et de piêtres et de mettre la tête sur l’articulation et d’étendre les couches à environ trois fois leur longueur (**Observé** sur le mâle LeomaCrabman et la reine Crab : 2.4 unités de cou équipées, les modèles possèdent 0.8, le jeu 1.4).
6. Les pieds se maintiennent bien et se déchirent de manière à ce que les soles s'allument sur le sol (les pieds de pauses et de crab sont plus élevés que les pieds du jeu).
7. Chaque vertu est la somme pondérée de ses articulations; ses poids vont à l'os de jeu, ses articules suivent, les quatre plus lourds sont maintenus.

**Vérifié**: les cartes de l’outil C# correspondent au prototype Python original (positions dans 1e-5 unités, poids osseux identiques) sur Crabman.

<a id="slimming"></a>
## La faiblesse

La robe du jeu est modélisée autour du corps humain de la rig, de sorte qu'il est un bulkier du corps qu'il montre. Environs de chaque torso et os limbes (pélvices, épines, clavicles, bras et jambes supérieurs et inférieurs) l'outil mesure la plus longue portée du corps humain par un sixième de l'os et par un douzième d'un tour autour de lui; un vertex fixé au-delà de cette portée plus 0,15 unités (1,5 cm de vêtement) maintient un cinquième de son excès, sa correction mélangée par ses poids osseux. La correction s'élève entre 1,5 et 2,5 fois la limite (à côté: un appendice comme un clavier, seul), c'est nul pour une verticale en dehors des couches (une colonne suspendue sous le pelvis), et est soulagé sur la surface (12 roues de voisin moyen, les clavier verticales) ainsi que les verticales voisines. **Observé** (le petit Leoma• 39% des vertices se déplacent, de 1,3 cm en moyenne et de 9,5 cm au maximum.

<a id="bust"></a>
## Bust

Le corps de la femme est élevé à l'homme de la femme (**Observé**: le Leoma et Crevan Les femmes ont mis leurs 2.0 et 1.75 unités inférieures, sous le sein de la robe et le bust de la partie de la carte). Le bûte est au milieu de l'unité de veine de veine, entre `Spine1` et les épaules et les unités de 0.3 à 1.8 à côté de la ligne moyenne, dont le vertex est le plus petit en avant. Chaque vertex s'élève par la différence de temps son poids torso-bone, des temps exp(-(d / 1.5)2) de sa hauteur de la poitrine, des temps un pas doux de la profondeur de la poussière à la poitrinière (1), de sorte que le dos, les bras et les jambes restent. Puis, le dégustement se fait.

<a id="human-only-bones"></a>
## Les os humains se

Le squelette féminin a `L Boob` et `R Boob` (la taille du sein de l'éditeur, la hauteur et l'échelle d'espace et les déplacer, le moteur docs/animation.mdqui manquent d’un béton. Après la perte de poids, chaque vertex dont le plus lourd est `Spine1` ou `Spine2` Prenez les poids humains du vertex humain le plus proche qui a quelques uns, dans 1 unité, évolués par 1 - distance; ses poids se réduit pour faire de l'espace. **Observé** (`human_female.mesh`Les vertices bust sont portées par les deux os (892 des 994 vertices frontales). Les épaules (`L Boob` 28, `R Boob` Les autres formes de la femelle sont différentes de celles de l'homme (`R Clavicle` 23 contre 25 ; l’homme a `Bip01 Jaw`Ainsi, les méchants sont toujours écrits contre les mains de leur propre rig.

<a id="part-map"></a>
## Une partie de la carte

La course de `part map male`/`female` PNG marques à laquelle le corps fait partie, à laquelle chaque vertex appartient ; les objets portés `hide parts` Arrêtez de dessiner ces pièces (moteur) docs/characters.md « Les parties du corps cachées » L'outil donne à chaque vertex la partie du vertex le plus proche du corps humain porté par le même os, lu de la partie du rig sur l'UV de ce vertex, et écrit à l'UV propre du vertex (1024 × 1024), d'autres textiles transparents. Chaque partie humaine est cultivée par 0,8 unités (un vertex sans partie prend celui du vertex le plus proche dans cette distance), puisque un corps de bulle clique sur une bande plus large.

**Observé** (`MaleMask_Default.png`2562, écrite à `human_male.mesh` UVs comme le jeu les montre : 642 des verts ont une part : rouge le haut du sein et les couches, des bandes blanches autour des bras supérieurs, vert le front du ventre. `hide parts` Dans les données de base (rebirth.mod): Drifter et Padded Leather Jacket 1, Sleeveless Longcoat et Dustcoat 3, Ninja Gi et Rags 4, robes 5, chaussures de peau et Crab Armour 7 ; Saint Servant Rags, Monk Pants et Hoods 0. Les éléments avec 0 ne cachent rien ; la fatigue couvre ceux-ci. **Observé** (`FemaleMask_Default.png` à `human_female.mesh` uvs : rouge le bust (1,102 vertices), blanc les bandes de l'arme supérieure (560), vert le ventre (250).

<a id="textures"></a>
## Les textures

- `_d`: couleur de base, alpha = gloss = (255 - dureté) / 2 ( dureté 200 sans carte); 20482, BC3 avec mips.
- `_n`: le layout des personnages du jeu, X en alpha et Y en vert, Y fléau glTF+ Y en haut ; rouge et bleu 0.
