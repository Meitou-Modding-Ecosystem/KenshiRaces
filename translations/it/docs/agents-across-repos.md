<a id="agents-working-across-repos"></a>
# Agenti che lavorano su repository

Assegna a un agente ogni repository toccato dalla modifica, digli quale repository possiede ciascuna parte e chiedigli di ottenere PR in ordine di dipendenza: motore, quindi framework, quindi mod. Una sessione dell'agente per modifica, non una per repository.

<a id="give-the-agent-the-repos"></a>
## Dai all'agente i pronti contro termine

- **Sessione cloud:** ogni repository deve essere allegato alla sessione o al progetto. L'agente li clona fianco a fianco e lavora sul proprio ramo in ciascuno.
- **Il tuo PC (telecomando):** i cloni nelle cartelle da [local-development.md](local-development.md) . L'agente può anche fare ciò che le sessioni cloud non possono fare, ad esempio `gh repo create` .

<a id="where-each-part-goes"></a>
## Dove va ogni parte

| Modifica | Repo | PR in |
| --- | --- | --- |
| Hook del motore (evento, campo salvato, comando) | MeitouClient | `fork` |
| Rivalità Core API o comportamento | RivalitàCore | `beta` |
| Logistics Core (nodi, bordi, proprietari, stock, trasporti) | LogisticaCore | `beta` |
| Orchestrazione di guerra (scenari di combattimento logistici, senza storia o personaggi) | WarOrchestration | `beta` |
| Mischia frontale Bast | BastFront | `beta` |
| Il codice di una mod, un generatore di dati, test, documenti | il repository di quella mod | `beta` |
| CI condiviso, imballaggio, cancello di rilascio, bersaglio di blocco | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Ordine per una modifica che attraversa i pronti contro termine

1. Motore PR in `fork` , unito. I pin del motore si spostano solo sui tag di rilascio upstream, quindi il framework e i PR mod che ne hanno bisogno attendono fino a quando upstream (VlcekM/ MeitouClient ) rilascia la modifica. Fino ad allora, compila localmente sul ramo del motore ([local-development.md](local-development.md#change-the-engine-and-a-mod-together) ).
2. Il framework PR che apporta la modifica (e sposta il suo pin `engine` sulla release che ha ciò di cui ha bisogno), confluito in `beta` . CI pubblica la prerelease della beta.
3. Mod PR che sposta `meitou.lock.json` a quella prerelease (e `engine` al commit del motore di blocco), fuso in `beta` .
4. Dopo il rilascio del framework viene promosso e taggato: un mod PR che sposta il blocco su `<framework>-v<version>` .

Ogni PR collega quello precedente. Una modifica di rilievo al framework API supera la versione secondaria del framework ed elenca tutti i repository dipendenti; ogni dipendente ottiene il proprio lucchetto PR .

In una catena, il passaggio 2 si ripete una volta per livello, dall'alto verso il basso: un cambiamento del Logistics Core avviene, quindi un War Orchestration PR blocca quello Logistics che costruisce e pubblica il proprio, quindi il Bast Front PR blocca entrambi.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Regole da incollare nelle istruzioni dell'agente

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
## Modello di richiesta

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
