<a id="kenshi-races"></a>
# Kenshi 레이스

새로운 경주를 위한 [메이저 클라이언트](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) 게임의 인간 척추, 캐릭터 편집기에서 재생할 수 있습니다 : Leoma 그리고 Crevan (남자와 여자) 그리고 남자 Crabfolk (crabman, crabqueen) 각 인간 골격에 대한 59 개의 애니메이션 라이브러리, 그리고 다른 모드는 자신의 레이스와 subras를 추가합니다. GPL3.0 또는 나중에

- 경주, 그들의 기록과 게임에서 그들을 테스트 : [docs/kenshi-races.md](docs/kenshi-races.md).
- 이러한 레이스를 사용하거나 다른 모드에서 자신의 추가 : [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (공무원 능력
`.claude/skills/kenshi-races-dependent`).
- 애니메이션 라이브러리 (인간 골격에 각 레이스에 대한 59 개의 클립) 및 그것이 어떻게 만들어졌는지에 대한 묘사 : [docs/animations.md](docs/animations.md).
- 굳이 굳게 굳게 `.glb` 신체 (Meshy, Hi3D "3ds Max Biped") 게임의 척추에 : [docs/race-import.md](docs/race-import.md).

- Visual Studio, VS 코드 또는 터미널 (이 리포의 엔진 또는 게임에 배치 된 설치된 라우너)을 사용하여 현지적으로 구축 및 실행하십시오. [docs/play.md](docs/play.md).
- 당신의 기계에 휴식을 설정하십시오: [docs/local-development.md](docs/local-development.md).
- 휴식을 취할 수 있는 변화에 대 한 에이전트 지침 : [docs/agents-across-repos.md](docs/agents-across-repos.md).
- 엔진 핀, 버전, CI 및 출시에 대한 에이전트 규칙 : [CLAUDE.md](../../CLAUDE.md) ( 같은 것처럼 `AGENTS.md`1) 단계별 절차를 수행하는 방법 :
`.claude/skills/meitou-engine-release`.
- 가장 최근의 Meitou 릴리스에 대한 내장 모드를 검사 (CI는 모든 PR에서 그것을합니다): [docs/refcheck.md](docs/refcheck.md).
- Steam 워크샵에 대한 모드를 처음으로 게시하고, 그 다음에 각각의 릴리스에서: [docs/steam-workshop.md](docs/steam-workshop.md).
- 회전 `.glb` 모델 번호(Meshy, Hi3D, 블렌더) 건설 가능한 구조에, 아니 FCS 필요한 것 : [docs/structures.md](docs/structures.md);
생성 된 모델의 폴리콘, 크기 및 텍스처를 먼저 얻으십시오 : [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## 숫자

작업의 종류 (`AddGoalType`, `AddJobType`1) 그리고 대화 코드 (`AddDialogueCondition`, `AddDialogueEffect`각각의 옵션을 사용하여 사용하십시오. ([모드 번호 블록](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi 인종은 아무것도 주장하지 않는다.

| 숫자 | 수영장 | 사용하기 |
| --- | --- | --- |

<a id="layout"></a>
## 레이아웃

- `engine/`: MeitouClient git 하위 모듈으로서, 엔진이 모드 건축을 반대하는 데 묶여 있습니다. `git pull` 그것을 움직이지 않는다 : 후에
각각의 움직임 `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([클론을 유지하는 방법](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`: 모드의 프로젝트 및 `meitou.json`; `KenshiRaces/items/races/`: 지정된 기관 (수입 수출을 위임)
`KenshiRaces.Tests/`: 그것의 테스트 (합성 데이터, 게임 설치가 필요하지 않습니다). `<Mod>.Data/` (선택) : 글을 쓰는 발전기 `<Mod>.mod` 설치된 게임에서 `<Mod>/Structures/` (이제 옵션에 대 한 : `.glb` 건축물에 대 한 수입 된 모델을 배치하고 ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`: 프레임 워크는 mods build against
([프레임 워크](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)3); 프레임워크 업데이트 작업 흐름은 CI가 통과 할 때 새로운 출시 및 합병으로 이동합니다 ([프레임 워크 업데이트](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`: 하나의 클릭으로 구축, 배치 및 실행.
- `tools/`: `Update-Local.ps1`, 현지 끌기 및 엔진 핀 체크 ([클론을 유지하는 방법](docs/local-development.md#keep-clones-current));
`refcheck.sh` 그리고 `RefCheck/`엔지니어링 컨트롤러([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`◇ ◇ □ ◇ △ ◇ ‧ ◇ † ◇ ◄[docs/structures.md](docs/structures.md)); `RaceImport/`신체가 깔끔하게 깔끔해진다([docs/race-import.md](docs/race-import.md)); `model-info.py` 그리고 `prepare-model.py`, 모델 검사 및 텍스트 삭감 ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` 엔진을 가져오고, 그래서 모드는 게임의 설정을 구축합니다 ( 경고는 오류).
- `Directory.Build.rsp` 가장 오래된 메이투가 모드 지원을 출시했을 때 엔진 참조를 니다. `gameVersion` 층 : The
게임은 새로운 엔진 표지판을 참조하는 모드를 거부하고, 하위 모듈은 `1.0.0.0` ([버전화](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). 메이투 유효성 작업 흐름 은 매일 새로운 메이투 릴리스에 대한 모드를 검사합니다 ([메이토우트](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## 부문들

PR에 들어가기 `beta`CI (건설 및 테스트)는 통과해야합니다. `beta` 그것은 촉진되는 `main` 한 번 테스트, 그리고 생산은 PR를 통해 `release`. 각각의 합병 `release` 타이틀은 `<slug>-v<version>` [중고] [중고: [중고 : [중고 ] [중고 에서 [중고에서] [중어] [중개] [중요]`.github/workflows/release.yml`이 표가 존재하지 않는 한; `tools\Tag-Release.ps1` 그것은 매뉴얼의 붕괴입니다.

- 새로운 보조제 및 영구 보호 beta/main/release 리뷰 보기 : [docs/branch-policy.md](docs/branch-policy.md). 템플릿 복사본; GutenStructure은 합의를 통해 설정합니다.
