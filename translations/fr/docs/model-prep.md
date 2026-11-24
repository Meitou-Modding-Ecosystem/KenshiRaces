<a id="preparing-a-model"></a>
# Préparer un modèle

Comment obtenir un générateur `.glb` (Meshy, Hi3D, Blender) à un polycount, la taille de la texture et la taille du monde réel le jeu, avant qu'il entre dans `<Mod>/Structures/` Les bâtiments) ou `<Mod>/ItemModels/` et les sujets. L’importation elle-même : [structures.md](structures.md).

Les commandes fonctionnent à partir de la racine repo dans PowerShell. Il faut Python 3 `prepare-model.py` Il faut aussi `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. Inspecteur

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Il imprimera la taille en mètres (X, Y up, Z), le côté le plus long, les matériaux, et la taille du pixel de chaque texture et MB. Une note sur l'échelle de nœuds ou la rotation `prepare-model.py` ne peut pas le reproduire ; il est exporté à nouveau de Blender avec des transformations appliquées (Object > Apply > All Transforms).

<a id="2-cut-the-polycount"></a>
## 2. Couper le polyconte

| Bonne | Les triangles | Les textures |
| --- | --- | --- |
| Construction ou décoration | 5 000 à 20 000 | 2048 |
| L'article (détenu, tombé, dans un magasin) | En dessous de 5 000 | 1024 |

Les générateurs donnent souvent 100 000 ou plus ; l'importation alerte plus de 150 000. Pour le couper :

- **Meshy:** Remplacer le modèle avec un polycount cible dans le budget ci-dessus (quad ou triangle fonctionnent tous les deux), garder les textures, et
Téléchargez comme GLB encore une fois.
- **Le Blender :** Fichiers > Importation > glTFSélectionnez la barre, ajoutez un modifier décimé (Collapse) et la Ratio inférieure jusqu'à ce que le nombre de visages soit compté dans le statut.
la barre est dans le budget, appliquez-la, puis File > Export > glTF Le Bénin (`.glb`• Appliquer des modifications.

La réduction de l’effet est séparée (`collisionTriangles`, par défaut 2000), donc la coupe de la messe visible est sur la vitesse du cadre et la taille du fichier.

<a id="3-choose-the-size"></a>
## 3. Choisissez la taille

L’importation traite le modèle comme des mètres ; la taille d’un générateur est arbitraire. Placez la taille réelle et placez-la dans le fichier de paramètres du modèle ([M. M. « Les paramètres »](structures.md#settings)Pas dans le modèle :

- Le bâtiment : `"height"` Dans les mètres `<model>.json`. KenshiLes gens sont environ 1,8 m de hauteur et lisent petit à côté des tailles du monde réel, donc prenez le
la taille que vous décrivez et ajoutez 20% (un post de 2,2 m devient `2.64`).
- L’article : `"size"`Son plus long côté en mètres, en `<model>.json` A côté de tout `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. Récupérer les textures

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Ceci redécouvre le fichier avec chaque texture à la fois (2048 pour les bâtiments, 1024 pour les articles), généralement coupé de dizaines de livres. Un troisième argument réserve également le modèle à ce côté en mètres, pour quand un fichier de paramètres n'est pas souhaité. Les produits sont également plus élevés (`textureSize`Mais en commettant le fichier réduit, le repo est petit.

Courir `model-info.py` Encore une fois pour vérifier le résultat.

<a id="5-import-and-check"></a>
## 5. Importer et vérifier

Construisez et déployez avec le jeu (ou effectuez l'importation seule, [M. M. « Démarrer rapidement »](structures.md#quick-start)). Le log a une ligne par modèle :

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Vérifiez la hauteur ou la longueur et le nombre de triangles, puis regardez dans le jeu: placez le bâtiment, ou descendez et récupérez l'élément.

<a id="6-commit"></a>
## 6. Commissariat

Commettre à `.glb`Et ses `.json` et le dossier de `ids.json` Ensemble ! `ids.json` Il maintient les identifiants enregistreux de chaque modèle fixes, et sauvegarde les bâtiments nommés par ces identifiants.
