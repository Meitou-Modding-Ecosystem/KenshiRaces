<a id="preparing-a-model"></a>
# モデル作り

どうやって生み出されたのか。 `.glb` (Meshy, Hi3Dブレイダー) ポリコン、テキストサイズ、現実のサイズ ゲームの操作、それに入る前に `<Mod>/Structures/` (建物)または `<Mod>/ItemModels/` (アイテム) ワイワイの輸入: [structures.md](structures.md).

コマンドは、PowerShellのレポのルートから実行されます。 Python 3 が必要です。 `prepare-model.py` 必要性も `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. 検査

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

サイズはメートル(X、Yアップ、Z)で、最も長い側面、三角形、材料、それぞれのテキストのピクセルサイズとMBを印刷します。 ノードスケールまたは回転の意味についてのメモ `prepare-model.py` 拡張されない:再びブレンダーから変換を実施する(オブジェクト > 適用 > すべての変換)で輸出します。

<a id="2-cut-the-polycount"></a>
## 2. ポリコントを切る

| 優しい | 三角形 | テキスト |
| --- | --- | --- |
| 建設・装飾 | 5万～20万 | 2048 |
| 物件(店内で持っている、落ちた) | 以下5000 | 1024 |

発電機はしばしば100,000以上を提供し、輸入は150,000以上を警告します。 切断するには:

- **Meshy:** 上記の予算にターゲットポリコンを置く(四角形または三角形のトポロジーは両方働く)、構造を維持し、
ダウンロードするように GLB また。
- **ブレンド:** ファイル > 輸入 glTFマッサージを選択し、悪化した変数(コラップ)を追加し、顔が状態で数えるまで割合を下げます。
バーは予算にあり、それを適用し、ファイル > 輸出 > glTF バイナリー(`.glb`改装を適用する。

単純化(単純化)`collisionTriangles`, デフォルト 2000 ) ですので、見えるメッシュはフレームレートとファイルサイズについてです。

<a id="3-choose-the-size"></a>
## 3. サイズを選択

輸入はモデルをメートルとして扱い、発電機のサイズは自発的である。 ちなみに, ちなみに ちなに ちなに の サイズ を 置いて モデルの 設定 ファイル に 置き ます ([「Settings」の設定](structures.md#settings)ちなみに、このモデルでは、

- 建物: `"height"` メートルで `<model>.json`. Kenshi人々は約1.8メートルの高さで、実際の世界の大きさのほかに小さく読むので、
あなたが描いたサイズで、20%を追加します(2.2mの投稿が表示されます。 `2.64`).
- アイテム: `"size"`最も長い側面は、メートルで、 `<model>.json` そのほか、 `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. テキストをまとめる

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

これは、複数のピクセル(建物のための1024)で、通常、数十MBから数分に分けられます。 選択肢の3つ目は、設定ファイルが望ましくない場合、最も長い側面にモデルをリリースします。 さらに、半分のコンテンツが大きくなります(`textureSize`)ですが、削減ファイルを処理すると、リポが小さくなります。

走る `model-info.py` 結果を再確認する。

<a id="5-import-and-check"></a>
## 5. 輸入・チェック

作り出し、作り出して、作り出す。 [トップページ > スピードスタート](structures.md#quick-start)). ロゴにはモデルごとに1行があります:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

長さや三角形を調べると、そのまま、建物を置く。

<a id="6-commit"></a>
## 6. コミット

コミットする `.glb`彼女の `.json` そして、フォルダーの `ids.json` 一緒に。 `ids.json` それぞれのモデルの記録IDを固定し、そのIDによって置かれた建物の名前を保存します。
