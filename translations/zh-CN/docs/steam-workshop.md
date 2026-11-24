<a id="publishing-to-the-steam-workshop"></a>
# 发布到 Steam 创意工坊

您可以从版本 ZIP 手动创建一次 mod 的创意工坊项目。之后，[Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) 在您的 Windows PC 上运行，并按计划将每个新的 GitHub 版本上传到该项目。所有步骤都在 PC 上的 **Windows PowerShell** 中运行，登录到拥有该项目的 Steam 帐户。

命令使用这些值。在每个 PowerShell 窗口中设置一次，并使用 mod 的文件夹名称（如 `release.yml`  `mod-folder` ）及其第一个发行版本：

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. 发布模组

1. 将 `beta` 合并到 `main` ，然后打开 PR 并将 `main` 合并到 `release` （[分支](../README.md#branches) ）。
2. 等待 `release` 上的发布工作流程。它使用 `meitou.json` 中的版本（例如 `example-mod-v0.1.0` ）标记合并 `<slug>-v<version>` ，并使用 `<Mod>-<version>.zip` 和 `<Mod>-<version>.zip.sha256` 创建版本。如果在标记之前运行失败， `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` 会手动推送标签（ `-DryRun` 仅打印命令）。

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2.安装Workshop Watcher并登录Steam

按照创意工坊观察者指南中的[安装](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) 。安装程序将 steamcmd 下载到 `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` 并登录到您的 Steam 帐户。当它询问 `Add a mod to watch now?` 时，回答 `n` ：该项目尚不存在。如果已经安装了 Workshop Watcher，请跳过此步骤。

<a id="3-prepare-the-items-files"></a>
## 3. 准备项目的文件

1. 下载 ZIP 版本并将其解压到 `$Work` 中：

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` 现在持有 `$Mod.dll` 、 `meitou.json` 和 `$Mod.mod` 。
2. 将项目的预览图像保存为 `$Work\preview.png` : PNG 或 JPG ，位于 1 MB 下，方形（512 x 512 像素有效）。

<a id="4-write-the-vdf"></a>
## 4.写入 VDF

steamcmd 从 VDF 文件创建项目： `"workshopitem"` 块内的 `"key" "value"` 对的文本文件。此命令写入 `$Work\create.vdf` 并填充路径。首先编辑 `title` 和 `description` ：

```powershell
$Dir = $Work.Replace('\', '/')
@"
"workshopitem"
{
    "appid"           "233860"
    "publishedfileid" "0"
    "contentfolder"   "$Dir/$Mod"
    "previewfile"     "$Dir/preview.png"
    "visibility"      "2"
    "title"           "[Meitou] My Mod"
    "description"     "What the mod does. Requires MeitouClient."
    "changenote"      "$Mod $Version"
}
"@ | Set-Content "$Work\create.vdf" -Encoding ASCII
```

| 钥匙 | 价值 |
| --- | --- |
| `appid` | `233860` 、 Kenshi 的 Steam 应用程序 ID 。 |
| `publishedfileid` | `0` 创建一个新项目。任何其他数字都会更新该现有项目。 |
| `contentfolder` | 其文件成为项目内容的文件夹的完整路径：来自 ZIP 的 mod 文件夹，而不是其父文件夹。 |
| `previewfile` | 预览图像的完整路径。 |
| `visibility` | `0` 公开， `1` 仅限好友， `2` 私人， `3` 未公开。从 `2` 开始，然后在项目页面上进行更改。 |
| `title` | 该物品在创意工坊中的名称。像其他 Meitou 代码 mod 一样，以 `[Meitou]` 启动它。 |
| `description` | 一行文字。之后在项目页面上写下完整的描述（它支持Steam的BBCode）。 |
| `changenote` | 项目的“更改注释”选项卡的第一个条目。 |

值规则：

- 使用正斜杠 ( `C:/Users/me/Downloads/...` ) 写入路径。反斜杠必须加倍 ( `C:\\Users\\me\\...` )。
- 值内的双引号写作 `\"` 。
- 保留文件 ASCII 。

<a id="5-create-the-item"></a>
## 5. 创建项目

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` 是您登录 Steam 时使用的名称，与安装程序相同。 steamcmd 使用保存的登录名。如果它要求输入密码或 Steam Guard 代码，请运行 `powershell -ExecutionPolicy Bypass -File $Watcher login` ，然后再次运行该命令。

运行以 `Success.` 结束。打开 Steam > 你的个人资料 > 创意工坊物品，打开新物品并复制其 URL 中 `?id=` 后面的数字：即物品 ID 。

<a id="6-finish-the-items-page"></a>
## 6. 完成项目页面

在 Steam 中的物品页面上：

1. 如果页面显示横幅，请接受 Steam 创意工坊法律协议。在此之前，只有您可以看到该项目。
2. 写出完整的描述。
3. 如果该模组需要另一个创意工坊模组（例如 Rivalry Core），请将其添加到**添加/删除所需项目**下。然后订阅者也被要求订阅它。由于 `meitou.json`  `dependencies` 中的依赖性，启动器的排列将其放在加载顺序中的第一位。
4. 页面准备就绪后，将可见性更改为 **公开**。

<a id="7-watch-the-item"></a>
## 7.观看物品

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

试运行报告当前版本已上传。一旦 `$Work` 出现，请将其移除。

<a id="each-release-after-that"></a>
## 之后的每个版本

按照步骤 1 发布 mod。观察者的下一次预定运行将下载 ZIP ，检查其 SHA -256 并将其上传到项目，并使用发布标题和链接作为更改说明。它永远不会更改项目的标题、描述、预览或可见性：更改项目页面上的内容。 `powershell -ExecutionPolicy Bypass -File $Watcher status` 显示时间表、下一次运行和最后一次上传。
