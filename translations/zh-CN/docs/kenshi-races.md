<a id="kenshi-races"></a>
# Kenshi Races

角色编辑器的三个新种族，每个种族都有子种族，位于游戏的人体骨骼上（ `kenshi.races` ，无依赖性）：加载时添加的每个记录，身体由 [race import](race-import.md) ] 安装。它还为这些骨架上的每场比赛提供了一个包含 59 个剪辑的动画库](animations.md) 。模组使用种族和剪辑，添加子种族或添加自己的种族：[depending-on-kenshi-races.md](depending-on-kenshi-races.md) 。

<a id="races"></a>
## 比赛

编辑器将每个 RACE_GROUP 列为种族，并将其种族列为子种族（引擎文档/characters.md“角色编辑器”）。

| 比赛 ( RACE_GROUP ) | 亚族 ( RACE ) | 性别 | 人物 |
| --- | --- | --- | --- |
| Leoma `30-` | 雷奥马 `10-` | 男、女 | 雄性 `11-` , 雌性 `17-` |
| Crevan `31-` | 克雷文 `18-` | 男、女 | 公羊 `19-` , 母羊 `21-` |
| Crabfolk `32-` | 蟹人 `12-` | 单身的 | 蟹人 `13-` |
|  | 螃蟹皇后 `14-` | 单身的 | 螃蟹皇后 `15-` |

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
## 游戏中测试

1. 在启动器中启用 Kenshi Races （或 `data/mods.cfg` ）。
2. 控制台： `spawn leomafemale` （或表中的任何 CHARACTER ）在光标处生成；与Recruit交谈，或F10调试Recruit。
3. F10 ** Kenshi Races ：种族** 列出加载的每个已安装的种族（此模组和相关者的）。
4. 新游戏：编辑的种族箭头到达 Leoma 、 Crevan 和 Crabfolk 。

<a id="clothing"></a>
## 衣服

盔甲共享身体的骨骼，因此它会随着这些身体一起移动，但它是围绕人体建模的。零件图隐藏了带有 `hide parts` 的物品（外套、长袍、夹克）下的皮肤；减肥会吸收比其他地方的人更笨重的东西（[race import](race-import.md#slimming) ）。远离人类的形状（贝壳、爪子）仍然刺穿严密的盔甲。
