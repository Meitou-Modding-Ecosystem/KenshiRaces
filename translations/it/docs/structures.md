<a id="structures-from-3d-models"></a>
# Strutture da modelli 3D

Possiamo a `.glb` Il modello (Meshy, Hi3D, un Blender esportazione) nel tuo mod `Structures` La cartella e la costruzione del gioco lo trasformano in un edificio che puoi mettere dal menu di costruzione: Ogre, tessuti compressi, collisione e registri `<Mod>.mod`. No FCSNon c’è Blender, non c’è Installazione per l’importazione.

<a id="quick-start"></a>
## Rapido inizio

1. Inserisci il modello `<Mod>/Structures/`di E.G. `MyMod/Structures/watch-tower.glb`.
2. Opzionale: oltre a questo `watch-tower.json` Con qualsiasi [Settimenti](#settings)di E.G.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Costruire e distribuire come di solito ([play.md](play.md)• Visual Studio F5, VS Codice F5, o `dotnet run --project Play`. Il log mostra una
`Structure ...` Linee per modello.
4. Nel gioco: costruire il menu, categoria `EXTERIOR` (o quello che avete messo) Con il default `"research": "start"` È noto dall’inizio di una
Il gioco, e anche i risparmi esistenti (Meitou lavora fuori dalla ricerca finita, che include sempre `_Default Start`).
5. Il modello, il suo `.json` e `Structures/ids.json` (scrive sul primo posto di importazione) I file generati sono di produzione e non sono
Compiuto da

Senza giocare: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## Impostazioni

Tutte le opzioni (`<model>.json`, i commenti consentiti; un nome sconosciuto è un errore per cui i tipi non vanno ignorati).

| Settiamo | di default | Significato |
| --- | --- | --- |
| `name` | Il nome del file, `-`/`_` Come spazi | Nome nel menu di costruzione |
| `description` | `""` | Creazione di menu testo |
| `height` | La dimensione del proprio modello | L'altezza finita in metri; il modello è scalato ugualmente a esso. I modelli AI vengono a dimensioni arbitrari, quindi impostare questo |
| `sink` | `0` | metri scendono nel terreno (protegge dal fluttuare sulle spalle) |
| `yaw` | `0` | Gradi girati intorno all'asse verticale |
| `category` | `EXTERIOR` | La categoria dei menu (`building category`Il gioco è (`BUILDINGS`, `CAMPING`, `FARMING`... o un nuovo nome |
| `group` | `""` | `building group`: edifici condividendo uno sono varianti di un'entrata |
| `materials` | `5` | Il materiale che costa |
| `buildSpeed` | `2` | `build speed mult` (la tenda del gioco 5, case da 1 a 2) |
| `research` | `start` | `start`Conosciuto dall’inizio; `none`e non lo fa nulla (o non lo fa) RESEARCH String ID e.g. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` o `both` |
| `createsTown` | `true` | L'immissione fuori da una città comincia con un outpost |
| `collision` | `mesh` | `mesh` (una copia semplificata del modello), `box` (e la scatola è limitata) o `none` (caminare attraverso di esso) |
| `collisionTriangles` | `2000` | La maggior parte dei triangoli `mesh` La collisione continua (12 a 50.000) |
| `textureSize` | `2048` | il più grande lato di tessuto in pixel (una potenza di due, con un massimo di 8192) |

<a id="item-models"></a>
## Modelli di oggetto

Inserisci un modello di oggetto `<Mod>/ItemModels/` E invece (`Structures` che lo renderebbe un edificio). L'importazione scrive la sua messa e le sue texture accanto alle strutture" e MATERIAL_SPECS_CLOTHING Il tuo mod; il tuo mod ITEM Il record poi segna loro. Il log mostra una `Item ...` Linee per modello con i nomi da usare.

1. `MyMod/ItemModels/lantern.glb`Opzionale `lantern.json`: `{ "size": 0.3 }`.
2. Nella registrazione degli articoli (generatore di dati o fase di dati), impostare:

Il valore del campo è
   | --- | --- |
| `mesh` e `ground mesh` (Filenami) Il suo `<mod>_<model>.mesh`di E.G. `mymod_lantern.mesh` | | `material` (Riflessione) Il Corano `<950000 + 100 × block>-<Mod>.mod`Il blocco da `ItemModels/ids.json`di E.G. `950000-MyMod.mod` |

Vediamo prima il materiale (`GameDatabase.Find`e mantenere lo sguardo quando è mancato: un zip rilasciato, e i test che leggono il gioco senza il mod, non lo hanno.
3. Il modello, il suo `.json` e `ItemModels/ids.json`.

| Settiamo | di default | Significato |
| --- | --- | --- |
| `name` | Il nome del file, `-`/`_` Come spazi | Il nome del registro materiale |
| `size` | La dimensione del proprio modello | il lato più lungo in metri; il modello è scalato in modo uguale a quello |
| `yaw` | `0` | Gradi girati intorno all'asse verticale |
| `textureSize` | `1024` | il più grande lato di tessuto in pixel (una potenza di due, con un massimo di 8192) |

- Un prodotto per modello (la ITEM I nomi uno `material`Il modello si trova al suo punto più basso, centrato, come fanno le scatole del gioco.
- Il gioco tira un elemento sul terreno, in una mano e in un'icona auto con la sua `mesh` (`ground mesh` prima sul terreno) e il
MATERIAL_SPECS_CLOTHING Il suo `material` Il nome del motore `docs/formats/runtime-materials.md` “Calleri” `WorldObjectLayout.ItemMesh`).
- L'icona dell'inventario viene ritirata dal modello a meno che i record non `auto icon image` falso e il proprio `icon`.

<a id="getting-a-good-model"></a>
## Ottenere un buon modello

- Chiediamo Meshy o Hi3D Per A PBR- Modello testo e scaricalo come GLB. Un paio di decine di migliaia di triangoli sono abbondanti; il log flagga un modello
Più di 150mila. Tagliare la piattaforma, scegliere la dimensione e tagliare le tessute: [model-prep.md](model-prep.md).
- Il modello di fronte è +Z nel file (glTFdi convenzione); utilizzo e `yaw` per girarlo.
- Ogni materiale diventa la sua parte. I generatori generatori di solito danno uno; al massimo 49.
- Il modello si trova al suo punto più basso, centrato.

<a id="what-it-writes"></a>
## Cosa scrive

Inserisci la scheda Mod (`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` 1 per materiale, `_1`, `_2`, ... quando più): Ogre `[MeshSerializer_v1.8]`Posizione, normale, UV e
tangente con la manovra in w, il layout del gioco di costruzione.
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: diffusa (BC3), normale (BC1, BC3 quando tagliato), metallica (BC1), con mipi.
- `structures/<mod>_<model>_col.xml`La collisione .
- Per un modello di oggetto: `structures/<mod>_<model>.mesh` e la sua composizione, nessuna collisione.
- I record si uniscono `<Mod>.mod` oltre a qualsiasi altra cosa in esso (i registri di un generatore di dati vengono conservati; viene sostituita una importazione precedente):
BUILDINGe uno BUILDING_PART e MATERIAL_SPEC per il materiale, e l'inclusione (la ricerca `enable buildings` La costruzione è un’edificio; uno MATERIAL_SPECS_CLOTHING Per il modello.

I nomi dei file iniziano con il nome del mod, perché il gioco mostra le risorse per il nome del file su ogni mod.

I record id sono `<n>-<Mod>.mod` con n da 900000 a 3⁄4, un blocco di 100 a 3⁄4 per modello (construzione +0, parti +1, materiali +50); modelli di oggetti contano da 950000 (materiale +0) in `ItemModels/ids.json`. `Structures/ids.json` Mantenere il blocco di ogni modello in modo che gli ID non cambiano mai: un salvataggio chiama un edificio posto per l'ID del suo record. Non modificare mai un blocco; rimuovere un modello rimuove il suo edificio dai giochi che lo hanno messo.

<a id="facts-the-import-relies-on"></a>
## Il fatto che l’importazione dipende da

Controllato contro i file del gioco (Kenshi 1.0.68 dati) e con i lettori del motore:

- **Unità**: l'unità mondiale è 0,1 m, Y in alto; glTF è metri, Y in alto, quindi la scala è 10 (i file di collisione del gioco utilizzano la gravità 98.1).
- **Materiali**La vista di una parte dell’edificio è MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`Non è la messa
Il nome del motore (Engine `docs/formats/runtime-materials.md`). Il giocatore del gioco (`data/materials/deferred/objects.hlsl`Leggi diffuso RGB come colore e A come gloss (tempo `specular mult`di norma RGB Come il Tango-spazio normale e A come la maschera di taglio (`material type` 1, `alpha threshold`Il metallico di R. L'importazione scrive gloss = 1 - glTF La rottura e il metalliismo da glTFil canale blu; glTF MASK e BLEND Diventare un taglio.
- **Il Tango**Il sistema binorale del schiaffo è a croce (tangente, normale) × w, e sulle messe del gioco segna lungo l'aumento di V (**Verificato**:
`prisoner_pole.mesh` 86 di questi triangoli, `Camp_tent1.mesh` 1858 e 1865. glTF Le mappe normali mettono in luce l'immagine (sconto V), quindi l'importazione ruota il canale verde e mette in luce il modo in cui si gioca.
- **Collisione**: una parte di `xml collision` è un file PhysX NxuStream2; un carico minimo di file scritto a mano (il gioco `Camp_tent1_COL.xml` è
una scatola) PhysX è Z up e le mappe di gioco (x, y, z) a (x, z, -y), quindi l'importazione scrive i punti della parte come (x, -z, y). Il triangolo entra come punti piatti e triangoli; il gioco lo fa carico, Meitou lo legge come è (motore) `docs/formats/collision.md`).
- **I campi**: BUILDING e BUILDING_PART trasportare il set di campo completo del gioco Camping Tent 1 (`18026-small_changes_otto.mod`E così niente
Si tratta di un default; no `functionality`E’ un edificio piatto.
- **disattivazione**: un edificio è nel menu di costruzione una volta che una ricerca finita lo elenca in `enable buildings`Ogni gioco inizia con
`_Default Start` (`5359-gamedata.base`e finito . **Verificato** con il motore `GameDatabase`, `ResearchGate` e `BuildingBuildData` sul gioco di base e su un mod importato: le strutture sono disattivate e costano i materiali di costruzione come stabilito.

<a id="not-covered-yet"></a>
## Non coperto ancora

Interiori, porte, funzioni (servizio, letti, lavorazione) e parti di rotazione.
