<a id="publishing-to-the-steam-workshop"></a>
# Опубликовано в Steam Workshop

Вы создаете элемент рабочего места один раз, вручную, с выхода ZIP. После этого, [Рабочий зал Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) Проводится на вашем компьютере Windows и загружает каждую новую версию GitHub в этот пункт по графику. Все шаги проходят **Windows PowerShell** На вашем компьютере, зарегистрированный в Steam, который будет принадлежать предмету.

Команды используют эти значения. Настроить их один раз по окну PowerShell, с фасциклами вашего режима (как в `release.yml` `mod-folder`1) и ее первая версия:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Откройте модуль

1. Межпозвон `beta` Внутри `main`Открыть и объединить PR `main` Внутри `release` ([Фанты](../README.md#branches)).
2. Дождайтесь работы по выпуску `release`. Название: The Fusion `<slug>-v<version>` С версией в `meitou.json` (например, в
`example-mod-v0.1.0`и создает освобождение с `<Mod>-<version>.zip` и `<Mod>-<version>.zip.sha256`. Если это не так, то перед тем, как выйти, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` Пожалуйста, обращайтесь к нам по руке (`-DryRun` Исключивать только командные знаки).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Инсталляция Workshop Watcher и вход в Steam

Следите за [Инсталляция](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) В рамках семинара «Ватчер». Инсталлятор загружает SteamCMD `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` Зарегистрируйте его на свой аккаунт в Steam. Когда он просит `Add a mod to watch now?`Ответ → `n`Итак, объект еще не существует. Отменить этот шаг, если Workshop Watcher уже установлен.

<a id="3-prepare-the-items-files"></a>
## 3. Подготовить файлы предмета

1. Скачать освобождение ZIP И не упаковывать его в `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` Теперь держит `$Mod.dll`, `meitou.json` и `$Mod.mod`.
2. Сохраните изображение предсказания объекта как `$Work\preview.png`: PNG или JPG, менее 1 Мб, квадрат (512 x 512 пикселей работает).

<a id="4-write-the-vdf"></a>
## 4. Напишите о VDF

cmcd создает элемент из VDF Файл: текстовый файл `"key" "value"` Внутри пары A `"workshopitem"` Блокируем Этот Командующий пишет `$Work\create.vdf` Путешествия, которые заполняются ЭДИТ `title` и `description` В первую очередь:

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

| Ключ | ценность |
| --- | --- |
| `appid` | `233860`, KenshiИдентификация Steam App ID. |
| `publishedfileid` | `0` Создает новый элемент. Любые другие обновления, которые существуют. |
| `contentfolder` | Полный путь к папке, файлы которого превращаются в содержимое элемента: папке mod из ZIPНо не его родители. |
| `previewfile` | Полный путь к премьере. |
| `visibility` | `0` Общественность → `1` Только друзья, `2` Приватный , `3` Несчитаны . Начинаем с `2` Изменить его на странице. |
| `title` | Название предмета на семинаре. Начните с `[Meitou]`Так же, как и другие модули кода Meitou. |
| `description` | Одна линия текста. Напишите полный опис на странице объекта (после этого он поддерживает Steam BBCode). |
| `changenote` | Первое вхождение в раздел Изменения записей. |

Правила для ценностей:

- Написать шаги с вперед слайдами (`C:/Users/me/Downloads/...`). В этом случае необходимо удвоить (`C:\\Users\\me\\...`).
- Двойная цитата внутри стоимости написана. `\"`.
- Сохранить файл ASCII.

<a id="5-create-the-item"></a>
## 5. Создайте элемент

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` Это имя, которое вы подписываете в Steam с, как это дано в установке. steamcmd использует сохранный вход. Если он просит пароль или код Steam Guard, запустите `powershell -ExecutionPolicy Bypass -File $Watcher login`После этого выдвигайте команду снова.

Проезд заканчивается с `Success.`. Откройте Steam > Ваш профиль > Рабочие элементы, открывайте новый элемент и копируйте номер после `?id=` В его URLЭто и есть идентификационный пункт.

<a id="6-finish-the-items-page"></a>
## 6. Завершить страницу по предмету

На странице статьи в Steam:

1. Принимайте юридическое соглашение по Steam Workshop, если на странице отображается баннер. До тех пор, пока вы не сделаете это, только вы можете увидеть объект.
2. Напишите полное описание.
3. Если для мода нужен и другой «Сменет» (например, Rivalry Core), добавьте его в **Add/Remove Необходимые предметы**. Подписчики тогда
Он также просит подписаться. С зависимостью от `meitou.json` `dependencies`Постановка запуска в первую очередь входит в порядок загрузки.
4. Изменение видимости на **Общественность** Когда страница готова.

<a id="7-watch-the-item"></a>
## 7. Смотреть пункт

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

Сухой маршрут сообщает о текущем выпуске, как уже загружен. Удаление `$Work` Как только это сделает.

<a id="each-release-after-that"></a>
## Каждое освобождение после

Откройте моду, как и в шаге 1. Следующая статьяВредник на следующий расписание запускает ZIPПроверьте его SHA-256 и загрузить его в пункт, с заголовком выпуска и ссылки как изменения. Он никогда не меняет название, описание, предварительный просмотр или видимость: меняет страницу. `powershell -ExecutionPolicy Bypass -File $Watcher status` Показать график, следующий раунд и последний загрузку.
