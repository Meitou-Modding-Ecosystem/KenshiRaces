<a id="preparing-a-model"></a>
# Preparar un modelo

Cómo conseguir una generación `.glb` (Meshy, Hi3D, Blender) a un polígono, tamaño de textura y tamaño real del juego, antes de que se entre en `<Mod>/Structures/` (construcción de edificios) o `<Mod>/ItemModels/` y el tema. La importación en sí misma: [structures.md](structures.md).

Los comandos se ejecutan desde la raíz repo en PowerShell. Necesidad de Python 3 `prepare-model.py` También necesitan `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. Inspección

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Se imprime el tamaño en metros (X, Y arriba, Z), el lado más largo, los materiales, y el tamaño de los píxeles de cada textura y MB. Una nota sobre la escala de nodo o el significado de la rotación `prepare-model.py` No se puede reorganizar: se exporta de nuevo desde Blender con transformaciones aplicadas (Objeto > Aplicación > Todas las transformaciones).

<a id="2-cut-the-polycount"></a>
## 2. Cortar el poligono

| Bien | Triángulos | Texturas |
| --- | --- | --- |
| Edificio o decoración | entre 5.000 y 20.000 | 2048 |
| El producto (hasta, caído, en una tienda) | Menos de 5.000 | 1024 |

Los generadores a menudo dan 100.000 o más; las importaciones son más de 150.000. Para cortarlo:

- **Meshy:** Remete el modelo con un polígono objetivo en el presupuesto anterior (tanto la topología como el triángulo funcionan), manteniendo las texturas y
Descargar como GLB de nuevo.
- **El Blender:** Archivo de la etiqueta: Import glTF, seleccione la mezcla, adicione un Modificador Decimado (Collapse) y la Ratio inferior hasta que el rostro cuente en el estado
La barra está en el presupuesto, aplique, luego Ficha > Exportación > glTF El binario (`.glb`Con la aplicación de los modificadores.

El proceso de desaceleración se simplifica por separado (`collisionTriangles`, por defecto 2000), así que cortar la mezcla visible es acerca de la tasa de marco y el tamaño del archivo.

<a id="3-choose-the-size"></a>
## 3. Elegir el tamaño

La importación trata el modelo como metros; el tamaño de un generador es arbitrario. Voy a pegar el tamaño real y ponerlo en el archivo de configuración del modelo ([Sitio web.md “Settings”](structures.md#settings)No en el modelo:

- El edificio: `"height"` En los metros de `<model>.json`. KenshiLas personas son de aproximadamente 1,8 metros de altura y leyen pequeños además de los tamaños del mundo real, por lo que toman el
tamaño que describirías y añadirás 20% (un post de 2,2 m se convierte en `2.64`).
- El objeto: `"size"`El lado más largo en metros, `<model>.json` Al lado de ella, en `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. Descargar las texturas

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Esto reafirma el archivo en el lugar con la mayor parte de los píxeles de un lado (2048 para los edificios, 1024 para los objetos), generalmente cortado de decenas de MB. Un tercer argumento también desliza al lado más largo en metros, para cuando no se desea un archivo de configuración. El precio de las entradas es más alto (`textureSize`Pero cometiendo el archivo reducido mantiene el repo pequeño.

correr `model-info.py` De nuevo ver el resultado.

<a id="5-import-and-check"></a>
## 5. Importación y verificación

Construye y despliega con Play (o despliega solo la importación, [Siguiente entradaSiguiente “El comienzo rápido”](structures.md#quick-start)). El log tiene una línea por modelo:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Véase la altura o la longitud y el número de triángulos, luego vea en el juego: colocar el edificio, o bajar y recoger el elemento.

<a id="6-commit"></a>
## 6. Compromiso

Compromiso con el `.glb`Y su `.json` Y el Folio `ids.json` Juntos en `ids.json` Mantiene los identificadores de registro de cada modelo fijados, y salva los nombres de los edificios colocados por esos identificadores.
