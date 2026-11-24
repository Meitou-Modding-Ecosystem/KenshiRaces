<a id="preparing-a-model"></a>
# Preparare un modello

Come ottenere una generazione `.glb` (Meshy, Hi3D, Blender) a un policonto, dimensioni di tessuto e dimensioni reali del gioco, prima di entrare `<Mod>/Structures/` (e le costruzioni o `<Mod>/ItemModels/` e gli elementi. L’importazione stessa: [structures.md](structures.md).

I comandi sono eseguiti dalla radice repo in PowerShell. La necessità di Python 3 `prepare-model.py` Anche il bisogno `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. Il controllo

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

La dimensione viene stampata in metri (X, Y up, Z), il lato più lungo, i materiali e la dimensione del pixel di ciascuna tesi e MB. Una nota sulla scala di nodi o il significato di rotazione `prepare-model.py` Non può essere distribuito: esportarlo di nuovo da Blender con trasformazioni applicate (Object > Apply > All Transforms).

<a id="2-cut-the-polycount"></a>
## 2. Tagliare il poligono

| Il bene | Il Triangolo | Testimonialità |
| --- | --- | --- |
| Costruzione o decorazione | da 5.000 a 20.000 | 2048 |
| Prodotto (trattato, caduto, in un negozio) | Sotto i 5.000 | 1024 |

I generatori spesso danno 100.000 o più; l'importazione avverte oltre 150.000. Per tagliarlo:

- **Meshy:** Rimuovere il modello con un obiettivo nel bilancio sopra (quad o triangolo funziona entrambi), mantenere le tesi, e
Scarica come GLB di nuovo.
- **di Blender:** File > Importazione glTF, selezionare la mezza, aggiungere un decimato modificatore (Collapse) e ridurre la Ratio fino a quando il numero facciale nel status
La barra è in bilancio, applicarla, quindi File > Export > glTF Il binario (`.glb`1) Applicare i Modificatori su.

Il processo di incisione è individuato in modo semplice (`collisionTriangles`, default 2000), quindi tagliare la maschera visibile è circa la velocità del frame e la dimensione del file.

<a id="3-choose-the-size"></a>
## 3. Scegli la dimensione

L'importazione tratta il modello come metri; la dimensione di un generatore è arbitraria. Pieta la dimensione reale e impostarla nel file di impostazioni del modello ([Scrivi una recensione per "Settings"](structures.md#settings)Non nel modello:

- La costruzione: `"height"` In metri di `<model>.json`. KenshiLe persone sono di circa 1,8 m di altezza e leggono piccole al di là delle dimensioni del mondo reale, quindi prendere il
dimensioni che descriveresti e aggiungi il 20% (un post di 2,2 m diventa `2.64`).
- Il tema: `"size"`Il suo lato più lungo in metri, in `<model>.json` Oltre a questo, in `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. Raccogliere i tessuti

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Questo rivela il file con ogni tessuto al più grande (2048 per gli edifici, 1024 per gli articoli), di solito tagliato da decine di MB. Un terzo argomento opzionale rilascia anche il modello a quel lato più lungo in metri, per quando un file di impostazione non è desiderato. Anche la produzione di prodotti per l’acqua è più grande (`textureSize`Ma commettere il file tagliato mantiene il repo piccolo.

corsa `model-info.py` Per controllare nuovamente il risultato.

<a id="5-import-and-check"></a>
## 5. Importare e controllare

Costruisci e distribuisci con Play (o gestisci l'importazione da solo, [Home » Curiosità » Quick Start](structures.md#quick-start)). Il log ha una linea per modello:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Verificare l'altezza o la lunghezza e il conteggio del triangolo, quindi guardare nel gioco: posizionare l'edificio, o scendere e prendere l'articolo.

<a id="6-commit"></a>
## 6. Comitato

Commette il `.glb`Il suo `.json` e il folder `ids.json` Insieme `ids.json` Mantenere i dati di registrazione di ciascun modello fisso, e salvare i nomi degli edifici posizionati da questi dati.
