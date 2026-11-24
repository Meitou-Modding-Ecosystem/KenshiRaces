# Kenshi Races

[English](../../README.md) · Français · [Deutsch](../de/README.md) · [Español](../es/README.md) · [Italiano](../it/README.md) · [Português do Brasil](../pt-BR/README.md) · [Русский](../ru/README.md) · [日本語](../ja/README.md) · [한국어](../ko/README.md) · [简体中文](../zh-CN/README.md)

Kenshi Races ajoute à [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) trois races jouables dans l’éditeur de personnages. Les Leoma et les Crevan disposent de corps masculins et féminins. Les Crabfolk comprennent deux variantes, `crabman` et `crabqueen`. Toutes utilisent les squelettes humains du jeu. Le mod fournit aussi 59 animations utilisables par toutes les races partageant ces squelettes, ainsi qu’un framework permettant à d’autres mods d’ajouter leurs propres races et sous-races. GPL-3.0-or-later.

## Guides

- [Les races, leurs données et les essais en jeu](docs/kenshi-races.md)
- [Utiliser le framework dans un autre mod et ajouter des sous-races](docs/depending-on-kenshi-races.md)
- [Bibliothèque de 59 animations et méthode de création](docs/animations.md)
- [Adapter un corps GLB au squelette du jeu](docs/race-import.md)
- [Compiler et lancer le jeu avec Visual Studio, VS Code ou le terminal](docs/play.md)
- [Configurer et mettre à jour les dépôts locaux](docs/local-development.md)
- [Coordonner les changements entre plusieurs dépôts](docs/agents-across-repos.md)
- [Vérifier la compatibilité avec une nouvelle version de Meitou](docs/refcheck.md)
- [Publier sur le Steam Workshop](docs/steam-workshop.md)
- [Importer un modèle GLB comme bâtiment sans utiliser le FCS](docs/structures.md)
- [Préparer la géométrie, les dimensions et les textures](docs/model-prep.md)
- [Protection des branches et règles de revue](docs/branch-policy.md)
- [Règles du dépôt et procédures pour les agents](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## Identifiants partagés

Les types de tâches et les codes de dialogue appartiennent à un espace de numérotation partagé par tous les mods chargés. Kenshi Races ne réserve aucun numéro. (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## Organisation du dépôt

- `engine/` — Sous-module MeitouClient fixé au commit utilisé pour compiler le mod. Après chaque extraction des changements, mettez-le à jour avec la commande ci-dessous. [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — Projet et manifeste du mod. Les corps adaptés sont versionnés dans le dépôt. Les tests utilisent des données synthétiques et ne nécessitent pas le jeu.
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — Les projets facultatifs génèrent les données à partir du jeu installé ou importent des modèles GLB en bâtiments au déploiement. [StructureImport](docs/structures.md)
- `meitou.lock.json` — Verrouille les versions des frameworks. Le workflow de mise à jour passe aux nouvelles versions puis fusionne après réussite de la CI. [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — Compiler, déployer et lancer en une étape.
- `tools/` — Outils de synchronisation, de vérification des références moteur, d’import des bâtiments, d’adaptation des corps et de préparation des textures. [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — Importe la configuration du moteur. Les avertissements de compilation sont traités comme des erreurs.
- `Directory.Build.rsp` — Fixe la version des références moteur au minimum accepté par le mod. Le jeu refuse une référence plus récente que son moteur. Le sous-module utilise la version d’assembly par défaut. (`gameVersion`, `1.0.0.0`).  Le contrôle de compatibilité s’exécute chaque jour sur les nouvelles versions de Meitou. [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## Branches

Envoyez les PR vers beta. Compilation et tests doivent réussir. Après essai en jeu, beta peut être promue vers main. La production passe par une PR vers release. Chaque fusion sur release crée le tag de version et publie le ZIP, sauf si le tag existe déjà. La commande de marquage reste disponible en secours. (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
