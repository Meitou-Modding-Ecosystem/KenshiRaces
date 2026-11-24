<a id="publishing-to-the-steam-workshop"></a>
# 发表在 Steam 研讨会

您可以从一个版本中创建Mod的研讨会项目一次。 ZIP. 此后, [工作室Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) 在您的Windows PC上运行,并在日程中上传每个新的GitHub发布。 所有步骤都在 **Windows PowerShell** 在您的PC上,登录到该项目的Steam帐户。

命令使用这些值。 按 PowerShell 窗口设置一次,您的 mod 文件夹名称(如下) `release.yml` `mod-folder`其第一版:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. 释放MOD

1. 混合 `beta` 进入 `main`然后打开并合并一个PR `main` 进入 `release` ([分支](../README.md#branches)).
2. 等待发布工作流 `release`. 标签合并 `<slug>-v<version>` 与此同时,版本 `meitou.json` (例如
`example-mod-v0.1.0`( ) 并创建一个解放 `<Mod>-<version>.zip` 和 `<Mod>-<version>.zip.sha256`. 如果它在标签之前失败, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` 用手按下(`-DryRun` 只打印命令。

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. 安装 Workshop Watcher 并登录到 Steam

跟随 [安装](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) 工作室Watcher指南 安装器下载 SteamCMD 到 `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` 并将其登录到您的Steam帐户。 當他問 `Add a mod to watch now?`答案 `n`此分類上一篇: The item does not exist yet 如果WorkWatcher已经安装了,请将此步骤转移到

<a id="3-prepare-the-items-files"></a>
## 3. 准备项目的文件

1. 下载释放 ZIP 把它包在里面 `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` 现在持有 `$Mod.dll`, `meitou.json` 和 `$Mod.mod`.
2. 保存项目的预览图像作为 `$Work\preview.png`: PNG 或 JPG低于 1 MB,平方(512 x 512 像素工作)。

<a id="4-write-the-vdf"></a>
## 4. 写下 The VDF

steamcmd 创建该项目从 A VDF 文件:文本文件 `"key" "value"` 双子座内 A `"workshopitem"` 区块。 这个命令写着 `$Work\create.vdf` 走过的路径。 编辑 `title` 和 `description` 首先是:

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

| 关键 | 價值 |
| --- | --- |
| `appid` | `233860`, KenshiSteam 应用 ID。 |
| `publishedfileid` | `0` 创建一个新的项目。 任何其他数字更新,即现有项目。 |
| `contentfolder` | 完整的路径到文件夹,其文件变成了项目的内容:从 ZIP不是他的父母。 |
| `previewfile` | 完整的路径到预览图像。 |
| `visibility` | `0` 公共, `1` 只有朋友, `2` 个人, `3` 未列 开始与 `2` 并在项目页面上更改它。 |
| `title` | 作品名称在研讨会上 开始它与 `[Meitou]`正如其他Meitou代码模式一样。 |
| `description` | 一个字符串。 然后在项目页面上写下完整的描述(它支持 Steam 的 BBCode). |
| `changenote` | 第一個項目的變更筆記頁面。 |

白色价值观:

- 以前列的路径写作(`C:/Users/me/Downloads/...`). 要有双重(`C:\\Users\\me\\...`).
- 一个值中的双重引用是写的 `\"`.
- 保持文件 ASCII.

<a id="5-create-the-item"></a>
## 5. 创建项目

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` 是您登录到 Steam 的名称,如安装者所提供的. steamcmd 使用保存登录。 如果您需要密码或 Steam Guard 代码,请运行 `powershell -ExecutionPolicy Bypass -File $Watcher login`然后再次执行命令。

跑步结束了 `Success.`. 打开 Steam > 您的个人资料 > 工作坊项目,打开新项目,然后复制 `?id=` 在他的 URL此分類上一篇: Item ID

<a id="6-finish-the-items-page"></a>
## 6. 關閉項目頁面

在 Steam 中的项目页面上:

1. 接受 Steam 工作室的法律协议,如果页面显示了旗帜。 直到你做到,只有你可以看到项目。
2. 写下完整的描述。
3. 如果 mod 需要另一个 Workshops 模式(例如 Rivalry Core),则将其添加到 **Add/Remove 需要的物品**. 订阅者是
她也被要求订阅。 与依赖性相同 `meitou.json` `dependencies`发射器的顺序将其首先放在负载顺序中。
4. 更改可见性 **公共** 当页面准备好时

<a id="7-watch-the-item"></a>
## 7. 观看项目

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

干跑报告现行发布,如已上传。 取消 `$Work` 一旦它。

<a id="each-release-after-that"></a>
## 每次释放后,

放下模式,如在步骤1中。 下一篇: 守护者的下一个计划下载 ZIP检查它的 SHA第256章 把它加到标题上,把它当成变更笔记 它永远不会改变项目的标题,描述,预览或可见性:改变项目的页面。 `powershell -ExecutionPolicy Bypass -File $Watcher status` 显示时间表,下一个运行和最后一个上传。
