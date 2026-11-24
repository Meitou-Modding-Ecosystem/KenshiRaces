<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Importación de carrera: encajar un cuerpo en el esqueleto del juego

`tools/RaceImport` convierte un cuerpo `.glb` manipulado ( Meshy , Hi3D : el equipo "3ds Max Biped", articulaciones `Bip001 ...` ) en un cuerpo que el juego anima con sus propias animaciones humanas: una malla de Ogro desollado vinculada a `male_skeleton.skeleton` o `female_skeleton.skeleton` , sus texturas DDS y un mapa parcial. Lee el esqueleto del equipo, el cuerpo humano y el mapa parcial de la instalación del juego, por lo que se ejecuta en una máquina con Kenshi , nunca en CI ; su producción está comprometida.

<a id="run-it"></a>
## ejecutarlo

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>` : la carpeta del juego ( `data/` dentro), p.e. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"` .
- La raíz nombra los archivos y debe ser única en todas las modificaciones (el juego busca archivos por su nombre simple): `<modfolder lower case>_<race>` .
- `male` / `female` : el equipo, macho predeterminado. Se ajusta un cuerpo femenino a `female_skeleton` y se compara con `human_female.mesh` y `FemaleMask_Default.png` (animaciones y voz femeninas en el juego, controles deslizantes de busto del editor); uno macho para `male_skeleton` , `human_male.mesh` y `MaleMask_Default.png` .
- `slim` : 0 a 1, predeterminado 1 ([Adelgazamiento](#slimming) ); bájalo para una carrera destinada a ser más voluminosa que su ropa.
- Escribe `<stem>.mesh` , `<stem>_d.dds` , `<stem>_n.dds` (solo cuando el modelo tiene un mapa normal) y `<stem>_parts.png` ; la línea de registro proporciona el número de vértices, la escala, los límites y cuántos vértices se encuentran en partes ocultables.
- Mantenga el `.glb` fuera del repositorio (decenas de MB ); comprometer las cuatro salidas. `.gitignore` deja pasar a `*/items/races/*.mesh` y `*.dds` .
- Un mod dependiente lo ejecuta desde un clon de KenshiRaces con su propia carpeta de salida ( `<Mod>/items/races` ) y raíz.

<a id="what-the-model-needs"></a>
## Lo que necesita el modelo

- Una malla desollada cuya piel tiene las articulaciones Bípedas `Bip001 Pelvis` , `Spine` , `Spine1` , `Spine2` , `Neck` , `Head` , ambas `Clavicle` , `UpperArm` , `Forearm` , `Hand` , `Thigh` , `Calf` , `Foot` , `Toe0` , y `mixamorig:HeadTop_End` por encima de la cabeza. Las articulaciones adicionales (dedos, colas, ayudantes) siguen a su antepasado bípedo más cercano. **Verificado** en Leoma , Crevan , Crabman and the Crab Queen (exportaciones de Meshy ).
- Una textura de color base; Cuando están presentes, se utilizan una rugosidad metálica y una textura normal.
- Cualquier postura de descanso: la postura T y la postura A encajan (la postura vinculante del juego tiene los brazos hacia abajo).

<a id="how-the-fit-works"></a>
## Cómo funciona el ajuste

1. Cada vértice se transforma en la postura de reposo que muestra el archivo (transformación de reposo de unión de tiempos de unión inversa).
2. Escala: la altura del hueso de la cabeza del juego sobre la articulación de la cabeza del modelo por encima de su vértice más bajo.
3. Cada hueso de la columna, el brazo y la pierna se gira para que apunte desde su articulación hacia su hijo como lo hace el juego, y se estira hasta la longitud del juego (0,6 a 1,6 veces).
4. La pelvis, la columna, el cuello y las clavículas se ajustan como una pieza rígida (mínimos cuadrados en las articulaciones del cuello, los hombros y la cadera del juego), de modo que el pecho mantenga su forma; Los ajustes por hueso lo cortan donde se mezclan los pesos de la columna y la clavícula. La pelvis y las articulaciones de la columna quedan fuera de ese ajuste: la pelvis del juego está al nivel de las caderas y encima de ellas se sitúa un exporte bípedo, que deja caer el pecho aproximadamente una unidad.
5. La cabeza aterriza a mitad de camino desde donde el torso la lleva hasta la articulación de la cabeza del juego, por lo que su malla queda debajo de su hueso. El cuello del juego se eleva aproximadamente una unidad por encima de sus hombros y el de estos modelos apenas lo hace: al colocar la cabeza en la articulación, se estiró el cuello hasta aproximadamente tres veces su longitud (**Observado** en el macho Leoma , Crabman y Crab Queen: cuello ajustado 2,4 unidades, el de los modelos 0,8, el del juego 1,4).
6. Los pies se mantienen erguidos y se aplastan para que las plantas aterricen en el suelo (las patas y las patas de cangrejo son más altas que el pie del juego).
7. Cada vértice es la suma ponderada de los ajustes de sus articulaciones; sus pesos van a los huesos del juego, le siguen las articulaciones y se guardan los cuatro más pesados.

**Verificado**: las mallas de la herramienta C# coinciden con las del prototipo original de Python (posiciones dentro de 1e-5 unidades, pesos óseos idénticos) en Crabman.

<a id="slimming"></a>
## Adelgazar

La ropa del juego está modelada alrededor del cuerpo humano de la plataforma, por lo que un cuerpo es más voluminoso de lo que se ve. Alrededor de cada torso y hueso de las extremidades (pelvis, columna vertebral, clavículas, parte superior e inferior de los brazos y piernas), la herramienta mide la extensión máxima del cuerpo humano por cada sexto del hueso y por cada doceavo de vuelta alrededor del mismo; un vértice ajustado más allá de esa extensión más 0,15 unidades (1,5 cm de tela) conserva una quinta parte de su exceso, cuya corrección se mezcla con el peso del hueso. La corrección se desvanece entre 1,5 y 2,5 veces el límite (más allá: un apéndice como una garra, dejado solo), es cero para un vértice fuera de las rodajas del hueso (una cola que cuelga debajo de la pelvis) y se suaviza sobre la superficie (12 rondas de vecino en promedio, costuras soldadas con rayos ultravioleta) para que los vértices vecinos se muevan por igual. **Observado** (macho Leoma ): El 39% de los vértices se mueven, 1,3 cm de media y 9,5 cm como máximo.

<a id="bust"></a>
## Busto

El busto de un cuerpo femenino se eleva al de la mujer humana (**Observado**: las mujeres Leoma y Crevan sentaron el suyo 2,0 y 1,75 unidades más abajo, debajo del pecho de la ropa y del busto del mapa parcial). El busto está en la mitad del corte de un cuarto de unidad, entre `Spine1` y los hombros y entre 0,3 y 1,8 unidades al lado de la línea media, cuyo vértice más frontal está más adelantado. Cada vértice se eleva por la diferencia multiplicada por el peso de los huesos del torso, multiplicada por exp(-(dy / 1,5)²) de su altura desde el busto, multiplicada por un paso suave desde la profundidad de la columna (0) hasta la profundidad del busto (1), de modo que la espalda, los brazos y las piernas permanecen. Luego carreras adelgazantes.

<a id="human-only-bones"></a>
## Huesos exclusivos para humanos

El esqueleto femenino tiene `L Boob` y `R Boob` (el tamaño del pecho, la altura y el espaciado del editor escalan y se mueven, motor docs/animation.md), de los cuales carece un equipo bípedo. Después de adelgazar, cada vértice cuyo hueso más pesado sea `Spine1` o `Spine2` toma los pesos exclusivamente humanos del vértice humano más cercano que tenga algo, dentro de 1 unidad, escalado en 1 - distancia; sus propios pesos se encogen para hacer espacio. **Observado** ( `human_female.mesh` ): los vértices del busto son llevados por los dos huesos (892 de los 994 vértices frontales del pecho). Los huesos ( `L Boob` 28, `R Boob` 29) y los otros mangos del aparejo femenino difieren de los del macho ( `R Clavicle` 23 contra 25; el macho tiene `Bip01 Jaw` ), por lo que las mallas siempre se escriben contra los mangos de su propio aparejo.

<a id="part-map"></a>
## Mapa de piezas

El `part map male` / `female`  PNG de la carrera marca a qué parte del cuerpo pertenece cada vértice; El `hide parts` de los artículos desgastados deja de dibujar esas partes (motor docs/characters.md "Partes ocultas del cuerpo"). La herramienta proporciona a cada vértice ajustado la parte del vértice del cuerpo humano más cercano que lleva el mismo hueso, lee del mapa de partes del equipo en la uv de ese vértice y la escribe en la uv propia del vértice ajustado (1024 × 1024, otros texels transparentes). Cada parte humana crece primero en 0,8 unidades (un vértice sin parte toma la del vértice marcado más cercano dentro de esa distancia), ya que un cuerpo más voluminoso se engancha sobre una banda más ancha.

**Observado** ( `MaleMask_Default.png` , 256², muestreado en `human_male.mesh` uvs como los muestra el juego): 642 de 15,504 vértices tienen una parte: roja en la parte superior del pecho y los hombros, bandas blancas alrededor de la parte superior de los brazos, verde en el frente del vientre. `hide parts` en los datos base (rebirth.mod): Chaqueta de cuero acolchada y de vagabundo 1, abrigo largo y guardapolvo sin mangas 2, abrigo largo y guardapolvo 3, Ninja Gi y Rags 4, túnicas 5, chaquetas de placas, trajes de piel y armadura de cangrejo 7; Trapos de Holy Servant, pantalones de monje y capuchas 0. Los elementos con 0 no ocultan nada; adelgazantes cubre esos. **Observado** ( `FemaleMask_Default.png` en `human_female.mesh` uvs): rojo el busto (1.102 vértices), blanco las bandas del brazo (560), verde el vientre (250).

<a id="textures"></a>
## Texturas

- `_d` : color base, alfa = brillo = (255 - rugosidad) / 2 (rugosidad 200 sin mapa); 2048², BC3 con mips.
- `_n` : diseño de personajes del juego, X en alfa e Y en verde, Y volteada desde +Y de glTF hacia arriba; rojo y azul 0.
