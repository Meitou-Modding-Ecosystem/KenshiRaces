<a id="local-development-setup"></a>
# Configurazione dello sviluppo locale

Clona solo i repository su cui lavori in una cartella. Ciascuno si costruisce da solo con `dotnet build` e i framework di cui ha bisogno si scaricano e controllano autonomamente l'hash.

<a id="repos"></a>
## Repos

Ogni repository mod prende il nome dal suo mod.

| Repo | Tiene | I PR vanno a |
| --- | --- | --- |
| MeitouClient | il motore e i suoi ganci mod | `fork` |
| RivalitàCore | Framework Rivalry Core e relativa abilità dell'agente | `beta` |
| LogisticaCore | Quadro centrale della logistica | `beta` |
| WarOrchestration | Framework di orchestrazione della guerra (utilizza Logistics Core) | `beta` |
| BastFront | Corpo a corpo Bast Front (utilizza War Orchestration e Logistics Core) | `beta` |
| AshenHand | The Ashen Hand (usa Rivalry Core) | `beta` |
| Allevamento | Allevamento | `beta` |
| SquadArena | Mod SquadArena e strumento di valutazione della squadra | `beta` |
| GutenStructure (Il disastro dell'avorio) | CI condiviso, packaging, gate di rilascio, destinazione del blocco del framework | `beta` |

<a id="folders"></a>
## Cartelle

Questi passaggi utilizzano otto cartelle. Impostali come variabili in ogni nuova finestra di PowerShell, quindi incolla i comandi seguenti così come sono:

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
## Prerequisiti

- . NET 11 SDK ( `global.json` di ogni repository nomina la versione) e Git.
- Un editor: Visual Studio , VS Code con il kit di sviluppo C# o Rider. Funziona anche un solo terminale.
- Kenshi installato. Se non è in `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` , imposta la variabile di ambiente `KENSHI_PATH` nella sua cartella.

<a id="clone"></a>
## Clone

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Ogni repository mod ha il proprio sottomodulo `engine` , quindi `$Engine` è necessario solo per scrivere gli hook del motore.

<a id="build-and-play-one-mod"></a>
## Costruisci e gioca con una mod

**Cambia nel repository della mod:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- La prima build scarica ciascun framework denominato in `meitou.lock.json` in `deps` e si interrompe se il suo SHA -256 non corrisponde.
- `dotnet run --project Play` copia i framework bloccati e la mod in `Kenshi\mods` e avvia il motore. Visual Studio , VS Code e il launcher installato: [play.md](play.md) .
- Abilita i framework sopra la mod nel launcher, come per qualsiasi dipendenza.

<a id="change-a-framework-and-a-mod-together"></a>
## Cambia un framework e una mod insieme

Punta il repository mod sul tuo clone del framework:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Le build in `$Ashen` ora compilano Rivalry Core da `$Rivalry` e utilizzano il motore in `$Rivalry\engine` e Play distribuisce quella build Rivalry Core.
- `Meitou.user.props` è ignorato da Git. Se contiene già le impostazioni Kenshi , aggiungi invece le due righe all'interno del suo `PropertyGroup` .
- `MeitouFrameworkSources` prende percorsi di progetto quadro separati da `;` , ad es. `$Logistics\LogisticsCore\LogisticsCore.csproj` . `MeitouEngineDir` nomina il motore su cui si basa il clone del framework.
- Elimina la riga per tornare allo ZIP bloccato.

**Catene quadro.** `meitou.lock.json` di Bast Front elenca il nucleo logistico e di orchestrazione della guerra. Per modificare contemporaneamente sia le strutture che Bast Front, sovrascrivere entrambi in Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

L'override viene eliminato, quindi War Orchestration si basa anche su `$Logistics` e l'intera build utilizza un motore. Sostituisci solo l'orchestrazione di guerra e si basa sul nucleo logistico indicato nella serratura di Bast Front.

Per spedirlo: unire il framework PR in `beta` di `$Rivalry` , che pubblica un prerelease `rivalry-core-<version>-beta.<commit>` . Quindi, in `$Ashen` , imposta `meitou.lock.json` su quel tag e sull'asset, SHA -256 e il commit del motore dalle note di rilascio e apri la mod PR . In una catena, spedisci dall'alto verso il basso: Logistics Core, quindi una build War Orchestration che lo blocca, quindi Bast Front che blocca entrambi. Dopo il rilascio in produzione del framework, spostare il lucchetto su `rivalry-core-v<version>` .

<a id="change-the-engine-and-a-mod-together"></a>
## Cambia il motore e una mod insieme

Controlla il ramo del tuo motore all'interno del sottomodulo del repository mod:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Non impegnare il nuovo puntatore del motore: i pin del motore si spostano solo sui tag di rilascio upstream, quindi la modifica raggiunge questo repository una volta che upstream lo rilascia.
- Una mod che utilizza un framework necessita anche che il framework venga ricostruito su quel motore. Utilizza l'override del framework sopra e controlla lo stesso ramo del motore in `$Rivalry\engine` .
- Ordine di atterrare: motore PR in `fork` ; dopo che l'upstream lo ha rilasciato, il framework PR sposta `$Rivalry\engine` a quella versione, quindi il mod PR sposta il suo blocco e `engine` .

<a id="switch-the-engine-to-play-or-test"></a>
## Cambia il motore per riprodurre o testare

Per riprodurre l'ultimo upstream, una versione o un ramo del motore, cambia il tuo clone `$Engine` con lo script in qualsiasi repository mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : il commit più recente di VlcekM/ MeitouClient  `master` (lo script aggiunge il telecomando `upstream` quando manca).
- `-To Release` : il più recente tag di rilascio upstream `vX.Y.Z` .
- `-To Fork` : ramo `fork` del fork dell'org, estratto.
- `-Branch <name>` : un ramo del fork org, come il ramo di un motore PR .
- Senza `-Path` utilizza la cartella `MeitouClient` accanto al repository mod. `-Run` crea il gioco in Release e lo avvia; `-Build` lo costruisce solo.
- Upstream, Release e `-Branch` lasciano il clone su un commit distaccato; le filiali locali `master` e `fork` non vengono modificate. Lo script si interrompe senza modificare nulla quando il clone ha modifiche non salvate.
- Non tocca mai il sottomodulo `engine` del repository mod (ovvero `Update-Local.ps1` e i pin).

<a id="keep-clones-current"></a>
## Mantieni aggiornati i cloni

Dopo ogni pull e ogni volta che una build fallisce con tipi di motore mancanti (ad esempio `The type or namespace name 'RelationCause' could not be found` ), chiudi Visual Studio ed esegui lo script di aggiornamento nel repository del mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Passa a `beta` e lo estrae, quindi aggiorna `engine` e `build/gutenstructure` sui pin `beta` di commit (solo `git pull` li lascia su quelli vecchi). `-Branch <name>` tira un altro ramo.
- Controlla ciascun sottomodulo rispetto al suo pin e stampa la versione del motore.
- Contrassegna `MeitouEngineDir` e `MeitouFrameworkSources` in `Meitou.user.props` o nell'ambiente: fanno sì che la build utilizzi il motore o il framework di un altro repository invece dei pin di questo repository. Eliminali a meno che tu non stia modificando un framework e una mod insieme.
- `-Clean` elimina le cartelle `bin` e `obj` ignorate da git, quindi nessuna build del vecchio motore viene riutilizzata. `-Build` esegue `dotnet build` .
- Termina con `Up to date` o un elenco di righe contrassegnate con `FIX` .

La build successiva scarica nuovamente un framework solo quando `meitou.lock.json` è cambiato.
