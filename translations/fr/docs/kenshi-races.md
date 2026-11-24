<a id="kenshi-races"></a>
# Kenshi Races

Trois nouvelles races pour l'éditeur de personnages, chacune avec des sous-races, sur les squelettes humains du jeu (`kenshi.races`, aucune dépendance) : chaque enregistrement ajouté au chargement, les corps ajustés par le [race import](race-import.md). Il contient également une bibliothèque d'animations](animations.md) de 59 clips pour chaque race sur ces squelettes. Les mods utilisent les courses et les clips, ajoutent des sous-races ou ajoutent leurs propres courses : [dedependent-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## Races jouables

L’éditeur présente chaque groupe RACE_GROUP comme une race et ses enregistrements RACE comme des sous-races.

| Race | Sous-race | Corps | Personnages |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | masculin, féminin | leomamale `11-`, leomafemale `17-` |
| Crevan `31-` | crevan `18-` | masculin, féminin | crevanmale `19-`, crevanfemale `21-` |
| Crabfolk `32-` | crabman `12-` | unique | crabman `13-` |
|  | crabqueen `14-` | unique | crabqueen `15-` |

Les identifiants se terminent par `KenshiRaces.mod`. Les noms Subrace et CHARACTER sont constitués d'un seul mot en minuscule, donc le `spawn` de la console les prend sans guillemets. Les fichiers de chaque corps sont `KenshiRaces/items/races/kenshiraces_<character>*` (cartes normales sur Leoma et Crevan). Les corps masculins sont adaptés au `male_skeleton`, les corps féminins au `female_skeleton` ; les corps des crabes sont des coupes masculines, donc une reine du crabe est unique avec les animations et la voix masculines. Modèles sources (exports Meshy, slim 1) : leomamale "Leonid Bare - 3ds Max Biped.glb", crabman et crabqueen "... - 3ds Max Biped.glb", les autres `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## Enregistrements

Ajouté dans la phase de données à partir des propres enregistrements du jeu chargé (`RaceRecords`), donc `KenshiRaces.mod` est livré vide et CI n'a pas besoin de jeu :

- RACE_GROUP : le nom de la race et les COURSES de ses sous-races, dans l'ordre de l'éditeur.
- RACE (sous-race) : une copie de Greenlander (`17-gamedata.quack` : statistiques, animations, voix) avec le maillage de chaque genre, la texture de couleur, la carte normale et la carte partielle (une sous-race monogenre porte son seul corps pour les deux) ; `single gender` à moins qu'il n'ait les deux corps ; pas de tête ni de cheveux (une race sans tête n'en dessine aucune ; le corps a les siens) ; `morph num` 0 ; `no shirts` (les chemises et les pantalons peignent la disposition UV du corps humain) ; le masque corporel et les cartes normales de force effacées ; `playable` avec `editor limits` [ `kenshiraces_editor.xml` ](#character-editor).
- CHARACTER : une copie de Drifter (`2205-gamedata.base`) de la sous-race et du sexe (`female chance` 100 ou 0), sans kit, ni argent ni prime, avec le discours de la recrue.
- Moteur : le champ `mesh` propre à un CHARACTER remplace le maillage du corps de sa course.

<a id="character-editor"></a>
## Éditeur de personnages

Les courses sont proposées à la création de personnages comme celles du jeu. `KenshiRaces/items/races/kenshiraces_editor.xml` conserve uniquement les curseurs qui déplacent les os, ils fonctionnent donc sur les corps ajustés comme sur un humain (moteur docs/animation.md, les échelles osseuses) : Le visage contient les curseurs de taille de la tête, de forme de la tête et du cou (pas de pose du visage : les têtes ajustées n'en ont pas) ; Le corps contient les curseurs du corps humain et le teint, avec la taille, la hauteur et l'espacement des seins pour les femmes, qui déplacent les `L Boob` / `R Boob` du squelette féminin sur lesquels les corps féminins sont pondérés ([importation de course](race-import.md#bust)). Les gammes sont celles humaines (`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## Recrutement

DIALOGUE  `1-KenshiRaces.mod` : le personnage dit "On dirait que vous pourriez utiliser une autre paire de mains.", le joueur répond "Rejoignez-nous". et l'action 18 (rejoindre l'équipe) ajoute le personnage à l'équipe du joueur.

<a id="testing-in-game"></a>
## Recrutement et essai en jeu

1. Activez Kenshi Races dans le lanceur ou dans `data/mods.cfg`.
2. Dans la console, saisissez `spawn leomafemale` pour faire apparaître le personnage sous le curseur. Vous pouvez utiliser tout identifiant de personnage du tableau.
3. Parlez au personnage et acceptez sa proposition de rejoindre l’escouade. La commande de débogage Recruit du menu F10 fonctionne aussi.
4. Dans le menu F10, **Kenshi Races: races**affiche les races adaptées chargées par ce mod et par les mods dépendants.

<a id="clothing"></a>
## Vêtements

Les armures suivent le même squelette, mais leur géométrie correspond à un corps humain. Les manteaux, peignoirs et vestes qui utilisent `hide parts` masquent la peau couverte. Les carapaces et les pinces peuvent dépasser d’une armure ajustée. La réduction du volume du corps rapproche les parties les plus larges des proportions humaines. ([race-import.md](race-import.md#slimming)).

[English](../../../docs/kenshi-races.md) · [Kenshi Races](../README.md)
