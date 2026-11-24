<a id="local-development-setup"></a>
# Настройка и обновление локальных репозиториев

Клонируйте в одну папку только те репозитории, над которыми вы работаете. Каждый из них строится самостоятельно с помощью `dotnet build`, а необходимые ему фреймворки загружаются и хэш-проверяются самостоятельно.

<a id="repos"></a>
## РЕПО

Каждый репозиторий модов назван в честь своего мода.

| Репо | держит | пиарщики идут в |
| --- | --- | --- |
| MeitouClient | движок и его мод-хуки | `fork` |
| СоперничествоЯдро | Структура Rivalry Core и навыки ее агента | `beta` |
| ЛогистикаЯдро | Основная структура логистики | `beta` |
| ВойнаОркестровка | Структура War Orchestration (использует Logistics Core) | `beta` |
| БастФронт | Ближний бой Bast Front (использует War Orchestration и Logistics Core) | `beta` |
| AshenHand | Пепельная Рука (использует Ядро Соперничества) | `beta` |
| Животноводство | Животноводство | `beta` |
| СквадАрена | Мод SquadArena и инструмент рейтинга отрядов | `beta` |
| GutenStructure (TheIvoryDisaster) | общий CI, упаковка, ворота выпуска, цель блокировки каркаса | `beta` |

<a id="folders"></a>
## Папки

В этих шагах используются восемь папок. Установите их как переменные в каждом новом окне PowerShell, а затем вставьте приведенные ниже команды как есть:

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
## Предварительные условия

-. NET 11 SDK (`global.json` каждого репозитория называет версию) и Git.
- Редактор: Visual Studio, VS Code с C# Dev Kit или Rider. Один терминал тоже работает.
- Kenshi установлен. Если его нет в `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`, установите переменную среды `KENSHI_PATH` в ее папку.

<a id="clone"></a>
## Клонировать

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

У каждого репозитория модов есть свой собственный подмодуль `engine`, поэтому `$Engine` нужен только для написания хуков движка.

<a id="build-and-play-one-mod"></a>
## Создайте и играйте в один мод

**Изменение в репозитории мода:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- Первая сборка загружает каждую платформу, указанную в `meitou.lock.json`, в `deps` и останавливается, если ее SHA -256 не соответствует.
- `dotnet run --project Play` копирует заблокированные фреймворки и мод в `Kenshi\mods` и запускает движок. Visual Studio, VS Code и установленный лаунчер: [play.md](play.md).
- Включите фреймворки над модом в лаунчере, как и для любой зависимости.

<a id="change-a-framework-and-a-mod-together"></a>
## Меняйте фреймворк и мод вместе

Направьте репозиторий мода на клон вашего фреймворка:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Сборки в `$Ashen` теперь компилируют Rivalry Core из `$Rivalry` и используют движок `$Rivalry\engine`, а Play развертывает эту сборку Rivalry Core.
- `Meitou.user.props` игнорируется git. Если он уже содержит ваши настройки Kenshi, вместо этого добавьте две строки в его `PropertyGroup`.
- `MeitouFrameworkSources` принимает пути проекта платформы, разделенные `;`, например. `$Logistics\LogisticsCore\LogisticsCore.csproj`. `MeitouEngineDir` называет движок, на котором построен клон фреймворка.
- Удалите строку, чтобы вернуться к заблокированному ZIP.

**Цепочки рамок.**В `meitou.lock.json` компании Bast Front перечислено ядро ​​военной оркестровки и логистики. Чтобы изменить обе платформы и Bast Front вместе, переопределите оба в Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

Переопределение передается вниз, поэтому War Orchestration также строится на основе `$Logistics`, и вся сборка использует один движок. Отмените только оркестровку войны, и она будет построена на базе логистического ядра, указанного в замке Баст Фронт.

Чтобы отправить его: объедините фреймворк PR с `beta` из `$Rivalry`, который публикует предварительную версию `rivalry-core-<version>-beta.<commit>`. Затем в `$Ashen` установите для `meitou.lock.json` этот тег и актив, SHA -256 и фиксацию движка из примечаний к выпуску, и откройте мод PR. В цепочке корабль сверху вниз: Логистическое ядро, затем сборка War Orchestration, блокирующая его, затем Bast Front, блокирующая оба. После выпуска рабочей версии платформы переместите блокировку на `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Меняйте движок и мод вместе

Проверьте ветку вашего движка внутри подмодуля репозитория мода:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Не фиксируйте указатель нового движка: контакты движка перемещаются только в теги выпуска вышестоящей версии, поэтому изменение достигает этого репозитория после того, как вышестоящая версия его выпустит.
- Мод, использующий фреймворк, также требует, чтобы фреймворк был перестроен на этом движке. Используйте переопределение фреймворка выше и проверьте ту же ветку движка в `$Rivalry\engine`.
- Порядок посадки: двигатель PR в `fork` ; после того, как исходная версия выпускает его, фреймворк PR перемещает `$Rivalry\engine` в этот выпуск, затем мод PR перемещает свою блокировку и `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Переключите двигатель, чтобы играть или тестировать

Чтобы воспроизвести последнюю версию исходной версии, релиз или ветку движка, переключите свой клон `$Engine` с помощью скрипта в любом репозитории модов:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : новейший коммит VlcekM/ MeitouClient  `master` (скрипт добавляет удаленный `upstream` при его отсутствии).
- `-To Release` : новейший тег выпуска `vX.Y.Z`.
- `-To Fork` : удалена ветвь `fork` ветки организации.
- `-Branch <name>` : ветвь ответвления организации, например ветка движка PR.
- Без `-Path` используется папка `MeitouClient` рядом с репозиторием мода. `-Run` собирает игру в Release и запускает ее; `-Build` только собирает его.
- Upstream, Release и `-Branch` оставляют клон в отдельном коммите; локальные ветки `master` и `fork` не изменяются. Сценарий останавливается, ничего не меняя, если в клоне есть незафиксированные изменения.
- Он никогда не затрагивает собственный подмодуль `engine` репозитория модов (то есть `Update-Local.ps1` и контакты).

<a id="keep-clones-current"></a>
## Поддерживайте актуальность клонов

После каждого извлечения и всякий раз, когда сборка завершается с ошибкой из-за отсутствия типов движка (например, `The type or namespace name 'RelationCause' could not be found`), закройте Visual Studio и запустите скрипт обновления в репозитории мода:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Он переключается на `beta` и тянет его, затем обновляет `engine` и `build/gutenstructure` на фиксирующие контакты `beta` (только `git pull` оставляет их на старых). `-Branch <name>` тянет еще одну ветку.
- Он сверяет каждый субмодуль с его выводом и печатает версию движка.
- Он помечает `MeitouEngineDir` и `MeitouFrameworkSources` в `Meitou.user.props` или в среде: они заставляют сборку использовать движок или структуру другого репозитория вместо контактов этого репо. Удалите их, если вы не меняете фреймворк и мод одновременно.
- `-Clean` удаляет игнорируемые git папки `bin` и `obj`, поэтому сборка старого движка не используется повторно. `-Build` запускает `dotnet build`.
- Он заканчивается `Up to date` или списком строк, отмеченных `FIX`.

Следующая сборка снова скачивает фреймворк только при изменении `meitou.lock.json`.

[English](../../../docs/local-development.md) · [Kenshi Races](../README.md)
