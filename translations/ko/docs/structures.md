<a id="structures-from-3d-models"></a>
# FCS 없이 GLB 모델을 건축물로 가져오기

`.glb` 모델(Meshy, Hi3D, Blender 내보내기)을 모드의 `Structures` 폴더에 넣으면 플레이 빌드가 이를 빌드 메뉴에서 배치할 수 있는 건물(오우거 메시, 압축된 텍스처, 충돌 및 `<Mod>.mod` 의 기록)로 바꿉니다. FCS 없음, Blender 없음, 가져오기 자체에 게임 설치가 필요하지 않습니다.

<a id="quick-start"></a>
## 빠른 시작

1. 모델을 `<Mod>/Structures/` 에 넣으세요. `MyMod/Structures/watch-tower.glb`.
2. 선택 사항: 옆에 [설정](#settings) 가 있는 `watch-tower.json`, 예:

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. 평소대로 빌드하고 배포합니다([play.md](play.md)): Visual Studio F5, VS Code F5 또는 `dotnet run --project Play`. 빌드 로그에는 모델당 하나의 `Structure ...` 라인이 표시됩니다.
4. 게임 내: 빌드 메뉴, 카테고리 `EXTERIOR` (또는 사용자가 설정한 카테고리). 기본 `"research": "start"` 를 사용하면 게임 시작부터 기존 저장에서도 알려집니다(Meitou 는 항상 `_Default Start` 를 포함하는 완료된 연구에서 잠금 해제를 수행합니다).
5. `.json` 및 `Structures/ids.json` 모델(첫 번째 가져오기에 작성됨)을 커밋합니다. 생성된 파일은 빌드 출력이며 커밋되지 않습니다.

재생 없음: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## 설정

모두 선택 사항입니다(`<model>.json`, 주석 허용, 알 수 없는 이름은 오류이므로 오타가 간과되지 않습니다).

| 환경 | 기본 | 의미 |
| --- | --- | --- |
| `name` | 파일 이름, 공백으로 `-` / `_` | 빌드 메뉴의 이름 |
| `description` | `""` | 메뉴 텍스트 작성 |
| `height` | 모델 본인 사이즈 | 완성 높이(미터); 모델의 크기가 균등하게 조정됩니다. AI 모델은 임의의 크기로 제공되므로 이것을 설정하십시오. |
| `sink` | `0` | 미터가 땅에 가라앉음(경사면에 떠다니는 것을 방지) |
| `yaw` | `0` | 수직축을 중심으로 각도 회전 |
| `category` | `EXTERIOR` | 빌드 메뉴 카테고리(`building category`): 게임의(`BUILDINGS`, `CAMPING`, `FARMING`,...) 또는 새 이름 |
| `group` | `""` | `building group` : 하나를 공유하는 건물은 하나의 항목의 변형입니다. |
| `materials` | `5` | 건축 자재 비용 |
| `buildSpeed` | `2` | `build speed mult` (게임 내 텐트 5개, 주택 1~2개) |
| `research` | `start` | `start` : 처음부터 알려져 있습니다. `none` : 아무것도 잠금 해제되지 않습니다(귀하의 기록이 잠금 해제됨). 또는 RESEARCH 문자열 ID입니다. 예: `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` 또는 `both` |
| `createsTown` | `true` | 마을 밖에 배치하면 전초기지가 시작됩니다. |
| `collision` | `mesh` | `mesh` (모델의 단순화된 복사본), `box` (경계 상자) 또는 `none` (보행 경로) |
| `collisionTriangles` | `2000` | `mesh` 충돌로 인해 유지되는 대부분의 삼각형(12~50,000) |
| `textureSize` | `2048` | 가장 큰 텍스처 면(픽셀)(2의 거듭제곱, 최대 8192) |

<a id="item-models"></a>
## 아이템 모델

대신 `<Mod>/ItemModels/` 에 아이템의 모델을 넣으세요(`Structures` 는 건물로 만듭니다). 가져오기는 구조와 하나의 MATERIAL_SPECS_CLOTHING 레코드 옆에 메시와 텍스처를 씁니다. 그러면 모드의 ITEM 레코드가 이를 가리킵니다. 빌드 로그에는 사용할 이름과 함께 모델당 하나의 `Item ...` 라인이 표시됩니다.

1. `MyMod/ItemModels/lantern.glb`, 선택적으로 `lantern.json` : `{ "size": 0.3 }`.
2. 항목 기록(데이터 생성기 또는 데이터 단계)에서 다음을 설정합니다.

| 필드 | 가치 |
   | --- | --- |
| `mesh` 및 `ground mesh` (파일 이름) | `<mod>_<model>.mesh`, 예: `mymod_lantern.mesh` | | `material` (참조) | `<950000 + 100 × block>-<Mod>.mod`, `ItemModels/ids.json` 의 블록, 예: `950000-MyMod.mod` |

먼저 자료를 찾아보고(`GameDatabase.Find`) 누락된 경우 대체 기능을 유지하십시오. 릴리스된 zip 및 배포된 모드 없이 게임을 읽는 테스트는 없습니다.
3. `.json` 및 `ItemModels/ids.json` 모델을 커밋합니다.

| 환경 | 기본 | 의미 |
| --- | --- | --- |
| `name` | 파일 이름, 공백으로 `-` / `_` | 자료 기록의 이름 |
| `size` | 모델 본인 사이즈 | 가장 긴 면(미터); 모델의 크기가 균등하게 조정됩니다. |
| `yaw` | `0` | 수직축을 중심으로 각도 회전 |
| `textureSize` | `1024` | 가장 큰 텍스처 면(픽셀)(2의 거듭제곱, 최대 8192) |

- 항목 모델당 하나의 재료(ITEM 는 하나의 `material` 를 지정함) 모델은 게임의 아이템 메쉬처럼 가장 낮은 지점, 중앙에 서 있습니다.
- 게임은 `mesh` (지상에서 먼저 `ground mesh`) 및 `material` 이름(엔진 `docs/formats/runtime-materials.md` "발신자", `WorldObjectLayout.ItemMesh`)이 있는 MATERIAL_SPECS_CLOTHING 를 사용하여 손과 자동 아이콘에 지상에 항목을 그립니다.
- 레코드가 `auto icon image` false 와 자체 `icon` 를 설정하지 않는 한 인벤토리 아이콘은 모델에서 그려집니다.

<a id="getting-a-good-model"></a>
## 좋은 모델을 얻다

- PBR 텍스처 모델을 Meshy 또는 Hi3D 에 문의하고 GLB 로 다운로드하세요. 수만 개의 삼각형이면 충분합니다. 로그에는 150,000개가 넘는 모델이 표시됩니다. 폴리카운트 자르기, 크기 선택 및 텍스처 축소: [model-prep.md](model-prep.md).
- 모델의 앞면은 파일에서 +Z를 향합니다(glTF의 규칙). `yaw` 를 사용하여 돌립니다.
- 각 재료는 그 자체의 부분이 됩니다. 생성기는 일반적으로 하나를 제공합니다. 최대 49개.
- 모델은 중앙의 가장 낮은 지점에 서 있습니다.

<a id="what-it-writes"></a>
## 무엇을 쓰는가

배포된 mod 폴더(`<Kenshi>/mods/<Mod>/`)에:

- `structures/<mod>_<model>.mesh` (머티리얼당 하나, `_1`, `_2`,...여러 개일 경우): 오우거 `[MeshSerializer_v1.8]`, 위치, 법선, UV 및 접선과 w의 손 모양, 게임 자체 건물 메시의 레이아웃.
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds` : 확산(BC3), 일반(잘라낸 경우 BC1, BC3), 금속성(BC1), 밉 포함.
- `structures/<mod>_<model>_col.xml` : 충돌.
- 아이템 모델: `structures/<mod>_<model>.mesh` 및 해당 텍스처의 경우 충돌이 없습니다.
- 기록은 그 안의 다른 항목 옆에 `<Mod>.mod` 로 병합되었습니다(데이터 생성기의 기록은 유지되고 이전 가져오기의 기록은 대체됨): BUILDING, 재료당 하나의 BUILDING_PART 및 MATERIAL_SPEC 및 잠금 해제(연구의 `enable buildings` 가 건물을 얻습니다) 품목 모델당 하나의 MATERIAL_SPECS_CLOTHING.

게임은 모든 모드에서 기본 파일 이름으로 리소스를 찾기 때문에 파일 이름은 모드 이름으로 시작됩니다.

레코드 ID는 `<n>-<Mod>.mod` 이며 n은 900000 이상, 모델당 블록은 100입니다(건물 +0, 부품 +1, 자재 +50). 아이템 모델은 `ItemModels/ids.json` 에서 950000(재료 +0)부터 계산됩니다. `Structures/ids.json` 는 각 모델의 블록을 유지하므로 ID가 변경되지 않습니다. 저장은 레코드 ID로 배치된 건물의 이름을 지정합니다. 블록을 편집하거나 제거하지 마십시오. 모델을 제거하면 모델을 배치한 게임에서 해당 건물이 제거됩니다.

<a id="facts-the-import-relies-on"></a>
## 가져오기가 의존하는 사실

게임 파일(Kenshi 1.0.68 데이터) 및 엔진 리더를 통해 확인했습니다.

- **단위**: 세계 단위는 0.1m, Y는 위쪽입니다. glTF는 미터이고 Y는 위쪽이므로 스케일은 10입니다(게임의 충돌 파일은 중력 98.1을 사용합니다).
- **재료**: 건물 부품의 모양은 메시의 재료 이름(엔진 `docs/formats/runtime-materials.md`)이 아니라 MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`)입니다. 게임의 건물 셰이더(`data/materials/deferred/objects.hlsl`)는 확산 RGB 를 색상으로, A를 광택(시간 `specular mult`)으로, 법선 RGB 를 접선 공간 법선으로, A를 컷아웃 마스크(`material type` 1, `alpha threshold`)로, 금속성을 R에서 읽습니다. 가져오기에서는 glTF의 파란색 채널에서 광택 = 1 - glTF 거칠기 및 금속성을 씁니다. glTF MASK 및 BLEND 는 컷아웃이 됩니다.
- **접선**: 셰이더의 바이노멀은 교차(접선, 법선) × w이며, 게임의 메시에서는 V가 증가하는 방향을 가리킵니다(**확인됨**: 86개 삼각형 중 `prisoner_pole.mesh` 86개, 1865년 중 `Camp_tent1.mesh` 1858개). glTF 노멀 맵은 이미지 위쪽에 녹색을 가리키므로(V 감소) 가져오기는 녹색 채널을 둥글게 바꾸고 게임 방식을 설정합니다.
- **충돌**: 부품의 `xml collision` 는 PhysX NxuStream2 파일입니다. 손으로 작성한 최소 파일이 로드됩니다(게임의 `Camp_tent1_COL.xml` 는 하나의 상자입니다). PhysX는 Z 위쪽이고 게임은 (x, y, z)를 (x, z, -y)에 매핑하므로 가져오기는 부품의 포인트를 (x, -z, y)로 기록합니다. 삼각형 메쉬는 일반 점과 삼각형으로 들어갑니다. 게임은 로드 시 이를 요리하고, Meitou 는 이를 있는 그대로 읽습니다(엔진 `docs/formats/collision.md`).
- **필드**: BUILDING 및 BUILDING_PART 는 게임 캠핑 텐트 1(`18026-small_changes_otto.mod`)의 전체 필드 세트를 가지고 있으므로 기본값으로 떨어지는 것은 없습니다. `functionality` 가 없으므로 평범한 건물입니다.
- **잠금 해제**: 완료된 연구가 `enable buildings` 에 나열되면 건물이 건설 메뉴에 표시됩니다. 모든 게임은 `_Default Start` (`5359-gamedata.base`) 완료로 시작됩니다. 기본 게임과 가져온 모드를 통해 엔진의 `GameDatabase`, `ResearchGate` 및 `BuildingBuildData` 로 **검증**되었습니다. 구조는 잠금 해제되었으며 건축 자재는 세트대로 비용이 듭니다.

<a id="not-covered-yet"></a>
## 아직 다루지 않음

인테리어, 문, 기능(보관함, 침대, 제작) 및 회전 부품.

[English](../../../docs/structures.md) · [Kenshi Races](../README.md)
