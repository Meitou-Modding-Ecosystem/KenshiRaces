<a id="publishing-to-the-steam-workshop"></a>
# Публикация в Мастерской Steam

Вы создаете элемент Мастерской мода один раз вручную из версии ZIP . После этого [Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) запускается на вашем Windows PC и загружает каждый новый выпуск GitHub в этот элемент по расписанию. Все действия выполняются в **Windows PowerShell** на вашем PC , войдя в учетную запись Steam, которой будет принадлежать этот предмет.

Команды используют эти значения. Установите их один раз для каждого окна PowerShell, указав имя папки вашего мода (как в `release.yml`  `mod-folder` ) и его первую версию:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Выпустить мод

1. Объедините `beta` с `main` , затем откройте и объедините PR из `main` с `release` ([Branches](../README.md#branches) ).
2. Дождитесь рабочего процесса выпуска на `release` . Он помечает слияние `<slug>-v<version>` с версией в `meitou.json` (например, `example-mod-v0.1.0` ) и создает выпуск с `<Mod>-<version>.zip` и `<Mod>-<version>.zip.sha256` . Если этот запуск завершается неудачно до присвоения тега, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` отправляет тег вручную ( `-DryRun` печатает только команды).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Установите Workshop Watcher и войдите в Steam.

Следуйте инструкциям [Установите](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) в руководстве Workshop Watcher. Установщик загружает steamcmd в `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` и авторизует его в вашей учетной записи Steam. Когда он спросит `Add a mod to watch now?` , ответьте `n` : предмет еще не существует. Пропустите этот шаг, если Workshop Watcher уже установлен.

<a id="3-prepare-the-items-files"></a>
## 3. Подготовьте файлы предмета.

1. Скачайте релиз ZIP и распакуйте его в `$Work` :

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` теперь содержит `$Mod.dll` , `meitou.json` и `$Mod.mod` .
2. Сохраните изображение предварительного просмотра элемента как `$Work\preview.png` : PNG или JPG под 1 MB , квадрат (512 x 512 пикселей подойдет).

<a id="4-write-the-vdf"></a>
## 4. Напишите VDF

Steamcmd создает элемент из файла VDF : текстового файла пар `"key" "value"` внутри блока `"workshopitem"` . Эта команда записывает `$Work\create.vdf` с заполненными путями. Сначала отредактируйте `title` и `description` :

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

| Ключ | Ценить |
| --- | --- |
| `appid` | `233860` , приложение Steam Kenshi  ID . |
| `publishedfileid` | `0` создает новый элемент. Любое другое число обновляет существующий элемент. |
| `contentfolder` | Полный путь к папке, файлы которой становятся содержимым элемента: папка мода из ZIP , а не его родительский элемент. |
| `previewfile` | Полный путь к изображению предварительного просмотра. |
| `visibility` | `0` общедоступный, `1` только для друзей, `2` частный, `3` не в списке. Начните с `2` и измените его на странице товара. |
| `title` | Название предмета в Мастерской. Запустите его с помощью `[Meitou]` , как это делают другие моды кода Meitou . |
| `description` | Одна строка текста. После этого напишите полное описание на странице предмета (оно поддерживает BBCode Steam). |
| `changenote` | Первая запись на вкладке «Примечания к изменениям» элемента. |

Правила для значений:

- Записывайте пути с косой чертой ( `C:/Users/me/Downloads/...` ). Обратная косая черта должна быть удвоена ( `C:\\Users\\me\\...` ).
- Двойная кавычка внутри значения записывается `\"` .
- Сохраните файл ASCII .

<a id="5-create-the-item"></a>
## 5. Создайте элемент

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` — это имя, под которым вы входите в Steam, данное установщику. Steamcmd использует сохраненный логин. Если он запросит пароль или код Steam Guard, запустите `powershell -ExecutionPolicy Bypass -File $Watcher login` , а затем повторите команду.

Прогон заканчивается `Success.` . Откройте Steam > ваш профиль > Предметы мастерской, откройте новый предмет и скопируйте номер после `?id=` в его URL : это предмет ID .

<a id="6-finish-the-items-page"></a>
## 6. Завершите страницу элемента.

На странице предмета в Steam:

1. Примите юридическое соглашение Мастерской Steam, если на странице отображается баннер. Пока вы этого не сделаете, только вы можете видеть этот предмет.
2. Напишите полное описание.
3. Если для мода требуется другой мод Workshop (например, Rivalry Core), добавьте его в разделе **Добавить/удалить необходимые элементы**. Затем подписчикам также предлагается подписаться на него. Благодаря зависимости в `meitou.json`  `dependencies` Arrange средства запуска помещает ее первой в порядке загрузки.
4. Измените видимость на **Общедоступная**, когда страница будет готова.

<a id="7-watch-the-item"></a>
## 7. Следите за объектом

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

Пробный прогон сообщает о том, что текущая версия уже загружена. Удалите `$Work` , как только это произойдет.

<a id="each-release-after-that"></a>
## Каждый выпуск после этого

Выпустите мод, как описано в шаге 1. Следующий запланированный запуск наблюдателя загружает ZIP , проверяет его SHA -256 и загружает его в элемент с названием выпуска и ссылкой в ​​качестве примечания об изменении. Он никогда не меняет заголовок, описание, предварительный просмотр или видимость элемента: измените их на странице элемента. `powershell -ExecutionPolicy Bypass -File $Watcher status` показывает расписание, следующий запуск и последнюю загрузку.
