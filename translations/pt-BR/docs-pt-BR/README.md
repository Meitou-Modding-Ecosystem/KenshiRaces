<a id="kenshi-races"></a>
# Kenshi Races

Novas raças para [ MeitouClient ](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) nos esqueletos humanos do jogo, jogáveis ​​​​no editor de personagens: Leoma e Crevan (masculino e feminino) e o Crabfolk (crabman, crabqueen), uma biblioteca de animação de 59 clipes para cada raça de esqueleto humano e a estrutura com a qual outros mods adicionam suas próprias raças e sub-raças. GPL -3.0 ou posterior.

- As corridas, seus recordes e testes no jogo: [docs/kenshi-races.md](docs/kenshi-races.md) .
- Usando essas corridas ou adicionando a sua própria de outro mod: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (habilidade do agente `.claude/skills/kenshi-races-dependent` ).
- A biblioteca de animação (59 clipes para cada raça nos esqueletos humanos) e como ela foi feita: [docs/animations.md](docs/animations.md) .
- Ajustando um corpo `.glb` manipulado ( Meshy , Hi3D "3ds Max Biped") no esqueleto do jogo: [docs/race-import.md](docs/race-import.md) .

- Construa e execute localmente com Visual Studio , VS Code ou o terminal (o mecanismo deste repositório ou o inicializador instalado, implantado no jogo): [docs/play.md](docs/play.md) .
- Configurando os repositórios em sua máquina: [docs/local-development.md](docs/local-development.md) .
- Instruindo os agentes sobre uma mudança que abrange repositórios: [docs/agents-across-repos.md](docs/agents-across-repos.md) .
- Regras do agente para pin do mecanismo, versões, CI e lançamentos: [ CLAUDE .md](../../CLAUDE.md) (igual a `AGENTS.md` ); procedimentos passo a passo: `.claude/skills/meitou-engine-release` .
- Verificando um mod construído em relação à versão mais recente do Meitou ( CI faz isso em todos os PR ): [docs/refcheck.md](docs/refcheck.md) .
- Publicando o mod no Steam Workshop pela primeira vez e depois em cada lançamento: [docs/steam-workshop.md](docs/steam-workshop.md) .
- Transformando modelos `.glb` ( Meshy , Hi3D , Blender ) em estruturas edificáveis, não é necessário FCS : [docs/structures.md](docs/structures.md) ; obter primeiro o polycount, o tamanho e as texturas de um modelo gerado: [docs/model-prep.md](docs/model-prep.md) .

<a id="numbers"></a>
## Números

Tipos de tarefas ( `AddGoalType` , `AddJobType` ) e códigos de diálogo ( `AddDialogueCondition` , `AddDialogueEffect` ) são compartilhados por cada mod carregado ([blocos de número de mod](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md) ). Kenshi Races não afirma nada.

| Número | Piscina | Usar |
| --- | --- | --- |

<a id="layout"></a>
## Disposição

- `engine/` : MeitouClient como um submódulo git, fixado no commit do mecanismo contra o qual o mod é compilado. `git pull` não o move: após cada pull, execute `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([manter os clones atuais](docs/local-development.md#keep-clones-current) ).
- `KenshiRaces/` : o projeto do mod e `meitou.json` ; `KenshiRaces/items/races/` : os corpos ajustados (saída de importação confirmada). `KenshiRaces.Tests/` : seus testes (dados sintéticos, sem necessidade de instalação de jogo). `<Mod>.Data/` (opcional): um gerador que grava `<Mod>.mod` do jogo instalado. `<Mod>/Structures/` (opcional): modelos `.glb` importados como estruturas edificáveis ​​na implantação ([docs/structures.md](docs/structures.md) ).
- `meitou.lock.json` : o framework libera os mods compilados ([framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ); o fluxo de trabalho de atualização do Framework o move para novas versões e mescla quando CI passa ([atualização do framework](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md) ).
- `Play/` : construção, implantação e execução com um clique.
- `tools/` : `Update-Local.ps1` , a tração local e verificação do pino do motor ([manter os clones atuais](docs/local-development.md#keep-clones-current) ); `refcheck.sh` e `RefCheck/` , a verificação de referência do mecanismo ([docs/refcheck.md](docs/refcheck.md) ); `StructureImport/` , a importação do modelo ([docs/structures.md](docs/structures.md) ); `RaceImport/` , o ajuste do corpo ([docs/race-import.md](docs/race-import.md) ); `model-info.py` e `prepare-model.py` , verificações de modelo e redução de textura ([docs/model-prep.md](docs/model-prep.md) ).
- `Directory.Build.props` importa o motor, então os mods são construídos com as configurações do jogo (avisos são erros).
- `Directory.Build.rsp` carimba as referências do motor como o Meitou mais antigo libera o suporte a mods, igual a cada andar `gameVersion` : o jogo recusa um mod que faz referência a um carimbo de motor mais recente, e o submódulo constrói `1.0.0.0` ([versionamento](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) ). O fluxo de trabalho de compatibilidade do Meitou verifica os mods diariamente em relação aos novos lançamentos do Meitou ([meitou compat](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md) ).

<a id="branches"></a>
## Galhos

PRs em `beta` ; CI (construção e testes) deve passar. `beta` é promovido a `main` depois de testado em jogo, e a produção passa de PR a `release` . Cada mesclagem em `release` marca `<slug>-v<version>` e publica o zip do mod ( `.github/workflows/release.yml` ), a menos que essa tag exista; `tools\Tag-Release.ps1` é o substituto manual.

- Proteções de ramificação de novos repositórios e revisões permanentes beta/principal/lançamento: [docs/branch-policy.md](docs/branch-policy.md) . Fonte de cópia dos modelos; GutenStructure aplica configurações por meio de reconciliação.
