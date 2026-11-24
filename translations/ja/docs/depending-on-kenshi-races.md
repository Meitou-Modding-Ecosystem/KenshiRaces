<a id="depending-on-kenshi-races"></a>
# 依存して Kenshi レース

コードモードの使い方 Kenshi レースのレースやアニメーション、それらにサブレースを追加するか、それら自身のレース(`kenshi.races`). 充電規則:モーター [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). エージェントのスキルは、同じステップを通過します: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. 建設時間に参照し、決して送ることはありません。

由来のレポで作られた [メイドモード](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)添加 Kenshi レースのリリース ZIP `meitou.lock.json`ダウンロード、チェック、参照を構築し、プレイはそれを実行します。 すべての値は、リリースのノートから来ています(`kenshi-races-v<version>`):

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

- あなたの `engine/` パインがそのエンジンに移動する(GutenStructure
[フレームロック](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- 決してリスト `KenshiRaces.dll` 自分のうちに `assemblies`・充電したもの `kenshi.races` タイプに仕える。

<a id="2-declare-it-in-meitoujson"></a>
## 2. で宣言する `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- 以前の 1.0 は、小さなバージョンが破ることができます。 API次に、次に、次の小さな範囲をカップします。
- Kenshi レースはあなたのモードの上に座る必要があります。 `data/mods.cfg` (ラウンチャーの順序はそうする) だから、データの段階が動くときにその記録が存在します。

<a id="3-use-its-races"></a>
## 3. レースを利用する

IDS を参照 `Races` あなたの記録から、例えば、データ段階において:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs (サブスクリプション) `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (無名、募集可能) `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (編集者のレース) `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` 」と話すと「つながる」
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`)と `Races.Groups` 説明する。 テーブル:
[Kenshi-races.md 「レース」](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. サブレースまたはレースを追加する

1. それぞれの体に合わせて、 [レース輸入](race-import.md) KenshiRacesのクローンから、あなたのモードに `items/races` 自分の声で
(`mymod_<character>`; `female` 女性の体のためのリッジとして)とファイルを結びつける。 追加 `!/<Mod>/items/races/*.mesh` そして、 `!/<Mod>/items/races/*.dds` へへへへ `.gitignore`.
2. 説明して、モードのデータ段階に記録を追加します(入力実装) `IModDataPhase`):

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

- サブレイクとA `Male` そしてA `Female` 体は編集者の性の矢を手に入れ、一つは単一の性であり、それぞれのキャラクターがそれを着ている。
- `RaceRecords.Add(c, subrace, talk)`: `talk` Aは DIALOGUE キャラクターのアイドル、 `null` 誰にも。
- 自分のレース: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` 添加する → RACE_GROUP あらゆるサブレイク
- 主体は `playable` とのこと Kenshi レイアウトのレイヤー(`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` 記録を追加する
自分(`c.Add`) 後により多くのフィールドを変更したい場合。
- IDs はあなたのものです: あなたの記録 `.mod` ファイル名

<a id="6-play-its-animations"></a>
## 6. アニメーションをプレイする

すべてのクリップ [animations.md](animations.md) ゲームの人間の骨格のすべてのレースに結びついている(サブレースが追加されます) `RaceRecords` しかも、モードはそれを演じるだけだ。 モード戦闘動きは、単なるアニメーションであり、ゲームが何の役にも立たないときに尋ねられます(エンジン) docs/modding.md「戦闘が動く」:

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` あなたのもの:安い、テープセキュリティ、あなたの状態から読むだけで答え、クリップの進歩は0から1(秒) `clip.Length`).
- クリップは現地でプレーし、あなたの動きよりも短いクリップは、あなたが進歩を回避しない限り、最後のフレームを保持します。
- アニメーションは依存症(第1部と第2部)のみを必要とし、自分の記録はありません。

<a id="5-check-it"></a>
## 5. チェックする

`dotnet build`試合後、F10で **Kenshi レース:レース** あなたがたは、あなたがたの手のひらを、 `spawn <name>` スパイする。 名称 サブタイトル& CHARACTERs 低い単語、例えば、 `ghostcrab`だから、コンソールは引用なしでそれらを取る; 新しいゲームの編集者はあなたのサブレースをリストします。
