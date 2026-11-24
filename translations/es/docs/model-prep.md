<a id="preparing-a-model"></a>
# Preparar la geometría, las dimensiones y las texturas

Cómo obtener un `.glb` (Meshy, Hi3D, Blender) generado a un número de polígonos, tamaño de textura y tamaño real que maneja el juego, antes de pasar a `<Mod>/Structures/` (edificios) o `<Mod>/ItemModels/` (elementos). La importación en sí: [structures.md](structures.md).

Los comandos se ejecutan desde la raíz del repositorio en PowerShell. Necesita Python 3; `prepare-model.py` también necesita `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. inspeccionar

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Imprime el tamaño en metros (X, Y arriba, Z), el lado más largo, triángulos, materiales y el tamaño de píxel de cada textura y MB. Una nota sobre la escala o rotación del nodo significa que `prepare-model.py` no puede cambiar su escala: expórtelo nuevamente desde Blender con las transformaciones aplicadas (Objeto > Aplicar > Todas las transformaciones).

<a id="2-cut-the-polycount"></a>
## 2. Cortar el policuenta

| Amable | Triángulos | Texturas |
| --- | --- | --- |
| Construcción o decoración | 5.000 a 20.000 | 2048 |
| Artículo (retenido, caído, en una tienda) | menos de 5.000 | 1024 |

Los generadores suelen dar 100.000 o más; la importación advierte por encima de 150.000. Para cortarlo:

- **Meshy :**Vuelva a mallar el modelo con un recuento de polígonos objetivo en el presupuesto anterior (la topología cuádruple o triangular funciona), mantenga las texturas y descárguelo como GLB nuevamente.
- **Blender :**Archivo > Importar > glTF, seleccione la malla, agregue un modificador Diezmar (Contraer) y baje la relación hasta que el recuento de caras en la barra de estado esté dentro del presupuesto, aplíquelo, luego Archivo > Exportar > glTF Binario (`.glb`) con Aplicar modificadores activado.

La colisión se simplifica por separado (`collisionTriangles`, predeterminado 2000), por lo que cortar la malla visible depende de la velocidad de fotogramas y el tamaño del archivo.

<a id="3-choose-the-size"></a>
## 3. Elige la talla

La importación trata el modelo como metros; El tamaño de un generador es arbitrario. Elija el tamaño real y configúrelo en el archivo de configuración del modelo ([structures.md "Settings"](structures.md#settings)), no en el modelo:

- Edificio: `"height"` en metros en `<model>.json`.  La gente de Kenshi mide aproximadamente 1,8 m de altura y se lee pequeña en comparación con los tamaños del mundo real, así que toma el tamaño que describirías y agrega un 20% (una publicación de 2,2 m se convierte en `2.64`).
- Artículo: `"size"`, su lado más largo en metros, en `<model>.json` junto a él en `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. Reducir las texturas

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Esto reescribe el archivo en su lugar con cada textura como máximo esa cantidad de píxeles por lado (2048 para edificios, 1024 para elementos), generalmente recortándolo de decenas de MB a unos pocos. Un tercer argumento opcional también cambia la escala del modelo a ese lado más largo en metros, para cuando no se desea un archivo de configuración. La importación también reduce a la mitad texturas más grandes (`textureSize`), pero confirmar el archivo reducido mantiene el repositorio pequeño.

Ejecute `model-info.py` nuevamente para verificar el resultado.

<a id="5-import-and-check"></a>
## 5. Importar y verificar

Compile e implemente con Play (o ejecute la importación solo, [structures.md "Inicio rápido"](structures.md#quick-start)). El registro tiene una línea por modelo:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Comprueba la altura o la longitud y el número de triángulos, luego míralo en el juego: coloca el edificio o suelta y recoge el objeto.

<a id="6-commit"></a>
## 6. Comprometerse

Confirme el `.glb`, su `.json` y el `ids.json` de la carpeta juntos. `ids.json` mantiene fijas las identificaciones de registros de cada modelo y guarda los nombres de los edificios colocados según esas identificaciones.

[English](../../../docs/model-prep.md) · [Kenshi Races](../README.md)
