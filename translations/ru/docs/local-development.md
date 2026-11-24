<a id="local-development-setup"></a>
# Местное развитие

Клонируйте только реставрацию, на которой вы работаете, в одну папку. Каждый из них строится самостоятельно с `dotnet build`И рамки, которые он должен загрузить и проверить сами.

<a id="repos"></a>
## Отдых

Каждая модель называется по своей модели.

| Репо | ДОЛЖНЫ | PRS пойдет на |
| --- | --- | --- |
| MeitouClient | Двигатель и его моды | `fork` |
| РИВАЛИКОР | Рамка Rivalry Core и его агентские навыки | `beta` |
| Логистическая корея | Логистическая картина | `beta` |
| Военная оркестрация | Военная оркестрационная рамка (употребляет логистический ядро) | `beta` |
| Бастфронт | Bast Front Melee (Употребляет в уших военную оркестрацию и логистическую ядро) | `beta` |
| Ашель | «Шереная рука» (употребляет Rivalry Core) | `beta` |
| Мужчина | Мужчина | `beta` |
| SquadArena | Мод SquadArena и инструмент рейтинга экипажа | `beta` |
| Вместе с структурой (TheIvoryDisaster) | Объединенный CI, упаковка, выпущенная дверь, цель замыкания | `beta` |

<a id="folders"></a>
## Фоллеры

Эти шаги используют восемь папок. Поставьте их как переменные в каждом новом окне PowerShell, а затем нажмите ниже указания, как они есть:

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
## Предупреждения

- .NET 11 SDK (в каждом репетиции `global.json` Название версии) и Git.
- Редактор: Visual Studio, VS Код с C# Dev Kit, или Rider. Также работает только терминал.
- Kenshi Встроенный . Если он не в `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Вспомним, что в `KENSHI_PATH` Экологические изменения
В его папке.

<a id="clone"></a>
## Клоны

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Каждый модуль репо имеет свой `engine` Субмодулы, так `$Engine` Нужно только написать двигатель.

<a id="build-and-play-one-mod"></a>
## Создайте и играйте в один модуль

**Изменение режима репо:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- Первая конструкция загружает каждую структуру, названную `meitou.lock.json` Внутри `deps` и останавливается, если его SHA256 не совпадает.
- `dotnet run --project Play` копирует заблокированные рамки и мод в `Kenshi\mods` И начинается двигатель. Visual Studio против кода
и установленный запуск: [play.md](play.md).
- Включите рамки выше режима в запускнике, как и для любой зависимости.

<a id="change-a-framework-and-a-mod-together"></a>
## Изменить рамку и моду вместе

Показать модуль репо на вашем клоне рамки:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Строительство в `$Ashen` Теперь вы можете скачать Rivalry Core `$Rivalry` Используйте двигатель в `$Rivalry\engine`Используется для того, чтобы создать Rivalry Core.
- `Meitou.user.props` Это гит-неизвестно Если она уже удерживает Kenshi Включите две строки внутри `PropertyGroup` Вместо этого.
- `MeitouFrameworkSources` Проектируем рамковые пути, отделенные от `;`и г. `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` Название двигателя, на котором строится клон.
- Удалите линию, чтобы вернуться к замкнутому ZIP.

**Рамковые цепи .** Бест фронт `meitou.lock.json` Список военных оркестров и логистики. Чтобы изменить обе рамки и Bast Front вместе, перейдите в Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

Обыкновенный перерыв падает, поэтому Война-оркестрация строится против `$Logistics` Также, и весь строй использует один двигатель. Он перекрывает только Военную оркестрацию и строится против логистического ядра, названного в замке Баст-Фронта.

Для его доставки: объединить рамку PR в `beta` От `$Rivalry`, который публикует прелесть `rivalry-core-<version>-beta.<commit>`. После этого, в `$Ashen`, Set `meitou.lock.json` Для этого и для активов, SHA-256 и двигатель придерживается своих нотеров, и открывает режим PR. В одной цепочке корабль вверх: логистика ядра, затем военная оркестра, закрывая его, затем Bast Front, закрывая оба. После выхода из строя, перейдите на `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Изменить двигатель и моду вместе

Проверьте свой двигатель в подмодуле mod repo:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Не завязывайте новый двигатель: двигатели переходят только на теги выхода, поэтому изменение достигает этого репо.
И освобождает его.
- Мод, который использует рамку, также нуждается в реставрации на этом двигателе. Используйте рамки и пересмотрите выше
Моторная отрасль в `$Rivalry\engine`.
- Заказать его приземлиться: двигатель PR `fork`После того, как Upstream выпустит его, рамка PR движется `$Rivalry\engine` После освобождения, после чего
Мод PR перемещает свой замк и `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Переключите двигатель для игры или тестирования

Чтобы играть в последний upstream, выпуску или двигатель, переключите `$Engine` Клонирование с сценарием в любом режиме репо:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`Последний заветный завет VlcekM/MeitouClient `master` (Скриншоты добавляют `upstream` Удаленный при отсутствии)
- `-To Release`Название: The latest upstream release `vX.Y.Z`.
- `-To Fork`Оригинальное название: The Org Fork's `fork` Взрыв и вытянутый.
- `-Branch <name>`: филиал органа, например, филиал двигателя PR.
- Без `-Path` Он использует `MeitouClient` Подарок рядом с mod repo. `-Run` создает игру в Release и начинает ее; `-Build` Только
Он его строит.
- Выход, выход и `-Branch` оставляет клонированный на отъездном участке; местный `master` и `fork` Отделы не меняются. Скриншоты
Он прекращается, не меняя ничего, когда у клона внезапные изменения.
- Он никогда не дотикует собственного мода репо `engine` Подмодуль (то есть `Update-Local.ps1` и пинтов)

<a id="keep-clones-current"></a>
## Дайте клонам актуальные

После каждого притяжения, и каждый раз, когда конструкция проваливается с отсутствием типов двигателей (например, `The type or namespace name 'RelationCause' could not be found`), закрыть Visual Studio и запустить обновленный скрипт в режиме репо:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Он переходит на `beta` и вытягивает его, а затем обновляет `engine` и `build/gutenstructure` В связи с обязательствами `beta` Пинс (`git pull` одинокий
И оставить их на старых). `-Branch <name>` И еще одна ветвь.
- Он проверяет каждый субмодуль против своего пина и печатает версию двигателя.
- Это флаг `MeitouEngineDir` и `MeitouFrameworkSources` В `Meitou.user.props` или окружающей среды: они делают строительство использоваться другим
двигатель или рамка репо вместо этого пина репо. Удалите их, если вы не меняете рамку и моду вместе.
- `-Clean` Удаление ГИТ-неизвестного `bin` и `obj` папки, поэтому не используется старый двигатель. `-Build` РУН `dotnet build`.
- Он заканчивается с `Up to date` Список линий, обозначенных `FIX`.

Следующий строй снова загружает рамку только тогда, когда `meitou.lock.json` Изменился
