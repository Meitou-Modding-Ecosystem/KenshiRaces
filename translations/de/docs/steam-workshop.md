<a id="publishing-to-the-steam-workshop"></a>
# Veröffentlichung im Steam Workshop

Sie erstellen den Workshop-Artikel des Mods einmalig von Hand aus einer Veröffentlichung ZIP . Danach läuft [Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) auf Ihrem Windows PC und lädt nach einem Zeitplan jede neue GitHub-Version auf dieses Element hoch. Alle Schritte werden in **Windows PowerShell** auf Ihrem PC ausgeführt, angemeldet bei dem Steam-Konto, dem der Artikel gehört.

Die Befehle verwenden diese Werte. Legen Sie sie einmal pro PowerShell-Fenster fest, mit dem Ordnernamen Ihres Mods (wie in `release.yml`  `mod-folder` ) und seiner ersten Veröffentlichungsversion:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Geben Sie den Mod frei

1. Führen Sie `beta` in `main` zusammen, öffnen Sie dann ein PR und führen Sie es von `main` in `release` zusammen ([Zweige](../README.md#branches) ).
2. Warten Sie auf den Release-Workflow für `release` . Es markiert die Zusammenführung `<slug>-v<version>` mit der Version in `meitou.json` (z. B. `example-mod-v0.1.0` ) und erstellt eine Version mit `<Mod>-<version>.zip` und `<Mod>-<version>.zip.sha256` . Wenn dieser Lauf vor dem Markieren fehlschlägt, schiebt `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` das Tag manuell ( `-DryRun` druckt nur die Befehle).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Installieren Sie Workshop Watcher und melden Sie sich bei Steam an

Folgen Sie [Installieren Sie](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) im Workshop Watcher-Handbuch. Das Installationsprogramm lädt Steamcmd auf `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` herunter und meldet es bei Ihrem Steam-Konto an. Wenn Sie `Add a mod to watch now?` fragen, antworten Sie `n` : Das Element existiert noch nicht. Überspringen Sie diesen Schritt, wenn Workshop Watcher bereits installiert ist.

<a id="3-prepare-the-items-files"></a>
## 3. Bereiten Sie die Dateien des Artikels vor

1. Laden Sie die Version ZIP herunter und entpacken Sie sie in `$Work` :

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` enthält jetzt `$Mod.dll` , `meitou.json` und `$Mod.mod` .
2. Speichern Sie das Vorschaubild des Artikels als `$Work\preview.png` : PNG oder JPG , unter 1 MB , quadratisch (512 x 512 Pixel funktionieren).

<a id="4-write-the-vdf"></a>
## 4. Schreiben Sie VDF

Steamcmd erstellt das Element aus einer VDF -Datei: einer Textdatei mit `"key" "value"` -Paaren innerhalb eines `"workshopitem"` -Blocks. Dieser Befehl schreibt `$Work\create.vdf` mit ausgefüllten Pfaden. Bearbeiten Sie zuerst `title` und `description` :

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
| `appid` | `233860` , Kenshi s Steam-App ID . |
| `publishedfileid` | `0` erstellt ein neues Element. Jede andere Nummer aktualisiert das vorhandene Element. |
| `contentfolder` | Vollständiger Pfad zum Ordner, dessen Dateien zum Inhalt des Elements werden: der Mod-Ordner von ZIP , nicht sein übergeordneter Ordner. |
| `previewfile` | Vollständiger Pfad zum Vorschaubild. |
| `visibility` | `0` öffentlich, `1` nur für Freunde, `2` privat, `3` nicht gelistet. Beginnen Sie mit `2` und ändern Sie es auf der Artikelseite. |
| `title` | Der Name des Artikels in der Werkstatt. Starten Sie es mit `[Meitou]` , wie es andere Meitou -Code-Mods tun. |
| `description` | Eine Textzeile. Schreiben Sie anschließend die vollständige Beschreibung auf die Seite des Artikels (es unterstützt den BBCode von Steam). |
| `changenote` | Der erste Eintrag auf der Registerkarte „Änderungsnotizen“ des Elements. |

Regeln für Werte:

- Schreiben Sie Pfade mit Schrägstrichen ( `C:/Users/me/Downloads/...` ). Ein Backslash muss verdoppelt werden ( `C:\\Users\\me\\...` ).
- Ein doppeltes Anführungszeichen innerhalb eines Werts wird als `\"` geschrieben.
- Behalten Sie die Datei ASCII .

<a id="5-create-the-item"></a>
## 5. Erstellen Sie das Element

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` ist der Name, mit dem Sie sich bei Steam anmelden, wie er vom Installationsprogramm angegeben wurde. Steamcmd verwendet den gespeicherten Login. Wenn Sie nach einem Passwort oder Steam Guard-Code gefragt werden, führen Sie `powershell -ExecutionPolicy Bypass -File $Watcher login` aus und führen Sie den Befehl dann erneut aus.

Der Lauf endet mit `Success.` . Öffnen Sie Steam > Ihr Profil > Workshop-Artikel, öffnen Sie den neuen Artikel und kopieren Sie die Nummer nach `?id=` in sein URL : Das ist der Artikel ID .

<a id="6-finish-the-items-page"></a>
## 6. Beenden Sie die Seite des Artikels

Auf der Seite des Artikels in Steam:

1. Akzeptieren Sie die rechtliche Vereinbarung von Steam Workshop, wenn auf der Seite das Banner angezeigt wird. Bis dahin können nur Sie den Artikel sehen.
2. Schreiben Sie die vollständige Beschreibung.
3. Wenn der Mod einen weiteren Workshop-Mod benötigt (z. B. Rivalry Core), fügen Sie ihn unter **Erforderliche Elemente hinzufügen/entfernen** hinzu. Abonnenten werden dann gebeten, es ebenfalls zu abonnieren. Mit der Abhängigkeit in `meitou.json`  `dependencies` wird sie beim Arrange-Befehl des Launchers in der Ladereihenfolge an die erste Stelle gesetzt.
4. Ändern Sie die Sichtbarkeit auf **Öffentlich**, wenn die Seite fertig ist.

<a id="7-watch-the-item"></a>
## 7. Beobachten Sie den Artikel

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

Der Probelauf meldet die aktuelle Version als bereits hochgeladen. Entfernen Sie `$Work` , sobald dies der Fall ist.

<a id="each-release-after-that"></a>
## Jede Veröffentlichung danach

Geben Sie den Mod wie in Schritt 1 frei. Der nächste geplante Lauf des Beobachters lädt den ZIP herunter, überprüft seinen SHA -256 und lädt ihn mit dem Veröffentlichungstitel und dem Link als Änderungsnotiz in den Artikel hoch. Es ändert nie den Titel, die Beschreibung, die Vorschau oder die Sichtbarkeit des Artikels: Ändern Sie diese auf der Seite des Artikels. `powershell -ExecutionPolicy Bypass -File $Watcher status` zeigt den Zeitplan, den nächsten Lauf und den letzten Upload.
