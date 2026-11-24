<a id="kenshi-races"></a>
# Kenshi raças

Novas corridas para [MeitouCliente](https://github.com/Meitou-Modding-Ecosystem/MeitouClient) sobre os esqueletos humanos do jogo, jogável no editor de personagens: Leoma e Crevan homem e mulher, e a Crabfolk (crabman, crabqueen), uma biblioteca de animação de 59 clipes para cada raça humana, e o quadro de outros modos adiciona suas raças e subraças. GPL0 ou mais tarde.

- As corridas, seus registros e testá-los no jogo: [docs/kenshi-races.md](docs/kenshi-races.md).
- Usando essas corridas ou adicionando o seu próprio de outro modo: [docs/depending-on-kenshi-races.md](docs/depending-on-kenshi-races.md) (Competência de Agente
`.claude/skills/kenshi-races-dependent`).
- A biblioteca de animação (capa de 59 clips para cada raça nos esqueletos humanos) e como foi feita: [docs/animations.md](docs/animations.md).
- Ficando um rigido `.glb` O corpo (Meshy, Hi3D "3ds Max Biped") no esqueleto do jogo: [docs/race-import.md](docs/race-import.md).

- Construindo e executando localmente com o Visual Studio, VS Code ou o terminal (o motor deste repo ou o lançador instalado, implantado no jogo): [docs/play.md](docs/play.md).
- Instale o repouso na sua máquina: [docs/local-development.md](docs/local-development.md).
- Instrução de agentes sobre uma mudança que espalhe repouso: [docs/agents-across-repos.md](docs/agents-across-repos.md).
- Regras do agente para o pin, versões, CI e lançamentos: [CLAUDE.md](../../CLAUDE.md) (Assim como `AGENTS.md`• Procedimentos passo a passo:
`.claude/skills/meitou-engine-release`.
- Verificar um mod construído contra o mais recente lançamento do Meitou (CI faz isso em cada PR): [docs/refcheck.md](docs/refcheck.md).
- Publicando o modo para o Steam Workshop pela primeira vez, em seguida, em cada edição: [docs/steam-workshop.md](docs/steam-workshop.md).
- Tornação `.glb` Os modelos (Meshy, Hi3D, Blender) em estruturas construíveis, não FCS Necessário para: [docs/structures.md](docs/structures.md);
Obtenha o polimérico, o tamanho e as texturas de um modelo gerado primeiro: [docs/model-prep.md](docs/model-prep.md).

<a id="numbers"></a>
## Números

Tipos de tarefas (`AddGoalType`, `AddJobType`e os códigos de diálogo (`AddDialogueCondition`, `AddDialogueEffect`A partir de então, todos os tipos de dados são compartilhados ([Modo de blocos](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)). Kenshi As raças não reivindicam nada.

| Número | Piscina | Usando |
| --- | --- | --- |

<a id="layout"></a>
## Layout

- `engine/`: MeitouClient Como um submódulo git, apertado para o motor compromete-se com o mod builds contra. `git pull` Não se move: depois de
Todos os pull run `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build` ([Mantenha os clones atuais](docs/local-development.md#keep-clones-current)).
- `KenshiRaces/`O projeto do Mod e `meitou.json`; `KenshiRaces/items/races/`Os organismos adotados (produção de importação adotada).
`KenshiRaces.Tests/`• Testes (dados sintéticos, não é necessário instalar jogos). `<Mod>.Data/` (Opcional): um gerador que escreve `<Mod>.mod` do jogo instalado. `<Mod>/Structures/` (a partir do dia seguinte): `.glb` As estruturas que são construídas em construção ([docs/structures.md](docs/structures.md)).
- `meitou.lock.json`• O framework libera os mods construídos contra
([Figura do framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)( ); o fluxo de trabalho da atualização do quadro o mova para novas publicações e fusões quando o CI passa ([Atualização do Framework](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)).
- `Play/`Um clique para criar, implantar e executar.
- `tools/`: `Update-Local.ps1`Avaliação e verificação do motor ([Mantenha os clones atuais](docs/local-development.md#keep-clones-current));
`refcheck.sh` e `RefCheck/`Avaliação do controle de referência ([docs/refcheck.md](docs/refcheck.md)); `StructureImport/`Apresentação do Modelo ([docs/structures.md](docs/structures.md)); `RaceImport/`O seu corpo está em conformidade ([docs/race-import.md](docs/race-import.md)); `model-info.py` e `prepare-model.py`Avaliação e verificação de texto ([docs/model-prep.md](docs/model-prep.md)).
- `Directory.Build.props` importar o motor, então os mods construem com as configurações do jogo (alarmas são erros).
- `Directory.Build.rsp` A marcação de referências de motores foi marcada como o Meitou mais antigo lançou o suporte de mods, igual a cada um. `gameVersion` Cidade: o
O jogo rejeita um modo que se refere a uma nova marca de motor, e o submodulo construi `1.0.0.0` ([Versão](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md)). O fluxo de trabalho de compatibilidade de Meitou, e de modo a verificar os mods diariamente contra os novos lançamentos de Meitou ([Meito Compatível](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)).

<a id="branches"></a>
## Brancas

PRs em `beta`CI (construção e testes) deve passar. `beta` É promovido para `main` quando testado no jogo, e a produção passa por um `release`. Cada um se une `release` tags em `<slug>-v<version>` e publicando o mod's zip (`.github/workflows/release.yml`a) a não ser que a marca existam; `tools\Tag-Release.ps1` É o manual de queda.

- Proteção de novos ramos de reposição e permanente beta/main/release Revisão em: [docs/branch-policy.md](docs/branch-policy.md). Templates fonte de cópia; GutenStructure aplica configurações através da reconciliação.
