<a id="engine-references-refcheck"></a>
# 엔진 참조 (RefCheck)

모드는 하나의 엔진에 대항하여 구축됩니다 (이 repo의 `engine` 하위 모듈) 그리고 플레이어가 가지고있는 무슨 메이투를 풀어. 릴리스가 모드 호출의 서명을 변경할 때, 모드의 출처는 여전히 컴파일 될 수 있습니다. DLL 이미 구축 된     ☆ ☆  ◇ ◇ ☆ ◇  △ ◇ △ △ ☆ △ `MissingMethodException`.

이것은 가설적이지 않습니다. 메이투 0.21 선물 `Inventory.Add(ItemInstance)` 두 번째, 선택적 인 파라미터 (`Add(ItemInstance, bool mainShut = false)`). 소리로 불리는 소리 `bag.Add(item)` 각각의 버전을 사용하지만, A DLL 0.20 또는 그 이상에 대 한 구축 하나의 매개 변수 방법을 호출, 0.21 더 이상 없습니다. The Wasteland은 플레이에 대한 보증금을 깨뜨렸고, 예외는 다른 세션 동안 전체 시스템을 중단했습니다. 테스트가 없었다 : 테스트는 하나의 엔진에 대 한 컴파일.

<a id="what-it-checks"></a>
## 무엇을 확인하는가

`tools/RefCheck` 건설된 모드를 읽는다. DLL's 메타데이터와 그것의 참조 엔진의 각 구성원 목록 (방법, 건축가, 종류에 필드 `Meitou*.dll`이름과 파라미터 유형에 따라, 특정 릴리스가 없습니다. 모드나 게임은 아무것도 실행하지 않습니다.

`tools/refcheck.sh` 각 모드를 리포에 구축합니다 (모든 상단 수준의 폴더와 함께 `meitou.json` 그리고 `<Folder>/<Folder>.csproj`윈-x64 zip 한 번 다운로드 (만 유지) `Meitou*.dll` 그리고 `0Harmony.dll`각 모드의 모드를 검사합니다. `meitou.json` 목록을

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

출력, 출력 및 모임 당 하나의 라인:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

혹은 휴식을 취하려면:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## 그 안에서

그들의 `Engine references` 일자리 달리기 `tools/refcheck.sh` 모든 PR와 압박에 대한 최신 출시, 그리고 `Release readiness` 그것을 기다리고 있습니다. 배를 타고 다니는 한 사람은 A DLL 더 오래된 엔진에 대하여 만들어진 것은 또한 DLL 각각의 자유를 위해서 `meitou.json` 허용할 수 있습니다

<a id="when-it-finds-something"></a>
## 뭔가를 발견할 때

당신이 지원하는 모든 릴리스에서 작동하는 방식으로 회원을 호출하십시오 : 당신이 오래된 엔진을 떨어 뜨리면, 또는 당신이 통과하지 않는 매개 변수를 찾을 수 있습니다. 잔인한 기억이 떠오른다" `InventoryCompat` 그 후에 그들은 `Inventory.Add`.

엔진에 대한 반사 통화는 확인되지 않습니다 : 그들은 당신이 그들을 작성하는 곳에서 부드럽게 실패합니다.
