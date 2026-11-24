<a id="agents-working-across-repos"></a>
# Agentes que trabalham em repouso

Dê um agente para cada toque de mudança, diga que o repo possui cada parte, e tem o PR em ordem de dependência: o motor, então o quadro, então o mod. Uma sessão por mudança, não uma por repo.

<a id="give-the-agent-the-repos"></a>
## Dê ao agente o repouso

- **Sessão em nuvem:** Cada repo deve ser anexado à sessão ou ao projeto. O agente clona-os de lado para lado e trabalha em sua própria rama
em cada um.
- **O seu PC (Controle remoto):** Os clones nas folhas de [local-development.md](local-development.md). O agente também pode fazer
As sessões de nuvem não podem ser, como `gh repo create`.

<a id="where-each-part-goes"></a>
## Onde cada parte vai

| Mudança | Repo | PR em |
| --- | --- | --- |
| Motor hook (evento, campo salvo, comando) | MeitouClient | `fork` |
| Rivalry Core API ou comportamento | Rivalho | `beta` |
| Logística Core (node, bordas, proprietários, estoque, transporte) | LogísticaCore | `beta` |
| Orquestração de guerra (escenários de luta sobre a logística, sem história ou personagens) | WarOrquestração | `beta` |
| Bast Frente Melee | O BastFront | `beta` |
| Código de um mod, gerador de dados, testes, docs | O repo do mod | `beta` |
| CI compartilhado, embalagem, porta de libertação, alvo de fechamento | Gutenstrutura | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Uma mudança que atravessa o descanso

1. O PR entra em `fork`E que se misturam. Os pins de motor vão apenas para os tags de lançamento, então os PRs de frame e modos que precisam esperar até
Apresentação (VlcekM/MeitouClientlibertação da mudança. Até então, construir contra a faixa de motor localmente ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. PR Framework que faz a mudança (e move a `engine` Pin para a libertação que tem o que precisa), unido em `beta`. Publicou
A premiação beta.
3. O PR que se move `meitou.lock.json` A partir daí, a primeira (e `engine` para o motor do lock compromete), misturado em `beta`.
4. Após o lançamento do framework é promovido e marcado: um mod PR que mova o bloqueio para `<framework>-v<version>`.

Todos os PR ligam-se a um antes. Um quadro quebrantado API A mudança é a versão mais pequena do quadro e lista cada repo; cada dependente recebe seu próprio PR.

Em uma cadeia, o passo 2 repete uma vez por camada, para baixo: uma mudança de logística, então uma Orquestração de PR fecha que a Logística cria e publica, então a Bast Front PR fecha ambos.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Regras para inserir nas instruções do agente

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
## Temporada Imediatamente

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
