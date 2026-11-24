<a id="kenshi-races"></a>
# Kenshi Races

Новые расы для [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) на человеческих скелетах игры, в которые можно играть в редакторе персонажей: Leoma и Crevan (мужчина и женщина) и Crabfolk (краб, королева крабов), библиотека анимации из 59 клипов для каждой расы человеческих скелетов, а также фреймворк, в который другие моды добавляют свои собственные расы и подрасы. GPL -3.0 или новее.

- Расы, их рекорды и тестирование в игре: [docs/kenshi-races.md](docs/kenshi-races.md) .
- Использование этих рас или добавление своих из другого мода: [docs/dependent-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (навык агента `.claude/skills/kenshi-races-dependent` ).
- Библиотека анимации (59 роликов для каждой расы на скелетах людей) и как она делалась: [docs/animations.md](docs/animations.md) .
- Установка оснащенного тела `.glb` ( Meshy , Hi3D "3ds Max Biped") на скелет игры: [docs/race-import.md](docs/race-import.md) .

- Соберите и запустите локально с помощью Visual Studio , VS Code или терминала (движок этого репозитория или установленный модуль запуска, развернутый в игре): [docs/play.md](docs/play.md) .
- Настройка репозиториев на вашем компьютере: [docs/local-development.md](docs/local-development.md) .
- Инструктирование агентов об изменении, охватывающем репозитории: [docs/agents-across-repos.md](docs/agents-across-repos.md) .
- Правила агента для вывода ядра, версий, CI и выпусков: [ CLAUDE .md](../../CLAUDE.md) (то же, что `AGENTS.md` ); пошаговые процедуры: `.claude/skills/meitou-engine-release` .
- Проверка встроенного мода на соответствие новейшей версии Meitou ( CI делает это на каждом PR ): [docs/refcheck.md](docs/refcheck.md) .
- Публикация мода в Мастерской Steam впервые, а затем при каждом выпуске: [docs/steam-workshop.md](docs/steam-workshop.md) .
- Превращая модели `.glb` ( Meshy , Hi3D , Blender ) в сборные конструкции, FCS не требуется: [docs/structures.md](docs/structures.md) ; сначала получить количество полигонов, размер и текстуры сгенерированной модели: [docs/model-prep.md](docs/model-prep.md) .

<a id="numbers"></a>
## Числа

Типы задач ( `AddGoalType` , `AddJobType` ) и коды диалогов ( `AddDialogueCondition` , `AddDialogueEffect` ) являются общими для каждого загруженного мода ([номер модуля блокирует](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) ). Kenshi Races не утверждает ничего.

| Число | Бассейн | Использовать |
| --- | --- | --- |

<a id="layout"></a>
## Макет

- `engine/` : MeitouClient как подмодуль git, прикрепленный к движку, для которого создается мод. `git pull` не перемещает его: после каждого запроса запускайте `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([сохранять текущие клоны](docs/local-development.md#keep-clones-current) ).
- `KenshiRaces/` : проект мода и `meitou.json` ; `KenshiRaces/items/races/` : установленные кузова (фиксированный импортный вывод). `KenshiRaces.Tests/` : его тесты (синтетические данные, установка игры не требуется). `<Mod>.Data/` (опционально): генератор, записывающий `<Mod>.mod` из установленной игры. `<Mod>/Structures/` (необязательно): модели `.glb` импортируются как сборные конструкции при развертывании ([docs/structures.md](docs/structures.md) ).
- `meitou.lock.json` : фреймворк выпускает сборки модов ([блокировка фреймворка](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ); рабочий процесс обновления Framework перемещает его в новые выпуски и объединяет после прохождения CI ([framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ).
- `Play/` : сборка, развертывание и запуск в один клик.
- `tools/` : `Update-Local.ps1` , локальная проверка тяги и штифта двигателя ([держать клоны текущими](docs/local-development.md#keep-clones-current) ); `refcheck.sh` и `RefCheck/` , проверка ссылок на движок ([docs/refcheck.md](docs/refcheck.md) ); `StructureImport/` , импорт модели ([docs/structures.md](docs/structures.md) ); `RaceImport/` , кузовной фитинг ([docs/race-import.md](docs/race-import.md) ); `model-info.py` и `prepare-model.py` , проверка модели и сжатие текстур ([docs/model-prep.md](docs/model-prep.md) ).
- `Directory.Build.props` импортирует движок, поэтому моды создаются с учетом настроек игры (предупреждения — это ошибки).
- `Directory.Build.rsp` помечает ссылки на движок как самый старый выпуск Meitou , поддерживающий моды, равный каждому этажу `gameVersion` : игра отклоняет мод, ссылающийся на более новую марку движка, и подмодуль собирает `1.0.0.0` ([версии](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ). Рабочий процесс совместимости Meitou ежедневно проверяет моды на соответствие новым выпускам Meitou ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ).

<a id="branches"></a>
## Филиалы

PR в `beta` ; CI (сборка и тесты) должен пройти. `beta` повышается до `main` после тестирования в игре, а производство проходит от PR до `release` . Каждое слияние с `release` помечает его как `<slug>-v<version>` и публикует zip-архив мода ( `.github/workflows/release.yml` ), если этот тег не существует; `tools\Tag-Release.ps1` — это резервный вариант вручную.

- Защита ветвей нового репозитория и постоянные обзоры бета-/основных/выпусков: [docs/branch-policy.md](docs/branch-policy.md) . Исходный код шаблонов; GutenStructure применяет настройки посредством согласования.
