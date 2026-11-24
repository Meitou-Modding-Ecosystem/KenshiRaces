<a id="agents-working-across-repos"></a>
# Gli agenti che lavorano per il riposo

Dai un agente ogni repo il tocco, dice che il repo possiede ogni parte, e ha il PR in ordine di dipendenza: il motore, poi il mod. Una sessione per cambiamento, non una per repo.

<a id="give-the-agent-the-repos"></a>
## Dai all’agente il riposo

- **Sessione Cloud:** Ogni repo deve essere allegato alla sessione o al progetto. L'agente li clone di lato in lato e lavora sulla propria filiale
In ciascuno.
- **Il tuo PC (Remote Control):** I cloni nelle cartelle di [local-development.md](local-development.md). Anche l'agente può fare ciò che
Non è possibile effettuare sessioni cloud, come `gh repo create`.

<a id="where-each-part-goes"></a>
## Dove ogni parte va

| Cambiamento | riposo | PR in arrivo |
| --- | --- | --- |
| Motor hook (evento, campo salvato, comando) | MeitouClient | `fork` |
| Rivalri di Terra API o comportamento | Il rivalore | `beta` |
| Logistica Core (nodi, confini, proprietari, stock, trasporti) | LogisticaCore | `beta` |
| Orchestra di guerra (scenari di combattimento sulla logistica, nessuna storia o personaggi) | WarArchestrazione | `beta` |
| Bast Fronte Melee | BastFront | `beta` |
| Codice di un mod, generatore di dati, test, doc | Il repo di questo mod | `beta` |
| CI condiviso, imballaggio, porta di rilascio, target di blocco | Struttura | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Un cambiamento che attraversa il riposo

1. Il motore PR `fork`E si unisce. I pini del motore si muovono solo verso le etichette di rilascio, quindi il framework e i mod PR che hanno bisogno di aspettare fino a
Il ritorno (VlcekM/MeitouClientRilascia il cambiamento. In questo caso, si può scegliere il motore ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. Framework PR che fa il cambiamento (e muove il suo `engine` Pin alla rilascio che ha ciò di cui ha bisogno), unito in `beta`. Pubblicati
La prima beta.
3. Il PR che si muove `meitou.lock.json` di questo pregiudizio (e `engine` per il motore del lock), mescolato `beta`.
4. Dopo che il rilascio del framework viene promosso e etichettato: un mod PR che sposta il blocco a `<framework>-v<version>`.

Ogni PR collega uno prima di lui. Un quadro rompente API Il cambiamento è la versione minore del quadro e elenca ogni repo dipendente; ogni dipendente riceve il proprio PR.

In una catena, il passo 2 si ripete una volta, in alto: un cambiamento di logistica, poi un'orchestrazione PR chiude che Logistica costruisce e pubblica, poi il Bast Front PR chiude entrambi.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Le regole da applicare nelle istruzioni dell'agente

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
## Il Tempio Pronto

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
