<a id="kenshi-races"></a>
# Kenshi Rennen

Drei neue Rennen für den Charakter-Editor, jede mit Subrazen, auf den menschlichen Skeletons des Spiels (`kenshi.races`, keine Abhängigkeiten): Jeder Aufruf, der von der [Rennen Import](race-import.md). Auch schifft er einen [Animationsbibliothek](animations.md) von 59 Clips für jede Rennen auf diesen Scheletons. Mods verwenden die Rennen und Clips, fügen Sie Subrazen hinzu oder fügen Sie Ihre eigenen Rennen hinzu: [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## Rennen

Der Herausgeber hat jede RACE_GROUP Als Rennen und seine RACEs als Subraze (Motor) docs/characters.md „Charakter Editor“

| Die Rennen (RACE_GROUP) | Die Subrace (RACE) | Geschlechter | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | Leoma `10-` | Mann, Frau | Leomamale `11-`Die Leomafemale `17-` |
| Crevan `31-` | Die Crew `18-` | Mann, Frau | Crevanmale `19-`Crevanfemale `21-` |
| Crabfolk `32-` | Crabman `12-` | Einzigartige | Crabman `13-` |
| | Crabkön `14-` | Einzigartige | Crabkön `15-` |

Endet in `KenshiRaces.mod`. Subrace und CHARACTER Namen sind ein niedriges Wort, so dass die Konsole `spawn` Sie nehmen sie ohne Zitate ein. Die Dateien jedes Körpers sind `KenshiRaces/items/races/kenshiraces_<character>*` (Normale Karten auf Leoma und Crevan). Die männlichen Körper sind für `male_skeleton`Die weiblichen Zeiten zu `female_skeleton`; die Krabkörper sind männlich, so dass eine Krab Queen ist einzig Geschlecht mit männlichen Animationen und Stimme. Quellenmodelle (Meshy Exporte, slim 1): Leomamale "Leonid Bare - 3ds Max Biped.glb", crabman und crabqueen "... - 3ds max Biped `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## Aufzeichnungen

In der Datenphase wird die Straße aus dem Laden-Spiel eigener Aufzeichnungen (`RaceRecords`) so `KenshiRaces.mod` Schiffe leer und CI braucht kein Spiel:

- RACE_GROUP„Der Name der Rasse und seine Subrasen“ RACEsIn Ordnung des Herausgebers.
- RACE (Subrace): Eine Kopie von Greenlander (`17-gamedata.quack`: Statistiken, Animationen, Stimmen) mit jedem Geschlecht, Farbtext, normale Karte
und Teilkarte (ein einseitige Subrace trägt seinen einen Körper für beide); `single gender` es sei denn, es hat beide Körper; keine Köpfe oder Haare (eine Rasse, die keinen Kopf aufzeichnet, zieht keine; der Körper hat seinen eigenen); `morph num` 0; `no shirts` (Schuhe und Pannen malen die UV-Layout des menschlichen Körpers); Körpermaske und Stärke normalen Karten gereinigt; `playable` mit `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTEREine Kopie von Drifter (`2205-gamedata.base`Die Subrace und das Geschlecht (`female chance` 100 oder 0), keine Kit, Geld oder Bounty, mit der
Rekrutierungsgespräche
- Motoren: A CHARACTER»S eigener `mesh` Das Feld überschreitet den Körpermesser seiner Rasse.

<a id="character-editor"></a>
## Character Editor

Die Rennen werden bei Charaktereinrichtung wie das Spiel angeboten. `KenshiRaces/items/races/kenshiraces_editor.xml` Er hält nur die Knochen, die sich bewegen, so dass sie auf den eingerichteten Körper wie auf einem Menschen arbeiten (Motor) docs/animation.md, die Knochenskala): Gesicht hält Kopfgröße, Kopfform und die Halsschläge (nicht Gesicht Poses: die eingerichteten Kopf haben keine); Körper hält die menschlichen Körperschläge und Haut Ton, mit Brustgröße und Höhe und Spaziergang für Frauen, die das weibliche Skeleton bewegt `L Boob` / `R Boob` Die Frauen sind in den Körper ([Rennen Import](race-import.md#bust)). Die Menschen sind die (`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## Rekrutierung

DIALOGUE `1-KenshiRaces.mod`Der Charakter sagt: Sie sehen aus, wie Sie ein anderes Handwerk verwenden können, und der Spieler antwortet: "Für uns" und die Aktion 18 fügt den Charakter zum Team hinzu.

<a id="testing-in-game"></a>
## Testung im Spiel

1. Ermöglicht Kenshi Rennen im Start (oder `data/mods.cfg`).
2. Die Konsole: `spawn leomafemale` (oder irgendwelche CHARACTER in der Tabelle) spawns am cursor; sprechen zu recruit, oder F10 Debug Recruit.
3. F10 **Kenshi Rennen: Rennen** Listen Sie alle angepassten Rennen (dies Mods und Abhängig).
4. Neues Spiel: Die Rennstreifen des Redaktors erreichen Leoma, Crevan und Crabfolk.

<a id="clothing"></a>
## Kleidung

Armour teilt das Schelet des Körpers, so dass es mit diesen Körpern bewegt wird, aber es wird um den menschlichen Körper geformt. Die Teilkarte verbergt die Haut unter den Gegenständen mit `hide parts` (Kleidung, Kleidung, Jacke); Schlankheit zieht in dem, was mehr als ein Mensch anderswo ist ([Rennen Import](race-import.md#slimming)). Formen weit von menschlichen (Schicht, Klavier) immer noch durch den festen Armour.
