<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Race Import: Ein Körper auf das Skeleton des Spiels anpassen

`tools/RaceImport` Umwandeln Sie sich in eine Rygge `.glb` Der Körper (Meshy, Hi3D: der "3ds Max Biped" Reifen, Verbindungen `Bip001 ...`) in einen Körper das Spiel mit seinen eigenen menschlichen Animationen animatiert: eine geschnittene Ogre-Messe, die mit `male_skeleton.skeleton` oder `female_skeleton.skeleton`Und seine DDS Texturen und eine Karte. Es liest das Schelet, menschliche Körper und Teilkarte aus dem Spiel, so dass es auf einer Maschine mit Kenshinie in CI; seine Produktion ist verpflichtet.

<a id="run-it"></a>
## Laufen Sie es

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>`: das Spielfolder (`data/` Innerhalb des Tages, e.g. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- Die Stamme nennen die Dateien und müssen über Mods einzigartig sein (das Spiel findet Dateien nach bare Namen): `<modfolder lower case>_<race>`.
- `male` / `female`Der Rich, der default mann. Ein weibliches Körper ist für `female_skeleton` und gemessen gegen `human_female.mesh` und
`FemaleMask_Default.png` (Frauen-Animationen und Stimme im Spiel, die Bust-Slider des Redaktors); ein Mann, der `male_skeleton`, `human_male.mesh` und `MaleMask_Default.png`.
- `slim`0 bis 1 Standard 1 ([Schwächen](#slimming)• Niedriger für einen Rennen, der bulkier als seine Kleidung bedeutete.
- Schreiben `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (nur wenn das Modell eine normale Karte hat) und `<stem>_parts.png`Die Log-Linie gibt die
Vertex-Zahl, Skala, Grenzen und wie viele Vertikale in versteckbaren Teilen fallen.
- Halten Sie die `.glb` außerhalb des Repo (Tenzen von MB); nehmen Sie die vier Ausgänge ein. `.gitignore` Lass uns `*/items/races/*.mesh` und `*.dds` durch.
- Ein abhängiges Mod führt es aus einem KenshiRaces-Klone mit seiner eigenen Ausgangsmappe (`<Mod>/items/races`Und Stimme.

<a id="what-the-model-needs"></a>
## Was das Modell braucht

- Ein geschnittenes Messer, dessen Haut die Biped Gelenke hat `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`Beide `Clavicle`, `UpperArm`,
`Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`und `mixamorig:HeadTop_End` Über dem Kopf. Zusätzliche Gelenke (Finger, Hals, Helfer) folgen ihren nahen Biped Vorfahren. **Überprüft** auf Leoma, Crevan, Crabman und die Crab Queen (Meshy Ausfuhren )
- Eine Basisfarbe Textur; eine metallische Scharfe und eine normale Textur werden verwendet, wenn vorhanden ist.
- Jede Ruhe Position: T-Pose und A-Pose beide passen (die Bind-Pose des Spiels hat die Arme unten).

<a id="how-the-fit-works"></a>
## Wie die Fit funktioniert

1. Jeder Vertex wird in den restlichen Possieren der Datei-Show geschnitten (inverse bind times joint rest transform).
2. Skala: Die Kopfknochenhöhe des Spiels über den Kopfgelenk des Modells über seinen niedrigsten Vertex.
3. Jede Spitze, Arm und Bein ist so geschaltet, dass es von seinem Gelenk zu seinem Kind wie das Spiel tut, und auf die Länge des Spiels
(0 bis 1,6 Mal)
4. Pelvis, Spinnen, Hals und Klavikeln sind als ein festes Stück (die geringsten Quadrat auf den Hals, Schulter und Hüftgelenke des Spiels) eingerichtet, so dass der Brust
beibehält seine Form; per-bone passt es an, wo Spine und Klavikel Gewicht vermischen. Die Knochen und Rücken werden aus diesem Gesicht gelassen: Das Knochen des Spiels ist mit seinen Hüften und einem Biped-Sitz über sie, das die Brust um eine Einheit verfallen hat.
5. Der Kopf läuft halb von dort, wo der Torso es in die Kopfhaut bringt, also sitzt seine Messe unter seinem Knochen. Das Hals des Spiels steigt
Schieße über die Schulter und diese Modelle schieße kaum: die Kopf auf den Gelenk zu legen, die ihre Hals auf etwa drei Mal ihre Länge ausstreckt (**beobachtet** Auf dem Mann Leoma, Crabman und die Crab Queen: eingerichtete Hals 2.4 Einheiten, die Modelle eignen 0.8, das Spiel 1.4).
6. Die Füße halten sich recht und schmücken sich, so dass die Solen auf dem Boden landen (Füße sind höher als die Füße des Spiels).
7. Jede Verteile ist die gewichtige Summe ihrer Gelenke; ihre Gewichtskörper gehen ihre Gelenke folgen, die schwersten vier halten.

**Überprüft**: Die Messer des C# entsprechen dem ursprünglichen Python (Positionen innerhalb von 1e-5 Einheiten, identische Knochen) auf Crabman.

<a id="slimming"></a>
## Schwächen

Das Kleid des Spiels wird um den menschlichen Körper des Rigs geformt, so dass es einen Body-Bulkier als er zeigt. Rund um jedes Torso und Knochen (Knochen, Spinen, Klavikeln, oberen und unteren Armen und Beinen) messen die Werkzeuge den längsten Umfang des menschlichen Körpers pro sechste der Knochen und pro zwölfte eines Umlaufs um sie herum; ein festes Vertex über diesen Umfang plus 0,15 Einheiten (1,5 cm Knochen) hält ein Fünftel seines Überschusses, seine Korrektur gemischt durch seine Knochengewicht. Die Korrektur liegt zwischen 1,5 und 2,5 mal in der Grenze (nach: eine Ergänzung wie ein Schwanz, allein), ist null für einen Rücken außerhalb der Knochen (ein Rücken, der unter dem Knochen hängt), und wird über die Oberfläche (12 Runden der Nachbarschaft, UV-Säume gewaschen) so, dass die Nachbarschaften sich bewegen. **beobachtet** (Männer Leoma• 39% der Vertikale bewegen sich durch 1,3 cm im Durchschnitt und 9,5 cm am meisten.

<a id="bust"></a>
## Busse

Der Körper eines weiblichen Körpers wird auf den weiblichen Menschen erhoben (**beobachtet**: Die Leoma und Crevan Frauen sitzen ihre 2.0 und 1.75 Einheiten niedriger, unter der Brust des Kleidens und unter der Teilkarte). Der Bust ist der Mitte des Weichen-Quartal-Einheit-Slitzes, zwischen `Spine1` und die Schultern und 0,3 bis 1,8 Einheiten neben der Mitte, deren Vordergrund am nächsten ist. Jeder Vertex erhöht sich durch die Unterschiede der Zeit sein Torso-Knochen Gewicht, die Zeit exp(-(dy / 1.5)2) seiner Höhe von der Spitze, die Zeit ein glattes Schritte von der Tiefe des Spitzes (0) bis zur Spitze (1), so dass die Rücken, Arme und Füße bleiben. Und dann läuft die Lange.

<a id="human-only-bones"></a>
## Menschen allein Knochen

Das weibliche Skelett hat `L Boob` und `R Boob` (Die Breastgröße, die Höhe und die Spaziergröße des Redaktors und die Bewegung, der Motor docs/animation.mdUnd das, was ein Biped rig fehlt. Nach dem Abnehmen ist jeder Vertex, dessen schwerste Knochen ist `Spine1` oder `Spine2` Er nimmt die menschlichen Gewichtungen des nahe gelegenen menschlichen Verteils an, die einige innerhalb von 1 Einheit durch die Distanz erhöht sind; seine eigenen Gewichtungen verkürzen, um Raum zu machen. **beobachtet** (`human_female.mesh`Die Bustvertikale werden von den beiden Knochen (892 der 994 Vorderbrustvertikalen) getragen. Die Knochen (`L Boob` 28, `R Boob` 29) und die anderen Handlungen der weiblichen Riga unterscheiden sich von den männlichen (`R Clavicle` 23 gegen 25; der Mann hat `Bip01 Jaw`So sind Messe immer gegen die Handlungen ihres eigenen Reiches geschrieben.

<a id="part-map"></a>
## Teil der Karte

Die Race ist `part map male`/`female` PNG Zeichen, an denen der Körper auf der Verteile jedes Verteils gehört; tragbare Gegenstände `hide parts` Holen Sie sich diese Teile auf (Motor) docs/characters.md „Verborgenen Körperteile“ Das Werkzeug gibt jedem vertikalen Teil des menschlichen Körpers, das von demselben Knochen getragen wird, von der Teilkarte des Rings an der UV des Vertikals gelesen und auf der UV des vertikalen (1024 × 1024) geschrieben wird. Jeder menschliche Teil ist zuerst mit 0,8 Einheiten gewachsen (ein Teil nimmt den am nächsten angezeichneten Teil an), da ein Bühne über ein breites Band klettert.

**beobachtet** (`MaleMask_Default.png`, 2562, gemeldet in `human_male.mesh` UVs wie das Spiel zeichnet sich aus: 642 von 15,504 haben einen Teil: Rot der oberen Brust und Schulter, weiße Bänder rund um die oberen Arme, grün die Brust. `hide parts` in den Daten (rebirth.mod): Drifter und Padded Leather Jacket 1, Sleeveless Longcoat und Dustcoat 3, Ninja Gi und Rags 4, Platten Jacks und Crab Armour 7; Heilige Servant Rags, Monk Pants und Hoods. Die Artikel mit 0 verbergen nichts; die Schwäche deckt sie ab. **beobachtet** (`FemaleMask_Default.png` Bei `human_female.mesh` UVs: rote die Bust (1,102 Vertikale), weiße die Oberarmbanden (560), grüne der Bauch (250).

<a id="textures"></a>
## Texturen

- `_d`: Basisfarbe, alpha = gloss = (255 - Hürde) / 2 (Hürde 200 ohne Karte); 20482, BC3 mit Mips.
- `_n`: das Charakter-Layout des Spiels, X in Alpha und Y in Grün, Y fließt von glTF's +Y up; rote und blaue 0.
