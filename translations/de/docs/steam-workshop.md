<a id="publishing-to-the-steam-workshop"></a>
# Veröffentlichung in der Steam Workshop

Sie erstellen das Workshop-Eintrag einmal, manuell, aus einer Release ZIP. Nach diesem, [Workshop mit Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) Es läuft auf Ihrem Windows-PC und laden Sie jede neue GitHub-Version zu diesem Element auf. Alle Schritte in **Windows PowerShell** auf Ihrem PC, unterschreiben Sie das Steam-Konto, das das Element besitzt.

Die Befehle verwenden diese Werte. Setzen Sie sie einmal pro PowerShell-Fenster, mit dem Ordnername Ihres Mods (wie in `release.yml` `mod-folder`und seine erste Veröffentlichung:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Lösen Sie die Mod

1. Mischung `beta` In den `main`Dann öffnen und vereinen Sie einen PR `main` In den `release` ([Branchen](../README.md#branches)).
2. Warten Sie auf den Release Workflow `release`. Es wird die Mündung `<slug>-v<version>` Mit der Version in `meitou.json` (z. B.
`example-mod-v0.1.0`und schafft eine Freilassung mit `<Mod>-<version>.zip` und `<Mod>-<version>.zip.sha256`. Wenn es vor dem Abschluß fällt, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` Sie drücken den Tag per Hand (`-DryRun` Nur die Befehle gedruckt).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Installieren Sie Workshop Watcher und loggen Sie sich an Steam

Folgen Sie [Installation](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) In der Workshop-Guide. Der Installer herunterlässt steamcmd `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` und unterschreiben Sie es in Ihr Steam-Konto. Wenn es fragt `Add a mod to watch now?`, Antworten `n`Das Element existiert noch nicht. Schalten Sie diesen Schritt ab, wenn Workshop Watcher bereits installiert ist.

<a id="3-prepare-the-items-files"></a>
## 3. Bereiten Sie die Dateien des Elements vor

1. Download der Release ZIP Und sie verpacken sich in `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` Jetzt hält `$Mod.dll`, `meitou.json` und `$Mod.mod`.
2. Speichern Sie das Vorbild des Objekts als `$Work\preview.png`: PNG oder JPG, unter 1 MB, Quadrat (512 x 512 Pixel arbeiten).

<a id="4-write-the-vdf"></a>
## 4. Schreiben Sie die VDF

Steamcmd schafft das Element aus einem VDF Datei: eine Textdatei von `"key" "value"` Paare innerhalb von A `"workshopitem"` Der Block. Dieser Befehl schreibt `$Work\create.vdf` Mit den gefüllten Wegen. Edit `title` und `description` Zuerst ist es:

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

| Schlüssel | Wert |
| --- | --- |
| `appid` | `233860`, KenshiDie Steam App ID. |
| `publishedfileid` | `0` Er schafft ein neues Element. Jede weitere Nummer-Updates, die vorhanden sind. |
| `contentfolder` | Vollweg zum Ordner, dessen Dateien in den Inhalt des Elements werden: die Mod-Folder aus dem ZIPNicht seine Eltern. |
| `previewfile` | Der vollständige Weg zum Vorbild. |
| `visibility` | `0` Die Öffentlichkeit, `1` Nur die Freunde, `2` Privat ist es, `3` Unlisted ist. Beginnen Sie mit `2` und ändern Sie es auf der Seite des Artikels. |
| `title` | Der Name des Artikels auf dem Workshop. Beginnen Sie es mit `[Meitou]`Wie andere Meitou-Code Mods tun. |
| `description` | Eine Linie des Textes. Schreiben Sie dann die vollständige Beschreibung auf der Seite des Elements (es unterstützt Steam BBCode). |
| `changenote` | Der erste Eingang des Artikels ist die Tab Änderungen. |

Regeln für Werte:

- Schriftgelehrten mit Vorderschlägen (`C:/Users/me/Downloads/...`). Ein Backslash muss verdoppelt werden (`C:\\Users\\me\\...`).
- Eine doppelte Zitate innerhalb eines Wertes wird geschrieben `\"`.
- Halten Sie die Datei ASCII.

<a id="5-create-the-item"></a>
## 5. Das Element erstellen

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` ist der Name, den Sie in Steam mit eingeben, wie dem Installator gegeben. steamcmd verwendet das gespeicherte Login. Wenn Sie ein Passwort oder Steam Guard-Code beantragen, führen Sie `powershell -ExecutionPolicy Bypass -File $Watcher login`Dann führt die Kommando wieder.

Der Rennen endet mit `Success.`. Öffnen Sie Steam > Ihr Profil > Workshop-Einheiten, öffnen Sie das neue Element und kopieren Sie die Nummer nach `?id=` In seiner URLDas ist das Element-ID.

<a id="6-finish-the-items-page"></a>
## 6. Finden Sie die Seite des Artikels

Auf der Seite des Artikels in Steam:

1. Akzeptieren Sie die Steam-Werkung, wenn die Seite den Banner zeigt. Bis Sie es tun, können Sie nur das Element sehen.
2. Schreiben Sie die volle Beschreibung.
3. Wenn das Mod eine weitere Workshop-Mod (z. B. Rivalry Core) benötigt, wird es unter **Add/Remove erforderliche Gegenstände**. Die Abonnenten sind dann
Sie haben auch gebeten, sich zu unterzeichnen. Mit der Abhängigkeit in `meitou.json` `dependencies`Das Laufwerk des Laufers legt es zunächst in die Ladegerät.
4. Wechseln Sie die Sichtbarkeit **öffentlich** Wenn die Seite fertig ist.

<a id="7-watch-the-item"></a>
## 7. Sehen Sie das Element

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

Der Trocknenlauf berichtet über die aktuelle Release, wie bereits hochgeladen ist. Entfernen `$Work` Sobald es tut.

<a id="each-release-after-that"></a>
## Jede Freilassung nach diesem

Lassen Sie den Mod wie im Schritt 1 veröffentlichen. Der nächste geplante Rennen des Wachers herunterlädt die ZIPÜberprüfen Sie Ihre SHA-256 und laden Sie es in das Element, mit dem Release-Titel und dem Link als Änderungsnot. Es ändert niemals den Titel, die Beschreibung, die Vorsicht oder die Sichtbarkeit des Artikels. `powershell -ExecutionPolicy Bypass -File $Watcher status` Sie zeigt den Zeitplan, die nächste Runde und die letzte Upgrade.
