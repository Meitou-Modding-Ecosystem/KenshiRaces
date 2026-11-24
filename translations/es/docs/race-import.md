<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Importación de carreras: ajustando un cuerpo en el esqueleto del juego

`tools/RaceImport` Se convierte en un rigido `.glb` El cuerpo (Meshy, Hi3D: el "3ds Max Biped" rig, joyas `Bip001 ...`) en un cuerpo el juego se anima con sus propias animaciones humanas: una cuchara de Ogre ligada a `male_skeleton.skeleton` o `female_skeleton.skeleton`Y su DDS Textos y un mapa. Lea el esqueleto, el cuerpo humano y el mapa del juego, por lo que funciona en una máquina con KenshiNunca en el CI; su producción está comprometida.

<a id="run-it"></a>
## correrlo

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>`El mapa de juego (`data/` dentro de ella, por ejemplo. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- Los símbolos llaman los archivos y deben ser únicos en modos (el juego encuentra los archivos por nombre bar): `<modfolder lower case>_<race>`.
- `male` / `female`El hombre, el hombre de default. El cuerpo de una mujer está preparado para `female_skeleton` Medidas contra el `human_female.mesh` y
`FemaleMask_Default.png` (animaciones femeninas y la voz en el juego, el editor de los rastros); un hombre de `male_skeleton`, `human_male.mesh` y `MaleMask_Default.png`.
- `slim`0 a 1 (default 1 a 1)[El debilitamiento](#slimming)• La baja por una carrera que significaba ser más bulkera que su ropa.
- Escritos `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (sólo cuando el modelo tiene un mapa normal) y `<stem>_parts.png`La línea de registro da el
Cuenta el vertido, la escala, los límites y cuántas verticales caen en partes ocultas.
- Mantenga el `.glb` fuera de la repo (tenes de MB); tome los cuatro saltos. `.gitignore` Dejó `*/items/races/*.mesh` y `*.dds` por el paso.
- Un modo dependiente va a correrlo desde un clon de KenshiRaces con su propia pasta de salida (`<Mod>/items/races`y el voto.

<a id="what-the-model-needs"></a>
## Lo que el modelo necesita

- Una mezcla cuya piel tiene las articulaciones Biped `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`Los dos `Clavicle`, `UpperArm`,
`Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`y `mixamorig:HeadTop_End` Sobre la cabeza. Las articulaciones extras (de los dedos, de las cuerdas, de los ayudantes) siguen su ancestro más cercano. **Verificado** En el Leoma, Crevan, Crabman y la reina Crab (Meshy de las exportaciones).
- Una textura de color básico; una dureza metálica y una textura normal se utilizan cuando están presentes.
- Cualquier posición de descanso: T-Pose y A-Pose ambos se ajustan (la posición de enlace del juego tiene los brazos abajo).

<a id="how-the-fit-works"></a>
## Cómo funciona el fío

1. Cada vertex es pintado en el resto de los espectáculos de archivo (los tiempos de enlace inversos de la transformación de reposo).
2. Escala: la altura del hueso del juego sobre la articulación de la cabeza del modelo por encima de su vertex más bajo.
3. Cada espalda, brazo y pierna se vuelve para que ponga de su articulación a su hijo como el juego hace, y se extiende a la longitud del juego.
(de 6 a 1.6 veces)
4. Las pelvis, espinas, cuello y clavicles están equipados como una pieza rígida (las cuerdas más pequeñas en el cuello, hombro y articulaciones de la espina del juego), por lo que el pecho
mantendrá su forma; per-bone se ajusta a la espina donde la espina y el clavicle pesan mezclar. Las articulaciones de la columna son dejadas de esa forma: la columna del juego está en su nivel y los asientos de la exportación están encima de ellos, lo que ha caído el pecho alrededor de una unidad.
5. La cabeza cae medio de donde el torso lo lleva a la articulación del juego, por lo que su mezcla se sitúa debajo de su hueso. El cuello del juego se eleva
En la parte inferior de la columna vertebrada, se pone la cabeza en la articulación, y se estende el cuello hasta aproximadamente tres veces su longitud (**observado** En el hombre LeomaCrabman y la Reina Crab: unidad de cuello de 2.4 unidades, los modelos de 0.8, el juego de 1.4).
6. Los pies se mantienen justo y se despejan de manera que los solos se derramen en el suelo (los pies de los paños y los pies de los crabes son más altos que los pies del juego).
7. Cada vertical es la suma de sus adhesiones; sus pesos van a los huesos de juego, los más pesados se mantienen.

**Verificado**Las mesas de la herramienta C# coinciden con el prototipo original de Python (posiciones dentro de 1e-5 unidades, pesos de hueso idénticos) en Crabman.

<a id="slimming"></a>
## El debilitamiento

La ropa del juego se modela alrededor del cuerpo humano de la riga, por lo que es un bulkier del cuerpo que muestra a través. Alrededor de cada oso torso y hueso (pelvis, espinas, clavicles, los brazos y piernas superiores y inferiores) la herramienta mide el alcance más largo del cuerpo humano por el sexto del hueso y por el doce de un giro alrededor de él; un vertex fijado más allá de ese alcance plus 0.15 unidades (1,5 cm de la ropa) mantiene un quinto de su exceso, su corrección mezclada por sus pesos huesos. La corrección se extiende entre 1.5 y 2.5 veces el límite (a la derecha: un apéndice como la espalda), es cero para una espalda fuera de los huesos (una espalda que se encuentra debajo de la espalda) y se desliza sobre la superficie (12 rondas de vecino mediano, las espaldas de la espaldas) así que las espaldaciones vecinas se mueven de la misma manera. **observado** (El hombre Leoma• 39% de las verticales se mueven, por 1,3 cm en promedio y 9,5 cm en máximo.

<a id="bust"></a>
## El bus

El cuerpo de una mujer se eleva al cuerpo de la mujer (**observado**• El Leoma y Crevan Las mujeres se colocaron sus 2.0 y 1.75 unidades más bajas, debajo del pecho de la ropa y el busto del mapa). El busto es la mezcla de la mezcla cuartel-unidad, entre `Spine1` y los hombros y las unidades de 0.3 a 1.8 junto a la línea media, cuyo vertido principal es el más cercano hacia adelante. Cada vertical se eleva por la diferencia de tiempos su peso torso-bono, tiempos exp(-(dy / 1.5)2) de su altura desde el estómago, tiempos un paso suave desde la profundidad de la espalda (0) hasta el estómano (1), así que la espalda, los brazos y las piernas permanecen. Luego, el alivio corre.

<a id="human-only-bones"></a>
## Los huesos humanos

El esqueleto femenino tiene `L Boob` y `R Boob` (el tamaño del pecho del editor, la altura y la escala de espacio y los mover, el motor docs/animation.mdque falta un Biped rig. Después de la pérdida de peso, cada vertido cuyo hueso más pesado es `Spine1` o `Spine2` toma los pesos humanos del vertido humano más cercano que tiene algunos, dentro de 1 unidad, escalonados por 1 - distancia; sus propios pesos se reducen para hacer espacio. **observado** (`human_female.mesh`Las vertices de la cáscara son llevadas por los dos huesos (892 de las 994 vertices del pecho delantero). Los huesos (`L Boob` 28, `R Boob` El resto de las manos de la mujer se distinguen de las de los hombres (`R Clavicle` 23 contra 25; el hombre tiene `Bip01 Jaw`Las mesas son siempre escritas contra las manos de su propio rigo.

<a id="part-map"></a>
## Parte del mapa

La carrera de `part map male`/`female` PNG las marcas a las que cada vertex pertenece; los objetos llevados `hide parts` Dejar de dibujar esas partes (el motor docs/characters.md “Las partes ocultas del cuerpo”. La herramienta da a cada vertiente la parte de la vertiente más cercana del cuerpo humano llevada por el mismo hueso, leído de la parte del rango en el UV de ese vertiente, y lo escribe en el propio UV (1024 × 1024), otros textiles transparentes. Cada parte humana se desarrolla por primera vez en 0.8 unidades (una parte toma la de la parte más cercana), ya que un cuerpo más amplio cubre una banda más amplia.

**observado** (`MaleMask_Default.png`, 2562, mostrado en `human_male.mesh` UVs como el juego los muestra: 642 de las verticales tienen una parte: rojo el pecho superior y los hombros, bandas blancas alrededor de los brazos superiores, verde el frente del estómago. `hide parts` En los datos de base (rebirth.mod): Drifter y Padded Leather Jacket 1, Sleeveless Longcoat y Dustcoat 3, Ninja Gi y Rags 4, ropa 5, ropa de piel y Crab Armour 7; San Servo Rags, Monk Pants y Hoods 0. Los elementos con 0 no ocultan nada; la pérdida de peso cubre los mismos. **observado** (`FemaleMask_Default.png` En el `human_female.mesh` UVs: rojo el busto (1,102 vertices), blanco las bandas de la mano superior (560), verde el estómago (250).

<a id="textures"></a>
## Texturas

- `_d`: color de base, alfa = gloss = (255 - dureza) / 2 ( dureza 200 sin un mapa); 20482, BC3 con mips.
- `_n`: el diseño de los personajes del juego, X en alfa y Y en verde, Y flipped de glTF+ Y arriba; rojo y azul 0.
