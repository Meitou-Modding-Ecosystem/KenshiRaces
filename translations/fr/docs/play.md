<a id="build-and-run-locally"></a>
# Compiler et lancer le jeu en local

La compilation du projet `Play` construit le moteur et tous les mods du dépôt, copie les mods et les frameworks indiqués dans `meitou.lock.json` dans le dossier des mods du jeu, puis lance le jeu. Visual Studio, VS Code et le terminal utilisent la même procédure.

Installez le SDK.NET 11 et récupérez les sous-modules avec `git clone --recurse-submodules <repo URL>` ou, dans un clone existant, `git submodule update --init --recursive`. Si les sous-modules manquent, la compilation s’arrête avec `MEITOU001` et indique la commande à exécuter.

<a id="visual-studio"></a>
## Visual Studio

1. Ouvrez le `.slnx` du dépôt.
2. Définissez `Play` comme projet de démarrage si Visual Studio ne l'a pas sélectionné (clic droit > Définir comme projet de démarrage).
3. Choisissez un profil dans la liste déroulante à côté du bouton Démarrer, puis appuyez sur F5 (débogage) ou Ctrl+F5 :

| Profil | Fonctionne | Utilisez-le pour |
| --- | --- | --- |
| `Play (engine + mods)` | `MeitouClient.exe` construit à partir du sous-module `engine` de ce dépôt | tester et déboguer votre mod ; points d'arrêt dans le travail du code du mod |
| `Meitou launcher (installed)` | le lanceur installé à partir des versions Meitou, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | changer la liste des mods ou l'ordre de chargement et faire confiance au code d'un nouveau mod |

<a id="vs-code"></a>
## VS Code

1. Installez l'extension C# Dev Kit (VS Code la propose lorsque vous ouvrez le dossier ; elle se trouve dans `.vscode/extensions.json`).
2. Ouvrez le dossier racine du dépôt (Fichier > Ouvrir le dossier).
3. Exécuter et déboguer (Ctrl+Shift+D), choisissez **Play (engine + mods)**, appuyez sur F5. Il construit et déploie d'abord, puis démarre le moteur sous le débogueur ; points d'arrêt dans le travail du code du mod.
4. Pour le lanceur installé : Terminal > Exécuter la tâche > **Meitou launcher (installed)**. Il construit et déploie, puis ouvre le lanceur.

Terminal > Exécuter la tâche de construction (Ctrl+Maj+B) construit et déploie uniquement.

<a id="without-an-editor"></a>
## Sans éditeur

Cliquez avec le bouton droit sur `tools\Build.ps1` > **Exécuter avec PowerShell**. Il construit et se déploie comme `dotnet build` (en vérifiant d'abord les sous-modules s'ils sont manquants) et garde la fenêtre ouverte afin que vous puissiez lire le résultat. `-Test` exécute également les tests ;  Le `-Run` démarre alors le moteur.

<a id="terminal"></a>
## Terminal

Dans le dossier racine du dépôt :

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` et `dotnet test` construisent et testent chaque projet.
- `dotnet run --project Play` construit, déploie et démarre le moteur (le profil "Play (moteur + mods)").
- Pour utiliser le lanceur installé à la place, déployez sans démarrer le moteur, puis ouvrez le lanceur :

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## À quoi sert une version de jeu

- Recherche les mods : chaque dossier de niveau supérieur contenant `meitou.json` et `<Folder>/<Folder>.csproj`. Un nouveau mod ne nécessite aucune modification de `Play`.
- Construit le moteur et chaque mod dans **Release**, quelle que soit la configuration que vous avez choisie : les builds du moteur de débogage incluent `TaskStart.CheckStarts` une fois qu'un mod démarre la tâche d'un personnage. Les points d'arrêt dans le code du mod fonctionnent toujours (les symboles de version sont déployés).
- Copie les `<Mod>.dll`, `<Mod>.pdb` et `meitou.json` de chaque mod vers `<Kenshi>\mods\<Mod>\`. Fermez d'abord le jeu : un jeu en cours verrouille la DLL.
- Copie chaque framework de `meitou.lock.json` depuis son dossier de version téléchargé vers `<Kenshi>\mods\<Framework>\` ou, pour un framework construit à partir d'un clone ([local-development.md](local-development.md)), sa sortie de build.
- Un mod avec un générateur de données (`<Mod>.Data/<Mod>.Data.csproj`, exécuté sous le nom `<generator> <Kenshi folder> <output .mod>`) obtient son `<Mod>.mod` généré à partir du jeu installé ; un échec est un avertissement de build et le mod est déployé sans lui. Tous les autres mods obtiennent un `<Mod>.mod` vide alors qu'il n'en a pas ; un existant n’est jamais remplacé.
- Un mod avec un dossier `Structures` obtient ses modèles `.glb` importés dans son dossier déployé, les enregistrements fusionnés dans son `.mod` ([structures.md](structures.md)).
- Lorsque le dossier mods n'existe pas, rien n'est copié et le build le dit.
- Le bouton Lecture du lanceur démarre la version qu'il a installée, pas le moteur de ce dépôt. Fermez-le et exécutez "Play (moteur + mods)" pour tester votre build.

<a id="settings"></a>
## Paramètres

Valeurs par défaut : jeu `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`, dossier mods `<game>\mods`. Modifiez-les avec la variable d'environnement `KENSHI_PATH` (le moteur la lit également, donc les deux sont d'accord), ou par machine dans un `Meitou.user.props` ignoré par git dans le dossier racine du dépôt :

```xml
<Project>
  <PropertyGroup>
    <KenshiDir>D:\Games\Kenshi</KenshiDir>           <!-- game folder; also sets the mods folder -->
    <KenshiModsDir>D:\Games\Kenshi\mods</KenshiModsDir> <!-- mods folder only -->
    <PlayConfiguration>Debug</PlayConfiguration>      <!-- engine and mods configuration (default Release) -->
    <PlayDeploy>false</PlayDeploy>                    <!-- build and run without copying -->
    <PlayArguments>--continue</PlayArguments>         <!-- engine command line (Visual Studio and dotnet run) -->
  </PropertyGroup>
</Project>
```

Le moteur trouve le jeu tout seul (Steam) ; `KenshiDir` défini uniquement dans `Meitou.user.props` ne lui est pas transmis.  La configuration de lancement de VS Code exécute le moteur Release ; modifiez `.vscode/launch.json` localement si vous modifiez `PlayConfiguration`.

<a id="first-run"></a>
## Première course

- Exécutez une fois le lanceur installé (de l’une des manières ci-dessus) : activez le mod et ses frameworks et faites confiance à son code.
- Si la copie échoue avec "accès refusé", accordez à votre utilisateur les droits de modification sur `Kenshi\mods` (Propriétés > Sécurité) plutôt que d'exécuter votre éditeur en tant qu'administrateur.

[English](../../../docs/play.md) · [Kenshi Races](../README.md)
