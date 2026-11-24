<a id="depending-on-kenshi-races"></a>
# 依据 Kenshi 赛事

如何使用代码 Kenshi 种族的种族和动画,添加主种或添加自己的种族(`kenshi.races`). 加载器规则: 摩托车 [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). 代理技能通过相同的步骤: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. 把它提到建造时间,永远不会发送。

在一个由 [模特模特](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)添加 Kenshi Races’ 释放 Zip 到 `meitou.lock.json`构建下载,检查和参考它,并播放它。 每個價值都來自發布的筆記(`kenshi-races-v<version>`):

```json
{
  "id": "kenshi.races",
  "repo": "Meitou-Modding-Ecosystem/KenshiRaces",
  "tag": "kenshi-races-v0.4.0",
  "asset": "KenshiRaces-0.4.0.zip",
  "sha256": "<Zip SHA-256 from the notes>",
  "folder": "KenshiRaces",
  "engine": "<Submodule engine commit from the notes>"
}
```

- 你的 `engine/` 将 pin 移动到该发动机承诺(GutenStructure)
[框架锁](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- 永远不会列出 `KenshiRaces.dll` 在你自己的 `assemblies`其余的负载 `kenshi.races` 服务于类型。

<a id="2-declare-it-in-meitoujson"></a>
## 2. 宣布它在 `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- 在 1.0 之前,一个小版本可能会破坏 API接下来,你要把小小的距离放在下一个小小的位置上。
- Kenshi 赛车必须坐在你的模式上 `data/mods.cfg` (启动器的安排),所以它的记录在您的数据阶段运行时存在。

<a id="3-use-its-races"></a>
## 3. 使用自己的赛车

提到 IDS 中的 `Races` 从您的记录中,例如在您的数据阶段:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs (重定向): `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (重定向自招聘): `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (编辑的作品): `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` 是“加入我们”的谈话。
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`)和 `Races.Groups` 描述它们。 桌子:
[Kenshi-races.md “种族”](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. 添加一种子或一种种子

1. 每個身體都與 [赛车进口](race-import.md) 从一个KenshiRaces克隆,进入你的模式 `items/races` 用自己的声音
(`mymod_<character>`; `female` 像一个女人的身体一样),并承诺文件。 添加 `!/<Mod>/items/races/*.mesh` 和 `!/<Mod>/items/races/*.dds` 到 `.gitignore`.
2. 将其描述并将其记录添加到您的模式的数据阶段(输入应用程序) `IModDataPhase`):

```csharp
static readonly Subrace GhostCrab = new("ghostcrab", "30-MyMod.mod",
    Male: new RaceBody("MyMod", "mymod_ghostcrab", NormalMap: true), Female: null,
    Characters: [new RaceCharacter("ghostcrab", "31-MyMod.mod", Female: false)]);

public void EditData(ModDataContext c)
{
    RaceRecords.Add(c, GhostCrab);                                       // RACE + CHARACTERs, the recruit talk by default
    RaceRecords.JoinGroup(c, Races.CrabfolkGroup, GhostCrab.RaceId);    // the editor lists it under Crabfolk
}
```

- 一种与A `Male` 和A `Female` 身体得到编辑的性别箭头,一个是单性,每个角色都穿着它。
- `RaceRecords.Add(c, subrace, talk)`: `talk` 是A DIALOGUE 对于这些人物来说, `null` 為沒有人。
- 一个自己的赛事: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` 添加到 RACE_GROUP 每种种族。
- 物品 `playable` 与 Kenshi 编辑器的限制(`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` 填写你添加的记录
自己( )`c.Add`)当你想更改更多的字段之后。
- IDs 是你的:记录是你的 `.mod` 文件名称

<a id="6-play-its-animations"></a>
## 6. 播放动画

每张剪辑 [animations.md](animations.md) 与游戏中的每一个人体骨骼的赛车相连(添加一个子种与 `RaceRecords` 也),所以一个模式只会玩它。 一个模式的战斗动作仅仅是动画,每当游戏中没有自己的动作时,都会被问及(发动机) docs/modding.md“战斗运动”):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` 是你的:一个廉价的,线条安全的,从自己的状态阅读的答案,剪辑的进展 0 到 1 (秒 / `clip.Length`).
- 剪辑在现场播放,面对前方;一个比你的动作更短的剪辑保持了最后的框架,除非你抓住进展。
- 动画只需要依赖(第1和第2节),没有自己的记录。

<a id="5-check-it"></a>
## 5. 查看它

`dotnet build`,你的测试,然后在游戏F10 **Kenshi 赛事:赛事** 以你的手,以你的手。 `spawn <name>` 把它擦去。 名称 和 CHARACTERs 一个低点的词,例如 `ghostcrab`因此,控制台不会引用它们;一个新的游戏编辑列出你的分类。
