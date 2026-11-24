<a id="preparing-a-model"></a>
# ポリゴン数、寸法、テクスチャを調整する

生成された `.glb` (Meshy 、 Hi3D 、 Blender) を、 `<Mod>/Structures/` (建物) または `<Mod>/ItemModels/` (アイテム) に移す前に、ゲームが処理するポリカウント、テクスチャ サイズ、現実世界のサイズに合わせる方法。インポート自体: [structurals.md](structures.md).

コマンドは PowerShell のリポジトリ ルートから実行されます。 Python 3 が必要です。 `prepare-model.py` には `pip install pillow numpy` も必要です。

<a id="1-inspect"></a>
## 1. 検査する

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

メートル単位のサイズ (X、Y 上、Z)、最長辺、三角形、マテリアル、各テクスチャのピクセル サイズと MB を出力します。ノードのスケールまたは回転に関する注意事項は、 `prepare-model.py` では再スケールできないことを意味します。変換を適用した状態で Blender から再度エクスポートします ([オブジェクト] > [適用] > [すべての変換])。

<a id="2-cut-the-polycount"></a>
## 2.ポリカウントをカットする

| 親切 | 三角形 | テクスチャ |
| --- | --- | --- |
| 建物や装飾品 | 5,000～20,000 | 2048 |
| アイテム（ショップ内で保持、ドロップ） | 5,000未満 | 1024 |

ジェネレーターは 100,000 以上を与えることがよくあります。インポートは 150,000 を超えると警告します。カットするには:

- **Meshy :**テクスチャを維持しながら、上記の予算内のターゲット ポリ数でモデルを再メッシュし (クワッドまたはトライアングル トポロジの両方が機能します)、それを GLB として再度ダウンロードします。
- **Blender :**ファイル > インポート > glTF でメッシュを選択し、デシメート モディファイア (縮小) を追加し、ステータス バーの面数が予算内になるまで比率を下げて適用し、次にファイル > エクスポート > glTF バイナリ (`.glb`) を適用してモディファイアをオンにします。

衝突は個別に簡素化されるため (`collisionTriangles` 、デフォルト 2000)、表示されるメッシュのカットはフレーム レートとファイル サイズに関係します。

<a id="3-choose-the-size"></a>
## 3. サイズを選択してください

インポートではモデルをメートルとして扱います。ジェネレーターのサイズは任意です。実際のサイズを選択し、モデル内ではなく、モデルの設定ファイル ([structurals.md "Settings"](structures.md#settings)) に設定します。

- 建物: `"height"` (メートル単位)、 `<model>.json` 。 Kenshi の人々の身長は約 1.8 m で、実際のサイズよりも小さいため、説明するサイズに 20% を追加します (2.2 m のポストは `2.64` になります)。
- アイテム: `"size"` 、その最長辺 (メートル)、 `<model>.json` の隣に `ItemModels/` 。

<a id="4-shrink-the-textures"></a>
## 4. テクスチャを縮小する

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

これにより、最大で各辺のピクセル数 (建物の場合は 2048、アイテムの場合は 1024) のすべてのテクスチャを使用してファイルが所定の位置に書き換えられ、通常は数十 MB から数ピクセルに削減されます。オプションの 3 番目の引数は、設定ファイルが必要ない場合に備えて、モデルの最長辺をメートル単位で再スケールします。インポートにより大きなテクスチャも半分になります (`textureSize`) が、縮小されたファイルをコミットするとリポジトリは小さく保たれます。

再度 `model-info.py` を実行して結果を確認します。

<a id="5-import-and-check"></a>
## 5.インポートして確認する

Play を使用してビルドしてデプロイします (またはインポートを単独で実行します ([structurals.md "Quick start"](structures.md#quick-start)))。ログにはモデルごとに 1 行が含まれます。

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

高さや長さ、三角形の数を確認してから、ゲーム内で建物を配置したり、アイテムを落としたり拾ったりしてください。

<a id="6-commit"></a>
## 6.コミット

`.glb` 、その `.json` 、およびフォルダーの `ids.json` を一緒にコミットします。 `ids.json` は各モデルのレコード ID を固定し、その ID で配置された建物の名前を保存します。

[English](../../../docs/model-prep.md) · [Kenshi Races](../README.md)
