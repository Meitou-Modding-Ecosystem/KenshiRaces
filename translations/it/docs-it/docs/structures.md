<a id="structures-from-3d-models"></a>
# Strutture da modelli 3D

Inserisci un modello `.glb` ( Meshy , Hi3D , un'esportazione Blender ) nella cartella `Structures` del tuo mod e la build di gioco lo trasformerà in un edificio che puoi posizionare dal menu di costruzione: mesh Ogre, texture compresse, collisione e record in `<Mod>.mod` . No FCS , no Blender , nessuna installazione del gioco necessaria per l'importazione stessa.

<a id="quick-start"></a>
## Avvio rapido

1. Inserisci il modello in `<Mod>/Structures/` , ad es. `MyMod/Structures/watch-tower.glb` .
2. Opzionale: accanto, `watch-tower.json` con qualsiasi [impostazioni](#settings) , ad es.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Costruisci e distribuisci come al solito ([play.md](play.md) ): Visual Studio F5, VS Code F5 o `dotnet run --project Play` . The build log shows one `Structure ...` line per model.
4. Nel gioco: menu di costruzione, categoria `EXTERIOR` (o quella impostata da te). Con lo `"research": "start"` predefinito è noto dall'inizio del gioco e anche nei salvataggi esistenti ( Meitou elabora gli sblocchi dalla ricerca terminata, che include sempre `_Default Start` ).
5. Impegna il modello, i suoi `.json` e `Structures/ids.json` (scritti alla prima importazione). I file generati sono output di compilazione e non vengono sottoposti a commit.

Senza gioco: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>` .

<a id="settings"></a>
## Impostazioni

Tutto facoltativo ( `<model>.json` , commenti consentiti; un nome sconosciuto è un errore quindi gli errori di battitura non passano inosservati).

| Collocamento | Predefinito | Senso |
| --- | --- | --- |
| `name` | nome file, `-` / `_` come spazi | nome nel menu di creazione |
| `description` | `""` | creare il testo del menu |
| `height` | taglia propria del modello | altezza finita in metri; il modello viene scalato in modo uniforme rispetto ad esso.  I modelli AI hanno dimensioni arbitrarie, quindi impostalo |
| `sink` | `0` | metri affondati nel terreno (impedisce che galleggi sui pendii) |
| `yaw` | `0` | gradi ruotati attorno all'asse verticale |
| `category` | `EXTERIOR` | categoria del menu di costruzione ( `building category` ): il gioco ( `BUILDINGS` , `CAMPING` , `FARMING` , ...) o un nuovo nome |
| `group` | `""` | `building group` : gli edifici che ne condividono uno sono varianti di una voce |
| `materials` | `5` | Materiali da costruzione costa |
| `buildSpeed` | `2` | `build speed mult` (tende del gioco 5, case da 1 a 2) |
| `research` | `start` | `start` : conosciuto fin dall'inizio; `none` : niente lo sblocca (i tuoi record lo fanno); o un ID stringa RESEARCH , ad es. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor` , `indoor` o `both` |
| `createsTown` | `true` | posizionandolo fuori da una città avvia un avamposto |
| `collision` | `mesh` | `mesh` (una copia semplificata del modello), `box` (il suo riquadro di delimitazione) o `none` (percorrilo) |
| `collisionTriangles` | `2000` | la maggior parte dei triangoli conservati dalla collisione `mesh` (da 12 a 50.000) |
| `textureSize` | `2048` | lato della texture più grande in pixel (una potenza di due, al massimo 8192) |

<a id="item-models"></a>
## Modelli di articoli

Metti invece il modello di un oggetto in `<Mod>/ItemModels/` ( `Structures` lo renderebbe un edificio). L'importazione scrive la sua mesh e le sue texture accanto alle strutture e ad un record MATERIAL_SPECS_CLOTHING ; il record ITEM del tuo mod quindi punta a loro. Il registro di costruzione mostra una riga `Item ...` per modello con i nomi da utilizzare.

1. `MyMod/ItemModels/lantern.glb` , opzionalmente `lantern.json` : `{ "size": 0.3 }` .
2. Nel record dell'articolo (generatore di dati o fase dati), imposta:

| Campo | Valore |
   | --- | --- |
| `mesh` e `ground mesh` (nomi file) | `<mod>_<model>.mesh` , ad es. `mymod_lantern.mesh` | | `material` (riferimento) | `<950000 + 100 × block>-<Mod>.mod` , blocco da `ItemModels/ids.json` , ad es. `950000-MyMod.mod` |

Cerca prima il materiale ( `GameDatabase.Find` ) e mantieni l'aspetto del sostituto quando manca: uno zip rilasciato e test che leggono il gioco senza il mod distribuito, non ce l'hanno.
3. Impegna il modello, i suoi `.json` e `ItemModels/ids.json` .

| Collocamento | Predefinito | Senso |
| --- | --- | --- |
| `name` | nome file, `-` / `_` come spazi | il nome del record materiale |
| `size` | taglia propria del modello | lato più lungo in metri; il modello viene ridimensionato in modo uniforme rispetto ad esso |
| `yaw` | `0` | gradi ruotati attorno all'asse verticale |
| `textureSize` | `1024` | lato della texture più grande in pixel (una potenza di due, al massimo 8192) |

- Un materiale per modello di articolo (lo ITEM ne nomina uno `material` ); il modello si trova nel punto più basso, centrato, come fanno le maglie degli oggetti del gioco.
- Il gioco disegna un oggetto sul terreno, in una mano e nell'icona di un'auto con il suo `mesh` ( `ground mesh` primo a terra) e lo MATERIAL_SPECS_CLOTHING con i suoi nomi `material` (motore `docs/formats/runtime-materials.md` "Chiamatori", `WorldObjectLayout.ItemMesh` ).
- L'icona dell'inventario viene ricavata dal modello a meno che il record non imposti `auto icon image` false e il proprio `icon` .

<a id="getting-a-good-model"></a>
## Ottenere un buon modello

- Chiedi a Meshy o Hi3D un modello con texture PBR e scaricalo come GLB . Alcune decine di migliaia di triangoli sono sufficienti; il registro contrassegna un modello superiore a 150.000. Tagliare il polycount, scegliere la dimensione e restringere le texture: [model-prep.md](model-prep.md) .
- La parte anteriore del modello è rivolta verso +Z nel file (convenzione glTF); utilizzare `yaw` per girarlo.
- Ogni materiale diventa parte di se stesso. I generatori di solito ne danno uno; al massimo 49.
- Il modello si trova nel punto più basso, centrato.

<a id="what-it-writes"></a>
## Cosa scrive

Nella cartella mod distribuita ( `<Kenshi>/mods/<Mod>/` ):

- `structures/<mod>_<model>.mesh` (uno per materiale, `_1` , `_2` , ... quando diversi): Ogre `[MeshSerializer_v1.8]` , posizione, normale, UV e tangente con la mano in w, il layout delle maglie dell'edificio del gioco.
- `structures/<mod>_<model>_d.dds` , `_n.dds` , `_m.dds` : diffuso (BC3), normale (BC1, BC3 se tagliato), metallizzato (BC1), con mips.
- `structures/<mod>_<model>_col.xml` : la collisione.
- Per un modello di articolo: `structures/<mod>_<model>.mesh` e le sue texture, nessuna collisione.
- I record vengono fusi in `<Mod>.mod` accanto a qualsiasi altra cosa al suo interno (vengono conservati i record di un generatore di dati; vengono sostituiti quelli di un'importazione precedente): uno BUILDING , uno BUILDING_PART e MATERIAL_SPEC per materiale e lo sblocco (lo `enable buildings` della ricerca ottiene l'edificio); uno MATERIAL_SPECS_CLOTHING per modello di articolo.

I nomi dei file iniziano con il nome della mod perché il gioco cerca le risorse in base al nome del file semplice in ogni mod.

Gli ID dei record sono `<n>-<Mod>.mod` con n da 900000 in su, un blocco di 100 per modello (edificio +0, parti +1, materiali +50); i modelli degli articoli contano da 950000 (materiale +0) in `ItemModels/ids.json` . `Structures/ids.json` mantiene il blocco di ciascun modello in modo che gli ID non cambino mai: un salvataggio nomina un edificio posizionato in base all'ID del suo record. Non modificare o rimuovere mai un blocco; rimuovere un modello rimuove il suo edificio dai giochi che lo hanno posizionato.

<a id="facts-the-import-relies-on"></a>
## Fatti su cui si basa l'importazione

Controllato con i file del gioco (dati Kenshi 1.0.68) e con i lettori del motore:

- **Unità**: l'unità mondiale è 0,1 m, Y in alto; glTF è metri, Y in su, quindi la scala è 10 (i file di collisione del gioco utilizzano la gravità 98,1).
- **Materiali**: l'aspetto di una parte di un edificio è il suo MATERIAL_SPEC ( `texture map` , `normal map` , `metalness map` , `material type` ), non il nome del materiale della mesh (motore `docs/formats/runtime-materials.md` ). Lo shader di costruzione del gioco ( `data/materials/deferred/objects.hlsl` ) legge RGB diffuso come colore e A come lucentezza (volte `specular mult` ), RGB normale come normale dello spazio tangente e A come maschera di ritaglio ( `material type` 1, `alpha threshold` ), metallità da R. L'importazione scrive gloss = 1 - rugosità e metallicità glTF dal canale blu di glTF; glTF MASK e BLEND diventano un ritaglio.
- **Tangenti**: la binormale dello shader è croce(tangente, normale) × w, e sulle mesh del gioco punta lungo la V crescente (**Verificato**: `prisoner_pole.mesh` 86 di 86 triangoli, `Camp_tent1.mesh` 1858 di 1865). Le mappe normali glTF puntano verso il verde verso l'alto dell'immagine (V decrescente), quindi l'importazione gira il canale verde e imposta la direzione del gioco.
- **Collisione**: `xml collision` di una parte è un file PhysX NxuStream2; viene caricato un file minimo scritto a mano (lo `Camp_tent1_COL.xml` del gioco è una scatola). PhysX è Z up e il gioco mappa (x, y, z) su (x, z, -y), quindi l'importazione scrive i punti della parte come (x, -z, y). La mesh triangolare si presenta come punti semplici e triangoli; il gioco lo cucina al caricamento, Meitou lo legge così com'è (motore `docs/formats/collision.md` ).
- **Campi**: BUILDING e BUILDING_PART trasportano il set completo di campi della tenda da campeggio 1 del gioco ( `18026-small_changes_otto.mod` ), quindi nulla viene impostato di default; no `functionality` , quindi è un edificio semplice.
- **Sblocco**: un edificio è nel menu di costruzione una volta che una ricerca terminata lo elenca in `enable buildings` ; ogni partita inizia con `_Default Start` ( `5359-gamedata.base` ) finito. **Verificato** con il motore `GameDatabase` , `ResearchGate` e `BuildingBuildData` nel gioco base più una mod importata: le strutture sono sbloccate e costano materiali da costruzione come impostato.

<a id="not-covered-yet"></a>
## Non ancora coperto

Interni, porte, funzioni (contenitori, letti, lavorazione) e parti tornite.
