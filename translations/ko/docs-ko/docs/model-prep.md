<a id="preparing-a-model"></a>
# 모델 준비

생성된 `.glb` ( Meshy , Hi3D , Blender )를 `<Mod>/Structures/` (건물) 또는 `<Mod>/ItemModels/` (항목)에 들어가기 전에 게임이 처리하는 폴리카운트, 텍스처 크기 및 실제 크기로 가져오는 방법. 가져오기 자체: [structures.md](structures.md) .

명령은 PowerShell의 리포지토리 루트에서 실행됩니다. Python 3이 필요합니다. `prepare-model.py` 에는 `pip install pillow numpy` 도 필요합니다.

<a id="1-inspect"></a>
## 1. 검사

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

크기(X, Y 위쪽, Z), 가장 긴 변, 삼각형, 재질, 각 텍스처의 픽셀 크기 및 MB 를 인쇄합니다. 노드 크기 조정 또는 회전에 대한 참고 사항은 `prepare-model.py` 가 크기를 조정할 수 없음을 의미합니다. 변환이 적용된 Blender 에서 다시 내보냅니다(Object > Apply > All Transforms).

<a id="2-cut-the-polycount"></a>
## 2. 폴리카운트 자르기

| 친절한 | 삼각형 | 텍스처 |
| --- | --- | --- |
| 건물이나 장식 | 5,000~20,000 | 2048 |
| 아이템(보유, 드랍, 상점 내) | 5,000명 미만 | 1024 |

제너레이터는 종종 100,000 이상을 제공합니다. 수입은 150,000 이상이라고 경고합니다. 자르려면:

- ** Meshy :** 위 예산(쿼드 또는 삼각형 토폴로지 모두 작동)에서 대상 폴리카운트를 사용하여 모델을 다시 메시하고 텍스처를 유지한 다음 GLB 로 다시 다운로드합니다.
- ** Blender :** 파일 > 가져오기 > glTF, 메시를 선택하고 Decimate 수정자(접기)를 추가하고 상태 표시줄의 면 수가 예산에 포함될 때까지 비율을 낮춰 적용한 다음 파일 > 내보내기 > 수정자 적용이 설정된 glTF 바이너리( `.glb` ).

충돌은 별도로 단순화되므로( `collisionTriangles` , 기본값 2000) 표시되는 메시를 자르는 것은 프레임 속도와 파일 크기에 관한 것입니다.

<a id="3-choose-the-size"></a>
## 3. 사이즈를 선택하세요

가져오기는 모델을 미터로 취급합니다. 발전기의 크기는 임의적입니다. 실제 크기를 선택하고 모델이 아닌 모델의 설정 파일([structures.md "Settings"](structures.md#settings) )에서 설정합니다.

- 건물: `<model>.json` 의 미터 단위 `"height"` . Kenshi 의 사람들은 키가 약 1.8m이고 실제 크기보다 작으므로 설명하는 크기에 20%를 추가합니다(2.2m 게시물은 `2.64` 가 됨).
- 항목: `"size"` , 가장 긴 면(미터), `<model>.json` 옆에 있는 `ItemModels/` .

<a id="4-shrink-the-textures"></a>
## 4. 텍스처 축소

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

이렇게 하면 한 면당 최대 픽셀 수(건물의 경우 2048, 항목의 경우 1024)만큼 모든 텍스처를 사용하여 파일을 다시 작성하며 일반적으로 수십 MB 에서 몇 개로 줄입니다. 선택적인 세 번째 인수는 설정 파일이 필요하지 않은 경우를 위해 모델의 크기를 가장 긴 면(미터)으로 다시 조정합니다. 가져오기로 인해 더 큰 텍스처도 절반으로 줄어들지만( `textureSize` ) 축소된 파일을 커밋하면 저장소가 작게 유지됩니다.

`model-info.py` 를 다시 실행하여 결과를 확인하세요.

<a id="5-import-and-check"></a>
## 5. 가져오기 및 확인

Play를 사용하여 빌드하고 배포합니다(또는 가져오기만 실행([structures.md "Quick start"](structures.md#quick-start) )). 로그에는 모델당 한 줄이 있습니다.

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

높이나 길이, 삼각형 개수를 확인한 다음 게임에서 확인하세요. 건물을 배치하거나 아이템을 떨어뜨려 집으세요.

<a id="6-commit"></a>
## 6. 커밋

`.glb` , `.json` 및 폴더의 `ids.json` 를 함께 커밋합니다. `ids.json` 는 각 모델의 기록 ID를 고정된 상태로 유지하고 해당 ID로 배치된 건물의 이름을 저장합니다.
