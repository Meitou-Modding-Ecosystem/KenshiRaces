<a id="local-development-setup"></a>
# Configuração de desenvolvimento local

Clone apenas os repositórios em que você trabalha em uma pasta. Cada um constrói por conta própria com `dotnet build` , e as estruturas necessárias são baixadas e verificadas por hash.

<a id="repos"></a>
## Repositórios

Cada repositório de mod recebe o nome de seu mod.

| Repositório | Detém | PRs vão para |
| --- | --- | --- |
| MeitouClient | o motor e seus mod hooks | `fork` |
| RivalidadeCore | Estrutura Rivalry Core e sua habilidade de agente | `beta` |
| Núcleo de Logística | Estrutura central de logística | `beta` |
| Orquestração de Guerra | Estrutura de orquestração de guerra (usa Logistics Core) | `beta` |
| BastFront | Corpo a corpo Bast Front (usa Orquestração de Guerra e Núcleo de Logística) | `beta` |
| Mão Cinzenta | A Mão Cinzenta (usa Rivalry Core) | `beta` |
| Criação | Criação | `beta` |
| Arena de Esquadrão | Mod SquadArena e ferramenta de classificação de esquadrão | `beta` |
| GutenStructure (O Desastre do Marfim) | CI compartilhado, embalagem, portão de liberação, alvo de bloqueio de estrutura | `beta` |

<a id="folders"></a>
## Pastas

Estas etapas usam oito pastas. Defina-os como variáveis ​​em cada nova janela do PowerShell e cole os comandos abaixo como estão:

```powershell
$Repos     = "C:\Users\<you>\source\repos"
$Engine    = "$Repos\MeitouClient"
$Rivalry   = "$Repos\RivalryCore"
$Ashen     = "$Repos\AshenHand"
$Husbandry = "$Repos\Husbandry"
$Logistics = "$Repos\LogisticsCore"
$War       = "$Repos\WarOrchestration"
$Bast      = "$Repos\BastFront"
```

<a id="prerequisites"></a>
## Pré-requisitos

- . NET 11 SDK (o `global.json` de cada repositório nomeia a versão) e Git.
- Um editor: Visual Studio , VS Code com o C# Dev Kit ou Rider. Um terminal sozinho também funciona.
- Kenshi instalado. Se não estiver em `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` , defina a variável de ambiente `KENSHI_PATH` para sua pasta.

<a id="clone"></a>
## Clone

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

Cada mod repo tem seu próprio submódulo `engine` , então `$Engine` é necessário apenas para escrever ganchos de mecanismo.

<a id="build-and-play-one-mod"></a>
## Crie e jogue um mod

**Mude para o repositório do mod:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- A primeira compilação baixa cada estrutura nomeada em `meitou.lock.json` para `deps` e para se seu SHA -256 não corresponder.
- `dotnet run --project Play` copia as estruturas bloqueadas e o mod para `Kenshi\mods` e inicia o mecanismo. Visual Studio , VS Code e o inicializador instalado: [play.md](play.md) .
- Habilite os frameworks acima do mod no launcher, como para qualquer dependência.

<a id="change-a-framework-and-a-mod-together"></a>
## Altere uma estrutura e um mod juntos

Aponte o mod repo para o clone do seu framework:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- As compilações no `$Ashen` agora compilam o Rivalry Core do `$Rivalry` e usam o mecanismo do `$Rivalry\engine` , e o Play implanta essa compilação do Rivalry Core.
- `Meitou.user.props` é ignorado pelo git. Se ele já contém as configurações do Kenshi , adicione as duas linhas dentro do `PropertyGroup` .
- `MeitouFrameworkSources` segue caminhos de projeto de estrutura separados por `;` , por exemplo. `$Logistics\LogisticsCore\LogisticsCore.csproj` . `MeitouEngineDir` nomeia o mecanismo no qual o clone da estrutura se baseia.
- Exclua a linha para voltar ao ZIP bloqueado.

**Cadeias de estrutura.** O `meitou.lock.json` da Bast Front lista a Orquestração de Guerra e o Núcleo de Logística. Para alterar as estruturas e o Bast Front juntos, substitua ambos no Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

A substituição é transmitida, então o War Orchestration também é baseado no `$Logistics` , e toda a construção usa um mecanismo. Substitua apenas a Orquestração de Guerra e ela se baseia no Núcleo Logístico nomeado no bloqueio de Bast Front.

Para enviá-lo: mescle a estrutura PR com `beta` de `$Rivalry` , que publica um pré-lançamento `rivalry-core-<version>-beta.<commit>` . Então, em `$Ashen` , defina `meitou.lock.json` para essa tag e o ativo SHA -256 e commit do mecanismo de suas notas de lançamento e abra o mod PR . Em uma cadeia, envie de cima para baixo: Núcleo de Logística, depois uma construção de Orquestração de Guerra bloqueando-o e, em seguida, Bast Front bloqueando ambos. Após o lançamento de produção da estrutura, mova o bloqueio para `rivalry-core-v<version>` .

<a id="change-the-engine-and-a-mod-together"></a>
## Mude o motor e um mod juntos

Confira o branch do seu mecanismo dentro do submódulo do mod repo:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Não confirme o novo ponteiro do mecanismo: os pinos do mecanismo se movem apenas para as tags de lançamento do upstream, portanto, a alteração chega a esse repositório assim que o upstream a libera.
- Um mod que usa uma estrutura também precisa da estrutura reconstruída nesse mecanismo. Use a substituição da estrutura acima e verifique a mesma ramificação do mecanismo em `$Rivalry\engine` .
- Ordem para pousar: motor PR em `fork` ; após o lançamento do upstream, a estrutura PR move `$Rivalry\engine` para essa versão, então o mod PR move seu bloqueio e `engine` .

<a id="switch-the-engine-to-play-or-test"></a>
## Mude o mecanismo para jogar ou testar

Para reproduzir o upstream mais recente, uma versão ou uma ramificação do mecanismo, troque seu clone `$Engine` pelo script em qualquer repositório de mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : o commit mais recente de VlcekM/ MeitouClient  `master` (o script adiciona o controle remoto `upstream` quando ausente).
- `-To Release` : a mais nova tag de lançamento upstream `vX.Y.Z` .
- `-To Fork` : a ramificação `fork` da bifurcação organizacional, extraída.
- `-Branch <name>` : uma ramificação da bifurcação organizacional, como uma ramificação do mecanismo PR .
- Sem `-Path` ele usa a pasta `MeitouClient` próxima ao mod repo. `-Run` constrói o jogo no Release e o inicia; `-Build` apenas constrói.
- Upstream, Release e `-Branch` deixam o clone em um commit desanexado; as ramificações locais `master` e `fork` não são alteradas. O script para sem alterar nada quando o clone tem alterações não confirmadas.
- Ele nunca toca no próprio submódulo `engine` do mod repo (que é `Update-Local.ps1` e os pinos).

<a id="keep-clones-current"></a>
## Mantenha os clones atualizados

Após cada pull, e sempre que uma compilação falhar com tipos de mecanismo ausentes (por exemplo, `The type or namespace name 'RelationCause' could not be found` ), feche Visual Studio e execute o script de atualização no repositório do mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- Ele muda para `beta` e o puxa, então atualiza `engine` e `build/gutenstructure` para os pinos `beta` de commit (somente `git pull` os deixa nos antigos). `-Branch <name>` puxa outro branch.
- Ele verifica cada submódulo em relação ao seu pino e imprime a versão do mecanismo.
- Ele sinaliza `MeitouEngineDir` e `MeitouFrameworkSources` em `Meitou.user.props` ou no ambiente: eles fazem a compilação usar o mecanismo ou estrutura de outro repositório em vez dos pinos deste repositório. Exclua-os, a menos que você esteja alterando uma estrutura e um mod juntos.
- `-Clean` exclui as pastas `bin` e `obj` ignoradas pelo git, portanto, nenhuma compilação do mecanismo antigo é reutilizada. `-Build` executa `dotnet build` .
- Termina com `Up to date` ou uma lista de linhas marcadas como `FIX` .

A próxima compilação baixa uma estrutura novamente somente quando `meitou.lock.json` é alterado.
