<a id="depending-on-kenshi-races"></a>
# Kenshi Races に応じて

コード MOD が Kenshi Races ' レースとアニメーションを使用する方法、それらにサブレースを追加する方法、または独自のレース ( `kenshi.races` ) を追加する方法。ローダー ルール: エンジン [modding-dependency.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md) .エージェント スキルは同じ手順を実行します: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md) 。

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. ビルド時に参照し、決して出荷しない

[MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate) ] から作成したリポジトリで、 Kenshi Races のリリース zip を `meitou.lock.json` に追加します。ビルドはダウンロード、チェック、参照し、Play によってデプロイされます。すべての値はリリースのノート ( `kenshi-races-v<version>` ) に基づいています。

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

- `engine/` ピンは、そのエンジン コミット ( GutenStructure [フレームワーク ロック](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ]) に移動します。
- 自分の `assemblies` に `KenshiRaces.dll` をリストしないでください。ロードされた `kenshi.races` は型を提供します。

<a id="2-declare-it-in-meitoujson"></a>
## 2. `meitou.json` で宣言する

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- 1.0 より前のマイナー バージョンでは API が壊れる可能性があるため、次のマイナーで範囲を制限します。
- Kenshi Races は `data/mods.cfg` の MOD の上に配置する必要があります (ランチャーの Arrange がそれを行います)。そのため、データ フェーズの実行時にそのレコードが存在します。

<a id="3-use-its-races"></a>
## 3. 種族を利用する

記録から `Races` の ID を参照します。データフェーズでは:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- レース (サブレース): `Races.LeomaRace` 、 `CrevanRace` 、 `CrabmanRace` 、 `CrabQueenRace` 。
- キャラクター (キットなし、採用可能): `LeomaMaleCharacter` 、 `LeomaFemaleCharacter` 、 `CrevanMaleCharacter` 、 `CrevanFemaleCharacter` 、 `CrabmanCharacter` 、 `CrabQueenCharacter` 。
- RACE_GROUPs (編集者の種族): `LeomaGroup` 、 `CrevanGroup` 、 `CrabfolkGroup` 。 `Races.Recruit` は「参加しましょう」です。話す。
- `Races.Leoma` 、 `Crevan` 、 `Crabman` 、 `CrabQueen` ( `Subrace` ) および `Races.Groups` で説明します。表: [kenshi-races.md "種族"](kenshi-races.md#races) 。

<a id="4-add-a-subrace-or-a-race"></a>
## 4. サブレースまたはレースを追加する

1. 各ボディを KenshiRaces クローンからのレース インポート](race-import.md) で、独自のステム ( `mymod_<character>` ; 女性ボディのリグとして `female` ) を備えた MOD の `items/races` にフィットさせ、ファイルをコミットします。 `!/<Mod>/items/races/*.mesh` と `!/<Mod>/items/races/*.dds` を `.gitignore` に追加します。
2. それを説明し、MOD のデータ フェーズにそのレコードを追加します (エントリは `IModDataPhase` を実装します)。

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

- `Male` および `Female` ボディを持つサブレースには、編集者の性別矢印が適用されます。 1つは単一の性別であり、すべてのキャラクターがそれを着用します。
- `RaceRecords.Add(c, subrace, talk)` : `talk` はキャラクターの DIALOGUE ID であり、 `null` はキャラクターの ID ではありません。
- あなた自身のレース: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` は、 RACE_GROUP とすべてのサブレースを追加します。
- サブレースは `playable` であり、 Kenshi Races ' エディター制限 ( `RaceRecords.EditorLimits` ) があります。 `FillRace` / `FillCharacter` は、後でさらにフィールドを変更する場合に、自分で追加したレコード ( `c.Add` ) を入力します。
- ID はあなたのものです。あなた自身の `.mod` ファイル名のレコードです。

<a id="6-play-its-animations"></a>
## 6. アニメーションを再生します

[animations.md](animations.md) ] のすべてのクリップは、ゲームの人間のスケルトンのすべての種族 ( `RaceRecords` で追加されたサブ種族も) にリンクされているため、MOD はそれを再生するだけです。 MOD の戦闘動作はアニメーションのみであり、ゲームがそれ自体で何も再生しないたびに尋ねられます (engine docs/modding.md、「戦闘動作」)。

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` はあなたのものです。あなた自身の状態、クリップの進行状況 0 から 1 (秒 / `clip.Length` ) からの、安価でスレッドセーフな読み取り専用の回答です。
- クリップは所定の位置で前を向いて再生されます。移動よりも短いクリップは、進行状況をラップしない限り、最後のフレームを保持します。
- アニメーションには依存関係 (セクション 1 と 2) のみが必要で、独自のレコードは必要ありません。

<a id="5-check-it"></a>
## 5. 確認してください

`dotnet build` 、テスト、そしてゲーム内 F10 ** Kenshi Races : レース** はメッシュを含むレースをリストし、 `spawn <name>` はそれを生成します。サブレースと CHARACTER には小文字の単語を 1 つ付けてください。 `ghostcrab` なので、コンソールは引用符なしでそれらを受け取ります。新しいゲームのエディターにサブレースがリストされます。
