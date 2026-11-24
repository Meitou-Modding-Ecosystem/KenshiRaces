<a id="kenshi-races"></a>
# Kenshi Race

Tre nuove corse per l'editore del personaggio, ognuna con subrassi, sul scheletro umano del gioco (`kenshi.races`, nessuna dipendenza): ogni record aggiunto alla carica, i corpi attrezzati da [La corsa di importazione](race-import.md). E’ anche una nave [Libreria di Animazione](animations.md) di 59 clip per ogni gara su questi scheletri. I mod utilizzano le corse e i clip, aggiungono le subrassi o aggiungono la propria corsa: [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## Race

L’editore elenca ogni RACE_GROUP Come una corsa e la sua RACEs come subrassi (in motore docs/characters.md “Il personaggio editoriale”.

| La corsa (RACE_GROUP) | Il sottosegretario (RACE) | Generi | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | Il Leoma `10-` | maschile, femminile | Il Leomale `11-`La leomafemmina `17-` |
| Crevan `31-` | La creatura `18-` | maschile, femminile | Crevanmale `19-`La crevanfemmina `21-` |
| Crabfolk `32-` | Il Crabman `12-` | Il singolo | Il Crabman `13-` |
| | Il crab `14-` | Il singolo | Il crab `15-` |

Si finisce in `KenshiRaces.mod`. Subrace e CHARACTER I nomi sono una parola inferiore, quindi la console `spawn` E’ possibile farlo senza citazioni. I file di ciascun corpo sono `KenshiRaces/items/races/kenshiraces_<character>*` (Normali mappe su Leoma e Crevan). I corpi maschili sono disposti a `male_skeleton`Le femmine a `female_skeleton`I corpi crab sono maschili, quindi una regina crab è single con le animazioni maschili e la voce. Il modello di base (Meshy esportazioni, slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman e crabqueen "... - 3ds max Biped,glb", gli altri `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## Il record

Aggiunto nella fase dei dati da Pietro dei propri record del gioco caricato (`RaceRecords`) così `KenshiRaces.mod` La nave è vuota e CI non ha bisogno di gioco:

- RACE_GROUPIl nome della razza e i suoi subrassi RACEsIn ordine dell’editore.
- RACE (sotto): una copia di Greenlander (`17-gamedata.quack`: statistiche, animazioni, voci) con la maschera di ogni genere, tessuto di colore, mappa normale
e una mappa parziale (una subraza single-sessuale porta il suo corpo per entrambi); `single gender` a meno che non abbia entrambi i corpi; nessuna testa o capelli (una corsa che non elenca nessuna testa; il corpo ha la sua); `morph num` 0; `no shirts` (T-shirts e pantaloni pittano il layout UV del corpo umano); maschera corporea e mappe normali di forza pulite; `playable` con `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTERUna copia di Drifter (`2205-gamedata.base`Il genere e il genere (`female chance` 100 o 0), nessun kit, denaro o bonus, con il
Il recruttore parla.
- motore: a CHARACTERIl proprio `mesh` Il campo supera il corpo della sua corsa.

<a id="character-editor"></a>
## Il personaggio dell'editore

Le corse sono offerte alla creazione di personaggi come il gioco stesso. `KenshiRaces/items/races/kenshiraces_editor.xml` Mantenere solo gli spazzolini che muovono le ossa, quindi lavorano sui corpi adatti come su un uomo (motore) docs/animation.md, le scale ossea): Il viso tiene la dimensione della testa, la forma della testa e le spalle del collo (non posizioni del viso: le teste fatte non hanno nessuna); il corpo tiene le spalle dell'organismo umano e Tone della pelle, con dimensioni del seno, altezza e spazio per le donne, che muovono lo scheletro femminile. `L Boob` / `R Boob` I corpi femminili sono sottoposti a ([La corsa di importazione](race-import.md#bust)). I rami sono gli uomini (`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## Il reclutamento

DIALOGUE `1-KenshiRaces.mod`Il personaggio dice: “Sembra che tu possa usare un altro paio di mani”, il giocatore risponde: “Unisciti a noi” e l’azione 18 aggiunge il personaggio alla squadra.

<a id="testing-in-game"></a>
## Testare nel gioco

1. Permette Kenshi Race nel lanciatore (o `data/mods.cfg`).
2. Il console: `spawn leomafemale` (o qualsiasi CHARACTER nella tabella) spawns al cursore; parlare di reclutamento, o F10 debug Recruit.
3. F10 **Kenshi Rassaggi: Rassaggi** L'elenco di tutte le corse accoppiate (questi mod e dipendenti).
4. Nuovo gioco: i lanci di gara dell'editore raggiungono Leoma, Crevan e Crabfolk.

<a id="clothing"></a>
## Abbigliamento

L'armore condivide lo scheletro del corpo, quindi si muove con questi corpi, ma è modellato intorno al corpo umano. La parte mappa nasconde la pelle sotto gli elementi con `hide parts` (cattivi, vestiti, giacche); la perdita di peso si affaccia su ciò che è più bulkier di un uomo altrove ([La corsa di importazione](race-import.md#slimming)). Le forme lontane dagli esseri umani (caccioli, cavi) ancora si spingono attraverso un armadio stretto.
