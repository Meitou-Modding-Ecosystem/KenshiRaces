<a id="engine-references-refcheck"></a>
# 엔진 참조( RefCheck )

모드는 하나의 엔진(이 저장소의 `engine` 하위 모듈)에 대해 구축되며 플레이어가 가지고 있는 Meitou 릴리스에서 재생됩니다. 릴리스가 모드가 호출하는 것의 서명을 변경하면 모드의 소스가 여전히 컴파일될 수 있지만 이미 빌드된 DLL 는 게임에 이전 멤버를 요청하고 `MissingMethodException` 와 함께 런타임에 실패합니다.

이것은 가설이 아닙니다. Meitou 0.21은 `Inventory.Add(ItemInstance)` 에 두 번째 선택적 매개변수( `Add(ItemInstance, bool mainShut = false)` )를 제공했습니다. `bag.Add(item)` 를 호출하는 소스는 두 버전 모두에 대해 컴파일되지만 0.20 이하 버전에 대해 빌드된 DLL 는 0.21에는 더 이상 없는 단일 매개변수 방법을 호출합니다. The Wasteland에서는 플레이 중인 기념품이 깨졌고 예외로 인해 나머지 세션 동안 전체 시스템이 중단된 것을 기억합니다. 어떤 테스트도 이를 포착하지 못했습니다. 테스트는 하나의 엔진에 대해 컴파일됩니다.

<a id="what-it-checks"></a>
## 검사 내용

`tools/RefCheck` 는 빌드된 모드 DLL 의 메타데이터를 읽고 참조하는 모든 엔진 멤버(메소드, 생성자, `Meitou*.dll` 유형의 필드)를 이름 및 매개변수 유형별로 나열하지만 해당 릴리스에는 없습니다. 모드나 게임이 실행되지 않습니다.

`tools/refcheck.sh` 는 저장소에 있는 모든 모드( `meitou.json` 및 `<Folder>/<Folder>.csproj` 가 있는 각 최상위 폴더)를 빌드하고 릴리스의 win-x64 zip을 한 번 다운로드하고( `Meitou*.dll` 및 `0Harmony.dll` 만 유지) 모드의 `meitou.json` 목록 각 어셈블리를 확인합니다.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

출력, 릴리스 및 어셈블리당 한 줄:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

또는 휴식을 위해:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## CI 에서

`Engine references` 작업은 모든 PR 및 푸시의 최신 릴리스에 대해 `tools/refcheck.sh` 를 실행하고 `Release readiness` 는 이를 기다립니다. 이전 엔진에 대해 구축된 DLL 를 제공하는 모드는 `meitou.json` 가 허용하는 각 릴리스에 대해 DLL 도 확인해야 합니다.

<a id="when-it-finds-something"></a>
## 무언가를 발견했을 때

지원하는 모든 릴리스에서 작동하는 방식으로 멤버를 호출합니다. 이전 엔진을 삭제한 경우 최신 엔진에 대해 다시 빌드하거나, 리플렉션을 통해 멤버를 찾고 전달하지 않은 매개변수를 기본값으로 채웁니다. The Wasteland Remembers의 `InventoryCompat` 는 `Inventory.Add` 에 대해 후자를 수행합니다.

엔진에 대한 리플렉션 호출은 확인되지 않습니다. 작성한 곳에서는 부드럽게 실패합니다.
