<a id="build-and-run-locally"></a>
# 本地构建并运行

构建 `Play` 会构建引擎和此存储库中的每个 mod，将 mod 和框架 `meitou.lock.json` 名称复制到游戏的 mods 文件夹中，然后启动游戏。使用您喜欢的任何工具： Visual Studio 、 VS Code 或终端都运行相同的构建。

这三个都需要 . NET 11 SDK 及其子模块的克隆（ `git clone --recurse-submodules <repo URL>` 或现有克隆中的 `git submodule update --init --recursive` ）。如果没有它们，构建会以 `MEITOU001` 停止，并命名该命令。

<a id="visual-studio"></a>
## Visual Studio

1. 打开存储库的 `.slnx` 。
2. 如果 Visual Studio 没有选择，请将 `Play` 设置为启动项目（右键 > 设置为启动项目）。
3. 在“开始”按钮旁边的下拉列表中选择一个配置文件，然后按 F5（调试）或 Ctrl+F5：

| 轮廓 | 跑步 | 用它来 |
| --- | --- | --- |
| 玩（引擎+模组） | `MeitouClient.exe` 从此存储库的 `engine` 子模块构建 | 测试和调试你的模组； mod 代码工作中的断点 |
| Meitou 启动器（已安装） | 从 Meitou 版本 `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` 安装的启动器 | 更改 mod 列表或加载顺序并信任新 mod 的代码 |

<a id="vs-code"></a>
## VS Code

1. 安装 C# Dev Kit 扩展（打开文件夹时 VS Code 会提供它；它位于 `.vscode/extensions.json` 中）。
2. 打开存储库的根文件夹（文件 > 打开文件夹）。
3. 运行并调试 (Ctrl+Shift+D)，选择 **播放（引擎 + mods）**，按 F5。它首先构建和部署，然后在调试器下启动引擎； mod 代码中的断点起作用。
4. 对于已安装的启动器：终端 > 运行任务 > ** Meitou 启动器（已安装）**。它会构建并部署，然后打开启动器。

终端 > 运行构建任务 (Ctrl+Shift+B) 仅构建和部署。

<a id="without-an-editor"></a>
## 没有编辑器

右键单击 `tools\Build.ps1` > **使用 PowerShell 运行**。它像 `dotnet build` 一样构建和部署（首先检查子模块是否丢失）并保持窗口打开，以便您可以读取结果。 `-Test` 也运行测试； `-Run` 然后启动发动机。

<a id="terminal"></a>
## 终端

在存储库的根文件夹中：

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` 和 `dotnet test` 构建并测试每个项目。
- `dotnet run --project Play` 构建、部署并启动引擎（“Play（引擎 + mods）”配置文件）。
- 要使用已安装的启动器，请在不启动引擎的情况下进行部署，然后打开启动器：

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## 游戏构建的作用是什么

- 查找 mods：包含 `meitou.json` 和 `<Folder>/<Folder>.csproj` 的每个顶级文件夹。新的 mod 不需要对 `Play` 进行任何更改。
- 无论您选择什么配置，都会在 **Release** 中构建引擎和每个 mod：一旦 mod 启动角色的任务，调试引擎构建就会抛出 `TaskStart.CheckStarts` 。 mod 代码中的断点仍然有效（部署释放符号）。
- 将每个 mod 的 `<Mod>.dll` 、 `<Mod>.pdb` 和 `meitou.json` 复制到 `<Kenshi>\mods\<Mod>\` 。首先关闭游戏：正在运行的游戏会锁定 dll。
- 将 `meitou.lock.json` 中的每个框架从其下载的发布文件夹复制到 `<Kenshi>\mods\<Framework>\` ，或者，对于从克隆 ([local-development.md](local-development.md) ) 构建的框架，复制其构建输出。
- 带有数据生成器（ `<Mod>.Data/<Mod>.Data.csproj` ，以 `<generator> <Kenshi folder> <output .mod>` 运行）的 mod 会从安装的游戏中生成 `<Mod>.mod` ；失败是构建警告，并且在没有它的情况下部署 mod。其他所有 mod 都没有时会得到一个空的 `<Mod>.mod` ；现有的永远不会被替换。
- 具有 `Structures` 文件夹的 mod 将其 `.glb` 模型导入到其部署文件夹中，记录合并到其 `.mod` （[structurals.md](structures.md) ）中。
- 当 mods 文件夹不存在时，不会复制任何内容，并且构建也是如此。
- 启动器的“播放”按钮启动它安装的版本，而不是此存储库的引擎。关闭它并运行“Play（引擎+ mods）”来测试您的构建。

<a id="settings"></a>
## 设置

默认值：游戏 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` ，mods 文件夹 `<game>\mods` 。使用 `KENSHI_PATH` 环境变量（引擎也会读取它，所以两者都同意）更改它们，或者在存储库根文件夹中的 git-ignored `Meitou.user.props` 中的每台机器上更改它们：

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

引擎自行找到游戏（Steam）；  仅在 `Meitou.user.props` 中设置的 `KenshiDir` 不会传递给它。 VS Code 的启动配置运行Release引擎；如果更改 `PlayConfiguration` ，请在本地编辑 `.vscode/launch.json` 。

<a id="first-run"></a>
## 第一次运行

- 运行已安装的启动器一次（上述任何方式）：启用 mod 及其框架，并信任其代码。
- 如果复制因“访问被拒绝”而失败，请授予您的用户对 `Kenshi\mods` 的修改权限（属性 > 安全性），而不是以管理员身份运行编辑器。
