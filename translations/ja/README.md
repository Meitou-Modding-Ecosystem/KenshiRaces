<a id="kenshi-races"></a>
# Kenshi レース

新たなレース [クライアント](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) ゲームの人間の骨折、キャラクター編集でプレイ可能: Leoma そして、 Crevan (男と女)と、 Crabfolk (クラブマン、クラブケーン)は、人間の骨折のための59クリップのアニメーションライブラリで、その他のモードは、自分自身のレースとサブレースを追加します。 GPL0 0 後後

- レース、彼らの記録とゲームでそれらをテスト: [docs/kenshi-races.md](docs/kenshi-races.md).
- これらのレースを使用するか、別のモードから自分自身を追加する: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (エージェントスキル)
`.claude/skills/kenshi-races-dependent`).
- アニメーション図書館(人間の骨髄のあらゆるレースのための59クリップ)とそれがどのように作られたか: [docs/animations.md](docs/animations.md).
- ひきこもり `.glb` 体(Meshy, Hi3D 「3ds Max Biped」は、ゲームのスケルトンに。 [docs/race-import.md](docs/race-import.md).

- Visual Studio、VSコード、またはターミナル(このリポのエンジン、またはゲームに配置されたインストールラウンサー)を使用して現地で構築および実行します。 [docs/play.md](docs/play.md).
- あなたの機械に休憩を設定する: [docs/local-development.md](docs/local-development.md).
- 休憩を延ばす変化についての代理人を指示する: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- エンジンピン、バージョン、CIのルール: [CLAUDE.md](../../CLAUDE.md) (同様に `AGENTS.md`ステップ・ステップ・プロセス:
`.claude/skills/meitou-engine-release`.
- 最新のメイトゥーリリースに対して構築されたモードをチェックする(CIはそれぞれのPRでそれを行います): [docs/refcheck.md](docs/refcheck.md).
- Steamワークショップへのモードを最初に公開し、それぞれのリリースで公開します。 [docs/steam-workshop.md](docs/steam-workshop.md).
- 回転 `.glb` モデル(Meshy, Hi3Dブレンド(ブレンド)は、構築可能な構造に、 FCS 必要: [docs/structures.md](docs/structures.md);
生成されたモデルのポリコン、サイズ、テキストを最初に取得する: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## 数字

仕事の種類(`AddGoalType`, `AddJobType`)と対話コード(`AddDialogueCondition`, `AddDialogueEffect`すべてのモードで共有される([ブロック番号](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi レースは何も言わない。

| 番号 | プール | 利用 |
| --- | --- | --- |

<a id="layout"></a>
## レイアウト

- `engine/`: MeitouClient git サブモジュールとして、エンジンが反対のモード構築を結びつける。 `git pull` 引っ越すことはない: 後
すべてのパイロット `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([クローンが流れ](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`モードのプロジェクトと `meitou.json`; `KenshiRaces/items/races/`関連商品(関連商品)
`KenshiRaces.Tests/`テスト(合成データ、ゲームインストールは必要ありません) `<Mod>.Data/` (オプション): 書く ジェネレーター `<Mod>.mod` インストールされたゲーム。 `<Mod>/Structures/` (オプション) : `.glb` 建物の構造を構築するための構造([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`: フレームワークは、モードが構築され、
([フレームロック](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)); フレームワークアップデートのワークフローはそれを新しいリリースに移動し、CI が通過したときに合併します([フレームアップ](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`1クリックで構築、実行、実行
- `tools/`: `Update-Local.ps1`エンジン・ピン・チェック([クローンが流れ](docs/local-development.md#keep-clones-current));
`refcheck.sh` そして、 `RefCheck/`エンジン・レファレンス・チェック([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`輸入モデル([docs/structures.md](docs/structures.md)); `RaceImport/`体の調子([docs/race-import.md](docs/race-import.md)); `model-info.py` そして、 `prepare-model.py`テクノロジー・テキスト・テクスト・コントロール([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` エンジンをインポートするので、モードはゲームの設定で構築します(警告はエラーです)。
- `Directory.Build.rsp` エンジンの参照を表示し、最古のメイトゥーがモードのサポートをリリースするように、それぞれのモードに等しい。 `gameVersion` 地面:The
ゲームは、新しいエンジンスタンプを参照するモードを拒否し、サブモジュールは構築します。 `1.0.0.0` ([バージョン](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). の  は 毎日  を チェック し て い ます。[メイタ・コンパクト](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## 部門

PR入り `beta`CI(構築とテスト)は通過する必要があります。 `beta` 推進される `main` ゲームでテストされ、生産はPRを通して行われます。 `release`. 各合併 `release` タグは `<slug>-v<version>` モードのZIP(`.github/workflows/release.yml`(このタグが存在しない限り、 `tools\Tag-Release.ps1` マニュアル・ファルバック

- 新規保管および永久保管 beta/main/release レビュー: [docs/branch-policy.md](docs/branch-policy.md). テンプレートコピーソース; GutenStructure は、調和を通じて設定を実施します。
