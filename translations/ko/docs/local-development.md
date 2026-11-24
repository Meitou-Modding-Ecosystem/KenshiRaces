<a id="local-development-setup"></a>
# 로컬 개발 설정

작업 중인 저장소만 하나의 폴더에 복제하세요. 각각은 `dotnet build` 를 사용하여 자체적으로 구축되며 필요한 프레임워크를 다운로드하고 해시 확인합니다.

<a id="repos"></a>
## 레포

각 모드 저장소의 이름은 해당 모드의 이름을 따서 지정됩니다.

| 레포 | 보류 | PR은 다음으로 이동합니다. |
| --- | --- | --- |
| MeitouClient | 엔진과 모드 후크 | `fork` |
| 라이벌리코어 | Rivalry Core 프레임워크 및 에이전트 기술 | `beta` |
| 물류핵심 | 물류 핵심 프레임워크 | `beta` |
| 전쟁오케스트레이션 | 전쟁 조정 프레임워크(물류 코어 사용) | `beta` |
| 바스트프론트 | Bast Front 근접 공격(전쟁 조율 및 물류 코어 사용) | `beta` |
| 잿빛손 | Ashen Hand (경쟁 코어 사용) | `beta` |
| 농업 | 농업 | `beta` |
| 스쿼드아레나 | SquadArena 모드 및 분대 평가 도구 | `beta` |
| GutenStructure (TheIvoryDisaster) | 공유 CI , 패키징, 릴리스 게이트, 프레임워크 잠금 대상 | `beta` |

<a id="folders"></a>
## 폴더

이 단계에서는 8개의 폴더를 사용합니다. 모든 새 PowerShell 창에서 이를 변수로 설정한 후 아래 명령을 그대로 붙여넣습니다.

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
## 전제조건

- . NET 11 SDK (각 저장소의 `global.json` 는 버전 이름을 지정함) 및 Git.
- 편집기: Visual Studio , C# Dev Kit가 있는 VS Code 또는 Rider. 터미널만으로도 작동합니다.
- Kenshi 가 설치되었습니다. `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` 에 없으면 `KENSHI_PATH` 환경 변수를 해당 폴더로 설정합니다.

<a id="clone"></a>
## 클론

```powershell
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/RivalryCore.git $Rivalry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/AshenHand.git $Ashen
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/Husbandry.git $Husbandry
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/LogisticsCore.git $Logistics
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/WarOrchestration.git $War
git clone --recurse-submodules https://github.com/Meitou-Modding-Ecosystem/BastFront.git $Bast
git clone https://github.com/Meitou-Modding-Ecosystem/MeitouClient.git $Engine
```

모든 모드 저장소에는 자체 `engine` 하위 모듈이 있으므로 `$Engine` 는 엔진 후크를 작성하는 데만 필요합니다.

<a id="build-and-play-one-mod"></a>
## 하나의 모드를 구축하고 플레이하세요

**모드 저장소로 변경:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- 첫 번째 빌드는 `meitou.lock.json` 에 명명된 각 프레임워크를 `deps` 로 다운로드하고 해당 SHA -256이 일치하지 않으면 중지됩니다.
- `dotnet run --project Play` 는 잠긴 프레임워크와 모드를 `Kenshi\mods` 에 복사하고 엔진을 시작합니다. Visual Studio , VS Code 및 설치된 런처: [play.md](play.md) .
- 모든 종속성과 마찬가지로 실행기에서 모드 위의 프레임워크를 활성화합니다.

<a id="change-a-framework-and-a-mod-together"></a>
## 프레임워크와 모드를 함께 변경

프레임워크 복제본에서 mod 저장소를 가리킵니다.

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- `$Ashen` 의 빌드는 이제 `$Rivalry` 의 Rivalry Core를 컴파일하고 `$Rivalry\engine` 의 엔진을 사용하며 Play는 해당 Rivalry Core 빌드를 배포합니다.
- `Meitou.user.props` 는 자식을 무시합니다. 이미 Kenshi 설정을 보유하고 있는 경우 대신 `PropertyGroup` 내부에 두 줄을 추가하십시오.
- `MeitouFrameworkSources` 는 `;` 로 구분된 프레임워크 프로젝트 경로를 사용합니다. `$Logistics\LogisticsCore\LogisticsCore.csproj` . `MeitouEngineDir` 는 프레임워크 클론이 빌드되는 엔진의 이름을 지정합니다.
- 잠긴 ZIP 로 돌아가려면 해당 줄을 삭제하세요.

**프레임워크 체인.** Bast Front의 `meitou.lock.json` 에는 전쟁 조율 및 물류 코어가 나열되어 있습니다. 프레임워크와 Bast Front를 함께 변경하려면 Bast Front에서 둘 다 재정의합니다.

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

재정의가 전달되므로 War Orchestration은 `$Logistics` 에 대해서도 빌드되며 전체 빌드는 하나의 엔진을 사용합니다. War Orchestration만 재정의하면 Bast Front의 잠금 장치에 명명된 Logistics Core에 대해 구축됩니다.

출시하려면: 프레임워크 PR 를 `$Rivalry` 의 `beta` 로 병합하여 시험판 `rivalry-core-<version>-beta.<commit>` 를 게시합니다. 그런 다음 `$Ashen` 에서 `meitou.lock.json` 를 해당 태그와 자산, SHA -256 및 릴리스 노트의 엔진 커밋으로 설정하고 모드 PR 를 엽니다. 체인에서는 하향식 배송: Logistics Core, War Orchestration 빌드로 잠그고 Bast Front에서 둘 다 잠급니다. 프레임워크의 프로덕션 릴리스 후 잠금을 `rivalry-core-v<version>` 로 이동합니다.

<a id="change-the-engine-and-a-mod-together"></a>
## 엔진과 모드를 함께 변경

mod repo의 하위 모듈 내부에서 엔진 브랜치를 확인하세요.

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- 새 엔진 포인터를 커밋하지 마세요. 엔진 핀은 업스트림 릴리스 태그로만 이동하므로 업스트림에서 릴리스하면 변경 사항이 이 저장소에 도달합니다.
- 프레임워크를 사용하는 모드에는 해당 엔진에 프레임워크를 다시 구축해야 합니다. 위의 프레임워크 재정의를 사용하고 `$Rivalry\engine` 에서 동일한 엔진 브랜치를 확인하세요.
- 착륙 명령: 엔진 PR 를 `fork` 로; 업스트림이 이를 릴리스한 후 프레임워크 PR 가 `$Rivalry\engine` 를 해당 릴리스로 이동한 다음 Mod PR 가 잠금 및 `engine` 를 이동합니다.

<a id="switch-the-engine-to-play-or-test"></a>
## 엔진을 전환하여 플레이하거나 테스트해보세요

최신 업스트림, 릴리스 또는 엔진 브랜치를 재생하려면 모드 저장소의 스크립트로 `$Engine` 클론을 전환하세요.

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream` : VlcekM/ MeitouClient  `master` 의 최신 커밋(누락된 경우 스크립트에서 `upstream` 원격을 추가함)
- `-To Release` : 최신 업스트림 릴리스 태그 `vX.Y.Z` .
- `-To Fork` : 조직 포크의 `fork` 분기를 가져왔습니다.
- `-Branch <name>` : 엔진 PR 의 분기와 같은 조직 포크의 분기입니다.
- `-Path` 가 없으면 모드 저장소 옆에 있는 `MeitouClient` 폴더를 사용합니다. `-Run` 는 Release에서 게임을 빌드하고 시작합니다. `-Build` 는 빌드만 합니다.
- 업스트림, 릴리스 및 `-Branch` 는 분리된 커밋에 복제본을 둡니다. 로컬 `master` 및 `fork` 분기는 변경되지 않습니다. 클론에 커밋되지 않은 변경 사항이 있으면 아무것도 변경하지 않고 스크립트가 중지됩니다.
- 모드 저장소의 자체 `engine` 하위 모듈( `Update-Local.ps1` 및 핀)에는 절대 건드리지 않습니다.

<a id="keep-clones-current"></a>
## 클론을 최신 상태로 유지

끌어올 때마다 엔진 유형이 누락되어(예: `The type or namespace name 'RelationCause' could not be found` ) 빌드가 실패할 때마다 Visual Studio 를 닫고 모드 저장소에서 업데이트 스크립트를 실행하세요.

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- `beta` 로 전환하여 가져온 다음 `engine` 및 `build/gutenstructure` 를 커밋 `beta` 핀으로 업데이트합니다( `git pull` 만 이전 핀에 그대로 둡니다). `-Branch <name>` 는 다른 분기를 가져옵니다.
- 각 하위 모듈을 해당 핀과 비교하여 엔진 버전을 인쇄합니다.
- `Meitou.user.props` 또는 환경에서 `MeitouEngineDir` 및 `MeitouFrameworkSources` 에 플래그를 지정합니다. 이는 빌드가 이 저장소의 핀 대신 다른 저장소의 엔진이나 프레임워크를 사용하도록 만듭니다. 프레임워크와 모드를 함께 변경하지 않는 한 삭제하세요.
- `-Clean` 는 git이 무시한 `bin` 및 `obj` 폴더를 삭제하므로 이전 엔진의 빌드가 재사용되지 않습니다. `-Build` 는 `dotnet build` 를 실행합니다.
- `Up to date` 또는 `FIX` 로 표시된 줄 목록으로 끝납니다.

다음 빌드에서는 `meitou.lock.json` 가 변경된 경우에만 프레임워크를 다시 다운로드합니다.
