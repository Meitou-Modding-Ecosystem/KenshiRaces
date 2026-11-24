<a id="build-and-run-locally"></a>
# ローカルでビルドして起動する

`Play`をビルドすると、エンジンとリポジトリ内の全MODがコンパイルされます。次に、MODと`meitou.lock.json`に記載されたフレームワークをゲームのMODフォルダーへコピーしてゲームを起動します。Visual Studio、VS Code、ターミナルのいずれでも同じビルドが実行されます。

.NET 11 SDKをインストールし、`git clone --recurse-submodules <repo URL>`でサブモジュールを含めてクローンしてください。既存のクローンでは`git submodule update --init --recursive`を実行します。サブモジュールがないと、ビルドは`MEITOU001`で停止し、必要なコマンドを表示します。

<a id="visual-studio"></a>
## Visual Studio

1. リポジトリの `.slnx` を開きます。
2. Visual Studio が選択しなかった場合は、 `Play` をスタートアップ プロジェクトとして設定します (右クリック > [スタートアップ プロジェクトとして設定])。
3. [スタート] ボタンの横にあるドロップダウンでプロファイルを選択し、F5 (デバッグ) または Ctrl+F5 を押します。

| プロフィール | 走る | 用途: |
| --- | --- | --- |
| `Play (engine + mods)` | このリポジトリの `engine` サブモジュールからビルドされた `MeitouClient.exe` | MOD のテストとデバッグ。改造コードのブレークポイントが機能する |
| `Meitou launcher (installed)` | Meitou リリース、 `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` からインストールされたランチャー | MOD リストまたはロード順序を変更し、新しい MOD のコードを信頼する |

<a id="vs-code"></a>
## VS Code

1. C# Dev Kit 拡張機能をインストールします (フォルダーを開くと VS Code が提供します。これは `.vscode/extensions.json` にあります)。
2. リポジトリのルート フォルダーを開きます ([ファイル] > [フォルダーを開く])。
3. 実行してデバッグ (Ctrl+Shift+D) し、**Play (engine + mods)**を選択して、F5 を押します。最初にビルドとデプロイが行われ、次にデバッガーでエンジンが開始されます。改造コードのブレークポイントは機能します。
4. インストールされているランチャーの場合: [ターミナル] > [タスクの実行] > **Meitou launcher (installed)**。ビルドしてデプロイし、ランチャーを開きます。

[ターミナル] > [ビルド タスクの実行] (Ctrl+Shift+B) はビルドとデプロイのみを行います。

<a id="without-an-editor"></a>
## エディターなしで

[ `tools\Build.ps1` ] を右クリックし、[**PowerShell で実行**] をクリックします。 `dotnet build` のようにビルドしてデプロイし (サブモジュールが欠落している場合は最初にサブモジュールをチェックアウトします)、結果を読めるようにウィンドウを開いたままにします。 `-Test` もテストを実行します。  次に、 `-Run` がエンジンを始動します。

<a id="terminal"></a>
## ターミナル

リポジトリのルート フォルダー内:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` と `dotnet test` は、すべてのプロジェクトをビルドしてテストします。
- `dotnet run --project Play` は、エンジン (「Play (エンジン + MOD)」プロファイル) をビルド、デプロイ、および開始します。
- 代わりにインストールされているランチャーを使用するには、エンジンを起動せずにデプロイし、ランチャーを開きます。

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## プレイビルドの機能

- MOD を検索します: `meitou.json` および `<Folder>/<Folder>.csproj` を含むすべての最上位フォルダー。新しい MOD では `Play` を変更する必要はありません。
- 選択した構成に関係なく、**リリース**でエンジンと各 MOD をビルドします。MOD がキャラクターのタスクを開始すると、デバッグ エンジン ビルドは `TaskStart.CheckStarts` をスローします。改造コード内のブレークポイントは引き続き機能します (リリース シンボルが展開されます)。
- 各 MOD の `<Mod>.dll` 、 `<Mod>.pdb` 、 `meitou.json` を `<Kenshi>\mods\<Mod>\` にコピーします。まずゲームを閉じます。実行中のゲームにより DLL がロックされます。
- `meitou.lock.json` の各フレームワークを、ダウンロードしたリリース フォルダーから `<Kenshi>\mods\<Framework>\` にコピーします。または、クローンからビルドされたフレームワーク ([local-development.md](local-development.md))] の場合は、そのビルド出力をコピーします。
- データ ジェネレーターを備えた MOD (`<Mod>.Data/<Mod>.Data.csproj` 、 `<generator> <Kenshi folder> <output .mod>` として実行) は、インストールされたゲームから生成された `<Mod>.mod` を取得します。失敗はビルド警告であり、MOD は警告なしでデプロイされます。他のすべての MOD には空の `<Mod>.mod` がありません。既存のものは決して置き換えられません。
- `Structures` フォルダーを持つ MOD は、その `.glb` モデルをそのデプロイされたフォルダーにインポートし、レコードはその `.mod` ([ Structures.md](structures.md))] にマージされます。
- mods フォルダーが存在しない場合、何もコピーされず、ビルドにその旨が表示されます。
- ランチャーの [再生] ボタンは、このリポジトリのエンジンではなく、インストールされているリリースを開始します。それを閉じて、「Play (engine + mods)」を実行してビルドをテストします。

<a id="settings"></a>
## 設定

デフォルト: ゲーム `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` 、mods フォルダー `<game>\mods` 。 `KENSHI_PATH` 環境変数を使用してこれらを変更するか (エンジンも環境変数を読み取るため、両方が一致します)、またはリポジトリのルート フォルダー内の git 無視された `Meitou.user.props` 内のマシンごとに変更します。

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

エンジンは独自にゲームを見つけます (Steam)。 `Meitou.user.props` にのみ設定された `KenshiDir` は渡されません。 VS Code の起動構成はリリース エンジンを実行します。 `PlayConfiguration` を変更する場合は、 `.vscode/launch.json` をローカルで編集します。

<a id="first-run"></a>
## 最初の実行

- インストールしたランチャーを 1 回実行します (上記のいずれかの方法)。MOD とそのフレームワークを有効にし、そのコードを信頼します。
- コピーが「アクセス拒否」で失敗した場合は、エディタを管理者として実行するのではなく、 `Kenshi\mods` に対する変更権限をユーザーに付与します ([プロパティ] > [セキュリティ])。

[English](../../../docs/play.md) · [Kenshi Races](../README.md)
