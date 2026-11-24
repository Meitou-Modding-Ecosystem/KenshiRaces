<a id="kenshi-races"></a>
# Kenshi 레이스

3 개의 새로운 레이스가 캐릭터 편집기, 각각의 하위 레이스, 게임의 인간 스케줄에 (`kenshi.races`, 의존성이 없습니다 : 부하에 추가 된 모든 기록, 몸에 의해 장착 [경주 수입](race-import.md). 그것은 또한 배를 [애니메이션 도서관](animations.md) 이 척추에 있는 각 경주에 대 한 59 클립. Mods는 레이스와 클립을 사용하고, subrases를 추가하거나 자신의 레이스를 추가합니다. [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## 레이스

편집자는 각각의 RACE_GROUP 경주와 그의 RACEs 엔진으로서의 빛(Motor) docs/characters.md “자연 편집자”

| 경주 (RACE_GROUP) | 하이브리드 (RACE) | 성별 | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | 레오마 `10-` | 남성, 여성 | 레오마일 `11-`레오메이즈 `17-` |
| Crevan `31-` | 승무원 `18-` | 남성, 여성 | 크레반마일 `19-`크레아니아 `21-` |
| Crabfolk `32-` | 크라브맨 `12-` | 싱글 | 크라브맨 `13-` |
| | 크라브케인 `14-` | 싱글 | 크라브케인 `15-` |

끝내는 `KenshiRaces.mod`. Subrace 및 CHARACTER 이름은 단어가 낮은 경우, 그래서 콘솔의 `spawn` 인용문 없이 그들을 빼앗아간다. 각 기관의 파일은 `KenshiRaces/items/races/kenshiraces_<character>*` (일반적인 지도에서 Leoma 그리고 Crevan). 남성의 몸은 그들의 `male_skeleton`여성들이 을 `female_skeleton`; 크라브 시체는 남성 균형, 그래서 크라브 여왕은 남성 애니메이션과 목소리와 단일 성입니다. 모델 번호(Meshy 수출, 슬림 1 : 레오마일 "Leonid Bare - 3ds Max Biped.glb", crabman 및 crabqueen "... - 3ds MAX Biped,glb", 다른 `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## 기록은

↑ 가 나 다 라 라 라 라  라 라 라 (`RaceRecords`) 그래서 `KenshiRaces.mod` 배가 텅 비어 있고 CI는 게임을 필요로하지 않습니다 :

- RACE_GROUP: 레이스의 이름과 그것의 하위 레이스 RACEs편집자의 명령에 따라
- RACE [위기] 한 번의 복사본 Greenlander (`17-gamedata.quack`: 통계, 애니메이션, 목소리) 각 성별의 메시지, 색상 텍스트, 정상 지도
그리고 일부 지도 (  은 한 몸을 양쪽으로 착용한다); `single gender` 두 개의 몸을 가지고 있지 않은 경우; 머리 또는 머리 (머리 목록에 머리를 끌지 않는 한; 몸은 그것을 가지고); `morph num` 0; `no shirts` (셔츠와 팬티는 인체의 UV 배열을 그린다); 신체 마스크와 힘은 정상적인 지도를 깨끗하게; `playable` 와 함께 `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTER[중고] 녹음하기 녹음한 녹음기 (`2205-gamedata.base`[중고] 성별과 성별에 관한 질문(`female chance` 100 또는 0), 키트, 돈 또는 보너스 없음
채용 대화
- 엔진 : A CHARACTER자신의 `mesh` 필드는 자신의 경주의 몸의 메시지를 넘어섰다.

<a id="character-editor"></a>
## 캐릭터 편집기

경주는 게임 자체와 같은 캐릭터 창조에 제공됩니다. `KenshiRaces/items/races/kenshiraces_editor.xml` 그것은 단지 뼈를 움직이는 슬라이더를 유지, 그래서 그들은 인체에있는 것처럼 작동 (엔진) docs/animation.md, 뼈 스케일 : 얼굴은 머리 크기, 머리 모양과 목 슬라이더를 유지합니다 (얼굴 포즈는 없습니다 : 묶여있는 머리에는 아무것도 없습니다); 몸은 인간의 몸 슬라이더와 피부 톤을 유지합니다, 가슴 크기, 높이 및 여성을위한 공간으로, 여성의 골격을 움직이는 `L Boob` / `R Boob` 여성의 몸은 ([경주 수입](race-import.md#bust)). 인간은 인간이 되는 것(`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## 고용

DIALOGUE `1-KenshiRaces.mod`: 캐릭터는 "당신은 다른 쌍의 손을 사용할 수 있습니다."라고 대답하고 행동 18 (연합 팀)은 캐릭터를 추가합니다.

<a id="testing-in-game"></a>
## 게임에서 테스트

1. 허용하는 Kenshi 라우터에 있는 레이스 (또는 `data/mods.cfg`).
2. 컨소시엄 : `spawn leomafemale` (또는 어떤 CHARACTER 테이블에서) 코서에 스파이; 대화 모집, 또는 F10 낭비 모집.
3. F10 **Kenshi 레이스 : 레이스** 각 에 맞는 에 대한 (이 모드와 의존자)의 ()을 니다.
4. 새로운 게임 : 편집자의 경주 총알이 도달 Leoma, Crevan 그리고 Crabfolk.

<a id="clothing"></a>
## 옷을 입고

Armour는 신체의 골격을 공유하여 이러한 몸과 함께 움직이지만 인간의 몸 주위에 모델링됩니다. 부분 지도는 물건 아래의 피부를 숨기고 `hide parts` 옷을 입고, 옷을 벗고, 다른 곳에서 사람보다 더 많은 것을 끌어 들이는 ([경주 수입](race-import.md#slimming)). 인간과 멀리 떨어져 있는 모양(, )은 여전히 좁은 무기를 통해 니다.
