<a id="kenshi-races"></a>
# Kenshi Races

Drei neue Rassen für den Charakter-Editor, jeweils mit Unterrassen, auf den menschlichen Skeletten des Spiels (`kenshi.races`, keine Abhängigkeiten): jeder beim Laden hinzugefügte Datensatz, die Körper, die durch den [Rassenimport](race-import.md). Es enthält außerdem eine [Animationsbibliothek](animations.md) mit 59 Clips für jedes Rennen auf diesen Skeletten. Mods verwenden die Rassen und Clips, fügen Unterrassen hinzu oder fügen ihre eigenen Rassen hinzu: [abhängig von kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## Spielbare Völker

Im Editor erscheint jede RACE_GROUP als Volk. Die zugehörigen RACE-Datensätze bilden seine Unterarten.

| Volk | Unterart | Körper | Charaktere |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | männlich, weiblich | leomamale `11-`, leomafemale `17-` |
| Crevan `31-` | crevan `18-` | männlich, weiblich | crevanmale `19-`, crevanfemale `21-` |
| Crabfolk `32-` | crabman `12-` | ein Körper | crabman `13-` |
|  | crabqueen `14-` | ein Körper | crabqueen `15-` |

IDs enden auf `KenshiRaces.mod`. Die Namen „Subrace“ und „ CHARACTER “ bestehen aus einem Wort in Kleinbuchstaben, sodass sie im `spawn` der Konsole ohne Anführungszeichen verwendet werden. Die Dateien jedes Körpers sind `KenshiRaces/items/races/kenshiraces_<character>*` (normale Karten auf Leoma und Crevan). Männliche Körper werden an `male_skeleton` angepasst, weibliche an `female_skeleton` ; Da die Körper der Krabben männlich sind, ist eine Krabbenkönigin mit männlichen Animationen und Stimmen gleichgeschlechtlich. Quellmodelle (Meshy -Exporte, schlank 1): leomamale „Leonid Bare – 3ds Max Biped.glb“, Crabman und Crabqueen „... – 3ds Max Biped.glb“, die anderen `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## Aufzeichnungen

In der Datenphase aus den eigenen Datensätzen des geladenen Spiels (`RaceRecords`) hinzugefügt, sodass `KenshiRaces.mod` leer geliefert wird und CI kein Spiel benötigt:

- RACE_GROUP : Der Name der Rasse und die RENNEN ihrer Unterrassen, in der Reihenfolge des Herausgebers.
- RACE (Unterrasse): eine Kopie von Greenlander (`17-gamedata.quack` : Statistiken, Animationen, Stimmen) mit Netz, Farbtextur, Normalkarte und Teilkarte jedes Geschlechts (eine gleichgeschlechtliche Unterrasse trägt für beide ihren einen Körper); `single gender`, es sei denn, es hat beide Körper; keine Köpfe oder Haare (eine Rasse, die keinen Kopf auflistet, zeichnet keinen; der Körper hat seinen eigenen); `morph num` 0; `no shirts` (Hemden und Hosen zeigen das UV-Layout des menschlichen Körpers); Körpermaske und Kraft-Normalkarten gelöscht; `playable` mit `editor limits` [ `kenshiraces_editor.xml` ](#character-editor).
- CHARACTER : eine Kopie von Drifter (`2205-gamedata.base`) der Unterrasse und des Geschlechts (`female chance` 100 oder 0), keine Ausrüstung, kein Geld oder Kopfgeld, mit dem Rekrutengespräch.
- Motor: Das eigene `mesh` -Feld eines CHARACTER überschreibt das Körpernetz seines Rennens.

<a id="character-editor"></a>
## Charaktereditor

Die Rennen werden bei der Charaktererstellung wie im Spiel angeboten. `KenshiRaces/items/races/kenshiraces_editor.xml` behält nur die Schieberegler, die Knochen bewegen, sodass sie auf die angepassten Körper wie auf einen Menschen wirken (engine docs/animation.md, die Knochenskalen): Gesicht behält Kopfgröße, Kopfform und die Halsschieberegler (keine Gesichtsposen: die angepassten Köpfe haben keine); „Körper“ enthält die Schieberegler für den menschlichen Körper und „Hautton“, mit Brustgröße, -höhe und -abstand für Frauen, die die `L Boob` / `R Boob` des weiblichen Skeletts bewegen, mit denen die weiblichen Körper gewichtet werden ([race import](race-import.md#bust)). Bereiche sind die menschlichen (`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## Rekrutierung

DIALOGUE  `1-KenshiRaces.mod` : Der Charakter sagt: „Du siehst aus, als könntest du noch ein Paar Hände gebrauchen.“ Der Spieler antwortet: „Komm mit.“ und Aktion 18 (Trupp beitreten) fügt den Charakter dem Trupp des Spielers hinzu.

<a id="testing-in-game"></a>
## Rekrutierung und Test im Spiel

1. Aktiviere Kenshi Races im Launcher oder in `data/mods.cfg`.
2. Mit `spawn leomafemale` erscheint der Charakter an der Cursorposition. Du kannst jeden Charakternamen aus der Tabelle verwenden.
3. Sprich die Figur an und nimm ihr Angebot an, sich deinem Trupp anzuschließen. Alternativ steht im F10-Debugmenü Recruit zur Verfügung.
4. **Kenshi Races: races**im F10-Menü listet alle geladenen angepassten Völker dieses Mods und der abhängigen Mods auf.

<a id="clothing"></a>
## Kleidung

Rüstungen folgen demselben Skelett, ihre Form ist jedoch auf menschliche Körper zugeschnitten. Mäntel, Roben und Jacken mit `hide parts` blenden die Haut darunter aus. Panzer und Scheren können durch eng anliegende Rüstungen ragen. Beim Verschlanken werden besonders breite Körperpartien an menschliche Proportionen angenähert. ([race-import.md](race-import.md#slimming)).

[English](../../../docs/kenshi-races.md) · [Kenshi Races](../README.md)
