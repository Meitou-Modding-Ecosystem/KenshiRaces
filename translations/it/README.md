<a id="kenshi-races"></a>
# Kenshi Races

Nuove razze per [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) sugli scheletri umani del gioco, giocabili nell'editor dei personaggi: Leoma e Crevan (maschio e femmina) e Crabfolk (crabman, crabqueen), una libreria di animazioni di 59 clip per ogni razza di scheletri umani e la struttura con cui altri mod aggiungono le proprie razze e sottorazze. GPL -3.0 o successivo.

- Le gare, i loro record e i test nel gioco: [docs/kenshi-races.md](docs/kenshi-races.md) .
- Usando queste razze o aggiungendone di tue da un altro mod: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (abilità agente `.claude/skills/kenshi-races-dependent` ).
- La libreria di animazioni (59 clip per ogni razza sugli scheletri umani) e come è stata realizzata: [docs/animations.md](docs/animations.md) .
- Montaggio di un corpo `.glb` truccato ( Meshy , Hi3D "3ds Max Biped") sullo scheletro del gioco: [docs/race-import.md](docs/race-import.md) .

- Costruisci ed esegui localmente con Visual Studio , VS Code o il terminale (il motore di questo repository o il launcher installato, distribuito nel gioco): [docs/play.md](docs/play.md) .
- Configurazione dei repository sul tuo computer: [docs/local-development.md](docs/local-development.md) .
- Istruire gli agenti su una modifica che si estende ai repository: [docs/agents-across-repos.md](docs/agents-across-repos.md) .
- Regole dell'agente per pin del motore, versioni, CI e versioni: [ CLAUDE .md](../../CLAUDE.md) (come `AGENTS.md` ); procedure passo passo: `.claude/skills/meitou-engine-release` .
- Controllo di una mod creata rispetto alla versione Meitou più recente ( CI lo fa su ogni PR ): [docs/refcheck.md](docs/refcheck.md) .
- Pubblicazione della mod su Steam Workshop per la prima volta, quindi ad ogni versione: [docs/steam-workshop.md](docs/steam-workshop.md) .
- Trasformando i modelli `.glb` ( Meshy , Hi3D , Blender ) in strutture costruibili, non è necessario FCS : [docs/structures.md](docs/structures.md) ; ottenere prima il numero di poligoni, le dimensioni e le trame di un modello generato: [docs/model-prep.md](docs/model-prep.md) .

<a id="numbers"></a>
## Numeri

I tipi di attività ( `AddGoalType` , `AddJobType` ) e i codici di dialogo ( `AddDialogueCondition` , `AddDialogueEffect` ) sono condivisi da ogni mod caricato ([numero mod blocchi](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) ). Kenshi Races non ne afferma nessuno.

| Numero | Piscina | Utilizzo |
| --- | --- | --- |

<a id="layout"></a>
## Disposizione

- `engine/` : MeitouClient come sottomodulo git, bloccato sul motore su cui si impegna la mod build. `git pull` non lo sposta: dopo ogni pull esegui `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([mantieni cloni correnti](docs/local-development.md#keep-clones-current) ).
- `KenshiRaces/` : il progetto del mod e `meitou.json` ; `KenshiRaces/items/races/` : le carrozzerie allestite (produzione di importazione impegnata). `KenshiRaces.Tests/` : i suoi test (dati sintetici, non è necessaria l'installazione del gioco). `<Mod>.Data/` (opzionale): un generatore che scrive `<Mod>.mod` dal gioco installato. `<Mod>/Structures/` (opzionale): modelli `.glb` importati come strutture costruibili al momento della distribuzione ([docs/structures.md](docs/structures.md) ).
- `meitou.lock.json` : il framework rilascia le mod costruite contro ([framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ); il flusso di lavoro di aggiornamento del Framework lo sposta nelle nuove versioni e si unisce al passaggio di CI ([aggiornamento del framework](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ).
- `Play/` : creazione, distribuzione ed esecuzione con un clic.
- `tools/` : `Update-Local.ps1` , il controllo locale del pull e del pin del motore ([mantieni i cloni](docs/local-development.md#keep-clones-current) correnti); `refcheck.sh` e `RefCheck/` , il controllo dei riferimenti del motore ([docs/refcheck.md](docs/refcheck.md) ); `StructureImport/` , l'importazione del modello ([docs/structures.md](docs/structures.md) ); `RaceImport/` , l'allestimento ([docs/race-import.md](docs/race-import.md) ); `model-info.py` e `prepare-model.py` , controlli del modello e restringimento della texture ([docs/model-prep.md](docs/model-prep.md) ).
- `Directory.Build.props` importa i motori, quindi le mod vengono create con le impostazioni del gioco (gli avvisi sono errori).
- `Directory.Build.rsp` contrassegna i riferimenti al motore poiché la versione Meitou più vecchia supporta le mod, uguale a ciascun piano `gameVersion` : il gioco rifiuta una mod che fa riferimento a un timbro del motore più recente e il sottomodulo crea `1.0.0.0` ([versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ). Il flusso di lavoro di compatibilità Meitou controlla quotidianamente le modifiche rispetto alle nuove versioni di Meitou ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ).

<a id="branches"></a>
## Rami

PR in `beta` ; CI (costruzione e test) deve superare. `beta` viene promosso a `main` una volta testato in gioco e la produzione passa da PR a `release` . Ogni unione in `release` lo tagga `<slug>-v<version>` e pubblica lo zip della mod ( `.github/workflows/release.yml` ) a meno che quel tag non esista; `tools\Tag-Release.ps1` è il fallback manuale.

- Protezioni dei rami del nuovo repository e revisioni permanenti della versione beta/principale/release: [docs/branch-policy.md](docs/branch-policy.md) . I modelli copiano l'origine; GutenStructure applica le impostazioni tramite la riconciliazione.
