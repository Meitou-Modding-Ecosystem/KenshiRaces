<a id="build-and-run-locally"></a>
# Bauen und laufen Sie lokal

Das Gebäude `Play` Er baut den Motor und jedes Mod in diesem Repo, kopiert die Mods und die Frameworks `meitou.lock.json` Name in die Mods-Flagge des Spiels, und dann beginnt das Spiel. Verwenden Sie jedes Werkzeug, das Sie mögen: Visual Studio, VS Code oder ein Terminal, das alle die gleiche Struktur durchführt.

Alle drei brauchen das.NET 11 SDK und eine Klone mit seinen Submodulen (`git clone --recurse-submodules <repo URL>`oder `git submodule update --init --recursive` in einer vorhandenen Klone). Ohne sie wird die Baustelle aufgehört `MEITOU001`Nennen Sie diesen Befehl.

<a id="visual-studio"></a>
## Visual Studio

1. Öffnen Sie die Repo `.slnx`.
2. Set `Play` als Startup-Projekt, wenn Visual Studio es nicht ausgewählt hat (rechtsklicken > Set as Startup Project).
3. Wählen Sie ein Profil in der Schaltfläche neben dem Start-Button aus und drücken Sie dann F5 (Debug) oder Ctrl+F5:

| Profil | Rennen | Verwenden Sie es für |
| --- | --- | --- |
| Spielen (Motor + Mods) | `MeitouClient.exe` von diesem Repo errichtet `engine` Untermodule | Test und Debugging Ihres Mods; Schwankpunkte in Mod-Code-Arbeit |
| Meitou Launcher (Installiert) | Der Lüfter, der aus dem Meitou installiert wird, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | Ändern der Mod-Liste oder Ladenordnung und vertrauen an einen neuen Mod-Code |

<a id="vs-code"></a>
## gegen Code

1. Installieren Sie die C# Dev Kit-Erweiterung (VS Code bietet es, wenn Sie die Folie öffnen; es ist in `.vscode/extensions.json`).
2. Öffnen Sie die Rootmappe von repo (File > Open Folder).
3. Run und Debug (Ctrl + Shift + D) **Spielen (Motor + Mods)**Pressemitteilung F5. Er baut und verlegt zunächst, dann beginnt der Motor unter dem
Debugger: Abschnitten im Mod-Code.
4. Für den installierten Laufwerk: Terminal > Run Task > **Meitou Launcher (Installiert)**. Es baut und deploys, dann öffnet der Launcher.

Terminal > Run Build Task (Ctrl+Shift+B) erstellt und implementiert nur.

<a id="without-an-editor"></a>
## Ohne einen Redakteur

Rechtsklick `tools\Build.ps1` > **Lauf mit PowerShell**. Er baut und verlegt wie `dotnet build` (Sie überprüfen die Untermodule zuerst, wenn sie fehlen) und halten das Fenster offen, so dass Sie das Ergebnis lesen können. `-Test` auch die Tests durchführen; `-Run` Dann beginnt der Motor.

<a id="terminal"></a>
## Terminal

In der Rootmappe des RPO:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` und `dotnet test` Entwickeln und testen Sie jedes Projekt.
- `dotnet run --project Play` Er baut, setzt und startet den Motor (das "Play (Motor + Mods)" Profil).
- Um den installierten Launcher stattdessen zu verwenden, ohne den Motor zu starten, öffnen Sie dann den Launcher:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## Was ein Spiel baut

- Finden Sie die Mods: Jede Top-Level-Folder-Holdung `meitou.json` und `<Folder>/<Folder>.csproj`. Eine neue Modus braucht keine Veränderung `Play`.
- Erstellt den Motor und jeder Mod in **Release**, unabhängig von der Konfiguration, die Sie ausgewählt haben: Debug-Motor baut Schuss in `TaskStart.CheckStarts`
Sobald ein Mod die Aufgabe eines Charakters beginnt. Schnittpunkte in Mod-Code sind immer noch funktionierend (Release-Symbole werden implementiert).
- Kopieren Sie jeden Mod `<Mod>.dll`, `<Mod>.pdb` und `meitou.json` zu `<Kenshi>\mods\<Mod>\`. Schließen Sie das Spiel zuerst: Ein laufendes Spiel schließt die DLL.
- Kopieren Sie jedes Rahmen in `meitou.lock.json` Von der Download-Folde zu `<Kenshi>\mods\<Framework>\`oder für einen erstellten Rahmen
von einer Klone ([local-development.md](local-development.md)Ihre Produktion bauen.
- Ein Mod mit einem Datengenerator (`<Mod>.Data/<Mod>.Data.csproj`und run wie `<generator> <Kenshi folder> <output .mod>`Sie bekommt ihre `<Mod>.mod`
Erzeugte aus dem installierten Spiel; ein Fehler ist eine Buildwarnung und der Mod wird ohne ihn eingesetzt. Jeder andere Mod bekommt einen leeren `<Mod>.mod` Wenn es keinen gibt, wird ein bestehendes nie ersetzt.
- Ein Mod mit einem `Structures` Die Folge bekommt ihre `.glb` Modelle, die in ihre eingesetzte Ordnung eingeführt werden, werden in ihre `.mod`
([structures.md](structures.md)).
- Wenn die Mods-Folde nicht existiert, wird nichts kopiert und das Gebäude sagt das.
- Der Start-Button beginnt mit der Release, die er installiert hat, nicht mit dem Motor dieses Repos. Schließen Sie es und starten Sie "Play (Motor + Mods)" um Ihre
aufbauen .

<a id="settings"></a>
## Die Settings

Fehlgebnisse: Spiel `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Mods Folder `<game>\mods`. Ändern Sie sie mit dem `KENSHI_PATH` Umweltveränderung (der Motor liest es auch, so dass beide zustimmen), oder per Maschine in einem git-unbekannt `Meitou.user.props` In der roten Folder des RPO:

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

Der Motor findet das Spiel selbst (Steam); `KenshiDir` Nur in der `Meitou.user.props` Kein Übergang dazu. VS Code Start-Konfiguration läuft den Release-Motor; Es wird `.vscode/launch.json` Lokalerweise, wenn Sie sich ändern `PlayConfiguration`.

<a id="first-run"></a>
## Erster Rennen

- Führen Sie den installierten Laufwerk einmal (eine der oben genannten Wege): Aktivieren Sie den Mod und seine Rahmenbedingungen und vertrauen Sie auf den Code.
- Wenn die Kopie mit "Zugang verweigert" versagt, geben Sie Ihrem Benutzer Änderungsrechte auf `Kenshi\mods` (Eigenschaften > Sicherheit) anstatt Ihre
Der Redakteur als Administrator.
