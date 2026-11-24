<a id="local-development-setup"></a>
# 지역 개발 설정

당신이 작업하는 레스토랑을 하나의 폴더로 클론하십시오. 각각은 자기 자신을 위해서 `dotnet build`, 그리고 그것은 다운로드하고 hash-check 자신을 필요로하는 프레임.

<a id="repos"></a>
## 휴식

각 모드 리포는 모드에 따라 이름을 지정합니다.

| 리포 | 홀드 | PR가 가는 |
| --- | --- | --- |
| MeitouClient | 엔진과 모드 호크 | `fork` |
| 리얼리 코어 | Rivalry Core 프레임 워크와 에이전트 기술 | `beta` |
| 로지스틱 코어 | Logistics Core 프레임 워크 | `beta` |
| 전쟁기구 | 전쟁 오케스트레이션 프레임 워크 (logistics core 사용) | `beta` |
| 바스트프론트 | Bast Front Melee (과과의 전쟁 오케스트라 및 로지스틱 코어를 사용) | `beta` |
| 아쉬운드 | 빛의 손 ( 빛의 코어를 사용) | `beta` |
| 결혼식 | 결혼식 | `beta` |
| 스쿠아드 | SquadArena 모드와 squad 등급 도구 | `beta` |
| 녹색 녹색 (TheIvoryDisaster) | 공유 CI, 포장, 출시 포트, 프레임 로크 목표 | `beta` |

<a id="folders"></a>
## 폴더

이 단계는 8 개의 폴더를 사용합니다. 각각의 새로운 PowerShell 창에서 변수로 설정한 다음 아래의 명령을 입력합니다.

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
## 원칙들

- .NET 11 SDK (모든 리포의 `global.json` 이름은 버전) 그리고 Git.
- 편집자: Visual Studio, C# Dev Kit 또는 Rider와 함께 VS 코드. 단 하나의 터미널도 작동합니다.
- Kenshi 설치된 것 입니다. 만약 그 안에 있지 않다면 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`세트 The `KENSHI_PATH` 환경 변수
그의 포스터에 대한

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

각 모드 리포는 자신의 `engine` 하위 모듈, 그래서 `$Engine` 단지 엔진 호크를 작성하는 것이 필요합니다.

<a id="build-and-play-one-mod"></a>
## 하나의 모드를 만들고 재생합니다.

**모드의 리포로 변경:**

```powershell
cd $Ashen
dotnet build
dotnet run --project Play
```

- 첫 번째 건물은 각 프레임을 다운로드합니다. `meitou.lock.json` 안으로 들어가기 `deps` 그리고 그것이 멈추면 SHA256 이거 맞지 않아요.
- `dotnet run --project Play` 닫힌 프레임과 모드를 복사합니다. `Kenshi\mods` 그리고 엔진이 시작됩니다. 시각 스튜디오 vs 코드
그리고 설치된 라운더: [play.md](play.md).
- 라우터에서 모드 위의 프레임을 허용하십시오, 어떤 의존도.

<a id="change-a-framework-and-a-mod-together"></a>
## 프레임 워크와 모드를 함께 변경합니다.

당신의 프레임 워크 클론에 모드 리포를 지정하십시오 :

```powershell
Set-Content "$Ashen\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$Rivalry\RivalryCore\RivalryCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Rivalry\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

- 건물에 건설 `$Ashen` 이제 Rivalry Core에서 컴파일 `$Rivalry` 그리고 엔진을 사용하여 `$Rivalry\engine`, 그리고 플레이는 Rivalry 코어가 구축하는 것을 배포합니다.
- `Meitou.user.props` 그들은 GIT 무시됩니다. 만약 당신이 이미 당신의 Kenshi 설정, 두 개의 라인을 추가합니다. `PropertyGroup` 대신에
- `MeitouFrameworkSources` 프로젝트를 통해 분리된 프레임 워크 `;`그럼 G. `$Logistics\LogisticsCore\LogisticsCore.csproj`.
`MeitouEngineDir` 프레임 클론이 구축하는 엔진을 이름합니다.
- 닫힌 줄로 돌아가기 위해 라인을 삭제 ZIP.

**프레임 워크 체인** 전선을 다 `meitou.lock.json` 전쟁 오케스트라 및 로지스틱 코어 목록. 두 개의 프레임과 Bast Front을 함께 변경하려면 Bast Front에서 두 개를 위로하십시오.

```powershell
Set-Content "$Bast\Meitou.user.props" "<Project><PropertyGroup><MeitouFrameworkSources>$War\WarOrchestration\WarOrchestration.csproj;$Logistics\LogisticsCore\LogisticsCore.csproj</MeitouFrameworkSources><MeitouEngineDir>$Bast\engine\</MeitouEngineDir></PropertyGroup></Project>"
```

은 을 고, 그래서 전쟁 오케스트라션이 건설 `$Logistics` 또한, 전체 건물은 하나의 엔진을 사용합니다. 그것은 전쟁 오케스트라션만을 초월하고, 그것은 바스트 프론트의 잠금에 명명 된 로지스틱스 코어에 대항하여 건설됩니다.

그것을 배송하기 위해 : 프레임 워크 PR를 통합 `beta` 는 는 `$Rivalry`◇ 출판된 ◇ ◇ □ ◇ △ ◇ ◎ ◇ ◄ `rivalry-core-<version>-beta.<commit>`. 그리고 그 후에, `$Ashen`세트 `meitou.lock.json` 그 자산과 그 자산에 대하여, SHA-256 그리고 엔진은 그것의 출시 노트에서, 그리고 모드 PR를 열어. 쇠사슬에서, 배는 위로 내려갑니다 : 로지스틱 코어, 그 다음에 전쟁 오케스트라를 구축 그것을 잠그고, 다음에 Bast 전선 두 개를 잠그고. 프레임의 생산을 시작한 후, 잠금을 옮기십시오. `rivalry-core-v<version>`.

<a id="change-the-engine-and-a-mod-together"></a>
## 엔진과 모드를 함께 변경합니다.

mod repo의 submodule 내부의 엔진 지점을 확인하십시오:

```powershell
git -C "$Ashen\engine" fetch origin <engine branch>
git -C "$Ashen\engine" checkout FETCH_HEAD
```

- 새로운 엔진 지표를 수행하지 마십시오 : 엔진 핀은 오프 스트리밍 태그에만 이동합니다.
그것을 풀어 주세요.
- 프레임을 사용하는 모드는 또한 그 엔진에 재건 된 프레임이 필요합니다. 위의 프레임 워크를 사용하고 동일한 것을 확인하십시오.
엔진의 흐름은 `$Rivalry\engine`.
- 주문을 착륙하십시오 : 엔진 PR `fork`· upstream이 출시되면 프레임 워크가 이동합니다. `$Rivalry\engine` 그 해방을 위해서, 그 후에
모드 PR는 잠금을 움직이고 `engine`.

<a id="switch-the-engine-to-play-or-test"></a>
## 엔진을 스위치하여 재생하거나 테스트하십시오.

최신 업스트림, 리버 또는 엔진 지점을 재생하려면 `$Engine` 모든 모드 리포에서 스크립트를 클론 :

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Switch-Engine.ps1" -To Upstream -Path $Engine -Run
```

- `-To Upstream`· 최신 헌신 VlcekM/MeitouClient `master` (이 글은 추가된 내용입니다) `upstream` 잃어버렸을 때 멀리 떨어져서)
- `-To Release`: 최신 upstream 출시 태그 `vX.Y.Z`.
- `-To Fork`오르가 포크의 `fork` 꼬리, 꼬리 꼬리
- `-Branch <name>`: 엔진 PR와 같은 오르가 포크의 지점.
- 없이는 `-Path` 그것은 그것을 사용한다 `MeitouClient` 모드 리포 근처의 폴더. `-Run` 게임을 시작하고, 그것을 시작합니다. `-Build` 오직
그것을 건설한다.
- 업그레이드, 출시 및 `-Branch` 클론을 분리된 위원회에 남겨 두고; 지역 `master` 그리고 `fork` 그들의 지점은 바뀌지 않았다. 스크립트는
클론이 명령되지 않은 변화를 가질 때 아무것도 바꾸지 않고 멈추십시오.
- 그것은 결코 모드 리포의 자신의 만지지 않습니다. `engine` 밑 모듈 (이것은 `Update-Local.ps1` 그리고 그 핀들.

<a id="keep-clones-current"></a>
## 클론을 현재로 유지

각 트랙 후, 그리고 엔진 유형이 없을 때마다 (예를 들어, `The type or namespace name 'RelationCause' could not be found`), Visual Studio를 닫고 mod의 repo에서 업데이트 스크립트를 실행하십시오 :

```powershell
powershell -ExecutionPolicy Bypass -File "$Ashen\tools\Update-Local.ps1" -Clean -Build
```

- 그것은 그대로 움직인다 `beta` 그리고 그것을 끌고, 그 다음에 업데이트 `engine` 그리고 `build/gutenstructure` 의무에 대하여 `beta` 핀스 (`git pull` 혼자서
오래된 것들 위에 던져 버린다.) `-Branch <name>` 또 하나의 지점을 끌어들인다.
- 그것은 각 하위 모듈을 그것의 핀에 대해 검사하고 엔진 버전을 인쇄합니다.
- 그것은 깃발 `MeitouEngineDir` 그리고 `MeitouFrameworkSources` 안에 있는 `Meitou.user.props` 또는 환경: 그들은 건물을 다른 사용
repo의 엔진 또는 프레임 대신이 repo의 핀. 프레임 워크와 모드를 함께 변경하지 않는 한 삭제합니다.
- `-Clean` Git-ignored를 삭제합니다. `bin` 그리고 `obj` 폴더, 그래서 오래된 엔진의 건설이 다시 사용되지 않습니다. `-Build` 경주 `dotnet build`.
- 그것은 끝나고 `Up to date` 또는 표시된 라인의 목록 `FIX`.

다음 건물은 다시 프레임을 다운로드 할 때만 `meitou.lock.json` 변화하는 것
