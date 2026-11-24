<a id="agents-working-across-repos"></a>
# Les agents travaillant à travers le repos

Donnez un agent à chaque touche, dites-leur quel est le parti, et faites-leur dans l’ordre de dépendance : le moteur, puis le cadre, puis le mod. Une séance par changement, pas une par rép.

<a id="give-the-agent-the-repos"></a>
## Donnez à l’agent le repos

- **Séance Cloud :** Chaque repo doit être annexé à la session ou au projet. L'agent les clone de côté en côté et travaille sur sa propre branche
dans chacun de nous.
- **Votre PC (Remote Control) :** Les clones dans les dossiers [local-development.md](local-development.md). L’agent peut aussi faire
Les séances en nuage ne peuvent pas, comme `gh repo create`.

<a id="where-each-part-goes"></a>
## Où chaque partie va

| Changement | Repo | Pr dans |
| --- | --- | --- |
| Moteur hook (événement, champ sauvé, commande) | MeitouClient | `fork` |
| Rivalry au cœur API ou comportement | RivalryCore | `beta` |
| Logistique de base (nœuds, bords, propriétaires, stock, transports) | Logistique | `beta` |
| War Orchestra (scenarios de combat sur la logistique, pas d'histoire ou de personnages) | Guerre de l'Orchestre | `beta` |
| Bast Front Melee | BastFront | `beta` |
| Un code mod, générateur de données, tests, docs | Le repo de ce mod | `beta` |
| CI partagé, emballage, porte de libération, cible de lock | Générale | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Un changement qui traverse le repos

1. Prendre en charge le PR `fork`Et se fusionne. Les pins de moteur ne se déplacent que vers les tags de sortie, de sorte que le cadre et les modes PR qui ont besoin de l'attente jusqu'à ce que
Le Résumé (VlcekM/MeitouClientIl libère le changement. Pour ce faire, il est possible de mettre en place un moteur local ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. Le cadre PR qui fait le changement (et le mouvement de `engine` pin à la libération qui a ce qu'elle a besoin), fusionné dans `beta`. Il publie
La préface beta.
3. Le PR qui se déplace `meitou.lock.json` à ce préalable (et à `engine` à l'engagement du moteur de la lock), fusionné dans `beta`.
4. Après que le framework est promu et étiqueté: un mod PR qui déplace le lock à `<framework>-v<version>`.

Chaque PR relie l’un avant. Un cadre brisant API Le changement bouleversera la version plus petite du cadre et la liste de chaque repo ; chaque dépendant reçoit son propre PR.

Dans une chaîne, le pas 2 se répète une fois par couche, au-dessus: un logistique de changement, puis un War Orchestra PR bloque que Logistics construit et publie son propre, puis le Bast Front PR bloque les deux.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Règles à appliquer dans les instructions de l'agent

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
## Le Temple Promet

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
