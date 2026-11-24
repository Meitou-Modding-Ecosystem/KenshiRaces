<a id="kenshi-races"></a>
# Kenshi Races

游戏人体骨骼上的 [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) 的新种族，可在角色编辑器中玩： Leoma 和 Crevan （男性和女性）和 Crabfolk （蟹人，蟹后），每个人体骨骼种族的 59 个剪辑的动画库，以及其他 mods 添加自己的种族和子种族的框架。 GPL -3.0 或更高版本。

- 比赛、他们的记录以及在游戏中的测试：[docs/kenshi-races.md](docs/kenshi-races.md) 。
- 使用这些比赛或从另一个模组添加您自己的比赛：[docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) （代理技能 `.claude/skills/kenshi-races-dependent` ）。
- 动画库（人体骨骼上每个种族的 59 个剪辑）及其制作方法：[docs/animations.md](docs/animations.md) 。
- 将装配好的 `.glb` 身体（ Meshy 、 Hi3D “3ds Max Biped”）安装到游戏的骨架上：[docs/race-import.md](docs/race-import.md) 。

- 使用 Visual Studio 、 VS Code 或终端（此存储库的引擎或已安装的启动器，部署到游戏）在本地构建和运行：[docs/play.md](docs/play.md) 。
- 在您的计算机上设置存储库：[docs/local-development.md](docs/local-development.md) 。
- 指导代理进行跨存储库的更改：[docs/agents-across-repos.md](docs/agents-across-repos.md) 。
- 引擎引脚、版本、 CI 和版本的代理规则：[ CLAUDE .md](../../CLAUDE.md) （与 `AGENTS.md` 相同）；分步程序： `.claude/skills/meitou-engine-release` 。
- 根据最新版本检查构建的模组 Meitou 发布 （ CI 做每一个 PR ): [文档/refcheck.md ](docs/refcheck.md) .
- 首次将 mod 发布到 Steam 创意工坊，然后在每个版本中发布：[docs/steam-workshop.md](docs/steam-workshop.md) 。
- 将 `.glb` 模型（ Meshy 、 Hi3D 、 Blender ）转换为可构建的结构，不需要 FCS ：[docs/structurals.md](docs/structures.md) ；首先获取生成模型的多边形数、大小和纹理：[docs/model-prep.md](docs/model-prep.md) 。

<a id="numbers"></a>
## 数字

任务类型（ `AddGoalType` 、 `AddJobType` ）和对话代码（ `AddDialogueCondition` 、 `AddDialogueEffect` ）由每个加载的 mod（[mod 编号块](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) ）共享。 Kenshi Races 声称没有。

| 数字 | 水池 | 使用 |
| --- | --- | --- |

<a id="layout"></a>
## 布局

- `engine/` ： MeitouClient 作为 git 子模块，固定到引擎提交 mod 构建。 `git pull` 不会移动它：每次拉动运行 `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` 后（[保持克隆当前的](docs/local-development.md#keep-clones-current) ）。
- `KenshiRaces/` ：mod 的项目和 `meitou.json` ； `KenshiRaces/items/races/` ：装配体（承诺进口输出）。 `KenshiRaces.Tests/` ：其测试（合成数据，无需安装游戏）。 `<Mod>.Data/` （可选）：从已安装的游戏写入 `<Mod>.mod` 的生成器。 `<Mod>/Structures/` （可选）： `.glb` 模型在部署时作为可构建结构导入（[docs/structurals.md](docs/structures.md) ）。
- `meitou.lock.json` ：框架发布针对（[框架锁](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ）构建的 mod；框架更新工作流程将其移动到新版本，并在 CI 通过时合并（[框架更新](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ）。
- `Play/` ：一键构建、部署和运行。
- `tools/` : `Update-Local.ps1` ，本地拉力和引擎引脚检查（[保持克隆当前](docs/local-development.md#keep-clones-current) ）； `refcheck.sh` 和 `RefCheck/` ，引擎引用检查（[docs/refcheck.md](docs/refcheck.md) ）； `StructureImport/` ，模型导入（[docs/structurals.md](docs/structures.md) ）； `RaceImport/` ，身体配件（[docs/race-import.md](docs/race-import.md) ）； `model-info.py` 和 `prepare-model.py` ，模型检查和纹理收缩（[docs/model-prep.md](docs/model-prep.md) ）。
- `Directory.Build.props` 导入引擎，因此 mods 使用游戏的设置构建（警告是错误）。
- `Directory.Build.rsp` 将引擎引用标记为最旧的 Meitou 发布的 mods 支持，等于每个 `gameVersion` 楼层：游戏拒绝引用较新引擎标记的 mod，并且子模块构建 `1.0.0.0` （[版本控制](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ）。 Meitou 兼容性工作流程每天根据新的 Meitou 版本检查模组（[meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ）。

<a id="branches"></a>
## 分支机构

PR 为 `beta` ； CI （构建和测试）必须通过。  一旦在游戏中进行测试， `beta` 就会升级为 `main` ，生产过程会经历 PR 到 `release` 。每次合并到 `release` 时都会将其标记为 `<slug>-v<version>` 并发布 mod 的 zip ( `.github/workflows/release.yml` ) 除非该标记存在； `tools\Tag-Release.ps1` 是手动后备。

- 新存储库分支保护和永久 beta/main/release 审查：[docs/branch-policy.md](docs/branch-policy.md) 。模板复制来源； GutenStructure 通过协调应用设置。
