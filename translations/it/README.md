# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · [Deutsch](../de/README.md) · [Español](../es/README.md) · Italiano · [Português do Brasil](../pt-BR/README.md) · [Русский](../ru/README.md) · [日本語](../ja/README.md) · [한국어](../ko/README.md) · [简体中文](../zh-CN/README.md)

Kenshi Races aggiunge tre razze giocabili all’editor dei personaggi di [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient). Leoma e Crevan hanno corpi maschili e femminili. I Crabfolk comprendono due varianti, `crabman` e `crabqueen`. Tutte usano gli scheletri umani del gioco. Il mod offre inoltre 59 animazioni per tutte le razze che condividono questi scheletri e un framework che permette ad altri mod di aggiungere razze e sottorazze proprie. GPL-3.0-or-later.

## Guide

- [Razze, record e prove in gioco](docs/kenshi-races.md)
- [Usare il framework in un altro mod e aggiungere sottorazze](docs/depending-on-kenshi-races.md)
- [Libreria di 59 animazioni e metodo di creazione](docs/animations.md)
- [Adattare un corpo GLB allo scheletro del gioco](docs/race-import.md)
- [Compilare e giocare con Visual Studio, VS Code o il terminale](docs/play.md)
- [Configurare e aggiornare i repository locali](docs/local-development.md)
- [Coordinare le modifiche tra repository](docs/agents-across-repos.md)
- [Verificare la compatibilità con le nuove versioni di Meitou](docs/refcheck.md)
- [Pubblicare su Steam Workshop](docs/steam-workshop.md)
- [Importare modelli GLB come edifici senza FCS](docs/structures.md)
- [Preparare geometria, dimensioni e texture](docs/model-prep.md)
- [Protezione dei branch e regole di revisione](docs/branch-policy.md)
- [Regole del repository e procedure per gli agenti](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## Identificatori condivisi

I mod caricati condividono la numerazione dei tipi di attività e dei codici di dialogo. Kenshi Races non riserva alcun numero. (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## Struttura del repository

- `engine/` — Sottomodulo MeitouClient fissato al commit usato per la compilazione. Aggiornalo con il comando seguente dopo ogni pull. [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — Progetto e manifesto del mod. I corpi adattati sono inclusi nel repository. I test usano dati sintetici e non richiedono il gioco.
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — I progetti facoltativi generano i dati del mod dal gioco installato o importano i modelli GLB come edifici durante la distribuzione. [StructureImport](docs/structures.md)
- `meitou.lock.json` — Fissa le versioni dei framework. Il workflow le aggiorna e unisce le modifiche quando la CI ha esito positivo. [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — Compilazione, distribuzione e avvio con un clic.
- `tools/` — Strumenti per sincronizzazione, verifica dei riferimenti al motore, importazione degli edifici, adattamento dei corpi e preparazione delle texture. [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — Importa le impostazioni del motore. Gli avvisi del compilatore sono trattati come errori.
- `Directory.Build.rsp` — Imposta i riferimenti alla versione del motore più vecchia supportata. Il gioco rifiuta i mod con riferimenti a un motore più recente. Il sottomodulo usa una versione predefinita per l’assembly. (`gameVersion`, `1.0.0.0`).  La compatibilità con le nuove versioni di Meitou viene verificata ogni giorno. [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## Branch

Le PR vanno su beta e devono superare compilazione e test. Dopo le prove in gioco, beta può essere promossa a main. La produzione passa per una PR su release. Ogni merge su release crea il tag di versione e pubblica lo ZIP, a meno che il tag esista già. Lo script di tagging permette di farlo manualmente. (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
