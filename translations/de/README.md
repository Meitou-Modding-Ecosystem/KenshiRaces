# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · Deutsch · [Español](../es/README.md) · [Italiano](../it/README.md) · [Português do Brasil](../pt-BR/README.md) · [Русский](../ru/README.md) · [日本語](../ja/README.md) · [한국어](../ko/README.md) · [简体中文](../zh-CN/README.md)

Kenshi Races ergänzt den Charaktereditor von [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) um drei spielbare Völker. Für Leoma und Crevan gibt es männliche und weibliche Körper. Zu den Crabfolk gehören `crabman` und `crabqueen`. Alle verwenden die menschlichen Skelette des Spiels. Zum Mod gehören außerdem 59 Animationen für alle Völker mit diesen Skeletten und ein Framework, mit dem andere Mods eigene Völker und Unterarten hinzufügen können. GPL-3.0-or-later.

## Anleitungen

- [Völker, Datensätze und Tests im Spiel](docs/kenshi-races.md)
- [Das Framework in anderen Mods verwenden und Unterarten hinzufügen](docs/depending-on-kenshi-races.md)
- [Bibliothek mit 59 Animationen und ihre Erstellung](docs/animations.md)
- [Einen GLB-Körper an das Skelett des Spiels anpassen](docs/race-import.md)
- [Mit Visual Studio, VS Code oder dem Terminal bauen und spielen](docs/play.md)
- [Lokale Repositories einrichten und aktualisieren](docs/local-development.md)
- [Änderungen über mehrere Repositories koordinieren](docs/agents-across-repos.md)
- [Kompatibilität mit neuen Meitou-Versionen prüfen](docs/refcheck.md)
- [Im Steam Workshop veröffentlichen](docs/steam-workshop.md)
- [GLB-Modelle ohne FCS als Gebäude importieren](docs/structures.md)
- [Polygonzahl, Größe und Texturen vorbereiten](docs/model-prep.md)
- [Branchschutz und Review-Regeln](docs/branch-policy.md)
- [Repository-Regeln und Arbeitsabläufe für Agenten](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## Gemeinsame Kennnummern

Alle geladenen Mods teilen sich den Nummernraum für Aufgabentypen und Dialogcodes. Kenshi Races belegt keine Nummern. (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## Aufbau des Repositorys

- `engine/` — MeitouClient-Submodul auf dem zum Bauen verwendeten Commit. Nach jedem Pull mit dem folgenden Befehl aktualisieren. [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — Mod-Projekt und Manifest. Die angepassten Körper sind eingecheckt. Die Tests arbeiten mit synthetischen Daten und benötigen keine Spielinstallation.
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — Optionale Projekte erzeugen Mod-Daten aus der Spielinstallation oder importieren GLB-Modelle bei der Bereitstellung als Gebäude. [StructureImport](docs/structures.md)
- `meitou.lock.json` — Hält die verwendeten Framework-Versionen fest. Der Update-Workflow aktualisiert sie und führt die Änderung nach bestandener CI zusammen. [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — Bauen, bereitstellen und starten mit einem Klick.
- `tools/` — Werkzeuge für Synchronisierung, Prüfung der Engine-Verweise, Gebäudeimport, Körperanpassung und Texturaufbereitung. [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — Übernimmt die Einstellungen der Engine. Compilerwarnungen gelten als Fehler.
- `Directory.Build.rsp` — Setzt die Engine-Verweise auf die älteste unterstützte Version. Das Spiel lehnt Mods mit neueren Engine-Verweisen ab. Das Submodul verwendet eine Standard-Assemblyversion. (`gameVersion`, `1.0.0.0`).  Die Kompatibilitätsprüfung läuft täglich gegen neue Meitou-Releases. [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## Branches

PRs gehen an beta. Build und Tests müssen bestehen. Nach Tests im Spiel kann beta nach main übernommen werden. Die Produktion läuft per PR nach release. Jeder Merge nach release setzt den Versionstag und veröffentlicht das ZIP, sofern der Tag noch fehlt. Das Tag-Skript dient als manueller Ersatz. (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
