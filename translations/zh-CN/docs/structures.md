<a id="structures-from-3d-models"></a>
# 3D 模型的结构

将 `.glb` 模型（ Meshy 、 Hi3D 、 Blender 导出）放入 mod 的 `Structures` 文件夹中，游戏构建会将其变成可以从构建菜单中放置的建筑物：Ogre 网格、压缩纹理、碰撞和 `<Mod>.mod` 中的记录。没有 FCS ，没有 Blender ，导入本身不需要安装游戏。

<a id="quick-start"></a>
## 快速启动

1. 将模型放入 `<Mod>/Structures/` 中，例如 `MyMod/Structures/watch-tower.glb` 。
2. 可选：除此之外， `watch-tower.json` 具有任何[设置](#settings) ，例如

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. 像往常一样构建和部署（[play.md](play.md) ）： Visual Studio F5、 VS Code F5 或 `dotnet run --project Play` 。构建日志显示每个模型一条 `Structure ...` 行。
4. 在游戏中：构建菜单，类别 `EXTERIOR` （或您设置的类别）。使用默认的 `"research": "start"` ，它从游戏一开始就已知，并且在现有的保存中也是已知的（ Meitou 从完成的研究中得出解锁，其中始终包括 `_Default Start` ）。
5. 提交模型，其 `.json` 和 `Structures/ids.json` （写在第一次导入时）。生成的文件是构建输出并且未提交。

不玩： `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>` 。

<a id="settings"></a>
## 设置

全部可选（ `<model>.json` ，允许注释；未知名称是一个错误，因此拼写错误不会被忽视）。

| 环境 | 默认 | 意义 |
| --- | --- | --- |
| `name` | 文件名， `-` / `_` 为空格 | 构建菜单中的名称 |
| `description` | `""` | 构建菜单文本 |
| `height` | 模特本身的尺寸 | 成品高度（米）；模型均匀地缩放到它。 AI 型号有任意尺寸，因此请设置此 |
| `sink` | `0` | 沉入地面的米数（防止其漂浮在斜坡上） |
| `yaw` | `0` | 绕垂直轴转动的度数 |
| `category` | `EXTERIOR` | 构建菜单类别（ `building category` ）：游戏的（ `BUILDINGS` ， `CAMPING` ， `FARMING` ，...）或新名称 |
| `group` | `""` | `building group` ：共享一个的建筑物是一个条目的变体 |
| `materials` | `5` | 建筑材料成本 |
| `buildSpeed` | `2` | `build speed mult` （游戏中的帐篷5个，房屋1到2个） |
| `research` | `start` | `start` ：从一开始就知道； `none` ：没有任何东西可以解锁它（你自己的记录可以）；或 RESEARCH 字符串 ID，例如 `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor` 、 `indoor` 或 `both` |
| `createsTown` | `true` | 将其放置在城镇外即可启动前哨站 |
| `collision` | `mesh` | `mesh` （模型的简化副本）、 `box` （其边界框）或 `none` （遍历它） |
| `collisionTriangles` | `2000` | `mesh` 碰撞保留的最多三角形（12 到 50,000） |
| `textureSize` | `2048` | 最大纹理边（以像素为单位）（2 的幂，最多 8192） |

<a id="item-models"></a>
## 项目型号

将物品的模型放入 `<Mod>/ItemModels/` 中（ `Structures` 会使其成为建筑物）。导入将其网格和纹理写入结构和一条 MATERIAL_SPECS_CLOTHING 记录旁边；然后你的 mod 的 ITEM 记录指向它们。构建日志显示每个模型一行 `Item ...` 以及要使用的名称。

1. `MyMod/ItemModels/lantern.glb` ，可选 `lantern.json` ： `{ "size": 0.3 }` 。
2. 在您的项目记录（数据生成器或数据阶段）中，设置：

|领域 |价值|
   | --- | --- |
| `mesh` 和 `ground mesh` （文件名）| `<mod>_<model>.mesh` ，例如 `mymod_lantern.mesh` | | `material` （参考）| `<950000 + 100 × block>-<Mod>.mod` ，来自 `ItemModels/ids.json` 的块，例如 `950000-MyMod.mod` |

首先查找材料（ `GameDatabase.Find` ），并在丢失时保持替身的外观：已发布的 zip，以及在没有部署的 mod 的情况下读取游戏的测试，没有它。
3. 提交模型，其 `.json` 和 `ItemModels/ids.json` 。

| 环境 | 默认 | 意义 |
| --- | --- | --- |
| `name` | 文件名， `-` / `_` 为空格 | 物料记录名称 |
| `size` | 模特本身的尺寸 | 最长边以米为单位；模型均匀地缩放到它 |
| `yaw` | `0` | 绕垂直轴转动的度数 |
| `textureSize` | `1024` | 最大纹理边（以像素为单位）（2 的幂，最多 8192） |

- 每个项目模型一种材料（ ITEM 命名为 `material` ）；模型站在最低点，居中，就像游戏的物品网格一样。
- 游戏在地上、手上和自动图标中绘制一个物品，其名称为 `mesh` （ `ground mesh` 首先在地面上）， MATERIAL_SPECS_CLOTHING 的名称为 `material` （引擎 `docs/formats/runtime-materials.md` “Callers”、 `WorldObjectLayout.ItemMesh` ）。
- 库存图标是从模型中绘制的，除非记录设置 `auto icon image` false 及其自己的 `icon` 。

<a id="getting-a-good-model"></a>
## 获得一个好的模型

- 向 Meshy 或 Hi3D 索要 PBR 纹理模型，并将其下载为 GLB 。几万个三角形就足够了；日志标记模型超过 150,000。切割多边形，选择尺寸并缩小纹理：[model-prep.md](model-prep.md) 。
- 文件中模型正面面向 +Z（glTF 约定）；使用 `yaw` 来转动它。
- 每种材料都成为自己的一部分。生成器通常给出一个；最多49个。
- 该模型位于最低点，居中。

<a id="what-it-writes"></a>
## 它写了什么

进入已部署的 mod 文件夹（ `<Kenshi>/mods/<Mod>/` ）：

- `structures/<mod>_<model>.mesh` （每种材质一个， `_1` ， `_2` ，...当有多个时）：Ogre `[MeshSerializer_v1.8]` ，位置，法线， UV 以及与 w 中的旋手性相切，游戏自己的建筑网格体的布局。
- `structures/<mod>_<model>_d.dds` 、 `_n.dds` 、 `_m.dds` ：漫反射 (BC3)、正常（BC1、剪切时的 BC3）、金属度 (BC1)、带 mips。
- `structures/<mod>_<model>_col.xml` ：碰撞。
- 对于物品模型： `structures/<mod>_<model>.mesh` 及其纹理，没有碰撞。
- 记录与其中的其他内容一起合并到 `<Mod>.mod` 中（保留数据生成器的记录；替换较早的导入记录）：一个 BUILDING 、每种材料一个 BUILDING_PART 和 MATERIAL_SPEC 以及解锁（研究的 `enable buildings` 获得建筑物）；每个型号一个 MATERIAL_SPECS_CLOTHING 。

文件名以模组名称开头，因为游戏通过每个模组的裸文件名查找资源。

记录ID为 `<n>-<Mod>.mod` ，n从900000以上，每个模型100个块（建筑+0，零件+1，材料+50）； `ItemModels/ids.json` 中的物品模型从 950000（材料+0）开始计数。 `Structures/ids.json` 保留每个模型的块，因此 id 永远不会改变：保存时通过其记录的 id 命名已放置的建筑物。切勿编辑或删除块；删除模型会从放置它的游戏中删除其建筑。

<a id="facts-the-import-relies-on"></a>
## 进口依赖的事实

对照游戏文件（ Kenshi 1.0.68 数据）和引擎读取器进行检查：

- **单位**：世界单位为0.1 m，Y向上； glTF 为米，Y 向上，因此比例为 10（游戏的碰撞文件使用重力 98.1）。
- **材质**：建筑部件的外观是其 MATERIAL_SPEC （ `texture map` 、 `normal map` 、 `metalness map` 、 `material type` ），而不是网格物体的材质名称（引擎 `docs/formats/runtime-materials.md` ）。游戏的建筑着色器 ( `data/materials/deferred/objects.hlsl` ) 将漫反射 RGB 读取为颜色，将 A 读取为光泽度（乘以 `specular mult` ），将法线 RGB 读取为切线空间法线，将 A 读取为剪切蒙版 ( `material type` 1, `alpha threshold` )，金属度来自 R。导入写入光泽度 = 1 - glTF 粗糙度和金属度（来自 glTF 的蓝色通道）； glTF MASK 和 BLEND 成为切口。
- **切线**：着色器的副法线为 cross(切线，法线) × w，在游戏的网格上，它沿着递增的 V 指向（**已验证**： `prisoner_pole.mesh` 86 of 86 个三角形， `Camp_tent1.mesh` 1858 of 1865）。 glTF 法线贴图将图像指向绿色（减小 V），因此导入会转动绿色通道并设置游戏的方式。
- **碰撞**：零件的 `xml collision` 是PhysX NxuStream2文件；手写的最小文件加载（游戏的 `Camp_tent1_COL.xml` 是一个盒子）。 PhysX 是 Z 向上，游戏将 (x, y, z) 映射到 (x, z, -y)，因此导入将零件的点写入为 (x, -z, y)。三角形网格以普通点和三角形的形式出现；游戏在加载时烹饪它， Meitou 按原样读取它（引擎 `docs/formats/collision.md` ）。
- **场地**： BUILDING 和 BUILDING_PART 携带游戏露营帐篷 1 ( `18026-small_changes_otto.mod` ) 的全套场地，因此不会出现默认情况；没有 `functionality` ，所以它是一座普通的建筑。
- **解锁**：一旦完成的研究将建筑物列在 `enable buildings` 中，建筑物就会出现在构建菜单中；每场比赛都以 `_Default Start` ( `5359-gamedata.base` ) 结束开始。 **在基础游戏上使用引擎的 `GameDatabase` 、 `ResearchGate` 和 `BuildingBuildData` 以及导入的模组进行**验证**：结构已解锁并按设置消耗建筑材料。

<a id="not-covered-yet"></a>
## 尚未覆盖

内饰、门、功能（存储、床、工艺）和转动部件。
