<a id="local-development-setup"></a>
# SISTEMA DE DESENVOLVimiento Local

Clone sólo el reposo en el que trabajas en una hoja. Cada uno construye su propio `dotnet build`Los cuadros que necesita descargar y comprobarlos.

<a id="repos"></a>
## Repos

Cada mod repo se llama por su mod.

| Repo | Mantener | PRS se va a |
| --- | --- | --- |
| MeitouClient | El motor y sus motores | `fork` |
| El rival | Rivalry Core y sus habilidades | `beta` |
| LogísticaCore | El marco de logística | `beta` |
| WarOrchestración | Marco de Orquestación de Guerra (Us Logistics Core) | `beta` |
| BastFront | Bast Front Melee (sugaja de la Orquestación de Guerra y el Núcleo de Logística) | `beta` |
| AshenHand | La mano de la aguja (utilizan el núcleo de Rivalry) | `beta` |
| El matrimonio | El matrimonio | `beta` |
| Escudo | SquadArena mod y la herramienta de clasificación de los equipos | `beta` |
| La estructura de la estructura (TheIvoryDisaster) | CI compartido, paquete, puerta de liberación, meta de bloqueo | `beta` |

<a id="folders"></a>
## Folderos

Estos pasos utilizan ocho carpetas. Configure como variables en cada nueva ventana de PowerShell, y apruebe los comandos que se encuentran a continuación:

```powershell
$Repos     = "C:\Users\<you>\source\repos"
$Engine    = "$Repos\MeitouClient"
$Rivalry   = "$Repos\RivalryCore"
$Ashen     = "$Repos\AshenHand"
$Husbandry = "$Repos\Husbandry"
$Logistics = "$Repos\LogisticsCore"
$War       = "$Repos\WarOrchestration"
$Bast      = "$Repos\BastFront"
```

<a id="prerequisites"></a>
## Prerequisitos

- .NET 11 SDK (Todos los repuestos `global.json` El nombre de la versión) y Git.
- Un editor: Visual Studio, VS Código con el C# Dev Kit, o Rider. También funciona un terminal.
- Kenshi Instalado en Si no está en `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Entonces, establece el `KENSHI_PATH` El medio ambiente variable
a su carpintería.

<a id="clone"></a>
## Clone

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Cada mod repo tiene su propia `engine` El submodulo, así `$Engine` Sólo es necesario escribir motores.

<a id="build-and-play-one-mod"></a>
## Crea y juega un mód

**Cambios en el repo de mod:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- El primer edificio descarga cada marco llamado `meitou.lock.json` En el `deps` y se detiene si la SHAEl 256 no se encuentra en el lugar.
- `dotnet run --project Play` Copia los cuadros cerrados y el mod `Kenshi\mods` Y empieza el motor. Visual Studio, VS Código
y el lanzador instalado: [play.md](play.md).
- Aplicar los marco por encima del modo en el lanzador, como para cualquier dependencia.

<a id="change-a-framework-and-a-mod-together"></a>
## Cambiar un marco y un mod juntos

Pon el mod repo en su clona de marco:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Construidas en `$Ashen` Ahora compila Rivalry Core de `$Rivalry` y utilizar el motor en `$Rivalry\engine`, y Play descarga que Rivalry Core construye.
- `Meitou.user.props` Y se ignora. Si ya tiene su Kenshi En el caso de las líneas, añade las dos líneas dentro de `PropertyGroup` En lugar de eso.
- `MeitouFrameworkSources` Se realizan los caminos del proyecto separados por `;`Y el g. `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` Nombre del motor en el que se construye el clon de marco.
- Eliminar la línea para volver al bloqueo ZIP.

**Cadenas de marco.** Bast Frente `meitou.lock.json` La Orquesta de la Guerra y el Cuerpo de Logística. Para cambiar ambos marco y Bast Front juntos, sobrevive ambos en Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

El sobrerido pasa, así que la Orquestación de la Guerra se construye contra `$Logistics` También, y todo el edificio utiliza un motor. Sobrevive sólo a la Orquestación de la Guerra y se construye contra el núcleo de la Logística llamado en el bloqueo de Bast Front.

Para enviarlo: unir el marco PR en `beta` de `$Rivalry`El que publica un préstamo `rivalry-core-<version>-beta.<commit>`. Entonces, en `$Ashen`El set `meitou.lock.json` de ese tipo y de los activos, SHA-256 y el motor se compromete a sus notas de lanzamiento, y abre el modo PR. En una cadena, el barco de arriba: el núcleo de la guerra, luego la construcción de la cerradura, luego el Bast Front cerradura ambos. Después de la liberación de la producción, mover el bloqueo a `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Cambiar el motor y un mod

Véase su rama de motor dentro del submodulo de mod repo:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- No se compromete con el nuevo indicador: los picos de motor solo se mueven a las etiquetas de lanzamiento, por lo que el cambio llega a este repo una vez que se mueve.
y liberarlo.
- Un mod que utiliza un marco también necesita el marco reconstruido en ese motor. Utilice el cuadro de arriba y comproba lo mismo
El motor en `$Rivalry\engine`.
- Comentarios para: Entra en el motor `fork`Después de que el upstream lo libere, el marco PR se mueve `$Rivalry\engine` En ese momento, la liberación
El PR se mueve con el bloqueo y `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Cambiar el motor para jugar o probar

Para jugar el último upstream, un lanzamiento o una rama de motor, cambiar el `$Engine` Clone con el guión en cualquier mod repo:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`El último compromiso de VlcekM/MeitouClient `master` (El guión añade el `upstream` a distancia en caso de ausencia).
- `-To Release`La última versión de Upstream `vX.Y.Z`.
- `-To Fork`El órgano fork `fork` La rama, atrapada
- `-Branch <name>`: una rama del foro, como una rama de PR del motor.
- sin `-Path` Se utiliza el `MeitouClient` El mapa junto al mod repo. `-Run` Construye el juego en Release y lo inicia; `-Build` Sólo
Construye lo
- Upstream, lanzamiento y `-Branch` dejar el clonado en un comité despedido; el local `master` y `fork` Las ramas no cambian. El guión
Se detiene sin cambiar nada cuando el clonado tiene cambios sin orden.
- Nunca toca el mod repo `engine` El submodulo (que es `Update-Local.ps1` y los pinos).

<a id="keep-clones-current"></a>
## Mantener los clones actuales

Después de cada empate, y cada vez que una construcción fracasará con los tipos de motores que faltan (por ejemplo. `The type or namespace name 'RelationCause' could not be found`), cierre Visual Studio y ejecutar el script de actualización en el repo del mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Se transforme en `beta` y se pone en marcha, luego se actualiza `engine` y `build/gutenstructure` A los compromisos `beta` El Pino (`git pull` Solamente
Dejarlos en los viejos). `-Branch <name>` Posee otra rama.
- Verifica cada submodulo contra su pin y imprime la versión del motor.
- que flamencen `MeitouEngineDir` y `MeitouFrameworkSources` En el `Meitou.user.props` o el medio ambiente: hacen que la construcción utilice otro
el motor o el marco de repo en lugar de los pinos de este repo. Eliminarlos a menos que cambie un marco y un mod juntos.
- `-Clean` Eliminar el git-ignorado `bin` y `obj` los archivos, por lo que no se reutiliza la construcción del viejo motor. `-Build` Corridas `dotnet build`.
- Se termina con `Up to date` Una lista de líneas marcadas `FIX`.

El siguiente edificio descarga un marco de nuevo sólo cuando `meitou.lock.json` y cambiaron.
