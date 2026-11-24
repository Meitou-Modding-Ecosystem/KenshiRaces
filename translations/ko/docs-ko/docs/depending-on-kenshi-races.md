<a id="depending-on-kenshi-races"></a>
# Kenshi Races 에 따라 다름

코드 모드가 Kenshi Races ' 종족과 애니메이션을 사용하고, 하위 종족을 추가하거나 자체 종족을 추가하는 방법( `kenshi.races` ). 로더 규칙: 엔진 [modding-dependents.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md) . 에이전트 스킬은 동일한 단계를 진행합니다: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md) .

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. 빌드 시 참조하고 배송하지 마세요.

[MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate) 로 만든 저장소에서 Kenshi Races ' 릴리스 zip을 `meitou.lock.json` 에 추가합니다. 빌드는 이를 다운로드하고 확인하고 참조하며 Play는 이를 배포합니다. 모든 값은 릴리스 노트( `kenshi-races-v<version>` )에서 나옵니다.

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

- `engine/` 핀은 해당 엔진 커밋( GutenStructure [프레임워크 잠금](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) )으로 이동합니다.
- 자신의 `assemblies` 에 `KenshiRaces.dll` 를 나열하지 마십시오. 로드된 `kenshi.races` 는 유형을 제공합니다.

<a id="2-declare-it-in-meitoujson"></a>
## 2. `meitou.json` 에서 선언

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- 1.0 이전의 마이너 버전은 API 를 손상시킬 수 있으므로 다음 마이너 버전에서 범위를 제한하세요.
- Kenshi Races 는 `data/mods.cfg` 에서 모드 위에 있어야 하므로(런처의 정렬이 이를 수행함) 데이터 단계가 실행될 때 해당 레코드가 존재합니다.

<a id="3-use-its-races"></a>
## 3. 종족을 활용하라

기록에서 `Races` 의 ID를 참조하세요. 데이터 단계에서:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACE(하위 경주): `Races.LeomaRace` , `CrevanRace` , `CrabmanRace` , `CrabQueenRace` .
- 캐릭터(키트 없음, 모집 가능): `LeomaMaleCharacter` , `LeomaFemaleCharacter` , `CrevanMaleCharacter` , `CrevanFemaleCharacter` , `CrabmanCharacter` , `CrabQueenCharacter` .
- RACE_GROUPs (편집자의 종족): `LeomaGroup` , `CrevanGroup` , `CrabfolkGroup` . `Races.Recruit` 는 "함께해요"입니다. 말하다.
- `Races.Leoma` , `Crevan` , `Crabman` , `CrabQueen` ( `Subrace` ) 및 `Races.Groups` 가 이를 설명합니다. 표: [kenshi-races.md "경주"](kenshi-races.md#races) .

<a id="4-add-a-subrace-or-a-race"></a>
## 4. 하위 경주 또는 경주 추가

1. [KenshiRaces 클론의 레이스 가져오기](race-import.md) 를 자신의 스템( `mymod_<character>` ; 여성 신체용 리그인 `female` )을 사용하여 모드의 `items/races` 에 맞추고 파일을 커밋합니다. `!/<Mod>/items/races/*.mesh` 및 `!/<Mod>/items/races/*.dds` 를 `.gitignore` 에 추가합니다.
2. 이를 설명하고 모드의 데이터 단계에 해당 레코드를 추가하십시오(항목은 `IModDataPhase` 를 구현합니다).

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

- `Male` 및 `Female` 본체가 있는 하위 경주는 편집자의 성별 화살표를 얻습니다. 하나는 단일 성별이고 모든 캐릭터가 그것을 착용합니다.
- `RaceRecords.Add(c, subrace, talk)` : `talk` 는 문자의 DIALOGUE ID이고, 없음의 경우 `null` 입니다.
- 나만의 레이스: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` 는 RACE_GROUP 와 모든 하위 레이스를 추가합니다.
- 하위 경주는 Kenshi Races ' 편집기 제한( `RaceRecords.EditorLimits` )이 있는 `playable` 입니다. `FillRace` / `FillCharacter` 채우기 레코드는 나중에 더 많은 필드를 변경하려는 경우 직접 추가( `c.Add` )합니다.
- ID는 귀하의 것입니다. 귀하의 `.mod` 파일 이름이 기록됩니다.

<a id="6-play-its-animations"></a>
## 6. 애니메이션 재생

[animations.md](animations.md) 의 모든 클립은 게임의 인간 뼈대에 있는 모든 종족( `RaceRecords` 와 함께 추가된 하위 종족도 있음)에 연결되어 있으므로 모드에서는 해당 클립만 재생합니다. 모드 전투 동작은 애니메이션일 뿐이며, 게임이 자체적으로 아무것도 재생하지 않을 때마다 요청됩니다(엔진 문서/modding.md, "전투 동작"):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` 는 귀하의 것입니다. 귀하의 상태에서 저렴하고 스레드로부터 안전하며 읽기 전용 답변을 얻을 수 있으며 클립의 진행률은 0에서 1(초/ `clip.Length` )입니다.
- 클립은 제자리에서 재생되며 앞을 향합니다. 이동보다 짧은 클립은 진행 상황을 래핑하지 않는 한 마지막 프레임을 유지합니다.
- 애니메이션에는 종속성(섹션 1 및 2)만 필요하며 자체 기록은 필요하지 않습니다.

<a id="5-check-it"></a>
## 5. 확인해보세요

`dotnet build` , 테스트 후 게임 F10 ** Kenshi Races : races** 에서 메시와 함께 레이스를 나열하고 `spawn <name>` 가 이를 생성합니다. 하위 경주 및 CHARACTER 이름은 소문자로 1개 단어로 지정하세요. `ghostcrab` 이므로 콘솔은 따옴표 없이 이를 사용합니다. 새 게임의 편집기에는 하위 인종이 나열됩니다.
