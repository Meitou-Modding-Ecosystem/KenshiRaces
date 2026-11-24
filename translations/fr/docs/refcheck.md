<a id="engine-references-refcheck"></a>
# Les références de moteur (RefCheck)

Un mod est construit contre un moteur (cette répo `engine` Le jeu se déroule sur tout ce que le joueur a. Lorsqu’une libération change la signature de quelque chose que le mod appelle, la source du mod peut encore compiler, mais la DLL Ce qui a déjà été construit demande le jeu pour l'ancien membre et échoue à l'heure avec `MissingMethodException`.

Ce n’est pas hypothétique. Méditerranée 0.21 donne `Inventory.Add(ItemInstance)` Un autre paramètre (`Add(ItemInstance, bool mainShut = false)`). Une source qui appelle `bag.Add(item)` La version de l'article, mais une DLL construit contre 0.20 ou plus appelle la méthode d'un paramètre, qui 0.21 n'a plus Dans The Wasteland Remembers qu'il a brisé les gardes en jeu, et l'exception a arrêté un système pour le reste de la séance. Aucun test ne l'a pris : les tests sont compilés contre un moteur.

<a id="what-it-checks"></a>
## Ce qu’il vérifie

`tools/RefCheck` Lire un mod intégré DLLLes métadonnées et la liste de chaque membre du moteur qu'il fait référence (methodes, constructeurs, champs sur les types de `Meitou*.dll`Par nom et par paramètre, qu'une version donnée n'a pas. Rien de la mode ou du jeu ne fonctionne.

`tools/refcheck.sh` récupère chaque mod dans le repo (toute carte de niveau supérieur avec `meitou.json` et `<Folder>/<Folder>.csproj`), télécharge un win-x64 zip de la sortie une fois (ne reste que `Meitou*.dll` et `0Harmony.dll`et vérifie chaque assemblée le mod `meitou.json` Les listes.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Une ligne par édition et par assemblée :

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

Pour une pause :

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## Dans le ci

Le `Engine references` Travail de course `tools/refcheck.sh` contre la dernière libération sur chaque PR et pression, et `Release readiness` Attendre pour ça. Un bateau qui a DLL Un moteur ancien doit également être DLL Pour chaque libération `meitou.json` Il permet.

<a id="when-it-finds-something"></a>
## Quand on trouve quelque chose

Appelez le membre d'une manière qui fonctionne sur chaque édition que vous soutenez : réparez-vous contre le moteur si vous débarrassez, ou trouvez-vous dans les paramètres que vous ne passez pas avec leurs défauts. Le désert se souvient » `InventoryCompat` Ce dernier pour `Inventory.Add`.

Les appels de réflexion dans le moteur ne sont pas vérifiés : ils échouent doucement où vous les écrivez.
