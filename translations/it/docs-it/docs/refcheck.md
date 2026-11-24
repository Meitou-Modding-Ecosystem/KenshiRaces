<a id="engine-references-refcheck"></a>
# Riferimenti motore ( RefCheck )

Una mod viene creata su un motore (il sottomodulo `engine` di questo repository) e riprodotta su qualunque versione Meitou abbia il giocatore. Quando una versione modifica la firma di qualcosa chiamato dal mod, il sorgente del mod può ancora essere compilato, ma DLL che era già stato creato richiede al gioco il vecchio membro e fallisce in fase di esecuzione con `MissingMethodException` .

Questo non è ipotetico. Meitou 0.21 ha fornito a `Inventory.Add(ItemInstance)` un secondo parametro opzionale ( `Add(ItemInstance, bool mainShut = false)` ). Il sorgente che chiama `bag.Add(item)` viene compilato con entrambe le versioni, ma uno DLL creato con la versione 0.20 o precedente chiama il metodo a un parametro, che la versione 0.21 non ha più. In The Wasteland Remembers ha rotto i ricordi in gioco e l'eccezione ha fermato un intero sistema per il resto della sessione. Nessun test lo ha rilevato: i test vengono compilati su un motore.

<a id="what-it-checks"></a>
## Cosa controlla

`tools/RefCheck` legge i metadati di una mod DLL creata ed elenca tutti i membri del motore a cui fa riferimento (metodi, costruttori, campi sui tipi in `Meitou*.dll` ), per nome e tipi di parametri, che una determinata versione non ha. Niente della mod o del gioco funziona.

`tools/refcheck.sh` crea ogni mod nel repository (ogni cartella di livello superiore con `meitou.json` e `<Folder>/<Folder>.csproj` ), scarica il file zip win-x64 di una versione una volta (mantenendo solo `Meitou*.dll` e `0Harmony.dll` ) e controlla ogni assemblaggio degli elenchi `meitou.json` del mod.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Output, una riga per rilascio e assemblaggio:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

oppure, per una pausa:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## Nel CI

Il lavoro `Engine references` esegue `tools/refcheck.sh` rispetto alla versione più recente su ogni PR e spinge, e `Release readiness` lo attende. Un mod che spedisce uno DLL costruito con un motore più vecchio dovrebbe anche verificare che DLL rispetto a ogni versione consentita dal suo `meitou.json` .

<a id="when-it-finds-something"></a>
## Quando trova qualcosa

Chiama il membro in un modo che funzioni su ogni versione supportata: ricostruisci con il motore più recente se elimini quelli più vecchi, oppure trova il membro per riflessione e inserisci i parametri che non passi con i loro valori predefiniti. `InventoryCompat` di Wasteland Remembers fa quest'ultimo per `Inventory.Add` .

Le chiamate di riflessione nel motore non vengono controllate: falliscono leggermente nel punto in cui le hai scritte.
