<a id="depending-on-kenshi-races"></a>
# 取决于 Kenshi Races

代码 mod 如何使用 Kenshi Races 的种族和动画，向其中添加子种族或添加自己的种族 ( `kenshi.races` )。加载器规则：引擎 [modding-dependency.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md) 。代理技能经历相同的步骤： [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md) 。

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. 在构建时引用它，而不是发布它

在由 [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate) 制作的存储库中，将 Kenshi Races ' 发布 zip 添加到 `meitou.lock.json` ；构建会下载、检查并引用它，然后 Play 会部署它。每个值都来自发行版注释 ( `kenshi-races-v<version>` )：

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

- 您的 `engine/` 引脚将移动到该引擎提交（ GutenStructure [框架锁](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ）。
- 切勿将 `KenshiRaces.dll` 列在您自己的 `assemblies` 中；加载的 `kenshi.races` 服务类型。

<a id="2-declare-it-in-meitoujson"></a>
## 2.在 `meitou.json` 中声明

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- 在 1.0 之前，次要版本可能会破坏 API ，因此请在下一个次要版本中限制范围。
- Kenshi Races 必须位于 `data/mods.cfg` 中 mod 的上方（启动器的 Arrange 会做到这一点），因此当您的数据阶段运行时，它的记录就存在。

<a id="3-use-its-races"></a>
## 3.利用它的种族

请参阅您记录中 `Races` 中的 ID，例如在你的数据阶段：

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- 种族（亚种）： `Races.LeomaRace` 、 `CrevanRace` 、 `CrabmanRace` 、 `CrabQueenRace` 。
- 角色（无套件，可招募）： `LeomaMaleCharacter` 、 `LeomaFemaleCharacter` 、 `CrevanMaleCharacter` 、 `CrevanFemaleCharacter` 、 `CrabmanCharacter` 、 `CrabQueenCharacter` 。
- RACE_GROUPs （编辑的种族）： `LeomaGroup` 、 `CrevanGroup` 、 `CrabfolkGroup` 。 `Races.Recruit` 是“加入我们”。讲话。
- `Races.Leoma` 、 `Crevan` 、 `Crabman` 、 `CrabQueen` ( `Subrace` ) 和 `Races.Groups` 描述它们。表：[kenshi-races.md“种族”](kenshi-races.md#races) 。

<a id="4-add-a-subrace-or-a-race"></a>
## 4. 添加子种族或种族

1. 使用来自 KenshiRaces 克隆的 [race import](race-import.md) ] 将每个身体安装到带有您自己的茎的 mod 的 `items/races` 中（ `mymod_<character>` ； `female` 作为女性身体的装备），然后提交文件。将 `!/<Mod>/items/races/*.mesh` 和 `!/<Mod>/items/races/*.dds` 添加到 `.gitignore` 。
2. 描述它并将其记录添加到 mod 的数据阶段（该条目实现 `IModDataPhase` ）：

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

- 具有 `Male` 和 `Female` 身体的亚种将获得编辑器的性别箭头；其中一个是单一性别，每个角色都穿着它。
- `RaceRecords.Add(c, subrace, talk)` ： `talk` 是字符的 DIALOGUE id， `null` 是无字符。
- 您自己的种族： `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` 添加了 RACE_GROUP 和每个子种族。
- 子种族为 `playable` 和 Kenshi Races ' 编辑器限制 ( `RaceRecords.EditorLimits` );  当您之后想要更改更多字段时， `FillRace` / `FillCharacter` 填写您自己添加的记录（ `c.Add` ）。
- ids 是你的：你自己的 `.mod` 文件名的记录。

<a id="6-play-its-animations"></a>
## 6. 播放动画

[animations.md](animations.md) 的每个剪辑都链接到游戏人体骨骼上的每个种族（也添加了 `RaceRecords` 的子种族），因此 mod 只播放它。 Mod 战斗动作只是动画，每当游戏没有播放任何自己的内容时就会被询问（引擎文档/modding.md，“战斗动作”）：

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` 是你的：来自你自己状态的廉价、线程安全、只读答案，剪辑的进度从 0 到 1（秒 / `clip.Length` ）。
- 剪辑在原地播放并面向前方；比您的移动短的剪辑将保留其最后一帧，除非您包裹进度。
- 动画只需要依赖项（第 1 节和第 2 节），不需要您自己的记录。

<a id="5-check-it"></a>
## 5.检查一下

`dotnet build` ，您的测试，然后在游戏 F10 ** Kenshi Races ：种族** 中列出您的种族及其网格，然后 `spawn <name>` 生成它。用一个小写单词命名子种族和字符，例如 `ghostcrab` ，因此控制台将它们不带引号；新游戏的编辑器会列出您的子种族。
