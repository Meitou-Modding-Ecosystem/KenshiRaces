<a id="local-development-setup"></a>
# Lokale Entwicklungsinstellungen

Clone nur den Rest, auf dem du arbeitest, in eine Folge. Jeder baut auf eigene Faust `dotnet build`Und die Frameworks, die sie herunterladen und Hash-Check selbst.

<a id="repos"></a>
## Entspannung

Jeder Mod repo wird nach seinem Mod benannt.

| Repo | Halten | PRS geht auf |
| --- | --- | --- |
| MeitouClient | Der Motor und seine Mod Hooks | `fork` |
| RivalryCore | Rivalry Core-Framework und seine Agent-Fähigkeiten | `beta` |
| Logistikkore | Logistik Core | `beta` |
| KriegOrchestration | War Orchestration Framework (Verwendung von Logistik Kern) | `beta` |
| BastFront | Bast Front Melee (Verkauft Kriegs-Orchestration und Logistik-Kern) | `beta` |
| AshenHand | Die Hand der Ashen (siehe Rivalry Core) | `beta` |
| Ehefrau | Ehefrau | `beta` |
| SquadArena | SquadArena Mod und das Squad Rating-Tool | `beta` |
| Die Gastronomie (TheIvoryDisaster) | Gemeinsame CI, Verpackung, Release-Gate, Framework-Lock-Zweck | `beta` |

<a id="folders"></a>
## Folder

Diese Schritte verwenden acht Ordner. Setzen Sie sie als Variablen in jedem neuen PowerShell-Fenster, und geben Sie dann die kommenden Befehl wie sie sind:

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

- .NET 11 SDK (Ein jeder Repo `global.json` Der Name der Version) und Git.
- Ein Editor: Visual Studio, VS Code mit dem C# Dev Kit oder Rider. Auch ein Terminal funktioniert.
- Kenshi installiert ist. Wenn es nicht in `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Set die `KENSHI_PATH` Umweltveränderung
Zu seinem Folder.

<a id="clone"></a>
## Klone

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Jedes Mod Repo hat seine eigene `engine` Submodule und so `$Engine` Es ist nur notwendig, Motoren zu schreiben.

<a id="build-and-play-one-mod"></a>
## Erstellen und spielen Sie ein Mod

**Ändern Sie die Mods Repo:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- Der erste Bau-Downloads nennt jedes Framework `meitou.lock.json` In den `deps` und stoppt, wenn sie SHA256 ist nicht gleich.
- `dotnet run --project Play` Kopieren Sie die geschlossenen Frameworks und das Mod in `Kenshi\mods` Und dann beginnt der Motor. Visual Studio vs. Code
und der installierte Launcher: [play.md](play.md).
- Erlauben Sie den Rahmen über dem Mod im Laufenden, wie für jede Abhängigkeit.

<a id="change-a-framework-and-a-mod-together"></a>
## Ändern Sie ein Framework und ein Mod zusammen

Stellen Sie den Mod repo an Ihrem Framework Clone an:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Gebäude in `$Ashen` Rivalry Core aus dem `$Rivalry` und verwenden Sie den Motor in `$Rivalry\engine`, und Play enthält, dass Rivalry Core baut.
- `Meitou.user.props` Git wird ignoriert. Wenn sie bereits ihre Kenshi Die Einstellungen, fügen Sie die beiden Linien innerhalb `PropertyGroup` stattdessen.
- `MeitouFrameworkSources` die Entwicklungspläne, die von `;`und g. `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` Name des Motors, auf dem der Rahmenklon baut.
- Entfernen Sie die Linie, um zurück zu dem Lock ZIP.

**Framework Ketten** Bast Fronten `meitou.lock.json` Die Liste der Kriegs-Orchestration und der Logistik-Kern. Um beide Frameworks und Bast Front zusammenzustellen, übertreiben Sie beide in Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

Die Überride geht ab und so baut die Kriegsorchester gegen `$Logistics` Auch das ganze Gebäude nutzt einen einzigen Motor. Er überwindet nur die Kriegs-Orchestration und baut gegen die Logistik-Kern namens in Bast Front.

Um es zu versenden: das Rahmen PR in `beta` von `$Rivalry`, die eine Vorlage veröffentlicht `rivalry-core-<version>-beta.<commit>`. Und dann, in `$Ashen`und Set `meitou.lock.json` für diese Art und die Vermögenswerte, SHA-256 und der Motor verpflichtet sich von seinen Release-Noten, und öffnen Sie den Mod PR. In einer Kette, Schiff Top-down: Logistik-Kern, dann eine Kriegs-Orchestration bauen, dann Bast Front, beide. Nach der Freisetzung des Rahmenes bewegt sich der Lock auf `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Ändern Sie den Motor und einen Mod zusammen

Überprüfen Sie Ihren Motorenbereich innerhalb des Mod repo-Submoduls:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Lassen Sie sich nicht an den neuen Motorrücker wenden: Die Motorrückern bewegen sich nur auf die Release-Tags, so dass die Veränderung diese Repo einmal erreicht.
Er wird frei.
- Ein Mod, der ein Rahmen verwendet, benötigt auch das auf diesem Motor neu errichtete Rahmen. Verwenden Sie den Rahmen oben und überprüfen Sie das gleiche
Motorische Branchen in `$Rivalry\engine`.
- Befehl, es zu landen: Motoren PR in `fork`Nach dem Aufstieg wird das Rahmen PR bewegt `$Rivalry\engine` An dieser Freilassung, dann die
Mod PR bewegt seinen Lock und `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Schalten Sie den Motor um zu spielen oder zu testen

Um den neuesten Upstream, eine Release oder eine Motorabteilung zu spielen, wechseln Sie `$Engine` Clone mit dem Script in jedem Mod Repo:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`Das neueste Engagement von VlcekM/MeitouClient `master` (Der Script fügt den `upstream` Abstand, wenn es fehlt)
- `-To Release`Die neuesten Upstream-Release `vX.Y.Z`.
- `-To Fork`Der Org Fork `fork` Branchen, die sich ziehen
- `-Branch <name>`: eine Branche des Organs, wie eine Motor PR.
- ohne `-Path` Es nutzt die `MeitouClient` Die Folder neben dem Mod Repo. `-Run` das Spiel in Release aufbauen und es starten; `-Build` nur
Er baut es.
- Upstream, Release und `-Branch` die Klone auf einem abgeschlossenen Kompromiss verlassen; die `master` und `fork` Branchen sind nicht geändert. Das Script
Stopp, ohne etwas zu ändern, wenn die Klone unbeforderte Veränderungen hat.
- Es berührt niemals die Mod-Repo `engine` Submodule (das ist `Update-Local.ps1` und die Pins).

<a id="keep-clones-current"></a>
## Klonen aktualisieren

Nach jedem Treffer und jedes Mal, wenn ein Bau mit fehlenden Motortypen fehlt (z. B. `The type or namespace name 'RelationCause' could not be found`), schließen Sie Visual Studio und führen Sie das Update-Script in der Mod-Repo aus:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Es schaltet sich auf `beta` und zieht es, dann aktualisiert `engine` und `build/gutenstructure` Für die Verpflichtungen `beta` Pinsen (`git pull` allein
Sie lassen sich auf die Alten ab). `-Branch <name>` Ein anderes Glanz zieht.
- Es überprüft jeden Untermodul gegen seinen Pin und druckt die Motorversion.
- Das Flagge `MeitouEngineDir` und `MeitouFrameworkSources` In `Meitou.user.props` oder die Umwelt: sie machen die Bauanwendung ein anderes
repo's engine oder frame statt dieser repo'n pins. Entfernen Sie sie, es sei denn, Sie ändern ein Rahmen und ein Mod zusammen.
- `-Clean` Entfernen Sie das Git-Unvergessliche `bin` und `obj` Folders, so dass kein Bau des alten Motors neu verwendet wird. `-Build` Rennen `dotnet build`.
- Es endet mit `Up to date` oder eine Liste der markierten Linien `FIX`.

Der nächste Build herunterladen ein Framework nur, wenn `meitou.lock.json` Veränderte sich.
