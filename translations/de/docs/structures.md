<a id="structures-from-3d-models"></a>
# Strukturen aus 3D-Modellen

Einstellen A `.glb` Das Modell (Meshy, Hi3D, ein Blender Export) in Ihrem Mod `Structures` Die Folge und das Spielbau verwandeln sie in ein Gebäude, das Sie aus dem Baumenü setzen können: Ogre-Mess, komprimierte Texturen, Kollision und die Aufzeichnungen `<Mod>.mod`. nicht FCS, keine Blender, keine Installation erforderlich für die Einfuhr selbst.

<a id="quick-start"></a>
## Schnell beginnt

1. Geben Sie das Modell `<Mod>/Structures/`und g. `MyMod/Structures/watch-tower.glb`.
2. Optionell: Neben ihm `watch-tower.json` Mit jedem [Einrichtungen](#settings)und g.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Erstellen und installieren wie gewöhnlich ([play.md](play.md)• Visual Studio F5, VS Code F5, oder `dotnet run --project Play`. Das Gebäude zeigt einen
`Structure ...` Linie pro Modell.
4. In dem Spiel: Bauen von Menüs, Kategorie `EXTERIOR` (oder dem, den du gesetzt hast) Mit dem Default `"research": "start"` Es ist seit Beginn des
Spiel, und in bestehenden Sparen auch (Meitou arbeitet aus der fertigen Forschung, die immer `_Default Start`).
5. Das Modell, sein `.json` und `Structures/ids.json` (Wie auf der ersten Einfuhr geschrieben) Die erzeugten Dateien erzeugen Ausgang und sind nicht
Verpflichtet ist.

Ohne Spiele: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## Die Settings

Alle Optionen (`<model>.json`, Kommentare erlaubt; ein unbekanntes Name ist ein Fehler, so dass Typos nicht unnachmerkt gehen).

| Setzen | Default | Bedeutung |
| --- | --- | --- |
| `name` | Datei Name , `-`/`_` als Räume | Name im Baumenü |
| `description` | `""` | Erstellen von Menü Text |
| `height` | Die eigene Größe des Modells | Fertighöhung in Metern; das Modell ist gleichmäßig zu ihm geschnitten. AI-Modelle kommen in arbiträren Größen, so setzen Sie dies |
| `sink` | `0` | Meter in den Boden fallen (schützt es vor, auf Schlössern zu schwimmen) |
| `yaw` | `0` | Grade drehen sich um die vertikale Axe |
| `category` | `EXTERIOR` | Die Liste der Menü (`building category`Das Spiel ist (`BUILDINGS`, `CAMPING`, `FARMING`... oder ein neuer Name |
| `group` | `""` | `building group`: Gebäude teilen ein sind Varianten eines Eingangs |
| `materials` | `5` | Bauausgaben kostet es |
| `buildSpeed` | `2` | `build speed mult` (Spielstände 5, Häuser 1 bis 2) |
| `research` | `start` | `start`: von Anfang an bekannt; `none`Nichts wird es (eine eigene Aufzeichnung macht) enthüllen oder eine RESEARCH String ID und e.g. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` oder `both` |
| `createsTown` | `true` | Die Einrichtung außerhalb einer Stadt beginnt mit einem Outpost. |
| `collision` | `mesh` | `mesh` (eine vereinfachte Kopie des Modells) `box` (Sie sind die Grenzbox) oder `none` (Sie gehen durch sie) |
| `collisionTriangles` | `2000` | Die meisten Dreieckstücke `mesh` Kollision hält (12 bis 50 000) |
| `textureSize` | `2048` | größte Texturseite in Pixeln (eine Leistung von zwei, bei maximal 8192) |

<a id="item-models"></a>
## Artikel Modelle

Ein Modell eines Objekts in `<Mod>/ItemModels/` Stattdessen (`Structures` Das würde ein Gebäude machen). Der Einfuhr schreibt seine Messe und seine Texturen neben den Strukturen und MATERIAL_SPECS_CLOTHING Aufzeichnung: Ihr Mod ITEM Die Aufzeichnungen weisen darauf hin. Das Gebäude zeigt einen `Item ...` Line per Model mit den Namen zu verwenden.

1. `MyMod/ItemModels/lantern.glb`Optionell `lantern.json`: `{ "size": 0.3 }`.
2. In Ihrem Element-Register (Datengenerator oder Datenphase) setzen Sie fest:

Das ist die Wertschätzung des
   | --- | --- |
| `mesh` und `ground mesh` (Filenamen) Die `<mod>_<model>.mesh`und g. `mymod_lantern.mesh` | | `material` (Referenz) Die `<950000 + 100 × block>-<Mod>.mod`Block von `ItemModels/ids.json`und g. `950000-MyMod.mod` |

Schauen Sie sich das Material zuerst an (`GameDatabase.Find`Und halten Sie den Blick, wenn es fehlt: eine freie Zip, und Tests, die das Spiel ohne die eingesetzte Modus lesen, haben Sie es nicht.
3. Das Modell, sein `.json` und `ItemModels/ids.json`.

| Setzen | Default | Bedeutung |
| --- | --- | --- |
| `name` | Datei Name , `-`/`_` als Räume | Der Name des Materialregisteres |
| `size` | Die eigene Größe des Modells | die längste Seite in Metern; das Modell ist gleichmäßig zu ihm skaliert |
| `yaw` | `0` | Grade drehen sich um die vertikale Axe |
| `textureSize` | `1024` | größte Texturseite in Pixeln (eine Leistung von zwei, bei maximal 8192) |

- Ein Material pro Element (ein ITEM Namen 1 `material`); das Modell steht auf seinem niedrigsten Punkt, zentralisiert, wie das Spiel-Objekt-Messen tun.
- Das Spiel zieht ein Element auf den Boden, in einer Hand und in einem Auto-Ikon mit `mesh` (`ground mesh` Erst auf der Erde) und
MATERIAL_SPECS_CLOTHING Seine `material` Die Namen des Motors `docs/formats/runtime-materials.md` „Kallern“ `WorldObjectLayout.ItemMesh`).
- Die Inventory-Ikon wird vom Modell abgeholt, es sei denn, die Aufzeichnungen `auto icon image` Falsch und seine eigene `icon`.

<a id="getting-a-good-model"></a>
## Ein gutes Modell zu bekommen

- Fragen Sie Meshy oder Hi3D Für A PBR- texturierte Modelle und herunterladen als GLB. Ein paar Dutzende von Tausenden von Dreieckern sind reich; die Log-Flagge sind ein Modell
Über 150.000 Schneiden Sie das Polycount, wählen Sie die Größe und verkürzen Sie die Texturen: [model-prep.md](model-prep.md).
- Das Vordergrund des Modells zeigt +Z in der Datei (glTFÜbereinkommen); Verwendung `yaw` Um es zu drehen.
- Jedes Material wird zu seinem eigenen Teil. Generatoren geben in der Regel eine; bei maximal 49.
- Das Modell steht auf seinem niedrigsten Punkt, zentralisiert.

<a id="what-it-writes"></a>
## Was es schreibt

In der eingesetzten Mod-Folder (`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` (ein für das Material, `_1`, `_2`, ... wenn mehrere): Ogre `[MeshSerializer_v1.8]`, Position, normal, UV und
tangent mit handedness in w, die layout des spiels eigener baum.
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: diffuse (BC3), normal (BC1, BC3 wenn abgeschnitten), metallische (BC1), mit Mips.
- `structures/<mod>_<model>_col.xml`Die Kollision ist.
- Für ein Element Modell: `structures/<mod>_<model>.mesh` und seine Texturen, keine Kollision.
- Die Aufzeichnungen werden eingeführt `<Mod>.mod` Neben dem, was in ihm enthalten wird, werden die Aufzeichnungen eines Datengenerators gespeichert; eine frühere Import wird ersetzt:
BUILDING, eine BUILDING_PART und MATERIAL_SPEC Das Material und die Unlock (die Forschung) `enable buildings` Das Gebäude gewinnt; eine MATERIAL_SPECS_CLOTHING Nach dem Modell.

Die Dateinamen beginnen mit dem Namen des Mods, da das Spiel Ressourcen durch den Dateinamen über jedes Mods zeigt.

Die Rekord-IDs sind `<n>-<Mod>.mod` mit n von 900000 bis m m m m, m m m und m m m ein Blok von 100 pro Modell (Bau +0, Teile +1, Materialien +50); Artikelmodelle zählen von 950000 (Material +0) in `ItemModels/ids.json`. `Structures/ids.json` hält den Block jedes Modells so, dass sich die IDs nie ändern: ein Speicher nennt ein gepostetes Gebäude durch das ID seines Rekords. Keine Bearbeitung oder Entfernung eines Blocks; das Entfernen eines Modells entfernt sein Gebäude aus den Spielen, die es platziert.

<a id="facts-the-import-relies-on"></a>
## Der Einfuhr beruht auf

Die Datei des Spiels werden überprüft (Kenshi 1.0.68 Daten) und mit den Lesern des Motors:

- **Einheiten**: die Welt-Einheit ist 0,1 m, Y up; glTF ist Meter, Y auf, so dass die Skala 10 ist (die Kollision-Dateien des Spiels verwenden die Gravitation 98.1).
- **Materialien**Der Ausblick eines Gebäudes ist sein MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`Nicht die Messe
Material Name (Motor) `docs/formats/runtime-materials.md`). Das Spiel der Schatten (`data/materials/deferred/objects.hlsl`Lesen diffuse RGB als Farbe und A als Gloss (Times) `specular mult`• Normaler RGB als die tangent-space normal und a als die cut-out mask (`material type` 1, `alpha threshold`) Metall aus R. Der Import schreibt gloss = 1 - glTF Schmutz und Metallität von glTFder blauen Kanal; glTF MASK und BLEND Es wird ein Cut-out.
- **Tangentum**: der Schwerpunkt des Schaches binormal ist cross(Tangent, normal) × w, und auf den Messen des Spiels zeigt es entlang der Erhöhung V (**Überprüft**:
`prisoner_pole.mesh` 86 von 86 Dreieck `Camp_tent1.mesh` 1858 und 1865). glTF Normale Karten grünen Bild (verringert V), so dass die Einfuhr der grünen Kanal dreht und die Art des Spiels festlegt.
- **Kollision**: ein Teil der `xml collision` ist eine PhysX NxuStream2 Datei; eine manuell geschriebene minimale Datei-Ladung (die `Camp_tent1_COL.xml` Es ist
Ein Kasten) PhysX ist Z up und die Spielkarte (x, y, z) zu (x, z, -y), so dass der Import die Punkte des Teils als (x, -z, y) schreibt. Der Dreieck geht wie reine Punkte und Dreieck; das Spiel kocht es auf, Meitou las es wie es ist (Motor) `docs/formats/collision.md`).
- **Felder**: BUILDING und BUILDING_PART mit dem vollständigen Feldset des Spiels Camping Tint 1 (`18026-small_changes_otto.mod`So nichts
Es ist ein Verfall; nicht `functionality`Es ist also ein reines Gebäude.
- **Unlockung**: ein Gebäude befindet sich im Baumenü, sobald eine fertige Forschung es in `enable buildings`Jedes Spiel beginnt mit
`_Default Start` (`5359-gamedata.base`) abgeschlossen ist. **Überprüft** Mit dem Motoren `GameDatabase`, `ResearchGate` und `BuildingBuildData` Über das Basisspiel sowie einen importierten Modus: Die Strukturen sind unbegrenzt und die Kosten für die Baumausrüstung.

<a id="not-covered-yet"></a>
## noch nicht abgedeckt

Innenräume, Türen, Funktionen (Speicher, Betten, Werkstatt) und Drehpartikel.
