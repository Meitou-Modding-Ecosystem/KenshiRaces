<a id="structures-from-3d-models"></a>
# 3D 모델의 구조

A 를 넣어 `.glb` 모델 번호 (Meshy, Hi3D, 블렌더 수출) 당신의 모드에서 `Structures` 폴더와 게임 건물은 건물 메뉴에서 배치 할 수있는 건물로 변환 : Ogre 메시지, 압축 된 텍스트, 충돌 및 기록 `<Mod>.mod`. 아니오 FCS, 블렌더, 게임 설치가 필요하지 않습니다.

<a id="quick-start"></a>
## 빠른 시작

1. 모델을 입력하십시오. `<Mod>/Structures/`그럼 G. `MyMod/Structures/watch-tower.glb`.
2. 옵션 : 그 외에, `watch-tower.json` 어떤 것과 함께 [설정을](#settings)그럼 G.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. 일반적으로 사용하고 사용하는 방법([play.md](play.md)• Visual Studio F5, VS 코드 F5 또는 `dotnet run --project Play`. 창고는 하나를 보여준다.
`Structure ...` 모델에 따라 라인
4. 게임에서 : 메뉴를 만들기, 카테고리 `EXTERIOR` (또는 당신이 쓴 것과 같이) 가짜와 함께 `"research": "start"` 그것은 시작부터 알려져있다.
게임, 그리고 존재하는 저장소에서도 (Meitou는 완성 된 연구에서 벗어나고, 항상 포함 `_Default Start`).
5. 그 모델을 따르고, 그것을 `.json` 그리고 `Structures/ids.json` (이제 첫 번째 수입에 대한 설명) 생성된 파일은 출력으로 구성되어 있으며,
헌신하는 것

게임 없음 : `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## 설정을

모든 옵션(`<model>.json`, 코멘트 허용; 알려지지 않은 이름은 오류 그래서 타입은 무시되지 않습니다).

| 설정하기 | 가상화 | 의미는 |
| --- | --- | --- |
| `name` | 파일 이름, `-`/`_` 공간으로서 | 이름을 만들기 메뉴 |
| `description` | `""` | 메뉴 텍스트 만들기 |
| `height` | 모델의 크기 | 완성 된 높이 미터; 모델은 그것과 동등하게 확장됩니다. AI 모델은 자발적인 크기로 나옵니다, 그래서 이것을 설정 |
| `sink` | `0` | 지상에 떨어지는 미터 (그것은 에 흐르는 것을 막는다) |
| `yaw` | `0` | 좌석 좌석을 고 좌석으로 고 |
| `category` | `EXTERIOR` | 메뉴 카테고리 (`building category`이 게임은 (`BUILDINGS`, `CAMPING`, `FARMING`... 또는 새로운 이름 |
| `group` | `""` | `building group`: 건물 공유 하나는 하나의 입력의 변형 |
| `materials` | `5` | 건설 재료는 비용 |
| `buildSpeed` | `2` | `build speed mult` (게임의 텐트 5, 집 1에서 2) |
| `research` | `start` | `start`* 시작부터 알려진 `none`[중고] [중고: [중고]: [중고 : [중고 ] [중고) [중고], [중고, [중고), [중고’, [중고', [중고]] [중고]. RESEARCH 링크 ID 등 `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` 또는 `both` |
| `createsTown` | `true` | 도시 밖으로 나가면 출발점이 시작됩니다. |
| `collision` | `mesh` | `mesh` (모델의 단순화된 복사본을 보자) `box` (그것은 상자에 묶여있다) 또는 `none` (그것을 통해 걷기) |
| `collisionTriangles` | `2000` | 대부분의 삼각형은 `mesh` 충돌이 지속됩니다 (12 ~ 50,000    ) |
| `textureSize` | `2048` | 가장 큰 텍스처 측면 픽셀 (두 개의 힘, 최대 8192) |

<a id="item-models"></a>
## 항목 모델

포인트의 모델을 포인트에 넣어 `<Mod>/ItemModels/` 대신에 (`Structures` 건물이 될 것 같아요) 수입은 그것의 메시지와 텍스처를 옆에 쓴다'와 하나의 MATERIAL_SPECS_CLOTHING 녹음; 당신의 모드 ITEM 그 후에 기록은 그들을 지적합니다. 창고는 하나를 보여준다. `Item ...` 사용해야 할 이름을 가진 모델에 따라 라인.

1. `MyMod/ItemModels/lantern.glb`옵션으로 `lantern.json`: `{ "size": 0.3 }`.
2. 항목 기록 (데이터 생성기 또는 데이터 단계)에서 다음을 설정합니다.

가치가 있는 가치가 있다.
   | --- | --- |
| `mesh` 그리고 `ground mesh` (영어로 발음한 사람들) `<mod>_<model>.mesh`그럼 G. `mymod_lantern.mesh` | | `material` [중고] 히어로 히어 `<950000 + 100 × block>-<Mod>.mod`블록에서 `ItemModels/ids.json`그럼 G. `950000-MyMod.mod` |

첫째, 물질을 보라(`GameDatabase.Find`그리고 그것이 사라질 때 스탠드의 모습을 유지하십시오 : 출시 된 지프, 그리고 배치 된 모드없이 게임을 읽는 테스트, 그것을 가지고 있지 않습니다.
3. 그 모델을 따르고, 그것을 `.json` 그리고 `ItemModels/ids.json`.

| 설정하기 | 가상화 | 의미는 |
| --- | --- | --- |
| `name` | 파일 이름, `-`/`_` 공간으로서 | 물질 기록의 이름 |
| `size` | 모델의 크기 | 가장 긴 측면은 미터; 모델은 그것에 동등하게 확장됩니다 |
| `yaw` | `0` | 좌석 좌석을 고 좌석으로 고 |
| `textureSize` | `1024` | 가장 큰 텍스처 측면 픽셀 (두 개의 힘, 최대 8192) |

- 1 개의 물자 매개 변수 (이제 ITEM 이름 1 `material`); 모델은 게임의 항목 메시지처럼 가장 낮은 점에 서있다.
- 게임은 바닥에 항목을 끌고, 손에, 그리고 자동차 아이콘에 그것의 `mesh` (`ground mesh` 먼저 땅에 앉아 있는 것과
MATERIAL_SPECS_CLOTHING 그녀의 `material` 엔진 이름 `docs/formats/runtime-materials.md` “전화자들” `WorldObjectLayout.ItemMesh`).
- 인벤토리 아이콘은 녹음 세트가 없을 경우 모델에서 추출됩니다 `auto icon image` 가짜와 자신의 `icon`.

<a id="getting-a-good-model"></a>
## 좋은 모델을 만들기 위해

- 질문하기 Meshy 또는 Hi3D A에 대하여 PBR- 텍스트 모델 및 다운로드 방법 GLB. 몇 십만 개의 삼각형이 충분합니다; 로그는 모델을 표시합니다.
150만 명이 넘는 세트로 폴리콘을 자르고 크기를 선택하고 텍스처를 줄이십시오 : [model-prep.md](model-prep.md).
- 이 문서의 값은 값의 값을 + Z로 표시합니다(glTF의 의 의 의; 사용 및 `yaw` 그것을 돌리기 위해서다.
- 각 재료는 자신의 부분이됩니다. 유전자는 일반적으로 하나를 제공합니다; 최대 49.
- 모델은 가장 낮은 지점, 중심에 서있다.

<a id="what-it-writes"></a>
## 무엇을 쓰고 있는가

에 삽입 된 mod 폴더 (`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` (물질에 한 개의 물질이 `_1`, `_2`, ... 몇 가지 경우 : 오그레 `[MeshSerializer_v1.8]`위치, 정상, UV 및
W의 수동성, 게임의 자신의 건물 메시지의 배열.
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: 분산 (BC3), 정상 (BC1, BC3 절단 할 때), 금속성 (BC1), 마이프.
- `structures/<mod>_<model>_col.xml`그럼에도 불구하고 충돌은
- 와 와 와와의 모델 : `structures/<mod>_<model>.mesh` 그리고 그것의 텍스트, 충돌이 없습니다.
- 레코드가 합쳐진다. `<Mod>.mod` 그것에 포함된 다른 모든 것 외에 (데이터 생성기의 기록이 유지되고 이전의 수입이 대체됩니다):
BUILDING한 명은 BUILDING_PART 그리고 MATERIAL_SPEC 물질에 따라, 그리고 열쇠 (연구의 `enable buildings` 건물에 대한 혜택을 받았습니다.) 1 MATERIAL_SPECS_CLOTHING 포인트 모델에 따라

파일 이름은 모드의 이름으로 시작되며, 게임은 각 모드에서 단순한 파일 이름으로 자원을 보입니다.

ID 레코드가 니다 `<n>-<Mod>.mod` n 에서 900000    까지,    ,   까지 (건축 +0, 부품 +1, 재료 +50); 항목 모델은 950000  (재료 +0)  에서 계산 `ItemModels/ids.json`. `Structures/ids.json` 각 모델의 블록을 유지하여 ID는 결코 변경되지 않습니다 : 저장소는 기록의 ID에 따라 위치한 건물을 이름합니다. 블록을 편집하거나 제거하지 마십시오; 모델을 제거하면 그것을 배치 한 게임에서 그것을 제거합니다.

<a id="facts-the-import-relies-on"></a>
## 수입이 의존하는 사실은

게임의 파일에 대한 검토 (Kenshi 1.0.68 데이터) 그리고 엔진의 독자와 함께 :

- **유니폼**: 세계 단위는 0.1 m, Y 위; glTF 그것은 미터, Y 위, 그래서 스케일은 10 (게임의 충돌 파일은 중력 98.1을 사용합니다).
- **재료**: 건물 부분의 모습은 그것의 MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`하지만 메시지의 것이 아니다.
엔진의 이름(Motor Name) `docs/formats/runtime-materials.md`). 게임의 건축 빛 (`data/materials/deferred/objects.hlsl`* 분산을 읽는 방법 RGB 색상과 A 빛으로 (시간) `specular mult`* 정상적인 RGB 은 과 이 을 이며 은 (`material type` 1, `alpha threshold`) 금속에서 R. 가져오는 글자는 gloss = 1 - glTF 그들의 금속성과 금속성 glTF블루 채널의 흐름 glTF MASK 그리고 BLEND 그것은 Cut-out가 될 것입니다.
- **탱크**: 층 층의 바이너마는 크로스(탱크, 정상) × w이며, 게임의 메시지에서 그것은 증가하는 V (**확인된**:
`prisoner_pole.mesh` 86 개의 삼각형, `Camp_tent1.mesh` 1858년부터 1865년까지) glTF 정상적인 지도는 그림을 녹색으로 (V를 줄이기 위해) 그래서 수입은 녹색 채널을 회전하고 게임의 방향을 설정합니다.
- **충돌**: 일부는 `xml collision` PhysX NxuStream2 파일; 수동으로 작성된 최소 파일 충전 (게임의 `Camp_tent1_COL.xml` 그들은
한 상자 입니다) PhysX는 Z 위와 게임지도 (x, y, z)에서 (x, z, -y)로, 그래서 수입은 부분의 포인트를 (x, -z, y)로 작성합니다. 삼각형 메시는 평평한 포인트와 삼각형으로 들어가며, 게임은 그것을 부하로 만듭니다. `docs/formats/collision.md`).
- **필드**: BUILDING 그리고 BUILDING_PART 이 게임의 캠핑 텐트 1의 전체 필드 세트를 가져오십시오 (`18026-small_changes_otto.mod`그래서 아무것도
망설임을 겪고 있는데, 아니요. `functionality`그래서 그것은 평평한 건물입니다.
- **잠금을 해제**: 건물이 건설 메뉴에있을 때 완성 된 연구가 그것을 목록에 넣어 `enable buildings`모든 게임은 시작됩니다
`_Default Start` (`5359-gamedata.base`) 완성된 **확인된** 그때 엔진의 `GameDatabase`, `ResearchGate` 그리고 `BuildingBuildData` 기본 게임 플러스 수입 모드 : 구조가 열려 있으며 세트에 따라 건설 재료를 지불합니다.

<a id="not-covered-yet"></a>
## 아직 덮여 있지 않음

인테리어, 문, 기능 (보관, 침대, 제작) 및 회전 부품.
