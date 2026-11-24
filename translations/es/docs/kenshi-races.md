<a id="kenshi-races"></a>
# Kenshi Las carreras

Tres nuevas carreras para el editor de personajes, cada una con subrasas, en los esqueletos humanos del juego (`kenshi.races`, sin dependencias): cada registro añadido a la carga, los cuerpos equipados por el [Las carreras de importación](race-import.md). Además, el barco también [Biblioteca de Animación](animations.md) de 59 clips para cada carrera en esos esqueletos. Los modos utilizan las carreras y los clips, añaden subraces o añaden sus propias carreras: [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## Las carreras

El editor incluye cada uno RACE_GROUP como una carrera y su RACEs como subraces (el motor docs/characters.md “Editor de personajes”.

| La carrera (RACE_GROUP) | El submarino (RACE) | Género | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | León `10-` | hombre, mujer | Leomacía `11-`La leona `17-` |
| Crevan `31-` | El tripulación `18-` | hombre, mujer | Crevanmales `19-`Creación `21-` |
| Crabfolk `32-` | Crabman `12-` | Solitario | Crabman `13-` |
| | Crabqueen `14-` | Solitario | Crabqueen `15-` |

terminará en `KenshiRaces.mod`. Subrace y CHARACTER los nombres son una palabra inferior, por lo que la consola `spawn` Los tomamos sin cotas. Los archivos de cada cuerpo son `KenshiRaces/items/races/kenshiraces_<character>*` (Mapa normal en el Leoma y Crevan). Los cuerpos masculinos están preparados para `male_skeleton`Las mujeres de `female_skeleton`Los cuerpos de crab son masculinos, por lo que una reina de crab es un solo género con las animaciones masculinas y la voz. El modelo de fuente (Meshy exportaciones, slim 1): leomal "Leonid Bare - 3ds Max Biped.glb", crabman y crabqueen "... - 3ds max Biped `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## Los registros

Añadido en la fase de datos de la plataforma de datos del juego cargado (`RaceRecords`Y así `KenshiRaces.mod` Los barcos vacíos y CI no necesitan juego:

- RACE_GROUPEl nombre de la raza y sus subraces RACEspor orden del editor.
- RACE (subrace): una copia de Greenlander (`17-gamedata.quack`: estadísticas, animaciones, voces) con la mezcla de cada género, textura de color, mapa normal
y un mapa parcial (una subraza de un solo género lleva su un cuerpo para ambos); `single gender` a menos que tenga dos cuerpos; no hay cabezas ni cabezas (una raza en la que no hay cabeza atrae ninguno; el cuerpo tiene su propio); `morph num` 0; `no shirts` (T-shirts y pantalones pintan el layout UV del cuerpo humano); máscara del cuerpo y la fuerza de los mapas normales limpiados; `playable` con `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTERUna copia de la letra (`2205-gamedata.base`de la raza y el género (`female chance` 100 o 0), sin kit, dinero o bonos, con el
Recrutación de conversaciones.
- El motor: A CHARACTERde su propia `mesh` El campo supera la mezcla corporal de su raza.

<a id="character-editor"></a>
## El personaje editor

Las carreras se ofrecen en la creación de personajes como el propio juego. `KenshiRaces/items/races/kenshiraces_editor.xml` Mantén sólo los eslavos que mueven los huesos, por lo que trabajan en los cuerpos adhesivos como en un hombre (motor) docs/animation.md, las escalas óseas): El rostro tiene el tamaño de la cabeza, la forma de la cabeza y los rastros del cuello (no las posiciones del rostro: las cabezas fijadas no tienen ninguna); El cuerpo tiene los rastros y el ton de la piel del cuerpo humano, con el tamaño del pecho, la altura y el espacio para las mujeres, que mueven el esqueleto femenino. `L Boob` / `R Boob` Los cuerpos de las mujeres son objeto de[Las carreras de importación](race-import.md#bust)). Los seres humanos son los seres humanos (`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## Recrutamiento

DIALOGUE `1-KenshiRaces.mod`El personaje dice: “Se ve como si pudiera usar otro par de manos”, y el jugador responde: “Juego” y la acción 18 añade el personaje al jugador.

<a id="testing-in-game"></a>
## Testar en el juego

1. Permite Kenshi Las raíces en el lanzador (o `data/mods.cfg`).
2. El console: `spawn leomafemale` (o cualquier CHARACTER en la tabla) espadas en el cursor; hablar para recrutar, o F10 recrutar.
3. F10 **Kenshi Categoría: Race** Lista de cada rase de plazas de plazas (los modos y dependientes).
4. Nuevo juego: los arcos de la carrera del editor alcanzan Leoma, Crevan y Crabfolk.

<a id="clothing"></a>
## La ropa

Armour comparte el esqueleto del cuerpo, por lo que se mueve con estos cuerpos, pero se modela alrededor del cuerpo humano. La parte del mapa esconde la piel debajo de los objetos con `hide parts` (Las camisetas, ropas, jaquetas); los rasgos de pérdida de peso en lo que es más bulkier que un humano en otros lugares ([Las carreras de importación](race-import.md#slimming)). Las formas lejanas de los humanos (heces, clavos) todavía se ponen a través de un armario estrecho.
