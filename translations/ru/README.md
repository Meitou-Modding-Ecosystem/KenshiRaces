<a id="kenshi-races"></a>
# Kenshi расы

Новые гонки для [Метуклиент](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) на человеческих скелетах игры, можно играть в редакторе персонажей: Leoma и Crevan Мужчины и женщины, а также Crabfolk (Крабман) — анимационная библиотека с 59 клипами для каждого человека, и в рамках других моделей добавляются собственные расы и подростки. GPL3.0 или позже.

- Соревнования, их записи и тестирование в игре: [docs/kenshi-races.md](docs/kenshi-races.md).
- Используйте эти расы или добавьте свой из другого режима: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) Агентские навыки
`.claude/skills/kenshi-races-dependent`).
- Анимационная библиотека (показана 59 клипов для каждой расы на человеческих скелетах) и как она была сделана: [docs/animations.md](docs/animations.md).
- Подготавливает риги `.glb` тело (Meshy, Hi3D «3ds Max Biped») на скелет игры: [docs/race-import.md](docs/race-import.md).

- Создать и запускать локально с помощью Visual Studio, VS Code или терминала (мотор этого репо или установленный запускник, размещенный в игре): [docs/play.md](docs/play.md).
- Настроить отдых на машине: [docs/local-development.md](docs/local-development.md).
- Направление агентов на промежуток, который провоцирует отдых: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- Правила агентства для двигателя, версии, CI и выпуски: [CLAUDE.md](../../CLAUDE.md) (как и `AGENTS.md`• шаг за шагом процедуры:
`.claude/skills/meitou-engine-release`.
- Проверка встроенного режима против последней версии Meitou (CI делает это на каждом PR): [docs/refcheck.md](docs/refcheck.md).
- Публикуя моду в Steam Workshop впервые, затем на каждом выпуске: [docs/steam-workshop.md](docs/steam-workshop.md).
- Обратно `.glb` Модель (Meshy, Hi3D, блендер) в строительные структуры, нет FCS Нужное для: [docs/structures.md](docs/structures.md);
Получить поликонт, размер и текстуры генерируемого модела в первую очередь: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## цифры

Типы заданий (`AddGoalType`, `AddJobType`) и диалоговые коды (`AddDialogueCondition`, `AddDialogueEffect`В каждом из них ([Блок номер](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi Расы не претендуют.

| Число | Бассейн | Использование |
| --- | --- | --- |

<a id="layout"></a>
## Лайоут

- `engine/`: MeitouClient Как гит-подмодуль, привязанный к двигателю, выполняет модульные строения против. `git pull` не перемещается: после
Каждый пулл проходит `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([Дайте клонм текущим](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`Проекты Мода и `meitou.json`; `KenshiRaces/items/races/`• приспособление организмов (коммиссированный импортный продукт).
`KenshiRaces.Tests/`: его тесты (синтетические данные, не требуется установка игры). `<Mod>.Data/` (опциональный): генератор, который пишет `<Mod>.mod` Из установленных игр. `<Mod>/Structures/` (в том числе и опционально ) `.glb` Внедрение в эксплуатацию конструкций в качестве конструкций ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`: рамка выводит модули, которые строят против
([Фреймворк](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)• Рамковый обновленный рабочий поток перемещает его на новые выпуски и сливается, когда CI проходит ([Рамковые обновления](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`Один-клик создает, развертывает и запускает.
- `tools/`: `Update-Local.ps1`, местный pull и двигатель-пин проверка ([Дайте клонм текущим](docs/local-development.md#keep-clones-current));
`refcheck.sh` и `RefCheck/`Проверка двигателя ([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`Внешний образ импорта ([docs/structures.md](docs/structures.md)); `RaceImport/`Вместе с тем, что тело ([docs/race-import.md](docs/race-import.md)); `model-info.py` и `prepare-model.py`, модели проверки и текстура сокращения ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` Импортирует двигатель, поэтому модули строят с настройками игры (остережения являются ошибками).
- `Directory.Build.rsp` Заметка двигателей, так как старейший Meitou выпускает поддержку моде, равная каждому `gameVersion` Поверхность: The
Игра отказывается от режима, ссылаясь на более новый двигатель, а подмодуль строит `1.0.0.0` ([Версия](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). Рабочий поток для совместимости с Meitou, где каждый день проверяются модули против новых выпусков Meitou ([Метус Соп](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## Фанты

В PRS `beta`• CI (конструкция и испытания) должны пройти. `beta` Поощряется для `main` После того, как он был пробён в игре, производство проходит через PR. `release`. Каждый из них объединяется `release` Теги это `<slug>-v<version>` Используется для публикации ZIP (`.github/workflows/release.yml`(е) если не существует такой знак; `tools\Tag-Release.ps1` Это и есть ручная пауза.

- Защита от новых репозиторий и постоянных beta/main/release Рецензии → [docs/branch-policy.md](docs/branch-policy.md). Шаблоны копируют источник; GutenStructure применяет настройки через примирение.
