<a id="agents-working-across-repos"></a>
# Mitarbeiter, die durch den Wiederaufenthalt

Geben Sie jeden Repo die Anschläge, sagen Sie, welche Repo hat, und haben Sie PRs in Abhängigkeit: Motor, dann Rahmen, dann Mod. Eine Agent-Sitzung pro Wechsel, nicht eine pro Repo.

<a id="give-the-agent-the-repos"></a>
## Geben Sie dem Agent den Rest

- **Die Cloudsitzung:** Jede Repo muss an die Sitzung oder das Projekt hinzugefügt werden. Der Agent kloniert sie Seite für Seite und arbeitet auf seiner eigenen Branche
In jedem von ihnen.
- **Ihr PC (Remote Control)** Die Klonen in den Folien von [local-development.md](local-development.md). Der Agent kann auch tun, was
Cloud-Sitzungen können nicht, wie `gh repo create`.

<a id="where-each-part-goes"></a>
## Wo jeder Teil geht

| Veränderung | Repo | PR in den |
| --- | --- | --- |
| Engine hook (Event, gesparte Felder, Befehl) | MeitouClient | `fork` |
| Rivalry Kern API oder Verhalten | RivalryCore | `beta` |
| Logistik Kern (Note, Rände, Eigentümer, Lager, Transport) | Logistikkore | `beta` |
| War Orchestra (Kämpfen Szenarien auf Logistik, keine Geschichte oder Charaktere) | KriegOrchestration | `beta` |
| Bast Front Melee | BastFront | `beta` |
| Ein Mod-Code, Datengenerator, Tests, Docs | Die Mods der Repo | `beta` |
| Teilen Sie CI, Verpackung, Release-Gate, Lock-Ziel | Gutenstruktur | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Eine Veränderung, die den Rest überquert

1. Die PR-Bewegung ist `fork`und Mischung. Motor Pins bewegen sich nur auf Upstream-Layout-Tags, so dass die Rahmen- und Mod-PRs, die es brauchen, warten
Aufsteigerung (VlcekM/MeitouClient• Erlöse die Veränderung. Bis dahin, auf dem Motorrück lokalisiert ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. Framework PR, das die Veränderung verursacht (und die `engine` Pin zu der Freigabe, die das hat, was sie braucht), vereint in `beta`. Ich veröffentlichte
Das ist die Beta-Premiere.
3. Mod PR, der sich bewegt `meitou.lock.json` und zu diesem Zweck (und `engine` zu dem Lock-Motor Kompromiss), vereint in `beta`.
4. Nach der Framework-Ausgabe wird gefördert und gekennzeichnet: ein Mod PR, der den Lock auf `<framework>-v<version>`.

Jede PR verbindet die eine vor ihm. Ein brechendes Rahmen API Das ändert die kleinere Version des Rahmens und liest jede abhängige Repo; jeder Abhängige bekommt seine eigene Lücke.

In einer Kette wiederholt sich der Schritt 2 einmal pro Schicht: Ein Logistik-Kern verändert sich, dann vergrößert sich ein Kriegs-Prozess, das Logistik erstellt und veröffentlicht wird, dann wird der Bast-Front-Prozesse beide vergrößen.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Regeln, die in die Anweisungen des Agentes festgelegt werden

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
## Schnelle Template

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
