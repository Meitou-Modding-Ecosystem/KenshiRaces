<a id="kenshi-races"></a>
# Kenshi 赛事

為角色編輯的三個新賽車,每個與超級賽車,在遊戲的人類骨骼(`kenshi.races`,没有依赖性):每个记录添加到负载,装备的身体 [赛车进口](race-import.md). 它还将一艘 [动画图书馆](animations.md) 每场比赛中有59个剪辑。 模特使用赛车和剪辑,添加赛车或添加自己的赛车: [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## 赛事

编辑列出每一个 RACE_GROUP 作为一场竞赛和其 RACEs 如  ( ) docs/characters.md “字符编辑器”

| 赛事(RACE_GROUP) | ( )RACE) | 性别 | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | 莱奥马 `10-` | 男子,女子 | 莱奥马尔 `11-`女同性恋 `17-` |
| Crevan `31-` | 船员 `18-` | 男子,女子 | 克里文马尔 `19-`女士 女士 `21-` |
| Crabfolk `32-` | 克拉布曼 `12-` | 单身 | 克拉布曼 `13-` |
| | 克拉布克 `14-` | 单身 | 克拉布克 `15-` |

将结束在 `KenshiRaces.mod`. Subrace 和 CHARACTER 名称是低点字,所以控制台的 `spawn` 把它们拿出来,没有引用。 每个身体的文件都是 `KenshiRaces/items/races/kenshiraces_<character>*` (常见地图) Leoma 和 Crevan). 男士的身体是为了 `male_skeleton`女士们在 `female_skeleton`克拉布的身体是男人的匹配,所以克拉布女王是单性与男人的动画和声音。 源模型(Meshy 出口,薄 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman 和 crabqueen "... - 3ds 麦克斯 Biped。glb",其他 `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## 记录

将其添加到彩票的数据阶段,从加载的游戏自己的记录(`RaceRecords`) ) ) ) `KenshiRaces.mod` 船是空的,CI不需要游戏:

- RACE_GROUP种族的名称和其子种族 RACEs在编辑的命令下。
- RACE (原文) 以此为副本 Greenlander (`17-gamedata.quack`: 统计,动画,声音) 与每个性别的<unk>,颜色结构,正常地图
和部分地图(一个单性子种为两者携带一个身体); `single gender` 除非它有兩個身體;沒有頭部或頭髮(一種列表沒有頭部吸引任何;身體有自己的); `morph num` 0; `no shirts` (衬衫和裤子绘制人体的UV布局);身体面具和强度正常地图清洁; `playable` 与 `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTER此分類上一篇: Drifter(`2205-gamedata.base`种族和性别(`female chance` 100或0),没有套件、金钱或奖金,与
招聘谈话
- 发动机:A CHARACTER自己的 `mesh` 田野超越了其赛车的身体梅斯。

<a id="character-editor"></a>
## 格拉格编辑

赛车在角色创作,如游戏本身。 `KenshiRaces/items/races/kenshiraces_editor.xml` 只保留移动骨头的滑板,所以它们像人类一样在装饰的身体上工作。 docs/animation.md,骨架):面部持头尺寸,头形和脖子滑板(没有面部姿势:固定头没有);身体持人人体滑板和皮肤色调,乳房尺寸,高度和宽度为女性,移动女性骨骼 `L Boob` / `R Boob` 女性体重为([赛车进口](race-import.md#bust)). 天下人(人)`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## 招聘

DIALOGUE `1-KenshiRaces.mod`字符说“你可以使用另一个手” ,玩家回答“加入我们” ,行动18(加入团队)将字符添加到球员的团队。

<a id="testing-in-game"></a>
## 在游戏中测试

1. 允许 Kenshi 发射器中的赛车(或 `data/mods.cfg`).
2. 安慰: `spawn leomafemale` (或任何 CHARACTER 在桌子上)在通道上旋转;谈到招聘,或F10垃圾招聘。
3. F10 **Kenshi 赛事:赛事** 列出每個配備的賽車(該模式和依賴者)的   。
4. 新游戏:编辑的赛车火箭到达 Leoma, Crevan 和 Crabfolk.

<a id="clothing"></a>
## 服装

Armour 分享了身体的骨骼,所以它与这些身体一起移动,但它在人体周围模型。 部分地图隐藏皮肤在物品下 `hide parts` (毛巾,衣服,外套);减肥拖在什么比其他地方的人更大([赛车进口](race-import.md#slimming)). 形状远离人类(<unk>,<unk>)仍然通过紧密的手套冲动。
