# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · [Deutsch](../de/README.md) · [Español](../es/README.md) · [Italiano](../it/README.md) · [Português do Brasil](../pt-BR/README.md) · [Русский](../ru/README.md) · [日本語](../ja/README.md) · 한국어 · [简体中文](../zh-CN/README.md)

Kenshi Races는 [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient)의 캐릭터 편집기에 플레이 가능한 종족 세 가지를 추가합니다. Leoma와 Crevan은 남성과 여성 몸체를 모두 제공하며, Crabfolk에는 `crabman`과 `crabqueen` 두 가지가 있습니다. 모두 게임의 인간형 스켈레톤을 사용합니다. 같은 스켈레톤을 쓰는 모든 종족에 적용할 수 있는 애니메이션 59종과 다른 모드에서 고유 종족 및 하위 종족을 추가할 수 있는 프레임워크도 포함합니다. GPL-3.0-or-later.

## 가이드

- [종족과 레코드, 게임 내 테스트](docs/kenshi-races.md)
- [다른 모드에서 프레임워크를 사용하고 하위 종족 추가하기](docs/depending-on-kenshi-races.md)
- [애니메이션 59종과 제작 과정](docs/animations.md)
- [GLB 몸체를 게임의 스켈레톤에 맞추기](docs/race-import.md)
- [Visual Studio, VS Code 또는 터미널에서 빌드하고 실행하기](docs/play.md)
- [로컬 저장소 설정 및 업데이트](docs/local-development.md)
- [여러 저장소에 걸친 변경 조율하기](docs/agents-across-repos.md)
- [새 Meitou 릴리스와의 호환성 확인](docs/refcheck.md)
- [Steam 창작마당에 게시하기](docs/steam-workshop.md)
- [FCS 없이 GLB 모델을 건축물로 가져오기](docs/structures.md)
- [폴리곤 수, 크기 및 텍스처 조정](docs/model-prep.md)
- [브랜치 보호 및 검토 규칙](docs/branch-policy.md)
- [저장소 규칙 및 에이전트 작업 절차](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## 공유 식별자

로드된 모든 모드는 작업 유형과 대화 코드의 번호 공간을 공유합니다. Kenshi Races가 예약한 번호는 없습니다. (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## 저장소 구성

- `engine/` — 모드를 빌드하는 커밋에 고정된 MeitouClient 서브모듈입니다. pull 이후에는 항상 아래 명령으로 갱신하세요. [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — 모드 프로젝트와 매니페스트입니다. 스켈레톤에 맞춘 몸체는 저장소에 포함됩니다. 테스트에는 합성 데이터를 사용하므로 게임 설치가 필요하지 않습니다.
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — 선택적 프로젝트는 설치된 게임에서 모드 데이터를 생성하거나 배포할 때 GLB 모델을 건축물로 가져옵니다. [StructureImport](docs/structures.md)
- `meitou.lock.json` — 프레임워크 버전을 고정합니다. 업데이트 워크플로는 새 버전을 반영하고 CI를 통과하면 병합합니다. [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — 한 번의 실행으로 빌드, 배포 및 게임 시작을 수행합니다.
- `tools/` — 동기화, 엔진 참조 검사, 건축물 가져오기, 몸체 맞춤, 텍스처 준비에 사용하는 도구입니다. [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — 엔진의 빌드 설정을 가져옵니다. 컴파일 경고는 오류로 처리합니다.
- `Directory.Build.rsp` — 엔진 참조를 모드가 지원하는 가장 오래된 버전으로 설정합니다. 게임은 현재 엔진보다 높은 버전을 참조하는 모드를 거부합니다. 서브모듈에는 기본 어셈블리 버전이 적용됩니다. (`gameVersion`, `1.0.0.0`).  새 Meitou 릴리스와의 호환성은 매일 검사합니다. [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## 브랜치

PR의 대상은 beta이며 빌드와 테스트를 통과해야 합니다. 게임에서 확인한 뒤 beta를 main으로 승격할 수 있습니다. 제품 배포는 release 대상 PR을 거칩니다. release에 병합할 때 버전 태그가 아직 없으면 태그를 만들고 ZIP을 게시합니다. 태그 스크립트는 수동 대체 수단입니다. (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
