<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Race import: adattare un corpo allo scheletro del gioco

`tools/RaceImport` Si trasforma in rigido `.glb` Il corpo (Meshy, Hi3D: il "3ds Max Biped" rig, articolazioni `Bip001 ...`In un corpo il gioco si anima con le proprie animazioni umane: una pelle di Ogre legata a `male_skeleton.skeleton` o `female_skeleton.skeleton`Il suo DDS Il testo e una parte della mappa. Legge lo scheletro, il corpo umano e la parte del gioco, quindi funziona su una macchina con KenshiNon è mai in Cio; la sua produzione è impegnata.

<a id="run-it"></a>
## Fornisce il

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>`Il gioco del gioco (`data/` all’interno, e.g. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- I nomi dei file e devono essere unici in mod (il gioco trova i file per nome semplice): `<modfolder lower case>_<race>`.
- `male` / `female`Il rig, il male default. Il corpo femminile è adatto a `female_skeleton` e misurato contro `human_female.mesh` e
`FemaleMask_Default.png` (animazioni femminili e voce nel gioco, il bust slider dell'editore); un maschio per `male_skeleton`, `human_male.mesh` e `MaleMask_Default.png`.
- `slim`0 a 1, default 1 ([Il rallentamento](#slimming)b) la riduzione per una corsa che intendeva essere più bulkico dei suoi vestiti.
- scrittura `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (solo quando il modello ha una mappa normale) e `<stem>_parts.png`La linea di registrazione dà il
Il conteggio delle verticali, la scala, i confini e il numero di verticali che cadono in parti nascosti.
- Mantenere il `.glb` E’ un’altra cosa che si può fare con il ritorno (quattro volte). `.gitignore` Lascia `*/items/races/*.mesh` e `*.dds` attraverso il
- Una modazione dipendente va eseguita da un KenshiRaces clone con la propria cartella di uscita (`<Mod>/items/races`e la voce.

<a id="what-the-model-needs"></a>
## Cosa ha bisogno il modello

- Una maschera di pelle la cui pelle ha le articolazioni Biped `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`e entrambi `Clavicle`, `UpperArm`,
`Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`e `mixamorig:HeadTop_End` Sopra la testa. Le articolazioni extra (delle dita, le colpi, gli aiutanti) seguono il loro antenato più vicino. **Verificato** di Leoma, Crevan, Crabman e la Regina Crab (Meshy delle esportazioni).
- Una texture di colore di base; una rigidità metallica e una texture normale vengono utilizzati quando presenti.
- Qualsiasi pose di riposo: T-poso e A-poso si adattano entrambi (la pose di legame del gioco ha le braccia in basso).

<a id="how-the-fit-works"></a>
## Come funziona il fit

1. Ogni vertice è scintillato nel resto posizionando gli schermi del file (inverso connessione tempo congiunto riposo trasforma).
2. Scala: l'osso capelluto del gioco sopra l'articolazione della testa del modello sopra la sua parte inferiore.
3. Ogni spalla, mano e gambe è girato in modo che segni dalla sua articolazione al suo bambino come il gioco fa, e esteso alla lunghezza del gioco.
da 0,6 a 1,6 volte.
4. Pellini, spine, collo e clavicole sono adattati come un pezzo rigido (più square sul collo, ombra e articolazioni del gioco), quindi il petto
Mantiene la sua forma; per-bone si adatta a farlo dove spina e clavicolo pesano mescolando. Le articolazioni della cintura e della spina sono lasciate fuori: la cintura del gioco è al livello delle sue colline e i posti di esportazione sono sopra di loro, che hanno fatto cadere il petto su un'unità.
5. La testa si allontana a metà strada da dove il torso lo porta all'articolazione della testa del gioco, quindi la sua maschera si trova sotto l'osso. Il collo del gioco aumenta
Pietra un'unità pietra sopra le spalle e pietra e questi modelli di pietra a poco tempo fa: posizionando la testa sulla articolazione esteso il collo a circa tre volte la loro lunghezza (**osservato** sul maschio LeomaCrabman e la regina Crab: il collo è dotato di 2.4 unità, i modelli sono propri 0.8, il gioco è 1.4).
6. I piedi si mantengono piatti e piatti sono squashati in modo che i solli si collocino sul terreno (le piatte e i piedi di crab sono più alti del piede del gioco).
7. Ogni vertice è la somma pesante delle sue articolazioni; i suoi pesi vanno all'osso della sua articolazione, i più pesanti sono tenuti.

**Verificato**Le schede dell'utente C# corrispondono al prototipo originale di Python (posizioni all'interno di unità 1e-5, pesanti ossa identici) su Crabman.

<a id="slimming"></a>
## Il rallentamento

Il vestito del gioco è modellato intorno al corpo umano del rig, in modo che il corpo sia un bulkier di quanto dimostra. Intorno a ciascun osso torso e limbico (pelvi, spine, clavicole, braccia e gambe superiori e inferiori) l'instrumento misura l'estensione più stretta del corpo umano per il sesto osso e per il dodici di un giro intorno a esso; un vertice fisso oltre tale estensione plus 0,15 unità (1,5 cm di spina) mantiene un quinto del suo eccesso, la sua correzione mescolata dai suoi peso osseo. La correzione si esaurisce tra 1,5 e 2,5 volte il limite (a sinistra: un appendice come una crosta), è zero per un vertice al di sotto della crosta, e si scioglie sopra la superficie (12 ruote di vicinato medio, si scioglierà) in modo che le verticali vicine si muovano allo stesso modo. **osservato** (Il piccolo LeomaIl 39% delle verticali si muove, a 1,3 cm in media e a 9,5 cm in più.

<a id="bust"></a>
## Autobus

Il bust del corpo femminile è elevato al bust del uomo femminile (**osservato**• Il Leoma e Crevan Le donne hanno messo i loro 2.0 e 1.75 unità in basso, sotto il petto del vestito e il bust della parte della mappa). Il busto è il veto medio del veto quadrato, tra `Spine1` e le spalle e 0.3 a 1.8 unità accanto alla linea media, il cui vertice è più stretto avanti. Ogni vertice aumenta per la differenza di tempo il suo peso torso-bone, tempo exp(-(dy / 1.5)2) della sua altezza dal petto, tempo un passo liscio dalla profondità della spina (0) al petto (1), quindi la schiena, le braccia e le gambe rimangono. E poi la rilassanza.

<a id="human-only-bones"></a>
## Gli ossa umani

Il femminile scheletro ha `L Boob` e `R Boob` (la dimensione del seno dell'editore, l'altezza e la scala dello spazio e spostarli, il motore docs/animation.mdche manca un rifiuto di Biped. Dopo la perdita di peso, ogni vertice il cui osso più pesante è `Spine1` o `Spine2` Prende i pesi umani del vertice umano più vicino che ha alcuni, all'interno di 1 unità, scalati per 1 distanza; i suoi pesi si ridurranno per creare spazio. **osservato** (`human_female.mesh`Le verticali del bust sono portate dalle due ossa (892 delle 994 verticali frontali). I capelli (`L Boob` 28, `R Boob` 29) e le altre maniche del rig femminile differiscono dal maschile (`R Clavicle` 23 contro 25; il maschio ha `Bip01 Jaw`In questo modo, le messe sono sempre scritte contro le maniche del proprio rig.

<a id="part-map"></a>
## Parte mappa

La corsa è `part map male`/`female` PNG I segni di cui il corpo fa parte, a cui ogni vertice appartiene; oggetti indossati' `hide parts` smettere di disegnare queste parti (motore docs/characters.md “Le parti del corpo nascosto”. L’instrumento dà a ciascun vertice la parte del vertice più vicino del corpo umano portato dal medesimo osso, letto dalla parte del rig su quella vertice, e scrive su quella stessa vertice (1024 × 1024), altri tessili trasparenti. Ogni parte umana è cresciuta con 0,8 unità (un vertice senza una parte prende quella del vertice più vicino in quella distanza), poiché un corpo più grande si affaccia su una banda più ampia.

**osservato** (`MaleMask_Default.png`, 2562, mostrato in `human_male.mesh` UV come il gioco li mostra: 642 delle orme hanno una parte: il rosso del petto e le spalle, le bandiere bianche intorno al braccio superiore, il verde della spalla. `hide parts` in base ai dati (rebirth.mod): Drifter's and Padded Leather Jacket 1, Sleeveless Longcoat e Dustcoat 2, Ninja Gi e Rags 4, abiti 5, abiti di pelle e Crab Armour 7; Sacro Servente Rags, Monk Pants e Hoods 0. Gli articoli con 0 non nascondono nulla; la perdita di peso copre quelli. **osservato** (`FemaleMask_Default.png` a `human_female.mesh` UV: il busto rosso (1,102 verticali), bianco le bandi superiori (560), verde il ventre (250).

<a id="textures"></a>
## Testimonialità

- `_d`: colore di base, alfa = gloss = (255 - rigidità) / 2 ( rigidità 200 senza una mappa); 20482, BC3 con mips.
- `_n`: layout dei personaggi del gioco, X in alfa e Y in verde, Y scappato da glTFS + Y in alto; rosso e blu 0.
