<a id="kenshi-races"></a>
# Kenshi 赛事

新赛事为 [客人](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) 在游戏中的人类骨骼,可在角色编辑器中播放: Leoma 和 Crevan (男女)和 Crabfolk 一个动画图书馆为每个人骨骼,以及其他模式添加自己的种族和种族。 GPL3.0 或更晚。

- 赛车,他们的记录,并在游戏中测试它们: [docs/kenshi-races.md](docs/kenshi-races.md).
- 使用这些赛车或从另一个模式添加自己的: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (代理技能)
`.claude/skills/kenshi-races-dependent`).
- 动画图书馆(在人类骨骼上每种种族的59个剪辑)以及它是如何制作的: [docs/animations.md](docs/animations.md).
- Fitting a rigged `.glb` 身体(Meshy, Hi3D “3ds Max Biped”)在游戏的骨骼上: [docs/race-import.md](docs/race-import.md).

- 使用 Visual Studio、VS Code 或终端构建和运行本地(该 repo 的发动机或安装的启动器,用于游戏): [docs/play.md](docs/play.md).
- 在您的机器上设置休息: [docs/local-development.md](docs/local-development.md).
- 指示代理人对延迟休息的变化: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- 引擎的规则,版本,CI和发布: [CLAUDE.md](../../CLAUDE.md) (如同 `AGENTS.md`( );步骤程序:
`.claude/skills/meitou-engine-release`.
- 对最新的 Meitou 发布进行测试(CI 在每个 PR 上进行测试): [docs/refcheck.md](docs/refcheck.md).
- 首次发布 Steam 工作室的模式,然后每次发布: [docs/steam-workshop.md](docs/steam-workshop.md).
- 转身 `.glb` 模型(Meshy, Hi3D,混合器)到可构建的结构,不 FCS 需要: [docs/structures.md](docs/structures.md);
首先,要获得产生的模型的聚数、尺寸和结构: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## 数字

任务类型(`AddGoalType`, `AddJobType`)和对话代码(`AddDialogueCondition`, `AddDialogueEffect`( ) 由每個充電模式分享( )[模型号码区块](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi 种族不要求任何人。

| 号码 | 游泳池 | 使用 |
| --- | --- | --- |

<a id="layout"></a>
## 布局

- `engine/`: MeitouClient 作为一个 git 子模块,粘贴到发动机承诺的模式构建反对。 `git pull` 它不移动: 后
每個Pull Run `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([保持克隆流动](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`• 由 mod 的项目和 `meitou.json`; `KenshiRaces/items/races/`定制机构(委托进口产量)。
`KenshiRaces.Tests/`其测试(合成数据,不需要游戏安装)。 `<Mod>.Data/` (可选):写作的发电机 `<Mod>.mod` 从安装的游戏。 `<Mod>/Structures/` (可选 ) : `.glb` 以可建造的建筑物为基础([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`: 框架释放了 mods 构建反
([框架锁](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)); 框架更新工作流将其转移到新的发布和合并,当 CI 通过([框架更新](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`单击创建、部署和运行。
- `tools/`: `Update-Local.ps1`,本地拖动和发动机点检查([保持克隆流动](docs/local-development.md#keep-clones-current));
`refcheck.sh` 和 `RefCheck/`, 引擎引用检查器([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`, 模型进口([docs/structures.md](docs/structures.md)); `RaceImport/`( )  ([docs/race-import.md](docs/race-import.md)); `model-info.py` 和 `prepare-model.py`,模型检查和结构缩小([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` 输入发动机,所以模式与游戏的设置构建(警告是错误)。
- `Directory.Build.rsp` 标签发动机的参考,因为最古老的 Meitou 释放了 mods 支持,相当于每个 `gameVersion` 地板:The
游戏拒绝一个引用一个新的发动机标签的模式,而子模块构建 `1.0.0.0` ([版本](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). 互相互相相互相互    日程 對  新  發行  則 則 會 檢查 ([相同的](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## 分支

PR 进入 `beta`CI(构建和测试)必须通过。 `beta` 被推广为 `main` 一旦在游戏中进行测试,生产将通过PR进行 `release`. 每个人都融入 `release` 标签它 `<slug>-v<version>` 此分類上一篇: ZIP(`.github/workflows/release.yml`除非此标签存在; `tools\Tag-Release.ps1` 这是手动落后。

- 新品种保养和永久性 beta/main/release 评论: [docs/branch-policy.md](docs/branch-policy.md). 模板复制来源;GutenStructure通过和解应用设置。
