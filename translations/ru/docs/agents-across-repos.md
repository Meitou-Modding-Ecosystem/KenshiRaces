<a id="agents-working-across-repos"></a>
# Агенты, работающие на расслаблении

Дайте одному агенту каждый звонок, скажите ему, какой звонок принадлежит каждому участку, и вы получите его в порядке: двигатель, затем рамка, затем модуль. Одна сессия агента по изменению, а не одна по репо.

<a id="give-the-agent-the-repos"></a>
## Дайте агентству отдохнуть

- **Сеанс в облаке:** Каждый реп должен быть включен в сессию или проект. Агент клонирует их бок за бок и работает на собственной ветви
В каждом .
- **Ваш компьютер (Remote Control):** Клоны в папках из [local-development.md](local-development.md). Агент также может сделать, что
В облачных сетях не может быть таких, как `gh repo create`.

<a id="where-each-part-goes"></a>
## Где каждая часть

| Изменения | Репо | В PR |
| --- | --- | --- |
| Моторный хок (событие, спасенное поле, командование) | MeitouClient | `fork` |
| Рыцарь Ветра API или поведение | РИВАЛИКОР | `beta` |
| Логистическая ядро (ноды, краи, владельцы, запасы, транспорт) | Логистическая корея | `beta` |
| Военная оркестрация (боевые сценарии на логистику, никакой истории или персонажей) | Военная оркестрация | `beta` |
| Предыдущая статьяПервый мел | Бастфронт | `beta` |
| Код мода, генерирующий данные, тесты, доки | Репо от этого мода | `beta` |
| Поделиться CI, упаковка, пусковая дверь, целенаправленная | ГУТЕНСТРУКТУРА | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Решение о перемене, которое пересекает отдых

1. Вход в PR `fork`И смешанный . Движение двигателей только к тегам высвобождения, поэтому рамки и модульные PR, которые нуждаются в этом, ждут
Взрослые (VlcekM/MeitouClient3) освобождает изменения. До этого, строить против двигателя ветви местно ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. Рамка PR, которая делает изменения (и перемещает их `engine` Пин к освобождению, который имеет то, что ему нужно), сливается в `beta`. И я публикую
Бета премьера.
3. Мод PR, который движется `meitou.lock.json` В этом случае (и `engine` к мотору замка, соединенному в `beta`.
4. После выхода рамок будет продвигаться и отображаться: режим PR, который перемещает замк в `<framework>-v<version>`.

Каждый PR связывает его сначала. Разрушительная рамка API Изменение мелкой версии рамки и перечисление каждого зависимого репо; каждый зависимый получает свой собственный PR.

В одной цепочке шаг 2 повторяется один раз, вверх-на-долу: логистический ядро меняется, затем военная PR закрывается, что логистика создает и публикует, а затем Bast Front PR закрывается.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Правила, которые нужно вписывать в инструкцию агента

```markdown
- Repos and owners: engine hooks -> MeitouClient (PR into fork); Rivalry Core -> RivalryCore (beta);
  Logistics Core -> LogisticsCore (beta); War Orchestration -> WarOrchestration (beta); Bast Front -> BastFront (beta);
  mods -> their own repo (beta); shared CI/packaging -> GutenStructure (beta).
- Layers: logistics.core -> war.orchestration -> Bast Front. A layer uses the one above only through its public API and
  a dependencies range. War Orchestration holds no story or character content.
- Land PRs in order: engine, then each framework layer top-down, then the mod. One PR per repo per step. Self-merge each
  when CI is green; link the previous PR.
- A meitou.lock.json lists every framework in the chain (Bast Front locks War Orchestration AND Logistics Core), all on
  one engine commit, each accepted by the others' dependency ranges.
- To build against unmerged framework changes, set MeitouFrameworkSources (the sibling clones' framework .csproj paths, ';'-separated)
  and MeitouEngineDir in the repo's git-ignored Meitou.user.props. Never commit Meitou.user.props.
- A lock points only at an immutable tag: <framework>-<version>-beta.<commit> or <framework>-v<version>, with the
  asset, SHA-256 and engine commit copied from that release's notes. Never the rolling <framework>-beta tag.
- Never commit inside an engine/ submodule. Pin engine/ to an upstream Meitou release tag (vX.Y.Z of VlcekM/MeitouClient), or
  with a lock, to the lock's engine commit. Never pin the fork's fork head or a nightly. Move Directory.Build.rsp and every
  gameVersion floor together, only when the mod needs the newer release (GutenStructure docs/versioning.md).
- After pulling, run tools/Update-Local.ps1 so local builds use the pinned engine.
- Don't merge a PR whose lock names a framework build that isn't published yet.
- Ask before release tags, promotions to main or release, and anything in another author's folder or repo.
- Report every PR link, in landing order.
```

<a id="prompt-template"></a>
## Сразу шаблоны

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
