<a id="local-development-setup"></a>
# Développement local

Clôtez seulement le repos sur lequel vous travaillez dans une seule feuille. Chacun construit lui-même avec `dotnet build`Et les cadres qu'il faut télécharger et les tester eux-mêmes.

<a id="repos"></a>
## Repos

Chaque mod repo est nommé par son mod.

| Repo | Gardez | PRS va à |
| --- | --- | --- |
| MeitouClient | Le moteur et ses modes | `fork` |
| RivalryCore | Rivalry Core et ses compétences | `beta` |
| Logistique | Le cadre logistique | `beta` |
| Guerre de l'Orchestre | Système d’orchestre de guerre (Us Logistics Core) | `beta` |
| BastFront | Bast Front Melee (sûr de Guerre et de Logistique) | `beta` |
| AshenHand | La main de l'aiguille (Use Rivalry Core) | `beta` |
| mariage | mariage | `beta` |
| Squadron | SquadArena mod et l'outil de notation de l'équipe | `beta` |
| La structure (TheIvoryDisaster) | CI partagé, emballage, porte de libération, cible de clôture | `beta` |

<a id="folders"></a>
## Les dossiers

Ces étapes utilisent huit dossiers. Mettez-les comme des variables dans chaque nouvelle fenêtre de PowerShell, puis appuyez sur les commandes ci-dessous comme elles sont:

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
## Les précautions

- .NET 11 SDK (Tout le repo) `global.json` Le nom de la version) et Git.
- Un éditeur : Visual Studio, VS Code avec le kit C# Dev, ou Rider. Un seul terminal fonctionne.
- Kenshi Installé par . Si elle n’est pas dans `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Et c’est la `KENSHI_PATH` Environnement variable
à son dossier.

<a id="clone"></a>
## Clone

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Chaque mod repo a sa propre `engine` Le sous-module est donc `$Engine` Il est nécessaire d’écrire des moteurs.

<a id="build-and-play-one-mod"></a>
## Créer et jouer un mode

**Changement de mode à repo :**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- Le premier bâtiment télécharge chaque cadre nommé dans `meitou.lock.json` à entrer `deps` Il s’arrête si son SHA256 n’est pas le cas.
- `dotnet run --project Play` Copie les cadres fermés et le mod `Kenshi\mods` Et commence le moteur. Visual Studio vs. Code
Le lanceur installé : [play.md](play.md).
- Autoriser les cadres au-dessus du mod dans le lanceur, comme pour toute dépendance.

<a id="change-a-framework-and-a-mod-together"></a>
## Changer un cadre et un mod ensemble

Affichez le mod repo sur votre clone de cadre :

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Les bâtiments en `$Ashen` Composer Rivalry Core à partir `$Rivalry` Utilisez le moteur en `$Rivalry\engine`Et le jeu déploie ce que Rivalry Core construit.
- `Meitou.user.props` Il est ignoré. Si elle tient déjà votre Kenshi Mettez les deux lignes à l'intérieur `PropertyGroup` Au lieu de cela.
- `MeitouFrameworkSources` Il s’agit d’un parcours de projet séparé par `;`et g. `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` nom du moteur sur lequel se construit le clone de cadre.
- Supprimer la ligne pour revenir au lock ZIP.

**Les chaînes de cadre.** Bâti Front `meitou.lock.json` Les lignes de l'orchestre de guerre et de la logistique. Pour changer les deux cadres et Bast Front ensemble, surrêtez les deux dans Bast Front :

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

Le surride s'éteint, donc l'orchestre de guerre se construit contre `$Logistics` Et tout ça, c’est un moteur. Il n’est plus que la War Orchestration et il se construit contre le logistique de base nommé dans le bloc de Bast Front.

Pour le livrer : fusionner le cadre PR dans les `beta` de `$Rivalry`qui publie un prétexte `rivalry-core-<version>-beta.<commit>`. Puis, dans `$Ashen`et Set `meitou.lock.json` à ce titre et à l’actif, SHA-256 et le moteur s'engage dans ses notes de sortie, et ouvre le mode PR. Dans une chaîne, le navire en bas : Logistique, puis une orchestration de guerre, le fermeture, puis Bast Front. Après la sortie du cadre, déplacez le `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Changer le moteur et un mod ensemble

Vérifiez votre branche de moteur à l'intérieur du sous-module de mod repo:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Ne communiquez pas avec le nouveau moteur : les pins de moteur ne se déplacent que vers les tags de sortie, donc le changement atteint ce repo une fois.
Il la libère.
- Un mod qui utilise un cadre a également besoin du cadre reconstruit sur ce moteur. Utilisez le framework et vérifiez le même
Le moteur en `$Rivalry\engine`.
- Commande pour l'atterrissage : le moteur PR `fork`Après la sortie, le cadre PR `$Rivalry\engine` Pour la libération, il y a
Mise en place de l’Outlook et `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Changer le moteur pour jouer ou tester

Pour jouer au dernier upstream, à une sortie ou à une branche de moteur, `$Engine` Clone avec le script dans n'importe quel mod repo :

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`Le dernier engagement de VlcekM/MeitouClient `master` (Le scénario ajoute le `upstream` à distance en cas d’absence).
- `-To Release`Le plus récent upstream `vX.Y.Z`.
- `-To Fork`L’org fork est `fork` La branche, elle est tirée.
- `-Branch <name>`Une branche de l'org, comme une branche de la PR.
- Sans `-Path` Il utilise le `MeitouClient` La carte est située à côté du mod repo. `-Run` - Créer le jeu en Release et le lancer; `-Build` Seulement
construit le .
- La sortie, la sortie et `-Branch` laisse le clone sur un comité détaché ; le local `master` et `fork` Les branches ne sont pas modifiées. Le script
Il s’arrête sans rien changer lorsque le clone a des changements inédits.
- Il ne touche jamais le mod repo `engine` Le sous-module (ce qui est `Update-Local.ps1` et les pins).

<a id="keep-clones-current"></a>
## Mettre les clones à jour

Après chaque tir, et à chaque fois qu'une construction échoue avec les types de moteurs manquants (par exemple, `The type or namespace name 'RelationCause' could not be found`), fermez Visual Studio et exécutez le script de mise à jour dans le repo du mod :

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Il s’échange à `beta` Et puis, il s'en va, il s’actualisera. `engine` et `build/gutenstructure` Pour les commissions `beta` Les pins (`git pull` seul
Il les laisse sur les anciens). `-Branch <name>` Il fait une autre branche.
- Il vérifie chaque sous-module contre son pin et imprimera la version du moteur.
- Il est drapeau `MeitouEngineDir` et `MeitouFrameworkSources` Dans `Meitou.user.props` ou l’environnement : ils font l’usage d’un autre
le moteur ou le cadre de repo au lieu des pins de ce repo. Supprimez-les, sauf si vous modifiez un cadre et un mod ensemble.
- `-Clean` Supprimer le Git-ignoré `bin` et `obj` Des fichiers, donc aucune construction de l'ancien moteur est réutilisée. `-Build` Rondeur `dotnet build`.
- Il se termine avec `Up to date` Une liste de lignes marquées `FIX`.

La prochaine construction télécharge un cadre à nouveau seulement lorsque `meitou.lock.json` changé par .
