<a id="kenshi-races"></a>
# Kenshi Races

게임의 인간 골격에 있는 [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) 의 새로운 종족은 캐릭터 편집기에서 플레이할 수 있습니다. Leoma 및 Crevan (남성 및 여성) 및 Crabfolk (게인, 게여왕), 모든 인간 골격 종족에 대한 59개 클립의 애니메이션 라이브러리, 그리고 다른 모드에서 자체 종족 및 하위 종족을 추가하는 프레임워크입니다. GPL -3.0 이상.

- 경주, 기록 및 게임 내 테스트: [docs/kenshi-races.md](docs/kenshi-races.md) .
- 이러한 레이스를 사용하거나 다른 모드에서 자신만의 레이스를 추가하세요: [docs/dependent-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (에이전트 스킬 `.claude/skills/kenshi-races-dependent` ).
- 애니메이션 라이브러리(인체 골격의 모든 종족에 대한 59개 클립) 및 제작 방법: [docs/animations.md](docs/animations.md) .
- 조작된 `.glb` 본체( Meshy , Hi3D "3ds Max Biped")를 게임의 뼈대에 장착: [docs/race-import.md](docs/race-import.md) .

- Visual Studio , VS Code 또는 터미널(이 저장소의 엔진 또는 게임에 배포된 설치된 런처)을 사용하여 로컬로 빌드하고 실행합니다: [docs/play.md](docs/play.md) .
- 컴퓨터에 저장소 설정: [docs/local-development.md](docs/local-development.md) .
- 저장소에 걸쳐 변경 사항에 대해 에이전트에 지시: [docs/agents-across-repos.md](docs/agents-across-repos.md) .
- 엔진 핀, 버전, CI 및 릴리스에 대한 에이전트 규칙: [ CLAUDE .md](../../CLAUDE.md) ( `AGENTS.md` 와 동일); 단계별 절차: `.claude/skills/meitou-engine-release` .
- 최신 Meitou 릴리스에 대해 빌드된 모드를 확인합니다( CI 는 모든 PR 에서 이를 수행합니다): [docs/refcheck.md](docs/refcheck.md) .
- 처음으로 Steam 창작마당에 모드를 게시한 후 모든 릴리스에서 게시: [docs/steam-workshop.md](docs/steam-workshop.md) .
- `.glb` 모델( Meshy , Hi3D , Blender )을 구축 가능한 구조로 전환하면 FCS 가 필요하지 않습니다: [docs/structures.md](docs/structures.md) ; 생성된 모델의 폴리카운트, 크기 및 텍스처를 먼저 가져옵니다: [docs/model-prep.md](docs/model-prep.md) .

<a id="numbers"></a>
## 숫자

작업 유형( `AddGoalType` , `AddJobType` ) 및 대화 코드( `AddDialogueCondition` , `AddDialogueEffect` )는 로드된 모든 모드([mod 번호 블록](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) )에서 공유됩니다. Kenshi Races 는 아무 것도 주장하지 않습니다.

| 숫자 | 수영장 | 사용 |
| --- | --- | --- |

<a id="layout"></a>
## 공들여 나열한 것

- `engine/` : MeitouClient 는 git 하위 모듈로, 모드 빌드에 대한 커밋 엔진에 고정됩니다. `git pull` 는 `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` 를 실행할 때마다 이동하지 않습니다([현재](docs/local-development.md#keep-clones-current) 클론 유지).
- `KenshiRaces/` : 모드의 프로젝트 및 `meitou.json` ; `KenshiRaces/items/races/` : 장착된 본체(확정된 가져오기 출력). `KenshiRaces.Tests/` : 테스트(합성 데이터, 게임 설치가 필요하지 않음). `<Mod>.Data/` (선택 사항): 설치된 게임에서 `<Mod>.mod` 를 작성하는 생성기입니다. `<Mod>/Structures/` (선택 사항): 배포 시 구축 가능한 구조물로 가져온 `.glb` 모델([docs/structures.md](docs/structures.md) ).
- `meitou.lock.json` : 프레임워크는 ([framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) )에 대한 모드 빌드를 릴리스합니다. 프레임워크 업데이트 워크플로는 이를 새 릴리스로 이동하고 CI 가 통과하면 병합합니다([프레임워크 업데이트](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ).
- `Play/` : 원클릭 빌드, 배포 및 실행.
- `tools/` : `Update-Local.ps1` , 로컬 풀 및 엔진 핀 확인([현재 클론 유지](docs/local-development.md#keep-clones-current) ); `refcheck.sh` 및 `RefCheck/` , 엔진 참조 확인([docs/refcheck.md](docs/refcheck.md) ); `StructureImport/` , 모델 가져오기([docs/structures.md](docs/structures.md) ); `RaceImport/` , 본체 피팅([docs/race-import.md](docs/race-import.md) ); `model-info.py` 및 `prepare-model.py` , 모델 검사 및 텍스처 축소([docs/model-prep.md](docs/model-prep.md) ).
- `Directory.Build.props` 는 엔진을 가져오므로 모드는 게임 설정으로 빌드됩니다(경고는 오류임).
- `Directory.Build.rsp` 는 가장 오래된 Meitou 가 각 `gameVersion` 층과 동일한 모드 지원을 릴리스할 때 엔진 참조에 스탬프를 찍습니다. 게임은 최신 엔진 스탬프를 참조하는 모드를 거부하고 하위 모듈은 `1.0.0.0` ([](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) 버전 관리)를 빌드합니다. Meitou 호환성 워크플로는 새로운 Meitou 릴리스([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) )에 대해 매일 모드를 확인합니다.

<a id="branches"></a>
## 지점

`beta` 에 대한 PR; CI (빌드 및 테스트)를 통과해야 합니다. `beta` 는 플레이 테스트를 거쳐 `main` 로 승격되며, 생산은 PR 를 거쳐 `release` 로 진행됩니다. `release` 로의 각 병합은 `<slug>-v<version>` 태그를 지정하고 해당 태그가 존재하지 않는 한 모드의 zip( `.github/workflows/release.yml` )을 게시합니다. `tools\Tag-Release.ps1` 는 수동 대체입니다.

- 새로운 저장소 브랜치 보호 및 영구 베타/메인/릴리스 검토: [docs/branch-policy.md](docs/branch-policy.md) . 템플릿 복사 소스; GutenStructure 는 조정을 통해 설정을 적용합니다.
