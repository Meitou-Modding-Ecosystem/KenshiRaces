<a id="structures-from-3d-models"></a>
# Strukturen aus 3D-Modellen

Legen Sie ein `.glb` -Modell ( Meshy , Hi3D , ein Blender -Export) in den `Structures` -Ordner Ihres Mods und der Play-Build verwandelt es in ein Gebäude, das Sie über das Build-Menü platzieren können: Ogre-Mesh, komprimierte Texturen, Kollision und die Datensätze in `<Mod>.mod` . Kein FCS , kein Blender , keine Spielinstallation für den Import selbst erforderlich.

<a id="quick-start"></a>
## Schnellstart

1. Legen Sie das Modell in `<Mod>/Structures/` ab, z.B. `MyMod/Structures/watch-tower.glb` .
2. Optional: daneben `watch-tower.json` mit beliebigen [Einstellungen](#settings) , z.B.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Erstellen und bereitstellen Sie wie gewohnt ([play.md](play.md) ): Visual Studio F5, VS Code F5 oder `dotnet run --project Play` . Das Build-Protokoll zeigt eine `Structure ...` -Zeile pro Modell.
4. Im Spiel: Build-Menü, Kategorie `EXTERIOR` (oder die von Ihnen festgelegte). Mit dem Standard- `"research": "start"` ist es vom Start eines Spiels an bekannt, und auch in vorhandenen Speicherungen ( Meitou berechnet Freischaltungen aus der abgeschlossenen Forschung, die immer `_Default Start` umfasst).
5. Übernehmen Sie das Modell, seine `.json` und `Structures/ids.json` (beim ersten Import geschrieben). Die generierten Dateien sind Build-Ausgaben und werden nicht festgeschrieben.

Ohne Spiel: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>` .

<a id="settings"></a>
## Einstellungen

Alles optional ( `<model>.json` , Kommentare erlaubt; ein unbekannter Name ist ein Fehler, sodass Tippfehler nicht unbemerkt bleiben).

| Einstellung | Standard | Bedeutung |
| --- | --- | --- |
| `name` | Dateiname, `-` / `_` als Leerzeichen | Namen im Build-Menü |
| `description` | `""` | Menütext erstellen |
| `height` | Eigengröße des Models | Fertighöhe in Metern; das Modell wird gleichmäßig darauf skaliert. AI -Modelle gibt es in beliebigen Größen, also stellen Sie diese ein |
| `sink` | `0` | Meter im Boden versenkt (verhindert das Aufschwimmen an Hängen) |
| `yaw` | `0` | Grad um die vertikale Achse gedreht |
| `category` | `EXTERIOR` | Build-Menükategorie ( `building category` ): das Spiel ( `BUILDINGS` , `CAMPING` , `FARMING` , ...) oder ein neuer Name |
| `group` | `""` | `building group` : Gebäude, die sich einen Eintrag teilen, sind Varianten eines Eintrags |
| `materials` | `5` | Baumaterialien es kostet |
| `buildSpeed` | `2` | `build speed mult` (die Zelte 5 des Spiels, die Häuser 1 bis 2) |
| `research` | `start` | `start` : von Anfang an bekannt; `none` : nichts entsperrt es (Ihre eigenen Aufzeichnungen schon); oder eine RESEARCH -String-ID, z. B. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor` , `indoor` oder `both` |
| `createsTown` | `true` | Wenn man es außerhalb einer Stadt platziert, entsteht ein Außenposten |
| `collision` | `mesh` | `mesh` (eine vereinfachte Kopie des Modells), `box` (sein Begrenzungsrahmen) oder `none` (durchgehen) |
| `collisionTriangles` | `2000` | die meisten Dreiecke behält die `mesh` -Kollision bei (12 bis 50.000) |
| `textureSize` | `2048` | größte Texturseite in Pixel (eine Zweierpotenz, höchstens 8192) |

<a id="item-models"></a>
## Artikelmodelle

Fügen Sie stattdessen das Modell eines Gegenstands in `<Mod>/ItemModels/` ein ( `Structures` würde daraus ein Gebäude machen). Der Import schreibt sein Netz und seine Texturen neben die Strukturen und einen MATERIAL_SPECS_CLOTHING -Datensatz. Der ITEM -Datensatz Ihres Mods zeigt dann auf sie. Das Build-Protokoll zeigt eine `Item ...` -Zeile pro Modell mit den zu verwendenden Namen.

1. `MyMod/ItemModels/lantern.glb` , optional `lantern.json` : `{ "size": 0.3 }` .
2. Legen Sie in Ihrem Artikeldatensatz (Datengenerator oder Datenphase) Folgendes fest:

| Feld | Wert |
   | --- | --- |
| `mesh` und `ground mesh` (Dateinamen) | `<mod>_<model>.mesh` , z.B. `mymod_lantern.mesh` | | `material` (Referenz) | `<950000 + 100 × block>-<Mod>.mod` , Block aus `ItemModels/ids.json` , z.B. `950000-MyMod.mod` |

Schlagen Sie zuerst das Material nach ( `GameDatabase.Find` ) und behalten Sie das Aussehen des Ersatzes bei, wenn es fehlt: Eine veröffentlichte Zip-Datei und Tests, die das Spiel ohne den bereitgestellten Mod lesen, sind nicht vorhanden.
3. Übernehmen Sie das Modell, seine `.json` und `ItemModels/ids.json` .

| Einstellung | Standard | Bedeutung |
| --- | --- | --- |
| `name` | Dateiname, `-` / `_` als Leerzeichen | der Name des Materialdatensatzes |
| `size` | Eigengröße des Models | längste Seite in Metern; das Modell wird gleichmäßig darauf skaliert |
| `yaw` | `0` | Grad um die vertikale Achse gedreht |
| `textureSize` | `1024` | größte Texturseite in Pixel (eine Zweierpotenz, höchstens 8192) |

- Ein Material pro Artikelmodell (der ITEM nennt einen `material` ); Das Modell steht auf seinem tiefsten Punkt, zentriert, wie es bei den Item-Meshs des Spiels der Fall ist.
- Das Spiel zeichnet einen Gegenstand auf dem Boden, in einer Hand und in einem Auto-Symbol mit seinem `mesh` ( `ground mesh` zuerst auf dem Boden) und dem MATERIAL_SPECS_CLOTHING mit seinem `material` -Namen (Motor `docs/formats/runtime-materials.md` „Anrufer“, `WorldObjectLayout.ItemMesh` ).
- Das Inventarsymbol wird aus dem Modell gezogen, es sei denn, der Datensatz setzt `auto icon image` auf „false“ und seinen eigenen `icon` .

<a id="getting-a-good-model"></a>
## Ein gutes Modell bekommen

- Fragen Sie Meshy oder Hi3D nach einem PBR -texturierten Modell und laden Sie es als GLB herunter. Ein paar Zehntausend Dreiecke sind ausreichend; Das Protokoll markiert ein Modell mit mehr als 150.000. Polycount reduzieren, Größe auswählen und Texturen verkleinern: [model-prep.md](model-prep.md) .
- Die Vorderseite des Modells zeigt in der Datei auf +Z (Konvention von glTF); Verwenden Sie `yaw` , um es zu drehen.
- Jedes Material wird zu einem eigenen Teil. Generatoren geben normalerweise einen ab; höchstens 49.
- Das Modell steht auf seinem tiefsten Punkt, zentriert.

<a id="what-it-writes"></a>
## Was es schreibt

In den bereitgestellten Mod-Ordner ( `<Kenshi>/mods/<Mod>/` ):

- `structures/<mod>_<model>.mesh` (einer pro Material, `_1` , `_2` , ... wenn mehrere): Ogre `[MeshSerializer_v1.8]` , Position, Normal, UV und Tangente mit Händigkeit in w, das Layout der spieleigenen Gebäudenetze.
- `structures/<mod>_<model>_d.dds` , `_n.dds` , `_m.dds` : diffus (BC3), normal (BC1, BC3 beim Ausschneiden), Metall (BC1), mit Mips.
- `structures/<mod>_<model>_col.xml` : die Kollision.
- Für ein Artikelmodell: `structures/<mod>_<model>.mesh` und seine Texturen, keine Kollision.
- In `<Mod>.mod` zusammengeführte Datensätze neben allem anderen darin (die Datensätze eines Datengenerators bleiben erhalten; die eines früheren Imports werden ersetzt): ein BUILDING , ein BUILDING_PART und MATERIAL_SPEC pro Material und die Freischaltung (der `enable buildings` der Forschung erhält das Gebäude); ein MATERIAL_SPECS_CLOTHING pro Artikelmodell.

Dateinamen beginnen mit dem Namen des Mods, da das Spiel in jedem Mod nach Ressourcen anhand des bloßen Dateinamens sucht.

Datensatz-IDs sind `<n>-<Mod>.mod` mit n von 900000 aufwärts, ein Block von 100 pro Modell (Gebäude +0, Teile +1, Materialien +50); Artikelmodelle zählen ab 950000 (Material +0) in `ItemModels/ids.json` . `Structures/ids.json` behält den Block jedes Modells bei, sodass sich die IDs nie ändern: Ein Speicherort benennt ein platziertes Gebäude anhand der ID seines Datensatzes. Bearbeiten oder entfernen Sie niemals einen Block; Durch das Entfernen eines Modells wird sein Gebäude aus den Spielen entfernt, in denen es platziert wurde.

<a id="facts-the-import-relies-on"></a>
## Fakten, auf die sich der Import stützt

Mit den Dateien des Spiels ( Kenshi 1.0.68-Daten) und mit den Lesegeräten der Engine verglichen:

- **Einheiten**: Die Welteinheit ist 0,1 m, Y aufwärts; glTF ist Meter, Y nach oben, daher beträgt die Skala 10 (die Kollisionsdateien des Spiels verwenden die Schwerkraft 98,1).
- **Materialien**: Das Aussehen eines Gebäudeteils ist sein MATERIAL_SPEC ( `texture map` , `normal map` , `metalness map` , `material type` ), nicht der Materialname des Netzes (Motor `docs/formats/runtime-materials.md` ). Der Gebäude-Shader des Spiels ( `data/materials/deferred/objects.hlsl` ) liest diffuses RGB als Farbe und A als Glanz (mal `specular mult` ), normales RGB als Tangentenraumnormale und A als Ausschnittmaske ( `material type` 1, `alpha threshold` ), Metallheit aus R. Der Import schreibt Glanz = 1 – glTF-Rauheit und Metallheit aus dem blauen Kanal von glTF; glTF MASK und BLEND werden zum Ausschnitt.
- **Tangenten**: Die Binormale des Shaders ist Kreuz(Tangente, Normal) × w, und auf den Netzen des Spiels zeigt sie entlang zunehmender V (**Verifiziert**: `prisoner_pole.mesh` 86 von 86 Dreiecken, `Camp_tent1.mesh` 1858 von 1865). glTF-Normalkarten zeigen grün nach oben im Bild (verringerndes V), sodass der Import den grünen Kanal umdreht und den Weg des Spiels festlegt.
- **Kollision**: `xml collision` eines Teils ist eine PhysX NxuStream2-Datei; Eine handgeschriebene minimale Datei lädt (das `Camp_tent1_COL.xml` des Spiels ist eine Box). PhysX ist Z hoch und das Spiel ordnet (x, y, z) auf (x, z, -y) zu, sodass der Import die Punkte des Teils als (x, -z, y) schreibt. Das Dreiecksnetz besteht aus einfachen Punkten und Dreiecken. Das Spiel kocht es unter Last, Meitou liest es so wie es ist (Engine `docs/formats/collision.md` ).
- **Felder**: BUILDING und BUILDING_PART verfügen über den gesamten Feldsatz des Campingzelts 1 ( `18026-small_changes_otto.mod` ) des Spiels, sodass nichts auf eine Standardeinstellung zurückfällt; kein `functionality` , also ein schlichtes Gebäude.
- **Freischalten**: Ein Gebäude befindet sich im Baumenü, sobald eine abgeschlossene Forschung es in `enable buildings` auflistet; Jedes Spiel beginnt mit `_Default Start` ( `5359-gamedata.base` ) und endet. **Verifiziert** mit den Engines `GameDatabase` , `ResearchGate` und `BuildingBuildData` gegenüber dem Basisspiel plus einem importierten Mod: Die Strukturen sind freigeschaltet und kosten Baumaterialien wie festgelegt.

<a id="not-covered-yet"></a>
## Noch nicht abgedeckt

Innenräume, Türen, Funktionen (Aufbewahrung, Betten, Basteln) und Drehteile.
