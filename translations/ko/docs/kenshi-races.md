<a id="kenshi-races"></a>
# Kenshi Races

게임의 인간 골격(`kenshi.races`, 종속성 없음)에 각각 하위 인종이 있는 캐릭터 편집기를 위한 세 가지 새로운 종족: 로드 시 추가된 모든 기록, [race import](race-import.md). 또한 해당 스켈레톤의 모든 종족에 대한 59개의 클립으로 구성된 [애니메이션 라이브러리](animations.md) ]도 제공합니다. 모드는 레이스와 클립을 사용하고, 하위 레이스를 추가하거나 자체 레이스를 추가합니다: [dependent-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## 플레이 가능한 종족

편집기에는 각 RACE_GROUP이 한 종족으로, 그 안의 RACE 레코드가 하위 종족으로 표시됩니다.

| 종족 | 하위 종족 | 몸체 | 캐릭터 |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | 남성, 여성 | leomamale `11-`, leomafemale `17-` |
| Crevan `31-` | crevan `18-` | 남성, 여성 | crevanmale `19-`, crevanfemale `21-` |
| Crabfolk `32-` | crabman `12-` | 단일 | crabman `13-` |
|  | crabqueen `14-` | 단일 | crabqueen `15-` |

ID는 `KenshiRaces.mod` 로 끝납니다. Subrace 및 CHARACTER 이름은 하나의 소문자 단어이므로 콘솔의 `spawn` 는 이를 따옴표 없이 사용합니다. 각 몸체의 파일은 `KenshiRaces/items/races/kenshiraces_<character>*` (Leoma 및 Crevan 의 노멀 맵)입니다. 남성 바디는 `male_skeleton` 에, 여성 바디는 `female_skeleton` 에 장착됩니다. 게 몸은 수컷에 맞으므로 게 여왕은 수컷 애니메이션과 목소리를 가진 단일 성별입니다. 소스 모델(Meshy 내보내기, 슬림 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman 및 crabqueen "... - 3ds Max Biped.glb", 기타 `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## 기록

로드된 게임 자체 기록(`RaceRecords`)의 데이터 단계에 추가되었으므로 `KenshiRaces.mod` 는 비어 있고 CI 에는 게임이 필요하지 않습니다.

- RACE_GROUP : 편집자의 순서에 따른 경주 이름과 해당 하위 경주의 RACE입니다.
- RACE (하위 경주): 각 성별의 메시, 색상 텍스처, 노멀 맵 및 부품 맵이 포함된 Greenlander(`17-gamedata.quack` : 통계, 애니메이션, 음성)의 복사본입니다(단일 성별 하위 경주는 두 성별 모두에 대해 하나의 본체를 착용합니다).  두 몸체가 모두 없는 경우 `single gender` ; 머리나 머리카락이 없습니다(머리가 없는 인종은 아무 것도 그리지 않습니다. 몸에는 고유한 머리가 있습니다). `morph num` 0; `no shirts` (셔츠와 바지는 인체의 UV 레이아웃을 칠합니다); 신체 마스크 및 강도 법선 맵이 지워졌습니다. `playable` 와 `editor limits` [ `kenshiraces_editor.xml` ](#character-editor).
- CHARACTER : 하위 인종 및 성별(`female chance` 100 또는 0)의 방랑자(`2205-gamedata.base`) 사본, 키트, 돈 또는 현상금 없음, 모집 대화 포함.
- 엔진: CHARACTER 의 자체 `mesh` 필드가 경주의 신체 메시를 재정의합니다.

<a id="character-editor"></a>
## 캐릭터 에디터

레이스는 게임과 마찬가지로 캐릭터 생성 시 제공됩니다. `KenshiRaces/items/races/kenshiraces_editor.xml` 는 뼈를 움직이는 슬라이더만 유지하므로 인간처럼 장착된 몸체에서 작동합니다(엔진 문서/animation.md, 뼈 스케일). 얼굴에는 머리 크기, 머리 모양 및 목 슬라이더가 포함됩니다(얼굴 포즈 없음: 장착된 머리에는 없음). Body는 인체 슬라이더와 Skin Tone을 보유하고 있으며, 여성의 경우 가슴 ​​크기, 키 및 간격을 사용하여 여성 골격의 `L Boob` / `R Boob` 를 이동하며 여성 신체에 가중치를 부여합니다([race import](race-import.md#bust)). 범위는 인간 범위입니다(`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## 모병

DIALOGUE  `1-KenshiRaces.mod` : 캐릭터가 "다른 손을 사용할 수 있을 것 같습니다."라고 말하면 플레이어는 "우리와 함께하세요"라고 대답합니다. 액션 18(분대 합류)은 캐릭터를 플레이어의 분대에 추가합니다.

<a id="testing-in-game"></a>
## 모집 및 게임 내 테스트

1. 런처나 `data/mods.cfg`에서 Kenshi Races를 활성화합니다.
2. 콘솔에 `spawn leomafemale`을 입력하면 커서 위치에 캐릭터가 생성됩니다. 표에 있는 다른 캐릭터 이름도 사용할 수 있습니다.
3. 캐릭터에게 말을 걸어 분대에 합류시키세요. F10 디버그 메뉴의 Recruit 기능도 사용할 수 있습니다.
4. F10 메뉴의 **Kenshi Races: races**에서 이 모드와 종속 모드가 불러온 적합화된 종족 목록을 확인할 수 있습니다.

<a id="clothing"></a>
## 의복과 방어구

방어구는 같은 스켈레톤을 따라 움직이지만 사람 체형을 기준으로 제작됩니다. `hide parts`를 쓰는 외투, 로브, 재킷은 옷 아래 피부를 숨깁니다. 등딱지나 집게발은 몸에 끼는 방어구를 뚫고 나올 수 있습니다. 몸체를 슬림화해 사람보다 두꺼운 부분을 인간의 체형에 가깝게 줄였습니다. ([race-import.md](race-import.md#slimming)).

[English](../../../docs/kenshi-races.md) · [Kenshi Races](../README.md)
