<a id="agents-working-across-repos"></a>
# 工作者在休闲中

给一个代理,每个转换,告诉它哪个部分,并在依赖的顺序:发动机,然后框架,然后模式。 每個變化,而不是每個轉換。

<a id="give-the-agent-the-repos"></a>
## 让代理人休息

- **天空云:** 每個重點都必須附上會議或項目。 代理人把它们一边克隆,并在自己的分支上工作。
在每一个。
- **您的PC(远程控制):** 以便在文件夹中的克隆 [local-development.md](local-development.md). 代理人也可以做什么
云会不会,如 `gh repo create`.

<a id="where-each-part-goes"></a>
## 每個地方都會走

| 改變 | 休息 | PR 入口 |
| --- | --- | --- |
| 发动机(事件、保存字段、命令) | MeitouClient | `fork` |
| 雷达 雷达 API 或行为 | 里瓦利 | `beta` |
| 物流核心(节点、边缘、所有者、库存、运输) | 物流中心 | `beta` |
| 战争乐队(战斗场景,没有故事或角色) | 战争 | `beta` |
| 直前 直前 | 巴斯特前 | `beta` |
| 一个模式代码,数据发明器,测试,DOCS | 此分類上一篇: Mod's Repo | `beta` |
| 共享 CI,包装,释放门,锁定目标 | 天文结构 | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## 命令交往休息的变化

1. 引进PR `fork`混合了。 发动机点只移动到上流发布标签,因此需要等待的框架和模式的PR
( )VlcekM/MeitouClient释放变革。 到目前为止,将对发动机的分支建立在本地([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. 框架 PR 使变革(并移动其 `engine` 到释放的,它有需要的东西),合并到 `beta`. 以便出版
这是Beta预告片。
3. 移动的PR模式 `meitou.lock.json` 以此为主, `engine` 到锁的发动机承诺),合并到 `beta`.
4. 框架发布后,将推广和标签:一个模式 PR 将锁转到 `<framework>-v<version>`.

每个人都把它放在前面。 破裂的框架 API 改变框架的较小版本,并列出每个依赖者;每个依赖者的自己的锁定。

在一个链条上,步骤2重复一次,上下:一个物流核心改变,然后一个战争组织的PR锁定,物流建立和发布自己的,然后Bast前的PR锁。

<a id="rules-to-paste-into-the-agents-instructions"></a>
## 规则 按代理人的指示

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
## 即时 template

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
