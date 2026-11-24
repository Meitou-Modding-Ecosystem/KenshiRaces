<a id="agents-working-across-repos"></a>
# Агенты, работающие в репозиториях

Дайте одному агенту каждое репозиторий, которого касается изменение, сообщите ему, какому репозиторию принадлежит каждая часть, и пусть он размещает PR в порядке зависимости: движок, затем фреймворк, затем мод. Один сеанс агента на каждое изменение, а не один на каждое репо.

<a id="give-the-agent-the-repos"></a>
## Передайте агенту репозитории

- **Облачный сеанс:** каждый репозиторий должен быть прикреплен к сеансу или проекту. Агент клонирует их рядом и работает в каждой из своих веток.
- **Ваш PC (Удаленное управление):** клоны в папках из [local-development.md](local-development.md) . Агент также может делать то, что не могут делать облачные сеансы, например `gh repo create` .

<a id="where-each-part-goes"></a>
## Куда идет каждая часть

| Изменять | Репо | PR в |
| --- | --- | --- |
| Перехватчик двигателя (событие, сохраненное поле, команда) | MeitouClient | `fork` |
| Ядро соперничества API или поведение | СоперничествоЯдро | `beta` |
| Логистическое ядро ​​(узлы, края, владельцы, запасы, транспорт) | ЛогистикаЯдро | `beta` |
| War Orchestration (боевые сценарии по логистике, без сюжета и персонажей) | ВойнаОркестровка | `beta` |
| Баст Фронт ближнего боя | БастФронт | `beta` |
| Код мода, генератор данных, тесты, документация | репозиторий этого мода | `beta` |
| Общий CI , упаковка, ворота выпуска, мишень блокировки | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Заказ на изменение, пересекающее репозитории

1. Двигатель PR в `fork` , объединен. Пины движка перемещаются только в теги выпуска вышестоящей версии, поэтому инфраструктура и PR-модули, которым это необходимо, ждут, пока вышестоящая версия (VlcekM/ MeitouClient ) не выпустит изменение. А пока собирайте ветку движка локально ([local-development.md](local-development.md#change-the-engine-and-a-mod-together) ).
2. Фреймворк PR , который вносит изменения (и перемещает свой вывод `engine` в версию, в которой есть все, что ему нужно), объединен с `beta` . CI публикует пререлиз бета-версии.
3. Мод PR , который перемещает `meitou.lock.json` в эту предварительную версию (и `engine` в фиксацию движка блокировки), объединен в `beta` .
4. После выпуска фреймворка продвигается и помечается: мод PR , который перемещает блокировку на `<framework>-v<version>` .

Каждый PR связывает предыдущий. Критическое изменение фреймворка API затрагивает второстепенную версию фреймворка и перечисляет все зависимые репозитории; каждый зависимый получает свой собственный замок PR .

В цепочке шаг 2 повторяется один раз для каждого уровня сверху вниз: появляется изменение Ядра Логистики, затем Военная Оркестрация PR блокирует то, что Логистика создает и публикует свои собственные, затем Баст Фронт PR блокирует оба.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Правила вставки в инструкции агенту

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
## Шаблон запроса

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
