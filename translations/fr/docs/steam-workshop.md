<a id="publishing-to-the-steam-workshop"></a>
# Publication au Steam Workshop

Vous créez l'élément du mod une fois, par la main, à partir d'une sortie ZIP. Après cela, [Le travail de Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) fonctionne sur votre PC Windows et télécharge chaque nouvelle version de GitHub à cet élément sur un calendrier. Toutes les étapes sont en cours **Windows pour PowerShell** Sur votre PC, vous êtes inscrit sur le compte Steam qui possède l'article.

Les commandes utilisent ces valeurs. Configurez-les une fois par fenêtre PowerShell, avec le nom de dossier de votre mod (comme dans `release.yml` `mod-folder`et sa première version :

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Réservez le mod

1. Mélange `beta` à entrer `main`ouvrir et fusionner un PR `main` à entrer `release` ([Les branches](../README.md#branches)).
2. Attendre le flux de travail de sortie `release`. Il s’agit de la fusion `<slug>-v<version>` Avec la version en `meitou.json` (par exemple à
`example-mod-v0.1.0`Il crée une libération avec `<Mod>-<version>.zip` et `<Mod>-<version>.zip.sha256`. Si ce n’est pas le cas avant de se faire passer, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` Il faut que la main soit enlevé (`-DryRun` Il n’y a que des commandes).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Installez Workshop Watcher et connectez-vous à Steam

Suivez [Installation](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) Dans le cadre du Workshop Watcher. L'installateur télécharge SteamCMD `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` Vous devez vous inscrire sur votre compte Steam. Quand il demande `Add a mod to watch now?`La réponse `n`L’objet n’existe pas encore. Remplacer cette étape si Workshop Watcher est déjà installé.

<a id="3-prepare-the-items-files"></a>
## 3. Préparer les fichiers de l'article

1. Téléchargez la libération ZIP et le débarrasser dans `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` Maintenant, les `$Mod.dll`, `meitou.json` et `$Mod.mod`.
2. Réserver l'image de l'élément comme `$Work\preview.png`: PNG ou JPG, en dessous de 1 MB, carré (512 x 512 pixels fonctionnent).

<a id="4-write-the-vdf"></a>
## 4. Écrivez le VDF

CGF crée l’élément à partir d’un VDF fichier : un fichier de texte de `"key" "value"` Partie à l’intérieur d’un `"workshopitem"` Le bloc. Ce commandement écrit `$Work\create.vdf` Les chemins remplis. Édit `title` et `description` Tout d’abord :

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

| La clé | Valeur |
| --- | --- |
| `appid` | `233860`, KenshiL’application Steam ID. |
| `publishedfileid` | `0` Créer un nouveau élément. Tout autre numéro de mise à jour que l'élément existant. |
| `contentfolder` | La route complète vers le dossier dont les fichiers deviennent le contenu de l'élément: le dossier mod de la ZIPEt pas ses parents. |
| `previewfile` | Le chemin vers l’image prévisible. |
| `visibility` | `0` Le public, `1` Seuls les amis, `2` privée , `3` Non classé ! Commencez par `2` et le modifier sur la page de l'article. |
| `title` | Le nom de l'article sur le atelier. Commencez par `[Meitou]`Comme les autres modes de code Meitou. |
| `description` | Une ligne de texte. Écrivez ensuite la description complète de l'élément (il prend en charge Steam BBCode). |
| `changenote` | La première entrée de l'article dans la barre Change Notes. |

Règles pour les valeurs :

- Pour les trajets en avant (`C:/Users/me/Downloads/...`). Le coffre doit être doublé (`C:\\Users\\me\\...`).
- Une double citation à l’intérieur d’une valeur est `\"`.
- Gardez le fichier ASCII.

<a id="5-create-the-item"></a>
## 5. Créer l’élément

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` est le nom que vous inscrivez à Steam avec, comme donné à l'installateur. steamcmd utilise le login sauvé. Si vous demandez un mot de passe ou un code Steam, `powershell -ExecutionPolicy Bypass -File $Watcher login`Puis, le commandement est à nouveau effectué.

La course se termine avec `Success.`. Ouvrez Steam > votre profil > Articles de travail, ouvrez le nouveau élément et copiez le numéro après `?id=` Dans sa URLC’est le code d’identité.

<a id="6-finish-the-items-page"></a>
## 6. Fin de la page de l'article

Sur la page de l'article dans Steam :

1. Accepter le contrat légal de Steam si la page affiche le banner. Jusqu’à ce que vous le faites, vous ne pouvez voir que l’article.
2. Écrire la description complète.
3. Si le mod a besoin d'un autre mod de Workshops (par exemple Rivalry Core), ajoutez-le dans le **Add/Remove Les éléments nécessaires**. Les abonnés sont alors
Il a également demandé de s'abonner. Avec la dépendance `meitou.json` `dependencies`L'arrange du lanceur le met d'abord dans l'ordre du chargement.
4. Changer la visibilité à **Public** Lorsque la page est prête.

<a id="7-watch-the-item"></a>
## 7. Regardez l'article

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

La sortie rapporte la sortie actuelle comme déjà téléchargée. Supprimer `$Work` Une fois qu’il fait.

<a id="each-release-after-that"></a>
## Chaque libération après

Récupérer le mod comme dans la étape 1. La prochaine mise à jour de l’enregistreur téléchargera le ZIPVérifier son SHA-256 et le télécharge à l'élément, avec le titre de sortie et le lien comme note de changement. Il ne change jamais le titre, la description ou la visibilité de l'article : il change ceux qui sont sur la page de l'élément. `powershell -ExecutionPolicy Bypass -File $Watcher status` Afficher le calendrier, la prochaine course et le dernier upload.
