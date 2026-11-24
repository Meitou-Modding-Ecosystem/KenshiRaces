<a id="local-development-setup"></a>
# Configurar y actualizar los repositorios locales

Clona solo los repositorios en los que trabajas en una carpeta. Cada uno se construye por sí solo con `dotnet build`, y los marcos que necesita se descargan y verifican por sí mismos.

<a id="repos"></a>
## Repos

Cada repositorio de mod lleva el nombre de su mod.

| repositorio | Sostiene | Los RP van a |
| --- | --- | --- |
| MeitouClient | el motor y sus ganchos mod | `fork` |
| RivalidadNúcleo | Marco Rivalry Core y su habilidad de agente | `beta` |
| LogísticaCore | Marco básico de logística | `beta` |
| GuerraOrquestación | Marco de orquestación de guerra (utiliza Logistics Core) | `beta` |
| BastFront | Cuerpo a cuerpo del Frente Bast (usa Orquestación de Guerra y Núcleo Logístico) | `beta` |
| AshenMano | La mano cinérea (usa núcleo de rivalidad) | `beta` |
| Agricultura | Agricultura | `beta` |
| EquipoArena | Mod SquadArena y herramienta de clasificación de escuadrón | `beta` |
| GutenStructure (El desastre de Marfil) | CI compartido, embalaje, puerta de liberación, objetivo de bloqueo del marco | `beta` |

<a id="folders"></a>
## Carpetas

Estos pasos utilizan ocho carpetas. Configúrelos como variables en cada nueva ventana de PowerShell, luego pegue los siguientes comandos tal como están:

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
## Requisitos previos

-. NET 11 SDK (el `global.json` de cada repositorio nombra la versión) y Git.
- Un editor: Visual Studio, VS Code con C# Dev Kit o Rider. Un terminal solo también sirve.
- Kenshi instalado. Si no está en `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`, configure la variable de entorno `KENSHI_PATH` en su carpeta.

<a id="clone"></a>
## Clon

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Cada repositorio de mod tiene su propio submódulo `engine`, por lo que `$Engine` solo es necesario para escribir enlaces de motor.

<a id="build-and-play-one-mod"></a>
## Construye y juega un mod

**Cambiar al repositorio del mod:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- La primera compilación descarga cada marco nombrado en `meitou.lock.json` en `deps` y se detiene si su SHA -256 no coincide.
- `dotnet run --project Play` copia los marcos bloqueados y el mod en `Kenshi\mods` y arranca el motor. Visual Studio, VS Code y el iniciador instalado: [play.md](play.md).
- Habilite los marcos encima del mod en el iniciador, como para cualquier dependencia.

<a id="change-a-framework-and-a-mod-together"></a>
## Cambiar un framework y un mod juntos

Apunte el repositorio de mods a su clon de marco:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Las compilaciones en `$Ashen` ahora compilan Rivalry Core desde `$Rivalry` y usan el motor en `$Rivalry\engine`, y Play implementa esa compilación de Rivalry Core.
- `Meitou.user.props` se ignora por git. Si ya contiene su configuración de Kenshi, agregue las dos líneas dentro de su `PropertyGroup`.
- `MeitouFrameworkSources` toma rutas de proyectos marco separadas por `;`, p. `$Logistics\LogisticsCore\LogisticsCore.csproj`. `MeitouEngineDir` nombra el motor en el que se basa el clon del marco.
- Elimine la línea para volver al ZIP bloqueado.

**Cadenas de marco.** `meitou.lock.json` de Bast Front enumera el núcleo de logística y orquestación de guerra. Para cambiar ambos marcos y Bast Front juntos, anule ambos en Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

La anulación se transmite, por lo que War Orchestration también se basa en `$Logistics`, y toda la compilación utiliza un solo motor. Anule solo la Orquestación de Guerra y se basará en el Núcleo Logístico nombrado en el bloqueo del Frente Bast.

Para enviarlo: fusione el marco PR con `beta` de `$Rivalry`, que publica una versión preliminar de `rivalry-core-<version>-beta.<commit>`. Luego, en `$Ashen`, configure `meitou.lock.json` con esa etiqueta y el activo, SHA -256 y la confirmación del motor de sus notas de la versión, y abra el mod PR. En una cadena, envíe de arriba hacia abajo: Logistics Core, luego una construcción de War Orchestration bloqueándolo, luego Bast Front bloqueando ambos. Después del lanzamiento de producción del marco, mueva el bloqueo a `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Cambiar el motor y un mod juntos

Consulte la rama de su motor dentro del submódulo del repositorio de mods:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- No confirme el puntero del nuevo motor: los pines del motor se mueven solo a las etiquetas de lanzamiento ascendentes, por lo que el cambio llega a este repositorio una vez que lo libera el ascendente.
- Un mod que utiliza un marco también necesita que el marco se reconstruya en ese motor. Utilice la anulación del marco anterior y consulte la misma rama del motor en `$Rivalry\engine`.
- Orden para aterrizarlo: motor PR en `fork` ; después de que upstream lo lanza, el marco PR mueve `$Rivalry\engine` a esa versión, luego el mod PR mueve su bloqueo y `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Cambie el motor para jugar o probar

Para reproducir la última versión, una versión o una rama del motor, cambie su clon de `$Engine` con el script en cualquier repositorio de mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : la confirmación más reciente de VlcekM/ MeitouClient  `master` (el script agrega el control remoto `upstream` cuando falta).
- `-To Release` : la etiqueta de lanzamiento ascendente más reciente `vX.Y.Z`.
- `-To Fork` : rama `fork` de la bifurcación de la organización, extraída.
- `-Branch <name>` : una rama de la bifurcación de la organización, como la rama de un motor PR.
- Sin `-Path`, usa la carpeta `MeitouClient` al lado del repositorio de mods. `-Run` crea el juego en Release y lo inicia; `-Build` solo lo construye.
- Upstream, Release y `-Branch` dejan el clon en una confirmación separada; las sucursales locales `master` y `fork` no se modifican. El script se detiene sin cambiar nada cuando el clon tiene cambios no confirmados.
- Nunca toca el submódulo `engine` del repositorio de mod (es decir, `Update-Local.ps1` y los pines).

<a id="keep-clones-current"></a>
## Mantener los clones actualizados

Después de cada extracción, y siempre que una compilación falle y falten tipos de motor (por ejemplo, `The type or namespace name 'RelationCause' could not be found`), cierre Visual Studio y ejecute el script de actualización en el repositorio del mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Cambia a `beta` y lo extrae, luego actualiza `engine` y `build/gutenstructure` a los pines `beta` de confirmación (solo `git pull` los deja en los antiguos). `-Branch <name>` tira de otra rama.
- Comprueba cada submódulo con su pin e imprime la versión del motor.
- Marca `MeitouEngineDir` y `MeitouFrameworkSources` en `Meitou.user.props` o en el entorno: hacen que la compilación utilice el motor o marco de otro repositorio en lugar de los pines de este repositorio. Elimínelos a menos que esté cambiando un marco y un mod juntos.
- `-Clean` elimina las carpetas `bin` y `obj` ignoradas por git, por lo que no se reutiliza ninguna compilación del motor anterior. `-Build` ejecuta `dotnet build`.
- Termina con `Up to date` o una lista de líneas marcadas como `FIX`.

La siguiente compilación descarga un marco nuevamente solo cuando `meitou.lock.json` cambió.

[English](../../../docs/local-development.md) · [Kenshi Races](../README.md)
