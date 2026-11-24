<a id="preparing-a-model"></a>
# 모델을 준비하는 방법

어떻게 생성되었는지 `.glb` (Meshy, Hi3D, 믹서) 폴리콘, 텍스트 크기와 실제 세계 크기 게임을 처리하기 전에, 그것은 `<Mod>/Structures/` 건물 또는 `<Mod>/ItemModels/` (이제는 이 와 와이파이의 수입 자체 : [structures.md](structures.md).

명령은 PowerShell의 repo 뿌리에서 실행됩니다. Python 3의 필요성 `prepare-model.py` 또한 필요 `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. 검사

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

그것은 미터 (X, Y 위, Z)의 크기를 인쇄하고, 가장 긴 측면, 삼각형, 재료, 각 텍스트의 픽셀 크기와 MB. 노드 스케일 또는 회전의 의미에 대한 메모 `prepare-model.py` 그것을 구축 할 수 없습니다: 그것을 다시 Blender에서 변환을 적용 (Object > Apply > All Transforms).

<a id="2-cut-the-polycount"></a>
## 2. 폴리콘트를 절단하라

| 친절한 | 삼각형 | 텍스트 |
| --- | --- | --- |
| 건설 또는 장식 | 5,000 ~ 20,000 | 2048 |
| 항목 (잡고, 떨어져, 상점에서) | 5만 명 미만 | 1024 |

발전기는 종종 100,000 이상을 제공합니다; 수입은 150,000 이상을 경고합니다. 그것을 절단하기 위해 :

- **Meshy:** 위의 예산에서 대상 폴리 숫자로 모델을 제거하십시오 (쿼드 또는 삼각형 토폴리오 모두 작동), 구조를 유지하고,
다운로드 하 고 GLB 다시 한 번
- **블렌더 :** 파일 > 수입 glTF, 메시지를 선택하고, 화살표 변형기 (Collapse)를 추가하고, 상태에서 얼굴을 계산할 때까지 비율을 낮추십시오.
바가 예산에 있고, 그것을 적용하고, 파일 > 수출 > glTF 바이너리 (`.glb`* 변호사를 사용하여 사용합니다.

분리된 분리( 분리된 값으로 분리된`collisionTriangles`, 기본 2000), 그래서 눈에 띄는 메시지의 절단은 프레임 속도와 파일 크기입니다.

<a id="3-choose-the-size"></a>
## 3. 크기를 선택하십시오

수입은 모델을 미터로 취급합니다; 발전기의 크기는 자발적입니다. 의 실제 크기를 선택하고 모델의 설정을 파일에 설정하십시오 ([기초.md “설정”](structures.md#settings)이 모델이 아닌 :

- 건물에 대하여 : `"height"` 미터 내에서 `<model>.json`. Kenshi사람들은 약 1.8m 높고 실제 크기 옆에 작은 읽기, 그래서
당신이 설명하고 추가하는 크기 20 % (2.2 m 포스트가됩니다) `2.64`).
- 항목 : `"size"`가장 긴 측면은 미터, `<model>.json` 그 옆에 `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. 텍스처를 녹여서

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

이것은 각각의 텍스트를 통해 측면에 많은 픽셀 (2048 건물, 1024 항목)을 절단합니다. 선택적 인 세 번째 논쟁은 설정 파일이 원하지 않을 때 가장 긴 측면으로 모델을 회전시킵니다. 또한 이중화가 절반 더 큰 텍스트(`textureSize`그러나 짧은 파일을 수행하면 리포가 작습니다.

달리기 `model-info.py` 다시 결과를 확인합니다.

<a id="5-import-and-check"></a>
## 5. 수입 및 체크

그럼에도 불구하고, 그들은 그들로 하여금 그들로부터 벗어나게 하거나, 그들로부터 그들로부터 멀리 떨어져 나갈 수 있게 하였다. [기초.md “빨리 시작”](structures.md#quick-start)). 로고에는 모델 당 하나의 라인이 있습니다:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

높이 또는 길이를 확인하고 삼각형을 계산하고 게임에서 그것을 확인하십시오 : 건물, 또는 떨어져서 항목을 얻습니다.

<a id="6-commit"></a>
## 6. 의무

에 의하여 `.glb`그리고 그녀의 `.json` 그리고 폴더의 `ids.json` 함께 하는 것 `ids.json` 각 모델의 레코드 ID를 고정하고, 그 ID에 의해 지정된 건물의 이름을 저장합니다.
