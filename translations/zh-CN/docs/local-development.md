<a id="local-development-setup"></a>
# 地点开发 图

只将您所工作的复制剂列入一个文件夹。 每个人都建立在自己的 `dotnet build`它需要下载和检查的框架。

<a id="repos"></a>
## 休息

每個 mod repo 都以其 mod 命名。

| 休息 | 保持 | PRS 走向 |
| --- | --- | --- |
| MeitouClient | 发动机及其模式 | `fork` |
| 里瓦利 | Rivalry Core 框架及其代理技能 | `beta` |
| 物流中心 | 物流核心 | `beta` |
| 战争 | 战争乐团框架(使用物流核心) | `beta` |
| 巴斯特前 | Bast Front Melee(用在地的战争乐团和物流核心) | `beta` |
| 阿森纳 | 火手(用 Rivalry Core) | `beta` |
| 婚姻 | 婚姻 | `beta` |
| 队伍 | SquadArena mod 和 squad 评级工具 | `beta` |
| 天文学(TheIvoryDisaster) | 共享CI,包装,释放门,框架锁目标 | `beta` |

<a id="folders"></a>
## 文件夹

这些步骤使用八个文件夹。 在每个新 PowerShell 窗口中设置它们为变量,然后按下列命令:

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
## 原則

- .NET 11 SDK (每個RPO `global.json` 命名版本)和Git。
- 编辑器:Visual Studio,VS代码与C# Dev Kit,或Rider。 只有一个终端才会工作。
- Kenshi 安装了。 如果它不在 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`设置 The `KENSHI_PATH` 环境变量
到他的文件夹。

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

每個 mod repo 都有自己的 `engine` 模块,所以 `$Engine` 只需要写发动机。

<a id="build-and-play-one-mod"></a>
## 创建和播放一个模式

**转换到 mod 的 repo:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- 第一個建築下載,每個名為 `meitou.lock.json` 进入 `deps` 如果它停止了 SHA第256章 没什么好
- `dotnet run --project Play` 将锁定框架和模式复制 `Kenshi\mods` 发动机开始了。 视觉工作室 vs 代码
安装的启动器: [play.md](play.md).
- 允许启动器的模式以上的框架,如任何依赖性。

<a id="change-a-framework-and-a-mod-together"></a>
## 改变一个框架和一个模式

在您的框架克隆中指定 mod repo:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- 建在 `$Ashen` 此分類上一篇: Rivalry Core `$Rivalry` 使用发动机在 `$Rivalry\engine`,并播放Rivalry Core构建的游戏。
- `Meitou.user.props` 被忽略了。 如果它已经保留了你的 Kenshi 设置,添加两条线在其内部 `PropertyGroup` 相反。
- `MeitouFrameworkSources` 以框架项目的路径分开 `;`是的,G `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` 发动机的名称,框架克隆建立。
- 删除线路 返回锁 ZIP.

**框架链** 最前线 `meitou.lock.json` 战争乐队和物流核心。 要改變兩個框架和巴斯特前線,將兩個在巴斯特的前線:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

下,所以战争乐团建造反 `$Logistics` 而且,整个建筑使用一个发动机。 它只覆盖了战争乐队,它反对在巴斯特前线的锁中命名的物流核心。

发送:将 PR 框架融入 `beta` 由 `$Rivalry`以此为主,以便在 `rivalry-core-<version>-beta.<commit>`. 然后,在 `$Ashen`设置 `meitou.lock.json` 那种财产和财产, SHA第256章 打开了 在一个链条上,船顶:物流核心,然后一个战争组建锁定它,然后Bast前锁定两者。 開放框架後,將關閉轉移到 `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## 改变发动机和模式

查看您的发动机分支在 mod repo 子模块内:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- 不要承诺新的发动机指标:发动机只移动到上流的发行标签,所以变化达到这一点。
释放它。
- 使用框架的模式也需要该引擎重建的框架。 使用上方的框架,并检查相同的
发动机分支 `$Rivalry\engine`.
- 命令登陆它:引擎PR `fork`在上流发布后,框架的PR移动 `$Rivalry\engine` 到此,释放,然后
mod PR 移动其锁 `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## 转动发动机进行游戏或测试

要播放最新升级、发布或发动机分支,请转换 `$Engine` 用任何 mod repo 中的脚本进行克隆:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`下一篇:最新承诺 VlcekM/MeitouClient `master` (剧本添加了 `upstream` 缺席时远程。
- `-To Release`下一篇:最新升级版 `vX.Y.Z`.
- `-To Fork`主持人:Org Fork `fork` 枝子,拖着。
- `-Branch <name>`一个器官的分支,如发动机的分支。
- 没有 `-Path` 它使用了 `MeitouClient` 接近Mod Repo。 `-Run` 建立游戏在发布中,并启动它; `-Build` 只有
建造它。
- 升流、释放和 `-Branch` 放下克隆在一个分离的承诺;当地 `master` 和 `fork` 分支没有改变。 剧本
停止不改变任何东西,当克隆有未经命令的变化。
- 它从来没有触摸到Mod Repo的 `engine` 模块(即 `Update-Local.ps1` 和PIN。

<a id="keep-clones-current"></a>
## 保持克隆的流程

每次拖动后,每当发动机类型失败时(例如: `The type or namespace name 'RelationCause' could not be found`),关闭 Visual Studio 并在 mod 的 repo 中启动更新脚本:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- 它交换为 `beta` 把它拿出来,然后更新。 `engine` 和 `build/gutenstructure` 至于承诺 `beta` ( )`git pull` 独自一人
把它们放在旧的身上。 `-Branch <name>` 把另一个分支
- 它检查了每个子模块对其插槽,并打印了发动机版本。
- 它旗帜 `MeitouEngineDir` 和 `MeitouFrameworkSources` 在 `Meitou.user.props` 或者是环境:他们使建筑使用另一个
repo 的发动机或框架,而不是这个 repo 的插槽。 除非您正在改变框架和模式。
- `-Clean` 删除Git 未知 `bin` 和 `obj` 文件夹,所以没有旧的发动机的重复使用。 `-Build` 跑步 `dotnet build`.
- 它结束了 `Up to date` 或列出标记的线条 `FIX`.

下一篇: 下载框架时 `meitou.lock.json` 改变了。
