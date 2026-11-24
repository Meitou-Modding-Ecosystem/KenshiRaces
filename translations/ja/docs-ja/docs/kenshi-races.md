<a id="kenshi-races"></a>
# Kenshi Races

ゲームの人間のスケルトン ( `kenshi.races` 、依存関係なし) に、キャラクター エディター用の 3 つの新しいレース (それぞれサブレースがあります): すべてのレコードがロード時に追加され、ボディは [レース インポート](race-import.md) によって適合されます。また、これらのスケルトンの種族ごとに 59 個のクリップを含むアニメーション ライブラリ](animations.md) も同梱されています。 MOD は種族とクリップを使用し、サブ種族を追加したり、独自の種族を追加したりします: [ddependent-on-kenshi-races.md](depending-on-kenshi-races.md) 。

<a id="races"></a>
## レース

エディターには、各 RACE_GROUP が種族としてリストされ、その RACE がサブ種族としてリストされます (engine docs/characters.md 「キャラクター エディター」)。

| レース ( RACE_GROUP ) | サブレース ( RACE ) | 性別 | キャラクター |
| --- | --- | --- | --- |
| Leoma `30-` | レオマ `10-` | 男性、女性 | レオマ男性 `11-` 、レオマ女性 `17-` |
| Crevan `31-` | クレバン `18-` | 男性、女性 | クレバン男性 `19-` 、クレヴァン女性 `21-` |
| Crabfolk `32-` | カニマン `12-` | シングル | カニマン `13-` |
|  | クラブクイーン `14-` | シングル | クラブクイーン `15-` |

ID は `KenshiRaces.mod` で終わります。 Subrace 名と CHARACTER 名は 1 つの小文字であるため、コンソールの `spawn` はそれらを引用符なしで受け取ります。各ボディのファイルは `KenshiRaces/items/races/kenshiraces_<character>*` ( Leoma および Crevan の法線マップ) です。男性のボディは `male_skeleton` に、女性のボディは `female_skeleton` に取り付けられます。カニの体は男性にぴったりであるため、カニの女王は男性のアニメーションと音声を備えた単一の性別です。ソース モデル ( Meshy エクスポート、スリム 1): leomamale "Leonid Bare - 3ds Max Biped.glb"、cravman およびクラブクイーン "... - 3ds Max Biped.glb"、その他の `leonidfemale.glb` 、 `corvidmale.glb` 、 `corvidfemale.glb` 。

<a id="records"></a>
## 記録

ロードされたゲーム自身のレコード ( `RaceRecords` ) からデータ フェーズで追加されるため、 `KenshiRaces.mod` は空の状態で出荷され、 CI にはゲームが必要ありません。

- RACE_GROUP : レースの名前とそのサブレースの RACE (編集者の順序)。
- RACE (サブレース): 各性別のメッシュ、カラー テクスチャ、ノーマル マップ、パーツ マップを備えた Greenlander ( `17-gamedata.quack` : 統計、アニメーション、音声) のコピー (単一の性別のサブレースは、両方の 1 つの体を着用します)。  両方のボディがある場合を除き、 `single gender` 。頭も髪もありません（種族リストに頭がなければ何も描画されません。体には独自のものがあります）。 `morph num` 0; `no shirts` (シャツとパンツは人体の UV レイアウトをペイントします);ボディマスクとストレングスノーマルマップがクリアされました。 `playable` と `editor limits` [ `kenshiraces_editor.xml` ](#character-editor) 。
- CHARACTER : 亜人種と性別 ( `female chance` 100 または 0) のドリフター ( `2205-gamedata.base` ) のコピー。キット、お金、賞金はありません。新兵の話が付いています。
- エンジン: CHARACTER 自身の `mesh` フィールドは、そのレースのボディ メッシュをオーバーライドします。

<a id="character-editor"></a>
## キャラクターエディター

レースは、ゲーム独自のものと同様に、キャラクター作成時に提供されます。 `KenshiRaces/items/races/kenshiraces_editor.xml` はボーンを動かすスライダーのみを保持するため、人間と同様にフィットしたボディで動作します (engine docs/animation.md、ボーン スケール): 顔は頭のサイズ、頭の形状、首のスライダーを保持します (顔のポーズはありません。フィットした頭には何もありません)。 Body は、女性のスケルトンの `L Boob` / `R Boob` を移動させ、女性の体の重み付けを行う人体スライダーと肌のトーン、女性の胸のサイズ、身長、間隔を保持します ([レース インポート](race-import.md#bust) ])。範囲は人間のものです ( `data/editor/editor_data_human.xml` )。

<a id="recruiting"></a>
## 人材募集

DIALOGUE  `1-KenshiRaces.mod` : キャラクターが「もう一組の手でも使えそうな感じですね。」と言うと、プレイヤーは「参加してください。」と答えます。アクション 18 (分隊に参加) では、キャラクターがプレイヤーの分隊に追加されます。

<a id="testing-in-game"></a>
## ゲーム内でのテスト

1. ランチャーで Kenshi Races (または `data/mods.cfg` ) を有効にします。
2. コンソール: `spawn leomafemale` (またはテーブル内の任意の CHARACTER ) がカーソルの位置に生成されます。 recruit に話しかけるか、F10 で Recruit をデバッグします。
3. F10 ** Kenshi Races : レース** は、ロードされているすべての適合レース (この MOD と依存物) をリストします。
4. 新しいゲーム: エディターのレース矢印は Leoma 、 Crevan 、および Crabfolk に到達します。

<a id="clothing"></a>
## 衣類

鎧は体の骨格を共有しているため、これらの体と一緒に動きますが、人体を中心にモデル化されています。パーツ マップは、 `hide parts` のアイテム (コート、ローブ、ジャケット) の下のスキンを非表示にします。痩身は、人間よりも大きなものを他の場所に引き込みます（[レースインポート](race-import.md#slimming) ]）。人間とはかけ離れた形状（甲羅、爪）が依然として堅い鎧を突き抜けています。
