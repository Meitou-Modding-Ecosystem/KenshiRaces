<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Import de course : adapter un corps sur le squelette du jeu

`tools/RaceImport` transforme un corps `.glb` truqué ( Meshy , Hi3D : le rig "3ds Max Biped", articulations `Bip001 ...` ) en un corps que le jeu anime avec ses propres animations humaines : un maillage Ogre skinné lié à `male_skeleton.skeleton` ou `female_skeleton.skeleton` , ses textures DDS et une carte de pièces. Il lit le squelette de la plate-forme, le corps humain et la carte des pièces à partir de l'installation du jeu, il fonctionne donc sur une machine avec Kenshi , jamais avec CI ; sa sortie est engagée.

<a id="run-it"></a>
## Exécutez-le

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>` : le dossier du jeu ( `data/` à l'intérieur), par ex. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"` .
- La racine nomme les fichiers et doit être unique d'un mod à l'autre (le jeu trouve les fichiers par leur nom simple) : `<modfolder lower case>_<race>` .
- `male` / `female` : le rig, mâle par défaut. Un corps féminin est ajusté sur `female_skeleton` et mesuré par rapport à `human_female.mesh` et `FemaleMask_Default.png` (animations féminines et voix dans le jeu, curseurs de buste de l'éditeur) ; un mâle à `male_skeleton` , `human_male.mesh` et `MaleMask_Default.png` .
- `slim` : 0 à 1, par défaut 1 ([Minceur](#slimming) ); baissez-le pour une course censée être plus volumineuse que ses vêtements.
- Écrit `<stem>.mesh` , `<stem>_d.dds` , `<stem>_n.dds` (uniquement lorsque le modèle a une carte normale) et `<stem>_parts.png` ; la ligne de journal donne le nombre de sommets, l'échelle, les limites et le nombre de sommets tombant dans les parties masquables.
- Gardez le `.glb` en dehors du dépôt (des dizaines de MB ); valider les quatre sorties. `.gitignore` laisse passer `*/items/races/*.mesh` et `*.dds` .
- Un mod dépendant l'exécute à partir d'un clone de KenshiRaces avec son propre dossier de sortie ( `<Mod>/items/races` ) et sa propre tige.

<a id="what-the-model-needs"></a>
## Ce dont le modèle a besoin

- Un maillage à peau dont la peau présente les articulations du bipède `Bip001 Pelvis` , `Spine` , `Spine1` , `Spine2` , `Neck` , `Head` , `Clavicle` , `UpperArm` , `Forearm` , `Hand` , `Thigh` , `Calf` , `Foot` , `Toe0` et `mixamorig:HeadTop_End` au-dessus de la tête. Des articulations supplémentaires (doigts, queues, aides) suivent leur ancêtre bipède le plus proche. **Vérifié** sur Leoma , Crevan , Crabman and the Crab Queen (exportations Meshy ).
- Une texture de couleur de base ; une rugosité métallique et une texture normale sont utilisées lorsqu'elles sont présentes.
- Toute pose de repos : la pose en T et la pose en A conviennent toutes deux (la pose de liaison du jeu a les bras baissés).

<a id="how-the-fit-works"></a>
## Comment fonctionne l'ajustement

1. Chaque sommet est intégré à la pose de repos présentée dans le fichier (transformation du repos articulaire par temps de liaison inverse).
2. Échelle : la hauteur de l'os de la tête du jeu par rapport à l'articulation de la tête du modèle, au-dessus de son sommet le plus bas.
3. Chaque os de la colonne vertébrale, du bras et de la jambe est tourné de manière à pointer de son articulation vers son enfant comme le fait le jeu, et étiré sur la longueur du jeu (0,6 à 1,6 fois).
4. Le bassin, la colonne vertébrale, le cou et les clavicules sont assemblés en une seule pièce rigide (les moindres carrés sur les articulations du cou, des épaules et des hanches du jeu), de sorte que la poitrine garde sa forme ; Les ajustements par os le cisaillent là où les poids de la colonne vertébrale et de la clavicule se mélangent. Les articulations du bassin et de la colonne vertébrale ne sont pas concernées par cet ajustement : le bassin du jeu est au niveau des hanches et celui d'un bipède se trouve au-dessus d'eux, ce qui fait baisser la poitrine d'environ une unité.
5. La tête atterrit à mi-chemin de l'endroit où le torse la porte jusqu'à l'articulation de la tête du jeu, de sorte que son maillage se trouve sous son os. Le cou du gibier s'élève d'environ une unité au-dessus de ses épaules et ces modèles le font à peine : placer la tête sur l'articulation a étiré leur cou jusqu'à environ trois fois sa longueur (**Observé** sur le mâle Leoma , Crabman and the Crab Queen : cou ajusté 2,4 unités, celui des modèles 0,8, celui du jeu 1,4).
6. Les pieds restent droits et sont écrasés pour que les semelles atterrissent sur le sol (les pattes et les pattes de crabe sont plus hautes que le pied du gibier).
7. Chaque sommet est la somme pondérée des ajustements de ses articulations ; ses poids vont aux os du gibier que ses articulations suivent, les quatre plus lourds étant conservés.

**Vérifié** : les maillages de l'outil C# correspondent à ceux du prototype Python original (positions dans les unités 1e-5, poids des os identiques) sur Crabman.

<a id="slimming"></a>
## Minceur

Les vêtements du jeu sont modelés autour du corps humain de la plate-forme, donc un corps plus volumineux qu'il ne le laisse paraître. Autour de chaque os du torse et des membres (bassin, colonnes vertébrales, clavicules, bras et jambes supérieurs et inférieurs), l'outil mesure l'étendue la plus éloignée du corps humain par sixième de l'os et par douzième de tour autour de lui ; un sommet ajusté au-delà de cette mesure plus 0,15 unités (1,5 cm de tissu) conserve un cinquième de son excédent, sa correction étant mélangée à ses poids osseux. La correction s'estompe entre 1,5 et 2,5 fois la limite (au-delà : un appendice tel qu'une griffe, laissé seul), est nulle pour un sommet en dehors des tranches de l'os (une queue pendant sous le bassin), et est lissée sur la surface (12 tours de moyenne voisine, coutures uv soudées) afin que les sommets voisins se déplacent de la même manière. **Observé** (mâle Leoma ) : 39% des sommets bougent, de 1,3 cm en moyenne et de 9,5 cm au maximum.

<a id="bust"></a>
## Bousiller

Le buste d'un corps féminin est élevé par rapport à celui de la femme humaine (**Observé** : les femmes Leoma et Crevan ont assis le leur 2,0 et 1,75 unités plus bas, sous la poitrine des vêtements et le buste de la carte partielle). Le buste est le milieu de la tranche d'un quart d'unité, entre `Spine1` et les épaules et 0,3 à 1,8 unités à côté de la ligne médiane, dont le sommet le plus en avant est le plus en avant. Chaque sommet s'élève de la différence multipliée par le poids de son torse, multiplié par exp(-(dy / 1,5)²) de sa hauteur à partir du buste, multiplié par un pas en douceur depuis la profondeur de la colonne vertébrale (0) jusqu'à celle du buste (1), de sorte que le dos, les bras et les jambes restent. Puis des courses minceur.

<a id="human-only-bones"></a>
## Des os uniquement humains

Le squelette féminin a `L Boob` et `R Boob` (l'échelle de taille, de hauteur et d'espacement des seins de l'éditeur et les déplacer, moteur docs/animation.md), qui manquent à une plate-forme Biped. Après l'amincissement, chaque sommet dont l'os le plus lourd est `Spine1` ou `Spine2` prend les poids réservés aux humains du sommet humain le plus proche qui en a, à 1 unité près, mis à l'échelle de 1 - distance ; ses propres poids rétrécissent pour faire de la place. **Observé** ( `human_female.mesh` ) : les sommets du buste sont portés par les deux os (892 des 994 sommets avant-poitrine). Les os ( `L Boob` 28, `R Boob` 29) et les autres poignées du gréement femelle diffèrent de ceux du mâle ( `R Clavicle` 23 contre 25 ; le mâle a `Bip01 Jaw` ), donc les maillages sont toujours écrits contre les poignées de leur propre gréement.

<a id="part-map"></a>
## Carte des pièces

Les `part map male` / `female`  PNG de la course indiquent à quelle partie du corps appartient chaque sommet ; Le `hide parts` des objets usés arrête de dessiner ces pièces (moteur docs/characters.md "Parties du corps cachées"). L'outil donne à chaque sommet ajusté la partie du sommet du corps humain le plus proche porté par le même os, lue sur la carte des pièces de l'appareil à l'UV de ce sommet, et l'écrit à l'UV du sommet ajusté (1024 × 1024, autres texels transparents). Chaque partie humaine est d'abord agrandie de 0,8 unité (un sommet sans partie prend celui du sommet marqué le plus proche à cette distance), puisqu'un corps plus volumineux se clipse sur une bande plus large.

**Observé** ( `MaleMask_Default.png` , 256², échantillonné aux uv `human_male.mesh` au fur et à mesure que le jeu les échantillonne) : 642 des 15 504 sommets ont une partie : rouge sur le haut de la poitrine et le haut des épaules, bandes blanches autour du haut des bras, vert sur le devant du ventre. `hide parts` dans les données de base (rebirth.mod) : Drifter's et veste en cuir rembourrée 1, Longcoat sans manches et Dustcoat 2, Longcoat et Dustcoat 3, Ninja Gi et Rags 4, robes 5, vestes en plaques, combinaisons en peau et Crab Armor 7 ; Chiffons de saint serviteur, pantalons de moine et capuches 0. Les objets avec 0 ne cachent rien ; l'amincissement les couvre. **Observé** ( `FemaleMask_Default.png` à `human_female.mesh` uvs) : rouge le buste (1 102 sommets), blanc les bandes des bras (560), vert le ventre (250).

<a id="textures"></a>
## Textures

- `_d` : couleur de base, alpha = brillant = (255 - rugosité) / 2 (rugosité 200 sans carte) ; 2048², BC3 avec mips.
- `_n` : la disposition des personnages du jeu, X en alpha et Y en vert, Y inversé à partir du +Y de glTF ; rouge et bleu 0.
