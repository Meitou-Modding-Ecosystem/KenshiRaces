<a id="preparing-a-model"></a>
# Vorbereiten eines Modells

Wie man ein generiertes `.glb` ( Meshy , Hi3D , Blender ) auf eine Polyanzahl, Texturgröße und reale Größe bringt, die das Spiel verarbeitet, bevor es in `<Mod>/Structures/` (Gebäude) oder `<Mod>/ItemModels/` (Gegenstände) geht. Der Import selbst: [structures.md](structures.md) .

Befehle werden vom Repo-Stammverzeichnis in PowerShell ausgeführt. Benötigt Python 3; `prepare-model.py` benötigt auch `pip install pillow numpy` .

<a id="1-inspect"></a>
## 1. Inspizieren

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Es druckt die Größe in Metern (X, Y nach oben, Z), die längste Seite, Dreiecke, Materialien sowie die Pixelgröße und MB jeder Textur. Ein Hinweis zur Knotenskalierung oder -rotation bedeutet, dass `prepare-model.py` ihn nicht neu skalieren kann: Exportieren Sie ihn erneut aus Blender mit angewendeten Transformationen (Objekt > Anwenden > Alle Transformationen).

<a id="2-cut-the-polycount"></a>
## 2. Schneiden Sie den Polycount ab

| Art | Dreiecke | Texturen |
| --- | --- | --- |
| Gebäude oder Dekoration | 5.000 bis 20.000 | 2048 |
| Artikel (gehalten, fallen gelassen, in einem Geschäft) | unter 5.000 | 1024 |

Generatoren liefern oft 100.000 oder mehr; Der Import warnt über 150.000. Um es zu schneiden:

- ** Meshy :** Vernetzen Sie das Modell erneut mit einer Ziel-Polyanzahl im oben genannten Budget (Quadrat- oder Dreieck-Topologie funktionieren beide), behalten Sie die Texturen bei und laden Sie es erneut als GLB herunter.
- ** Blender :** Datei > Importieren > glTF, wählen Sie das Netz aus, fügen Sie einen Dezimierungsmodifikator (Reduzieren) hinzu und verringern Sie das Verhältnis, bis die Flächenanzahl in der Statusleiste im Budget liegt, wenden Sie es an, dann Datei > Exportieren > glTF-Binär ( `.glb` ) mit aktivierter Option „Modifikatoren anwenden“.

Die Kollision wird separat vereinfacht ( `collisionTriangles` , Standard 2000), sodass es beim Schneiden des sichtbaren Netzes um Bildrate und Dateigröße geht.

<a id="3-choose-the-size"></a>
## 3. Wählen Sie die Größe

Der Import behandelt das Modell als Meter; Die Größe eines Generators ist beliebig. Wählen Sie die tatsächliche Größe und legen Sie sie in der Einstellungsdatei des Modells fest ([structures.md „Settings“](structures.md#settings) ), nicht im Modell:

- Gebäude: `"height"` in Metern in `<model>.json` .  Die Menschen von Kenshi sind etwa 1,8 m groß und fallen im Vergleich zu realen Größen kleiner aus. Nehmen Sie also die Größe, die Sie beschreiben würden, und addieren Sie 20 % (ein 2,2 m großer Pfosten wird zu `2.64` ).
- Artikel: `"size"` , seine längste Seite in Metern, in `<model>.json` daneben in `ItemModels/` .

<a id="4-shrink-the-textures"></a>
## 4. Verkleinern Sie die Texturen

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Dadurch wird die Datei an Ort und Stelle neu geschrieben, wobei jede Textur höchstens so viele Pixel pro Seite hat (2048 für Gebäude, 1024 für Gegenstände), wobei sie normalerweise von Dutzenden MB auf einige wenige reduziert wird. Ein optionales drittes Argument skaliert das Modell auch auf die längste Seite in Metern neu, wenn keine Einstellungsdatei erforderlich ist. Der Import halbiert auch größere Texturen ( `textureSize` ), aber durch das Festschreiben der verkleinerten Datei bleibt das Repo klein.

Führen Sie `model-info.py` erneut aus, um das Ergebnis zu überprüfen.

<a id="5-import-and-check"></a>
## 5. Importieren und prüfen

Erstellen und bereitstellen Sie mit Play (oder führen Sie den Import alleine aus, [structures.md „Schnellstart“](structures.md#quick-start) ). Das Protokoll enthält eine Zeile pro Modell:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Überprüfen Sie die Höhe oder Länge und die Anzahl der Dreiecke und sehen Sie sich diese dann im Spiel an: Platzieren Sie das Gebäude oder lassen Sie den Gegenstand fallen und nehmen Sie ihn auf.

<a id="6-commit"></a>
## 6. Commit

Übertragen Sie `.glb` , `.json` und `ids.json` des Ordners zusammen. `ids.json` behält die Datensatz-IDs jedes Modells bei und speichert namentlich platzierte Gebäude anhand dieser IDs.
