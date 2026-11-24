<a id="structures-from-3d-models"></a>
# 3Dモデルからの構造

`.glb` モデル ( Meshy 、 Hi3D 、 Blender エクスポート) を MOD の `Structures` フォルダーに置くと、プレイ ビルドによってそれがビルド メニューから配置できる建物に変わります: Ogre メッシュ、圧縮テクスチャ、コリジョン、および `<Mod>.mod` のレコード。 FCS 、 Blender 、インポート自体にゲームのインストールは必要ありません。

<a id="quick-start"></a>
## クイックスタート

1. モデルを `<Mod>/Structures/` に入れます。 `MyMod/Structures/watch-tower.glb` 。
2. オプション: その横に、 `watch-tower.json` 任意の [設定 ](#settings) 、例えば

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. 通常どおりビルドしてデプロイします ([play.md](play.md) ): Visual Studio F5、 VS Code F5、または `dotnet run --project Play` 。ビルド ログには、モデルごとに 1 つの `Structure ...` 行が表示されます。
4. ゲーム内: ビルド メニュー、カテゴリ `EXTERIOR` (または設定したもの)。デフォルトの `"research": "start"` では、ゲームの開始時から既知であり、既存のセーブでも既知です ( Meitou は、完了した研究からロック解除を計算します。これには常に `_Default Start` が含まれます)。
5. モデル、その `.json` および `Structures/ids.json` (最初のインポートに書き込まれた) をコミットします。生成されたファイルはビルド出力であり、コミットされません。

遊びなし: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>` 。

<a id="settings"></a>
## 設定

すべてオプション ( `<model>.json` 、コメントは許可されています。不明な名前はエラーとなるため、タイプミスは見逃されません)。

| 設定 | デフォルト | 意味 |
| --- | --- | --- |
| `name` | ファイル名、 `-` / `_` (スペースとして) | ビルドメニューの名前 |
| `description` | `""` | ビルドメニューテキスト |
| `height` | モデル自身のサイズ | 仕上がりの高さ（メートル単位）。モデルはそれに合わせて均等にスケールされます。 AI モデルには任意のサイズがあるため、これを設定します |
| `sink` | `0` | メートルが地面に沈みます（斜面に浮くのを防ぎます） |
| `yaw` | `0` | 垂直軸を中心に回転した度 |
| `category` | `EXTERIOR` | ビルド メニュー カテゴリ ( `building category` ): ゲームの ( `BUILDINGS` 、 `CAMPING` 、 `FARMING` 、 ...) または新しい名前 |
| `group` | `""` | `building group` : 1 つを共有する建物は 1 つのエントリのバリアントです |
| `materials` | `5` | 建築資材にかかる費用 |
| `buildSpeed` | `2` | `build speed mult` (ゲームのテント 5、家 1 ～ 2) |
| `research` | `start` | `start` : 最初からわかっています。 `none` : ロックを解除するものは何もありません (自分の記録がロックを解除します)。または RESEARCH 文字列 ID (例: `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor` 、 `indoor` または `both` |
| `createsTown` | `true` | 町の外に置くと前哨基地が始まります |
| `collision` | `mesh` | `mesh` (モデルの簡略化されたコピー)、 `box` (その境界ボックス)、または `none` (モデルのウォークスルー) |
| `collisionTriangles` | `2000` | `mesh` コリジョンが保持するほとんどの三角形 (12 ～ 50,000) |
| `textureSize` | `2048` | ピクセル単位の最大テクスチャ面 (2 のべき乗、最大 8192) |

<a id="item-models"></a>
## アイテムモデル

代わりにアイテムのモデルを `<Mod>/ItemModels/` に置きます ( `Structures` では建物になります)。インポートは、構造と 1 つの MATERIAL_SPECS_CLOTHING レコードの横にメッシュとテクスチャを書き込みます。 MOD の ITEM レコードはそれらを指します。ビルド ログには、モデルごとに 1 つの `Item ...` 行と使用する名前が表示されます。

1. `MyMod/ItemModels/lantern.glb` 、オプションで `lantern.json` : `{ "size": 0.3 }` 。
2. 品目レコード (データ ジェネレーターまたはデータ フェーズ) で、次のように設定します。

|フィールド |値 |
   | --- | --- |
| `mesh` そして `ground mesh` (ファイル名) | `<mod>_<model>.mesh` 、例えば `mymod_lantern.mesh` | | `material` (参考) | `<950000 + 100 × block>-<Mod>.mod` 、からブロックします `ItemModels/ids.json` 、例えば `950000-MyMod.mod` |

最初にマテリアル ( `GameDatabase.Find` ) を検索し、不足している場合は代役の外観を維持します。リリースされた zip と、デプロイされた MOD なしでゲームを読み取るテストには、それがありません。
3. モデル、その `.json` および `ItemModels/ids.json` をコミットします。

| 設定 | デフォルト | 意味 |
| --- | --- | --- |
| `name` | ファイル名、 `-` / `_` (スペースとして) | マテリアルレコードの名前 |
| `size` | モデル自身のサイズ | 最長辺をメートル単位で表します。モデルはそれに均等にスケールされます |
| `yaw` | `0` | 垂直軸を中心に回転した度 |
| `textureSize` | `1024` | ピクセル単位の最大テクスチャ面 (2 のべき乗、最大 8192) |

- 項目モデルごとに 1 つのマテリアル ( ITEM は 1 つの `material` に名前を付けます)。モデルは、ゲームのアイテム メッシュと同様に、中心の最下点に立っています。
- ゲームは、 `mesh` (最初に地面にある `ground mesh` ) と MATERIAL_SPECS_CLOTHING の `material` 名 (エンジン `docs/formats/runtime-materials.md` "Callers"、 `WorldObjectLayout.ItemMesh` ) を使用して、地面、手、および自動アイコンにアイテムを描画します。
- レコードが `auto icon image` false と独自の `icon` を設定しない限り、インベントリ アイコンはモデルから描画されます。

<a id="getting-a-good-model"></a>
## 良いモデルを入手する

- Meshy または Hi3D に PBR テクスチャ モデルを要求し、 GLB としてダウンロードします。三角形は数万個あれば十分です。ログには 150,000 を超えるモデルのフラグが立てられます。ポリカウントをカットし、サイズを選択し、テクスチャを縮小します: [model-prep.md](model-prep.md) .
- ファイルではモデルの前面が +Z を向いています (glTF の規則)。 `yaw` を使って回してください。
- それぞれの素材が独自の部品になります。ジェネレータは通常 1 つを与えます。せいぜい49。
- モデルは中央の最下点に立っています。

<a id="what-it-writes"></a>
## 書かれていること

デプロイされた MOD フォルダー ( `<Kenshi>/mods/<Mod>/` ):

- `structures/<mod>_<model>.mesh` (マテリアルごとに 1 つ、 `_1` 、 `_2` 、... 複数の場合): Ogre `[MeshSerializer_v1.8]` 、位置、法線、 UV および w の利き手による接線、ゲーム独自の建物メッシュのレイアウト。
- `structures/<mod>_<model>_d.dds` 、 `_n.dds` 、 `_m.dds` : ディフューズ (BC3)、ノーマル (BC1、切り抜き時 BC3)、メタルネス (BC1)、ミップあり。
- `structures/<mod>_<model>_col.xml` : 衝突。
- 項目モデルの場合: `structures/<mod>_<model>.mesh` とそのテクスチャ、衝突なし。
- レコードは、その中の他のものと並んで `<Mod>.mod` にマージされます (データ ジェネレーターのレコードは保持され、以前のインポートは置き換えられます): BUILDING 、マテリアルごとに 1 つの BUILDING_PART および MATERIAL_SPEC 、およびロック解除 (リサーチの `enable buildings` が建物を取得します)。アイテムモデルごとに 1 つの MATERIAL_SPECS_CLOTHING 。

ゲームはすべての MOD にわたって裸のファイル名でリソースを検索するため、ファイル名は MOD の名前で始まります。

レコード ID は `<n>-<Mod>.mod` で、n は 900000 からで、モデルごとに 100 のブロック (建物 +0、部品 +1、材料 +50)。項目モデルは `ItemModels/ids.json` で 950000 (材質 +0) からカウントされます。 `Structures/ids.json` は各モデルのブロックを保持するため、ID は決して変更されません。保存では、配置された建物にそのレコードの ID で名前が付けられます。ブロックを編集したり削除したりしないでください。モデルを削除すると、その建物を配置したゲームからその建物が削除されます。

<a id="facts-the-import-relies-on"></a>
## インポートが依存する事実

ゲームのファイル ( Kenshi 1.0.68 データ) およびエンジンのリーダーと照合して確認しました。

- **単位**: 世界単位は 0.1 メートル、Y は上です。 glTF はメートル、Y が上なので、スケールは 10 です (ゲームの衝突ファイルは重力 98.1 を使用します)。
- **マテリアル**: 建築パーツの外観は、メッシュのマテリアル名 (エンジン `docs/formats/runtime-materials.md` ) ではなく、その MATERIAL_SPEC ( `texture map` 、 `normal map` 、 `metalness map` 、 `material type` ) です。ゲームの構築シェーダ ( `data/materials/deferred/objects.hlsl` ) は、拡散 RGB をカラーとして、A を光沢 ( `specular mult` の倍) として読み取り、通常の RGB を接線空間法線として、A をカットアウト マスク ( `material type` 1、 `alpha threshold` ) として読み取り、メタルネスを R から読み取ります。インポートでは、glTF から glTF の粗さとメタルネスを書き込みます。ブルーチャンネル。 glTF MASK 、 BLEND はカットアウトとなります。
- **接線**: シェーダーの従法線はcross(接線, 法線) × wであり、ゲームのメッシュ上では増加するVに沿って指します(**検証済み**: `prisoner_pole.mesh` 86/86三角形、 `Camp_tent1.mesh` 1858/1865)。 glTF 法線マップは画像の上に緑を指す (V が減少する) ため、インポートによって緑のチャンネルが丸くなり、ゲームの方向が設定されます。
- **衝突**: パーツの `xml collision` は PhysX NxuStream2 ファイルです。手書きの最小限のファイルがロードされます (ゲームの `Camp_tent1_COL.xml` は 1 つのボックスです)。 PhysX は Z アップで、ゲームは (x, y, z) を (x, z, -y) にマップするため、インポートではパーツのポイントが (x, -z, y) として書き込まれます。三角形メッシュは、単純な点と三角形として入力されます。ゲームはロード時にそれをクックし、 Meitou はそれをそのまま読み取ります (エンジン `docs/formats/collision.md` )。
- **フィールド**: BUILDING および BUILDING_PART には、ゲームのキャンプ テント 1 ( `18026-small_changes_otto.mod` ) の完全なフィールド セットが含まれるため、デフォルトに該当するものはありません。 `functionality` がないので、地味な建物です。
- **ロック解除**: 完了した調査で `enable buildings` に建物がリストされると、建物は構築メニューに表示されます。すべてのゲームは `_Default Start` ( `5359-gamedata.base` ) で始まり、終了します。 **エンジンの `GameDatabase` 、 `ResearchGate` 、および `BuildingBuildData` でベース ゲームとインポートされた MOD を使用して検証済み**: 構造はロックが解除されており、設定どおりの建築材料がかかります。

<a id="not-covered-yet"></a>
## まだカバーされていません

インテリア、ドア、機能（収納、ベッド、クラフト）、回転部品。
