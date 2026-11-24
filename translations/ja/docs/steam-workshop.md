<a id="publishing-to-the-steam-workshop"></a>
# Steam ワークショップに公開する

MOD のワークショップ アイテムは、リリース ZIP から手動で 1 回作成します。その後、[Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) が Windows PC 上で実行され、新しい GitHub リリースがスケジュールに従ってそのアイテムにアップロードされます。すべての手順は、アイテムを所有する Steam アカウントにサインインした PC 上の **Windows PowerShell** で実行されます。

コマンドではこれらの値が使用されます。 Mod のフォルダー名 ( `release.yml`  `mod-folder` など) とその最初のリリース バージョンを使用して、PowerShell ウィンドウごとに 1 回設定します。

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1.MODをリリースします

1. `beta` を `main` にマージし、次に `main` から PR を開いて `release` にマージします ([ブランチ](../README.md#branches) ])。
2. `release` のリリース ワークフローを待ちます。マージ `<slug>-v<version>` に `meitou.json` のバージョン (たとえば、 `example-mod-v0.1.0` ) をタグ付けし、 `<Mod>-<version>.zip` および `<Mod>-<version>.zip.sha256` でリリースを作成します。タグ付けする前にその実行が失敗した場合、 `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` はタグを手動でプッシュします ( `-DryRun` はコマンドを出力するだけです)。

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Workshop Watcherをインストールし、Steamにサインインします。

[Workshop Watcher ガイドの](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) をインストールします。インストーラーは steamcmd を `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` にダウンロードし、Steam アカウントにサインインします。 `Add a mod to watch now?` と尋ねられたら、 `n` と答えます: アイテムはまだ存在しません。 Workshop Watcher がすでにインストールされている場合は、この手順をスキップしてください。

<a id="3-prepare-the-items-files"></a>
## 3. アイテムのファイルを準備する

1. リリース ZIP をダウンロードし、 `$Work` に解凍します。

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` は、 `$Mod.dll` 、 `meitou.json` 、および `$Mod.mod` を保持するようになりました。
2. 項目のプレビュー画像を `$Work\preview.png` : PNG または JPG 、 1 MB の下、正方形 (512 x 512 ピクセルで機能します) として保存します。

<a id="4-write-the-vdf"></a>
## 4. VDF を書き込みます

steamcmd は、 VDF ファイル ( `"workshopitem"` ブロック内の `"key" "value"` ペアのテキスト ファイル) からアイテムを作成します。このコマンドは、パスが入力された `$Work\create.vdf` を書き込みます。最初に `title` と `description` を編集します。

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

| 鍵 | 価値 |
| --- | --- |
| `appid` | `233860` 、 Kenshi の Steam アプリ ID 。 |
| `publishedfileid` | `0` は新しいアイテムを作成します。他の番号を指定すると、その既存の項目が更新されます。 |
| `contentfolder` | ファイルがアイテムのコンテンツとなるフォルダーへのフルパス: ZIP の mod フォルダーであり、その親ではありません。 |
| `previewfile` | プレビュー画像へのフルパス。 |
| `visibility` | `0` 公開、 `1` 友達のみ、 `2` 非公開、 `3` 非公開。 `2` から始めて、アイテムのページで変更します。 |
| `title` | ワークショップ上のアイテムの名前。他の Meitou コード MOD と同様に、 `[Meitou]` で開始します。 |
| `description` | 1 行のテキスト。その後、アイテムのページに完全な説明を書きます (Steam の BBCode をサポートしています)。 |
| `changenote` | アイテムの「変更ノート」タブの最初のエントリ。 |

値の規則:

- パスはスラッシュ ( `C:/Users/me/Downloads/...` ) で記述します。バックスラッシュは 2 つ付ける必要があります ( `C:\\Users\\me\\...` )。
- 値内の二重引用符は `\"` と書かれます。
- ファイル ASCII を保持します。

<a id="5-create-the-item"></a>
## 5. アイテムを作成する

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` は、インストーラーに与えられた、Steam へのサインインに使用する名前です。 steamcmd は保存されたログインを使用します。パスワードまたは Steam ガード コードを要求された場合は、 `powershell -ExecutionPolicy Bypass -File $Watcher login` を実行してから、コマンドを再度実行します。

実行は `Success.` で終了します。 Steam > プロフィール > ワークショップ アイテムを開き、新しいアイテムを開き、その URL の `?id=` の後の番号をコピーします。つまり、アイテム ID です。

<a id="6-finish-the-items-page"></a>
## 6. アイテムのページを終了します

Steam のアイテムページ:

1. ページにバナーが表示されている場合は、Steam ワークショップの法的契約に同意します。そうするまでは、そのアイテムはあなただけが見ることができます。
2. 完全な説明を書きます。
3. MOD に別のワークショップ MOD (Rivalry Core など) が必要な場合は、**必要なアイテムの追加/削除** の下に追加します。その後、購読者も購読するように求められます。 `meitou.json`  `dependencies` の依存関係により、ランチャーのアレンジによりロード順序の最初に配置されます。
4. ページの準備ができたら、公開設定を **パブリック** に変更します。

<a id="7-watch-the-item"></a>
## 7. アイテムを見る

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

ドライランでは、現在のリリースがすでにアップロードされていると報告されます。 `$Work` が削除されたら、削除します。

<a id="each-release-after-that"></a>
## その後のリリースごとに

ステップ 1 と同様に MOD をリリースします。ウォッチャーの次回のスケジュールされた実行では、 ZIP をダウンロードし、その SHA -256 をチェックして、リリース タイトルとリンクを変更ノートとしてアイテムにアップロードします。アイテムのタイトル、説明、プレビュー、表示設定は変更されません。アイテムのページで変更してください。 `powershell -ExecutionPolicy Bypass -File $Watcher status` には、スケジュール、次回の実行、最後のアップロードが表示されます。
