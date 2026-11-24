<a id="local-development-setup"></a>
# Il sistema di sviluppo locale

Clonare solo il riposo su cui si lavora in una cartella. Ognuno costruisce da solo con `dotnet build`E i frame che ha bisogno di scaricare e hash-check stessi.

<a id="repos"></a>
## riposo

Ogni mod repo viene nominato per il suo mod.

| riposo | Togliere | Il PRS va |
| --- | --- | --- |
| MeitouClient | Il motore e i suoi mod hooks | `fork` |
| Il rivalore | Rivalry Core framework e le sue abilità | `beta` |
| LogisticaCore | Il framework logistico | `beta` |
| WarArchestrazione | Strumento di War Orchestration (utilizzo di Logistics Core) | `beta` |
| BastFront | Bast Front Melee (utilizza il Sistema di Orchestrazione e Logistica di Guerra) | `beta` |
| di Ashen | La mano di Ashen (utilizzo di Rivalry Core) | `beta` |
| Il matrimonio | Il matrimonio | `beta` |
| Squadrone | SquadArena mod e lo strumento di rating squad | `beta` |
| La struttura (TheIvoryDisaster) | CI condiviso, imballaggio, porta di rilascio, target di blocco | `beta` |

<a id="folders"></a>
## I cartelloni

Questi passaggi utilizzano otto cartelle. Imposta come variabili in ogni nuova finestra di PowerShell, quindi aggiungi i comandi qui sotto come sono:

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

- .NET 11 SDK (Tutti i repo `global.json` Il nome della versione) e Git.
- Un editor: Visual Studio, VS Codice con il C# Dev Kit, o Rider. Anche un terminale funziona.
- Kenshi installato in Se non è in `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`E si impone la `KENSHI_PATH` L’ambiente è variabile
per il suo cartello.

<a id="clone"></a>
## Il clone

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Ogni mod repo ha la sua `engine` Il sottomodulo, così `$Engine` È necessario scrivere solo i motori.

<a id="build-and-play-one-mod"></a>
## Creare e giocare un mod

**Cambiare il mod per il repo:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- La prima costruzione scarica ogni quadro chiamato `meitou.lock.json` In entrata `deps` e si ferma se la sua SHA256 non è un caso.
- `dotnet run --project Play` copia i frame bloccati e il mod in `Kenshi\mods` E comincia il motore. Visual Studio vs. Codice
e il lanciatore installato: [play.md](play.md).
- Permettere i quadri sopra il mod nel lanciatore, come per qualsiasi dipendenza.

<a id="change-a-framework-and-a-mod-together"></a>
## Cambiare un framework e un mod insieme

Scopri il mod repo sul tuo clone quadro:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Costruito in `$Ashen` Rivalry Core da `$Rivalry` Utilizzare il motore in `$Rivalry\engine`, e Play rilascia che Rivalry Core costruisce.
- `Meitou.user.props` è ignorato. Se si è già tenuto il vostro Kenshi In questo modo, aggiungi le due linee all'interno `PropertyGroup` Piuttosto è
- `MeitouFrameworkSources` Il progetto si svolge in modo separato da `;`di E.G. `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` Nome del motore su cui si costruisce il clone quadro.
- Rimuovere la linea per tornare al blocco ZIP.

**Le catene quadro.** Bast di fronte `meitou.lock.json` La storia di War Orchestra e Logistics Core. Per cambiare entrambi i quadri e il Bast Front insieme, superare entrambi nel Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

Il superrido si scende, quindi l'orchestrazione di guerra si costruisce contro `$Logistics` Inoltre, l'intero edificio utilizza un motore. Sulla base di questa struttura si trova solo la War Orchestra e si costruisce contro il Logistics Core chiamato nel blocco di Bast Front.

Per inviarlo: unire il framework PR in `beta` di `$Rivalry`che ha pubblicato una preliminare `rivalry-core-<version>-beta.<commit>`. Poi, in `$Ashen`Il set `meitou.lock.json` per questo oggetto e per il suo patrimonio, SHA-256 e il motore si impegna dalle note di rilascio, e aprire il mod PR. In una catena, la nave è top-down: Logistics Core, poi un'orchestrazione costruito per bloccare, poi Bast Front bloccare entrambi. Dopo il rilascio della produzione, spostare il blocco `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Cambiare il motore e un mod insieme

Scopri il tuo ramo motore all'interno del sottomodulo del mod repo:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Non impegnarsi nel nuovo motore: i pini del motore si muovono solo alle etichette di rilascio, quindi il cambiamento raggiunge questo repo una volta
La rilascia.
- Un mod che utilizza un quadro ha anche bisogno del quadro ricostruito su quel motore. Utilizzare il framework sopra e controllare lo stesso
Il motore è in `$Rivalry\engine`.
- Ordine di atterrare: motore PR in `fork`Dopo la rilascio di Upstream, il framework PR si muove `$Rivalry\engine` A questo punto, la liberazione
Mod PR spostare il suo blocco e `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Spostare il motore per giocare o testare

Per giocare al più recente upstream, un rilascio o una filiale del motore, spostare il tuo `$Engine` Clone con lo script in qualsiasi mod repo:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`Il più recente impegno di VlcekM/MeitouClient `master` (Il script aggiunge il `upstream` a distanza quando mancano).
- `-To Release`Il nuovo tag di rilascio upstream `vX.Y.Z`.
- `-To Fork`L’org fork è `fork` Il ramo, il ramo.
- `-Branch <name>`Un ramo del forco, come un ramo del motore.
- senza `-Path` che utilizza il `MeitouClient` Il mod repo è il mod repo. `-Run` costruisce il gioco in Rilascio e lo inizia; `-Build` solo
Lo costruisce.
- Upstream, rilascio e `-Branch` lasciare il clone su un comando separato; il locale `master` e `fork` I rami non sono cambiati. Il script
Si ferma senza cambiare nulla quando il clone ha cambiamenti non commessi.
- Non tocca mai il mod repo `engine` Il sottomodulo (questo è `Update-Local.ps1` e i pini).

<a id="keep-clones-current"></a>
## Mantenere i cloni aggiornati

Dopo ogni ritiro, e ogni volta che una costruzione fallisce con i tipi di motore mancanti (ad es. `The type or namespace name 'RelationCause' could not be found`), chiudere Visual Studio e eseguire lo script di aggiornamento nel repo del mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Si sposta per `beta` E poi si aggiunge, poi si aggiorna. `engine` e `build/gutenstructure` Per gli impegni `beta` Il pino (`git pull` da soli
Lasciatelo sui vecchi). `-Branch <name>` Un altro ramo.
- Controlla ogni sottomodulo contro il suo pin e stampa la versione del motore.
- E’ una bandiera `MeitouEngineDir` e `MeitouFrameworkSources` In `Meitou.user.props` o l'ambiente: fanno usare l'edificio un altro
Il motore o il frame di repo invece di questo repo. Rimuovere se non si sta cambiando un framework e un mod insieme.
- `-Clean` Rimuovere il git-ignorato `bin` e `obj` I cartelli, quindi nessuna costruzione del vecchio motore è riutilizzato. `-Build` corsa `dotnet build`.
- Si conclude con `Up to date` Un elenco di linee segnalate `FIX`.

La prossima costruzione rimuove un quadro solo quando `meitou.lock.json` Cambiato è
