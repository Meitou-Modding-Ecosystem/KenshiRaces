<a id="engine-references-refcheck"></a>
# Il motore di riferimento (RefCheck)

Un mod è costruito contro un motore (questo repo `engine` Il gioco si svolge su qualsiasi tipo di gioco che il giocatore abbia. Quando un rilascio cambia la firma di qualcosa che il mod chiama, la fonte del mod può ancora compilare, ma DLL che è già stato costruito chiede il gioco per il vecchio membro e fallisce nel tempo con `MissingMethodException`.

Questo non è ipotetico. Sito 021 da `Inventory.Add(ItemInstance)` Il secondo modello è opzionale (`Add(ItemInstance, bool mainShut = false)`). La fonte che chiama `bag.Add(item)` La versione è contraria, ma una DLL costruito contro 0.20 o più, chiama il metodo di un parametro, che 0.21 non ha più. In The Wasteland ricordiamo che i mantelli sono in gioco, e l'eccezione ha interrotto un intero sistema per il resto della sessione. Nessun test lo ha catturato: i test si compilano contro un motore.

<a id="what-it-checks"></a>
## Che cosa controlla

`tools/RefCheck` Leggi un mod costruito DLLi metadati e elenca ogni membro del motore che si riferisce (metodi, costruttori, campi su tipi in `Meitou*.dll`Il nome e i tipi di parametri, che un dato rilascio non ha. Niente del mod o del gioco funziona.

`tools/refcheck.sh` costruisce ogni mod nel repo (ogni cartella di livello superiore con `meitou.json` e `<Folder>/<Folder>.csproj`Scarica una versione win-x64 zip una volta (solo `Meitou*.dll` e `0Harmony.dll`e verifica ogni assemblea il mod `meitou.json` Le liste .

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Una linea per rilascio e un’assemblea:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

O per una pausa:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## In questo

Il `Engine references` Il lavoro corrente `tools/refcheck.sh` contro il più recente rilascio su ogni PR e pressione, e `Release readiness` Aspettiamo per questo. Un’impresa che porta a DLL Un motore più vecchio dovrebbe anche controllare DLL Per ogni rilascio di `meitou.json` Permette di

<a id="when-it-finds-something"></a>
## Quando si trova qualcosa

Chiama il membro in modo che funziona su ogni rilascio che supporta: ricostruisci il motore se lascia il vecchio, o trovi il membro per riflettere e riempire i parametri che non passano con i loro default. Il deserto si ricorda `InventoryCompat` Il secondo per `Inventory.Add`.

Le chiamate di riflessione nel motore non sono controllate: falliscono su come le hai scritte.
