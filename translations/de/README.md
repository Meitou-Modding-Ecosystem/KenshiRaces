<a id="kenshi-races"></a>
# Kenshi Rennen

Neue Rennen für [MeitouKlient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) auf den menschlichen Skeletons des Spiels, spielbar im Charakter-Editor: Leoma und Crevan (Männer und Frauen) und die Crabfolk (Crabman, Crabqueen), eine Animationsbibliothek von 59 für jede menschliche Rasse, und die anderen Mods fügen ihre eigenen Rassen und Subrasse mit. GPL3.0 oder später.

- Die Rennen, ihre Aufzeichnungen und testen sie im Spiel: [docs/kenshi-races.md](docs/kenshi-races.md).
- Verwenden Sie diese Rennen oder fügen Sie Ihre aus einem anderen Mod hinzu: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) Agent Skills
`.claude/skills/kenshi-races-dependent`).
- Die Animationsbibliothek (Zeit (59 Clips für jede Rasse auf den menschlichen Scheletons) und wie es gemacht wurde: [docs/animations.md](docs/animations.md).
- Fügen Sie ein Rygged `.glb` Der Körper (Meshy, Hi3D "3ds Max Biped") auf das Skeleton des Spiels: [docs/race-import.md](docs/race-import.md).

- Bauen und laufen Sie lokal mit Visual Studio, VS Code oder dem Terminal (dieses Repo-Motor oder der installierte Luncher, das für das Spiel eingesetzt wird): [docs/play.md](docs/play.md).
- Die Ruhe auf Ihrer Maschine setzen: [docs/local-development.md](docs/local-development.md).
- Einführen von Agenten über eine Änderung, die einen Ruhe erhöht: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- Agentregeln für den Motor, Versionen, CI und Releases: [CLAUDE.md](../../CLAUDE.md) (So wie `AGENTS.md`Schritt für Schritt Verfahren:
`.claude/skills/meitou-engine-release`.
- Überprüfen Sie ein eingebautes Mod gegen die neueste Meitou-Ausgabe (CI macht es auf jeder PR): [docs/refcheck.md](docs/refcheck.md).
- Das Publikum des Steam-Works zum ersten Mal, dann auf jeder Release: [docs/steam-workshop.md](docs/steam-workshop.md).
- Umkehr `.glb` Die Modelle (Meshy, Hi3D, Blender) in aufbauliche Strukturen, nicht FCS nötig ist: [docs/structures.md](docs/structures.md);
Erhalten Sie die Polycount, Größe und Texturen eines generierten Modells erstmals: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## Zahlen

Die Aufgaben (`AddGoalType`, `AddJobType`und Dialogkodex (`AddDialogueCondition`, `AddDialogueEffect`Sie werden von jedem Laden ([Mod Nummer Blöcke](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi Rassen behaupten nicht.

| Zahl | Pool | Gebrauch |
| --- | --- | --- |

<a id="layout"></a>
## Layout

- `engine/`: MeitouClient Als Git-Submodul wird der Motor mit dem Mod gegensetzt. `git pull` nicht bewegen: nach dem
Jeder Pull Run `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([Klonen aktualisieren](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`• Das Mod-Projekt und `meitou.json`; `KenshiRaces/items/races/`: die eingesetzten Organe (verpflichtetes Einfuhrprodukt).
`KenshiRaces.Tests/`: seine Tests (synthetische Daten, keine Spielinstallation erforderlich). `<Mod>.Data/` (Optional): ein Generator, der schreibt `<Mod>.mod` Von dem installierten Spiel. `<Mod>/Structures/` (Optionell für den Tag) `.glb` Modelle, die als baulicher Strukturen im Einsatz eingeführt werden ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`: das Framework veröffentlicht die Mods gegen
([Framework Locker](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)Der Framework-Update-Workfluss bewegt es auf neue Releases und vereint, wenn CI passiert ([Framework Aktualisierung](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`Einklick auf Bau, Verwendung und Durchführung.
- `tools/`: `Update-Local.ps1`, der lokalen pull und engine-pin check ([Klonen aktualisieren](docs/local-development.md#keep-clones-current));
`refcheck.sh` und `RefCheck/`, die Motorenreferenzprüfung ([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`, das Modell importiert ([docs/structures.md](docs/structures.md)); `RaceImport/`und das Körper ([docs/race-import.md](docs/race-import.md)); `model-info.py` und `prepare-model.py`, Modellprüfungen und Textur abschreiben ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` Importiert die Motoren, so dass Mods mit den Einstellungen des Spiels bauen (Warnungen sind Fehler).
- `Directory.Build.rsp` Zeichen Sie die Motoren-Referenzen, da die älteste Meitou die Mods-Unterstützung veröffentlicht, gleich jedem `gameVersion` Untergrund: Die
Das Spiel verweigert einen Mod, der einen neuen Motorrät bezieht, und das Submodul baut `1.0.0.0` ([Versionen](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). Der Meitou-Kompatibilitätswerkfluss wird die Mods täglich gegen neue Meitou-Ausgabe überprüfen ([Meitou Kompatibilität](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## Branchen

PRs in `beta`CI (Konstruktion und Test) muss durchgeführt werden. `beta` Es wird gefördert, `main` Einmal im Spiel getestet, und die Produktion geht durch eine PR `release`. Jeder Miteinander `release` tags es `<slug>-v<version>` und veröffentlicht die Mod's Zip (`.github/workflows/release.yml`(a) es sei denn, dass diese Zeichnung vorhanden ist; `tools\Tag-Release.ps1` Das ist der manuelle Fallback.

- Neue Repositorien-Schutz und dauerhaft beta/main/release Bewertungen zu: [docs/branch-policy.md](docs/branch-policy.md). Template kopieren Quelle; GutenStructure verwendet Einstellungen durch Versöhnung.
