<a id="agents-working-across-repos"></a>
# 协调跨仓库的更改

为每个涉及更改的存储库提供一个代理，告诉它哪个存储库拥有每个部分，并让它按照依赖顺序放置 PR：引擎，然后是框架，然后是 mod。每次更改一个代理会话，而不是每个存储库一个代理会话。

<a id="give-the-agent-the-repos"></a>
## 向代理提供存储库

- **云会话：**每个存储库必须附加到会话或项目。代理并排克隆它们，并在每个分支各自的分支上工作。
- **您的 PC （远程控制）：**[local-development.md](local-development.md) 文件夹中的克隆。代理还可以执行云会话无法执行的操作，例如 `gh repo create` 。

<a id="where-each-part-goes"></a>
## 每个部分去哪里

| 改变 | 回购协议 | PR 进 |
| --- | --- | --- |
| 引擎挂钩（事件、保存字段、命令） | MeitouClient | `fork` |
| Rivalry Core API 或行为 | 竞争核心 | `beta` |
| 物流核心（节点、边缘、所有者、库存、运输） | 物流核心 | `beta` |
| 战争编排（后勤作战场景，无故事或人物） | 战争编排 | `beta` |
| 韧皮前线近战 | 韧皮前线 | `beta` |
| mod 的代码、数据生成器、测试、文档 | 该模组的回购协议 | `beta` |
| 共享 CI ，包装，释放门，锁定目标 | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## 跨存储库的更改订单

1. 发动机 PR 变为 `fork` ，合并。引擎引脚仅移动到上游发布标签，因此需要它的框架和 mod PR 会等到上游 (VlcekM/ MeitouClient) 发布更改。在那之前，在本地针对引擎分支进行构建（[local-development.md](local-development.md#change-the-engine-and-a-mod-together) ）。
2. 进行更改的框架 PR （并将其 `engine` 引脚移至具有所需内容的版本），合并到 `beta` 中。 CI 发布 Beta 预发行版。
3. Mod PR 将 `meitou.lock.json` 移动到该预发布（并将 `engine` 移动到锁的引擎提交），并合并到 `beta` 中。
4. 框架版本升级并标记后：将锁移至 `<framework>-v<version>` 的 mod PR 。

每个 PR 都链接它前面的一个。一个破坏性的框架 API 更改会影响框架的次要版本并列出每个依赖的存储库；每个依赖者都有自己的锁 PR 。

在一条链中，步骤 2 每层自上而下重复一次：后勤核心变更落地，然后战争编排 PR 锁定后勤构建并发布自己的，然后堡垒前线 PR 锁定两者。

<a id="rules-to-paste-into-the-agents-instructions"></a>
## 粘贴到代理指令中的规则

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
## 提示模板

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```

[English](../../../docs/agents-across-repos.md) · [Kenshi Races](../README.md)
