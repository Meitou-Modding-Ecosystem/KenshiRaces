<a id="preparing-a-model"></a>
# Préparer la géométrie, les dimensions et les textures

Comment obtenir un `.glb` généré (Meshy, Hi3D, Blender) à un polycount, une taille de texture et une taille réelle gérées par le jeu, avant qu'il n'entre dans `<Mod>/Structures/` (bâtiments) ou `<Mod>/ItemModels/` (objets). L'importation elle-même : [structures.md](structures.md).

Les commandes sont exécutées à partir de la racine du dépôt dans PowerShell. Nécessite Python 3 ; `prepare-model.py` a également besoin de `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. Inspecter

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Il imprime la taille en mètres (X, Y vers le haut, Z), le côté le plus long, les triangles, les matériaux, ainsi que la taille des pixels de chaque texture et MB. Une note sur l'échelle ou la rotation du nœud signifie que `prepare-model.py` ne peut pas le redimensionner : exportez-le à nouveau depuis Blender avec les transformations appliquées (Objet > Appliquer > Toutes les transformations).

<a id="2-cut-the-polycount"></a>
## 2. Coupez le polycompte

| Gentil | Triangles | Textures |
| --- | --- | --- |
| Construction ou décoration | 5 000 à 20 000 | 2048 |
| Objet (détenu, déposé, dans un magasin) | moins de 5 000 | 1024 |

Les générateurs en donnent souvent 100 000 ou plus ; l'importation prévient au-dessus de 150 000. Pour le couper :

- **Meshy :**Remaillez le modèle avec un polycompte cible dans le budget ci-dessus (la topologie quad ou triangle fonctionne toutes les deux), en conservant les textures, et téléchargez-le à nouveau en tant que GLB.
- **Blender :**Fichier > Importer > glTF, sélectionnez le maillage, ajoutez un modificateur Décimer (Réduire) et réduisez le rapport jusqu'à ce que le nombre de faces dans la barre d'état soit dans le budget, appliquez-le, puis Fichier > Exporter > glTF Binaire (`.glb`) avec Appliquer les modificateurs activé.

La collision est simplifiée séparément (`collisionTriangles`, par défaut 2000), donc la découpe du maillage visible concerne la fréquence d'images et la taille du fichier.

<a id="3-choose-the-size"></a>
## 3. Choisissez la taille

L'importation traite le modèle comme des mètres ; la taille d'un générateur est arbitraire. Choisissez la taille réelle et définissez-la dans le fichier de paramètres du modèle ([structures.md "Settings"](structures.md#settings)), pas dans le modèle :

- Bâtiment : `"height"` en mètres en `<model>.json`.  Les personnes de Kenshi mesurent environ 1,8 m et lisent petit par rapport aux tailles réelles, alors prenez la taille que vous décrivez et ajoutez 20 % (un poteau de 2,2 m devient `2.64`).
- Objet : `"size"`, son côté le plus long en mètres, dans `<model>.json` à côté dans `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. Réduisez les textures

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Cela réécrit le fichier en place avec chaque texture au maximum autant de pixels par côté (2048 pour les bâtiments, 1024 pour les objets), le réduisant généralement de dizaines de MB à quelques-uns. Un troisième argument facultatif redimensionne également le modèle sur le côté le plus long en mètres, lorsqu'un fichier de paramètres n'est pas souhaité. L'importation réduit également de moitié les textures plus grandes (`textureSize`), mais la validation du fichier réduit maintient le dépôt petit.

Exécutez à nouveau `model-info.py` pour vérifier le résultat.

<a id="5-import-and-check"></a>
## 5. Importer et vérifier

Créez et déployez avec Play (ou exécutez l'importation seule, [structures.md "Quick start"](structures.md#quick-start)). Le journal comporte une ligne par modèle :

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Vérifiez la hauteur ou la longueur et le nombre de triangles, puis regardez-les dans le jeu : placez le bâtiment, ou déposez et ramassez l'objet.

<a id="6-commit"></a>
## 6. S'engager

Validez le `.glb`, son `.json` et le `ids.json` du dossier ensemble. `ids.json` conserve les identifiants d'enregistrement de chaque modèle fixes et enregistre les bâtiments nommés par ces identifiants.

[English](../../../docs/model-prep.md) · [Kenshi Races](../README.md)
