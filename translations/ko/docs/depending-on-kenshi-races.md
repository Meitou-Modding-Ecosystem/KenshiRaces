<a id="depending-on-kenshi-races"></a>
# 의존에 따라 Kenshi 레이스

코드 모드 사용 방법 Kenshi 인종과 애니메이션, 그들에게 수브라스를 추가하거나 자신의 인종을 추가합니다 (`kenshi.races`). 로더 규칙 : 엔진 [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). 에이전트 기술은 같은 단계를 통과합니다: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. 건설 시간에 그것을 참조하십시오, 결코 그것을 배치하지 마십시오

에서 만든 에서 [템플릿](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)추가하기 Kenshi Races' release Zip에 대한 리뷰 보기 `meitou.lock.json`; 건물 다운로드, 검사 및 참조 그것, 그리고 플레이 그것을 배포. 모든 값은 값의 값으로 나온다(`kenshi-races-v<version>`):

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

- 당신의 `engine/` 핀이 그 엔진에 움직이기 때문에 (GutenStructure)
[프레임 워크](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- 결코 목록 `KenshiRaces.dll` 당신의 자신의 `assemblies`• 충전된 `kenshi.races` 그들은 유형을 봉사합니다.

<a id="2-declare-it-in-meitoujson"></a>
## 2. 그것을 선언하라 `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- 1.0 이전에 작은 버전이 깨질 수 있습니다. API그래서 다음 미니에 대한 범위를 캡처합니다.
- Kenshi 레이스는 당신의 모드 위에 서 있어야합니다. `data/mods.cfg` (시작자의 순서) 그래서 그것의 기록은 당신의 데이터 단계가 실행 될 때 존재합니다.

<a id="3-use-its-races"></a>
## 3. 그들의 경주를 사용하라

IDS에 대한 참조 `Races` 귀하의 기록에서, 예를 들어, 귀하의 데이터 단계에서:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs (에 대한 의견 : `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (그러나 그들은 그들을 괴롭히지 않았다) `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (이 편집자의 레이스 : `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` ‘우리에게 합류하라’는 말이 있다.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) 그리고 `Races.Groups` 그들을 묘사하라. 테이블 :
[Kenshi-races.md “종사”](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Subrace 또는 Race를 추가하십시오

1. 각 몸은 각 몸과 함께 [경주 수입](race-import.md) KenshiRaces 클론에서, 당신의 모드에 `items/races` 당신의 목소리로
(`mymod_<character>`; `female` 여성의 몸에 대한 잔디처럼), 그리고 파일을 묶어. 추가하기 `!/<Mod>/items/races/*.mesh` 그리고 `!/<Mod>/items/races/*.dds` 는 는 `.gitignore`.
2. 그것을 설명하고 모드의 데이터 단계에 기록을 추가하십시오 (입력 구현 `IModDataPhase`):

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

- A와 함께 한 잔의 `Male` 그리고 A `Female` 몸은 편집자의 성총을 얻는다; 하나는 단일 성이며, 각 캐릭터는 그것을 착용합니다.
- `RaceRecords.Add(c, subrace, talk)`: `talk` A는 A DIALOGUE 캐릭터를 위한 ID, `null` 아무에게도
- 당신의 자신의 경주 : `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` 추가하십시오 RACE_GROUP 그리고 각각의 잔치.
- 주제가 니다 `playable` 와 함께 Kenshi 녹음기 녹음하기 녹음 (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` 당신이 추가하는 레코드를 작성합니다.
자신을 위해서(`c.Add`) 후에 더 많은 필드를 변경하려면.
- ID는 당신의 것입니다 : 당신의 기록 `.mod` 파일 이름

<a id="6-play-its-animations"></a>
## 6. 그의 애니메이션을 재생합니다.

각 클립은 [animations.md](animations.md) 게임의 인간 척추에있는 모든 경주와 연결되어있다 (subrace 추가 `RaceRecords` 또한), 그래서 모드는 그것을 재생합니다. 모드 전투 움직임은 단지 애니메이션이며, 게임이 자신의 무언가를 재생할 때마다 요청됩니다. docs/modding.md‘투쟁이 움직인다’는 말이다.

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` 귀하의: 저렴한, 스트립 안전한, 귀하의 상태에서 읽기만한 답변, 클립의 진행 0에서 1 (초 / `clip.Length`).
- 클립은 위치에 있고 앞으로 나아가며, 당신이 진보를 멈추지 않는 한, 당신의 움직임보다 짧은 클립은 마지막으로 유지됩니다.
- 애니메이션은 의존성만 필요합니다 (제 1 및 제 2 장), 자신의 기록이 없습니다.

<a id="5-check-it"></a>
## 5. 그것을 확인하십시오

`dotnet build`, 당신의 테스트, 그 다음 게임 F10 **Kenshi 레이스 : 레이스** 그대는 그대의 잔치와 함께 그대의 삶을 살아가며, `spawn <name>` 그것을 스파이. 이름은 고 CHARACTERs 한마디 낮은 단어, 예를 들면 `ghostcrab`그래서 콘솔은 인용없이 그들을 얻습니다; 새로운 게임의 편집자는 당신의 수족관을 목록.
