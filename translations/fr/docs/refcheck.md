<a id="engine-references-refcheck"></a>
# Vérifier la compatibilité avec une nouvelle version de Meitou

Un mod est construit sur un moteur (le sous-module `engine` de ce dépôt) et joué sur n'importe quelle version Meitou du joueur. Lorsqu'une version modifie la signature de quelque chose que le mod appelle, la source du mod peut toujours être compilée, mais le DLL déjà construit demande au jeu l'ancien membre et échoue au moment de l'exécution avec `MissingMethodException`.

Ce n’est pas hypothétique. Meitou 0.21 a donné à `Inventory.Add(ItemInstance)` un deuxième paramètre facultatif (`Add(ItemInstance, bool mainShut = false)`). La source qui appelle `bag.Add(item)` se compile avec l'une ou l'autre version, mais un DLL construit avec la version 0.20 ou une version antérieure appelle la méthode à un paramètre, que la version 0.21 n'a plus. Dans The Wasteland Remembers, cela a brisé des souvenirs en jeu et l'exception a arrêté tout un système pour le reste de la session. Aucun test ne l'a détecté : les tests sont compilés sur un seul moteur.

<a id="what-it-checks"></a>
## Ce qu'il vérifie

`tools/RefCheck` lit les métadonnées d'un mod DLL construit et répertorie tous les membres du moteur auxquels il fait référence (méthodes, constructeurs, champs sur les types dans `Meitou*.dll`), par nom et types de paramètres, qu'une version donnée n'a pas. Rien du mod ou du jeu ne fonctionne.

`tools/refcheck.sh` construit chaque mod du référentiel (chaque dossier de niveau supérieur avec `meitou.json` et `<Folder>/<Folder>.csproj`), télécharge une fois le zip win-x64 d'une version (en ne conservant que `Meitou*.dll` et `0Harmony.dll`) et vérifie chaque assemblage dans les listes `meitou.json` du mod.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Sortie, une ligne par version et assemblage :

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

ou, pour une pause :

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## Dans CI

Le travail `Engine references` exécute `tools/refcheck.sh` sur la version la plus récente sur chaque PR et chaque push, et `Release readiness` l'attend. Un mod qui expédie un DLL construit sur un moteur plus ancien devrait également vérifier que DLL permet à chaque version de son `meitou.json`.

<a id="when-it-finds-something"></a>
## Quand il trouve quelque chose

Appelez le membre d'une manière qui fonctionne sur chaque version que vous prenez en charge : reconstruisez avec le moteur le plus récent si vous supprimez les plus anciens, ou recherchez le membre par réflexion et remplissez les paramètres que vous ne transmettez pas avec leurs valeurs par défaut. Le `InventoryCompat` de Wasteland Remembers fait ce dernier pour `Inventory.Add`.

Les appels de réflexion dans le moteur ne sont pas vérifiés : ils échouent doucement là où vous les avez écrits.

[English](../../../docs/refcheck.md) · [Kenshi Races](../README.md)
