<a id="agents-working-across-repos"></a>
# Agentes trabalhando em repositórios

Dê a um agente cada repositório que a mudança toca, diga qual repositório possui cada parte e faça com que ele receba PRs em ordem de dependência: mecanismo, depois estrutura e, em seguida, mod. Uma sessão de agente por mudança, não uma por repositório.

<a id="give-the-agent-the-repos"></a>
## Dê ao agente os repositórios

- **Sessão na nuvem:** cada repositório deve ser anexado à sessão ou projeto. O agente os clona lado a lado e trabalha em sua própria filial em cada um.
- **Seu PC (Controle Remoto):** os clones nas pastas de [local-development.md](local-development.md) . O agente também pode fazer o que as sessões na nuvem não podem, como `gh repo create` .

<a id="where-each-part-goes"></a>
## Para onde vai cada parte

| Mudar | Repositório | PR em |
| --- | --- | --- |
| Gancho do mecanismo (evento, campo salvo, comando) | MeitouClient | `fork` |
| Rivalry Core API ou comportamento | RivalidadeCore | `beta` |
| Núcleo Logístico (nós, arestas, proprietários, estoque, transportes) | Núcleo de Logística | `beta` |
| Orquestração de Guerra (cenários de combate em logística, sem história ou personagens) | Orquestração de Guerra | `beta` |
| Corpo a corpo Bast Front | BastFront | `beta` |
| Código de um mod, gerador de dados, testes, documentos | o repositório daquele mod | `beta` |
| CI compartilhado, embalagem, portão de liberação, alvo de bloqueio | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Pedido de uma mudança que cruza repositórios

1. Motor PR em `fork` , mesclado. Os pinos do mecanismo se movem apenas para tags de lançamento upstream, portanto, os PRs de estrutura e mod que precisam deles aguardam até que o upstream (VlcekM/ MeitouClient ) libere a alteração. Até então, construa localmente na ramificação do mecanismo ([local-development.md](local-development.md#change-the-engine-and-a-mod-together) ).
2. Framework PR que faz a alteração (e move seu pino `engine` para a versão que tem o que precisa), mesclado em `beta` . CI publica o pré-lançamento beta.
3. Mod PR que move `meitou.lock.json` para esse pré-lançamento (e `engine` para o commit do mecanismo de bloqueio), mesclado em `beta` .
4. Após o lançamento do framework ser promovido e marcado: um mod PR que move o bloqueio para `<framework>-v<version>` .

Cada PR liga o anterior. Uma alteração significativa na estrutura API ultrapassa a versão secundária da estrutura e lista todos os repositórios dependentes; cada dependente recebe seu próprio bloqueio PR .

Em uma cadeia, a etapa 2 se repete uma vez por camada, de cima para baixo: uma mudança no Núcleo de Logística ocorre, em seguida, uma Orquestração de Guerra PR bloqueia essa construção de Logística e publica a sua própria, e então a Frente Bast PR bloqueia ambos.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Regras para colar nas instruções do agente

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
## Modelo de prompt

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
