<a id="publishing-to-the-steam-workshop"></a>
# Pubblicazione al Steam Workshop

Crea l'oggetto Workshop di mod una volta, per mano, da una versione ZIP. Dopo questo, [Workshop di Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) funziona sul tuo PC Windows e carica ogni nuova versione di GitHub su un programma. Tutti i passaggi in corso **Windows PowerShell** sul tuo PC, iscriviti al conto Steam che sarà proprietario dell'articolo.

I comandi utilizzano questi valori. Imposta una volta per la finestra PowerShell, con il nome della cartella del mod (come in `release.yml` `mod-folder`e la sua prima versione:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Rilasciare il mod

1. Mescolare `beta` In entrata `main`Poi si apre e si unisce un PR `main` In entrata `release` ([I rami](../README.md#branches)).
2. Aspettate il flusso di lavoro di rilascio `release`. E’ il segno della fusione `<slug>-v<version>` Con la versione in `meitou.json` (per esempio il
`example-mod-v0.1.0`e crea una liberazione con `<Mod>-<version>.zip` e `<Mod>-<version>.zip.sha256`. Se si fa male prima di farlo, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` Il segnale viene premuto per mano (`-DryRun` Scrivi solo i comandi).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Installare Workshop Watcher e iscriversi a Steam

Seguiamo [Installazione](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) In questo caso, il workshop Watcher. L'installazione scarica SteamCMD `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` Inserisci il tuo account di Steam. Quando chiede `Add a mod to watch now?`La risposta `n`L’oggetto non esiste ancora. Rimuovere questo passo se Workshop Watcher è già installato.

<a id="3-prepare-the-items-files"></a>
## 3. Preparare i file dell'articolo

1. Scarica il rilascio ZIP e spazzatelo in `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` Ora si detiene `$Mod.dll`, `meitou.json` e `$Mod.mod`.
2. Salva l'immagine prevedibile dell'oggetto come `$Work\preview.png`: PNG o JPG, sotto 1 MB, quadrato (512 x 512 pixel funziona).

<a id="4-write-the-vdf"></a>
## 4. Scrivi il VDF

l'impostazione è creata dall'elemento a VDF file: un file di testo di `"key" "value"` Coppia all'interno di A `"workshopitem"` Il blocco. Questo comando scrive `$Work\create.vdf` con i sentieri riempiti. Edito `title` e `description` Prima di tutto:

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

| La chiave | Valore |
| --- | --- |
| `appid` | `233860`, KenshiIl Steam app ID. |
| `publishedfileid` | `0` Crea un nuovo elemento. Qualsiasi altro numero di aggiornamenti che l'articolo esistente. |
| `contentfolder` | Il percorso completo verso la cartella i cui file diventano il contenuto dell'articolo: la cartella mod dal ZIPMa non i suoi genitori. |
| `previewfile` | Il percorso completo verso la preview. |
| `visibility` | `0` Il pubblico, `1` Solo gli amici, `2` Il privato, `3` Non elencato Inizia con `2` e cambiare la pagina dell'articolo. |
| `title` | Il nome dell'articolo sul workshop. Inizia con `[Meitou]`Come fanno gli altri mod di codice Meitou. |
| `description` | Una linea di testo. Inserisci la descrizione completa sulla pagina dell'elemento e poi (sostenisce Steam BBCode). |
| `changenote` | La prima voce dell'articolo è la scheda Cambiamenti. |

Regole per i valori:

- Scrivi i tuoi passaggi con i passaggi (`C:/Users/me/Downloads/...`). Un taglio deve essere duplicato (`C:\\Users\\me\\...`).
- Una doppia citazione all'interno di un valore è scritta `\"`.
- Mantenere il file ASCII.

<a id="5-create-the-item"></a>
## 5. Crea l’oggetto

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` è il nome che si iscrive a Steam con, come dato all'installer. steamcmd utilizza il login salvato. Se chiede una password o un codice Steam Guard, eseguire `powershell -ExecutionPolicy Bypass -File $Watcher login`E poi riprende il comando.

La corsa si conclude con `Success.`. Apri Steam > il tuo profilo > Elementi di Workshop, apri il nuovo oggetto e copia il numero dopo `?id=` Nel suo URLQuesto è l’identità dell’oggetto.

<a id="6-finish-the-items-page"></a>
## 6. Fini la pagina dell'articolo

Sulla pagina del prodotto in Steam:

1. Accetta il contratto legale di Steam Workshop se la pagina mostra il banner. Fino a quando, solo voi potete vedere l'articolo.
2. Scrivi la descrizione completa.
3. Se il mod è in grado di sviluppare un altro mod di Workshop (ad esempio Rivalry Core), aggiungerlo sotto **Add/Remove Gli oggetti necessari**. Gli abbonati sono
Chiedono di iscriversi anche a questo. Con la dipendenza da `meitou.json` `dependencies`L'ordine del lanciatore lo mette prima nell'ordine di carico.
4. Cambiare la visibilità per **Pubblico** Quando la pagina è pronta.

<a id="7-watch-the-item"></a>
## 7. Guardate l'articolo

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

Il corso secco riferisce il rilascio attuale come già caricato. Rimuovere `$Work` Una volta che lo fa.

<a id="each-release-after-that"></a>
## Ogni rilascio dopo questo

Rilasciare il mod come in fase 1. La prossima edizione del programma di scarica il ZIPControllare la sua SHA-256 e lo carica all'articolo, con il titolo di rilascio e il link come nota di cambiamento. Non cambia mai il titolo, la descrizione, la visibilità dell’oggetto: cambia quello che è nella pagina dell’oggetti. `powershell -ExecutionPolicy Bypass -File $Watcher status` Mostra il calendario, la prossima corsa e l'ultimo upload.
