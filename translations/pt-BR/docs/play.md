<a id="build-and-run-locally"></a>
# Compilar e executar o jogo localmente

Ao compilar `Play`, o projeto compila o motor e todos os mods do repositório, copia os mods e os frameworks listados em `meitou.lock.json` para a pasta de mods do jogo e inicia o jogo. Visual Studio, VS Code e o terminal executam a mesma compilação.

Instale o SDK do.NET 11 e clone o repositório com `git clone --recurse-submodules <repo URL>`. Em um clone existente, execute `git submodule update --init --recursive`. Se os submódulos estiverem ausentes, a compilação para com o erro `MEITOU001` e indica o comando necessário.

<a id="visual-studio"></a>
## Visual Studio

1. Abra o `.slnx` do repositório.
2. Defina `Play` como o projeto de inicialização se Visual Studio não o escolheu (clique com o botão direito > Definir como projeto de inicialização).
3. Escolha um perfil no menu suspenso ao lado do botão Iniciar e pressione F5 (depuração) ou Ctrl+F5:

| Perfil | Corre | Use-o para |
| --- | --- | --- |
| `Play (engine + mods)` | `MeitouClient.exe` criado a partir do submódulo `engine` deste repositório | testando e depurando seu mod; pontos de interrupção no código mod funcionam |
| `Meitou launcher (installed)` | o iniciador instalado a partir das versões Meitou, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | alterando a lista de mods ou ordem de carregamento e confiando no código de um novo mod |

<a id="vs-code"></a>
## VS Code

1. Instale a extensão C# Dev Kit (VS Code oferece quando você abre a pasta; está em `.vscode/extensions.json`).
2. Abra a pasta raiz do repositório (Arquivo > Abrir Pasta).
3. Execute e depure (Ctrl+Shift+D), escolha **Play (engine + mods)**, pressione F5. Ele compila e implanta primeiro e, em seguida, inicia o mecanismo no depurador; pontos de interrupção no código mod funcionam.
4. Para o inicializador instalado: Terminal > Executar tarefa > **Meitou launcher (installed)**. Ele cria e implanta e, em seguida, abre o inicializador.

Terminal > Executar tarefa de compilação (Ctrl+Shift+B) apenas compila e implanta.

<a id="without-an-editor"></a>
## Sem editor

Clique com o botão direito em `tools\Build.ps1` > **Executar com PowerShell**. Ele compila e implanta como `dotnet build` (verificando primeiro os submódulos se estiverem faltando) e mantém a janela aberta para que você possa ler o resultado. `-Test` também executa os testes; `-Run` então dá partida no motor.

<a id="terminal"></a>
## terminal

Na pasta raiz do repositório:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` e `dotnet test` constroem e testam cada projeto.
- `dotnet run --project Play` constrói, implanta e inicia o mecanismo (o perfil "Play (motor + mods)").
- Para usar o inicializador instalado, implante sem iniciar o mecanismo e abra o inicializador:

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## O que uma construção de jogo faz

- Encontra os mods: todas as pastas de nível superior contendo `meitou.json` e `<Folder>/<Folder>.csproj`. Um novo mod não precisa de alterações no `Play`.
- Constrói o mecanismo e cada mod no **Release**, qualquer que seja a configuração escolhida: As compilações do mecanismo de depuração são lançadas em `TaskStart.CheckStarts` assim que um mod inicia a tarefa de um personagem. Os pontos de interrupção no código mod ainda funcionam (os símbolos de liberação são implantados).
- Copia `<Mod>.dll`, `<Mod>.pdb` e `meitou.json` de cada mod para `<Kenshi>\mods\<Mod>\`. Feche o jogo primeiro: um jogo em execução bloqueia a dll.
- Copia cada estrutura em `meitou.lock.json` de sua pasta de lançamento baixada para `<Kenshi>\mods\<Framework>\` ou, para uma estrutura construída a partir de um clone ([local-development.md](local-development.md)), sua saída de construção.
- Um mod com gerador de dados (`<Mod>.Data/<Mod>.Data.csproj`, executado como `<generator> <Kenshi folder> <output .mod>`) obtém seu `<Mod>.mod` gerado a partir do jogo instalado; uma falha é um aviso de compilação e o mod é implantado sem ele. Todos os outros mods obtêm um `<Mod>.mod` vazio quando não possuem nenhum; an existing one is never replaced.
- Um mod com uma pasta `Structures` obtém seus modelos `.glb` importados para sua pasta implantada, registros mesclados em seu `.mod` ([structures.md](structures.md)).
- Quando a pasta mods não existe, nada é copiado e a compilação diz isso.
- O botão Play do inicializador inicia a versão instalada, não o mecanismo deste repositório. Feche-o e execute "Play (engine + mods)" para testar sua compilação.

<a id="settings"></a>
## Configurações

Padrões: jogo `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`, pasta mods `<game>\mods`. Altere-os com a variável de ambiente `KENSHI_PATH` (o mecanismo também a lê, então ambos concordam) ou por máquina em um `Meitou.user.props` ignorado pelo git na pasta raiz do repositório:

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

O motor encontra o jogo sozinho (Steam); `KenshiDir` definido apenas em `Meitou.user.props` não é passado para ele.  A configuração de inicialização do VS Code executa o mecanismo Release; edite `.vscode/launch.json` localmente se você alterar `PlayConfiguration`.

<a id="first-run"></a>
## Primeira corrida

- Execute o inicializador instalado uma vez (qualquer uma das formas acima): habilite o mod e suas estruturas e confie em seu código.
- Se a cópia falhar com "acesso negado", conceda ao seu usuário direitos de modificação em `Kenshi\mods` (Propriedades > Segurança) em vez de executar seu editor como administrador.

[English](../../../docs/play.md) · [Kenshi Races](../README.md)
