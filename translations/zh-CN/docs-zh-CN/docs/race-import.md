<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# 种族导入：将身体安装到游戏的骨架上

`tools/RaceImport` 将已装备的 `.glb` 身体（ Meshy 、 Hi3D ：“3ds Max Biped”装备，关节 `Bip001 ...` ）转换为游戏用其自己的人类动画进行动画处理的身体：链接到 `male_skeleton.skeleton` 或 `female_skeleton.skeleton` 的蒙皮 Ogre 网格、其 DDS 纹理和零件贴图。它从游戏安装中读取装备的骨架、人体和零件图，因此它在 Kenshi 的机器上运行，而不是在 CI 中运行；其输出已提交。

<a id="run-it"></a>
## 运行它

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>` ：游戏文件夹（里面有 `data/` ），例如 `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"` 。
- 词干命名文件，并且在模组中必须是唯一的（游戏通过裸名查找文件）： `<modfolder lower case>_<race>` 。
- `male` / `female` ：装备，默认男性。将女性身体安装到 `female_skeleton` 上，并与 `human_female.mesh` 和 `FemaleMask_Default.png` 进行测量（游戏中的女性动画和声音，编辑器的胸围滑块）；男一为 `male_skeleton` 、 `human_male.mesh` 和 `MaleMask_Default.png` 。
- `slim` : 0 到 1，默认 1 ([瘦身](#slimming) );参加比衣服更笨重的比赛时，请将其降低。
- 写入 `<stem>.mesh` 、 `<stem>_d.dds` 、 `<stem>_n.dds` （仅当模型有法线贴图时）和 `<stem>_parts.png` ；对数线给出了顶点数、比例、边界以及有多少顶点落在可隐藏部分中。
- 将 `.glb` 保留在存储库之外（数十个 MB ）；提交四个输出。 `.gitignore` 让 `*/items/races/*.mesh` 和 `*.dds` 通过。
- 依赖 mod 从 KenshiRaces 克隆运行它，具有自己的输出文件夹 ( `<Mod>/items/races` ) 和茎。

<a id="what-the-model-needs"></a>
## 模型需要什么

- 一种蒙皮网格物体，其蒙皮具有 Biped 关节 `Bip001 Pelvis` 、 `Spine` 、 `Spine1` 、 `Spine2` 、 `Neck` 、 `Head` 、 `Clavicle` 、 `UpperArm` 、 `Forearm` 、 `Hand` 、 `Thigh` 、 `Calf` 、 `Foot` 、 `Toe0` 和 `mixamorig:HeadTop_End` 位于头部上方。额外的关节（手指、尾巴、辅助关节）遵循其最近的 Biped 祖先。 **已在 Leoma 、 Crevan 、Crabman 和 Crab Queen（ Meshy 导出）上验证**。
- 底色纹理；如果存在金属粗糙度和正常纹理，则使用该纹理。
- 任何休息姿势：T 姿势和 A 姿势都适合（游戏的束缚姿势是手臂向下）。

<a id="how-the-fit-works"></a>
## 适配如何运作

1. 每个顶点都被蒙皮到文件显示的静止姿势（逆绑定时间关节静止变换）。
2. 比例：游戏头部骨骼相对于模型头部关节最低顶点的高度。
3. 每个脊柱、手臂和腿骨都被转动，以便像游戏一样从其关节指向其子骨骼，并拉伸到游戏的长度（0.6 到 1.6 倍）。
4. 骨盆、脊柱、颈部和锁骨被安装为一个刚性部件（与游戏的颈部、肩部和髋关节的最小直角），因此胸部保持其形状；每骨配合在脊柱和锁骨重量混合处剪切它。骨盆和脊柱关节不适合：游戏的骨盆与臀部齐平，Biped 出口位于它们上方，这使胸部下降了大约一个单位。
5. 头部落在躯干将其带到游戏头部关节的中间位置，因此其网格位于其骨骼下方。游戏中的颈部比肩膀高出大约一个单位，而这些模型几乎没有这样做：将头部放在关节上，将颈部拉伸到大约三倍的长度（**在雄性 Leoma 、蟹人和蟹女王上观察到**：安装的颈部 2.4 个单位，模型自己的 0.8 个单位，游戏的 1.4 个单位）。
6. 脚保持直立并被压扁，以便脚底着地（爪子和蟹脚比游戏的脚高）。
7. 每个顶点都是其关节拟合的加权和；它的重量归于其关节所跟随的游戏骨骼，保留最重的四个。

**已验证**：C# 工具的网格与 Crabman 上原始 Python 原型的网格（位置在 1e-5 单位内，相同的骨骼权重）相匹配。

<a id="slimming"></a>
## 减肥

游戏的服装是根据装备的人体建模的，因此人体比显示的要笨重。在每个躯干和四肢骨骼（骨盆、脊柱、锁骨、上臂和下臂以及腿部）周围，该工具测量人体每六分之一骨骼和每十二分之一圈的最远范围；超出该范围的拟合顶点加上 0.15 个单位（1.5 厘米的布料）将保留其超出部分的五分之一，其校正由其骨骼权重混合而成。校正在限制的 1.5 到 2.5 倍之间逐渐消失（超出：爪子等附属物，单独留下），对于骨骼切片外部的顶点（悬挂在骨盆下方的尾巴）为零，并且在表面上平滑（12 轮相邻顶点平均，UV 焊缝焊接），因此相邻顶点移动相似。 **观察**（雄性 Leoma ）：39% 的顶点移动，平均移动 1.3 厘米，最多移动 9.5 厘米。

<a id="bust"></a>
## 摔碎

女性身体的胸围提高到女性人类的胸围（**观察到**： Leoma 和 Crevan 女性的胸围降低了 2.0 和 1.75 个单位，位于衣服的胸部和零件图的胸围下方）。半身像是四分之一单位切片的中间， `Spine1` 与肩部之间，中线旁 0.3 至 1.8 单位处，其最前面的顶点最靠前。每个顶点的上升幅度为差值乘以躯干骨骼重量、乘以胸部高度的 exp(-(dy / 1.5)²)、乘以从脊柱深度 (0) 到胸部深度 (1) 的平滑步长，因此背部、手臂和腿保持不变。然后就是跑步减肥。

<a id="human-only-bones"></a>
## 人类独有的骨骼

女性骨骼具有 `L Boob` 和 `R Boob` （编辑器的乳房尺寸、高度和间距比例并移动它们，engine docs/animation.md），这是 Biped 装备所缺少的。瘦身后，最重骨骼为 `Spine1` 或 `Spine2` 的每个顶点均采用最近的人类顶点的仅人类权重，该权重在 1 单位内按 1 - 距离缩放；它自身的重量会缩小以腾出空间。 **观察** ( `human_female.mesh` )：胸部顶点由两块骨头承载（994 个前胸部顶点中的 892 个）。骨骼（ `L Boob` 28、 `R Boob` 29）和女性装备的其他手柄与男性的不同（ `R Clavicle` 23 与 25；男性有 `Bip01 Jaw` ），因此网格体总是针对其自己的装备手柄写入。

<a id="part-map"></a>
## 零件图

种族的 `part map male` / `female`  PNG 标记每个顶点属于哪个身体部位；磨损物品的 `hide parts` 停止绘制这些零件（engine docs/characters.md“隐藏的身体零件”）。该工具为每个拟合顶点提供由同一骨骼承载的最近人体顶点的部分，从该顶点 uv 处的装备部分图读取，并将其写入拟合顶点自己的 uv（1024 × 1024，其他纹理像素透明）。每个人体部位首先生长 0.8 个单位（没有部位的顶点采用该距离内最近的标记顶点的顶点），因为较大的身体会夹在更宽的带上。

**观察**（ `MaleMask_Default.png` ，256²，在游戏采样时在 `human_male.mesh` uvs 采样）：15,504 个顶点中的 642 个有一个部分：上胸部和肩部顶部为红色，上臂周围为白色带，腹部前部为绿色。  基础数据（rebirth.mod）中的 `hide parts` ：漂流者和衬垫皮夹克1，无袖长衣和风衣2，长衣和风衣3，忍者道服和碎布4，长袍5，板甲，皮衣和蟹甲7；圣仆破布、僧侣裤和兜帽 0。0 的物品没有任何隐藏；减肥涵盖了这些。 **观察**（ `FemaleMask_Default.png` at `human_female.mesh` uvs）：红色半身像（1,102 个顶点），白色上臂带（560），绿色腹部（250）。

<a id="textures"></a>
## 纹理

- `_d` ：基色，alpha=光泽度=（255-粗糙度）/2（没有贴图时粗糙度为200）； 2048²，BC3，带 mips。
- `_n` ：游戏的角色布局，X为alpha，Y为绿色，Y从glTF的+Y向上翻转；红色和蓝色 0.
