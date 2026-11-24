<a id="preparing-a-model"></a>
# Vorbereitung eines Modells

Wie man eine generierte `.glb` (Meshy, Hi3D, Blender) zu einem Polycount, Texturgröße und realen Spielgröße, bevor es in `<Mod>/Structures/` Gebäude oder `<Mod>/ItemModels/` und das Thema. Die Einfuhr selbst: [structures.md](structures.md).

Die Befehlskräfte werden von der Repo-Reihe in PowerShell ausgeführt. Wir brauchen Python 3 `prepare-model.py` Auch benötigen Sie `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. Inspektion

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Es druckt die Größe in Metern (X, Y Up, Z), die längste Seite, Dreieck, und die Pixelgröße jeder Textur. Ein Hinweis auf die Node-Skala oder Rotation bedeutet `prepare-model.py` Es wird nicht aufgebaut: Es wird wieder aus Blender mit angewandten Transformationen exportiert (Object > Apply > All Transforms).

<a id="2-cut-the-polycount"></a>
## 2. Das Polycount abschneiden

| gut | Triangeln | Texturen |
| --- | --- | --- |
| Gebäude oder Dekoration | 5.000 bis 20.000 | 2048 |
| Artikel (gehalten, fallen, in einem Laden) | Unter 5000 | 1024 |

Generatoren geben oft 100.000 oder mehr; die Einfuhr warnt über 150.000. Um es zu schneiden:

- **Meshy:** Das Modell mit einem Ziel in dem oben genannten Budget (Quad oder Triangle-Topologie) beibehalten, und
Laden Sie es als GLB Wieder einmal.
- **Das Blender:** Datei > Import glTF, wählen Sie die Messe aus, fügen Sie einen Decimate modifierer (Collapse) hinzu und senken Sie den Ratio, bis das Gesicht im Status zählt
Bar ist im Budget, applieren Sie es, dann Datei > Export > glTF Das binäre (`.glb`mit Anwendung von Modifikatoren auf.

Die Anzahl der Anlagen wird separat vereinbart (`collisionTriangles`, Standard 2000), so dass die sichtbare Messe um die Frame und Dateigröße.

<a id="3-choose-the-size"></a>
## 3. Wählen Sie die Größe

Der Einfuhr behandelt das Modell als Meter; die Größe eines Generators ist arbitrarisch. Wählen Sie die tatsächliche Größe und setzen Sie sie in die Einstellungsdatei des Modells ([Structures.md „Einstellungen“](structures.md#settings)Und nicht im Modell:

- Das Gebäude: `"height"` In Meter in `<model>.json`. KenshiDie Menschen sind etwa 1,8 m hoch und lesen kleine neben der realen Größe, so nehmen Sie die
Größe, die Sie beschreiben und hinzufügen 20% (ein 2.2 m Post wird `2.64`).
- Gegenstand : `"size"`Die längste Seite in Metern, in `<model>.json` Neben ihm im `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. Die Texturen schneiden

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Dies wiederholt die Datei mit jeder Textur, die viele Pixel auf der Seite (2048 für Gebäude, 1024 für Objekte) abschneidet. Ein optionales drittes Argument schlägt auch auf die längste Seite in Metern, wenn ein Setup-Datei nicht gewünscht ist. Auch die Verwendung von Texturen ist halb größer (`textureSize`, aber die Schrunk-Datei veranstaltet, hält die Repo klein.

Rennen `model-info.py` Wieder einmal das Ergebnis zu überprüfen.

<a id="5-import-and-check"></a>
## 5. Import und Check

Bauen und verwalten Sie mit Play (oder führen Sie die Einfuhr allein, [Startseite » Quick Start](structures.md#quick-start)). Der Log hat eine Linie pro Modell:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Überprüfen Sie die Höhe oder die Länge und die Dreieckzahl, dann sehen Sie es im Spiel: Platzieren Sie das Gebäude, oder fallen Sie und holen Sie das Element.

<a id="6-commit"></a>
## 6. Kompromiss

Verpflichtet Sie sich `.glb`Und seine `.json` Und die Folder `ids.json` Gemeinsam zu sein. `ids.json` hält die Rekord-IDs jedes Modells fest, und speichert die Namen von Gebäuden, die von diesen IDs gelegt werden.
