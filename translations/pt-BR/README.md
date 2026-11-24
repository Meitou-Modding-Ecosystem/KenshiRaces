# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · [Deutsch](../de/README.md) · [Español](../es/README.md) · [Italiano](../it/README.md) · Português do Brasil · [Русский](../ru/README.md) · [日本語](../ja/README.md) · [한국어](../ko/README.md) · [简体中文](../zh-CN/README.md)

Kenshi Races adiciona três raças jogáveis ao editor de personagens do [MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient). Leoma e Crevan têm corpos masculinos e femininos. Os Crabfolk têm duas variantes, `crabman` e `crabqueen`. Todas usam os esqueletos humanos do jogo. O mod também oferece 59 animações para as raças que usam esses esqueletos e um framework para que outros mods adicionem suas próprias raças e sub-raças. GPL-3.0-or-later.

## Guias

- [Raças, registros e testes no jogo](docs/kenshi-races.md)
- [Usar o framework em outro mod e adicionar sub-raças](docs/depending-on-kenshi-races.md)
- [Biblioteca de 59 animações e processo de criação](docs/animations.md)
- [Adaptar um corpo GLB ao esqueleto do jogo](docs/race-import.md)
- [Compilar e jogar usando Visual Studio, VS Code ou o terminal](docs/play.md)
- [Configurar e atualizar os repositórios locais](docs/local-development.md)
- [Coordenar alterações entre repositórios](docs/agents-across-repos.md)
- [Verificar a compatibilidade com novas versões do Meitou](docs/refcheck.md)
- [Publicar na Oficina Steam](docs/steam-workshop.md)
- [Importar modelos GLB como construções sem usar o FCS](docs/structures.md)
- [Preparar a geometria, as dimensões e as texturas](docs/model-prep.md)
- [Proteção de branches e regras de revisão](docs/branch-policy.md)
- [Regras do repositório e procedimentos para agentes](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## Identificadores compartilhados

Todos os mods carregados compartilham a numeração dos tipos de tarefa e dos códigos de diálogo. O Kenshi Races não reserva nenhum número. (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## Estrutura do repositório

- `engine/` — Submódulo do MeitouClient fixado no commit usado para compilar o mod. Depois de cada pull, atualize-o com o comando abaixo. [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — Projeto e manifesto do mod. Os corpos adaptados estão versionados no repositório. Os testes usam dados sintéticos e dispensam a instalação do jogo.
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — Os projetos opcionais geram os dados do mod a partir do jogo instalado ou importam modelos GLB como construções durante a implantação. [StructureImport](docs/structures.md)
- `meitou.lock.json` — Fixa as versões dos frameworks. O fluxo de atualização adota novas versões e mescla a alteração quando a CI passa. [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — Compila, implanta e executa com um clique.
- `tools/` — Ferramentas de sincronização, verificação de referências do motor, importação de construções, adaptação de corpos e preparação de texturas. [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — Importa as configurações do motor. Avisos de compilação são tratados como erros.
- `Directory.Build.rsp` — Fixa as referências na versão mais antiga do motor compatível com o mod. O jogo recusa mods que referenciam um motor mais recente. O submódulo usa uma versão padrão de assembly. (`gameVersion`, `1.0.0.0`).  A compatibilidade com as novas versões do Meitou é verificada diariamente. [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## Branches

Envie as PRs para beta. A compilação e os testes precisam passar. Após testar no jogo, beta pode ser promovida para main. A publicação em produção passa por uma PR para release. Cada mesclagem em release cria a tag de versão e publica o ZIP se a tag ainda não existir. O script de criação de tags é a alternativa manual. (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
