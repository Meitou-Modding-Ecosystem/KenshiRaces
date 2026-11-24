<a id="publishing-to-the-steam-workshop"></a>
# Publicado en el Steam Workshop

Crea el elemento de Workshop de modos una vez, por mano, de una versión ZIP. Después de eso, [El taller de Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) ejecuta en su PC de Windows y carga cada nueva versión de GitHub a ese elemento en un horario. Todos los pasos se realizan en **Windows PowerShell** En su PC, se inscribe en la cuenta de Steam que posee el artículo.

Los comandos utilizan estos valores. Configure una vez por la ventana de PowerShell, con el nombre de la pasta de su mod (como en `release.yml` `mod-folder`y su primera versión:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Descargar el Mod

1. Mezcla `beta` En el `main`Entonces, abriendo y fusionando un PR `main` En el `release` ([Las ramas](../README.md#branches)).
2. Espera el flujo de trabajo de liberación en `release`. Se trata de la fusión `<slug>-v<version>` Con la versión en `meitou.json` (por ejemplo
`example-mod-v0.1.0`y crea una liberación con `<Mod>-<version>.zip` y `<Mod>-<version>.zip.sha256`. Si esto no es posible antes de que se haga una prueba, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` La entrada se hace de la mano (`-DryRun` Sólo imprimir las órdenes).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Instalar Workshop Watcher y inscribirse en Steam

Seguimos [Instalación](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) En la guía del Workshop Watcher. El installer descarga SteamCMD para `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` y inscribirlo en su cuenta de Steam. Cuando se pregunta `Add a mod to watch now?`La respuesta `n`El elemento no existe todavía. Descarga este paso si Workshop Watcher ya está instalado.

<a id="3-prepare-the-items-files"></a>
## 3. Preparar los archivos del elemento

1. Descarga la liberación ZIP y envuelve en `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` Ahora tiene `$Mod.dll`, `meitou.json` y `$Mod.mod`.
2. Salva la imagen previa del artículo como `$Work\preview.png`: PNG o JPG, menos de 1 MB, cuadrado (512 x 512 píxeles de trabajo).

<a id="4-write-the-vdf"></a>
## 4. Escribe el VDF

C.C.C. crea el elemento a partir de VDF archivo: un archivo de texto de `"key" "value"` Las parejas dentro de la `"workshopitem"` El bloque. Este mandamiento escribe `$Work\create.vdf` con los caminos llenos. El Edit `title` y `description` En primer lugar:

```powershell
$Dir = $Work.Replace('\', '/')
@"
"workshopitem"
{
    "appid"           "233860"
    "publishedfileid" "0"
    "contentfolder"   "$Dir/$Mod"
    "previewfile"     "$Dir/preview.png"
    "visibility"      "2"
    "title"           "[Meitou] My Mod"
    "description"     "What the mod does. Requires MeitouClient."
    "changenote"      "$Mod $Version"
}
"@ | Set-Content "$Work\create.vdf" -Encoding ASCII
```

| La clave | Valor |
| --- | --- |
| `appid` | `233860`, KenshiLa aplicación Steam ID. |
| `publishedfileid` | `0` Crea un nuevo elemento. Cualquier otro número de actualizaciones que el elemento existente. |
| `contentfolder` | Paseo completo a la pasta cuyos ficheros se convierten en el contenido del elemento: la pasta mod de la ZIPPero no a sus padres. |
| `previewfile` | El camino hacia la imagen previa. |
| `visibility` | `0` El público, `1` Sólo los amigos, `2` El privado, `3` No listado . Comienza con `2` y cambiarlo en la página del artículo. |
| `title` | El nombre del artículo en el taller. Comienza con `[Meitou]`Como lo hacen los otros modos de código de Meitou. |
| `description` | Una línea de texto. Escribe la descripción completa en la página del artículo después (suporta Steam BBCode). |
| `changenote` | La primera entrada de la tabla de los cambios de la sección. |

Reglas para los valores:

- Páginas en la que se muestran los caminos (`C:/Users/me/Downloads/...`). Se debe duplicar el número de datos (`C:\\Users\\me\\...`).
- Una cita doble dentro de un valor se escribe `\"`.
- Mantener el archivo ASCII.

<a id="5-create-the-item"></a>
## 5. Crea el elemento

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` es el nombre que se inscribe en Steam con, como se le da al instalador. steamcmd utiliza el login salvado. Si solicita una contraseña o código de Steam Guard, `powershell -ExecutionPolicy Bypass -File $Watcher login`Luego vuelve a dirigir el comando.

El curso termina con `Success.`. Abre Steam > tu perfil > Elementos de taller, abre el nuevo elemento y copia el número después `?id=` En su URLEsta es la identificación del elemento.

<a id="6-finish-the-items-page"></a>
## 6. Finalizar la página del artículo

En la página del artículo en Steam:

1. Aceptar el acuerdo legal de Steam si la página muestra el banner. Hasta que lo haga, sólo puedes ver el artículo.
2. Escribe la descripción completa.
3. Si el mód necesita otro Mod de Workshop (por ejemplo, Rivalry Core), añadirlo a continuación. **Add/Remove Objetos necesarios**. Los suscriptores son
Se le pidió que suscribiera también. Con la dependencia de `meitou.json` `dependencies`El orden del lanzador lo pone primero en el orden de carga.
4. Cambiar la visibilidad de **público** Cuando la página está listo.

<a id="7-watch-the-item"></a>
## 7. Mira el artículo

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

El lanzamiento se informa del lanzamiento actual como ya se ha descargado. Eliminar `$Work` Una vez que lo hace.

<a id="each-release-after-that"></a>
## Cada uno de ellos, después de

Descarga el mód como en el paso 1. El próximo lanzamiento de la guardia se descarga ZIPVerifica su SHA-256 y lo carga en el artículo, con el título de liberación y el enlace como la nota de cambio. Nunca cambia el título, la descripción, la previsión o la visibilidad de la página. `powershell -ExecutionPolicy Bypass -File $Watcher status` muestra el horario, el siguiente curso y el último cargamento.
