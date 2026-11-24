# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · [Deutsch](../de/README.md) · [Español](../es/README.md) · [Italiano](../it/README.md) · [Português do Brasil](../pt-BR/README.md) · Русский · [日本語](../ja/README.md) · [한국어](../ko/README.md) · [简体中文](../zh-CN/README.md)

Kenshi Races добавляет в редактор персонажей [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) три игровые расы. Для Leoma и Crevan доступны мужские и женские тела. У Crabfolk есть два варианта — `crabman` и `crabqueen`. Все они используют человеческие скелеты игры. В комплект также входят 59 анимаций для любых рас на этих скелетах и фреймворк, с помощью которого другие моды могут добавлять собственные расы и подрасы. GPL-3.0-or-later.

## Руководства

- [Расы, записи и проверка в игре](docs/kenshi-races.md)
- [Использование фреймворка в другом моде и добавление подрас](docs/depending-on-kenshi-races.md)
- [Библиотека из 59 анимаций и процесс их создания](docs/animations.md)
- [Подгонка тела в формате GLB к игровому скелету](docs/race-import.md)
- [Сборка и запуск через Visual Studio, VS Code или терминал](docs/play.md)
- [Настройка и обновление локальных репозиториев](docs/local-development.md)
- [Координация изменений в нескольких репозиториях](docs/agents-across-repos.md)
- [Проверка совместимости с новыми версиями Meitou](docs/refcheck.md)
- [Публикация в Мастерской Steam](docs/steam-workshop.md)
- [Импорт моделей GLB в качестве построек без FCS](docs/structures.md)
- [Подготовка геометрии, размеров и текстур](docs/model-prep.md)
- [Защита веток и правила проверки изменений](docs/branch-policy.md)
- [Правила репозитория и инструкции для агентов](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## Общие идентификаторы

Все загруженные моды используют общее пространство номеров задач и кодов диалога. Kenshi Races не резервирует номеров. (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## Структура репозитория

- `engine/` — Подмодуль MeitouClient закреплён на коммите, с которым собирается мод. После каждого pull обновляйте его командой ниже. [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — Проект и манифест мода. Подогнанные модели тел хранятся в репозитории. Тесты используют синтетические данные и не требуют установки игры.
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — Необязательные проекты создают данные мода из установленной игры или импортируют модели GLB как постройки при развёртывании. [StructureImport](docs/structures.md)
- `meitou.lock.json` — Фиксирует версии фреймворков. Процесс обновления переходит на новые версии и выполняет слияние после успешной CI. [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — Сборка, развёртывание и запуск одним действием.
- `tools/` — Инструменты синхронизации, проверки ссылок на движок, импорта построек, подгонки тел и подготовки текстур. [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — Импортирует настройки движка. Предупреждения компилятора считаются ошибками.
- `Directory.Build.rsp` — Фиксирует ссылки на самую старую поддерживаемую версию движка. Игра отклоняет моды со ссылками на более новую версию. Подмодуль по умолчанию использует стандартную версию сборки. (`gameVersion`, `1.0.0.0`).  Совместимость с новыми релизами Meitou проверяется ежедневно. [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## Ветки

Направляйте PR в beta. Сборка и тесты должны пройти. После проверки в игре beta можно перенести в main. Для выпуска требуется PR в release. При каждом слиянии в release создаётся тег версии и публикуется ZIP, если такого тега ещё нет. Скрипт создания тега служит запасным ручным способом. (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
