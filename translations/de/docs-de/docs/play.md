<a id="build-and-run-locally"></a>
# Lokal erstellen und ausführen

Beim Erstellen von `Play` werden die Engine und alle Mods in diesem Repo erstellt, die Mods und Frameworks `meitou.lock.json` -Namen in den Mods-Ordner des Spiels kopiert und dann das Spiel gestartet. Verwenden Sie ein beliebiges Tool: Visual Studio , VS Code oder ein Terminal, auf denen alle denselben Build ausführen.

Alle drei benötigen die . NET 11 SDK und ein Klon mit seinen Submodulen ( `git clone --recurse-submodules <repo URL>` oder `git submodule update --init --recursive` in einem vorhandenen Klon). Ohne sie stoppt der Build mit `MEITOU001` und benennt diesen Befehl.

<a id="visual-studio"></a>
## Visual Studio

1. Öffnen Sie `.slnx` des Repos.
2. Legen Sie `Play` als Startprojekt fest, wenn Visual Studio es nicht ausgewählt hat (Rechtsklick > Als Startprojekt festlegen).
3. Wählen Sie im Dropdown-Menü neben der Schaltfläche „Start“ ein Profil aus und drücken Sie dann F5 (Debug) oder Strg+F5:

| Profil | Läuft | Benutze es für |
| --- | --- | --- |
| Spielen (Engine + Mods) | `MeitouClient.exe` wurde aus dem `engine` -Submodul dieses Repos erstellt | Testen und Debuggen Ihres Mods; Haltepunkte in der Mod-Code-Arbeit |
| Meitou -Launcher (installiert) | der aus den Meitou -Versionen installierte Launcher, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | Ändern der Mod-Liste oder Ladereihenfolge und Vertrauen in den Code eines neuen Mods |

<a id="vs-code"></a>
## VS Code

1. Installieren Sie die C# Dev Kit-Erweiterung ( VS Code bietet sie an, wenn Sie den Ordner öffnen; sie befindet sich in `.vscode/extensions.json` ).
2. Öffnen Sie den Stammordner des Repos (Datei > Ordner öffnen).
3. Ausführen und Debuggen (Strg+Umschalt+D), wählen Sie **Play (Engine + Mods)** und drücken Sie F5. Es wird zuerst erstellt und bereitgestellt und dann die Engine unter dem Debugger gestartet. Haltepunkte in der Mod-Code-Arbeit.
4. Für den installierten Launcher: Terminal > Task ausführen > ** Meitou Launcher (installiert)**. Es wird erstellt und bereitgestellt und dann der Launcher geöffnet.

Terminal > Build-Aufgabe ausführen (Strg+Umschalt+B) erstellt nur Builds und stellt sie bereit.

<a id="without-an-editor"></a>
## Ohne Editor

Klicken Sie mit der rechten Maustaste auf `tools\Build.ps1` > **Mit PowerShell ausführen**. Es wird wie `dotnet build` erstellt und bereitgestellt (wobei zuerst die Submodule überprüft werden, wenn sie fehlen) und das Fenster geöffnet bleibt, damit Sie das Ergebnis lesen können. `-Test` führt auch die Tests durch; `-Run` startet dann den Motor.

<a id="terminal"></a>
## Terminal

Im Stammordner des Repos:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` und `dotnet test` erstellen und testen jedes Projekt.
- `dotnet run --project Play` erstellt, stellt die Engine bereit und startet sie (das Profil „Play (Engine + Mods)“).
- Um stattdessen den installierten Launcher zu verwenden, stellen Sie ihn bereit, ohne die Engine zu starten, und öffnen Sie dann den Launcher:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## Was ein Spielaufbau bewirkt

- Findet die Mods: jeden Ordner der obersten Ebene, der `meitou.json` und `<Folder>/<Folder>.csproj` enthält. Ein neuer Mod benötigt keine Änderung an `Play` .
- Erstellt die Engine und jeden Mod in **Release**, unabhängig von der gewählten Konfiguration: Debug-Engine-Builds werfen `TaskStart.CheckStarts` ein, sobald ein Mod die Aufgabe eines Charakters startet. Haltepunkte im Mod-Code funktionieren weiterhin (Release-Symbole werden bereitgestellt).
- Kopiert `<Mod>.dll` , `<Mod>.pdb` und `meitou.json` jedes Mods nach `<Kenshi>\mods\<Mod>\` . Schließen Sie zuerst das Spiel: Ein laufendes Spiel sperrt die DLL.
- Kopiert jedes Framework in `meitou.lock.json` aus seinem heruntergeladenen Release-Ordner nach `<Kenshi>\mods\<Framework>\` oder, für ein Framework, das aus einem Klon erstellt wurde ([local-development.md](local-development.md) ), seine Build-Ausgabe.
- Ein Mod mit einem Datengenerator ( `<Mod>.Data/<Mod>.Data.csproj` , ausgeführt als `<generator> <Kenshi folder> <output .mod>` ) erhält seinen `<Mod>.mod` aus dem installierten Spiel generiert; Ein Fehler ist eine Build-Warnung und der Mod wird ohne diese bereitgestellt. Jeder andere Mod erhält einen leeren `<Mod>.mod` , wenn er keinen hat; ein vorhandener wird niemals ersetzt.
- Bei einem Mod mit einem `Structures` -Ordner werden die `.glb` -Modelle in seinen bereitgestellten Ordner importiert und die Datensätze werden in seinem `.mod` ([structures.md](structures.md) ) zusammengeführt.
- Wenn der Mods-Ordner nicht vorhanden ist, wird nichts kopiert und der Build sagt dies.
- Die Play-Schaltfläche des Launchers startet die von ihm installierte Version, nicht die Engine dieses Repos. Schließen Sie es und führen Sie „Play (Engine + Mods)“ aus, um Ihren Build zu testen.

<a id="settings"></a>
## Einstellungen

Standardeinstellungen: Spiel `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` , Mods-Ordner `<game>\mods` . Ändern Sie sie mit der Umgebungsvariablen `KENSHI_PATH` (die Engine liest sie auch, also stimmen beide zu) oder pro Maschine in einem von Git ignorierten `Meitou.user.props` im Stammordner des Repos:

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

Die Engine findet das Spiel selbstständig (Steam); `KenshiDir` , das nur in `Meitou.user.props` festgelegt ist, wird nicht an diesen übergeben.  In der Startkonfiguration von VS Code wird die Release-Engine ausgeführt. Bearbeiten Sie `.vscode/launch.json` lokal, wenn Sie `PlayConfiguration` ändern.

<a id="first-run"></a>
## Erster Lauf

- Führen Sie den installierten Launcher einmal aus (auf eine der oben genannten Arten): Aktivieren Sie den Mod und seine Frameworks und vertrauen Sie seinem Code.
- Wenn der Kopiervorgang mit „Zugriff verweigert“ fehlschlägt, erteilen Sie Ihrem Benutzer Änderungsrechte für `Kenshi\mods` (Eigenschaften > Sicherheit), anstatt Ihren Editor als Administrator auszuführen.
