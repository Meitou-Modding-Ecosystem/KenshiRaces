<a id="publishing-to-the-steam-workshop"></a>
# Pubblicare su Steam Workshop

Crei l'oggetto Workshop del mod una volta, a mano, da una versione ZIP. Successivamente, [Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) viene eseguito su Windows PC e carica ogni nuova versione di GitHub su quell'elemento in base a una pianificazione. Tutti i passaggi vengono eseguiti in **Windows PowerShell**sul tuo PC, dopo aver effettuato l'accesso all'account Steam che sarà proprietario dell'oggetto.

I comandi utilizzano questi valori. Impostali una volta per finestra di PowerShell, con il nome della cartella della tua mod (come in `release.yml`  `mod-folder`) e la sua prima versione di rilascio:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Rilascia il mod

1. Unisci `beta` in `main`, quindi apri e unisci uno PR da `main` in `release` ([Rami](../README.md#branches)).
2. Attendi il flusso di lavoro di rilascio su `release`. Contrassegna l'unione `<slug>-v<version>` con la versione in `meitou.json` (ad esempio `example-mod-v0.1.0`) e crea una versione con `<Mod>-<version>.zip` e `<Mod>-<version>.zip.sha256`. Se l'esecuzione fallisce prima dell'etichettatura, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` inserisce manualmente l'etichetta (`-DryRun` stampa solo i comandi).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Installa Workshop Watcher e accedi a Steam

Seguire [Installa](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) nella guida Workshop Watcher. Il programma di installazione scarica steamcmd su `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` e accede al tuo account Steam. Quando chiede `Add a mod to watch now?`, rispondi `n` : l'oggetto non esiste ancora. Saltare questo passaggio se Workshop Watcher è già installato.

<a id="3-prepare-the-items-files"></a>
## 3. Preparare i file dell'articolo

1. Scarica la versione ZIP e scompattala in `$Work` :

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` ora detiene `$Mod.dll`, `meitou.json` e `$Mod.mod`.
2. Salva l'immagine di anteprima dell'elemento come `$Work\preview.png` : PNG o JPG, sotto 1 MB, quadrato (funziona 512 x 512 pixel).

<a id="4-write-the-vdf"></a>
## 4. Scrivere VDF

steamcmd crea l'elemento da un file VDF : un file di testo di coppie `"key" "value"` all'interno di un blocco `"workshopitem"`. Questo comando scrive `$Work\create.vdf` con i percorsi compilati. Modifica prima `title` e `description` :

```powershell
$Dir = $Work.Replace('\', '/')
@"
"workshopitem"
{
    "appid"           "233860"
    "publishedfileid" "0"
    "contentfolder"   "$Dir/$Mod"
    "previewfile"     "$Dir/preview.png"
    "visibility"      "2"
    "title"           "[Meitou] My Mod"
    "description"     "What the mod does. Requires MeitouClient."
    "changenote"      "$Mod $Version"
}
"@ | Set-Content "$Work\create.vdf" -Encoding ASCII
```

| Chiave | Valore |
| --- | --- |
| `appid` | `233860`, app Steam di Kenshi  ID. |
| `publishedfileid` | `0` crea un nuovo elemento. Qualsiasi altro numero aggiorna l'elemento esistente. |
| `contentfolder` | Percorso completo della cartella i cui file diventano il contenuto dell'elemento: la cartella mod dello ZIP, non il suo genitore. |
| `previewfile` | Percorso completo dell'immagine di anteprima. |
| `visibility` | `0` pubblico, `1` solo amici, `2` privato, `3` non in elenco. Inizia con `2` e modificalo nella pagina dell'articolo. |
| `title` | Il nome dell'oggetto sul Workshop. Avvialo con `[Meitou]`, come fanno gli altri mod del codice Meitou. |
| `description` | Una riga di testo. Successivamente scrivi la descrizione completa sulla pagina dell'oggetto (supporta il BBCode di Steam). |
| `changenote` | La prima voce della scheda Note di modifica dell'elemento. |

Regole per i valori:

- Scrivi i percorsi con le barre (`C:/Users/me/Downloads/...`). Una barra rovesciata deve essere raddoppiata (`C:\\Users\\me\\...`).
- Una doppia virgoletta all'interno di un valore viene scritta `\"`.
- Conservare il file ASCII.

<a id="5-create-the-item"></a>
## 5. Crea l'oggetto

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` è il nome con cui accedi a Steam, assegnato all'installatore. steamcmd utilizza il login salvato. Se richiede una password o un codice Steam Guard, esegui `powershell -ExecutionPolicy Bypass -File $Watcher login`, quindi esegui nuovamente il comando.

La corsa termina con `Success.`. Apri Steam > il tuo profilo > Oggetti Workshop, apri il nuovo oggetto e copia il numero dopo `?id=` nel suo URL : cioè l'oggetto ID.

<a id="6-finish-the-items-page"></a>
## 6. Completa la pagina dell'articolo

Nella pagina dell'oggetto su Steam:

1. Accetta l'accordo legale di Steam Workshop se la pagina mostra il banner. Fino a quando non lo farai, solo tu potrai vedere l'oggetto.
2. Scrivi la descrizione completa.
3. Se la mod necessita di un'altra mod Workshop (ad esempio Rivalry Core), aggiungila in **Aggiungi/Rimuovi elementi richiesti**. Agli abbonati viene quindi chiesto di abbonarsi anche ad esso. Con la dipendenza in `meitou.json`  `dependencies`, Arrange del launcher lo inserisce per primo nell'ordine di caricamento.
4. Modifica la visibilità in **Pubblica**quando la pagina è pronta.

<a id="7-watch-the-item"></a>
## 7. Guarda l'oggetto

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

Il test riporta la versione corrente come già caricata. Rimuovi `$Work` una volta fatto.

<a id="each-release-after-that"></a>
## Ogni uscita successiva

Rilascia la mod come nel passaggio 1. La successiva esecuzione programmata dell'osservatore scarica ZIP, controlla il suo SHA -256 e lo carica nell'elemento, con il titolo di rilascio e il collegamento come nota di modifica. Non cambia mai il titolo, la descrizione, l'anteprima o la visibilità dell'articolo: cambiali nella pagina dell'articolo. `powershell -ExecutionPolicy Bypass -File $Watcher status` mostra il programma, la corsa successiva e l'ultimo caricamento.

[English](../../../docs/steam-workshop.md) · [Kenshi Races](../README.md)
