<a id="local-development-setup"></a>
# ローカルリポジトリのセットアップと更新

作業するリポジトリのみを 1 つのフォルダーにクローン作成します。それぞれは `dotnet build` を使用して独自に構築され、必要なフレームワークはダウンロードしてハッシュ チェック自体を行います。

<a id="repos"></a>
## リポジトリ

各 MOD リポジトリには、その MOD にちなんで名前が付けられます。

| リポ | ホールド | PR は次の場所に移動します |
| --- | --- | --- |
| MeitouClient | エンジンとそのMODフック | `fork` |
| ライバルコア | Rivalry Core フレームワークとそのエージェント スキル | `beta` |
| 物流コア | 物流コアフレームワーク | `beta` |
| 戦争組織化 | War Orchestration フレームワーク (Logistics Core を使用) | `beta` |
| バストフロント | バストフロントの近接攻撃（戦争オーケストレーションと兵站コアを使用） | `beta` |
| アシェンハンド | アシェンハンド（ライバルコア使用） | `beta` |
| 畜産業 | 畜産業 | `beta` |
| 分隊アリーナ | SquadArena MOD と分隊評価ツール | `beta` |
| GutenStructure (象牙災害) | 共有 CI 、パッケージ化、リリース ゲート、フレームワーク ロック ターゲット | `beta` |

<a id="folders"></a>
## フォルダー

これらの手順では 8 つのフォルダーを使用します。新しい PowerShell ウィンドウごとにこれらを変数として設定し、以下のコマンドをそのまま貼り付けます。

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
## 前提条件

- 。 NET 11 SDK (各リポジトリの `global.json` にバージョン名が付けられます) と Git。
- エディタ: C# 開発キットを備えた Visual Studio 、 VS Code 、または Rider。端末単体でも動作します。
- Kenshi を取り付けました。 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` にない場合は、 `KENSHI_PATH` 環境変数をそのフォルダーに設定します。

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

すべての MOD リポジトリには独自の `engine` サブモジュールがあるため、 `$Engine` はエンジン フックを記述する場合にのみ必要です。

<a id="build-and-play-one-mod"></a>
## 1 つの MOD を構築してプレイする

**MOD のリポジトリに変更します:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- 最初のビルドは、 `meitou.lock.json` で指定された各フレームワークを `deps` にダウンロードし、その SHA -256 が一致しない場合は停止します。
- `dotnet run --project Play` は、ロックされたフレームワークと MOD を `Kenshi\mods` にコピーし、エンジンを開始します。 Visual Studio 、 VS Code およびインストールされているランチャー: [play.md](play.md).
- 依存関係については、ランチャーの MOD の上にあるフレームワークを有効にします。

<a id="change-a-framework-and-a-mod-together"></a>
## フレームワークとMODを一緒に変更する

MOD リポジトリをフレームワーク クローンに指定します。

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- `$Ashen` のビルドは `$Rivalry` の Rivalry Core をコンパイルし、 `$Rivalry\engine` のエンジンを使用するようになり、Play はその Rivalry Core ビルドをデプロイします。
- `Meitou.user.props` は git 無視されます。すでに Kenshi 設定が保持されている場合は、代わりに `PropertyGroup` 内に 2 行を追加します。
- `MeitouFrameworkSources` は、 `;` で区切られたフレームワーク プロジェクト パスを受け取ります。 `$Logistics\LogisticsCore\LogisticsCore.csproj` 。 `MeitouEngineDir` は、フレームワーク クローンが構築されるエンジンに名前を付けます。
- 行を削除して、ロックされた ZIP に戻ります。

**フレームワーク チェーン。**Bast Front の `meitou.lock.json` には、War Orchestration と Logistics Core がリストされています。フレームワークと Bast Front の両方を一緒に変更するには、Bast Front で両方をオーバーライドします。

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

オーバーライドは受け継がれるため、War Orchestration は `$Logistics` に対してもビルドされ、ビルド全体で 1 つのエンジンが使用されます。 War Orchestration のみをオーバーライドすると、Bast Front のロックで指定された Logistics Core に対して構築されます。

出荷するには、フレームワーク PR を `$Rivalry` の `beta` にマージし、プレリリース `rivalry-core-<version>-beta.<commit>` を公開します。次に、 `$Ashen` で、 `meitou.lock.json` をそのタグとアセット、リリース ノートの SHA -256 およびエンジン コミットに設定し、mod PR を開きます。チェーンでは、ロジスティック コア、次にそれをロックする War Orchestration ビルド、次に両方をロックするバスト フロントというトップダウンで出荷します。フレームワークの製品リリース後、ロックを `rivalry-core-v<version>` に移動します。

<a id="change-the-engine-and-a-mod-together"></a>
## エンジンとMODを一緒に変更する

MOD リポジトリのサブモジュール内のエンジン ブランチを確認してください。

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- 新しいエンジン ポインターをコミットしないでください。エンジン ピンは上流のリリース タグにのみ移動するため、上流でリリースされると変更はこのリポジトリに届きます。
- フレームワークを使用する MOD には、そのエンジン上で再構築されたフレームワークも必要です。上記のフレームワーク オーバーライドを使用して、 `$Rivalry\engine` の同じエンジン ブランチをチェックアウトします。
- 着陸の順序: PR を `fork` にエンジンします。アップストリームがリリースした後、フレームワーク PR が `$Rivalry\engine` をそのリリースに移動し、次にモッド PR がロックを移動し、 `engine` を移動します。

<a id="switch-the-engine-to-play-or-test"></a>
## エンジンを切り替えて再生またはテストします

最新のアップストリーム、リリース、またはエンジン ブランチを再生するには、任意の MOD リポジトリのスクリプトを使用して `$Engine` クローンを切り替えます。

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : VlcekM/ MeitouClient  `master` の最新のコミット (不足している場合、スクリプトは `upstream` リモートを追加します)。
- `-To Release` : 最新のアップストリーム リリース タグ `vX.Y.Z` 。
- `-To Fork` : 組織フォークの `fork` ブランチ、プルされました。
- `-Branch <name>` : エンジン PR のブランチなど、組織フォークのブランチ。
- `-Path` を使用しないと、MOD リポジトリの隣にある `MeitouClient` フォルダーが使用されます。 `-Run` は、Release でゲームをビルドして開始します。 `-Build` はビルドするだけです。
- アップストリーム、リリース、および `-Branch` は、切り離されたコミットにクローンを残します。ローカルの `master` および `fork` ブランチは変更されません。クローンにコミットされていない変更がある場合、スクリプトは何も変更せずに停止します。
- MOD リポジトリ自体の `engine` サブモジュール (つまり、 `Update-Local.ps1` とピン) には決して触れません。

<a id="keep-clones-current"></a>
## クローンを最新の状態に保つ

プルするたびに、またエンジン タイプが欠落している (例: `The type or namespace name 'RelationCause' could not be found`) ためにビルドが失敗するたびに、 Visual Studio を閉じて、MOD のリポジトリで更新スクリプトを実行します。

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- `beta` に切り替えてプルし、 `engine` と `build/gutenstructure` をコミット `beta` ピンに更新します (`git pull` のみは古いピンに残します)。 `-Branch <name>` は別のブランチをプルします。
- 各サブモジュールをピンと照合してチェックし、エンジンのバージョンを出力します。
- `Meitou.user.props` または環境の `MeitouEngineDir` と `MeitouFrameworkSources` にフラグを立てます。ビルドでこのリポジトリのピンの代わりに別のリポジトリのエンジンまたはフレームワークを使用します。フレームワークと MOD を一緒に変更する場合を除き、これらを削除してください。
- `-Clean` は、git で無視された `bin` フォルダーと `obj` フォルダーを削除するため、古いエンジンのビルドは再利用されません。 `-Build` は `dotnet build` を実行します。
- これは、 `Up to date` または `FIX` とマークされた行のリストで終わります。

次のビルドでは、 `meitou.lock.json` が変更された場合にのみ、フレームワークが再度ダウンロードされます。

[English](../../../docs/local-development.md) · [Kenshi Races](../README.md)
