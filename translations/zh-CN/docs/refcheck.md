<a id="engine-references-refcheck"></a>
# 引擎参考(RefCheck)

一个模式是建立在一个发动机(这个 repo 的 `engine` 它是由任何人发放的,它是由玩家发放的 當一個發行器改變了 mod 呼叫的簽名時, mod 的來源可能仍然是複製的。 DLL 它已经建造的,所以要求游戏为老成员,并在运行时间失败。 `MissingMethodException`.

这不是假设的。 米特 0.21 交付 `Inventory.Add(ItemInstance)` 二、选择性分数(`Add(ItemInstance, bool mainShut = false)`). 源头呼唤 `bag.Add(item)` 相反的版本,但 DLL 与 0.20 或更高版本相反,称为单参数方法,该方法不再有 0.21。 在The Wasteland 记得打破了游戏,而例外则停止了整个系统。 没有测试:测试对一个发动机进行编辑。

<a id="what-it-checks"></a>
## 什么是检查

`tools/RefCheck` 阅读一个构建的模式 DLL它的代数据和列出它参考的每个发动机成员(方法,构建器,字段的类型 `Meitou*.dll`根据名称和参数类型,该版本没有。 没有任何模式或游戏运行。

`tools/refcheck.sh` 构建每个模式在 repo (每个顶级文件夹与 `meitou.json` 和 `<Folder>/<Folder>.csproj`)下载一个版本的win-x64 zip 一次(只保持 `Meitou*.dll` 和 `0Harmony.dll`)并检查每个集合的模式 `meitou.json` 列表

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

出行,每行发行和组合:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

或者,在休息时:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## 在我

是的, `Engine references` 工作跑步 `tools/refcheck.sh` 反对最新发布的每一个PR和压力,以及 `Release readiness` 等待它。 一条船,一条船 DLL 但是,对于一个老发动机来说,这也应该是 DLL 每個解放的 `meitou.json` 允许。

<a id="when-it-finds-something"></a>
## 当他发现某种

呼叫会员的方式,每一个你支持的发动机:如果你放弃旧的,或者找到会员,并填补的参数,你不会通过他们的假设。 “荒野记得” `InventoryCompat` 后者为 `Inventory.Add`.

发动机的反射呼叫不被检查:它们在你写的位置轻松失败。
