<a id="build-and-run-locally"></a>
# Construire et fonctionner localement

Le bâtiment `Play` construit le moteur et chaque mod dans ce repo, copie les modes et les cadres `meitou.lock.json` Les noms sont dans la boîte modes du jeu, puis le jeu commence. Utilisez n'importe quel outil que vous aimez : Visual Studio, VS Code ou un terminal qui fonctionne tous les mêmes.

Tous les trois ont besoin.NET 11 SDK et une clone avec ses sous-modules (`git clone --recurse-submodules <repo URL>`ou `git submodule update --init --recursive` dans un clone existant. Sans eux, la construction s’arrête. `MEITOU001`On appelle ce commandement.

<a id="visual-studio"></a>
## Studio visuel

1. Ouverture du repo `.slnx`.
2. Sélectionner `Play` Si le projet de démarrage n’a pas été sélectionné, il sera placé par Visual Studio (clic sur droite > Démarrer comme Projet de démarrage).
3. Choisissez un profil dans le bouton de descente à côté du bouton Démarrer, puis appuyez sur F5 (dégoût) ou Ctrl+F5 :

| Profils | Rours | Utilisez-le pour |
| --- | --- | --- |
| Jeu (moteur + mods) | `MeitouClient.exe` construit à partir de ce récapitulatif `engine` sous-module | Tester et déboucher sur votre mod; les points de dégradation dans le code mod |
| Meitou lanceur (installé) | Le lanceur installé de la Meitou délivre, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | Modifier la liste de mods ou mettre à jour l'ordre de chargement et faire confiance à un nouveau code de mods |

<a id="vs-code"></a>
## contre le code

1. Installez l'extension C# Dev Kit (VS Code vous la propose lors de l'ouverture de la dossier; il est `.vscode/extensions.json`).
2. Ouvrez la dossier racine du repo (File > Open Folder).
3. Exécution et déboogage (Ctrl + Shift + D) **Jeu (moteur + mods)**La pression F5. Il construit et déploie d'abord, puis le moteur commence sous le
Début ; points de débris dans le mode de travail.
4. Pour le lanceur installé: Terminal > Task Run > **Meitou lanceur (installé)**. Il construit et déploie, puis ouvre le lanceur.

Terminal > Run Build Task (Ctrl+Shift+B) ne se construit et ne déploie que.

<a id="without-an-editor"></a>
## Sans éditeur

Cliquer droit `tools\Build.ps1` > **Réservez avec PowerShell**. Il construit et déplore comme `dotnet build` (Vous vérifiez les sous-modules d'abord si ils sont absents) et garder la fenêtre ouverte afin que vous puissiez lire le résultat. `-Test` Il effectue également les tests. `-Run` Puis le moteur commence.

<a id="terminal"></a>
## Terminal

Dans la boîte de la racine de la repo :

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` et `dotnet test` Construire et tester chaque projet.
- `dotnet run --project Play` Il crée, déplore et démarre le moteur (le profil « Play (moteur + mods) »).
- Pour utiliser le lanceur installé, déployer sans démarrer le moteur, puis ouvrir le lanceurs:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## Ce qu’un jeu construit

- Trouver les modes: chaque maintien de dossiers de haut niveau `meitou.json` et `<Folder>/<Folder>.csproj`. Un nouveau mode n’a pas besoin de changement `Play`.
- Le moteur et chaque mode **libération**, quelle que soit la configuration que vous avez choisie : le moteur de déboucher construit le jet dans `TaskStart.CheckStarts`
Une fois qu'un mod commence, la tâche d'un personnage. Les points de défaut dans le code de mode sont encore en fonctionnement (les symboles de libération sont déployés).
- Copie de chaque mod `<Mod>.dll`, `<Mod>.pdb` et `meitou.json` à `<Kenshi>\mods\<Mod>\`. Fermez le jeu d'abord : un jeu en cours bloque le DLL.
- Copier chaque cadre dans `meitou.lock.json` de la version téléchargée à `<Kenshi>\mods\<Framework>\`ou pour un cadre construit
Pour une clone ([local-development.md](local-development.md)et sa production de construction.
- Un mod avec un générateur de données (`<Mod>.Data/<Mod>.Data.csproj`courir comme `<generator> <Kenshi folder> <output .mod>`Elle obtient son `<Mod>.mod`
généré à partir du jeu installé; un échec est un avertissement de construction et le mod est déployé sans lui. Chaque autre mode a un vide `<Mod>.mod` Quand il n'a pas d'autre, un existant n'est jamais remplacé.
- Un mode avec un `Structures` Le dossier a été `.glb` Les modèles importés dans son dossier déployé, les dossiers intégrés dans son `.mod`
([structures.md](structures.md)).
- Lorsque le dossier mods n'existe pas, rien n'est copié et le bâtiment dit cela.
- Le bouton de jeu du lanceur commence par le lancement qu'il a installé, pas le moteur de ce repo. Fermez et exécutez "Play (moteur + mods)" pour tester votre
construire .

<a id="settings"></a>
## Les paramètres

Défauts : jeu `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Modes de dossier `<game>\mods`. Changez-les avec le `KENSHI_PATH` variable de l'environnement (le moteur le lit aussi, donc les deux sont d'accord), ou par machine dans un git-ignoré `Meitou.user.props` Dans la boîte de la racine du repo :

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

Le moteur trouve le jeu sur lui-même (Steam); `KenshiDir` Seulement dans la `Meitou.user.props` n’est pas passée à ça. La configuration de lancement de VS Code fonctionne avec le moteur de sortie ; et `.vscode/launch.json` localement si vous changez `PlayConfiguration`.

<a id="first-run"></a>
## Première course

- Exécutez le lanceur installé une fois (cette des façons ci-dessus): activez le mod et ses cadres, et faites confiance à son code.
- Si la copie échoue avec « l’accès refusé », donnez à votre utilisateur les droits de modifier `Kenshi\mods` (Propriétés > Sécurité) plutôt que d'exécuter votre
L’éditeur comme administrateur.
