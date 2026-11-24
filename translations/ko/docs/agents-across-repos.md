<a id="agents-working-across-repos"></a>
# 휴식과 함께 일하는 멤버들

각각의 리포트를 제공하고, 각각의 레포트를 소유하고, 그것은 중독 순서에있다 : 엔진, 그 다음 프레임, 그 다음 모드. 변수에 한 명의 에이전트 세션, 리포에 한 명이 아니다.

<a id="give-the-agent-the-repos"></a>
## 공무원에게 휴식을 주어

- **클라우드 세션 :** 각 리포는 세션이나 프로젝트에 연결되어야합니다. 에이전트는 그들을 측면에서 측면으로 클론하고 자신의 지점에서 작업합니다.
각각에 있는
- **원격 컨트롤 (Remote Control) :** 폴더에 있는 클론은 [local-development.md](local-development.md). 그럼에도 불구하고 공무원은 무엇을 할 수 있는가.
클라우드 세션은 할 수 없습니다, 예를 들어 `gh repo create`.

<a id="where-each-part-goes"></a>
## 각 부분이 가는 곳

| 변화 | 리포 | PR에 들어가기 |
| --- | --- | --- |
| 엔진 호크 ( 이벤트, 저장 필드, 명령) | MeitouClient | `fork` |
| 리버트 코어 API 또는 행동 | 리얼리 코어 | `beta` |
| Logistics Core (노드, 옆구리, 소유자, 주식, 운송) | 로지스틱 코어 | `beta` |
| 전쟁 오케스트라 (로지스틱에 대한 전투 시나리오, 이야기 또는 캐릭터 없음) | 전쟁기구 | `beta` |
| 전선 | 바스트프론트 | `beta` |
| 모드 코드, 데이터 생성기, 테스트, 도크 | 이 모드의 리포 | `beta` |
| 공유 CI, 포장, 출시 포트, 잠금 목표 | 혜택 | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## 휴식을 통과하는 변화에 대한 명령

1. PR를 입력합니다. `fork`융합된 엔진 핀은 오프 스트림 출시 태그로만 이동하므로 프레임 워크와 모드 PR가 필요합니다.
오프화이트 (VlcekM/MeitouClient3) 변화를 풀어주고 있다. 그럼에도 불구하고, 그들은 현지에 있는 엔진에 대하여 구축 ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. 프레임 워크 PR는 변화를 일으키는 (그리고 그것을 움직이는) `engine` 핀은 그것이 필요로하는 것을 가지고있는 해방을위한 것입니다), 합병 `beta`. 이 출판
베타 프리레일러
3. 움직이는 모드 PR `meitou.lock.json` 그 후에 그들은 (그리고 그들은) `engine` 엔진의 잠금에 묶여있다), 융합 `beta`.
4. 프레임 워크 릴리스가 홍보되고 표시되면 : 잠금을 움직이는 모드 PR `<framework>-v<version>`.

각 PR는 그것보다 먼저 연결되어 있습니다. 깨진 프레임 워크 API 변화는 프레임의 작은 버전을 삽입하고 각각의 의존 리포를 목록; 각각은 자신의 잠금 PR를 얻습니다.

체인에서, 단계 2는 층에 한 번 반복됩니다 : 로지스틱 코어는 땅을 바꾸고, 전쟁 오케스트라 PR는 로지스티크가 건설하고 출판하는 것을 닫습니다.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## 에이전트의 지시에 붙어있는 규칙

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
## 즉시 템플릿

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
