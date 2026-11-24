<a id="structures-from-3d-models"></a>
# 由 3D 模型的结构

把A `.glb` 模型(Meshy, Hi3D,一个混合器出口) 在您的模式下 `Structures` 文件夹和游戏构建将其转化为您可以从构建菜单中放置的建筑物:Ogre mesh,压缩的结构,碰撞和记录 `<Mod>.mod`. 没有 FCS没有混合器,没有游戏安装需要进口本身。

<a id="quick-start"></a>
## 快速启动

1. 把模型放在 `<Mod>/Structures/`是的,G `MyMod/Structures/watch-tower.glb`.
2. 可选:除此之外, `watch-tower.json` 与任何 [设置](#settings)是的,G

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. 如往常( )[play.md](play.md)):Visual Studio F5,VS代码 F5,或 `dotnet run --project Play`. 创建日志显示一个
`Structure ...` 按模型线。
4. 在游戏中:构建菜单,类别 `EXTERIOR` (或你所设立的) 与假设相同 `"research": "start"` 从一开始就知道了。
游戏,以及现有储蓄(Meitou工作从完成的研究,总是包括 `_Default Start`).
5. 采用模型,其 `.json` 和 `Structures/ids.json` (重定向自第一条) 创建的文件是创建输出,并且不
承诺。

没有玩游戏: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## 设置

可选(`<model>.json`,评论允许;一个未知的名字是错误,所以类型不会被忽略)。

| 设置 | 假设 | 意思 |
| --- | --- | --- |
| `name` | 文件名称, `-`/`_` 作为空间 | 创建菜单中的名称 |
| `description` | `""` | 创建菜单文本 |
| `height` | 模型的尺寸 | 完成高度在米;模型平等地扩展到它。 AI模型到达自愿的尺寸,所以设置这个 |
| `sink` | `0` | 地面下滑的尺寸(阻止它在滑板上浮动) |
| `yaw` | `0` | 等级转向垂直轴 |
| `category` | `EXTERIOR` | 菜单类别(`building category`( ) : 遊戲的( )`BUILDINGS`, `CAMPING`, `FARMING`( )或新名字 |
| `group` | `""` | `building group`: 建筑共享一个是一个输入的变量 |
| `materials` | `5` | 建造材料,成本 |
| `buildSpeed` | `2` | `build speed mult` (游戏的帐篷5、房屋1到2) |
| `research` | `start` | `start`此分類上一篇: 從一開始就知道 `none`沒有任何人會把它(你的自己的記錄做),或一個 RESEARCH 字符串 ID 等 `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` 或 `both` |
| `createsTown` | `true` | 把它放在一个城市外面,开始了一个出点。 |
| `collision` | `mesh` | `mesh` (简化模型的副本), `box` (是边界盒子)或 `none` (重定向之) |
| `collisionTriangles` | `2000` | 大多数三角形 `mesh` 冲突持续(12至50万) |
| `textureSize` | `2048` | 最大的纺织侧在像素(两个功率,最多为8192) |

<a id="item-models"></a>
## 项目模型

将一个项目的模型插入 `<Mod>/ItemModels/` 相反(`Structures` 它会变成一座建筑。 进口写着它的墨和结构,除了结构和一个 MATERIAL_SPECS_CLOTHING 上一篇: 你的MOD ITEM 记录后,他们就点了点。 创建日志显示一个 `Item ...` 按模型使用的名称。

1. `MyMod/ItemModels/lantern.glb`可选的 `lantern.json`: `{ "size": 0.3 }`.
2. 在您的项目记录(数据发明器或数据阶段)中,设置:

的值  的价值
   | --- | --- |
| `mesh` 和 `ground mesh` () `<mod>_<model>.mesh`是的,G `mymod_lantern.mesh` | | `material` (重定向) 雅 `<950000 + 100 × block>-<Mod>.mod`区块从 `ItemModels/ids.json`是的,G `950000-MyMod.mod` |

首先,请参见(`GameDatabase.Find`保持停留时的外观:发放的ZIP,并且没有部署模式的测试,没有它。
3. 采用模型,其 `.json` 和 `ItemModels/ids.json`.

| 设置 | 假设 | 意思 |
| --- | --- | --- |
| `name` | 文件名称, `-`/`_` 作为空间 | 物质记录的名称 |
| `size` | 模型的尺寸 | 最长的侧面在米;模型平等于它 |
| `yaw` | `0` | 等级转向垂直轴 |
| `textureSize` | `1024` | 最大的纺织侧在像素(两个功率,最多为8192) |

- 每件物品的模型(由 ITEM 名字一 `material`);模型站在其最低点,集中,因为游戏的项目板。
- 遊戲將一個項目放在地面上,在一隻手中,並在一個自動圖標上。 `mesh` (`ground mesh` 首先,在地面上,
MATERIAL_SPECS_CLOTHING 他的 `material` 发动机名称 `docs/formats/runtime-materials.md` “呼叫” `WorldObjectLayout.ItemMesh`).
- 存储图标从模型中提取,除非记录设置 `auto icon image` 虚假和自己的 `icon`.

<a id="getting-a-good-model"></a>
## 格拉格好模型

- 问答 Meshy 或 Hi3D 对于A PBR编写模型并下载它作为 GLB. 几十万个三角形是丰富的;日志旗帜模型
超過150萬 切割面板,选择尺寸,缩短结构: [model-prep.md](model-prep.md).
- 图为图为 + Z 图为 + 图为glTF条约;使用和 `yaw` 要转身。
- 每个物质都变成了自己的部分。 发电机通常提供一个;最多为49个。
- 模型站在其最低点,中心。

<a id="what-it-writes"></a>
## 它所写的

将其转换为“MOD”文件(`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` (每件物品, `_1`, `_2`,......当多人):Ogre `[MeshSerializer_v1.8]`,位置,正常,UV和
与手动性在w,游戏自己的建筑板的布局。
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: 分散(BC3),正常(BC1,BC3切割时),金属性(BC1),与粘膜。
- `structures/<mod>_<model>_col.xml`答:碰撞。
- 对于一个项目模型: `structures/<mod>_<model>.mesh` 它的结构,没有冲突。
- 记录合并 `<Mod>.mod` 除其中的任何东西外,将其列入(数据发电机的记录被保留;以前的进口被取代):
BUILDING一、一 BUILDING_PART 和 MATERIAL_SPEC 材料,以及开锁(研究的 `enable buildings` 获胜的建筑物;一 MATERIAL_SPECS_CLOTHING 根据项目模型。

檔案名稱開始於模式的名稱,因為遊戲的資源在每個模式上都會出現。

记录 ID 是 `<n>-<Mod>.mod` 由 n 到 900000    加 加  加 一 块 每 模型 (构建 +0, 零件 +1, 材料 +50); 物品 模型 数 从 950000  加 (材料 +0) 在 `ItemModels/ids.json`. `Structures/ids.json` 保留每个模型的区块,以便标识符永远不会改变:一个保存器以其记录标识符命名一个定位的建筑。 永远不要编辑或删除一个区块;删除一个模型将其从游戏中删除。

<a id="facts-the-import-relies-on"></a>
## 事实上,进口依赖于

对游戏的文件进行检查(Kenshi 1.0.68 数据) 和发动机的读者:

- **单位**: 世界单位为 0.1 m, Y up; glTF 是尺寸,Y上,所以尺寸是10(游戏的碰撞文件使用重力98.1)。
- **材料**一个建筑物的外观是它的 MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`不是梅西的
材料名称(机械 `docs/formats/runtime-materials.md`). 游戏的游戏( )`data/materials/deferred/objects.hlsl`阅读分散 RGB 如色 A 如光(时光) `specular mult`( ) 正常 RGB 如同Tankt-space正常,A如切断面具(`material type` 1, `alpha threshold`) ,金属从R。 进口字符格拉斯 = 1 - glTF 金属和金属从 glTF蓝色频道; glTF MASK 和 BLEND 成为一个切割。
- **坦克**其形状的阴影是交叉(紧张,正常) × w,在游戏中,它指向沿着增加V(**已验证**:
`prisoner_pole.mesh` 86个三角形 `Camp_tent1.mesh` 1858年至1865年。 glTF 正常的地图是绿色的图像(减少V),所以进口转向绿色频道,并设置游戏的方式。
- **碰撞**答:一部分 `xml collision` 是 PhysX NxuStream2 文件; 手写的最小文件加载(游戏的 `Camp_tent1_COL.xml` 是
一个盒子) PhysX 是 Z 和游戏地图(x,y,z)到(x,z, -y),所以进口写的部分点为(x, -z,y)。 三角形按钮进入平面点和三角形;游戏在负载上煮熟,梅图阅读它如同(发动机) `docs/formats/collision.md`).
- **田野**: BUILDING 和 BUILDING_PART 带到游戏的Camper 地板的完整集(`18026-small_changes_otto.mod`因此,没有什么
它是虚假的;不 `functionality`因此,它是一个平坦的建筑。
- **解锁**建筑物在建筑菜单中,一旦完成的研究列出它 `enable buildings`每场比赛都开始了
`_Default Start` (`5359-gamedata.base`( )结束了。 **已验证** 与发动机的 `GameDatabase`, `ResearchGate` 和 `BuildingBuildData` 基本游戏加上进口模式:结构被解锁,建筑材料按设置。

<a id="not-covered-yet"></a>
## 尚未覆盖

室内、门、功能(存储、床、制作)和旋转部件。
