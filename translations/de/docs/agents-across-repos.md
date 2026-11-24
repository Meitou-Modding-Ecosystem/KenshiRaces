<a id="agents-working-across-repos"></a>
# Änderungen über mehrere Repositories koordinieren

Geben Sie einem Agenten jedes Repo, das die Änderung berührt, teilen Sie ihm mit, welches Repo die einzelnen Teile besitzt, und lassen Sie es PRs in der Abhängigkeitsreihenfolge landen: Engine, dann Framework, dann Mod. Eine Agentensitzung pro Änderung, nicht eine pro Repo.

<a id="give-the-agent-the-repos"></a>
## Geben Sie dem Agenten die Repos

- **Cloud-Sitzung:**Jedes Repo muss an die Sitzung oder das Projekt angehängt werden. Der Agent klont sie nebeneinander und arbeitet in jedem an einem eigenen Zweig.
- **Ihr PC (Fernsteuerung):**die Klone in den Ordnern von [local-development.md](local-development.md). Der Agent kann auch das tun, was Cloud-Sitzungen nicht können, z. B. `gh repo create`.

<a id="where-each-part-goes"></a>
## Wohin jeder Teil geht

| Ändern | Repo | PR in |
| --- | --- | --- |
| Engine-Hook (Ereignis, gespeichertes Feld, Befehl) | MeitouClient | `fork` |
| Rivalität Kern API oder Verhalten | RivalryCore | `beta` |
| Logistikkern (Knoten, Kanten, Eigentümer, Lager, Transporte) | LogisticsCore | `beta` |
| Kriegsorchestrierung (Kampfszenarien zur Logistik, keine Geschichte oder Charaktere) | Kriegsorchestrierung | `beta` |
| Bast-Front-Nahkampf | BastFront | `beta` |
| Der Code eines Mods, Datengenerator, Tests, Dokumente | Das Repo dieses Mods | `beta` |
| Gemeinsam genutztes CI, Verpackung, Freigabetor, Sperrziel | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Bestellen Sie eine Repo-übergreifende Änderung

1. Motor PR in `fork`, zusammengeführt. Engine-Pins verschieben sich nur zu Upstream-Release-Tags, daher warten die Framework- und Mod-PRs, die sie benötigen, bis der Upstream (VlcekM/ MeitouClient) die Änderung veröffentlicht. Bis dahin bauen Sie lokal auf dem Engine-Zweig auf ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. Framework PR, das die Änderung vornimmt (und seinen `engine` -Pin auf die Version verschiebt, die alles hat, was es benötigt), wurde in `beta` zusammengeführt. CI veröffentlicht die Beta-Vorabversion.
3. Mod PR, der `meitou.lock.json` zu dieser Vorabversion verschiebt (und `engine` zum Engine-Commit der Sperre), zusammengeführt mit `beta`.
4. Nachdem die Framework-Veröffentlichung beworben und markiert wurde: ein Mod PR, der die Sperre auf `<framework>-v<version>` verschiebt.

Jeder PR verknüpft den davor. Eine bahnbrechende API -Änderung des Frameworks beeinträchtigt die Nebenversion des Frameworks und listet alle abhängigen Repos auf; Jeder abhängige Benutzer erhält seine eigene Sperre PR.

In einer Kette wird Schritt 2 einmal pro Ebene von oben nach unten wiederholt: Eine Änderung des Logistikkerns landet, dann sperrt ein War Orchestration PR diesen Logistik-Build und veröffentlicht seinen eigenen, dann sperrt der Bast Front PR beide.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Regeln zum Einfügen in die Anweisungen des Agenten

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
## Prompt-Vorlage

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```

[English](../../../docs/agents-across-repos.md) · [Kenshi Races](../README.md)
