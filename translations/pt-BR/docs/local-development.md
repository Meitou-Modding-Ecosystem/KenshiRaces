<a id="local-development-setup"></a>
# Local de desenvolvimento

Clone apenas o repos que você está trabalhando em uma pasta. Cada um cria em si mesmo com `dotnet build`E os quadros que ele precisa baixar e hash-check-se.

<a id="repos"></a>
## Reposão

Cada mod repo é nomeado por seu mod.

| Repo | Mantenha | PRs vão para |
| --- | --- | --- |
| MeitouClient | O motor e seus mod hooks | `fork` |
| Rivalho | Rivalry Core e suas habilidades | `beta` |
| LogísticaCore | O Quadro de Logística | `beta` |
| WarOrquestração | Sistema de Orquestração de Guerra (Us Logistics Core) | `beta` |
| O BastFront | Bast Front Melee (métodos de Operação de Guerra e Logística) | `beta` |
| Açoando | A mão de chão (usando o Rivalry Core) | `beta` |
| Casamento | Casamento | `beta` |
| Escudo | SquadArena mod e a ferramenta de classificação da equipe | `beta` |
| A Estrutura (TheIvoryDisaster) | CI compartilhado, embalagem, porta de libertação, objetivo de bloqueio de framework | `beta` |

<a id="folders"></a>
## Folhetos

Estes passos usam oito folhetos. Configure-os como variáveis em cada nova janela do PowerShell, em seguida, coloque os comandos abaixo como eles são:

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
## Precondições

- .NET 11 SDK Cada repo `global.json` Nome da versão) e Git.
- Um editor: Visual Studio, VS Código com o C# Dev Kit, ou Rider. Um terminal também funciona.
- Kenshi Instalado em Se não estiver em `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`Então, coloque o `KENSHI_PATH` Ambiente variável
para o seu papel.

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

Cada mod repo tem sua própria `engine` O submodulo, assim `$Engine` É preciso apenas escrever motores.

<a id="build-and-play-one-mod"></a>
## Criar e jogar um mod

**Mudança no repo do mod:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- O primeiro edifício descarga cada quadro `meitou.lock.json` Entrar em `deps` E se a sua SHA256 não é possível.
- `dotnet run --project Play` Copia os quadros fechados e o mod `Kenshi\mods` E começa o motor. Visual Studio vs. Código
e o lançador instalado: [play.md](play.md).
- Permitir os quadros acima do modo no lançador, como para qualquer dependência.

<a id="change-a-framework-and-a-mod-together"></a>
## Mude um quadro e um mod juntos

Punha o mod repo no seu clone de framework:

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- Construindo em `$Ashen` Agora compilando Rivalry Core de `$Rivalry` Use o motor em `$Rivalry\engine`E o jogo desloca o que o Rivalry Core está construindo.
- `Meitou.user.props` É um GIT ignorado. Se ele já manteve sua Kenshi Em seguida, adicione as duas linhas dentro do `PropertyGroup` Em vez disso.
- `MeitouFrameworkSources` O projeto tem parcerias separadas por `;`e o g. `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` Nome do motor em que o clone de quadro é construído.
- Remova a linha para voltar ao lock ZIP.

**cadeias de estrutura.** Bast Frente `meitou.lock.json` A Orquestração da Guerra e a Logística Core. Para alterar ambos os quadros e o Bast Front juntos, superando ambos no Bast Front:

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

A super-rida passa para baixo, então a Orquestração da Guerra se construi contra `$Logistics` Além disso, todo o sistema usa um único motor. A Orquestração de Guerra só se encaixa e se construi contra o Céu de Logística, chamado no bloqueio do Bast Front.

Para enviá-lo: combinar o quadro PR em `beta` de `$Rivalry`que publicou uma pré-referência `rivalry-core-<version>-beta.<commit>`. Em seguida, em `$Ashen`O Set `meitou.lock.json` Para esse patrimônio e para o patrimônio, SHA-256 e o motor se compromete com as notas de lançamento, e abre o modo PR. Em uma cadeia, o navio para baixo: Logística Core, então uma Orquestração construindo fechando, então Bast Front fechando ambos. Após a libertação do quadro, mude o fechamento para `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## Mude o motor e um mod juntos

Verifique sua rama de motores dentro do submodulo do mod repo:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- Não se compromete com o novo motor: os pins de motor se movem apenas para as etiquetas de lançamento, então a mudança chega a este repo uma vez.
libertação de ele.
- Um mod que usa um quadro também precisa do quadro reconstruído nesse motor. Use o quadro acima e verifique o mesmo
A indústria do motor em `$Rivalry\engine`.
- Encontro: Encontro do motor PR `fork`Após a publicação do upstream, o framework PR `$Rivalry\engine` Para a libertação, a
A PR move o seu lock e `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## Transforme o motor para jogar ou testar

Para jogar o mais recente upstream, um lançamento ou um ramo de motor, mude o seu `$Engine` Clone com o script em qualquer mod repo:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`O mais recente compromisso de VlcekM/MeitouClient `master` (O script adiciona o `upstream` em caso de ausência).
- `-To Release`O mais recente upstream release tag `vX.Y.Z`.
- `-To Fork`O org fork é `fork` Fonte, é tirado
- `-Branch <name>`Uma rama do fork org, como uma rama de PR do motor.
- Sem `-Path` Ele usa o `MeitouClient` O mapa fica ao lado do mod repo. `-Run` Construa o jogo em Release e inicia; `-Build` apenas
construindo isso.
- Apresentação, lançamento e `-Branch` Deixar o clone em um comando separado; o local `master` e `fork` As ramas não mudam. O script
Parar sem mudar nada quando o clone tem mudanças inesperadas.
- Nunca toca o mod repo `engine` O submodulo (o que é `Update-Local.ps1` e os pins).

<a id="keep-clones-current"></a>
## Mantenha os clones atuais

Depois de cada pedaço, e sempre que um edifício falha com os tipos de motores que faltam (por exemplo, `The type or namespace name 'RelationCause' could not be found`), feche o Visual Studio e execute o script de atualização no repo do mod:

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- É a mudança para `beta` e retira, depois atualiza `engine` e `build/gutenstructure` Para os compromissos `beta` Pintura (`git pull` Sozinho
Deixamos os velhos para os velhos). `-Branch <name>` Atirando outro ramo.
- Ele verifica cada submodulo contra o seu pin e imprime a versão do motor.
- É uma bandeira `MeitouEngineDir` e `MeitouFrameworkSources` Em `Meitou.user.props` ou o meio ambiente: eles fazem o edifício usar outro
O motor ou o quadro do repo, em vez dos pins deste repo. Elimine-os a menos que você mude um quadro e um mod juntos.
- `-Clean` Eliminar o git-ignorado `bin` e `obj` cartões, para que nenhuma construção do velho motor é reutilizado. `-Build` Corrida `dotnet build`.
- Isso termina com `Up to date` Ou uma lista de linhas marcadas `FIX`.

O próximo edifício descarrega um quadro novamente apenas quando `meitou.lock.json` mudou o
