<a id="kenshi-races"></a>
# Kenshi レース

3つの新しいレースが、キャラクター編集者、それぞれサブレースで、ゲームの人間のスケルトン(`kenshi.races`すべての記録が積み重ねられ、体が装備されている。 [レース輸入](race-import.md). また、船は [アニメーション図書館](animations.md) 各レースのための59クリップ。 モードはレースやクリップを使用し、サブレースを追加するか、独自のレースを使用します。 [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## レース

編集者はそれぞれ RACE_GROUP レースとその RACEs エンジンとして() docs/characters.md 「編集者」の内容です。

| レース(RACE_GROUP) | サブレイク(RACE) | ジェンダー | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | レオマ `10-` | 男性、女性 | レオマル `11-`レオマフェマ `17-` |
| Crevan `31-` | 乗組員 `18-` | 男性、女性 | クライアント `19-`クルマ `21-` |
| Crabfolk `32-` | クラブマン `12-` | シングル | クラブマン `13-` |
| | クラブ `14-` | シングル | クラブ `15-` |

終わりは、 `KenshiRaces.mod`. Subrace と CHARACTER 名前は低い単語なので、コンソールの `spawn` 引用なしで取り上げます。 各体のファイルは、 `KenshiRaces/items/races/kenshiraces_<character>*` (通常の地図) Leoma そして、 Crevan). 男性の体は、 `male_skeleton`女性は、 `female_skeleton`クラブの体は男性の合うので、クラブの女王は男性のアニメーションと声で単性です。 原作モデル(Meshy 輸出、スムーズ 1):レオマール「レオニド・バー - 3ds Max Biped.glb」、クラブマンとクラブケーン... - 3ds マックス・ビペッド.glb、その他 `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## レコード

ちなみに、このゲームのデータは、ゲームのデータに含まれています(`RaceRecords`)、そう `KenshiRaces.mod` 船は空っぽで、CIはゲームを必要としません:

- RACE_GROUPレースの名前とそのサブレース RACEs編集者の命令で
- RACE (原題: A copy of Greenlander (`17-gamedata.quack`: 統計、アニメーション、声) 各性別のメッシュ、色のテキスト、通常の地図
地図の一部(単性のスベラスは両方に1つの体を着用する) `single gender` 二つの体を有し、頭も髪もない(頭もないレースは、頭もない、体も自分のものを持っている)。 `morph num` 0; `no shirts` (シャツとパンツは人間の体のUVの配置を塗り、体のマスクと強さの正常な地図を清めます。 `playable` とのこと `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTERドライバーのコピー(`2205-gamedata.base`性別と性別(`female chance` 100または0、金、金、ボーナスなし
募集会話
- エンジン:A CHARACTER自分自身 `mesh` フィールドは、そのレースの体のマッサージを超えています。

<a id="character-editor"></a>
## キャラクター編集

レースは、ゲームのようなキャラクターの作成で提供されます。 `KenshiRaces/items/races/kenshiraces_editor.xml` 骨を動かすスライダーだけを保持し、人間の体と同じように動作します。 docs/animation.md骨のスケール:顔は頭の大きさ、頭の形と首のスライダーを保持します(顔のポーズはありません:固体の頭は1つありません)、身体は人間の体のスライドと皮膚のトーンを保持します、女性の乳房の大きさと高さとスペースで、女性の骨の動きを動かします。 `L Boob` / `R Boob` 女性の体重が重い([レース輸入](race-import.md#bust)). 人間は人間(`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## 募集

DIALOGUE `1-KenshiRaces.mod`キャラクターは「あなたはもう一つの手を使うことができるようだ」と答え、アクション18(チームに加わる)は、プレイヤーのチームに追加されます。

<a id="testing-in-game"></a>
## 試用ゲーム

1. 許可 Kenshi ラウンサーのレース(または `data/mods.cfg`).
2. コンソール: `spawn leomafemale` (あるいは、いかなる CHARACTER テーブル) コースにスパウン; 話して募集する, または F10 デビュー 募集。
3. F10 **Kenshi レース:レース** 各レース(このモードと依存者)の()を記録します。
4. 新しいゲーム:編集者のレースラウンドが到達 Leoma, Crevan そして、 Crabfolk.

<a id="clothing"></a>
## 服装

アムールは体の骨を共有するので、これらの体とともに移動しますが、それは人間の体の周りにモデル化されています。 部分地図は、物件の下に皮膚を隠す `hide parts` (スカート、コート、ジャケット) 体重減少は、他の場所の人間よりもより多くのものに引き込まれます([レース輸入](race-import.md#slimming)). 人間から遠く離れた形(シェル、クラウド)は、まだ、緊密な腕を介してポッキーしています。
