<a id="build-and-run-locally"></a>
# Construir e executar localmente

Edifício `Play` Construa o motor e cada mod neste repo, copia os modos e os quadros `meitou.lock.json` Nomear a pasta do jogo e, em seguida, iniciar o jogo. Use qualquer ferramenta que você gosta: Visual Studio, VS Code ou um terminal todos executar a mesma construção.

Todos os três precisam.NET 11 SDK e um clone com seus submodulos (`git clone --recurse-submodules <repo URL>`ou `git submodule update --init --recursive` em um clone existente). Sem ele, a construção se detém. `MEITOU001`nomeando esse comando.

<a id="visual-studio"></a>
## Estúdio Visual

1. Abre o repo `.slnx`.
2. Setagem `Play` Assim como o projeto de start-up, se o Visual Studio não o selecionou (click direito > Configurar como o Projeto de Start-up).
3. Selecione um perfil na barra de desligamento ao lado do botão Iniciar e, em seguida, pressione F5 (debug) ou Ctrl+F5:

| O perfil | Runs | Use-o para |
| --- | --- | --- |
| Jogo (Motor + Mods) | `MeitouClient.exe` Construído a partir deste repo `engine` Submodulo | Testar e desbloquear o seu mod; brochuras na tarefa do código |
| Meitou Launcher (instalar) | O lançador instalado do Meitou libera, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | Modificar a lista de mods ou colocar a ordem de carregamento e confiar em um novo código de mod |

<a id="vs-code"></a>
## Contra o Código

1. Instale a extensão C# Dev Kit (Código VS oferece quando você abre a pasta; está em `.vscode/extensions.json`).
2. Abra a pasta de raiz do repo (File > Open Folder).
3. Executar e desligar (Ctrl + Shift + D), selecionar **Jogo (Motor + Mods)**Apresentação F5. Primeiro, ele cria e desloca, e depois começa o motor.
Debugger; pontos de partida no trabalho de código mod.
4. Para o lançador instalado: Terminal > Tarefas de execução > **Meitou Launcher (instalar)**. Ele cria e desloca, e depois abre o lançador.

Terminal > Run Build Task (Ctrl+Shift+B) só cria e desloca.

<a id="without-an-editor"></a>
## Sem um editor

Clique em Direito `tools\Build.ps1` > **Conheça o PowerShell**. Construindo e distribuindo como `dotnet build` (Verificar os submodutos primeiro se eles estão faltando) e manter a janela aberta para que você possa ler o resultado. `-Test` Também realizam os testes; `-Run` Depois começa o motor.

<a id="terminal"></a>
## Terminal

Na pasta de raiz do repo:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` e `dotnet test` Construir e testar todos os projetos.
- `dotnet run --project Play` Construa, desloca e inicia o motor (o perfil "Play (motor + mods)").
- Para usar o lançador instalado, implante sem iniciar o motor, e depois abra o lançador:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## O que um jogo faz

- Encontre os mods: cada postagem de nível superior `meitou.json` e `<Folder>/<Folder>.csproj`. Um novo modelo não precisa de mudança `Play`.
- Construa o motor e cada mod **libertação**Qualquer que seja a configuração que você escolheu: o motor de desgaste está construindo o lançamento em `TaskStart.CheckStarts`
Uma vez que um mod começa a tarefa de um personagem. Os pontos de interrupção no código de modo ∞ ainda funcionam (símbolos de libertação são implantados).
- Copia cada mod `<Mod>.dll`, `<Mod>.pdb` e `meitou.json` Para `<Kenshi>\mods\<Mod>\`. Feche o jogo primeiro: um jogo corrente fecha o dll.
- Copie cada quadro em `meitou.lock.json` A partir do arquivo downloadado para `<Kenshi>\mods\<Framework>\`Ou, por exemplo, para um quadro
A partir de um clone ([local-development.md](local-development.md)a sua produção de construção.
- Um sistema de dados com um sistema de dados (`<Mod>.Data/<Mod>.Data.csproj`Correndo como `<generator> <Kenshi folder> <output .mod>`e obtém a sua `<Mod>.mod`
gerado a partir do jogo instalado; um fracasso é um aviso de construção e o mod é implementado sem ele. Todos os outros modos recebem um vazio `<Mod>.mod` Quando não há, um existente nunca é substituído.
- Um mod com a `Structures` A folha obtém `.glb` modelos importados para a sua pasta, registros misturados para a `.mod`
([structures.md](structures.md)).
- Quando a pasta de mods não existe, nada é copiado e o edifício diz isso.
- O botão do lançador começa com o lançamento que ele instalou, e não com o motor do repo. Feche-o e execute "Play (engine + mods)" para testar o seu
Construção .

<a id="settings"></a>
## A configuração

Desvantagens: Jogo `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Mods em Folha `<game>\mods`. Mude-os com o `KENSHI_PATH` ambiente variável (o motor também lê, então ambos concordam), ou por máquina em um git-ignorado `Meitou.user.props` Na pasta de raiz do repo:

```xml
<Project>
  <PropertyGroup>
    <KenshiDir>D:\Games\Kenshi</KenshiDir>           <!-- game folder; also sets the mods folder -->
    <KenshiModsDir>D:\Games\Kenshi\mods</KenshiModsDir> <!-- mods folder only -->
    <PlayConfiguration>Debug</PlayConfiguration>      <!-- engine and mods configuration (default Release) -->
    <PlayDeploy>false</PlayDeploy>                    <!-- build and run without copying -->
    <PlayArguments>--continue</PlayArguments>         <!-- engine command line (Visual Studio and dotnet run) -->
  </PropertyGroup>
</Project>
```

O motor encontra o jogo por conta própria (Steam); `KenshiDir` Apenas em `Meitou.user.props` Não passaram para isso. A configuração de lançamento do VS Code executa o motor de lançamento; em seguida, `.vscode/launch.json` Localmente, se você mudar `PlayConfiguration`.

<a id="first-run"></a>
## Primeira corrida

- Execute o lançador instalado uma vez (alguma das maneiras acima): habilite o mod e seus quadros, e confie no seu código.
- Se a cópia falhar com o "accesso negado", dê ao seu usuário os direitos de Modificação `Kenshi\mods` (Propriedades > Segurança) em vez de executar a sua
O editor como administrador.
