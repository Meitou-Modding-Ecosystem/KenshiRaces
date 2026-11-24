<a id="local-development-setup"></a>
# Lokale Entwicklungseinrichtung

Klonen Sie nur die Repos, an denen Sie arbeiten, in einen Ordner. Jeder baut eigenständig mit `dotnet build` auf, und die benötigten Frameworks laden sich selbst herunter und führen einen Hash-Check durch.

<a id="repos"></a>
## Repos

Jedes Mod-Repo ist nach seinem Mod benannt.

| Repo | Hält | PRs gehen zu |
| --- | --- | --- |
| MeitouClient | die Engine und ihre Mod-Hooks | `fork` |
| RivalryCore | Rivalry Core Framework und seine Agentenfähigkeiten | `beta` |
| LogisticsCore | Logistik-Kernrahmen | `beta` |
| Kriegsorchestrierung | War Orchestration-Framework (verwendet Logistics Core) | `beta` |
| BastFront | Bast Front-Nahkampf (verwendet Kriegsorchestrierung und Logistikkern) | `beta` |
| AshenHand | Die Ashen Hand (verwendet Rivalry Core) | `beta` |
| Tierhaltung | Tierhaltung | `beta` |
| SquadArena | SquadArena-Mod und das Squad-Bewertungstool | `beta` |
| GutenStructure (TheIvoryDisaster) | Shared CI , Verpackung, Release-Gate, Framework-Sperrziel | `beta` |

<a id="folders"></a>
## Ordner

Diese Schritte verwenden acht Ordner. Legen Sie sie in jedem neuen PowerShell-Fenster als Variablen fest und fügen Sie dann die folgenden Befehle so ein, wie sie sind:

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
## Voraussetzungen

- . NET 11 SDK (der `global.json` jedes Repos benennt die Version) und Git.
- Ein Editor: Visual Studio , VS Code mit dem C# Dev Kit oder Rider. Ein Terminal allein funktioniert auch.
- Kenshi installiert. Wenn es sich nicht in `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` befindet, legen Sie die Umgebungsvariable `KENSHI_PATH` auf seinen Ordner fest.

<a id="clone"></a>
## Klon

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Jedes Mod-Repo verfügt über ein eigenes `engine` -Submodul, sodass `$Engine` nur zum Schreiben von Engine-Hooks benötigt wird.

<a id="build-and-play-one-mod"></a>
## Erstellen und spielen Sie einen Mod

**Änderung zum Repo des Mods:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- Der erste Build lädt jedes in `meitou.lock.json` genannte Framework in `deps` herunter und stoppt, wenn sein SHA -256 nicht übereinstimmt.
- `dotnet run --project Play` kopiert die gesperrten Frameworks und den Mod in `Kenshi\mods` und startet die Engine. Visual Studio , VS Code und der installierte Launcher: [play.md](play.md) .
- Aktivieren Sie die Frameworks über dem Mod im Launcher, wie für jede Abhängigkeit.

<a id="change-a-framework-and-a-mod-together"></a>
## Ändern Sie ein Framework und einen Mod gemeinsam

Richten Sie das Mod-Repo auf Ihren Framework-Klon:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Builds in `$Ashen` kompilieren jetzt Rivalry Core aus `$Rivalry` und verwenden die Engine in `$Rivalry\engine` , und Play stellt diesen Rivalry Core-Build bereit.
- `Meitou.user.props` wird von Git ignoriert. Wenn es bereits Ihre Kenshi -Einstellungen enthält, fügen Sie stattdessen die beiden Zeilen in seinem `PropertyGroup` hinzu.
- `MeitouFrameworkSources` verwendet Framework-Projektpfade, die durch `;` getrennt sind, z. B. `$Logistics\LogisticsCore\LogisticsCore.csproj` . `MeitouEngineDir` benennt die Engine, auf der der Framework-Klon aufbaut.
- Löschen Sie die Zeile, um zum gesperrten ZIP zurückzukehren.

**Framework-Ketten.** `meitou.lock.json` von Bast Front listet War Orchestration und Logistics Core auf. Um beide Frameworks und Bast Front gemeinsam zu ändern, überschreiben Sie beide in Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

Die Überschreibung wird weitergegeben, sodass War Orchestration auch gegen `$Logistics` erstellt wird und der gesamte Build eine Engine verwendet. Überschreiben Sie nur War Orchestration und bauen Sie es gegen den Logistikkern auf, der im Schloss von Bast Front genannt wird.

Um es zu versenden: Führen Sie das Framework PR mit `beta` von `$Rivalry` zusammen, wodurch eine Vorabversion `rivalry-core-<version>-beta.<commit>` veröffentlicht wird. Setzen Sie dann in `$Ashen`  `meitou.lock.json` auf dieses Tag und das Asset SHA -256 und den Engine-Commit aus den Versionshinweisen und öffnen Sie den Mod PR . In einer Kette von oben nach unten versenden: Logistics Core, dann ein War Orchestration-Build, der ihn verriegelt, und dann Bast Front, der beide verriegelt. Verschieben Sie die Sperre nach der Produktionsfreigabe des Frameworks auf `rivalry-core-v<version>` .

<a id="change-the-engine-and-a-mod-together"></a>
## Ändern Sie die Engine und einen Mod zusammen

Schauen Sie sich Ihren Engine-Zweig im Submodul des Mod-Repos an:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Übernehmen Sie den neuen Engine-Zeiger nicht: Engine-Pins werden nur zu Upstream-Release-Tags verschoben, sodass die Änderung dieses Repo erreicht, sobald der Upstream sie veröffentlicht.
- Ein Mod, der ein Framework verwendet, muss auch das Framework auf dieser Engine neu erstellen. Verwenden Sie die obige Framework-Überschreibung und sehen Sie sich denselben Engine-Zweig in `$Rivalry\engine` an.
- Befehl zur Landung: Motor PR in `fork` ; Nachdem der Upstream es veröffentlicht hat, verschiebt das Framework PR  `$Rivalry\engine` auf diese Version, dann verschiebt der Mod PR seine Sperre und `engine` .

<a id="switch-the-engine-to-play-or-test"></a>
## Schalten Sie die Engine zum Spielen oder Testen ein

Um den neuesten Upstream, eine Veröffentlichung oder einen Engine-Zweig abzuspielen, tauschen Sie Ihren `$Engine` -Klon mit dem Skript in einem beliebigen Mod-Repo aus:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : das neueste Commit von VlcekM/ MeitouClient  `master` (das Skript fügt die `upstream` -Remote hinzu, wenn sie fehlt).
- `-To Release` : das neueste Upstream-Release-Tag `vX.Y.Z` .
- `-To Fork` : Der `fork` -Zweig des Org-Forks, gezogen.
- `-Branch <name>` : ein Zweig der Organisationsgabel, z. B. der Zweig einer Engine von PR .
- Ohne `-Path` wird der Ordner `MeitouClient` neben dem Mod-Repo verwendet. `-Run` erstellt das Spiel in Release und startet es; `-Build` baut es nur.
- Upstream, Release und `-Branch` verlassen den Klon bei einem getrennten Commit; Die lokalen Zweige `master` und `fork` werden nicht geändert. Das Skript stoppt ohne Änderungen, wenn der Klon nicht festgeschriebene Änderungen aufweist.
- Es berührt niemals das `engine` -Submodul des Mod-Repos (das heißt `Update-Local.ps1` und die Pins).

<a id="keep-clones-current"></a>
## Halten Sie die Klone aktuell

Nach jedem Pull und immer dann, wenn ein Build aufgrund fehlender Engine-Typen fehlschlägt (z. B. `The type or namespace name 'RelationCause' could not be found` ), schließen Sie Visual Studio und führen Sie das Update-Skript im Repo des Mods aus:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Es wechselt zu `beta` und zieht es, aktualisiert dann `engine` und `build/gutenstructure` auf die Commits-Pins `beta` (nur `git pull` belässt sie auf den alten). `-Branch <name>` zieht einen weiteren Zweig.
- Es prüft jedes Submodul anhand seines Pins und gibt die Engine-Version aus.
- Es markiert `MeitouEngineDir` und `MeitouFrameworkSources` in `Meitou.user.props` oder der Umgebung: Sie sorgen dafür, dass der Build die Engine oder das Framework eines anderen Repos anstelle der Pins dieses Repos verwendet. Löschen Sie sie, es sei denn, Sie ändern gleichzeitig ein Framework und einen Mod.
- `-Clean` löscht die von Git ignorierten Ordner `bin` und `obj` , sodass kein Build der alten Engine wiederverwendet wird. `-Build` führt `dotnet build` aus.
- Es endet mit `Up to date` oder einer Liste von Zeilen mit der Bezeichnung `FIX` .

Der nächste Build lädt ein Framework nur dann erneut herunter, wenn sich `meitou.lock.json` geändert hat.
