<a id="kenshi-races"></a>
# Kenshi Race

Nuove corse per [di MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) su scheletri umani del gioco, giocabile nel personaggio editor: Leoma e Crevan (femminile e maschile) e il Crabfolk (crabman, crabqueen), una libreria di 59 clip per ogni razza umana, e il quadro di altri modi aggiunge le proprie razze e subrasioni. GPL3.0 o più tardi.

- Le corse, i loro record e testare in gioco: [docs/kenshi-races.md](docs/kenshi-races.md).
- Utilizzare queste corse o aggiungere il proprio da un altro mod: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (L'agente di competenza
`.claude/skills/kenshi-races-dependent`).
- La libreria di animazioni (cittadini (59 clip per ogni razza sulle schelette umane) e come è stata realizzata: [docs/animations.md](docs/animations.md).
- Adattare un rigido `.glb` Il corpo (Meshy, Hi3D "3ds Max Biped") sul scheletro del gioco: [docs/race-import.md](docs/race-import.md).

- Costruire e eseguire localmente con Visual Studio, VS Codice o il terminale (il motore di questo repo o il lanciatore installato, impostato nel gioco): [docs/play.md](docs/play.md).
- Imposta il riposo sulla tua macchina: [docs/local-development.md](docs/local-development.md).
- Inviare agenti su un cambiamento che spinge il riposo: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- Regole di agente per il pin, versioni, CI e rilasciamenti: [CLAUDE.md](../../CLAUDE.md) (Sì come `AGENTS.md`Procedure passo dopo passo:
`.claude/skills/meitou-engine-release`.
- Verificare un mod costruito contro il più recente rilascio di Meitou (CI lo fa su ogni PR): [docs/refcheck.md](docs/refcheck.md).
- Pubblicare il mod per il Steam Workshop per la prima volta, poi su ogni rilascio: [docs/steam-workshop.md](docs/steam-workshop.md).
- Il turno `.glb` I modelli (Meshy, Hi3D, Blender) in strutture costruibili, no FCS Necessario di: [docs/structures.md](docs/structures.md);
Ottenere la policonta, la dimensione e le tesi di un modello generato prima: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## Numeri

Tipi di attività (`AddGoalType`, `AddJobType`e i codici di dialogo (`AddDialogueCondition`, `AddDialogueEffect`(a) è condiviso da ogni mod di carico[Mod numero di blocchi](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi Le razze non affermano nulla.

| Numero | Piscina | Utilizzo |
| --- | --- | --- |

<a id="layout"></a>
## Il layout

- `engine/`: MeitouClient Come un submodulo git, impiccato al motore commette il mod costruisce contro. `git pull` Non si muove: dopo
Tutti i pull run `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([Mantenere i cloni attuali](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`Il progetto di mod e `meitou.json`; `KenshiRaces/items/races/`• I prodotti di importazione (committe import).
`KenshiRaces.Tests/`I suoi test (dati sintetici, nessuna installazione di gioco è necessaria). `<Mod>.Data/` (opzionale): un generatore che scrive `<Mod>.mod` Il gioco installato. `<Mod>/Structures/` (a seconda delle opzioni): `.glb` i modelli importati come strutture costruibili su misura di implementazione ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`Il framework rilascia i mods costruiti contro
([Il framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)Il flusso di lavoro dell'aggiornamento del quadro lo sposta a nuovi rilasci e si unisce quando CI passa ([Aggiornamento Framework](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`Un clic su Costruire, disporre e eseguire.
- `tools/`: `Update-Local.ps1`, il locale pull e il motore-pin check ([Mantenere i cloni attuali](docs/local-development.md#keep-clones-current));
`refcheck.sh` e `RefCheck/`, il motore di riferimento verifica ([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`Il modello di importazione ([docs/structures.md](docs/structures.md)); `RaceImport/`Il corpo è adatto ([docs/race-import.md](docs/race-import.md)); `model-info.py` e `prepare-model.py`, modelli di controllo e tessuto di raccolta ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` importa il motore, quindi i mod si costruiscono con le impostazioni del gioco (avvertenze sono errori).
- `Directory.Build.rsp` stampano i riferimenti del motore, e il più antico Meitou rilascia il supporto dei mods, pari a ciascuno `gameVersion` Il piano: il
Il gioco rifiuta un mod che si riferisce a un nuovo stampo motore, e il submodulo costruisce `1.0.0.0` ([Versione](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). Il flusso di lavoro di compatibilità di Meitou, e il modo di verificare i mods ogni giorno contro i nuovi rilasciamenti di Meitou ([di Meitou](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## I rami

PRS in arrivo `beta`CI (construzione e test) deve passare. `beta` è promosso per `main` Una volta testato nel gioco, e la produzione va attraverso un PR `release`. Tutti si uniscono `release` Tagliamo `<slug>-v<version>` e pubblicare il mod's zip (`.github/workflows/release.yml`a) a meno che non sia presente; `tools\Tag-Release.ps1` Il manuale è fallito.

- Protezione dei nuovi rami di ripristino e permanente beta/main/release Recensioni di Viviano: [docs/branch-policy.md](docs/branch-policy.md). Template di codice; GutenStructure applica le impostazioni attraverso la riconciliazione.
