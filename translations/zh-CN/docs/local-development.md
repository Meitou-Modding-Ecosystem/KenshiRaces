<a id="local-development-setup"></a>
# 设置和更新本地仓库

仅将您正在处理的存储库克隆到一个文件夹中。每个都使用 `dotnet build` 自行构建，并且需要自行下载和哈希检查的框架。

<a id="repos"></a>
## 回购协议

每个 mod 存储库均以其 mod 命名。

| 回购协议 | 持有 | PR 转到 |
| --- | --- | --- |
| MeitouClient | 引擎及其模组挂钩 | `fork` |
| 竞争核心 | Rivalry Core框架及其代理技巧 | `beta` |
| 物流核心 | 物流核心框架 | `beta` |
| 战争编排 | 战争编排框架（使用后勤核心） | `beta` |
| 韧皮前线 | Bast Front 混战（使用战争编排和后勤核心） | `beta` |
| 灰手 | 灰烬之手（使用竞争核心） | `beta` |
| 畜牧业 | 畜牧业 | `beta` |
| 小队竞技场 | SquadArena 模组和小队评级工具 | `beta` |
| GutenStructure （象牙灾难） | 共享 CI ，包装，释放门，框架锁定目标 | `beta` |

<a id="folders"></a>
## 文件夹

这些步骤使用八个文件夹。在每个新的 PowerShell 窗口中将它们设置为变量，然后按原样粘贴以下命令：

```powershell
$Repos     = "C:\Users\<you>\source\repos"
$Engine    = "$Repos\MeitouClient"
$Rivalry   = "$Repos\RivalryCore"
$Ashen     = "$Repos\AshenHand"
$Husbandry = "$Repos\Husbandry"
$Logistics = "$Repos\LogisticsCore"
$War       = "$Repos\WarOrchestration"
$Bast      = "$Repos\BastFront"
```

<a id="prerequisites"></a>
## 先决条件

- 。 NET 11 SDK （每个存储库的 `global.json` 命名版本）和 Git。
- 编辑器： Visual Studio 、带有 C# 开发工具包的 VS Code 或 Rider。单独的终端也可以工作。
- 安装了 Kenshi 。如果它不在 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` 中，请将 `KENSHI_PATH` 环境变量设置为其文件夹。

<a id="clone"></a>
## 克隆

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

每个 mod 仓库都有自己的 `engine` 子模块，因此仅在编写引擎挂钩时才需要 `$Engine` 。

<a id="build-and-play-one-mod"></a>
## 构建并运行一个模组

**更改为模组的存储库：**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- 第一个构建将 `meitou.lock.json` 中命名的每个框架下载到 `deps` 中，如果其 SHA -256 不匹配，则停止。
- `dotnet run --project Play` 将锁定的框架和mod复制到 `Kenshi\mods` 中并启动引擎。 Visual Studio 、 VS Code 和安装的启动器： [play.md](play.md) 。
- 对于任何依赖项，在启动器中启用 mod 上方的框架。

<a id="change-a-framework-and-a-mod-together"></a>
## 一起改变框架和模组

将 mod 存储库指向您的框架克隆：

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- `$Ashen` 中的构建现在从 `$Rivalry` 编译 Rivalry Core 并使用 `$Rivalry\engine` 中的引擎，并且 Play 部署该 Rivalry Core 构建。
- `Meitou.user.props` 被 git 忽略。如果它已经保存了您的 Kenshi 设置，请在其 `PropertyGroup` 内添加这两行。
- `MeitouFrameworkSources` 采用由 `;` 分隔的框架项目路径，例如 `$Logistics\LogisticsCore\LogisticsCore.csproj` 。 `MeitouEngineDir` 命名框架克隆所基于的引擎。
- 删除该行以返回到锁定的 ZIP 。

**框架链。**Bast Front 的 `meitou.lock.json` 列出了战争编排和后勤核心。要同时更改框架和 Bast Front，请在 Bast Front 中覆盖两者：

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

覆盖会向下传递，因此 War Orchestration 也是针对 `$Logistics` 构建的，并且整个构建使用一个引擎。仅覆盖战争编排，它是针对 Bast Front 锁中命名的后勤核心构建的。

要发布它：将框架 PR 合并到 `$Rivalry` 的 `beta` 中，后者发布了预发行版 `rivalry-core-<version>-beta.<commit>` 。然后，在 `$Ashen` 中，将 `meitou.lock.json` 设置为该标签和资产、 SHA -256 以及发行说明中的​​引擎提交，然后打开 mod PR 。在一条链中，自上而下地运输：后勤核心，然后是战争编排构建锁定它，然后韧带前线锁定两者。框架正式发布后，将锁移至 `rivalry-core-v<version>` 。

<a id="change-the-engine-and-a-mod-together"></a>
## 一起更换引擎和模组

检查 mod 存储库子模块内的引擎分支：

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- 不要提交新的引擎指针：引擎引脚仅移动到上游发布标签，因此一旦上游发布它，更改就会到达此存储库。
- 使用框架的 mod 也需要在该引擎上重建框架。使用上面的框架覆盖并检查 `$Rivalry\engine` 中的相同引擎分支。
- 着陆命令：将 PR 发动机改为 `fork` ；上游发布后，框架 PR 将 `$Rivalry\engine` 移动到该版本，然后 mod PR 移动其锁和 `engine` 。

<a id="switch-the-engine-to-play-or-test"></a>
## 切换引擎进行播放或测试

要播放最新的上游、发行版或引擎分支，请使用任何 mod 存储库中的脚本切换 `$Engine` 克隆：

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` ：VlcekM/ MeitouClient  `master` 的最新提交（脚本在丢失时添加 `upstream` 远程）。
- `-To Release` ：最新的上游发布标签 `vX.Y.Z` 。
- `-To Fork` ：组织分叉的 `fork` 分支，已拉出。
- `-Branch <name>` ：组织分支的分支，例如引擎 PR 的分支。
- 如果没有 `-Path` ，它将使用 mod 存储库旁边的 `MeitouClient` 文件夹。 `-Run` 在Release中构建游戏并启动； `-Build` 只构建它。
- Upstream、Release 和 `-Branch` 将克隆保留在分离提交上；本地 `master` 和 `fork` 分支不变。当克隆有未提交的更改时，脚本将停止而不更改任何内容。
- 它永远不会触及 mod repo 自己的 `engine` 子模块（即 `Update-Local.ps1` 和引脚）。

<a id="keep-clones-current"></a>
## 保持克隆最新

每次拉取后，每当构建因缺少引擎类型而失败（例如 `The type or namespace name 'RelationCause' could not be found` ）时，请关闭 Visual Studio 并在 mod 的存储库中运行更新脚本：

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- 它切换到 `beta` 并拉动它，然后将 `engine` 和 `build/gutenstructure` 更新为提交 `beta` 引脚（仅 `git pull` 将它们保留在旧引脚上）。 `-Branch <name>` 又拉了一根树枝。
- 它根据其引脚检查每个子模块并打印引擎版本。
- 它在 `Meitou.user.props` 或环境中标记 `MeitouEngineDir` 和 `MeitouFrameworkSources` ：它们使构建使用另一个存储库的引擎或框架，而不是此存储库的引脚。删除它们，除非您同时更改框架和模组。
- `-Clean` 删除了 git 忽略的 `bin` 和 `obj` 文件夹，因此旧引擎的构建不会被重用。 `-Build` 运行 `dotnet build` 。
- 它以 `Up to date` 或标记为 `FIX` 的行列表结尾。

仅当 `meitou.lock.json` 更改时，下一个版本才会再次下载框架。

[English](../../../docs/local-development.md) · [Kenshi Races](../README.md)
