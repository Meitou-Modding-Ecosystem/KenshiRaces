<a id="preparing-a-model"></a>
# Preparazione di un modello

Come ottenere un `.glb` generato ( Meshy , Hi3D , Blender ) a un numero di poligoni, dimensioni della trama e dimensioni del mondo reale gestite dal gioco, prima che entri in `<Mod>/Structures/` (edifici) o `<Mod>/ItemModels/` (oggetti). L'importazione stessa: [structures.md](structures.md) .

I comandi vengono eseguiti dalla radice del repository in PowerShell. Richiede Python 3; `prepare-model.py` necessita anche di `pip install pillow numpy` .

<a id="1-inspect"></a>
## 1. Ispezionare

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Stampa la dimensione in metri (X, Y su, Z), il lato più lungo, i triangoli, i materiali e la dimensione in pixel di ciascuna texture e MB . Una nota sulla scala o rotazione del nodo significa che `prepare-model.py` non può ridimensionarlo: esportalo di nuovo da Blender con le trasformazioni applicate (Oggetto > Applica > Tutte le trasformazioni).

<a id="2-cut-the-polycount"></a>
## 2. Taglia il poligono

| Tipo | Triangoli | Trame |
| --- | --- | --- |
| Edificio o decorazione | Da 5.000 a 20.000 | 2048 |
| Oggetto (tenuto, lasciato cadere, in un negozio) | sotto i 5.000 | 1024 |

I generatori spesso ne danno 100.000 o più; l'import avverte sopra i 150.000. Per tagliarlo:

- ** Meshy :** Rimesh il modello con un numero di poligoni target nel budget sopra (funzionano entrambe le topologie quadruple o triangolari), mantenendo le texture, e scaricalo di nuovo come GLB .
- ** Blender :** File > Importa > glTF, seleziona la mesh, aggiungi un modificatore Decimate (Comprimi) e abbassa il rapporto finché il conteggio dei volti nella barra di stato non rientra nel budget, applicalo, quindi File > Esporta > glTF Binario ( `.glb` ) con Applica modificatori attivato.

La collisione è semplificata separatamente ( `collisionTriangles` , default 2000), quindi il taglio della mesh visibile riguarda la frequenza dei fotogrammi e la dimensione del file.

<a id="3-choose-the-size"></a>
## 3. Scegli la taglia

L'importazione tratta il modello come metri; la dimensione di un generatore è arbitraria. Scegli la dimensione reale e impostala nel file delle impostazioni del modello ([structures.md "Settings"](structures.md#settings) ), non nel modello:

- Edificio: `"height"` in metri in `<model>.json` .  Le persone di Kenshi sono alte circa 1,8 m e leggono in piccolo rispetto alle dimensioni del mondo reale, quindi prendi la dimensione che descriveresti e aggiungi il 20% (un post di 2,2 m diventa `2.64` ).
- Articolo: `"size"` , il suo lato più lungo in metri, in `<model>.json` accanto ad esso in `ItemModels/` .

<a id="4-shrink-the-textures"></a>
## 4. Riduci le trame

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Questo riscrive il file sul posto con ogni texture al massimo di un numero di pixel per lato (2048 per gli edifici, 1024 per gli oggetti), solitamente tagliandolo da decine di MB a pochi. Un terzo argomento opzionale ridimensiona anche il modello al lato più lungo in metri, per quando non si desidera un file di impostazioni. L'importazione dimezza anche le texture più grandi ( `textureSize` ), ma il commit del file ridotto mantiene il repository piccolo.

Eseguire nuovamente `model-info.py` per verificare il risultato.

<a id="5-import-and-check"></a>
## 5. Importa e controlla

Costruisci e distribuisci con Play (o esegui l'importazione da solo, [structures.md "Quick start"](structures.md#quick-start) ). Il registro ha una riga per modello:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Controlla l'altezza o la lunghezza e il numero dei triangoli, poi guardalo nel gioco: posiziona l'edificio, oppure lascia cadere e raccogli l'oggetto.

<a id="6-commit"></a>
## 6. Impegnarsi

Impegna insieme `.glb` , il suo `.json` e lo `ids.json` della cartella. `ids.json` mantiene fissi gli ID record di ciascun modello e salva gli edifici posizionati con nome in base a tali ID.
