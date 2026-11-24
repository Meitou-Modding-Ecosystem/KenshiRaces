# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · [Deutsch](../de/README.md) · [Español](../es/README.md) · [Italiano](../it/README.md) · [Português do Brasil](../pt-BR/README.md) · [Русский](../ru/README.md) · [日本語](../ja/README.md) · [한국어](../ko/README.md) · 简体中文

Kenshi Races 为 [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) 的角色编辑器添加了三个可玩种族。Leoma 和 Crevan 均有男性和女性身体模型，Crabfolk 则包含 `crabman` 和 `crabqueen` 两个亚种。它们都使用游戏原有的人类骨骼。模组还提供了59段可供所有人形骨骼种族使用的动画，以及供其他模组添加自定义种族和亚种的框架。 GPL-3.0-or-later.

## 指南

- [种族、数据记录和游戏内测试](docs/kenshi-races.md)
- [在其他模组中使用本框架并添加亚种](docs/depending-on-kenshi-races.md)
- [59段动画及其制作方法](docs/animations.md)
- [将GLB身体模型适配到游戏骨骼](docs/race-import.md)
- [使用Visual Studio、VS Code或终端构建并运行](docs/play.md)
- [设置和更新本地仓库](docs/local-development.md)
- [协调跨仓库的更改](docs/agents-across-repos.md)
- [检查与新版Meitou的兼容性](docs/refcheck.md)
- [发布到Steam创意工坊](docs/steam-workshop.md)
- [无需FCS即可将GLB模型导入为建筑](docs/structures.md)
- [调整模型面数、尺寸和纹理](docs/model-prep.md)
- [分支保护与审查规则](docs/branch-policy.md)
- [仓库规则与代理操作流程](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## 共享编号

所有已加载的模组共用任务类型和对话代码的编号空间。Kenshi Races 未占用任何编号。 (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## 仓库结构

- `engine/` — MeitouClient 子模块固定到构建此模组所用的提交。每次 pull 后请用下方命令更新。 [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — 模组项目及清单。已适配的身体模型已提交到仓库。测试使用合成数据，无需安装游戏。
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — 可选项目从已安装的游戏生成模组数据，或在部署时将 GLB 模型导入为建筑。 [StructureImport](docs/structures.md)
- `meitou.lock.json` — 锁定所用框架的版本。更新工作流采用新版本，待 CI 通过后合并。 [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — 一键构建、部署并运行。
- `tools/` — 用于同步、检查引擎引用、导入建筑、适配身体模型和处理纹理的工具。 [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — 导入引擎的构建设置。编译警告按错误处理。
- `Directory.Build.rsp` — 将引擎引用固定到模组支持的最低版本。引用高于当前引擎版本的模组会被游戏拒绝。子模块默认使用固定程序集版本。 (`gameVersion`, `1.0.0.0`).  兼容性工作流每天检查与新版 Meitou 的兼容性。 [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## 分支

PR 应提交至 beta，且必须通过构建和测试。在游戏中验证后可将 beta 提升至 main。正式发布须经面向 release 的 PR。每次合并至 release 时，若版本标签尚不存在，就创建标签并发布 ZIP。必要时可用标签脚本手动处理。 (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
