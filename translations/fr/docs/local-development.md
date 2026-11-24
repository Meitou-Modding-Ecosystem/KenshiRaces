<a id="local-development-setup"></a>
# Configurer et mettre à jour les dépôts locaux

Clonez uniquement les dépôts sur lesquels vous travaillez dans un seul dossier. Chacun se construit seul avec `dotnet build`, et les frameworks dont il a besoin se téléchargent et se vérifient eux-mêmes.

<a id="repos"></a>
## Repos

Chaque dépôt de mod porte le nom de son mod.

| Repo | Détient | Les PR vont à |
| --- | --- | --- |
| MeitouClient | le moteur et ses crochets de mod | `fork` |
| RivalitéCore | Framework Rivalry Core et ses compétences d’agent | `beta` |
| LogistiqueCore | Cadre de base de la logistique | `beta` |
| GuerreOrchestration | Cadre d'orchestration de guerre (utilise Logistics Core) | `beta` |
| BastFront | Bast Front mêlée (utilise l'orchestration de guerre et le noyau logistique) | `beta` |
| CendreMain | La main cendrée (utilise le noyau de rivalité) | `beta` |
| Agriculture | Agriculture | `beta` |
| SquadArène | Mod SquadArena et l'outil d'évaluation des équipes | `beta` |
| GutenStructure (Le désastre ivoire) | partagé CI, emballage, porte de libération, cible de verrouillage du cadre | `beta` |

<a id="folders"></a>
## Dossiers

Ces étapes utilisent huit dossiers. Définissez-les comme variables dans chaque nouvelle fenêtre PowerShell, puis collez les commandes ci-dessous telles quelles :

```powershell
$Repos     = "C:\Users\<you>\source\repos"
$Engine    = "$Repos\MeitouClient"
$Rivalry   = "$Repos\RivalryCore"
$Ashen     = "$Repos\AshenHand"
$Husbandry = "$Repos\Husbandry"
$Logistics = "$Repos\LogisticsCore"
$War       = "$Repos\WarOrchestration"
$Bast      = "$Repos\BastFront"
```

<a id="prerequisites"></a>
## Conditions préalables

-. NET 11 SDK (le `global.json` de chaque dépôt nomme la version) et Git.
- Un éditeur : Visual Studio, VS Code avec le C# Dev Kit, ou Rider. Un terminal seul fonctionne également.
- Kenshi installé. Si ce n'est pas le cas dans `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`, définissez la variable d'environnement `KENSHI_PATH` dans son dossier.

<a id="clone"></a>
## Cloner

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Chaque référentiel de mod possède son propre sous-module `engine`, donc `$Engine` n'est nécessaire que pour écrire des hooks de moteur.

<a id="build-and-play-one-mod"></a>
## Construisez et jouez un mod

**Modification du dépôt du mod :**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- La première version télécharge chaque framework nommé dans `meitou.lock.json` dans `deps` et s'arrête si son SHA -256 ne correspond pas.
- `dotnet run --project Play` copie les frameworks verrouillés et le mod dans `Kenshi\mods` et démarre le moteur. Visual Studio, VS Code et le lanceur installé : [play.md](play.md).
- Activez les frameworks au-dessus du mod dans le lanceur, comme pour toute dépendance.

<a id="change-a-framework-and-a-mod-together"></a>
## Changer un framework et un mod ensemble

Pointez le dépôt de mod sur votre clone de framework :

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Les builds dans `$Ashen` compilent désormais Rivalry Core à partir de `$Rivalry` et utilisent le moteur de `$Rivalry\engine`, et Play déploie cette build Rivalry Core.
- `Meitou.user.props` est ignoré par git. S'il contient déjà vos paramètres Kenshi, ajoutez plutôt les deux lignes à l'intérieur de son `PropertyGroup`.
- `MeitouFrameworkSources` emprunte des chemins de projet-cadre séparés par `;`, par ex. `$Logistics\LogisticsCore\LogisticsCore.csproj`. `MeitouEngineDir` nomme le moteur sur lequel le clone du framework s'appuie.
- Supprimez la ligne pour revenir au ZIP verrouillé.

**Chaînes de framework.**Le `meitou.lock.json` de Bast Front répertorie le noyau d'orchestration de guerre et de logistique. Pour modifier les frameworks et Bast Front ensemble, remplacez les deux dans Bast Front :

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

Le remplacement est transmis, donc War Orchestration s'appuie également sur `$Logistics`, et l'ensemble de la version utilise un seul moteur. Remplacez uniquement War Orchestration et il s'appuie sur le noyau logistique nommé dans le verrou de Bast Front.

Pour l'expédier : fusionnez le framework PR dans `beta` de `$Rivalry`, qui publie une prerelease `rivalry-core-<version>-beta.<commit>`. Ensuite, dans `$Ashen`, définissez `meitou.lock.json` sur cette balise et l'actif, SHA -256 et la validation du moteur à partir de ses notes de version, et ouvrez le mod PR. Dans une chaîne, expédiez de haut en bas : Logistics Core, puis une construction War Orchestration le verrouillant, puis Bast Front verrouillant les deux. Après la sortie de production du framework, déplacez le verrou vers `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Changer le moteur et un mod ensemble

Vérifiez la branche de votre moteur dans le sous-module du dépôt de mod :

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Ne validez pas le nouveau pointeur de moteur : les broches du moteur se déplacent uniquement vers les balises de version en amont, de sorte que la modification atteint ce dépôt une fois que l'amont l'a publié.
- Un mod qui utilise un framework a également besoin que le framework soit reconstruit sur ce moteur. Utilisez le remplacement du framework ci-dessus et consultez la même branche de moteur dans `$Rivalry\engine`.
- Ordre de l'atterrir : moteur PR dans `fork` ; après sa publication en amont, le framework PR déplace `$Rivalry\engine` vers cette version, puis le mod PR déplace son verrou et `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Changez le moteur pour jouer ou tester

Pour lire la dernière version en amont, une version ou une branche de moteur, changez votre clone `$Engine` avec le script dans n'importe quel dépôt de mod :

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : le dernier commit de VlcekM/ MeitouClient  `master` (le script ajoute la télécommande `upstream` en cas d'absence).
- `-To Release` : la dernière balise de version en amont `vX.Y.Z`.
- `-To Fork` : branche `fork` de l'org fork, retirée.
- `-Branch <name>` : une branche du fork org, comme une branche du moteur PR.
- Sans `-Path`, il utilise le dossier `MeitouClient` à côté du dépôt de mod. `-Run` construit le jeu dans Release et le démarre ; `-Build` ne fait que le construire.
- Upstream, Release et `-Branch` laissent le clone sur une validation détachée ; les branches locales `master` et `fork` ne sont pas modifiées. Le script s'arrête sans rien changer lorsque le clone comporte des modifications non validées.
- Il ne touche jamais le sous-module `engine` du dépôt de mod (c'est-à-dire `Update-Local.ps1` et les broches).

<a id="keep-clones-current"></a>
## Gardez les clones à jour

Après chaque pull, et chaque fois qu'une build échoue avec des types de moteur manquants (par exemple `The type or namespace name 'RelationCause' could not be found`), fermez Visual Studio et exécutez le script de mise à jour dans le dépôt du mod :

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Il passe à `beta` et l'extrait, puis met à jour `engine` et `build/gutenstructure` avec les broches `beta` validées (`git pull` seul les laisse sur les anciennes). `-Branch <name>` tire une autre branche.
- Il vérifie chaque sous-module par rapport à sa broche et imprime la version du moteur.
- Il signale `MeitouEngineDir` et `MeitouFrameworkSources` dans `Meitou.user.props` ou l'environnement : ils obligent la construction à utiliser le moteur ou le framework d'un autre dépôt au lieu des broches de ce dépôt. Supprimez-les sauf si vous modifiez ensemble un framework et un mod.
- `-Clean` supprime les dossiers `bin` et `obj` ignorés par git, donc aucune version de l'ancien moteur n'est réutilisée. `-Build` exécute `dotnet build`.
- Il se termine par `Up to date` ou une liste de lignes marquées `FIX`.

La version suivante télécharge à nouveau un framework uniquement lorsque `meitou.lock.json` a été modifié.

[English](../../../docs/local-development.md) · [Kenshi Races](../README.md)
