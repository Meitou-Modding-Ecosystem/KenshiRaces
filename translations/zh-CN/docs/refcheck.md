<a id="engine-references-refcheck"></a>
# 检查与新版Meitou的兼容性

Mod 是针对一个引擎（此存储库的 `engine` 子模块）构建的，并在玩家拥有的任何 Meitou 版本上运行。当版本更改 mod 调用的某些内容的签名时，mod 的源代码仍可能编译，但已构建的 DLL 会向游戏请求旧成员，并在运行时失败并显示 `MissingMethodException` 。

这不是假设。 Meitou 0.21 为 `Inventory.Add(ItemInstance)` 提供了第二个可选参数 (`Add(ItemInstance, bool mainShut = false)`)。调用 `bag.Add(item)` 的源代码针对任一版本进行编译，但针对 0.20 或更早版本构建的 DLL 调用单参数方法，而 0.21 不再具有该方法。在《荒原记忆》中，破坏了游戏中的纪念品，并且该异常在会话的其余部分中停止了整个系统。没有测试发现它：测试针对一个引擎进行编译。

<a id="what-it-checks"></a>
## 它检查什么

`tools/RefCheck` 读取构建的 mod DLL 的元数据，并按给定版本所没有的名称和参数类型列出它引用的每个引擎成员（方法、构造函数、 `Meitou*.dll` 中类型的字段）。模组或游戏均无法运行。

`tools/refcheck.sh` 构建存储库中的每个 mod（每个顶级文件夹均包含 `meitou.json` 和 `<Folder>/<Folder>.csproj` ），下载一次发行版的 win-x64 zip（仅保留 `Meitou*.dll` 和 `0Harmony.dll` ）并检查 mod 的 `meitou.json` 列表中的每个程序集。

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

输出，每次发布和组装一行：

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

或者，休息一下：

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## 在 CI

`Engine references` 作业针对每个 PR 上的最新版本运行 `tools/refcheck.sh` 并推送， `Release readiness` 等待它。发布针对较旧引擎构建的 DLL 的 mod 还应该检查 DLL 是否针对其 `meitou.json` 允许的每个版本。

<a id="when-it-finds-something"></a>
## 当它发现某物时

以适用于您支持的每个版本的方式调用成员：如果您删除旧引擎，则针对较新的引擎进行重建，或者通过反射找到成员并用默认值填充您未传递的参数。 The Wasteland Remembers 的 `InventoryCompat` 为 `Inventory.Add` 做了后者。

对引擎的反射调用不会被检查：它们会在您写入它们的地方轻微失败。

[English](../../../docs/refcheck.md) · [Kenshi Races](../README.md)
