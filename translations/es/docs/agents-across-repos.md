<a id="agents-working-across-repos"></a>
# Los agentes que trabajan a través del reposo

Dale un agente a cada repo, diga qué repo pertenece a cada parte, y tenga el PR en el orden: el motor, luego el marco, luego el módulo. Una sesión por cambio, no una por repo.

<a id="give-the-agent-the-repos"></a>
## Deja al agente el descanso

- **Sesión en la nube:** Cada repo debe ser anexado a la sesión o el proyecto. El agente los clona lado a lado y trabaja en su propia rama
en cada uno.
- **El control remoto (Remote Control):** Los clones en las cartas de [local-development.md](local-development.md). El agente también puede hacer
Las sesiones en la nube no pueden ser, como `gh repo create`.

<a id="where-each-part-goes"></a>
## Dónde cada parte va

| Cambios | Repo | PR en el |
| --- | --- | --- |
| Motor hook (evento, campo salvado, mando) | MeitouClient | `fork` |
| Rivalry Cuervo API o el comportamiento | El rival | `beta` |
| El núcleo logístico (nodos, bordes, propietarios, stock, transportes) | LogísticaCore | `beta` |
| Orquestación de la guerra (escenarios de lucha sobre la logística, sin historias o personajes) | WarOrchestración | `beta` |
| Bast Frente Melee | BastFront | `beta` |
| Código de un mod, generador de datos, pruebas, doces | El repo de este mod | `beta` |
| CI compartido, embalaje, puerta de liberación, objetivo de cierre | Gutenestructuras | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Un cambio que cruza el descanso

1. Entonces el PR entra `fork`y se mezclaron. Los pinos de motor solo se mueven a las etiquetas de lanzamiento, por lo que los PR de marco y modos que necesitan esperar hasta
El Corazón (VlcekM/MeitouClientliberar el cambio. Hasta entonces, construye contra la rama del motor localmente ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. PR marco que hace el cambio (y mueve su `engine` pin a la liberación que tiene lo que necesita), unido en `beta`. Es publicado
La premisa de la beta.
3. PR que se mueve `meitou.lock.json` En el caso de que se `engine` El motor de la locación se une a `beta`.
4. Después de que se promueve y se etiquete el lanzamiento del marco: un mod PR que mueve el bloqueo a `<framework>-v<version>`.

Todos los PR se conectan con el primero. Un marco rompiendo API El cambio es la versión más pequeña del marco y lista cada repo dependiente; cada dependiente recibe su propio bloqueo.

En una cadena, el paso 2 repite una vez por capa, arriba y abajo: un cambio de logística, luego una Orquesta de Relaciones Exteriores bloquea que Logística construye y publica su propia, luego el Bast Front PR bloquea ambos.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Reglas para adherirse a las instrucciones del agente

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
## Pronto templado

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
