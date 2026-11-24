<a id="publishing-to-the-steam-workshop"></a>
# Publication sur le Steam Workshop

Vous créez l'élément Workshop du mod une fois, à la main, à partir d'une version ZIP . Après cela, [Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) s'exécute sur votre Windows PC et télécharge chaque nouvelle version de GitHub sur cet élément selon un calendrier. Toutes les étapes s'exécutent dans **Windows PowerShell** sur votre PC , connecté au compte Steam qui sera propriétaire de l'élément.

Les commandes utilisent ces valeurs. Définissez-les une fois par fenêtre PowerShell, avec le nom du dossier de votre mod (comme dans `release.yml`  `mod-folder` ) et sa première version :

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Libérez le module

1. Fusionnez `beta` dans `main` , puis ouvrez et fusionnez un PR de `main` dans `release` ([Branches](../README.md#branches) ).
2. Attendez le workflow Release sur `release` . Il balise la fusion `<slug>-v<version>` avec la version dans `meitou.json` (par exemple `example-mod-v0.1.0` ) et crée une version avec `<Mod>-<version>.zip` et `<Mod>-<version>.zip.sha256` . Si cette exécution échoue avant le balisage, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` pousse le tag à la main ( `-DryRun` imprime uniquement les commandes).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Installez Workshop Watcher et connectez-vous à Steam

Suivez [Installer](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) dans le guide Workshop Watcher. Le programme d'installation télécharge steamcmd sur `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` et le connecte à votre compte Steam. Lorsqu'il demande `Add a mod to watch now?` , répondez `n` : l'élément n'existe pas encore. Ignorez cette étape si Workshop Watcher est déjà installé.

<a id="3-prepare-the-items-files"></a>
## 3. Préparez les fichiers de l'article

1. Téléchargez la version ZIP et décompressez-la dans `$Work` :

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` contient désormais `$Mod.dll` , `meitou.json` et `$Mod.mod` .
2. Enregistrez l'image d'aperçu de l'élément sous le nom `$Work\preview.png` : PNG ou JPG , sous 1 MB , carré (512 x 512 pixels fonctionne).

<a id="4-write-the-vdf"></a>
## 4. Écrivez le VDF

steamcmd crée l'élément à partir d'un fichier VDF : un fichier texte de paires `"key" "value"` à l'intérieur d'un bloc `"workshopitem"` . Cette commande écrit `$Work\create.vdf` avec les chemins renseignés. Modifiez d'abord `title` et `description` :

```powershell
$Dir = $Work.Replace('\', '/')
@"
"workshopitem"
{
    "appid"           "233860"
    "publishedfileid" "0"
    "contentfolder"   "$Dir/$Mod"
    "previewfile"     "$Dir/preview.png"
    "visibility"      "2"
    "title"           "[Meitou] My Mod"
    "description"     "What the mod does. Requires MeitouClient."
    "changenote"      "$Mod $Version"
}
"@ | Set-Content "$Work\create.vdf" -Encoding ASCII
```

| Clé | Valeur |
| --- | --- |
| `appid` | `233860` , l'application Steam de Kenshi  ID . |
| `publishedfileid` | `0` crée un nouvel élément. Tout autre numéro met à jour cet élément existant. |
| `contentfolder` | Chemin complet vers le dossier dont les fichiers deviennent le contenu de l'élément : le dossier mod du ZIP , pas son parent. |
| `previewfile` | Chemin d'accès complet à l'image d'aperçu. |
| `visibility` | `0` public, amis `1` uniquement, `2` privé, `3` non répertorié. Commencez par `2` et modifiez-le sur la page de l'article. |
| `title` | Le nom de l'objet sur l'atelier. Démarrez-le avec `[Meitou]` , comme le font les autres mods de code Meitou . |
| `description` | Une ligne de texte. Écrivez ensuite la description complète sur la page de l'article (elle prend en charge le BBCode de Steam). |
| `changenote` | Première entrée de l'onglet Notes de modification de l'élément. |

Règles pour les valeurs :

- Écrivez les chemins avec des barres obliques ( `C:/Users/me/Downloads/...` ). Une barre oblique inverse doit être doublée ( `C:\\Users\\me\\...` ).
- Un guillemet double à l’intérieur d’une valeur s’écrit `\"` .
- Conservez le fichier ASCII .

<a id="5-create-the-item"></a>
## 5. Créez l'élément

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` est le nom avec lequel vous vous connectez à Steam, tel qu'il a été donné au programme d'installation. steamcmd utilise la connexion enregistrée. S'il demande un mot de passe ou un code Steam Guard, exécutez `powershell -ExecutionPolicy Bypass -File $Watcher login` , puis réexécutez la commande.

L'exécution se termine par `Success.` . Ouvrez Steam > votre profil > Workshop Items, ouvrez le nouvel élément et copiez le numéro après `?id=` dans son URL : c'est l'élément ID .

<a id="6-finish-the-items-page"></a>
## 6. Terminez la page de l'article

Sur la page de l'élément dans Steam :

1. Acceptez l'accord juridique de Steam Workshop si la page affiche la bannière. Jusqu’à ce que vous le fassiez, vous seul pouvez voir l’élément.
2. Écrivez la description complète.
3. Si le mod nécessite un autre mod Workshop (par exemple Rivalry Core), ajoutez-le sous **Ajouter/Supprimer des éléments requis**. Les abonnés sont ensuite invités à s'y abonner également. Avec la dépendance dans `meitou.json`  `dependencies` , l'Arrangement du lanceur le place en premier dans l'ordre de chargement.
4. Modifiez la visibilité sur **Public** lorsque la page est prête.

<a id="7-watch-the-item"></a>
## 7. Regardez l'article

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

L'essai à sec indique que la version actuelle est déjà téléchargée. Supprimez `$Work` une fois que c'est fait.

<a id="each-release-after-that"></a>
## Chaque sortie après ça

Libérez le mod comme à l'étape 1. La prochaine exécution programmée de l'observateur télécharge le ZIP , vérifie son SHA -256 et le télécharge sur l'élément, avec le titre de la version et le lien comme note de modification. Il ne modifie jamais le titre, la description, l'aperçu ou la visibilité de l'élément : modifiez ceux sur la page de l'élément. `powershell -ExecutionPolicy Bypass -File $Watcher status` affiche le calendrier, la prochaine exécution et le dernier téléchargement.
