<a id="build-and-run-locally"></a>
# Construir y ejecutar localmente

El edificio `Play` construye el motor y cada mod en este repo, copia los modos y los `meitou.lock.json` Los nombres se encuentran en la carpeta de los modos del juego, y luego se inicia el juego. Utilice cualquier herramienta que le guste: Visual Studio, VS Code o un terminal que realice la misma construcción.

Todos los tres necesitan.NET 11 SDK y un clan con sus submodulos (`git clone --recurse-submodules <repo URL>`o `git submodule update --init --recursive` en una clona existente). Sin ellos, la construcción se detiene `MEITOU001`que se llama ese mandamiento.

<a id="visual-studio"></a>
## Visual estudio

1. Abre el repo `.slnx`.
2. Sitio `Play` Como proyecto de inicio, se vea a disposición de Visual Studio si no lo ha seleccionado (clicar a la derecha > Configurar como proyecto de inicio).
3. Seleccione un perfil en el botón de descifrado junto al botón Inici, y luego presione F5 (debug) o Ctrl+F5:

| El perfil | Corridas | Utilice para |
| --- | --- | --- |
| Juego (Motor + Mods) | `MeitouClient.exe` construido en este repo `engine` Submodulo | Testar y desbloquear su mod; los puntos de desbloqueo en el trabajo de código mod |
| Meitou launcher (instalar) | El lanzador instalado desde el Meitou libera, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | Modificar la lista de modos o poner la orden de carga y confiar en un nuevo código de modos |

<a id="vs-code"></a>
## El Código

1. Instalar la extensión C# Dev Kit (VS Code la ofrece cuando se abre la pasta; está en `.vscode/extensions.json`).
2. Abre la carpeta de raíz del repo (File > Open Folder).
3. Descargar y descargar (Ctrl+Shift+D) **Juego (Motor + Mods)**La impresión de F5. Construye y despliega primero, luego comienza el motor bajo el
Debugger; puntos de desgaste en el trabajo de código mod.
4. Para el lanzador instalado: Terminal > Tareas de ejecución > **Meitou launcher (instalar)**. Construye y desploma, luego abre el lanzador.

Terminal > Run Build Task (Ctrl+Shift+B) sólo construye y despliega.

<a id="without-an-editor"></a>
## Sin un editor

Clica derecha `tools\Build.ps1` > **Con PowerShell**. Se construye y se despliega como `dotnet build` (ver los submodulares primero si están faltando) y mantener la ventana abierta para que pueda leer el resultado. `-Test` También se realizan las pruebas. `-Run` Entonces comienza el motor.

<a id="terminal"></a>
## Terminales

En la carpeta de la raíz del repo:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` y `dotnet test` Construir y probar cada proyecto.
- `dotnet run --project Play` Construye, implementa y comienza el motor (el perfil de "Play (motor + mods)").
- Para utilizar el lanzador instalado, desplegarse sin iniciar el motor, y abrir el lanzador:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## Lo que hace un juego

- Encuentra los mods: cada posesión de la carpeta de alto nivel `meitou.json` y `<Folder>/<Folder>.csproj`. Un nuevo modo no necesita cambios `Play`.
- Construcción del motor y cada mód **liberación**, cualquiera que sea la configuración que usted haya elegido: el motor de desgaste construye el lanzamiento en `TaskStart.CheckStarts`
Una vez que un mod comienza la tarea de un personaje. Los puntos de desviación en el código de modo que sigan funcionando (los símbolos de liberación se despliegue).
- Copia de cada mod `<Mod>.dll`, `<Mod>.pdb` y `meitou.json` para `<Kenshi>\mods\<Mod>\`. Cerrar el juego primero: un juego corriente bloquea el dll.
- Copiar cada marco en `meitou.lock.json` de su descarga de la cartera a `<Kenshi>\mods\<Framework>\`, o, para un marco construido
En el caso de un clón ([local-development.md](local-development.md)y su producción construida.
- Un sistema de control de datos con un sistema de control (`<Mod>.Data/<Mod>.Data.csproj`y correr como `<generator> <Kenshi folder> <output .mod>`Se obtiene su `<Mod>.mod`
generado del juego instalado; un fracaso es un aviso de construcción y el modo se implementa sin él. Todos los demás modos reciben un vacío `<Mod>.mod` Cuando no hay uno, el que existe nunca se sustituye.
- Un mod con un `Structures` El archivo tiene su `.glb` Los modelos importados en su paquete, los registros incorporados en su `.mod`
([structures.md](structures.md)).
- Cuando la carpeta de modos no existe, nada se copia y el edificio dice que es así.
- El botón de juego del lanzador comienza el lanzamiento que ha instalado, no el motor de este repo. Cerrar y ejecutar "Play (motor + mods)" para probar su
Construir .

<a id="settings"></a>
## Establecimiento

Deficiencias: Juego `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`El Mods en el mapa `<game>\mods`. Cambiarlos con el `KENSHI_PATH` variable ambiental (el motor también lo lee, por lo que ambos están de acuerdo), o por máquina en un git-ignorado `Meitou.user.props` En la carpeta de la raíz del repo:

```xml
<Project>
  <PropertyGroup>
    <KenshiDir>D:\Games\Kenshi</KenshiDir>           <!-- game folder; also sets the mods folder -->
    <KenshiModsDir>D:\Games\Kenshi\mods</KenshiModsDir> <!-- mods folder only -->
    <PlayConfiguration>Debug</PlayConfiguration>      <!-- engine and mods configuration (default Release) -->
    <PlayDeploy>false</PlayDeploy>                    <!-- build and run without copying -->
    <PlayArguments>--continue</PlayArguments>         <!-- engine command line (Visual Studio and dotnet run) -->
  </PropertyGroup>
</Project>
```

El motor encuentra el juego por sí mismo (Steam); `KenshiDir` Sólo en el `Meitou.user.props` No se ha pasado a eso. La configuración de lanzamiento de VS Code funciona con el motor de lanzamiento; Ese `.vscode/launch.json` local, si se cambia `PlayConfiguration`.

<a id="first-run"></a>
## Primera ronda

- Desarrollar el lanzamiento instalado una vez (cada una de las formas anteriores): habilitar el modo y sus placas, y confiar en su código.
- Si la copia fracasó con "acceso rechazado", dar a su usuario los derechos de Modificación en `Kenshi\mods` (Propiedades > Seguridad) en lugar de llevar a cabo su
El editor como administrador.
