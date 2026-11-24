<a id="engine-references-refcheck"></a>
# エンジンリファレンス(RefCheck)

モードは1つのエンジンに対して構築されます(このレポの `engine` サブモジュール)とプレイするすべてのメイトは、プレイヤーが持っている。 リリースがモードのサインを変更する場合、モードのソースはまだ作成されます。 DLL すでに構築されているので、旧メンバーのためのゲームを要求し、時間通りに失敗します。 `MissingMethodException`.

これは仮説ではない。 メイトゥー 0.21 プレゼント `Inventory.Add(ItemInstance)` 二つ目は、選択肢(`Add(ItemInstance, bool mainShut = false)`). 呼びかけた源泉 `bag.Add(item)` どちらかというと、バージョンで、 DLL 0.20 またはそれ以上に構築された1つのパラメーターの方法を呼び出し、0.21 はもはや持っていません。 The Wasteland Remembers は、ゲーム内のメンテナンスを破り、その例外は、セッションの残りまで全体を止めました。 テストは一つずつ作られていません。

<a id="what-it-checks"></a>
## 何をチェックするか

`tools/RefCheck` 建てられたモードを読む DLLメタデータとリストは、それぞれのエンジンメンバーが参照する(方法、構築者、タイプのフィールド) `Meitou*.dll`名前やパラメーターの種類によって、特定のリリースがありません。 モードやゲームは動かない。

`tools/refcheck.sh` レポの各モードを構築する(各トップレベルのフォルダーを `meitou.json` そして、 `<Folder>/<Folder>.csproj`)、リリースのWin-x64 ZIPを1回ダウンロードします(残るだけ) `Meitou*.dll` そして、 `0Harmony.dll`各モードをチェックする `meitou.json` リスト

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

リリース・リリース1回目と会議1回目:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

あるいは、休憩のため:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## こちらで

☆☆☆☆ `Engine references` 仕事の走り `tools/refcheck.sh` 最新のリリースに対して、すべてのPRとプレッシャー、および `Release readiness` 待ってます。 船を運ぶモード DLL 古いエンジンに基づいて、その DLL それぞれの解放について `meitou.json` 許される。

<a id="when-it-finds-something"></a>
## 何かを見つけたら

あなたがサポートするすべてのリリースで働く方法でメンバーを呼び出す:あなたが古いエンジンに反対する場合、またはメンバーを見つけることによって、あなたは彼らのデフォルトを通過しないパラメーターを満たす。 砂漠は思い出す」 `InventoryCompat` 後者は、 `Inventory.Add`.

エンジンへの反射通話はチェックされていません:あなたがそれらを書いた場所に柔らかく失敗します。
