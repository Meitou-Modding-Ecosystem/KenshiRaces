<a id="engine-references-refcheck"></a>
# エンジンリファレンス ( RefCheck )

MOD は 1 つのエンジン (このリポジトリの `engine` サブモジュール) に対してビルドされ、プレイヤーが所有する Meitou リリースで再生されます。リリースで MOD が呼び出すもののシグネチャが変更された場合、MOD のソースは引き続きコンパイルされる可能性がありますが、すでにビルドされている DLL はゲームに古いメンバーを要求し、実行時に `MissingMethodException` で失敗します。

これは仮説ではありません。 Meitou 0.21 では、 `Inventory.Add(ItemInstance)` に 2 番目のオプションのパラメーター ( `Add(ItemInstance, bool mainShut = false)` ) が与えられました。 `bag.Add(item)` を呼び出すソースはどちらのバージョンに対してもコンパイルされますが、0.20 以前に対してビルドされた DLL は 1 パラメーター メソッドを呼び出しますが、0.21 にはこのメソッドは存在しません。 「The Wasteland」では、プレイ中の記念品が壊れたことを覚えており、例外により残りのセッションでシステム全体が停止しました。テストはそれを捕捉しませんでした。テストは 1 つのエンジンに対してコンパイルされます。

<a id="what-it-checks"></a>
## チェック内容

`tools/RefCheck` は、ビルドされた mod DLL のメタデータを読み取り、指定されたリリースにはない、参照するすべてのエンジン メンバー (メソッド、コンストラクター、 `Meitou*.dll` の型のフィールド) を名前とパラメーターの型ごとにリストします。 MODもゲームも何も実行されません。

`tools/refcheck.sh` は、リポジトリ内のすべての MOD ( `meitou.json` および `<Folder>/<Folder>.csproj` を含む各最上位フォルダー) をビルドし、リリースの win-x64 zip を 1 回ダウンロードし ( `Meitou*.dll` および `0Harmony.dll` のみを保持)、MOD の `meitou.json` リストの各アセンブリをチェックします。

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

出力、リリースおよびアセンブリごとに 1 行:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

または、休憩のために:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## CI で

`Engine references` ジョブは、 PR およびプッシュごとに最新リリースに対して `tools/refcheck.sh` を実行し、 `Release readiness` はそれを待ちます。古いエンジンに対してビルドされた DLL を出荷する MOD は、 `meitou.json` が許可する各リリースに対して DLL をチェックする必要もあります。

<a id="when-it-finds-something"></a>
## 何かを見つけたとき

サポートするすべてのリリースで機能する方法でメンバーを呼び出します。古いエンジンを削除した場合は新しいエンジンに対して再構築するか、リフレクションによってメンバーを見つけて、デフォルトで渡さないパラメーターを入力します。 The Wasteland Remembers の `InventoryCompat` は、 `Inventory.Add` に対して後者を実行します。

エンジンへのリフレクション呼び出しはチェックされません。呼び出しは、書き込み先でソフトに失敗します。
