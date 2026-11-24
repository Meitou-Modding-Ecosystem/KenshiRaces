<a id="build-and-run-locally"></a>
# 地元で構築・実行

建物 `Play` エンジンを構築し、このリポのすべてのモードをコピーし、モードとフレームをコピーします。 `meitou.lock.json` ゲームのモードフォルダーに名前を入れて、ゲームが始まります。 好きなツールを使用する:Visual Studio、VSコード、または同じ構造を実行します。

3人とも必要です。NET 11 SDK () (し)`git clone --recurse-submodules <repo URL>`あるいは `git submodule update --init --recursive` 既存のクローンで。 建物がなくなったので、 `MEITOU001`その命令を呼ぶ。

<a id="visual-studio"></a>
## ビジュアルスタジオ

1. リポのオープン `.slnx`.
2. セット `Play` プログラミング プロジェクト で は あり た 場合 で あり ませ ん で でしょ う (右クリック > スタートアップ プロジェクションとして設定)
3. スタートボタンの隣のダウンロードボタンでプロフィールを選択し、F5(デボグ)またはCtrl+F5を押します。

| プロフィール | レース | 使用するために |
| --- | --- | --- |
| ゲーム(エンジン + モード) | `MeitouClient.exe` このレポの建て方 `engine` サブモジュール | テストおよびモードのデビュー; モードコードのパラスポイント |
| Meitou Launcher(インストール) | メイツからインストールされたリリースは、 `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | モードリストまたはロードオーダーを変更し、新しいモードコードを信頼する |

<a id="vs-code"></a>
## コード対コード

1. C# Dev Kit 拡張子をインストールする (VS コードがそれを提供すると、フォルダーを開きます。 `.vscode/extensions.json`).
2. リポのルートフォルダーを開く(ファイル > オープンフォルダー)
3. 実行とデブグ(Ctrl+Shift+D) **ゲーム(エンジン + モード)**F5 プレス 最初に起動し、エンジンを起動させる。
デブギャー:モードコード作業における分断点
4. インストールされた起動器:ターミナル > Run Task > **Meitou Launcher(インストール)**. それは構築し、配置し、その後、ラウンチャーを開きます。

ターミナル > Run Build タスク (Ctrl+Shift+B) は構築およびリリースのみです。

<a id="without-an-editor"></a>
## 編集者なし

右クリック `tools\Build.ps1` > **・PowerShellで実行**. 作り出し、作り出したように `dotnet build` (サブモジュールを最初にチェックすると、それらが欠けている) ウィンドウを開いて、結果を読むことができます。 `-Test` テストも実施しています。 `-Run` その後、エンジンが始まる。

<a id="terminal"></a>
## ターミナル

レポのルートフォルダー:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` そして、 `dotnet test` すべてのプロジェクトを構築し、テストする。
- `dotnet run --project Play` エンジンを構築し、起動し、「Play(エンジン+モード)」プロフィールを開始します。
- 代わりにインストールされたラウンサーを使用するには、エンジンを起動せずに、ラウンサーを開きます。

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## ゲーム構築とは

- モードを見つける:各トップレベルのフォルダーを保有する `meitou.json` そして、 `<Folder>/<Folder>.csproj`. 新しいモードは変更は必要ありません。 `Play`.
- エンジンを構築し、それぞれのモード **リリース**どんな設定でも選んだ: デビューエンジンが構築する 投げ込み `TaskStart.CheckStarts`
キャラクターの仕事が始まるとき。 モードコードのスケッチがまだ動作します(リリースシンボルが配置されています)。
- 各モードのコピー `<Mod>.dll`, `<Mod>.pdb` そして、 `meitou.json` へへへへ `<Kenshi>\mods\<Mod>\`. ゲームを閉じる:実行ゲームがDLLを閉じます。
- 各フレームワークをコピーする `meitou.lock.json` ダウンロードされたリリースフォルダーから、 `<Kenshi>\mods\<Framework>\`あるいは、構築された枠組み
クローン([local-development.md](local-development.md)) 生産を構築する。
- データ発電機(データ生成機)`<Mod>.Data/<Mod>.Data.csproj`走るように `<generator> <Kenshi folder> <output .mod>`(得られる) `<Mod>.mod`
インストールされたゲームから生成され、失敗は構築警告であり、モードはそれなしで実行されます。 すべてのモードが空っぽになります。 `<Mod>.mod` 既存のものは、決して置き換えられません。
- モードとA `Structures` フォルダーが得られる `.glb` 入力されたフォルダーに輸入されたモデル、記録を組み合わせた。 `.mod`
([structures.md](structures.md)).
- モードフォルダーが存在しない場合、何もコピーされず、構築はそう言います。
- ラウンチャーのプレイボタンは、このレポのエンジンではなく、インストールされたリリースを開始します。 閉じて「Play(エンジン + モード)」を実行して、
建設する

<a id="settings"></a>
## 設定

欠点:ゲーム `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`モードフォルダー `<game>\mods`. 変えようと、The `KENSHI_PATH` 環境変数(エンジンも読むので、両方とも同意する) `Meitou.user.props` レポのルートフォルダー:

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

エンジンはゲームを自分で見つけます(Steam)。 `KenshiDir` 単に設定する `Meitou.user.props` それに渡されていない。 VS コードのリリース構成はリリースエンジンを実行します; ↓ ↓ `.vscode/launch.json` 地元で変化する場合 `PlayConfiguration`.

<a id="first-run"></a>
## 最初の走り

- インストールされたラウンサーを一度実行する(上記の方法のいずれか):モードとそのフレームワークを有効にし、コードを信頼します。
- コピーが「アクセス拒否」で失敗した場合、ユーザーに変更権を与える `Kenshi\mods` (セキュリティ > セキュリティ) 代わりにあなたの
編集者は管理者。
