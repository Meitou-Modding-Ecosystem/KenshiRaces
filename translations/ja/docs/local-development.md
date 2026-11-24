<a id="local-development-setup"></a>
# 地元開発の仕組み

あなたが働いているリポのみを一つのフォルダーにクローンします。 各自で構築する。 `dotnet build`ダウンロードする必要があるフレームワークとハッシュチェック自体

<a id="repos"></a>
## リラックス

各モードのリポは、モードに従って名付けられています。

| リポー | ハンドル | PRは、 |
| --- | --- | --- |
| MeitouClient | エンジンとモードハウス | `fork` |
| リバリーコア | リバリー・コア・フレームワークとそのエージェント・スキル | `beta` |
| ロジスティクスコア | ロジスティック・コア・フレームワーク | `beta` |
| 戦略 | ・戦争・オーケストラーション・フレームワーク(ロジスティクス・コアを使用) | `beta` |
| バストフロント | () (し)  ()  (せ)  し (か) | `beta` |
| アシュンハンド | ( ) ( )の() | `beta` |
| 夫婦 | 夫婦 | `beta` |
| スカウト | SquadArena モードとチーム評価ツール | `beta` |
| グローバル・グローブ(TheIvoryDisaster) | パッケージ、リリースゲート、フレームロックターゲット | `beta` |

<a id="folders"></a>
## フォルダー

このステップは、8つのフォルダーを使用します。 各新しい PowerShell ウィンドウで変数として設定し、以下のコマンドを貼り付けます。

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

- .NET 11 SDK (すべてのレポー) `global.json` バージョンの名前)とGit。
- 編集者:Visual Studio、VSコード、C# Dev Kit、またはRider。 ターミナルだけが動作します。
- Kenshi 設置。 もし、その中にないなら、 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`設定する → set the `KENSHI_PATH` 環境変動
フォルダーに

<a id="clone"></a>
## クローン

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

すべてのモード・レポには、それぞれの `engine` サブモジュールは、 `$Engine` エンジンハウスを書くだけです。

<a id="build-and-play-one-mod"></a>
## 作成して1つのモードをプレイ

**モードのリポに変更する:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- 最初の構築は、それぞれのフレームをダウンロードします。 `meitou.lock.json` 入り口 `deps` 止まるならば、その SHA256は合わない。
- `dotnet run --project Play` 閉鎖されたフレームワークとモードをコピーする `Kenshi\mods` エンジンが始まる。 ビデオ・スタジオ VS コード
インストールしたラウンサー: [play.md](play.md).
- ラウンサーのモードの上のフレームを可能にし、いかなる依存性にも適用されます。

<a id="change-a-framework-and-a-mod-together"></a>
## フレームワークとモードを組み合わせる

フレームワーククローンにモードレポを指定する:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- 建てられた `$Ashen` リバリー・コアをコピーする `$Rivalry` エンジンを使用して、 `$Rivalry\engine`リバリー・コアが作るゲームをプレイする。
- `Meitou.user.props` 無視されている。 もし、あなたがすでにあなたの Kenshi 設定、内側に2つのラインを追加 `PropertyGroup` 代わりに
- `MeitouFrameworkSources` プロジェクトの枠組みを分離して、 `;`☆☆☆☆ `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` フレームクローンが構築するエンジンの名前。
- 線を削除して閉鎖に戻る ZIP.

**フレームワークチェーン** フロントのバスト `meitou.lock.json` 戦争オーケストラとロジスティクスコア 両方のフレームワークとバストフロントを組み合わせるには、バスト・フロントの両方を組み合わせる。

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

逆に、オーケストラは戦争に反対する。 `$Logistics` しかも、全体が一つのエンジンを使用している。 戦争オーケストラーションだけを超え、バストフロントのロックで名付けられたロジスティクス・コアに反対する。

送信するには: フレームワーク PR を組み合わせる `beta` ☆☆☆☆ `$Rivalry`予告書を掲載する `rivalry-core-<version>-beta.<commit>`. その後、内 `$Ashen`セット `meitou.lock.json` その資産とその資産は、 SHA-256とエンジンは、リリースノートから取り出し、モードPRを開きます。 チェーンでは、トップダウン: ロジスティック・コア、その後、戦争オーケストラはそれを閉じ、次にバスト・フロントは両方を閉じます。 フレームの生産を解放した後、ロックを移動させる `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## エンジンとモードを組み合わせる

モードレポのサブモジュールの内側のエンジン分野をチェック:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- 新しいエンジン指標を採用しないでください:エンジンはアップストリームのタグに移動しますので、変更はアップストリートの後にこのリポに到達します。
リリースする。
- フレームを使用するモードには、そのエンジンに再構築されたフレームも必要です。 上のフレームワークを超え、同じようにチェックする。
エンジンは、 `$Rivalry\engine`.
- エンジン・PR 入り口 エンジンを入り口にする `fork`アップストリームがリリースされた後、フレームワークPRが動く。 `$Rivalry\engine` その解放、その後、
モードPRはロックを動かし、 `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## エンジンを切り替え、試す。

最新のアップストリーム、リリース、またはエンジン部門をプレイするには、 `$Engine` スクリプトをリポモードでクローンする:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`■最新のコミットメント VlcekM/MeitouClient `master` (脚本が追加する) `upstream` 欠席時に遠くへ。
- `-To Release`最新のアップストリームリリース `vX.Y.Z`.
- `-To Fork`オーガ・フォークの `fork` 枝、引っ張る。
- `-Branch <name>`: エンジンPRの分野のようなオーガンフォークの分野。
- なしで `-Path` 使っているのは、 `MeitouClient` モード・レポの隣にあるフォルダー。 `-Run` ゲームをリリースし、スタートする。 `-Build` しかも、
建設する。
- Upstream、リリース、そして `-Branch` クローンを離れたコマンドに置き去りにし、地元の `master` そして、 `fork` 部門は変わらない。 脚本は
クローンが未定の変化を起こしたときに、何も変えずに止まる。
- 決してモード・レポに触れることはありません。 `engine` サブモジュール(これは `Update-Local.ps1` そして、ピン。

<a id="keep-clones-current"></a>
## クローンが流れ

毎回引っ張った後、エンジンタイプが欠けている場合(たとえば、 `The type or namespace name 'RelationCause' could not be found`)、Visual Studioを閉じて、モードのリポでアップデートスクリプトを実行します。

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- スイッチ に `beta` 引き出し、更新。 `engine` そして、 `build/gutenstructure` コミットメントについて `beta` ピンク(`git pull` ひとりで
古いものに置いておく。 `-Branch <name>` もう一つの枝を引っ張る。
- それはそれぞれのサブモジュールをピンに対してチェックし、エンジンバージョンを印刷します。
- 国旗 `MeitouEngineDir` そして、 `MeitouFrameworkSources` で、 `Meitou.user.props` あるいは環境:彼らは建物の使用を別の
repo のエンジンまたはフレームワークは、この repo の ピン の代わりに。 フレームワークとモードを組み合わせて変更しない限り、削除します。
- `-Clean` 無視されたギットを削除 `bin` そして、 `obj` フォルダーは、古いエンジンの構築は再利用されません。 `-Build` レース `dotnet build`.
- 結局は、 `Up to date` あるいは、標識されたラインのリスト `FIX`.

次の構築は、再びフレームワークをダウンロードするだけで、 `meitou.lock.json` 変更しました。
