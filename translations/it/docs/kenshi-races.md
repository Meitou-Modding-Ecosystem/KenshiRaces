<a id="kenshi-races"></a>
# Kenshi Races

Tre nuove razze per l'editor dei personaggi, ciascuna con sottorazze, sugli scheletri umani del gioco ( `kenshi.races` , nessuna dipendenza): ogni record aggiunto al caricamento, i corpi adattati dal [race import](race-import.md) . Viene inoltre fornita una [libreria di animazioni](animations.md) di 59 clip per ogni razza su quegli scheletri. I mod utilizzano razze e clip, aggiungono sottorazze o aggiungono le proprie razze: [depending-on-kenshi-races.md](depending-on-kenshi-races.md) .

<a id="races"></a>
## Razze

L'editor elenca ogni RACE_GROUP come razza e le sue CORSE come sottorazze (engine docs/characters.md "Character editor").

| Gara ( RACE_GROUP ) | Sottorazza ( RACE ) | Generi | Caratteri |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | maschio, femmina | leomamaschio `11-` , leomafemmina `17-` |
| Crevan `31-` | crevan `18-` | maschio, femmina | crevanmaschio `19-` , crevanfemmina `21-` |
| Crabfolk `32-` | granchio `12-` | separare | granchio `13-` |
|  | regina dei granchi `14-` | separare | regina dei granchi `15-` |

Gli ID finiscono con `KenshiRaces.mod` . I nomi di sottorazza e CHARACTER sono una parola minuscola, quindi `spawn` della console li prende senza virgolette. I file di ciascun corpo sono `KenshiRaces/items/races/kenshiraces_<character>*` (mappe normali su Leoma e Crevan ). I corpi maschio sono montati su `male_skeleton` , quelli femminili su `female_skeleton` ; i corpi del granchio sono di vestibilità maschile, quindi una regina del granchio è dello stesso sesso con animazioni e voce maschili. Modelli di origine (esportazioni Meshy , slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman e crabqueen "... - 3ds Max Biped.glb", gli altri `leonidfemale.glb` , `corvidmale.glb` , `corvidfemale.glb` .

<a id="records"></a>
## Record

Aggiunto nella fase dei dati dai record del gioco caricato ( `RaceRecords` ), quindi `KenshiRaces.mod` viene spedito vuoto e CI non necessita di gioco:

- RACE_GROUP : il nome della razza e le RAZZE delle sue sottorazze, nell'ordine dell'editore.
- RACE (sottorazza): una copia di Greenlander ( `17-gamedata.quack` : statistiche, animazioni, voci) con mesh di ogni genere, texture di colore, mappa normale e mappa delle parti (una sottorazza dello stesso sesso indossa il suo unico corpo per entrambi); `single gender` a meno che non abbia entrambi i corpi; niente teste né capelli (una razza che elenca nessuna testa non ne disegna nessuna; il corpo ha il suo); `morph num` 0; `no shirts` (camicie e pantaloni dipingono il layout UV del corpo umano); la maschera del corpo e le mappe normali della forza sono state cancellate; `playable` con `editor limits` [ `kenshiraces_editor.xml` ](#character-editor) .
- CHARACTER : una copia del Ramingo ( `2205-gamedata.base` ) della sottorazza e del sesso ( `female chance` 100 o 0), senza kit, denaro o taglia, con il discorso sulla recluta.
- Motore: il campo `mesh` di uno CHARACTER prevale sulla mesh del corpo della sua razza.

<a id="character-editor"></a>
## Editor di personaggi

Le razze vengono offerte durante la creazione del personaggio come quelle del gioco. `KenshiRaces/items/races/kenshiraces_editor.xml` mantiene solo i cursori che muovono le ossa, quindi funzionano sui corpi adattati come su un essere umano (engine docs/animation.md, le scale ossee): il viso contiene le dimensioni della testa, la forma della testa e i cursori del collo (nessuna posa del viso: le teste adattate non ne hanno); Il corpo contiene i cursori del corpo umano e il tono della pelle, con le dimensioni del seno, l'altezza e la spaziatura per le donne, che spostano lo scheletro femminile `L Boob` / `R Boob` su cui sono pesati i corpi femminili ([importazione razza](race-import.md#bust) ). I range sono quelli umani ( `data/editor/editor_data_human.xml` ).

<a id="recruiting"></a>
## Reclutamento

DIALOGUE  `1-KenshiRaces.mod` : il personaggio dice "Sembra che ti servirebbero un altro paio di mani.", il giocatore risponde "Unisciti a noi." e l'azione 18 (unirsi alla squadra) aggiunge il personaggio alla squadra del giocatore.

<a id="testing-in-game"></a>
## Test nel gioco

1. Abilita Kenshi Races nel launcher (o `data/mods.cfg` ).
2. Console: `spawn leomafemale` (o qualsiasi CHARACTER nella tabella) viene generato in corrispondenza del cursore; parla con il reclutatore o esegui il debug F10 di Recluta.
3. F10 ** Kenshi Races : gare** elenca tutte le razze montate caricate (queste mod e dipendenti).
4. Nuovo gioco: le frecce di corsa dell'editor raggiungono Leoma , Crevan e Crabfolk .

<a id="clothing"></a>
## Vestiario

L'armatura condivide lo scheletro del corpo, quindi si muove con questi corpi, ma è modellata attorno al corpo umano. La mappa delle parti nasconde la pelle sotto gli articoli con `hide parts` (cappotti, vesti, giacche); il dimagrimento attira ciò che è più voluminoso di un essere umano altrove ([race import](race-import.md#slimming) ). Forme lontane dall'essere umano (conchiglie, artigli) penetrano ancora attraverso le armature strette.
