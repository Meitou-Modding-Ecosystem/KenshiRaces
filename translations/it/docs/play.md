<a id="build-and-run-locally"></a>
# Costruire e eseguire localmente

La costruzione `Play` costruisce il motore e ogni mod in questo repo, copia i mod e i frame `meitou.lock.json` I nomi vengono inseriti nella cartella dei mod e poi il gioco inizia. Utilizzare qualsiasi strumento che ti piaccia: Visual Studio, VS Codice o un terminale che esegue lo stesso.

Tutti e tre hanno bisogno.NET 11 SDK e un clone con i suoi submoduli (`git clone --recurse-submodules <repo URL>`o `git submodule update --init --recursive` di un clone esistente). Senza di essi, la costruzione si ferma `MEITOU001`Chiamare questo comando.

<a id="visual-studio"></a>
## Studio visivo

1. Apri il repo `.slnx`.
2. Il set `Play` Come programma di avvio, se Visual Studio non lo ha selezionato (clicca a destra > Imposta come programma di avviazione).
3. Selezionare un profilo nella scheda a basso accanto al pulsante Start, quindi premere F5 (debug) o Ctrl+F5:

| Il profilo | corsa | Utilizzare per |
| --- | --- | --- |
| Gioca (Motor + Mods) | `MeitouClient.exe` Costruito da questo repo `engine` Il submodulo | testare e debuggere il mod; i punti di partenza nel lavoro di codice mod |
| Il lanciatore Meitou (installazione) | Il lanciatore installato dal Meitou rilascia, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | cambiare l'elenco di mod o farlo caricare e fidarsi di un nuovo codice di mod |

<a id="vs-code"></a>
## contro il codice

1. Installare l'estensione C# Dev Kit (VS Code lo offre quando si apre la cartella; è in `.vscode/extensions.json`).
2. Apri la cartella root del repo (File > Open Folder).
3. Eseguire e disattivare (Ctrl+Shift+D) **Gioca (Motor + Mods)**La pressione F5. Si costruisce e si disegna prima, poi si avvia il motore sotto il
Debugger: i punti di partenza nel mod code work.
4. Per il lanciatore installato: Terminal > Run Task > **Il lanciatore Meitou (installazione)**. Costruisce e distribuisce, poi apre il lanciatore.

Terminal > Run Build Task (Ctrl+Shift+B) viene utilizzato solo per la costruzione e l’installazione.

<a id="without-an-editor"></a>
## Senza editore

Clicca destra `tools\Build.ps1` > **Con PowerShell**. Costruisce e distribuisce come `dotnet build` (controllare i sottomoduli prima se mancano) e mantenere la finestra aperta in modo da poter leggere il risultato. `-Test` eseguire anche i test; `-Run` Poi si avvia il motore.

<a id="terminal"></a>
## Terminali

Nella cartella root del repo:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` e `dotnet test` Costruire e testare ogni progetto.
- `dotnet run --project Play` Il motore viene costruito, installato e avviato (il profilo "Play (Motor + Mods)").
- Per usare il lanciatore installato invece, sfruttare senza avviare il motore, poi aprire il lancio:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## Che cosa fa un gioco

- Trova i mod: ogni mappa di alto livello `meitou.json` e `<Folder>/<Folder>.csproj`. Un nuovo mod non ha bisogno di cambiamenti `Play`.
- Costruisce il motore e ogni mod in **Rilascio**, qualunque sia la configurazione che hai selezionato: il motore di debug costruisce il lancio in `TaskStart.CheckStarts`
Una volta che un mod inizia il compito di un personaggio. I punti di interruzione del codice di modalità stanno ancora funzionando (simboli di rilascio sono impostati).
- Copia ogni mod `<Mod>.dll`, `<Mod>.pdb` e `meitou.json` per `<Kenshi>\mods\<Mod>\`. Chiudere il gioco prima: un gioco in esecuzione chiude il dll.
- Copia ogni quadro in `meitou.lock.json` Dalla cartella scaricata alla `<Kenshi>\mods\<Framework>\`o, per un quadro costruito
di un clone ([local-development.md](local-development.md)e la sua produzione.
- Un mod con un generatore di dati (`<Mod>.Data/<Mod>.Data.csproj`Correndo come `<generator> <Kenshi folder> <output .mod>`e ottenere il suo `<Mod>.mod`
generato dal gioco installato; un fallimento è un avvertimento di costruzione e il mod viene implementato senza di esso. Ogni altro mod riceve un vuoto `<Mod>.mod` Quando non c’è nulla, un esistente non viene mai sostituito.
- Un mod con un `Structures` La cartella riceve la `.glb` i modelli importati nella sua cartella, i record messi in `.mod`
([structures.md](structures.md)).
- Quando la cartella di mod non esiste, nulla è copiato e la costruzione dice così.
- Il pulsante Play del lanciatore inizia con il rilascio che ha installato, non con il motore di questo repo. Chiudere e eseguire "Play (Motor + Mods)" per testare il vostro
costruire .

<a id="settings"></a>
## Impostazioni

Default: il gioco `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Il mods è `<game>\mods`. Cambiare con il `KENSHI_PATH` variabile ambientale (il motore lo legge anche, quindi entrambi sono d'accordo) o per macchina in un'immagine `Meitou.user.props` Nella cartella root del repo:

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

Il motore trova il gioco da solo (Steam); `KenshiDir` Inserisci solo in `Meitou.user.props` Non è passato a questo. La configurazione di lancio di VS Code funziona il motore di rilascio; e `.vscode/launch.json` locale, se si cambia `PlayConfiguration`.

<a id="first-run"></a>
## Prima corsa

- Eseguire il lanciatore installato una volta (qualsiasi delle modalità sopra): attivare il mod e i suoi quadri, e fidarsi del suo codice.
- Se la copia fallisce con "accesso rifiutato", concedi al tuo utente i diritti di modificare `Kenshi\mods` (Proprietà > Sicurezza) piuttosto che eseguire il vostro
Editore come amministratore.
