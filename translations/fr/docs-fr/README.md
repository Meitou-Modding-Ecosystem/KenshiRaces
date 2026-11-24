<a id="kenshi-races"></a>
# Kenshi Races

De nouvelles races pour [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) sur les squelettes humains du jeu, jouables dans l'éditeur de personnages : Leoma et Crevan (mâle et femelle) et le Crabfolk (crabman, crabqueen), une bibliothèque d'animation de 59 clips pour chaque race de squelette humain, et le cadre avec lequel d'autres mods ajoutent leurs propres races et sous-races. GPL -3.0 ou version ultérieure.

- Les courses, leurs records et leurs tests en jeu : [docs/kenshi-races.md](docs/kenshi-races.md) .
- Utiliser ces races ou ajouter les vôtres à partir d'un autre mod : [docs/dedependent-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (compétence d'agent `.claude/skills/kenshi-races-dependent` ).
- La bibliothèque d'animations (59 clips pour chaque race sur les squelettes humains) et comment elle a été réalisée : [docs/animations.md](docs/animations.md) .
- Montage d'un corps `.glb` gréé ( Meshy , Hi3D "3ds Max Biped") sur le squelette du jeu : [docs/race-import.md](docs/race-import.md) .

- Construisez et exécutez localement avec Visual Studio , VS Code ou le terminal (le moteur de ce dépôt ou le lanceur installé, déployé dans le jeu) : [docs/play.md](docs/play.md) .
- Configuration des dépôts sur votre machine : [docs/local-development.md](docs/local-development.md) .
- Instruire les agents sur une modification qui s'étend sur les dépôts : [docs/agents-across-repos.md](docs/agents-across-repos.md) .
- Règles d'agent pour le code PIN du moteur, les versions, CI et les versions : [ CLAUDE .md](../../CLAUDE.md) (identique à `AGENTS.md` ) ; procédures étape par étape : `.claude/skills/meitou-engine-release` .
- Vérification d'un mod construit par rapport à la dernière version de Meitou ( CI le fait sur chaque PR ) : [docs/refcheck.md](docs/refcheck.md) .
- Publication du mod sur Steam Workshop pour la première fois, puis à chaque version : [docs/steam-workshop.md](docs/steam-workshop.md) .
- En transformant les modèles `.glb` ( Meshy , Hi3D , Blender ) en structures à construire, aucun FCS n'est nécessaire : [docs/structures.md](docs/structures.md) ; obtenir en premier le nombre de polycomptes, la taille et les textures d'un modèle généré : [docs/model-prep.md](docs/model-prep.md) .

<a id="numbers"></a>
## Nombres

Les types de tâches ( `AddGoalType` , `AddJobType` ) et les codes de dialogue ( `AddDialogueCondition` , `AddDialogueEffect` ) sont partagés par chaque mod chargé ([blocs de numéros de mod](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) ). Kenshi Races n’en revendique aucun.

| Nombre | Piscine | Utiliser |
| --- | --- | --- |

<a id="layout"></a>
## Mise en page

- `engine/` : MeitouClient en tant que sous-module git, épinglé au moteur, valide les builds du mod. `git pull` ne le déplace pas : après chaque exécution d'extraction, `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([conserver les clones](docs/local-development.md#keep-clones-current) actuels).
- `KenshiRaces/` : le projet du mod et `meitou.json` ; `KenshiRaces/items/races/` : les carrosseries montées (sortie import engagée). `KenshiRaces.Tests/` : ses tests (données synthétiques, aucune installation de jeu nécessaire). `<Mod>.Data/` (facultatif) : un générateur qui écrit `<Mod>.mod` à partir du jeu installé. `<Mod>/Structures/` (facultatif) : modèles `.glb` importés en tant que structures constructibles lors du déploiement ([docs/structures.md](docs/structures.md) ).
- `meitou.lock.json` : le framework publie les mods construits contre ([framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ); le workflow de mise à jour du Framework le déplace vers les nouvelles versions et le fusionne lorsque CI passe ([framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ).
- `Play/` : créer, déployer et exécuter en un clic.
- `tools/` : `Update-Local.ps1` , la vérification locale de la traction et des broches du moteur ([conserver les clones](docs/local-development.md#keep-clones-current) actuels) ; `refcheck.sh` et `RefCheck/` , la vérification de référence moteur ([docs/refcheck.md](docs/refcheck.md) ) ; `StructureImport/` , l'import du modèle ([docs/structures.md](docs/structures.md) ) ; `RaceImport/` , le raccord de carrosserie ([docs/race-import.md](docs/race-import.md) ) ; `model-info.py` et `prepare-model.py` , vérifications de modèle et réduction de texture ([docs/model-prep.md](docs/model-prep.md) ).
- `Directory.Build.props` importe celui du moteur, donc les mods sont construits avec les paramètres du jeu (les avertissements sont des erreurs).
- `Directory.Build.rsp` tamponne les références du moteur comme la plus ancienne version Meitou du support des mods, égal à chaque étage `gameVersion` : le jeu refuse un mod faisant référence à un tampon moteur plus récent, et le sous-module construit `1.0.0.0` ([versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ). Le flux de travail de compatibilité Meitou vérifie quotidiennement les mods par rapport aux nouvelles versions de Meitou ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ).

<a id="branches"></a>
## Succursales

PR dans `beta` ; CI (build et tests) doit réussir. `beta` est promu `main` une fois testé en jeu, et la production passe par un PR vers `release` . Chaque fusion dans `release` balise le `<slug>-v<version>` et publie le zip du mod ( `.github/workflows/release.yml` ) à moins que cette balise n'existe ; `tools\Tag-Release.ps1` est la solution de secours manuelle.

- Protections des branches du nouveau référentiel et révisions permanentes de la version bêta/principale/version : [docs/branch-policy.md](docs/branch-policy.md) . Source de copie des modèles ; GutenStructure applique les paramètres via la réconciliation.
