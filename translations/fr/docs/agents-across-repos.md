<a id="agents-working-across-repos"></a>
# Coordonner les changements entre plusieurs dépôts

Donnez à un agent chaque dépôt touché par le changement, dites-lui quel dépôt possède chaque partie et demandez-lui d'attribuer les PR dans l'ordre des dépendances : moteur, puis framework, puis mod. Une session d'agent par changement, pas une par dépôt.

<a id="give-the-agent-the-repos"></a>
## Donnez les dépôts à l'agent

- **Session Cloud :**chaque dépôt doit être attaché à la session ou au projet. L'agent les clone côte à côte et travaille sur sa propre branche dans chacune.
- **Votre PC (télécommande) :**les clones dans les dossiers de [local-development.md](local-development.md). L'agent peut également faire ce que les sessions cloud ne peuvent pas faire, comme `gh repo create`.

<a id="where-each-part-goes"></a>
## Où va chaque partie

| Changement | Repo | PR dans |
| --- | --- | --- |
| Hook moteur (événement, champ enregistré, commande) | MeitouClient | `fork` |
| Rivalité Core API ou comportement | RivalitéCore | `beta` |
| Noyau Logistique (nœuds, bords, propriétaires, stock, transports) | LogistiqueCore | `beta` |
| War Orchestration (scénarios de combat sur la logistique, pas d'histoire ni de personnages) | GuerreOrchestration | `beta` |
| Bast Front mêlée | BastFront | `beta` |
| Le code d'un mod, un générateur de données, des tests, de la documentation | le dépôt de ce mod | `beta` |
| CI partagé, emballage, porte de déverrouillage, cible de verrouillage | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Commande pour un changement qui traverse les pensions

1. Moteur PR en `fork`, fusionné. Les broches du moteur se déplacent uniquement vers les balises de version en amont, de sorte que les PR du framework et des mods qui en ont besoin attendent que l'amont (VlcekM/ MeitouClient) publie le changement. En attendant, construisez localement sur la branche du moteur ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. Framework PR qui effectue le changement (et déplace sa broche `engine` vers la version qui contient ce dont il a besoin), fusionné dans `beta`. CI publie la version bêta.
3. Mod PR qui déplace `meitou.lock.json` vers cette version préliminaire (et `engine` vers la validation du moteur de la serrure), fusionné dans `beta`.
4. Une fois la version framework promue et étiquetée : un mod PR qui déplace le verrou vers `<framework>-v<version>`.

Chaque PR relie celui qui le précède. Un changement radical du framework API supprime la version mineure du framework et répertorie tous les dépôts dépendants ; chaque personne à charge obtient son propre verrou PR.

Dans une chaîne, l'étape 2 se répète une fois par couche, de haut en bas : un changement de noyau logistique arrive, puis une orchestration de guerre PR verrouille cette construction logistique et publie la sienne, puis le Bast Front PR verrouille les deux.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Règles à coller dans les instructions de l'agent

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
## Modèle d'invite

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```

[English](../../../docs/agents-across-repos.md) · [Kenshi Races](../README.md)
