<a id="publishing-to-the-steam-workshop"></a>
# Publicar en Steam Workshop

El elemento del Taller del mod se crea una vez, a mano, a partir de una versión ZIP. Después de eso, [Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) se ejecuta en su Windows PC y carga cada nueva versión de GitHub en ese elemento según un cronograma. Todos los pasos se ejecutan en **Windows PowerShell**en su PC, iniciando sesión en la cuenta de Steam que será propietaria del artículo.

Los comandos utilizan estos valores. Configúrelos una vez por ventana de PowerShell, con el nombre de la carpeta de su mod (como en `release.yml`  `mod-folder`) y su primera versión de lanzamiento:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Libera el mod.

1. Fusione `beta` en `main`, luego abra y fusione un PR de `main` en `release` ([Branches](../README.md#branches)).
2. Espere el flujo de trabajo de lanzamiento en `release`. Etiqueta la combinación `<slug>-v<version>` con la versión en `meitou.json` (por ejemplo, `example-mod-v0.1.0`) y crea una versión con `<Mod>-<version>.zip` y `<Mod>-<version>.zip.sha256`. Si esa ejecución falla antes de etiquetar, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` empuja la etiqueta manualmente (`-DryRun` solo imprime los comandos).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Instale Workshop Watcher e inicie sesión en Steam.

Siga [Instalar](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) en la guía Workshop Watcher. El instalador descarga steamcmd en `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` e inicia sesión en su cuenta de Steam. Cuando le pregunte `Add a mod to watch now?`, responda `n` : el artículo aún no existe. Omita este paso si Workshop Watcher ya está instalado.

<a id="3-prepare-the-items-files"></a>
## 3. Preparar los archivos del artículo.

1. Descargue la versión ZIP y descomprímala en `$Work` :

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` ahora contiene `$Mod.dll`, `meitou.json` y `$Mod.mod`.
2. Guarde la imagen de vista previa del elemento como `$Work\preview.png` : PNG o JPG, en 1 MB, cuadrado (512 x 512 píxeles funciona).

<a id="4-write-the-vdf"></a>
## 4. Escribe el VDF

steamcmd crea el elemento a partir de un archivo VDF : un archivo de texto de pares `"key" "value"` dentro de un bloque `"workshopitem"`. Este comando escribe `$Work\create.vdf` con las rutas completadas. Edite `title` y `description` primero:

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

| Llave | Valor |
| --- | --- |
| `appid` | `233860`, aplicación Steam de Kenshi  ID. |
| `publishedfileid` | `0` crea un nuevo elemento. Cualquier otro número actualiza ese elemento existente. |
| `contentfolder` | Ruta completa a la carpeta cuyos archivos se convierten en el contenido del elemento: la carpeta mod del ZIP, no su padre. |
| `previewfile` | Ruta completa a la imagen de vista previa. |
| `visibility` | `0` público, `1` solo amigos, `2` privado, `3` no listado. Comience con `2` y cámbielo en la página del artículo. |
| `title` | El nombre del objeto en el Taller. Comience con `[Meitou]`, como lo hacen otras modificaciones de código Meitou. |
| `description` | Una línea de texto. Luego escribe la descripción completa en la página del artículo (es compatible con BBCode de Steam). |
| `changenote` | La primera entrada de la pestaña Notas de cambio del elemento. |

Reglas para los valores:

- Escriba rutas con barras diagonales (`C:/Users/me/Downloads/...`). Se debe duplicar una barra invertida (`C:\\Users\\me\\...`).
- Se escribe una comilla doble dentro de un valor `\"`.
- Conserve el archivo ASCII.

<a id="5-create-the-item"></a>
## 5. Crea el artículo

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` es el nombre con el que inicia sesión en Steam, tal como se lo proporcionó el instalador. steamcmd utiliza el inicio de sesión guardado. Si solicita una contraseña o un código Steam Guard, ejecute `powershell -ExecutionPolicy Bypass -File $Watcher login` y luego ejecute el comando nuevamente.

La carrera termina con `Success.`. Abre Steam > tu perfil > Artículos del Taller, abre el nuevo artículo y copia el número después de `?id=` en su URL : ese es el artículo ID.

<a id="6-finish-the-items-page"></a>
## 6. Termina la página del artículo.

En la página del artículo en Steam:

1. Acepta el acuerdo legal de Steam Workshop si la página muestra el banner. Hasta que lo hagas, sólo tú podrás ver el artículo.
2. Escribe la descripción completa.
3. Si el mod necesita otro mod de Workshop (por ejemplo, Rivalry Core), agréguelo en **Agregar o quitar elementos necesarios**. Luego se pide a los suscriptores que también se suscriban. Con la dependencia en `meitou.json`  `dependencies`, Arrange del lanzador lo coloca primero en el orden de carga.
4. Cambie la visibilidad a **Pública**cuando la página esté lista.

<a id="7-watch-the-item"></a>
## 7. Mira el artículo

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

El simulacro informa que la versión actual ya está cargada. Elimine `$Work` una vez que lo haga.

<a id="each-release-after-that"></a>
## Cada lanzamiento posterior a eso

Libere el mod como en el paso 1. La próxima ejecución programada del observador descarga el ZIP, verifica su SHA -256 y lo carga en el elemento, con el título de la versión y el enlace como nota de cambio. Nunca cambia el título, la descripción, la vista previa o la visibilidad del artículo: cámbielos en la página del artículo. `powershell -ExecutionPolicy Bypass -File $Watcher status` muestra el cronograma, la siguiente ejecución y la última carga.

[English](../../../docs/steam-workshop.md) · [Kenshi Races](../README.md)
