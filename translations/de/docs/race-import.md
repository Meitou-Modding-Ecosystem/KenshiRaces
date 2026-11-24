<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Einen GLB-Körper an das Skelett des Spiels anpassen

`tools/RaceImport` verwandelt einen manipulierten `.glb` -Körper (Meshy, Hi3D : das „3ds Max Biped“-Rig, Gelenke `Bip001 ...`) in einen Körper, den das Spiel mit seinen eigenen menschlichen Animationen animiert: einem gehäuteten Ogre-Netz, das mit `male_skeleton.skeleton` oder `female_skeleton.skeleton` verknüpft ist, seinen DDS -Texturen und einer Teilekarte. Es liest das Skelett, den menschlichen Körper und die Teilkarte des Rigs aus der Spielinstallation, sodass es auf einer Maschine mit Kenshi läuft, niemals auf CI ; seine Ausgabe ist festgeschrieben.

<a id="run-it"></a>
## Führen Sie es aus

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>` : der Spielordner (`data/` darin), z.B. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- Der Stamm benennt die Dateien und muss in allen Mods eindeutig sein (das Spiel findet Dateien anhand ihres bloßen Namens): `<modfolder lower case>_<race>`.
- `male` / `female` : das Rig, standardmäßig männlich. Ein weiblicher Körper wird an `female_skeleton` angepasst und mit `human_female.mesh` und `FemaleMask_Default.png` verglichen (weibliche Animationen und Stimme im Spiel, die Büstenregler des Herausgebers); ein männlicher für `male_skeleton`, `human_male.mesh` und `MaleMask_Default.png`.
- `slim` : 0 bis 1, Standard 1 ([Abnehmen](#slimming)); Senken Sie es für ein Rennen, das sperriger sein soll als seine Kleidung.
- Schreibt `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (nur wenn das Modell eine Normalkarte hat) und `<stem>_parts.png` ; Die Protokollzeile gibt die Anzahl der Scheitelpunkte, den Maßstab, die Grenzen und die Anzahl der Scheitelpunkte an, die in ausblendbare Teile fallen.
- Halten Sie den `.glb` außerhalb des Repos (Zehner von MB); Übertragen Sie die vier Ausgaben. `.gitignore` lässt `*/items/races/*.mesh` und `*.dds` durch.
- Ein abhängiger Mod führt es von einem KenshiRaces-Klon mit eigenem Ausgabeordner (`<Mod>/items/races`) und Stamm aus.

<a id="what-the-model-needs"></a>
## Was das Modell braucht

- Ein mit Haut versehenes Netz, dessen Haut die Biped-Gelenke `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`, beide `Clavicle`, `UpperArm`, `Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0` und aufweist `mixamorig:HeadTop_End` über dem Kopf. Zusätzliche Gelenke (Finger, Schwänze, Helfer) folgen ihrem nächsten Biped-Vorfahren. **Verifiziert**für Leoma, Crevan, Crabman und die Crab Queen (Meshy -Exporte).
- Eine Grundfarbtextur; Sofern vorhanden, werden eine metallische Rauheit und eine normale Textur verwendet.
- Jede Ruhepose: T-Pose und A-Pose passen beide (die Bindungspose des Spiels hat die Arme nach unten).

<a id="how-the-fit-works"></a>
## So funktioniert die Passform

1. Jeder Scheitelpunkt wird in die in der Datei angezeigte Ruheposition gehäutet (umgekehrte Bindung mal gemeinsame Ruhetransformation).
2. Maßstab: die Höhe des Kopfknochens des Spiels über dem Kopfgelenk des Modells über seinem niedrigsten Scheitelpunkt.
3. Jeder Wirbelsäulen-, Arm- und Beinknochen wird so gedreht, dass er wie beim Spiel von seinem Gelenk zum Kind zeigt, und auf die Länge des Spiels gestreckt (0,6- bis 1,6-fach).
4. Becken, Wirbelsäule, Hals und Schlüsselbeine sind als ein starres Stück angebracht (am wenigsten im Quadrat zu den Hals-, Schulter- und Hüftgelenken des Spiels), sodass die Brust ihre Form behält; Pro-Knochen-Passungen scheren dort, wo Wirbelsäulen- und Schlüsselbeingewichte verschmelzen. Das Becken und die Wirbelsäulengelenke bleiben bei dieser Passform unberücksichtigt: Das Becken des Tieres befindet sich auf gleicher Höhe mit seinen Hüften und darüber sitzt ein Biped-Export, der die Brust um etwa eine Einheit nach unten senkt.
5. Der Kopf landet auf halber Strecke von der Stelle, an der der Rumpf ihn trägt, bis zum Kopfgelenk des Wildes, sodass sein Netz unter seinem Knochen sitzt. Der Hals des Spiels ragt etwa eine Einheit über die Schultern, was bei diesen Modellen kaum der Fall ist: Durch das Platzieren des Kopfes auf dem Gelenk wurde der Hals auf etwa das Dreifache seiner Länge gedehnt (**Beobachtet**beim männlichen Leoma, Crabman und der Crab Queen: angepasster Hals 2,4 Einheiten, der der Modelle selbst 0,8, der des Spiels 1,4).
6. Die Füße bleiben aufrecht und werden gequetscht, sodass die Sohlen auf dem Boden landen (Pfoten und Krebsfüße sind höher als der Fuß des Wildes).
7. Jeder Scheitelpunkt ist die gewichtete Summe der Anpassungen seiner Gelenke; Seine Gewichte gehen auf die Wildknochen über, gefolgt von seinen Gelenken, wobei die schwersten vier gehalten werden.

**Verifiziert**: Die Netze des C#-Tools stimmen mit denen des ursprünglichen Python-Prototyps (Positionen innerhalb von 1e-5 Einheiten, identische Knochengewichte) auf Crabman überein.

<a id="slimming"></a>
## Abnehmen

Die Kleidung des Spiels ist dem menschlichen Körper des Rigs nachempfunden, also ein Körper, der voluminöser ist, als er durchscheint. Um jeden Rumpf- und Gliedmaßenknochen herum (Becken, Wirbelsäule, Schlüsselbeine, Ober- und Unterarme und Beine) misst das Gerät die weiteste Ausdehnung des menschlichen Körpers pro Sechstel des Knochens und pro Zwölftel Umdrehung um ihn herum; Ein angepasster Scheitelpunkt über dieser Ausdehnung plus 0,15 Einheiten (1,5 cm Stoff) behält ein Fünftel seines Überschusses, seine Korrektur wird durch seine Knochengewichte ausgeglichen. Die Korrektur wird zwischen dem 1,5- und 2,5-fachen des Grenzwerts ausgeblendet (darüber hinaus: ein Anhängsel wie eine Klaue, das allein gelassen wird), beträgt Null für einen Scheitelpunkt außerhalb der Knochenscheiben (ein Schwanz hängt unter dem Becken) und wird über die Oberfläche geglättet (12 Runden benachbarter Mittelung, UV-Nähte verschweißt), sodass sich benachbarte Scheitelpunkte gleich bewegen. **Beobachtet**(männlich Leoma): 39 % der Eckpunkte bewegen sich, um durchschnittlich 1,3 cm und höchstens um 9,5 cm.

<a id="bust"></a>
## Büste

Die Brust eines weiblichen Körpers ist auf die des weiblichen Menschen angehoben (**Beobachtet**: Die Leoma - und Crevan -Frauen saßen 2,0 bzw. 1,75 Einheiten tiefer, unter der Brust des Kleidungsstücks und der Brust der Teilkarte). Die Brust befindet sich in der Mitte des Viertel-Einheiten-Schnitts, zwischen `Spine1` und den Schultern und 0,3 bis 1,8 Einheiten neben der Mittellinie, deren vorderster Scheitelpunkt am weitesten vorne liegt. Jeder Scheitelpunkt steigt um die Differenz mal seinem Rumpfknochengewicht, mal exp(-(dy / 1,5)²) seiner Höhe von der Brust, mal einen sanften Schritt von der Tiefe der Wirbelsäule (0) zur Brusttiefe (1), sodass Rücken, Arme und Beine bleiben. Dann Schlankheitsläufe.

<a id="human-only-bones"></a>
## Knochen nur für Menschen

Das weibliche Skelett verfügt über `L Boob` und `R Boob` (die Brustgröße, -höhe und -abstände des Herausgebers skalieren und verschieben, engine docs/animation.md), die einem Biped-Rig fehlen. Nach dem Abnehmen übernimmt jeder Scheitelpunkt, dessen schwerster Knochen `Spine1` oder `Spine2` ist, die rein menschlichen Gewichte des nächstgelegenen menschlichen Scheitelpunkts, der einige hat, innerhalb einer Einheit, skaliert um 1 - Abstand; seine eigenen Gewichte schrumpfen, um Platz zu schaffen. **Beobachtet**(`human_female.mesh`): Die Brustscheitel werden von den beiden Knochen getragen (892 der 994 vorderen Brustscheitel). Die Knochen (`L Boob` 28, `R Boob` 29) und die anderen Griffe des weiblichen Rigs unterscheiden sich von denen des männlichen (`R Clavicle` 23 gegenüber 25; das Männchen hat `Bip01 Jaw`), daher werden Meshes immer gegen die Griffe des eigenen Rigs geschrieben.

<a id="part-map"></a>
## Teilkarte

Der `part map male` / `female`  PNG des Rennens markiert, zu welchem ​​Körperteil jeder Scheitelpunkt gehört; `hide parts` für abgenutzte Gegenstände hört auf, diese Teile zu zeichnen (engine docs/characters.md „Versteckte Körperteile“). Das Werkzeug gibt jedem angepassten Scheitelpunkt den Teil des nächstgelegenen menschlichen Körperscheitelpunkts, der von demselben Knochen getragen wird, liest aus der Teilekarte des Rigs im UV-Bereich dieses Scheitelpunkts und schreibt ihn im eigenen UV-Bereich des angepassten Scheitelpunkts (1024 × 1024, andere Texel transparent). Jeder menschliche Teil wird zunächst um 0,8 Einheiten vergrößert (ein Scheitelpunkt ohne Teil nimmt den Wert des nächstgelegenen markierten Scheitelpunkts innerhalb dieser Entfernung an), da ein voluminöserer Körper über ein breiteres Band klemmt.

**Beobachtet**(`MaleMask_Default.png`, 256², abgetastet bei `human_male.mesh` -UVs, während das Spiel sie abtastet): 642 von 15.504 Scheitelpunkten haben einen Teil: rot die oberen Brust- und Schulteroberseiten, weiße Bänder um die Oberarme, grün die Bauchvorderseite. `hide parts` in den Basisdaten (rebirth.mod): Drifter- und gepolsterte Lederjacke 1, ärmelloser Langmantel und Staubmantel 2, Langmantel und Staubmantel 3, Ninja-Gi und Lumpen 4, Roben 5, Plattenjacken, Hautanzüge und Krabbenrüstung 7; Lumpen des Heiligen Dieners, Mönchshosen und Kapuzen 0. Gegenstände mit 0 verbergen nichts; Das Abnehmen deckt diese ab. **Beobachtet**(`FemaleMask_Default.png` und `human_female.mesh` UVS): Rot die Brust (1.102 Eckpunkte), Weiß die Oberarmbänder (560), Grün der Bauch (250).

<a id="textures"></a>
## Texturen

- `_d` : Grundfarbe, Alpha = Glanz = (255 – Rauheit) / 2 (Rauheit 200 ohne Karte); 2048², BC3 mit Mips.
- `_n` : das Charakterlayout des Spiels, X in Alpha und Y in Grün, Y von glTFs +Y nach oben gespiegelt; rot und blau 0.

[English](../../../docs/race-import.md) · [Kenshi Races](../README.md)
