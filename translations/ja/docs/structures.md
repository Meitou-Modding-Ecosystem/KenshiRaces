<a id="structures-from-3d-models"></a>
# 3Dモデルの構造

ポットA `.glb` モデル(Meshy, Hi3Dブレイヤー(モード)で `Structures` フォルダーとゲームの構築は、構築メニューから置くことができる建物に変換されます: オグレメッシュ、圧縮されたテキスト、衝突と記録 `<Mod>.mod`. ノー FCSブレイダー、ゲームインストールは、輸入自体に必要ありません。

<a id="quick-start"></a>
## 早速スタート

1. モデルを入れて `<Mod>/Structures/`☆☆☆☆ `MyMod/Structures/watch-tower.glb`.
2. 選択肢:それに加えて、 `watch-tower.json` どんなものと [設定](#settings)☆☆☆☆

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. いつものように作り上げて([play.md](play.md)・Visual Studio F5、VS コード F5、または `dotnet run --project Play`. レコードは1つを示す。
`Structure ...` モデルによるライン
4. ゲーム内:メニュー、カテゴリー `EXTERIOR` (あるいは、あなたがたが定めたもの) デフォルトで `"research": "start"` 初めから知られているのは、
ゲーム、そして既存の節約も(Meitouは、常に含まれる完成調査のオープンです。 `_Default Start`).
5. モデルを採用し、 `.json` そして、 `Structures/ids.json` (最初の輸入に記載) 生成されたファイルは、出力を作成し、それ以外の
約束した。

ゲームなし: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## 設定

すべてのオプション(`<model>.json`, コメントが許可されます; 未知の名前はエラーですので、タイプは無視されません)。

| 設定 | デフォルト | 意味 |
| --- | --- | --- |
| `name` | ファイル名、 `-`/`_` スペースとして | メニューの名称 |
| `description` | `""` | メニューテキスト作成 |
| `height` | モデルの大きさ | 完成高度はメートルで、モデルはそれに等しくスケールされています。 AIモデルは、任意のサイズで来るので、これを設定します。 |
| `sink` | `0` | 地面に落ちるメートル(スロープに浮かぶのを止める) |
| `yaw` | `0` | 階段は垂直軸の周りを振り回す |
| `category` | `EXTERIOR` | メニューカテゴリー(`building category`ゲーム( )`BUILDINGS`, `CAMPING`, `FARMING`・・・あるいは新しい名前。 |
| `group` | `""` | `building group`: 建物 共有 1 は 1 入り口の変数 |
| `materials` | `5` | 建物は費用がかかる。 |
| `buildSpeed` | `2` | `build speed mult` (ゲームのテント5、ハウス1から2) |
| `research` | `start` | `start`初めから知られています。 `none`あなたがたは、何事もなく、あなたがたの手のひらに、あるいは、 RESEARCH ストレッチID、e.g `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` あるいは `both` |
| `createsTown` | `true` | 町の外に置くのは、出発点です。 |
| `collision` | `mesh` | `mesh` (シンプル化されたコピー) `box` (ボックスの入り口) `none` (その通りを歩く) |
| `collisionTriangles` | `2000` | ほとんどの三角形は `mesh` 衝突が続く(12〜50万) |
| `textureSize` | `2048` | ピクセルで最大のテキストサイド(最大8192点のパワー) |

<a id="item-models"></a>
## アイテムモデル

アイテムのモデルを入力する `<Mod>/ItemModels/` 代わりに(`Structures` 建物に変えようかな) 輸入は、そのメッシュと構造を、構造と一つの構造に加えて書く。 MATERIAL_SPECS_CLOTHING レコーディング:あなたのモード ITEM レコードはそれらを指摘します。 レコードは1つを示す。 `Item ...` 使用する名前を含むモデルによるライン

1. `MyMod/ItemModels/lantern.glb`オプションで `lantern.json`: `{ "size": 0.3 }`.
2. 項目記録(データ生成器またはデータ段階)に、以下を設定します。

田舎の価値観
   | --- | --- |
| `mesh` そして、 `ground mesh` (フィレンツェ) `<mod>_<model>.mesh`☆☆☆☆ `mymod_lantern.mesh` | | `material` (参照) イラスト `<950000 + 100 × block>-<Mod>.mod`ブロックから `ItemModels/ids.json`☆☆☆☆ `950000-MyMod.mod` |

まず、物質を調べる(`GameDatabase.Find`)そして、それが欠けているときにスタンドインを見つける:リリースされたジップ、そしてプレイされたモードなしでゲームを読むテストは、それを持っていません。
3. モデルを採用し、 `.json` そして、 `ItemModels/ids.json`.

| 設定 | デフォルト | 意味 |
| --- | --- | --- |
| `name` | ファイル名、 `-`/`_` スペースとして | 物質記録の名称 |
| `size` | モデルの大きさ | 最長の側面はメートルで、モデルはそれに平等にスケールされている。 |
| `yaw` | `0` | 階段は垂直軸の周りを振り回す |
| `textureSize` | `1024` | ピクセルで最大のテキストサイド(最大8192点のパワー) |

- 1 項目ごとに 1 項目(グローバル) ITEM 名前 1 `material`);モデルは、ゲームのアイテムマッサージが行うように、その最も低い点に立っています。
- ゲームは、地面にアイコン、手のひらにアイコンを引き出す。 `mesh` (`ground mesh` 初めに地上に、そして、
MATERIAL_SPECS_CLOTHING 彼女の `material` エンジン(エンジン) `docs/formats/runtime-materials.md` 「呼び」、 `WorldObjectLayout.ItemMesh`).
- 記録のアイコンは、記録のセットが付いている場合を除き、 `auto icon image` 偽りと自分の `icon`.

<a id="getting-a-good-model"></a>
## グローバルモデル

- 質問 Meshy あるいは Hi3D Aのために PBRテキストモデルをダウンロードして、 GLB. 数十万の三角形がたくさんあり、ロゴの旗はモデルです。
150万以上。 形状を切り、サイズを選択し、テキストを縮小します。 [model-prep.md](model-prep.md).
- 図面の前面は、図面に Z + Z を表示します(glTF使用(使用) 使用 `yaw` 逆転する。
- それぞれの物質が自分の部分になる。 発電機は通常1個、最大49個を提供します。
- モデルは、最も低い点、中心に立っています。

<a id="what-it-writes"></a>
## 何を書く

モードフォルダー(`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` (物質に1つ、 `_1`, `_2`いくつかの場合:オグレ `[MeshSerializer_v1.8]`位置、正常、UV、
手動性のW、ゲームの独自の建物の配置。
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: ディフーズ(BC3)、正常(BC1、BC3、切断時に)、金属性(BC1)、ミップ。
- `structures/<mod>_<model>_col.xml`・衝突
- アイテムモデルについて: `structures/<mod>_<model>.mesh` そして、その構造は、衝突なし。
- レコードが合う `<Mod>.mod` その他のものを含む(データ生成器の記録が保存され、以前の輸入が置き換えられている):
BUILDINGひとつひとつ BUILDING_PART そして、 MATERIAL_SPEC 物質と開封(研究) `enable buildings` 建物の収穫(一つ) MATERIAL_SPECS_CLOTHING アイテムモデル

ファイル名はモードの名前で始まりますが、ゲームはすべてのモードで単純なファイル名でリソースを表示します。

レコードIDは `<n>-<Mod>.mod` で n から 900000 まで で で は で あり でし て でしょ う で も でし と し て い ます でし, でし の モデル に は 100 個 の ブロック (構築 +0, 部品 +1, 材料 +50); 項目 モデル の 数 は 950000 個 (材料 +0) で です。 `ItemModels/ids.json`. `Structures/ids.json` それぞれのモデルのブロックを保持し、IDは決して変わらない:保存者は記録のIDによって位置づけられた建物を名付けます。 決してブロックを編集したり、削除したりすることは、モデルを置いたゲームから取り除くことです。

<a id="facts-the-import-relies-on"></a>
## 輸入が依存する事実

ゲームのファイルをチェックする(Kenshi 1.0.68 データ) とエンジンの読者と共に:

- **ユニット**世界単位は0.1m、Y up。 glTF メートル、Yアップですので、スケールは10です(ゲームの衝突ファイルは重力98.1を使用します)。
- **材料**建物の一部の見た目は、 MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`(メッサの)メッサではない
エンジン名(エンジン) `docs/formats/runtime-materials.md`). ゲームのシェイダー(`data/materials/deferred/objects.hlsl`読む ディフューズ RGB 色とAは輝き(時間) `specular mult`(普通に) RGB マッサージはマッサジとマッサリーはマッシャージ(`material type` 1, `alpha threshold`金属のR。 輸入はグラス = 1 - glTF 金属性と金属性は、 glTFブルーチャンネル glTF MASK そして、 BLEND カットアウトになる。
- **タンジン**ちなみに、シェイダーのバイノーマルはクロス(タンゲン、正常) × w で、ゲームのメッセージでは、増加する V に沿って点数を表示します。**検証**:
`prisoner_pole.mesh` 86個の三角形のうち、 `Camp_tent1.mesh` 1858年(1865年) glTF 通常の地図は、画像をグリーンにする(Vを減らす)ので、輸入はグリーンチャンネルを回転させ、ゲームの道を設定します。
- **衝突**●一部の `xml collision` PhysX NxuStream2 ファイル; 手書きの最小ファイル充電(ゲームの `Camp_tent1_COL.xml` は、
1箱) PhysX は Z up とゲーム マップ (x, y, z) から (x, z, -y) まで ですので、輸入 は パーツ の ポイント を (x, -z, y) として 書きます。 三角形は、平らな点と三角形として入り、ゲームはそれを積み重ね、メイトはそれを読む。 `docs/formats/collision.md`).
- **フィールド**: BUILDING そして、 BUILDING_PART は ゲーム キャンプ テント 1 の 完全 フィールド セット を 運ぶ でしょ (`18026-small_changes_otto.mod`だから、何も
デフォルトに落ちる; ノー `functionality`それは、平らな建物です。
- **オフロック**: 建物が構築メニューに置かれているので、完成した研究がリストに置かれる `enable buildings`すべてのゲームが始まる。
`_Default Start` (`5359-gamedata.base`(終わり) **検証** エンジンと共に `GameDatabase`, `ResearchGate` そして、 `BuildingBuildData` ベースゲームと輸入モード:構造は解体され、構造材料は設定されます。

<a id="not-covered-yet"></a>
## まだカバーされていない

室内、ドア、機能(ストレージ、ベッド、デザイン)および回転部品。
