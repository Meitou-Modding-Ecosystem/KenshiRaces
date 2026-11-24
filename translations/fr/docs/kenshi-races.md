<a id="kenshi-races"></a>
# Kenshi Les races

Trois nouvelles randonnées pour l'éditeur du personnage, chacune avec des sous-randonnées, sur les skeletons humains du jeu (`kenshi.races`, pas de dépendance : chaque record ajouté à la charge, les corps équipés par le [Les races importées](race-import.md). Il a également mis en place un [Bibliothèque Animation](animations.md) 59 clips pour chaque course sur ces écrans. Les modes utilisent les races et les clips, ajoutent des subraces ou ajoutent leurs propres races : [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## Les races

L’éditeur enregistre chaque RACE_GROUP Une course et son RACEs comme des subrases (moteur docs/characters.md « L’éditeur de caractère »

| La course (RACE_GROUP) | Le Soleil (RACE) | Général | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | Leom `10-` | Homme, femme | Leomamale `11-`La femme leoma `17-` |
| Crevan `31-` | L’équipage `18-` | Homme, femme | Crevanmale `19-`La femme crevan `21-` |
| Crabfolk `32-` | Crabman `12-` | Un seul | Crabman `13-` |
| | Crabqueen `14-` | Un seul | Crabqueen `15-` |

Il finit en `KenshiRaces.mod`. Subrace et CHARACTER Les noms sont un mot de base, donc la console `spawn` Je les ai pris sans quotes. Les fichiers de chaque corps sont `KenshiRaces/items/races/kenshiraces_<character>*` (Normales cartes sur Leoma et Crevan). Les corps masculins sont conçus pour `male_skeleton`Les femmes à `female_skeleton`Les corps de crab sont des coups mâles, donc une reine de crab est un seul sexe avec les animations mâles et la voix. Les modèles (Meshy exportations, slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman et crabqueen "... - 3ds max Biped,glb", les autres `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## Les enregistrements

Il a été ajouté dans la phase des données de la partie chargée de l'enregistrement (`RaceRecords`) et ainsi `KenshiRaces.mod` Les bateaux sont vides et CI n'a pas besoin de jeu:

- RACE_GROUPLe nom de la race et ses sous-races RACEsà l’ordre de l’éditeur.
- RACE (subrace) : une copie de Greenlander (`17-gamedata.quack`: statistiques, animations, voix) avec chaque sexe, texture de couleur, carte normale
et une carte partielle (une subrace d'un seul sexe porte son corps pour les deux); `single gender` à moins qu'il n'ait les deux corps ; il n'y ait ni tête ni cheveux (une race qui n'a pas de tête ne tire pas ; le corps a son propre); `morph num` 0; `no shirts` (T-shirts et pantalons peindre l'affichage UV du corps humain); masque corporelle et cartes de force normales nettoyées; `playable` Avec `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTERUne copie de l’écriture (`2205-gamedata.base`Le sexe et le sexe (`female chance` 100 ou 0), aucun kit, argent ou bonus, avec le
Le recrutement.
- Moteur : A CHARACTERde son propre `mesh` Le champ dépasse le corps de sa race.

<a id="character-editor"></a>
## Character éditeur

Les courses sont offertes à la création de personnages comme le jeu lui-même. `KenshiRaces/items/races/kenshiraces_editor.xml` Il maintient uniquement les éclats qui déplacent les os, de sorte qu'ils travaillent sur les corps équipés comme sur un humain (moteur) docs/animation.md, les échelles osseuses : Face détient la taille de la tête, la forme de la tête et les couches (pas de poses de visage: les têtes fixées n'ont pas); Le corps détient les couches du corps humain et le ton de la peau, avec la taille du sein, la hauteur et l'espace pour les femmes, qui déplacent le squelette féminin `L Boob` / `R Boob` Le corps féminin est élevé ([Les races importées](race-import.md#bust)). Les humains sont les humains (`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## Recrutement

DIALOGUE `1-KenshiRaces.mod`Le personnage dit : « Vous pouvez utiliser un autre couple de mains » ; le joueur répond : « Joignez-vous » et l’action 18 ajoute le personnage à l’équipe.

<a id="testing-in-game"></a>
## Test dans le jeu

1. permettent Kenshi Race dans le lanceur (ou `data/mods.cfg`).
2. La console : `spawn leomafemale` (ou n’importe quel CHARACTER dans la table) spawns sur le cursor; parler à recruter, ou F10 débris Recruit.
3. F10 **Kenshi Étiquette : races** Listes de chaînes à chaînes de chaîne à chaîne (les modes et les dépendants).
4. Nouveau jeu: les armes de course de l'éditeur atteignent Leoma, Crevan et Crabfolk.

<a id="clothing"></a>
## Les vêtements

Armour partage le squelette du corps, donc il se déplace avec ces corps, mais il est modélisé autour du corps humain. La partie de la carte cache la peau sous les éléments avec `hide parts` (Pour les coussins, les robes, les chaussures) ; la perte de poids se pousse dans ce qui est plus bulkier qu'un humain ailleurs ([Les races importées](race-import.md#slimming)). Les formes éloignées des humains (les coquilles, les coquilles) poussent encore à travers un armure étroit.
