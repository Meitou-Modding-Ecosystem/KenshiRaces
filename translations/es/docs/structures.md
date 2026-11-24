<a id="structures-from-3d-models"></a>
# Estructuras a partir de modelos 3D.

Coloque un modelo `.glb` ( Meshy , Hi3D , una exportación de Blender ) en la carpeta `Structures` de su mod y la compilación de juego lo convertirá en un edificio que puede colocar desde el menú de compilación: malla de ogro, texturas comprimidas, colisión y registros en `<Mod>.mod` . Sin FCS , sin Blender , no se necesita instalación del juego para la importación en sí.

<a id="quick-start"></a>
## Inicio rápido

1. Ponga el modelo en `<Mod>/Structures/` , p.e. `MyMod/Structures/watch-tower.glb` .
2. Opcional: al lado, `watch-tower.json` con cualquier [configuración](#settings) , p.e.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Compile e implemente como de costumbre ([play.md](play.md) ): Visual Studio F5, VS Code F5 o `dotnet run --project Play` . El registro de compilación muestra una línea `Structure ...` por modelo.
4. En el juego: menú de construcción, categoría `EXTERIOR` (o la que tú establezcas). Con el `"research": "start"` predeterminado, se conoce desde el inicio de un juego, y también en las partidas guardadas existentes ( Meitou calcula los desbloqueos a partir de la investigación finalizada, que siempre incluye `_Default Start` ).
5. Confirme el modelo, su `.json` y `Structures/ids.json` (escrito en la primera importación). Los archivos generados son resultados de compilación y no están confirmados.

Sin Juego: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>` .

<a id="settings"></a>
## Ajustes

Todo opcional ( `<model>.json` , se permiten comentarios; un nombre desconocido es un error para que los errores tipográficos no pasen desapercibidos).

| Configuración | Por defecto | Significado |
| --- | --- | --- |
| `name` | nombre de archivo, `-` / `_` como espacios | nombre en el menú de construcción |
| `description` | `""` | construir texto del menú |
| `height` | tamaño propio del modelo | altura terminada en metros; el modelo se escala uniformemente a él.  Los modelos AI vienen en tamaños arbitrarios, así que configure esto |
| `sink` | `0` | metros hundidos en el suelo (evita que flote en pendientes) |
| `yaw` | `0` | grados girados alrededor del eje vertical |
| `category` | `EXTERIOR` | categoría del menú de compilación ( `building category` ): el juego ( `BUILDINGS` , `CAMPING` , `FARMING` , ...) o un nuevo nombre |
| `group` | `""` | `building group` : los edificios que comparten uno son variantes de una entrada |
| `materials` | `5` | Materiales de construcción que cuesta |
| `buildSpeed` | `2` | `build speed mult` (las tiendas de campaña 5 del juego, las casas 1 a 2) |
| `research` | `start` | `start` : conocido desde el principio; `none` : nada lo desbloquea (tus propios registros lo hacen); o una identificación de cadena RESEARCH , p. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor` , `indoor` o `both` |
| `createsTown` | `true` | Colocarlo fuera de una ciudad inicia un puesto de avanzada. |
| `collision` | `mesh` | `mesh` (una copia simplificada del modelo), `box` (su cuadro delimitador) o `none` (recorrerlo) |
| `collisionTriangles` | `2000` | la mayoría de los triángulos que mantiene la colisión `mesh` (12 a 50.000) |
| `textureSize` | `2048` | lado de textura más grande en píxeles (una potencia de dos, como máximo 8192) |

<a id="item-models"></a>
## Modelos de artículos

En su lugar, coloque el modelo de un artículo en `<Mod>/ItemModels/` ( `Structures` lo convertiría en un edificio). La importación escribe su malla y texturas junto a las estructuras y un registro MATERIAL_SPECS_CLOTHING ; El registro ITEM de tu mod luego apunta a ellos. El registro de compilación muestra una línea `Item ...` por modelo con los nombres a usar.

1. `MyMod/ItemModels/lantern.glb` , opcionalmente `lantern.json` : `{ "size": 0.3 }` .
2. En el registro de su artículo (generador de datos o fase de datos), establezca:

| Campo | Valor |
   | --- | --- |
| `mesh` y `ground mesh` (nombres de archivo) | `<mod>_<model>.mesh` , por ej. `mymod_lantern.mesh` | | `material` (referencia) | `<950000 + 100 × block>-<Mod>.mod` , bloque de `ItemModels/ids.json` , p.e. `950000-MyMod.mod` |

Busque el material primero ( `GameDatabase.Find` ) y mantenga la apariencia del sustituto cuando falte: un zip publicado y pruebas que leen el juego sin el mod implementado, no lo tienen.
3. Confirme el modelo, su `.json` y `ItemModels/ids.json` .

| Configuración | Por defecto | Significado |
| --- | --- | --- |
| `name` | nombre de archivo, `-` / `_` como espacios | el nombre del registro de material |
| `size` | tamaño propio del modelo | lado más largo en metros; el modelo está escalado uniformemente |
| `yaw` | `0` | grados girados alrededor del eje vertical |
| `textureSize` | `1024` | lado de textura más grande en píxeles (una potencia de dos, como máximo 8192) |

- Un material por modelo de artículo (el ITEM nombra un `material` ); el modelo se sitúa en su punto más bajo, centrado, como lo hacen las mallas de elementos del juego.
- El juego dibuja un elemento en el suelo, en una mano y en un ícono automático con su `mesh` ( `ground mesh` primero en el suelo) y el MATERIAL_SPECS_CLOTHING con sus nombres `material` (motor `docs/formats/runtime-materials.md` "Callers", `WorldObjectLayout.ItemMesh` ).
- El ícono de inventario se extrae del modelo a menos que el registro establezca `auto icon image` como falso y su propio `icon` .

<a id="getting-a-good-model"></a>
## Conseguir un buen modelo

- Pregunte a Meshy o Hi3D por un modelo con textura PBR y descárguelo como GLB . Unas pocas decenas de miles de triángulos son suficientes; el registro señala un modelo de más de 150.000. Cortar el polycount, elegir el tamaño y reducir las texturas: [model-prep.md](model-prep.md) .
- El frente del modelo mira hacia +Z en el archivo (convención de glTF); use `yaw` para girarlo.
- Cada material se convierte en su propia parte. Los generadores suelen dar uno; como máximo 49.
- El modelo se encuentra en su punto más bajo, centrado.

<a id="what-it-writes"></a>
## lo que escribe

En la carpeta mod implementada ( `<Kenshi>/mods/<Mod>/` ):

- `structures/<mod>_<model>.mesh` (uno por material, `_1` , `_2` ,... cuando sean varios): Ogro `[MeshSerializer_v1.8]` , posición, normal, UV y tangente con lateralidad en w, el diseño de las mallas de construcción propias del juego.
- `structures/<mod>_<model>_d.dds` , `_n.dds` , `_m.dds` : difuso (BC3), normal (BC1, BC3 cuando se corta), metalismo (BC1), con mips.
- `structures/<mod>_<model>_col.xml` : la colisión.
- Para un modelo de artículo: `structures/<mod>_<model>.mesh` y sus texturas, sin colisión.
- Los registros se fusionaron en `<Mod>.mod` junto con cualquier otra cosa en él (se mantienen los registros de un generador de datos; se reemplazan los de una importación anterior): un BUILDING , un BUILDING_PART y un MATERIAL_SPEC por material, y el desbloqueo (el `enable buildings` de la investigación gana el edificio); un MATERIAL_SPECS_CLOTHING por modelo de artículo.

Los nombres de los archivos comienzan con el nombre del mod porque el juego busca recursos por nombre de archivo simple en cada mod.

Los identificadores de registro son `<n>-<Mod>.mod` con n desde 900000 en adelante, un bloque de 100 por modelo (construcción +0, piezas +1, materiales +50); Los modelos de artículos cuentan desde 950000 (material +0) en `ItemModels/ids.json` . `Structures/ids.json` mantiene el bloque de cada modelo para que los identificadores nunca cambien: al guardar se nombra un edificio colocado por el identificador de su registro. Nunca edites ni elimines un bloque; Al eliminar un modelo, se elimina su edificio de los juegos que lo colocaron.

<a id="facts-the-import-relies-on"></a>
## Hechos en los que se basa la importación

Comparado con los archivos del juego (datos de Kenshi 1.0.68) y con los lectores del motor:

- **Unidades**: la unidad mundial es 0,1 m, Y arriba; glTF son metros, Y arriba, por lo que la escala es 10 (los archivos de colisión del juego usan gravedad 98.1).
- **Materiales**: el aspecto de una pieza de construcción es su MATERIAL_SPEC ( `texture map` , `normal map` , `metalness map` , `material type` ), no el nombre del material de la malla (motor `docs/formats/runtime-materials.md` ). El sombreador de construcción del juego ( `data/materials/deferred/objects.hlsl` ) lee RGB difuso como color y A como brillo (veces `specular mult` ), normal RGB como normal del espacio tangente y A como máscara de recorte ( `material type` 1, `alpha threshold` ), metalidad de R. La importación escribe brillo = 1 - rugosidad y metalidad de glTF del canal azul de glTF; glTF MASK y BLEND se convierten en un recorte.
- **Tangentes**: la binormal del sombreador es cruzada(tangente, normal) × w, y en las mallas del juego apunta a lo largo de V creciente (**Verificado**: `prisoner_pole.mesh` 86 de 86 triángulos, `Camp_tent1.mesh` 1858 de 1865). Los mapas normales de glTF apuntan en verde hacia arriba en la imagen (disminuyendo V), por lo que la importación gira el canal verde y establece la dirección del juego.
- **Colisión**: el `xml collision` de una pieza es un archivo PhysX NxuStream2; Se carga un archivo mínimo escrito a mano (el `Camp_tent1_COL.xml` del juego es una caja). PhysX tiene Z arriba y el juego asigna (x, y, z) a (x, z, -y), por lo que la importación escribe los puntos de la parte como (x, -z, y). La malla triangular se presenta como puntos y triángulos simples; el juego lo cocina bajo carga, Meitou lo lee tal cual (motor `docs/formats/collision.md` ).
- **Campos**: BUILDING y BUILDING_PART incluyen el conjunto de campos completo de la tienda de campaña 1 del juego ( `18026-small_changes_otto.mod` ), por lo que nada cae por defecto; no `functionality` , por lo que es un edificio sencillo.
- **Desbloqueo**: un edificio está en el menú de construcción una vez que una investigación terminada lo incluye en `enable buildings` ; Cada juego comienza con `_Default Start` ( `5359-gamedata.base` ) terminado. **Verificado** con los motores `GameDatabase` , `ResearchGate` y `BuildingBuildData` en el juego base más un mod importado: las estructuras están desbloqueadas y cuestan materiales de construcción según lo establecido.

<a id="not-covered-yet"></a>
## Aún no cubierto

Interiores, puertas, funciones (almacenamiento, camas, artesanía) y piezas giratorias.
