<a id="preparing-a-model"></a>
# 调整模型面数、尺寸和纹理

在生成的 `.glb` （ Meshy 、 Hi3D 、 Blender ）进入 `<Mod>/Structures/` （建筑物）或 `<Mod>/ItemModels/` （物品）之前，如何将其获取到游戏处理的多边形数、纹理大小和真实世界大小。导入本身： [structurals.md](structures.md) 。

命令从 PowerShell 中的存储库根运行。需要Python 3； `prepare-model.py` 还需要 `pip install pillow numpy` 。

<a id="1-inspect"></a>
## 1. 检查

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

它打印以米为单位的尺寸（X、Y 向上、Z）、最长边、三角形、材质以及每个纹理的像素尺寸和 MB 。关于节点缩放或旋转的注释意味着 `prepare-model.py` 无法重新缩放它：在应用变换的情况下再次从 Blender 导出它（对象 > 应用 > 所有变换）。

<a id="2-cut-the-polycount"></a>
## 2. 减少多边形数量

| 种类 | 三角形 | 纹理 |
| --- | --- | --- |
| 建筑或装饰 | 5,000 至 20,000 | 2048 |
| 物品（持有、掉落、在商店中） | 5,000以下 | 1024 |

生成器往往给出10万或更多；进口警告超过15万。切割它：

- **Meshy ：**使用上述预算中的目标多边形数重新网格化模型（四边形或三角形拓扑都可以），保留纹理，然后再次将其下载为 GLB 。
- **Blender ：**文件 > 导入 > glTF，选择网格，添加抽取修改器（折叠）并降低比率，直到状态栏中的面数在预算内，应用它，然后文件 > 导出 > glTF 二进制 (`.glb`) 并启用“应用修改器”。

碰撞被单独简化（ `collisionTriangles` ，默认 2000），因此切割可见网格与帧速率和文件大小有关。

<a id="3-choose-the-size"></a>
## 3. 选择尺寸

导入将模型视为米；生成器的大小是任意的。选择实际尺寸并将其设置在模型的设置文件（[structurals.md "Settings"](structures.md#settings) ）中，而不是在模型中：

- 建筑物： `"height"` （以米为单位） `<model>.json` 。 Kenshi 的人身高约为 1.8 m，阅读起来比实际尺寸小，因此请采用您描述的尺寸并添加 20%（2.2 m 的柱子变成 `2.64` ）。
- 项目： `"size"` ，其最长边以米为单位，在 `<model>.json` 中，旁边是 `ItemModels/` 。

<a id="4-shrink-the-textures"></a>
## 4. 缩小纹理

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

这会重写文件，每个纹理每边最多有那么多像素（建筑物为 2048 个，物品为 1024 个），通常将其从几十个 MB 削减到几个。当不需要设置文件时，可选的第三个参数还将模型重新缩放到最长的边（以米为单位）。导入也会将更大的纹理减半（ `textureSize` ），但提交缩小的文件会使存储库保持较小。

再次运行 `model-info.py` 查看结果。

<a id="5-import-and-check"></a>
## 5.导入并检查

使用 Play 构建和部署（或单独运行导入，[structurals.md“快速启动”](structures.md#quick-start) ]）。每个模型的日志只有一行：

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

检查高度或长度以及三角形数量，然后在游戏中查看：放置建筑物，或放下并拾起物品。

<a id="6-commit"></a>
## 6. 承诺

将 `.glb` 、其 `.json` 和文件夹的 `ids.json` 一起提交。 `ids.json` 保持每个模型的记录 ID 固定，并根据这些 ID 保存放置的建筑物的名称。

[English](../../../docs/model-prep.md) · [Kenshi Races](../README.md)
