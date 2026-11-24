<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Importazione di gare: inserimento di un corpo nello scheletro del gioco

`tools/RaceImport` trasforma un corpo `.glb` truccato ( Meshy , Hi3D : il rig "3ds Max Biped", articolazioni `Bip001 ...` ) in un corpo che il gioco anima con le proprie animazioni umane: una mesh di Ogre dalla pelle collegata a `male_skeleton.skeleton` o `female_skeleton.skeleton` , le sue texture DDS e una mappa delle parti. Legge lo scheletro dell'attrezzatura, il corpo umano e la mappa delle parti dall'installazione del gioco, quindi funziona su una macchina con Kenshi , mai in CI ; il suo output è impegnato.

<a id="run-it"></a>
## Eseguilo

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>` : la cartella del gioco ( `data/` al suo interno), ad es. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"` .
- La radice nomina i file e deve essere univoca tra i mod (il gioco trova i file in base al nome semplice): `<modfolder lower case>_<race>` .
- `male` / `female` : il rig, maschio predefinito. Un corpo femminile è adattato a `female_skeleton` e misurato rispetto a `human_female.mesh` e `FemaleMask_Default.png` (animazioni femminili e voce nel gioco, cursori del busto dell'editore); uno maschile a `male_skeleton` , `human_male.mesh` e `MaleMask_Default.png` .
- `slim` : da 0 a 1, predefinito 1 ([Dimagrante](#slimming) ); abbassatelo per una gara destinata ad essere più ingombrante dei suoi vestiti.
- Scrive `<stem>.mesh` , `<stem>_d.dds` , `<stem>_n.dds` (solo quando il modello ha una mappa normale) e `<stem>_parts.png` ; la linea di registro fornisce il conteggio dei vertici, la scala, i limiti e il numero di vertici che rientrano in parti nascosti.
- Mantieni lo `.glb` fuori dal repository (decine di MB ); impegnare i quattro output. `.gitignore` lascia passare `*/items/races/*.mesh` e `*.dds` .
- Una mod dipendente lo esegue da un clone di KenshiRaces con la propria cartella di output ( `<Mod>/items/races` ) e stem.

<a id="what-the-model-needs"></a>
## Di cosa ha bisogno il modello

- Una mesh rivestita la cui pelle ha le articolazioni Biped `Bip001 Pelvis` , `Spine` , `Spine1` , `Spine2` , `Neck` , `Head` , entrambi `Clavicle` , `UpperArm` , `Forearm` , `Hand` , `Thigh` , `Calf` , `Foot` , `Toe0` e `mixamorig:HeadTop_End` sopra la testa. Le articolazioni extra (dita, code, aiutanti) seguono il loro antenato bipede più vicino. **Verificato** su Leoma , Crevan , Crabman e Crab Queen (esportazioni Meshy ).
- Una trama di colore di base; quando presenti vengono utilizzate una rugosità metallica e una struttura normale.
- Qualsiasi posa di riposo: la posa a T e la posa ad A sono entrambe adatte (la posa di legame del gioco ha le braccia abbassate).

<a id="how-the-fit-works"></a>
## Come funziona l'adattamento

1. Ogni vertice viene inserito nella posa del resto mostrata dal file (trasformazione del riposo congiunto dei tempi di associazione inversa).
2. Scala: l'altezza dell'osso della testa del gioco sopra l'articolazione della testa del modello sopra il suo vertice più basso.
3. Ogni osso della colonna vertebrale, del braccio e della gamba viene ruotato in modo che punti dalla sua articolazione al bambino come fa nel gioco e allungato fino alla lunghezza del gioco (da 0,6 a 1,6 volte).
4. Bacino, colonna vertebrale, collo e clavicole sono montati come un unico pezzo rigido (i minimi quadrati sulle articolazioni del collo, delle spalle e dell'anca del gioco), in modo che il torace mantenga la sua forma; Gli adattamenti per osso lo tagliano dove i pesi della colonna vertebrale e della clavicola si fondono. Le articolazioni del bacino e della colonna vertebrale sono escluse da questo adattamento: il bacino del gioco è all'altezza dei fianchi e un bipede esportato si trova sopra di loro, facendo cadere il torace di un'unità.
5. La testa atterra a metà strada dal punto in cui il busto la trasporta all'articolazione della testa del gioco, quindi la sua rete si trova sotto l'osso. Il collo del gioco si solleva di circa un'unità sopra le spalle e quello di questi modelli lo fa a malapena: posizionando la testa sull'articolazione si allungano i colli fino a circa tre volte la loro lunghezza (**Osservato** sul maschio Leoma , Crabman e Crab Queen: collo aderente 2,4 unità, proprio dei modelli 0,8, 1,4 del gioco).
6. I piedi rimangono dritti e schiacciati in modo che le piante dei piedi tocchino il terreno (le zampe e le zampe del granchio sono più alte del piede del gioco).
7. Ogni vertice è la somma ponderata degli adattamenti dei suoi giunti; i suoi pesi vanno alle ossa del gioco seguite dalle sue articolazioni, le quattro più pesanti mantenute.

**Verificato**: le mesh dello strumento C# corrispondono a quelle del prototipo Python originale (posizioni entro 1e-5 unità, pesi ossei identici) su Crabman.

<a id="slimming"></a>
## Dimagrimento

L'abbigliamento del gioco è modellato sul corpo umano dell'attrezzatura, quindi un corpo più voluminoso di quello che sembra. Attorno a ciascun osso del tronco e degli arti (bacino, colonna vertebrale, clavicole, braccia e gambe) lo strumento misura la massima estensione del corpo umano per sesto dell'osso e per dodicesimo di giro attorno ad esso; un vertice adattato oltre tale estensione più 0,15 unità (1,5 cm di tessuto) mantiene un quinto del suo eccesso, la sua correzione è miscelata dai suoi pesi ossei. La correzione sfuma tra 1,5 e 2,5 volte il limite (oltre: un'appendice come un artiglio, lasciata sola), è zero per un vertice esterno alle fette dell'osso (una coda che pende sotto il bacino) ed è levigata sulla superficie (12 giri di media dei vicini, cuciture UV saldate) in modo che i vertici vicini si muovano allo stesso modo. **Osservato** (maschio Leoma ): il 39% dei vertici si muove, di 1,3 cm in media e di 9,5 cm al massimo.

<a id="bust"></a>
## Torace

Il busto di un corpo femminile è sollevato rispetto a quello della donna umana (**Osservato**: le donne Leoma e Crevan sedevano 2,0 e 1,75 unità più in basso, sotto il petto dei vestiti e il busto della mappa parziale). Il busto è il centro della fetta di un quarto, tra `Spine1` e le spalle e da 0,3 a 1,8 unità accanto alla linea mediana, il cui vertice più anteriore è più in avanti. Ogni vertice si solleva della differenza moltiplicata per il peso dell'osso del tronco, per exp(-(dy / 1.5)²) della sua altezza dal busto, per un passo graduale dalla profondità della colonna vertebrale (0) a quella del busto (1), quindi la schiena, le braccia e le gambe rimangono. Poi si corre il dimagrimento.

<a id="human-only-bones"></a>
## Ossa esclusivamente umane

Lo scheletro femminile ha `L Boob` e `R Boob` (le dimensioni del seno, l'altezza e la spaziatura dell'editor vengono ridimensionate e spostate, motore docs/animation.md), cosa che manca a un impianto bipede. Dopo il dimagrimento, ciascun vertice il cui osso più pesante è `Spine1` o `Spine2` assume i pesi esclusivamente umani del vertice umano più vicino che ne ha alcuni, entro 1 unità, scalati di 1 distanza; i suoi stessi pesi si restringono per fare spazio. **Osservato** ( `human_female.mesh` ): i vertici del busto sono portati dalle due ossa (892 dei 994 vertici della parte anteriore del torace). Le ossa ( `L Boob` 28, `R Boob` 29) e gli altri manici del rig femminile differiscono da quelli del maschio ( `R Clavicle` 23 contro 25; il maschio ha `Bip01 Jaw` ), quindi le mesh sono sempre scritte contro i manici del proprio rig.

<a id="part-map"></a>
## Mappa delle parti

Lo `part map male` / `female`  PNG della gara indica a quale parte del corpo appartiene ciascun vertice; `hide parts` degli oggetti usurati smette di disegnare quelle parti (engine docs/characters.md "Parti del corpo nascoste"). Lo strumento assegna a ciascun vertice adattato la parte del vertice del corpo umano più vicino portato dallo stesso osso, legge dalla mappa delle parti del rig in corrispondenza dell'uv di quel vertice e la scrive nell'uv del vertice adattato (1024 × 1024, altri texel trasparenti). Ogni parte umana viene prima cresciuta di 0,8 unità (un vertice senza parte prende quello del vertice contrassegnato più vicino entro quella distanza), poiché un corpo più voluminoso si aggancia su una fascia più ampia.

**Osservato** ( `MaleMask_Default.png` , 256², campionato a `human_male.mesh` uvs mentre il gioco li campiona): 642 dei 15.504 vertici hanno una parte: rossa la parte superiore del torace e delle spalle, bande bianche attorno alla parte superiore delle braccia, verde la parte anteriore del ventre. `hide parts` nei dati di base (rebirth.mod): giacca di pelle imbottita e del vagabondo 1, cappotto lungo senza maniche e spolverino 2, cappotto lungo e spolverino 3, Ninja Gi e stracci 4, abiti 5, giacche a piastre, tute di pelle e armatura di granchio 7; Stracci del Servo Santo, pantaloni da monaco e cappucci 0. Gli oggetti con 0 non nascondono nulla; il dimagrimento copre quelli. **Osservato** ( `FemaleMask_Default.png` a `human_female.mesh` uvs): rosso il busto (1.102 vertici), bianche le fasce del braccio (560), verdi il ventre (250).

<a id="textures"></a>
## Trame

- `_d` : colore base, alfa = gloss = (255 - rugosità) / 2 (rugosità 200 senza mappa); 2048², BC3 con mips.
- `_n` : la disposizione dei personaggi del gioco, X in alfa e Y in verde, Y invertita da +Y di glTF in su; rosso e blu 0.
