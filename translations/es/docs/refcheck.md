<a id="engine-references-refcheck"></a>
# Referencias de motores (RefCheck)

Un mod se construye contra un motor (este repo `engine` Submundo) y jugar en lo que Meitou libera el jugador tiene. Cuando una liberación cambia la firma de algo que el mod llama, la fuente del mod puede seguir compilando, pero el DLL que ya se construyó, piden el juego para el viejo miembro y fallan en el tiempo con `MissingMethodException`.

Esto no es hipotético. Meitou 0.21 dio `Inventory.Add(ItemInstance)` El segundo parámetro es opcional (`Add(ItemInstance, bool mainShut = false)`). La fuente que llama `bag.Add(item)` Se trata de una versión, pero una DLL construido contra 0.20 o más llama el método de un parámetro, que 0.21 ya no tiene. En The Wasteland recuerda que rompió los guardas en el juego, y la excepción detuvo todo el sistema para el resto de la sesión. No hay prueba: los ensayos se compilan contra un motor.

<a id="what-it-checks"></a>
## Lo que verifica

`tools/RefCheck` Se trata de una mod DLLLos metadatos y listas de cada miembro del motor que se refiere (métodos, constructores, campos en los tipos de `Meitou*.dll`Por nombre y parámetros, que un dato no tiene. Nada de la moda o el juego funciona.

`tools/refcheck.sh` construye todos los modos en el repo (cada pasta de nivel superior con `meitou.json` y `<Folder>/<Folder>.csproj`Descarga una versión de win-x64 zip una vez (solo `Meitou*.dll` y `0Harmony.dll`y comprobar cada reunión el mod `meitou.json` las listas.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

La salida, una línea por liberación y asamblea:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

Para una pausa:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## En el CI

El `Engine references` Trabajos RUNS `tools/refcheck.sh` contra el último lanzamiento en cada PR y presión, y `Release readiness` Esperando por eso. Un barco que lleva a DLL El motor que se construye contra un motor mayor debe ser DLL Cada uno de ellos se libera `meitou.json` Permite que.

<a id="when-it-finds-something"></a>
## Cuando se encuentra algo

Chamar al miembro de una manera que funciona en cada lanzamiento que usted apoya: reconstruir contra el motor más nuevo si se pierde el viejo, o encontrar el miembro por reflexión y rellenar los parámetros que no pasa con sus defectos. El desierto recuerda `InventoryCompat` ¿Por qué la última `Inventory.Add`.

Las llamadas de reflexión en el motor no se verifican: fracasan suavemente donde las escribió.
