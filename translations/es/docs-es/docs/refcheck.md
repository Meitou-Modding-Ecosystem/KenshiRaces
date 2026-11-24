<a id="engine-references-refcheck"></a>
# Referencias de motores ( RefCheck )

Un mod se construye contra un motor (el submódulo `engine` de este repositorio) y se reproduce en cualquier versión de Meitou que tenga el reproductor. Cuando una versión cambia la firma de algo que llama el mod, es posible que la fuente del mod aún se compile, pero el DLL que ya se creó le pregunta al juego por el miembro anterior y falla en tiempo de ejecución con `MissingMethodException` .

Esto no es hipotético. Meitou 0.21 le dio a `Inventory.Add(ItemInstance)` un segundo parámetro opcional ( `Add(ItemInstance, bool mainShut = false)` ). La fuente que llama a `bag.Add(item)` se compila con cualquiera de las versiones, pero un DLL creado con 0.20 o anterior llama al método de un parámetro, que 0.21 ya no tiene. En The Wasteland Remembers, se rompieron los recuerdos en juego y la excepción detuvo todo un sistema por el resto de la sesión. Ninguna prueba lo detectó: las pruebas se compilan en un motor.

<a id="what-it-checks"></a>
## lo que comprueba

`tools/RefCheck` lee los metadatos de un mod construido DLL y enumera cada miembro del motor al que hace referencia (métodos, constructores, campos de tipos en `Meitou*.dll` ), por nombre y tipos de parámetros, que una versión determinada no tiene. No se ejecuta nada del mod o del juego.

`tools/refcheck.sh` compila cada mod en el repositorio (cada carpeta de nivel superior con `meitou.json` y `<Folder>/<Folder>.csproj` ), descarga el zip win-x64 de una versión una vez (manteniendo solo `Meitou*.dll` y `0Harmony.dll` ) y verifica cada ensamblaje en las listas `meitou.json` del mod.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Salida, una línea por versión y ensamblaje:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

o, para un descanso:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## En CI

El trabajo `Engine references` ejecuta `tools/refcheck.sh` con la versión más reciente en cada PR y push, y `Release readiness` lo espera. Un mod que envía un DLL construido con un motor más antiguo también debe verificar ese DLL con cada versión que permita su `meitou.json` .

<a id="when-it-finds-something"></a>
## Cuando encuentra algo

Llame al miembro de una manera que funcione en cada versión que admita: reconstruya con el motor más nuevo si elimina los más antiguos, o busque el miembro por reflexión y complete los parámetros que no pasa con sus valores predeterminados. `InventoryCompat` de Wasteland Remembers hace lo último para `Inventory.Add` .

Las llamadas de reflexión en el motor no se verifican: fallan suavemente donde las escribiste.
