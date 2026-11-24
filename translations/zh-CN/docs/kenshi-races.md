<a id="kenshi-races"></a>
# Kenshi Races

角色编辑器的三个新种族，每个种族都有子种族，位于游戏的人体骨骼上（ `kenshi.races` ，无依赖性）：加载时添加的每个记录，身体由 [race import](race-import.md) ] 安装。它还为这些骨架上的每场比赛提供了一个包含 59 个剪辑的动画库](animations.md) 。模组使用种族和剪辑，添加子种族或添加自己的种族：[depending-on-kenshi-races.md](depending-on-kenshi-races.md) 。

<a id="races"></a>
## 可玩种族

编辑器将每个 RACE_GROUP 显示为一个种族，其中的 RACE 记录显示为亚种。

| 种族 | 亚种 | 身体模型 | 角色 |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | 男性、女性 | leomamale `11-`, leomafemale `17-` |
| Crevan `31-` | crevan `18-` | 男性、女性 | crevanmale `19-`, crevanfemale `21-` |
| Crabfolk `32-` | crabman `12-` | 单一 | crabman `13-` |
|  | crabqueen `14-` | 单一 | crabqueen `15-` |

Id 以 `KenshiRaces.mod` 结尾。 Subrace 和 CHARACTER 名称是一个小写单词，因此控制台的 `spawn` 将它们不带引号。每个主体的文件都是 `KenshiRaces/items/races/kenshiraces_<character>*` （ Leoma 和 Crevan 上的法线贴图）。公头适用于 `male_skeleton` ，母头适用于 `female_skeleton` ；螃蟹的身体是男性的，所以螃蟹女王是单一性别的，动画和声音都是男性的。源模型（ Meshy 导出，slim 1）：leomamale“Leonid Bare - 3ds Max Biped.glb”，crabman 和crabqueen“... - 3ds Max Biped.glb”，其他 `leonidfemale.glb` 、 `corvidmale.glb` 、 `corvidfemale.glb` 。

<a id="records"></a>
## 记录

从加载的游戏自己的记录（ `RaceRecords` ）添加到数据阶段，因此 `KenshiRaces.mod` 为空，而 CI 不需要游戏：

- RACE_GROUP ：种族的名称及其子种族的种族，按编辑的顺序。
- RACE （子种族）：格陵兰人的副本（ `17-gamedata.quack` ：统计数据、动画、声音），包含每个性别的网格、颜色纹理、法线贴图和部分贴图（单性别子种族两者都穿着同一个身体）； `single gender` 除非它有两个实体；没有头或头发（列出没有头的种族就没有头；身体有自己的）； `morph num` 0; `no shirts` （衬衫和裤子画出人体的uv布局）；清除身体遮罩和强度法线贴图； `playable` 与 `editor limits` [ `kenshiraces_editor.xml` ](#character-editor) 。
- CHARACTER ：亚种和性别（ `female chance` 100或0）的漂流者（ `2205-gamedata.base` ）副本，没有套件，金钱或赏金，与新兵交谈。
- 引擎： CHARACTER 自己的 `mesh` 场会覆盖其种族的身体网格。

<a id="character-editor"></a>
## 角色编辑器

比赛是在角色创建时提供的，就像游戏本身一样。 `KenshiRaces/items/races/kenshiraces_editor.xml` 只保留移动骨骼的滑块，因此它们像在人类上一样在安装的身体上工作（引擎文档/动画.md，骨骼比例）：面部保持头部尺寸，头部形状和颈部滑块（没有面部姿势：安装的头部没有）；身体包含人体滑块和肤色，以及女性的乳房尺寸、高度和间距，这会移动女性骨骼的 `L Boob` / `R Boob` 女性身体的权重（[种族导入](race-import.md#bust) ）。范围是人类的范围（ `data/editor/editor_data_human.xml` ）。

<a id="recruiting"></a>
## 招聘

DIALOGUE  `1-KenshiRaces.mod` ：角色说“你看起来可以使用另一双手。”，玩家回答“加入我们”。动作 18（加入小队）将角色添加到玩家的小队中。

<a id="testing-in-game"></a>
## 招募与游戏内测试

1. 在启动器或 `data/mods.cfg` 中启用 Kenshi Races。
2. 在控制台输入 `spawn leomafemale`，角色就会出现在光标位置。也可以使用表中的其他角色名称。
3. 与角色交谈并同意其加入小队，也可以使用 F10 调试菜单中的 Recruit。
4. F10 菜单中的 **Kenshi Races: races**会列出本模组及依赖它的模组已加载的适配种族。

<a id="clothing"></a>
## 服装与护甲

护甲会随同一套骨骼活动，但外形按人体制作。使用 `hide parts` 的外套、长袍和夹克会隐藏覆盖区域的皮肤。甲壳与蟹钳仍可能穿出紧身护甲。 适配时会收窄超出人类体形的部分，使其更接近人体比例。 ([race-import.md](race-import.md#slimming)).

[English](../../../docs/kenshi-races.md) · [Kenshi Races](../README.md)
