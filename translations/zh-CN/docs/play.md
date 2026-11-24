<a id="build-and-run-locally"></a>
# 建造和运行本地

建造 `Play` 构建发动机和每个模式在这个复制器,复制模式和框架 `meitou.lock.json` 将游戏的名称输入到游戏的模式文件夹中,然后开始游戏。 使用任何你喜欢的工具:Visual Studio,VS Code或终端所有运行相同的结构。

三个都需要它。NET 11 SDK 以其子模块为主(`git clone --recurse-submodules <repo URL>`或 `git submodule update --init --recursive` 在现有克隆中。 没有他们,建筑就停止了。 `MEITOU001`把这个命令命名。

<a id="visual-studio"></a>
## 视觉工作室

1. 开启RPO `.slnx`.
2. 设置 `Play` 如果 Visual Studio 没有选择它(右键单击 > 设置为 Startup Project)。
3. 点击下载点旁边的下载点,然后按 F5(下载)或Ctrl+F5:

| 个人资料 | 跑步 | 用它为 |
| --- | --- | --- |
| 播放器(Motor + Mods) | `MeitouClient.exe` 由此建造的 `engine` 子模块 | 测试和解体您的模式; 图为模式代码工作中的分点 |
| Meitou Launcher(安装) | 发射器安装在Meitou释放, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | 更改模式列表或加载命令,并信任新的模式代码 |

<a id="vs-code"></a>
## 对代码

1. 安装 C# Dev Kit 扩展(VS 代码在打开文件夹时提供它;它在 `.vscode/extensions.json`).
2. 打开 repo 的根文件夹(文件 > 打开文件夹)。
3. 运行和驱动(Ctrl+Shift+D),选择 **播放器(Motor + Mods)**印刷F5。 它首先建立和部署,然后在发动机下启动。
Debugger;在模式代码工作中的分点。
4. 安装的启动器:终端 > 运行任务 > **Meitou Launcher(安装)**. 它建造和部署,然后打开启动器。

Terminal > Run Build Task (Ctrl+Shift+B) 只构建和部署。

<a id="without-an-editor"></a>
## 没有编辑

右点击 `tools\Build.ps1` > **使用 PowerShell**. 它建造和部署如 `dotnet build` (首先,如果缺席,检查下方) 并保持窗户开放,以便您可以阅读结果。 `-Test` 还进行测试; `-Run` 然后发动机开始了。

<a id="terminal"></a>
## 终端

在雷波的根文件夹中:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` 和 `dotnet test` 创建和测试每个项目。
- `dotnet run --project Play` 构建、部署和启动发动机(“播放(发动机 + 模式)”配置)。
- 要使用安装的启动器,则无启动发动机,然后打开启动器:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## 什么是游戏建设

- 查找模式:每个顶级文件夹持有 `meitou.json` 和 `<Folder>/<Folder>.csproj`. 一个新的模式不需要改变 `Play`.
- 创建发动机和每个模式 **释放**无论您选择的配置如何:Debug 发动机构建投射 `TaskStart.CheckStarts`
一旦一个模式开始一个字符的任务。 模式代码中的分点   现在仍在运行(释放符号正在部署)。
- 复制每个模式 `<Mod>.dll`, `<Mod>.pdb` 和 `meitou.json` 到 `<Kenshi>\mods\<Mod>\`. 首先关闭游戏:运行游戏锁定DLL。
- 将每个框架复制到 `meitou.lock.json` 从下载的文件夹到 `<Kenshi>\mods\<Framework>\`或者,在构建的框架
由一个克隆([local-development.md](local-development.md)其建造产量。
- 用数据发电机(`<Mod>.Data/<Mod>.Data.csproj`跑步如 `<generator> <Kenshi folder> <output .mod>`( ) 得到它的 `<Mod>.mod`
从安装游戏中产生;一个失败是构建警告,而模式则没有它。 每個其他模式都會得到一個空白 `<Mod>.mod` 當它沒有時,一個存在的永遠不會被取代。
- 一个模特与一个 `Structures` 文件夹得到它的 `.glb` 进口到其部署的文件夹,记录合并到其 `.mod`
([structures.md](structures.md)).
- 当模块文件夹不存在时,没有任何复制,构建说。
- 启动器的播放按钮开始了它安装的发布,而不是这个repo的发动机。 关闭它并运行“播放(引擎 + 模式)”以测试您的
建造。

<a id="settings"></a>
## 设置

缺点:游戏 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`此分類上一篇: Mods `<game>\mods`. 换了它们与 `KENSHI_PATH` 环境变量(发动机也读到它,所以两者都同意),或者在Git未知的机器上 `Meitou.user.props` 在 repo 的根文件夹中:

```xml
<Project>
  <PropertyGroup>
    <KenshiDir>D:\Games\Kenshi</KenshiDir>           <!-- game folder; also sets the mods folder -->
    <KenshiModsDir>D:\Games\Kenshi\mods</KenshiModsDir> <!-- mods folder only -->
    <PlayConfiguration>Debug</PlayConfiguration>      <!-- engine and mods configuration (default Release) -->
    <PlayDeploy>false</PlayDeploy>                    <!-- build and run without copying -->
    <PlayArguments>--continue</PlayArguments>         <!-- engine command line (Visual Studio and dotnet run) -->
  </PropertyGroup>
</Project>
```

发动机自行找到游戏(Steam); `KenshiDir` 只设置在 `Meitou.user.props` 没有转到它。 VS 代码的启动配置运行了发布引擎; 将其 `.vscode/launch.json` 當地,如果你改變 `PlayConfiguration`.

<a id="first-run"></a>
## 第一次跑步

- 运行安装的启动器一次(上面的任何方式):启动模式和框架,并信任它的代码。
- 如果副本失败“拒绝访问”,请给用户修改权 `Kenshi\mods` (属性 > 安全) 而不是运行您的
编辑作为管理员。
