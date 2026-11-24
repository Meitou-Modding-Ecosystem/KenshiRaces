<a id="agents-working-across-repos"></a>
# Coordinar cambios entre repositorios

Déle a un agente cada repositorio que toque el cambio, dígale qué repositorio posee cada parte y haga que obtenga los PR en orden de dependencia: motor, luego marco, luego mod. Una sesión de agente por cambio, no una por repositorio.

<a id="give-the-agent-the-repos"></a>
## Darle al agente los repositorios

- **Sesión en la nube:**cada repositorio debe estar adjunto a la sesión o proyecto. El agente los clona uno al lado del otro y trabaja en su propia rama en cada uno.
- **Tu PC (Control remoto):**los clones en las carpetas de [local-development.md](local-development.md). El agente también puede hacer lo que las sesiones en la nube no pueden, como `gh repo create`.

<a id="where-each-part-goes"></a>
## donde va cada parte

| Cambiar | repositorio | PR en |
| --- | --- | --- |
| Gancho del motor (evento, campo guardado, comando) | MeitouClient | `fork` |
| Rivalry Core API o comportamiento | RivalidadNúcleo | `beta` |
| Núcleo Logístico (nodos, bordes, propietarios, stock, transportes) | LogísticaCore | `beta` |
| War Orchestration (escenarios de combate sobre logística, sin historia ni personajes) | GuerraOrquestación | `beta` |
| Frente de Bast cuerpo a cuerpo | BastFront | `beta` |
| Código de mod, generador de datos, pruebas, documentos. | repositorio de ese mod | `beta` |
| CI compartido, embalaje, puerta de liberación, objetivo de bloqueo | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## Orden de cambio que cruza repos

1. Motor PR en `fork`, fusionado. Los pines del motor se mueven solo a las etiquetas de versión ascendentes, por lo que el marco y los RP mod que lo necesitan esperan hasta que las etiquetas ascendentes (VlcekM/ MeitouClient) publiquen el cambio. Hasta entonces, cree localmente en la rama del motor ([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. Framework PR que realiza el cambio (y mueve su pin `engine` a la versión que tiene lo que necesita), fusionado en `beta`. CI publica la versión preliminar beta.
3. Mod PR que mueve `meitou.lock.json` a esa versión preliminar (y `engine` al compromiso del motor de bloqueo), fusionado en `beta`.
4. Después de promocionar y etiquetar el lanzamiento del marco: un mod PR que mueve el bloqueo a `<framework>-v<version>`.

Cada PR vincula al anterior. Un cambio importante en el marco API afecta la versión menor del marco y enumera todos los repositorios dependientes; cada dependiente obtiene su propio bloqueo PR.

En una cadena, el paso 2 se repite una vez por capa, de arriba hacia abajo: un cambio de Logistics Core aterriza, luego un War Orchestration PR bloquea esa logística que construye y publica el suyo propio, luego Bast Front PR bloquea ambos.

<a id="rules-to-paste-into-the-agents-instructions"></a>
## Reglas para pegar en las instrucciones del agente.

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
## plantilla de aviso

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```

[English](../../../docs/agents-across-repos.md) · [Kenshi Races](../README.md)
