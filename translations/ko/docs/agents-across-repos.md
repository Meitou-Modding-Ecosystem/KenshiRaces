<a id="agents-working-across-repos"></a>
# 여러 저장소에 걸친 변경 조율하기

변경 사항이 적용되는 모든 저장소마다 하나의 에이전트를 제공하고, 어느 저장소가 각 부분을 소유하는지 알려주고, PR을 종속성 순서(엔진, 프레임워크, 모드)로 지정하도록 합니다. 리포지토리당 하나가 아닌 변경당 하나의 에이전트 세션입니다.

<a id="give-the-agent-the-repos"></a>
## 에이전트에게 저장소 제공

- **클라우드 세션:**각 저장소는 세션 또는 프로젝트에 연결되어야 합니다. 에이전트는 이를 나란히 복제하고 각각의 자체 분기에서 작동합니다.
- **PC (원격 제어):**[local-development.md](local-development.md) 의 폴더에 있는 복제본입니다. 에이전트는 `gh repo create` 와 같이 클라우드 세션에서 할 수 없는 작업도 수행할 수 있습니다.

<a id="where-each-part-goes"></a>
## 각 부분이 어디로 가는지

| 변화 | 레포 | PR 안으로 |
| --- | --- | --- |
| 엔진 후크(이벤트, 저장된 필드, 명령) | MeitouClient | `fork` |
| 라이벌리 코어 API 또는 동작 | 라이벌리코어 | `beta` |
| 물류 핵심(노드, 에지, 소유자, 재고, 운송) | 물류핵심 | `beta` |
| 전쟁 조율(물류 전투 시나리오, 스토리나 캐릭터 없음) | 전쟁오케스트레이션 | `beta` |
| 바스트 프론트 근접전 | 바스트프론트 | `beta` |
| 모드의 코드, 데이터 생성기, 테스트, 문서 | 그 모드 저장소 | `beta` |
| 공유 CI, 패키징, 릴리스 게이트, 잠금 대상 | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## 저장소를 넘어서는 변경 주문

1. 엔진 PR 가 `fork` 로 병합되었습니다. 엔진 핀은 업스트림 릴리스 태그로만 이동하므로 이를 필요로 하는 프레임워크 및 모드 PR은 업스트림(VlcekM/ MeitouClient)이 변경 사항을 릴리스할 때까지 기다립니다. 그때까지는 엔진 브랜치를 로컬로 빌드하세요([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. 변경을 수행하고 `engine` 핀을 필요한 릴리스로 이동하는 프레임워크 PR 가 `beta` 로 병합되었습니다. CI 는 베타 시험판을 게시합니다.
3. `meitou.lock.json` 를 해당 시험판으로 이동하고 `engine` 를 잠금 엔진 커밋으로 이동하는 Mod PR 가 `beta` 로 병합되었습니다.
4. 프레임워크 릴리스가 승격되고 태그가 지정된 후: 잠금을 `<framework>-v<version>` 로 이동하는 모드 PR.

각 PR 그 앞의 것을 연결합니다. 파괴적인 틀 API 변경은 프레임워크의 부 버전을 범프하고 모든 종속 저장소를 나열합니다. 각 종속 항목은 자체 잠금을 갖습니다. PR.

체인에서 2단계는 계층당 한 번씩 하향식으로 반복됩니다. Logistics Core 변경이 적용된 다음 War Orchestration PR 가 Logistics가 자체적으로 구축하고 게시하는 것을 잠근 다음 Bast Front PR 가 두 가지를 모두 잠급니다.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## 상담원 지시사항에 붙여넣을 규칙

```markdown
- Repos and owners: engine hooks -> MeitouClient (PR into fork); Rivalry Core -> RivalryCore (beta);
  Logistics Core -> LogisticsCore (beta); War Orchestration -> WarOrchestration (beta); Bast Front -> BastFront (beta);
  mods -> their own repo (beta); shared CI/packaging -> GutenStructure (beta).
- Layers: logistics.core -> war.orchestration -> Bast Front. A layer uses the one above only through its public API and
  a dependencies range. War Orchestration holds no story or character content.
- Land PRs in order: engine, then each framework layer top-down, then the mod. One PR per repo per step. Self-merge each
  when CI is green; link the previous PR.
- A meitou.lock.json lists every framework in the chain (Bast Front locks War Orchestration AND Logistics Core), all on
  one engine commit, each accepted by the others' dependency ranges.
- To build against unmerged framework changes, set MeitouFrameworkSources (the sibling clones' framework .csproj paths, ';'-separated)
  and MeitouEngineDir in the repo's git-ignored Meitou.user.props. Never commit Meitou.user.props.
- A lock points only at an immutable tag: <framework>-<version>-beta.<commit> or <framework>-v<version>, with the
  asset, SHA-256 and engine commit copied from that release's notes. Never the rolling <framework>-beta tag.
- Never commit inside an engine/ submodule. Pin engine/ to an upstream Meitou release tag (vX.Y.Z of VlcekM/MeitouClient), or
  with a lock, to the lock's engine commit. Never pin the fork's fork head or a nightly. Move Directory.Build.rsp and every
  gameVersion floor together, only when the mod needs the newer release (GutenStructure docs/versioning.md).
- After pulling, run tools/Update-Local.ps1 so local builds use the pinned engine.
- Don't merge a PR whose lock names a framework build that isn't published yet.
- Ask before release tags, promotions to main or release, and anything in another author's folder or repo.
- Report every PR link, in landing order.
```

<a id="prompt-template"></a>
## 프롬프트 템플릿

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```

[English](../../../docs/agents-across-repos.md) · [Kenshi Races](../README.md)
