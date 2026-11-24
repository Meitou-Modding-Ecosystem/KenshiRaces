<a id="kenshi-races"></a>
# Kenshi Races

Tres nuevas razas para el editor de personajes, cada una con subrazas, en los esqueletos humanos del juego ( `kenshi.races` , sin dependencias): cada registro agregado durante la carga, los cuerpos equipados por la [carrera importada](race-import.md) . También incluye una [biblioteca de animación](animations.md) de 59 clips para cada raza en esos esqueletos. Los mods usan las carreras y los clips, agregan subrazas o agregan sus propias razas: [depending-on-kenshi-races.md](depending-on-kenshi-races.md) .

<a id="races"></a>
## carreras

El editor enumera cada RACE_GROUP como una carrera y sus CARRERAS como subrazas (engine docs/characters.md "Editor de personajes").

| Carrera ( RACE_GROUP ) | Subraza ( RACE ) | Géneros | PERSONAJES |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | masculino, femenino | leomamacho `11-` , leomamacho `17-` |
| Crevan `31-` | crevan `18-` | masculino, femenino | crevanmacho `19-` , crevanhembra `21-` |
| Crabfolk `32-` | hombre cangrejo `12-` | soltero | hombre cangrejo `13-` |
|  | reina cangrejo `14-` | soltero | reina cangrejo `15-` |

Los identificadores terminan en `KenshiRaces.mod` . Los nombres Subrace y CHARACTER son una palabra en minúscula, por lo que el `spawn` de la consola los toma sin comillas. Los archivos de cada cuerpo son `KenshiRaces/items/races/kenshiraces_<character>*` (mapas normales en Leoma y Crevan ). Los cuerpos masculinos están instalados en `male_skeleton` , los femeninos en `female_skeleton` ; Los cuerpos de los cangrejos son trajes masculinos, por lo que una reina cangrejo es de un solo género con las animaciones y la voz masculinas. Modelos de origen (exportaciones Meshy , slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", cangrejo y cangrejo queen "... - 3ds Max Biped.glb", los otros `leonidfemale.glb` , `corvidmale.glb` , `corvidfemale.glb` .

<a id="records"></a>
## Archivos

Agregado en la fase de datos de los propios registros del juego cargado ( `RaceRecords` ), por lo que `KenshiRaces.mod` se envía vacío y CI no necesita juego:

- RACE_GROUP : el nombre de la carrera y las CARRERAS de sus subrazas, en el orden del editor.
- RACE (subraza): una copia de Greenlander ( `17-gamedata.quack` : estadísticas, animaciones, voces) con la malla, textura de color, mapa normal y mapa parcial de cada género (una subraza de un solo género usa su mismo cuerpo para ambos); `single gender` a menos que tenga ambos cuerpos; sin cabezas ni cabellos (una raza que no tiene cabeza no dibuja ninguna; el cuerpo tiene la suya propia); `morph num` 0; `no shirts` (camisas y pantalones pintan la disposición ultravioleta del cuerpo humano); máscara corporal y mapas normales de fuerza borrados; `playable` con `editor limits` [ `kenshiraces_editor.xml` ](#character-editor) .
- CHARACTER : una copia de Drifter ( `2205-gamedata.base` ) de la subraza y género ( `female chance` 100 o 0), sin kit, dinero ni recompensa, con la charla de recluta.
- Motor: el propio campo `mesh` de un CHARACTER anula la malla de la carrocería de su carrera.

<a id="character-editor"></a>
## editor de personajes

Las carreras se ofrecen en la creación de personajes como las del juego. `KenshiRaces/items/races/kenshiraces_editor.xml` mantiene solo los controles deslizantes que mueven los huesos, por lo que funcionan en los cuerpos ajustados como en un humano (motor docs/animation.md, las escalas óseas): La cara contiene el tamaño de la cabeza, la forma de la cabeza y los controles deslizantes del cuello (sin posturas de la cara: las cabezas ajustadas no tienen ninguna); El cuerpo contiene los controles deslizantes del cuerpo humano y el tono de piel, con el tamaño, la altura y el espaciado de los senos para las mujeres, que mueven el `L Boob` / `R Boob` del esqueleto femenino al que están ponderados los cuerpos femeninos ([race import](race-import.md#bust) ). Los rangos son los humanos ( `data/editor/editor_data_human.xml` ).

<a id="recruiting"></a>
## Reclutamiento

DIALOGUE  `1-KenshiRaces.mod` : el personaje dice "Parece que te vendrían bien otro par de manos", el jugador responde "Únete a nosotros". y la acción 18 (unirse al escuadrón) agrega el personaje al escuadrón del jugador.

<a id="testing-in-game"></a>
## Pruebas en el juego

1. Habilite Kenshi Races en el iniciador (o `data/mods.cfg` ).
2. Consola: `spawn leomafemale` (o cualquier CHARACTER en la tabla) aparece en el cursor; hablar con reclutar, o F10 depurar Reclutar.
3. F10 ** Kenshi Races : carreras ** enumera todas las carreras equipadas y cargadas (este mod y sus dependientes).
4. Nuevo juego: las flechas de carrera del editor llegan a Leoma , Crevan y Crabfolk .

<a id="clothing"></a>
## Ropa

La armadura comparte el esqueleto del cuerpo, por lo que se mueve con estos cuerpos, pero está modelada alrededor del cuerpo humano. El mapa de piezas oculta la piel debajo de los elementos con `hide parts` (abrigos, batas, chaquetas); adelgazar atrae a lo que es más voluminoso que un ser humano en otros lugares ([importación de raza](race-import.md#slimming) ). Formas muy alejadas de las humanas (conchas, garras) todavía se asoman a través de la ajustada armadura.
