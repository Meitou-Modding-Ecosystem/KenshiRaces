<a id="build-and-run-locally"></a>
# Construir y ejecutar localmente

La compilación de `Play` construye el motor y cada mod en este repositorio, copia los mods y los nombres de los marcos `meitou.lock.json` en la carpeta de mods del juego y luego inicia el juego. Utilice la herramienta que desee: Visual Studio , VS Code o un terminal ejecutan la misma compilación.

Los tres necesitan el . NET 11 SDK y un clon con sus submódulos ( `git clone --recurse-submodules <repo URL>` o `git submodule update --init --recursive` en un clon existente). Sin ellos, la compilación se detiene con `MEITOU001` y nombra ese comando.

<a id="visual-studio"></a>
## Visual Studio

1. Abra el `.slnx` del repositorio.
2. Configure `Play` como proyecto de inicio si Visual Studio no lo seleccionó (haga clic derecho > Establecer como proyecto de inicio).
3. Elija un perfil en el menú desplegable junto al botón Inicio, luego presione F5 (depurar) o Ctrl+F5:

| Perfil | Corre | Úselo para |
| --- | --- | --- |
| Jugar (motor + mods) | `MeitouClient.exe` construido a partir del submódulo `engine` de este repositorio | probar y depurar tu mod; puntos de interrupción en el trabajo del código mod |
| Lanzador Meitou (instalado) | el lanzador instalado desde las versiones Meitou , `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | cambiar la lista de mods o el orden de carga y confiar en el código de un nuevo mod |

<a id="vs-code"></a>
## VS Code

1. Instale la extensión C# Dev Kit ( VS Code la ofrece cuando abre la carpeta; está en `.vscode/extensions.json` ).
2. Abra la carpeta raíz del repositorio (Archivo > Abrir carpeta).
3. Ejecutar y depurar (Ctrl+Shift+D), seleccione **Reproducir (motor + mods)**, presione F5. Primero construye y despliega, luego arranca el motor bajo el depurador; Los puntos de interrupción en el código mod funcionan.
4. Para el iniciador instalado: Terminal > Ejecutar tarea > ** Lanzador Meitou (instalado) **. Se construye y se implementa, luego abre el iniciador.

Terminal > Ejecutar tarea de compilación (Ctrl+Shift+B) solo compila e implementa.

<a id="without-an-editor"></a>
## Sin editor

Haga clic derecho en `tools\Build.ps1` > **Ejecutar con PowerShell**. Se construye y se implementa como `dotnet build` (comprobando primero los submódulos si faltan) y mantiene la ventana abierta para que pueda leer el resultado. `-Test` también ejecuta las pruebas;  Luego, `-Run` arranca el motor.

<a id="terminal"></a>
## Terminal

En la carpeta raíz del repositorio:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` y `dotnet test` construyen y prueban cada proyecto.
- `dotnet run --project Play` construye, implementa e inicia el motor (el perfil "Jugar (motor + mods)").
- Para utilizar el iniciador instalado, implemente sin iniciar el motor y luego abra el iniciador:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## ¿Qué hace una construcción de juego?

- Encuentra las modificaciones: todas las carpetas de nivel superior que contienen `meitou.json` y `<Folder>/<Folder>.csproj` . Un nuevo mod no necesita cambios en `Play` .
- Crea el motor y cada mod en **Lanzamiento**, cualquiera que sea la configuración que hayas elegido: las compilaciones del motor de depuración incluyen `TaskStart.CheckStarts` una vez que un mod inicia la tarea de un personaje. Los puntos de interrupción en el código mod aún funcionan (se implementan símbolos de versión).
- Copia el `<Mod>.dll` , `<Mod>.pdb` y `meitou.json` de cada mod a `<Kenshi>\mods\<Mod>\` . Primero cierre el juego: un juego en ejecución bloquea la dll.
- Copia cada marco en `meitou.lock.json` desde su carpeta de versión descargada a `<Kenshi>\mods\<Framework>\` o, para un marco creado a partir de un clon ([local-development.md](local-development.md) ), su resultado de compilación.
- Un mod con un generador de datos ( `<Mod>.Data/<Mod>.Data.csproj` , ejecutado como `<generator> <Kenshi folder> <output .mod>` ) obtiene su `<Mod>.mod` generado a partir del juego instalado; una falla es una advertencia de compilación y el mod se implementa sin ella. Todos los demás mods obtienen un `<Mod>.mod` vacío cuando no tienen ninguno; uno existente nunca se reemplaza.
- Un mod con una carpeta `Structures` obtiene sus modelos `.glb` importados en su carpeta implementada y los registros se fusionan en su `.mod` ([structures.md](structures.md) ).
- Cuando la carpeta de mods no existe, no se copia nada y la compilación lo dice.
- El botón Reproducir del iniciador inicia la versión que instaló, no el motor de este repositorio. Ciérralo y ejecuta "Play (motor + mods)" para probar tu compilación.

<a id="settings"></a>
## Ajustes

Valores predeterminados: juego `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` , carpeta de mods `<game>\mods` . Cámbielos con la variable de entorno `KENSHI_PATH` (el motor también la lee, por lo que ambos están de acuerdo), o por máquina en un `Meitou.user.props` ignorado por git en la carpeta raíz del repositorio:

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

El motor encuentra el juego por sí solo (Steam); `KenshiDir` configurado solo en `Meitou.user.props` no se le pasa.  La configuración de lanzamiento de VS Code ejecuta el motor de lanzamiento; edite `.vscode/launch.json` localmente si cambia `PlayConfiguration` .

<a id="first-run"></a>
## Primera carrera

- Ejecute el iniciador instalado una vez (cualquiera de las formas anteriores): habilite el mod y sus marcos, y confíe en su código.
- Si la copia falla con "acceso denegado", otorgue a su usuario derechos de Modificación en `Kenshi\mods` (Propiedades > Seguridad) en lugar de ejecutar su editor como administrador.
