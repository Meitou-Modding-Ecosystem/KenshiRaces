<a id="kenshi-races"></a>
# Kenshi Les races

Nouvelles races pour [MétroClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) sur les skeletons humains du jeu, jouable dans l'éditeur du personnage: Leoma et Crevan (le mâle et la femelle) Crabfolk (Crabman, crabqueen) une bibliothèque d'animation de 59 clips pour chaque race humaine, et le cadre d'autres modes ajoutent leurs propres races et sous-races. GPL0 ou plus tard.

- Les courses, leurs records et les tester dans le jeu: [docs/kenshi-races.md](docs/kenshi-races.md).
- Utiliser ces courses ou ajouter votre propre à partir d'un autre mode: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (Compétence de l’agent
`.claude/skills/kenshi-races-dependent`).
- La bibliothèque d'animation (environ 59 clips pour chaque race sur les skeletons humains) et comment elle a été faite: [docs/animations.md](docs/animations.md).
- Faire une rigide `.glb` Le corps (Meshy, Hi3D "3ds Max Biped") sur le squelette du jeu: [docs/race-import.md](docs/race-import.md).

- Construire et fonctionner localement avec Visual Studio, VS Code ou le terminal (moteur de ce repo ou le lanceur installé, déployé pour le jeu): [docs/play.md](docs/play.md).
- Mettre le repos sur votre appareil : [docs/local-development.md](docs/local-development.md).
- Instruire des agents sur un changement qui pousse au repos : [docs/agents-across-repos.md](docs/agents-across-repos.md).
- Les règles d'agent pour le pin, les versions, les sorties : [CLAUDE.md](../../CLAUDE.md) (Tout comme `AGENTS.md`Les procédures étape par étape :
`.claude/skills/meitou-engine-release`.
- Vérifier un mod construit contre le dernier Meitou (CI le fait sur chaque PR): [docs/refcheck.md](docs/refcheck.md).
- Publier le mode à l'atelier Steam pour la première fois, puis sur chaque édition: [docs/steam-workshop.md](docs/steam-workshop.md).
- tourner `.glb` Le modèle (Meshy, Hi3D, Blender) dans des structures construites, non FCS Il faut : [docs/structures.md](docs/structures.md);
Obtenir le polycount, la taille et les textures d'un modèle généré tout d'abord: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## Nombres

Les types de tâches (`AddGoalType`, `AddJobType`et les codes de dialogue (`AddDialogueCondition`, `AddDialogueEffect`Il est partagé par tous les modes ([Mod numéro blocs](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi Les races ne prétendent rien.

| Numéro | Piscine | Utiliser |
| --- | --- | --- |

<a id="layout"></a>
## Layout

- `engine/`: MeitouClient En tant que sous-module git, appuyé sur le moteur communique le mod construit contre. `git pull` Il n’y a pas de mouvement : après
Chaque pull run `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([Mettre les clones à jour](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`Le projet de mod et `meitou.json`; `KenshiRaces/items/races/`- les organismes équipés (produits d’importation commandés).
`KenshiRaces.Tests/`: ses tests (données synthétiques, aucune installation de jeu n'est nécessaire). `<Mod>.Data/` (Optionnel): un générateur qui écrit `<Mod>.mod` du jeu installé. `<Mod>/Structures/` (dans le cadre de l’événement : `.glb` les modèles importés comme structures constructives en déploiement ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`: le framework libère les mods construits contre
([Le framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)Le flux de travail de mise à jour du cadre le transforme vers de nouvelles éditions et se fusionne lorsque CI passe ([Actualité Framework](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`Un clic pour construire, déployer et exécuter.
- `tools/`: `Update-Local.ps1`Le moteur et le moteur-pin ([Mettre les clones à jour](docs/local-development.md#keep-clones-current));
`refcheck.sh` et `RefCheck/`Le moteur de référence ([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`Le modèle d’importation ([docs/structures.md](docs/structures.md)); `RaceImport/`Le Corps de l’Homme ([docs/race-import.md](docs/race-import.md)); `model-info.py` et `prepare-model.py`- les modèles et les textures de l'écran ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` Importe le moteur, donc les modes se construisent avec les paramètres du jeu (les alertes sont des erreurs).
- `Directory.Build.rsp` Les références du moteur sont imprimées comme le plus ancien Meitou délivre le support mods, égal à chaque `gameVersion` Étiquette : le
Le jeu rejette un mod qui fait référence à un nouveau moteur, et le sous-module construit `1.0.0.0` ([La version](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). Le flux de travail de la compatibilité de Meitou va voir les mods quotidiennement contre les nouvelles sorties de Meitou ([Métro compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## Les branches

PRS à l'intérieur `beta`C’est la construction et les tests qui doivent être effectués. `beta` Il est promu pour `main` Une fois testé dans le jeu, et la production passe par un PR à `release`. Chacun s’engage dans `release` Téléchargez `<slug>-v<version>` et publie le mod's zip (`.github/workflows/release.yml`a) sauf si ce type de marque existe ; `tools\Tag-Release.ps1` C’est le manuel de Fallback.

- Nouveaux protecteurs de branches et permanents beta/main/release Les critiques : [docs/branch-policy.md](docs/branch-policy.md). Templates source; GutenStructure applique les paramètres par la réconciliation.
