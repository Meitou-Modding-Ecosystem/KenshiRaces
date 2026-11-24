<a id="structures-from-3d-models"></a>
# Estructuras de los modelos 3D

Póngase a `.glb` El modelo (Meshy, Hi3D, un blender de exportación) en su modo `Structures` La carpeta y el juego lo convierten en un edificio que puede colocar desde el menú: Ogre mesh, texturas comprimidas, colisión y los registros en `<Mod>.mod`. No FCSNo hay Blender, no hay instalación necesaria para la importación.

<a id="quick-start"></a>
## Inicio rápido

1. Póngase el modelo en `<Mod>/Structures/`Y el g. `MyMod/Structures/watch-tower.glb`.
2. Opcional: además de `watch-tower.json` Con cualquier [Establecimiento](#settings)Y el g.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Construcción y implementación como siempre ([play.md](play.md)• Visual Studio F5, VS Código F5, o `dotnet run --project Play`. El registro muestra una
`Structure ...` La línea por modelo.
4. En el juego: construir el menú, categoría `EXTERIOR` (o lo que has puesto) Con el default `"research": "start"` Se conoce desde el comienzo de la
juego, y en los saltos existentes también (Meitou trabaja con los desechos de la investigación terminada, que siempre incluye `_Default Start`).
5. Con el modelo, su `.json` y `Structures/ids.json` (Escrito en la primera entrada) Los ficheros generados son de producción y no son
Comprometido por

Sin jugar: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## Establecimiento

Todos los productos (Opcional)`<model>.json`, comentarios permitidos; un nombre desconocido es un error para que los tipos no se van desapercibidos).

| Establecimiento | Definición | Significado |
| --- | --- | --- |
| `name` | Nombre de archivo, `-`/`_` Como espacios | Nombre en el menú de construcción |
| `description` | `""` | Creación de Menú Texto |
| `height` | El tamaño del modelo | La altura terminada en metros; el modelo se escaló igualmente a él. Los modelos de AI vienen en tamaños arbitrarios, por lo que |
| `sink` | `0` | metros caen en el suelo (que lo mantiene de flotar en las cajas) |
| `yaw` | `0` | grados girando alrededor del ácido vertical |
| `category` | `EXTERIOR` | La lista de menú (`building category`El juego (`BUILDINGS`, `CAMPING`, `FARMING`...o un nuevo nombre |
| `group` | `""` | `building group`: edificios compartiendo uno son variantes de una entrada |
| `materials` | `5` | El material que cuesta |
| `buildSpeed` | `2` | `build speed mult` (Tendas del juego 5, casas 1 a 2) |
| `research` | `start` | `start`conocido desde el comienzo; `none`No hay nada que lo desbloquee (su propio registro hace); o un RESEARCH Título: Título ID, e.g `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` o `both` |
| `createsTown` | `true` | Ponerlo fuera de una ciudad comienza un outpost |
| `collision` | `mesh` | `mesh` (una copia simplificada del modelo), `box` (en el caso de la caja) o `none` (Caminando por ella) |
| `collisionTriangles` | `2000` | La mayoría de los triángulos `mesh` La colisión se mantiene (12 a 50 000) |
| `textureSize` | `2048` | mayor lado de textura en píxeles (una potencia de dos, en un máximo de 8192) |

<a id="item-models"></a>
## Modelos de objetos

Poner un modelo de elemento en `<Mod>/ItemModels/` En lugar de eso (`Structures` Lo haría un edificio). La importación escribe su mezquita y sus texturas junto a las estructuras" y MATERIAL_SPECS_CLOTHING El registro; su mod ITEM Los registros, entonces, señalan a ellos. El registro muestra una `Item ...` línea por modelo con los nombres a usar.

1. `MyMod/ItemModels/lantern.glb`Opcionalmente `lantern.json`: `{ "size": 0.3 }`.
2. En su registro de artículos (generador de datos o fase de datos), establezca:

El valor de los campos
   | --- | --- |
| `mesh` y `ground mesh` (Filánamés) de la `<mod>_<model>.mesh`Y el g. `mymod_lantern.mesh` | | `material` (en el caso de la `<950000 + 100 × block>-<Mod>.mod`El bloque de `ItemModels/ids.json`Y el g. `950000-MyMod.mod` |

En primer lugar, ver el material (`GameDatabase.Find`y mantenga el aspecto de la stand-in cuando está ausente: un zip liberado, y los test que leen el juego sin el modo desplegado, no lo tienen.
3. Con el modelo, su `.json` y `ItemModels/ids.json`.

| Establecimiento | Definición | Significado |
| --- | --- | --- |
| `name` | Nombre de archivo, `-`/`_` Como espacios | El nombre del registro material |
| `size` | El tamaño del modelo | el lado más largo en metros; el modelo es escalado justo a él |
| `yaw` | `0` | grados girando alrededor del ácido vertical |
| `textureSize` | `1024` | mayor lado de textura en píxeles (una potencia de dos, en un máximo de 8192) |

- Un material por elemento (la ITEM Los nombres uno `material`El modelo se encuentra en su punto más bajo, centrado, como lo hacen las mesas del juego.
- El juego atrae un elemento en el suelo, en una mano y en un icono automático con su `mesh` (`ground mesh` En primer lugar, en la tierra y en
MATERIAL_SPECS_CLOTHING Su `material` Nombre de los motores `docs/formats/runtime-materials.md` “Los llamadores”, `WorldObjectLayout.ItemMesh`).
- El icono de inventario se extrae del modelo a menos que los registros se `auto icon image` La falsa y su propia `icon`.

<a id="getting-a-good-model"></a>
## Obtener un buen modelo

- Pregunta Meshy o Hi3D Para el A PBR-Descubrir el modelo y descargarlo como GLB. Algunas decenas de miles de triángulos son abundantes; la bandera del log es un modelo
Más de 150.000. Cortar el polígono, elegir el tamaño y cortar las texturas: [model-prep.md](model-prep.md).
- En el caso de que se trate de un modelo +Z en el archivo (glTFde la convención); uso de `yaw` para girarlo.
- Cada material se convierte en su parte. Generadores generadores generalmente dan uno; en el máximo 49.
- El modelo se encuentra en su punto más bajo, centrado.

<a id="what-it-writes"></a>
## Lo que escribe

En la página web de la página web (`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` 1 por material, `_1`, `_2`, ... cuando hay varios: Ogre `[MeshSerializer_v1.8]`, posición, normal, UV y
tangent con la manedumbre en w, el diseño de las mesas de construcción del juego.
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: difuso (BC3), normal (BC1, BC3 cuando se cortan), metalicidad (BC1), con mips.
- `structures/<mod>_<model>_col.xml`La colisión .
- Para un modelo de objetos: `structures/<mod>_<model>.mesh` y sus texturas, sin colisión.
- Los registros se mezclan `<Mod>.mod` Además de cualquier otra cosa que se pueda ver en ella (los registros de un generador de datos se mantienen; se sustituyen las importaciones anteriores):
BUILDINGY uno BUILDING_PART y MATERIAL_SPEC por material, y el desbloqueo (la investigación `enable buildings` El edificio se encuentra en el centro; uno MATERIAL_SPECS_CLOTHING por el modelo.

Los nombres de los archivos comienzan con el nombre del mód porque el juego muestra los recursos por el nombre del archivo en cada módulo.

Los registros son `<n>-<Mod>.mod` con n a partir de 900000 a;        (construcción +0, piezas +1, materiales +50); el número de modelos de artículos a partir de 950000 (material +0) `ItemModels/ids.json`. `Structures/ids.json` Mantiene el bloque de cada modelo para que los identificadores no cambien nunca: un salvador llama a un edificio ubicado por el identificador de su registro. Nunca editar o eliminar un bloque; eliminar un modelo remove su edificio de los juegos que lo colocan.

<a id="facts-the-import-relies-on"></a>
## La importación depende de

Se han visto los archivos del juego (Kenshi 1.0.68 datos) y con los lectores del motor:

- **Unidades**: la unidad mundial es de 0,1 m, Y arriba; glTF es metros, Y arriba, por lo que la escala es 10 (los archivos de colisión del juego utilizan la gravedad 98.1).
- **Materiales**El aspecto de un edificio es su MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`No, no es la mezquita
Nombre del material (Motor `docs/formats/runtime-materials.md`). El juego de la construcción de Shāga (`data/materials/deferred/objects.hlsl`Lea la difusión RGB como color y A como brillo (temas `specular mult`• Normalidad RGB como el tangent-space normal y A como la máscara de corte (`material type` 1, `alpha threshold`El metal de R. La importación escribe gloss = 1 - glTF El metal y el metal de glTFEl canal azul; glTF MASK y BLEND Se convierte en un cut-out.
- **Tango**El sistema binárico de la imagen de la imagen es cruzado (tangente, normal) × w, y en las mesas del juego señala a lo largo del aumento de V (**Verificado**:
`prisoner_pole.mesh` 86 de los triángulos, `Camp_tent1.mesh` 1858 y 1865. glTF Los mapas normales ponen verde la imagen (decorando V), por lo que la importación vuelve el canal verde y establece el camino del juego.
- **Colisión**• Una parte de `xml collision` es un archivo de NXX; un archivo escrito a mano de carga mínima (el juego `Camp_tent1_COL.xml` Es el
una cuchara) PhysX es Z y los mapas del juego (x, y, z) a (x, z, -y), por lo que la importación escribe los puntos de la parte como (x, -z, y). La mezcla de triángulo entra como puntos y triángulos; el juego la cocina, Meitou la lee como es. `docs/formats/collision.md`).
- **Los campos**: BUILDING y BUILDING_PART Por favor, envíe la entrada completa de la página web de la página Web del juego (`18026-small_changes_otto.mod`Por lo tanto nada
Se encuentra en un defecto; no `functionality`Por lo tanto, es un edificio plano.
- **Desbloquear**: un edificio está en el menú de construcción una vez que una investigación terminada lo lista en `enable buildings`Cada juego comienza con
`_Default Start` (`5359-gamedata.base`Y terminó. **Verificado** Con el motor de `GameDatabase`, `ResearchGate` y `BuildingBuildData` sobre el juego de base más un modo importado: las estructuras son desactivadas y costan los materiales de construcción como se establece.

<a id="not-covered-yet"></a>
## No cubierto aún

Interiores, puertas, funciones ( almacenamiento, camas, diseño) y piezas de vuelta.
