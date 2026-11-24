<a id="build-and-run-locally"></a>
# Costruisci ed esegui localmente

Costruendo `Play` costruisci il motore e ogni mod in questo repository, copia i mod e i nomi dei framework `meitou.lock.json` nella cartella mods del gioco, quindi avvia il gioco. Usa lo strumento che preferisci: Visual Studio , VS Code o un terminale eseguono tutti la stessa build.

Tutti e tre hanno bisogno del . NET 11 SDK e un clone con i suoi sottomoduli ( `git clone --recurse-submodules <repo URL>` o `git submodule update --init --recursive` in un clone esistente). Senza di essi la compilazione si interrompe con `MEITOU001` , nominando quel comando.

<a id="visual-studio"></a>
## Visual Studio

1. Apri `.slnx` del repository.
2. Imposta `Play` come progetto di avvio se Visual Studio non lo ha selezionato (fai clic con il pulsante destro del mouse > Imposta come progetto di avvio).
3. Scegli un profilo nel menu a discesa accanto al pulsante Start, quindi premi F5 (debug) o Ctrl+F5:

| Profilo | Corre | Usalo per |
| --- | --- | --- |
| Gioca (motore + mod) | `MeitouClient.exe` creato dal sottomodulo `engine` di questo repository | testare ed eseguire il debug della tua mod; punti di interruzione nel lavoro del codice mod |
| Lanciatore Meitou (installato) | il launcher installato dalle versioni Meitou , `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | cambiare l'elenco delle mod o l'ordine di caricamento e fidarsi del codice di una nuova mod |

<a id="vs-code"></a>
## VS Code

1. Installa l'estensione C# Dev Kit ( VS Code la offre quando apri la cartella; è in `.vscode/extensions.json` ).
2. Apri la cartella principale del repository (File > Apri cartella).
3. Esegui ed esegui debug (Ctrl+Shift+D), seleziona **Gioca (motore + mod)**, premi F5. Innanzitutto viene creato e distribuito, quindi avvia il motore nel debugger; punti di interruzione nel lavoro del codice mod.
4. Per il launcher installato: Terminale > Esegui attività > ** Launcher Meitou (installato)**. Si crea e distribuisce, quindi apre il programma di avvio.

Terminale > Esegui attività di compilazione (Ctrl+Shift+B) crea e distribuisce solo.

<a id="without-an-editor"></a>
## Senza un editore

Fare clic con il pulsante destro del mouse su `tools\Build.ps1` > **Esegui con PowerShell**. Si costruisce e si distribuisce come `dotnet build` (controllando prima i sottomoduli se mancano) e mantiene la finestra aperta in modo da poter leggere il risultato.  Anche `-Test` esegue i test; `-Run` avvia quindi il motore.

<a id="terminal"></a>
## terminale

Nella cartella principale del repository:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` e `dotnet test` costruiscono e testano ogni progetto.
- `dotnet run --project Play` costruisce, distribuisce e avvia il motore (il profilo "Gioca (motore + mod)").
- Per utilizzare invece il launcher installato, esegui la distribuzione senza avviare il motore, quindi apri il launcher:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## Cosa fa una build di gioco

- Trova le mod: ogni cartella di livello superiore che contiene `meitou.json` e `<Folder>/<Folder>.csproj` . Una nuova mod non necessita di modifiche a `Play` .
- Costruisce il motore e ogni mod in **Release**, qualunque sia la configurazione scelta: le build del motore di debug vengono inserite in `TaskStart.CheckStarts` una volta che una mod avvia l'attività di un personaggio. I punti di interruzione nel codice mod funzionano ancora (i simboli di rilascio vengono distribuiti).
- Copia `<Mod>.dll` , `<Mod>.pdb` e `meitou.json` di ogni mod in `<Kenshi>\mods\<Mod>\` . Chiudi prima il gioco: un gioco in esecuzione blocca la dll.
- Copia ogni framework in `meitou.lock.json` dalla cartella di rilascio scaricata in `<Kenshi>\mods\<Framework>\` o, per un framework creato da un clone ([local-development.md](local-development.md) ), il suo output di compilazione.
- Una mod con un generatore di dati ( `<Mod>.Data/<Mod>.Data.csproj` , eseguito come `<generator> <Kenshi folder> <output .mod>` ) ottiene il suo `<Mod>.mod` generato dal gioco installato; un errore è un avviso di build e la mod viene distribuita senza di esso. Ogni altra mod ottiene uno `<Mod>.mod` vuoto quando non ne ha nessuno; uno esistente non viene mai sostituito.
- Una mod con una cartella `Structures` importa i suoi modelli `.glb` nella cartella distribuita, i record vengono uniti nel suo `.mod` ([structures.md](structures.md) ).
- Quando la cartella mods non esiste, non viene copiato nulla e la build lo dice.
- Il pulsante Riproduci del launcher avvia la versione installata, non il motore di questo repository. Chiudilo ed esegui "Gioca (motore + mod)" per testare la tua build.

<a id="settings"></a>
## Impostazioni

Impostazioni predefinite: gioco `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` , cartella mod `<game>\mods` . Modificali con la variabile di ambiente `KENSHI_PATH` (anche il motore la legge, quindi entrambi sono d'accordo) o per macchina in uno `Meitou.user.props` ignorato da git nella cartella principale del repository:

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

Il motore trova il gioco da solo (Steam); `KenshiDir` impostato solo in `Meitou.user.props` non gli viene passato.  La configurazione di lancio di VS Code esegue il motore di rilascio; modifica `.vscode/launch.json` localmente se cambi `PlayConfiguration` .

<a id="first-run"></a>
## Prima corsa

- Esegui il launcher installato una volta (in uno dei modi sopra indicati): abilita la mod e i suoi framework e fidati del suo codice.
- Se la copia fallisce con "accesso negato", concedi al tuo utente i diritti di modifica su `Kenshi\mods` (Proprietà > Sicurezza) anziché eseguire l'editor come amministratore.
