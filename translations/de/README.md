<a id="kenshi-races"></a>
# Kenshi Races

Neue Rassen für [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) auf den menschlichen Skeletten des Spiels, spielbar im Charaktereditor: Leoma und Crevan (männlich und weiblich) und Crabfolk (Krabbenmensch, Krabbenkönigin), eine Animationsbibliothek mit 59 Clips für jede menschliche Skelettrasse und das Framework, mit dem andere Mods ihre eigenen Rassen und Unterrassen hinzufügen. GPL -3.0 oder höher.

- Die Rennen, ihre Rekorde und ihre Tests im Spiel: [docs/kenshi-races.md](docs/kenshi-races.md) .
- Verwenden Sie diese Rassen oder fügen Sie Ihre eigenen aus einem anderen Mod hinzu: [docs/dependent-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (Agentenfähigkeit `.claude/skills/kenshi-races-dependent` ).
- Die Animationsbibliothek (59 Clips für jede Rasse auf den menschlichen Skeletten) und wie sie erstellt wurde: [docs/animations.md](docs/animations.md) .
- Anbringen eines manipulierten `.glb` -Körpers ( Meshy , Hi3D „3ds Max Biped“) am Grundgerüst des Spiels: [docs/race-import.md](docs/race-import.md) .

- Erstellen und lokal ausführen mit Visual Studio , VS Code oder dem Terminal (der Engine dieses Repos oder dem installierten Launcher, der im Spiel bereitgestellt wird): [docs/play.md](docs/play.md) .
- Einrichten der Repos auf Ihrem Computer: [docs/local-development.md](docs/local-development.md) .
- Anweisungen für Agenten zu einer Änderung, die sich über Repos erstreckt: [docs/agents-across-repos.md](docs/agents-across-repos.md) .
- Agentenregeln für den Engine-Pin, Versionen, CI und Releases: [ CLAUDE .md](../../CLAUDE.md) (identisch mit `AGENTS.md` ); Schritt-für-Schritt-Anleitungen: `.claude/skills/meitou-engine-release` .
- Überprüfen eines erstellten Mods mit der neuesten Meitou -Version ( CI führt dies auf jedem PR durch): [docs/refcheck.md](docs/refcheck.md) .
- Veröffentlichen Sie den Mod zum ersten Mal im Steam Workshop, dann bei jeder Veröffentlichung: [docs/steam-workshop.md](docs/steam-workshop.md) .
- Um `.glb` -Modelle ( Meshy , Hi3D , Blender ) in baubare Strukturen umzuwandeln, ist kein FCS erforderlich: [docs/structures.md](docs/structures.md) ; Zuerst die Polyanzahl, Größe und Texturen eines generierten Modells richtig ermitteln: [docs/model-prep.md](docs/model-prep.md) .

<a id="numbers"></a>
## Zahlen

Aufgabentypen ( `AddGoalType` , `AddJobType` ) und Dialogcodes ( `AddDialogueCondition` , `AddDialogueEffect` ) werden von jedem geladenen Mod geteilt ([Mod-Nummernblöcke](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) ). Kenshi Races behauptet nichts.

| Nummer | Pool | Verwenden |
| --- | --- | --- |

<a id="layout"></a>
## Layout

- `engine/` : MeitouClient als Git-Submodul, angeheftet an den Engine-Commit, gegen den der Mod erstellt wird. `git pull` verschiebt es nicht: nach jedem Pull-Lauf `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([Klone aktuell halten](docs/local-development.md#keep-clones-current) ).
- `KenshiRaces/` : das Projekt des Mods und `meitou.json` ; `KenshiRaces/items/races/` : die angepassten Karosserien (zugesagte Importausgabe). `KenshiRaces.Tests/` : seine Tests (synthetische Daten, keine Spielinstallation erforderlich). `<Mod>.Data/` (optional): ein Generator, der `<Mod>.mod` aus dem installierten Spiel schreibt. `<Mod>/Structures/` (optional): `.glb` -Modelle, die bei der Bereitstellung als erstellbare Strukturen importiert werden ([docs/structures.md](docs/structures.md) ).
- `meitou.lock.json` : Das Framework gibt die Mods frei, gegen die gebaut wurde ([Framework-Sperre](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ); Der Framework-Update-Workflow verschiebt es auf neue Releases und führt sie zusammen, wenn CI erfolgreich ist ([Framework-Update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ).
- `Play/` : Erstellen, Bereitstellen und Ausführen mit einem Klick.
- `tools/` : `Update-Local.ps1` , die lokale Pull- und Engine-Pin-Prüfung ([Klone aktuell halten](docs/local-development.md#keep-clones-current) ); `refcheck.sh` und `RefCheck/` , die Engine-Referenzprüfung ([docs/refcheck.md](docs/refcheck.md) ); `StructureImport/` , der Modellimport ([docs/structures.md](docs/structures.md) ); `RaceImport/` , die Körperanpassung ([docs/race-import.md](docs/race-import.md) ); `model-info.py` und `prepare-model.py` , Modellprüfungen und Texturverkleinerung ([docs/model-prep.md](docs/model-prep.md) ).
- `Directory.Build.props` importiert die Engines, sodass Mods mit den Einstellungen des Spiels erstellt werden (Warnungen sind Fehler).
- `Directory.Build.rsp` stempelt die Engine-Referenzen als älteste Meitou -Version, die die Mods unterstützt, gleich jeder `gameVersion` -Etage: Das Spiel lehnt einen Mod ab, der auf einen neueren Engine-Stempel verweist, und das Submodul erstellt `1.0.0.0` ([Versionierung](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ). Der Meitou -Kompatibilitätsworkflow überprüft die Mods täglich mit neuen Meitou -Versionen ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ).

<a id="branches"></a>
## Zweige

PRs in `beta` ; CI (Build und Tests) muss bestehen. `beta` wird zum `main` hochgestuft, sobald es im Spiel getestet wurde, und die Produktion läuft über einen PR bis `release` . Jede Zusammenführung in `release` markiert sie mit `<slug>-v<version>` und veröffentlicht die ZIP-Datei des Mods ( `.github/workflows/release.yml` ), sofern dieses Tag nicht vorhanden ist. `tools\Tag-Release.ps1` ist der manuelle Fallback.

- Schutzmaßnahmen für neue Repository-Zweige und permanente Beta-/Haupt-/Release-Reviews: [docs/branch-policy.md](docs/branch-policy.md) . Kopierquelle für Vorlagen; GutenStructure wendet Einstellungen durch Abgleich an.
