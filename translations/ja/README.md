<a id="kenshi-races"></a>
# Kenshi Races

ゲームの人間のスケルトンの MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) の新しい種族、キャラクター エディターで再生可能: Leoma と Crevan (男性と女性) および Crabfolk (カニマン、クラブクイーン)、すべての人間のスケルトン レースの 59 クリップのアニメーション ライブラリ、および他の MOD が独自のレースとサブレースを追加するフレームワーク。 GPL -3.0以降。

- レース、その記録、ゲーム内でのテスト: [docs/kenshi-races.md](docs/kenshi-races.md) 。
- これらの種族を使用するか、別の MOD から独自の種族を追加します: [docs/ddependent-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (エージェント スキル `.claude/skills/kenshi-races-dependent` )]。
- アニメーション ライブラリ (人間のスケルトンの人種ごとに 59 個のクリップ) とその作成方法: [docs/animations.md](docs/animations.md) .
- リグ付き `.glb` ボディ ( Meshy 、 Hi3D "3ds Max Biped") をゲームのスケルトンにフィッティングします: [docs/race-import.md](docs/race-import.md) 。

- Visual Studio 、 VS Code 、またはターミナル (このリポジトリのエンジン、またはゲームにデプロイされたインストール済みランチャー) を使用してローカルでビルドして実行します: [docs/play.md](docs/play.md) .
- マシン上にリポジトリをセットアップします: [docs/local-development.md](docs/local-development.md) .
- リポジトリにまたがる変更についてエージェントに指示する: [docs/agents-across-repos.md](docs/agents-across-repos.md) 。
- エンジンのピン、バージョン、 CI 、リリースのエージェント ルール: [ CLAUDE .md](../../CLAUDE.md) ( `AGENTS.md` と同じ);段階的な手順: `.claude/skills/meitou-engine-release` 。
- ビルドされた MOD を最新の Meitou リリースと照合してチェックします ( CI はすべての PR で実行します): [docs/refcheck.md](docs/refcheck.md) .
- MOD を初めて Steam ワークショップに公開し、その後はリリースごとに公開します: [docs/steam-workshop.md](docs/steam-workshop.md) 。
- 旋回 `.glb` モデル ( Meshy , Hi3D , Blender ) 建築可能な構造物に、いいえ FCS 必要: [docs/structs.md] ](docs/structures.md) ;まず、生成されたモデルのポリ数、サイズ、テクスチャを正しく取得します: [docs/model-prep.md] ](docs/model-prep.md) .

<a id="numbers"></a>
## 数字

タスク タイプ ( `AddGoalType` 、 `AddJobType` ) とダイアログ コード ( `AddDialogueCondition` 、 `AddDialogueEffect` ) は、ロードされたすべての MOD ([MOD 番号ブロック](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) )] によって共有されます。 Kenshi Races は何も主張していない。

| 番号 | プール | 使用 |
| --- | --- | --- |

<a id="layout"></a>
## レイアウト

- `engine/` : Git サブモジュールとしての MeitouClient は、MOD ビルドの対象となるエンジンに固定されます。 `git pull` は移動しません。プル実行のたびに `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` を実行します ([クローンを最新の](docs/local-development.md#keep-clones-current) に維持します)。
- `KenshiRaces/` : MOD のプロジェクトと `meitou.json` ; `KenshiRaces/items/races/` : フィットしたボディ (コミットされたインポート出力)。 `KenshiRaces.Tests/` : そのテスト (合成データ、ゲームのインストールは必要ありません)。 `<Mod>.Data/` (オプション): インストールされているゲームから `<Mod>.mod` を書き込むジェネレーター。 `<Mod>/Structures/` (オプション): `.glb` モデルは、デプロイ時に構築可能な構造としてインポートされます ([docs/ Structures.md](docs/structures.md) )。
- `meitou.lock.json` : フレームワークは ([フレームワーク ロック](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ); に対して MOD ビルドをリリースします。フレームワーク更新ワークフローは、フレームワークを新しいリリースに移動し、 CI が通過するとマージします ([フレームワーク更新](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ])。
- `Play/` : ワンクリックでビルド、デプロイ、実行。
- `tools/` : `Update-Local.ps1` 、ローカル プルとエンジン ピン チェック ([クローンを最新の](docs/local-development.md#keep-clones-current) に保つ)。 `refcheck.sh` および `RefCheck/` 、エンジン参照チェック ([docs/refcheck.md](docs/refcheck.md) )]; `StructureImport/` 、モデルのインポート ([docs/structs.md](docs/structures.md) ); `RaceImport/` 、ボディフィッティング ([docs/race-import.md](docs/race-import.md) )]; `model-info.py` および `prepare-model.py` 、モデル チェックとテクスチャ縮小 ([docs/model-prep.md](docs/model-prep.md) )]。
- `Directory.Build.props` はエンジンをインポートするため、MOD はゲームの設定でビルドされます (警告はエラーです)。
- `Directory.Build.rsp` は、各 `gameVersion` フロアに等しい、MOD サポートをサポートする最も古い Meitou としてエンジン参照をスタンプします。ゲームは、より新しいエンジン スタンプを参照する MOD を拒否し、サブモジュールは `1.0.0.0` ([バージョン管理](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ]) をビルドします。 Meitou 互換性ワークフローは、新しい Meitou リリース ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ]) に対して MOD を毎日チェックします。

<a id="branches"></a>
## 支店

`beta` に PR します。 CI (ビルドとテスト) に合格する必要があります。 `beta` はプレイでテストされると `main` に昇格し、本番環境は PR から `release` に進みます。 `release` にマージするたびに `<slug>-v<version>` タグが付けられ、そのタグが存在しない限り MOD の zip ( `.github/workflows/release.yml` ) が公開されます。 `tools\Tag-Release.ps1` は手動フォールバックです。

- 新しいリポジトリのブランチ保護と永続的なベータ/メイン/リリースのレビュー: [docs/branch-policy.md](docs/branch-policy.md) 。テンプレートのコピーソース。 GutenStructure は調整を通じて設定を適用します。
