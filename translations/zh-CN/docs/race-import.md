<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# 赛车进口:将身体匹配到游戏的骨骼上

`tools/RaceImport` 变成一根 `.glb` 身体(Meshy, Hi3D: “3ds Max Biped” rig,关节 `Bip001 ...`)在一个身体中,游戏以自己的人类动画动画:一个皮革的奥格尔马斯与 `male_skeleton.skeleton` 或 `female_skeleton.skeleton`其它 DDS 文本和部分地图。 它从游戏安装中阅读了骨骼、人体和部分地图,所以它在机器上运行。 Kenshi從來沒有在CI;它的生產是承諾的。

<a id="run-it"></a>
## 运行它

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>`游戏文件夹(`data/` 内在,如 `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- 基因名称文件,并且必须在各个模式中独特(游戏以色列的名称找到文件): `<modfolder lower case>_<race>`.
- `male` / `female`是的,老板,老板是老板。 一个女人的身体是为了 `female_skeleton` 被测量为 `human_female.mesh` 和
`FemaleMask_Default.png` (游戏中的女性动画和声音,编辑的泡沫滑板);一个男人 `male_skeleton`, `human_male.mesh` 和 `MaleMask_Default.png`.
- `slim`0 到 1 默认 1 ([弱化](#slimming)它低于一个比它的衣服更大的赛车。
- 写作 `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (只有当模型具有正常地图时) `<stem>_parts.png`路由器给了
垂直计算,规模,边界和多少垂直落入可隐藏的部分。
- 保持它 `.glb` 外面是雷波(MB的分数);完成四个输出。 `.gitignore` 让 `*/items/races/*.mesh` 和 `*.dds` 通过。
- 一个依赖模式将其从一个KenshiRaces克隆与其自己的输出文件夹运行(`<Mod>/items/races`)和投票。

<a id="what-the-model-needs"></a>
## 模型需要什么

- 一个皮革的皮革,皮肤有双关节 `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`两者 `Clavicle`, `UpperArm`,
`Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`,和 `mixamorig:HeadTop_End` 头顶上。 其余的关节(手指,肩膀,助手)跟随他们最接近的Biped祖先。 **已验证** 在 Leoma, Crevan, Crabman 和 Crab 女王 (Meshy 出口。
- 一个基本的颜色结构;一个金属硬度和一个正常的结构,当存在时。
- 任何休息姿势: T-pose 和 A-pose 都合适(游戏的连接姿势有手臂下)。

<a id="how-the-fit-works"></a>
## 如何适合工作

1. 每一个垂直被涂在其余的位置的文件显示器(逆连接时间结合休息转换)。
2. 尺寸:游戏的头骨高于模型的头关高于其最低的垂直。
3. 每个背部、手臂和脚骨都被转动,因为它从关节到孩子,因为它是游戏的长度。
(六到六次)
4. 阴道、脊椎、脖子和<unk>子配备为一件硬件(最小方位在游戏的脖子、肩膀和肩膀关节上),所以胸部
保持其形状; 每肢都适合在脊椎和脊椎重量混合的地方。 腰部和脊椎关节被遗弃了:游戏的腰部与其顶部和一个Biped出口的座位,它落在胸部约一个单位。
5. 头部落在一半的道路上,从那里的<unk>带到游戏的头部关节,所以它的<unk>坐在它的骨头下。 游戏的脖子上涨
在上,在上,在下上;在中。**观察** 男人的 Leoma,Crabman和Crab Queen:配备脖子2.4单位,模型拥有0.8,游戏的1.4。
6. 脚保持平滑,   和  是  以便  地面上 地面( 和 脚 比 游戏的脚 更高)。
7. 每個角落都是它的重量;它的重量走向它的結合,最重的四個保持。

**已验证**该 C# 工具的标志与原始 Python 模型相匹配(在 1e-5 单位内,相同的骨重)

<a id="slimming"></a>
## 弱化

游戏的衣服是模拟围绕着人体的里格,这样一个身体的布局,它表明通过。 围绕每个骨头和肢体骨头(脊椎,脊椎,骨头,上部和下部手臂和腿)的工具测量人体最长的范围为骨头的六分之一和周围的十二分之一;一个固定的垂直超越这个范围,加上0.15个单位(1,5厘米的背部)保持其余额的五分之一,其纠正混合其骨重。 修正案在 1.5 到 2.5 倍的限制(横向:一个补充,如<unk>,单独),是零的骨头(一个尾巴挂在阴道下),并在表面上滑动(12 轮的邻居平均,UV 轴承焊接),所以邻居的垂直移动相同。 **观察** (男孩) Leoma)39%的垂直移动,平均为1.3厘米,最大为9.5厘米。

<a id="bust"></a>
## 巴士

女人的身体的泡沫被提升到女人的身体(**观察**答:The Leoma 和 Crevan 女士们把他们的2.0和1.75个单位低下,在衣服的胸部和部分地图的胸部下。 是的中间。 `Spine1` 肩膀和 0.3 到 1.8 单位在中间线上,其中最前端是最前端。 每個背部都會因其重量的差異而上升,從腰部的高度的 exp(-(d / 1.5)2) 時刻,從脊椎深度(0)到腰部(1),所以背部、手臂和腳保持。 然后,滑雪跑。

<a id="human-only-bones"></a>
## 人只骨头

女性骨骼有 `L Boob` 和 `R Boob` (编辑器的乳房大小、高度和空间规模,并将其移动,发动机 docs/animation.md( ) 缺少一片混合的。 在减肥后,每个骨头的重量都是 `Spine1` 或 `Spine2` 接近人类的重量,其中一些,在1个单位,规模为1 - 距离;它的重量缩小,使空间。 **观察** (`human_female.mesh`):骨头垂直由两只骨头(前胸垂直994个垂直892个)。 骨头(`L Boob` 28, `R Boob` 29 )和女王的其他手套不同于男人的手套(`R Clavicle` 23 對 25; 男子有 `Bip01 Jaw`因此,子总是写在自己的子的手中。

<a id="part-map"></a>
## 部分地图

赛车的 `part map male`/`female` PNG 标志 由 由  由 与 由 各 由 归 于 由 将 归 到 由 向 归 至 到 向 到 到 至 向 至 至 以 到 以 至 为 到 为 以 为 为 向 向 为 由 为 至 于 向 以 向 為 为 `hide parts` 停止绘制这些部分(发动机) docs/characters.md “隐藏的身体部位” 该工具给了每一个固定垂直的最接近的人体垂直的部分,由相同的骨头,从该垂直的Uv部分地图阅读,并写在固定垂直本身的Uv(1024×1024),其他纺织品透明。 每個人,首先是 0.8 個單位(一個沒有部分的垂直取出最接近的垂直),因為一個超越的身體在更廣泛的線上。

**观察** (`MaleMask_Default.png`第2562章 看见了 `human_male.mesh` 如同游戏样本一样,有642个角落:上胸部和肩膀顶部的红色,上手围的白色带,腹部的前面绿色。 `hide parts` 在基数据中(rebirth.mod):Drifter和Padded皮革夹克1、无缝皮革2、长皮革3、Ninja Gi和Rags4、衣柜5、皮革衣和Crab Armour7;圣仆人Rags、Monk Pants和Hoods0。 0 的物品不隐藏;减肥覆盖这些物品。 **观察** (`FemaleMask_Default.png` 在 `human_female.mesh` 紫外线: 红色 布斯(1102 垂直), 白色 顶部 手臂 带(560), 绿色 腹部(250)。

<a id="textures"></a>
## 文学

- `_d`: 基礎顏色, alpha = gloss = (255 - 硬度) / 2 (硬度 200 沒有地圖); 20482, BC3 與 mips.
- `_n`游戏的角色配置,X在阿尔法和Y在绿色,Y从 glTF+ Y 向上; 红色和蓝色 0.
