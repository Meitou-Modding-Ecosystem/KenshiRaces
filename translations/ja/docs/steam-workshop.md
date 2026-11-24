<a id="publishing-to-the-steam-workshop"></a>
# スティーブ・ワークショップ

あなたは、モードのワークショップのアイテムを一度、手元に、リリースから作成します。 ZIP. その後、 [ワークショップウォッチャー](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) Windows PC で実行され、新しい GitHub リリースがスケジュールでその項目にアップロードされます。 すべてのステップは、 **Windows パワーシェル** あなたのPCでは、アイテムを所有するSteamアカウントにサインインします。

コマンドはこれらの値を使用します。 PowerShell ウィンドウで 1 回設定し、モードのフォルダー名( `release.yml` `mod-folder`初版版:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. モードをリリース

1. 合併 `beta` 入り口 `main`開いてPRを組み合わせる。 `main` 入り口 `release` ([部門](../README.md#branches)).
2. リリースワークフローを待つ `release`. 合併を表す `<slug>-v<version>` バージョンは、 `meitou.json` (例えば、
`example-mod-v0.1.0`解放を創り出し、 `<Mod>-<version>.zip` そして、 `<Mod>-<version>.zip.sha256`. たとえば、前回の試合で失敗した場合、 `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` 手でタグを押す(`-DryRun` コマンドを押すだけです。

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. ワークショップウォッチャーをインストールしてSteamにサインイン

フォロー [インストール](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) ワークショップウォッチャーガイド インストラクターがSteamCMDをダウンロード `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` あなたのSteamアカウントにサインインします。 尋ねるとき `Add a mod to watch now?`答えは `n`物件はまだ存在しない。 ワークショップウォッチャーがすでにインストールされている場合、このステップを中止します。

<a id="3-prepare-the-items-files"></a>
## 3. アイテムのファイルの準備

1. リリースダウンロード ZIP そして、それを包み込む。 `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` 今は持っている `$Mod.dll`, `meitou.json` そして、 `$Mod.mod`.
2. アイテムのプレビュー画像を保存する `$Work\preview.png`: PNG あるいは JPG1MB未満、平方メートル(512×512ピクセル)

<a id="4-write-the-vdf"></a>
## 4. 書く → VDF

steamcmd は、アイテムを a から作成します。 VDF ファイル: テキストファイル `"key" "value"` 内側のカップルA `"workshopitem"` ブロック この命令は、 `$Work\create.vdf` 満たされた道のり 編集 `title` そして、 `description` まず:

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

| キー | 価値 |
| --- | --- |
| `appid` | `233860`, KenshiSteam アプリ ID |
| `publishedfileid` | `0` 新しいアイテムを作る。 既存の項目を更新する他の番号 |
| `contentfolder` | ファイルがアイテムのコンテンツになるフォルダーへの完全なパスワード:モードフォルダーから ZIP親ではない。 |
| `previewfile` | プレビュー画像への完全な道 |
| `visibility` | `0` 公衆、 `1` 友人だけ、 `2` プライベート、 `3` 未分類 まずは、 `2` 項目のページで変更します。 |
| `title` | ワークショップの名称です。 始まるのは、 `[Meitou]`他のMeitouコードモードと同じように。 |
| `description` | テキストの一行。 次のページに詳細を書き、その後(Steam のサポート) BBCode). |
| `changenote` | 項目の変更のタブの最初の入力です。 |

価値観のルール:

- は  で は あり ます が(`C:/Users/me/Downloads/...`). バックグラスはダブル(`C:\\Users\\me\\...`).
- 値段内の二重引用が書かれています。 `\"`.
- ファイルを保存 ASCII.

<a id="5-create-the-item"></a>
## 5. アイテムを作る

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` Steam にサインインしている名前は、インストラクターに与えられたように、Steamcmd は保存されたログインを使用しています。 パスワードまたは Steam Guard コードを要求する場合は、実行してください。 `powershell -ExecutionPolicy Bypass -File $Watcher login`そして、再びコマンドを実行します。

レースは終わり `Success.`. スティーブ > プロフィール > ワークショップ アイテムを開き、次に番号をコピーします。 `?id=` 彼女の URLこれがアイデンティティです。

<a id="6-finish-the-items-page"></a>
## 6. 記事のページを終了

Steam の記事のページ:

1. Steam Workshop の法的契約を受け入れ、ページにバナーが表示されます。 あなたがそれを行うまで、あなただけがアイテムを見ることができます。
2. 完全な説明を書きます。
3. 他のワークショップモード(例えば、Rivalry Core)が必要になります。 **Add/Remove 必要な物件**. サブスクリプションは、
こちらもサブスクリプションをお願いします。 依存性は、 `meitou.json` `dependencies`ラウンチャーの順序は、最初にロードの順序に置かれます。
4. 見通しを変更する **公務員** ページが完成したとき。

<a id="7-watch-the-item"></a>
## 7. 項目を見る

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

乾燥は、すでにアップロードされている現在のリリースを報告します。 削除 `$Work` 一度やったら。

<a id="each-release-after-that"></a>
## その後、それぞれの解放

ステップ1 に記載されているようにモードをリリースします。 次回のウォーチャーの予定通りダウンロード ZIPそのチェック SHA-256 と エピソード に アップ し て 、 リリース タイトル と リンク を 変更 ノート と し ます。 アイテムのタイトル、説明、見通しを決して変更する:アイテムページの変更。 `powershell -ExecutionPolicy Bypass -File $Watcher status` スケジュール、次のラウンド、そして最後のアップロードを表示します。
