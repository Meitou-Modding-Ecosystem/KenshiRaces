<a id="build-and-run-locally"></a>
# 로컬에서 빌드 및 실행

`Play` 를 빌드하면 이 저장소의 엔진과 모든 모드가 빌드되고 `meitou.lock.json` 이름의 모드와 프레임워크가 게임의 mods 폴더에 복사된 다음 게임이 시작됩니다. 원하는 도구를 사용하세요. Visual Studio , VS Code 또는 터미널은 모두 동일한 빌드를 실행합니다.

세 가지 모두 . NET 11 SDK 및 하위 모듈이 있는 클론( `git clone --recurse-submodules <repo URL>` 또는 기존 클론의 `git submodule update --init --recursive` ). 그것들이 없으면 빌드는 `MEITOU001` 로 중지되고 해당 명령의 이름이 지정됩니다.

<a id="visual-studio"></a>
## Visual Studio

1. 저장소의 `.slnx` 를 엽니다.
2. Visual Studio 가 선택하지 않은 경우 `Play` 를 시작 프로젝트로 설정합니다(마우스 오른쪽 버튼 클릭 > 시작 프로젝트로 설정).
3. 시작 버튼 옆의 드롭다운에서 프로필을 선택한 다음 F5(디버그) 또는 Ctrl+F5를 누릅니다.

| 윤곽 | 실행 | 다음 용도로 사용하세요. |
| --- | --- | --- |
| 플레이(엔진 + 모드) | 이 저장소의 `engine` 하위 모듈에서 빌드된 `MeitouClient.exe` | 모드 테스트 및 디버깅 모드 코드 작업의 중단점 |
| Meitou 런처(설치됨) | Meitou 릴리스, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` 에서 설치된 런처 | 모드 목록 또는 로드 순서 변경 및 새 모드 코드 신뢰 |

<a id="vs-code"></a>
## VS Code

1. C# Dev Kit 확장(폴더를 열면 VS Code 가 제공하며 `.vscode/extensions.json` 에 있음)을 설치합니다.
2. 저장소의 루트 폴더를 엽니다(파일 > 폴더 열기).
3. 실행 및 디버그(Ctrl+Shift+D)하고 **재생(엔진 + 모드)**을 선택한 다음 F5를 누릅니다. 먼저 빌드하고 배포한 다음 디버거에서 엔진을 시작합니다. 모드 코드 작업의 중단점.
4. 설치된 런처의 경우: 터미널 > 작업 실행 > ** Meitou 런처(설치됨)**. 빌드 및 배포한 다음 실행기를 엽니다.

터미널 > 빌드 작업 실행(Ctrl+Shift+B)은 빌드 및 배포만 수행합니다.

<a id="without-an-editor"></a>
## 편집자 없이

`tools\Build.ps1` > **PowerShell로 실행**을 마우스 오른쪽 버튼으로 클릭합니다. 이는 `dotnet build` 처럼 빌드 및 배포하고(하위 모듈이 누락된 경우 먼저 확인) 결과를 읽을 수 있도록 창을 열어 둡니다. `-Test` 도 테스트를 실행합니다.  그러면 `-Run` 가 엔진을 시동합니다.

<a id="terminal"></a>
## 단말기

저장소의 루트 폴더에서:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` 및 `dotnet test` 는 모든 프로젝트를 빌드하고 테스트합니다.
- `dotnet run --project Play` 는 엔진("Play(엔진 + 모드)" 프로필)을 빌드, 배포 및 시작합니다.
- 대신 설치된 런처를 사용하려면 엔진을 시작하지 않고 배포한 다음 런처를 엽니다.

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## 플레이 빌드의 역할

- 모드를 찾습니다: `meitou.json` 및 `<Folder>/<Folder>.csproj` 를 보유한 모든 최상위 폴더. 새로운 모드는 `Play` 를 변경할 필요가 없습니다.
- 선택한 구성에 관계없이 **릴리스**에서 엔진과 각 모드를 빌드합니다. 모드가 캐릭터의 작업을 시작하면 디버그 엔진 빌드에서 `TaskStart.CheckStarts` 가 발생합니다. 모드 코드의 중단점이 여전히 작동합니다(릴리스 기호가 배포됨).
- 각 모드의 `<Mod>.dll` , `<Mod>.pdb` 및 `meitou.json` 를 `<Kenshi>\mods\<Mod>\` 에 복사합니다. 먼저 게임을 닫으세요. 실행 중인 게임이 dll을 잠급니다.
- 다운로드한 릴리스 폴더에서 `meitou.lock.json` 의 각 프레임워크를 `<Kenshi>\mods\<Framework>\` 로 복사하거나, 복제본([local-development.md](local-development.md) )에서 빌드된 프레임워크의 경우 해당 빌드 출력을 복사합니다.
- 데이터 생성기가 있는 모드( `<Mod>.Data/<Mod>.Data.csproj` , 다음과 같이 실행 `<generator> <Kenshi folder> <output .mod>` ) 그것을 얻는다 `<Mod>.mod` 설치된 게임에서 생성됨 실패는 빌드 경고이며 모드는 경고 없이 배포됩니다. 다른 모든 모드는 비어 있습니다. `<Mod>.mod` 아무것도 없을 때; 기존 것은 결코 교체되지 않습니다.
- `Structures` 폴더가 있는 모드는 `.glb` 모델을 배포된 폴더로 가져오고 레코드는 `.mod` ([structures.md](structures.md) )에 병합됩니다.
- mods 폴더가 존재하지 않으면 아무것도 복사되지 않으며 빌드에서 그렇게 말합니다.
- 런처의 재생 버튼은 이 저장소의 엔진이 아닌 설치된 릴리스를 시작합니다. 그것을 닫고 "Play (engine + mods)"를 실행하여 빌드를 테스트하십시오.

<a id="settings"></a>
## 설정

기본값: 게임 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi` , 모드 폴더 `<game>\mods` . `KENSHI_PATH` 환경 변수를 사용하여 변경하거나(엔진도 이를 읽으므로 둘 다 동의함) 저장소 루트 폴더에 있는 git 무시된 `Meitou.user.props` 의 머신별로 변경합니다.

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

엔진은 자체적으로 게임을 찾습니다(Steam). `Meitou.user.props` 에만 설정된 `KenshiDir` 는 전달되지 않습니다. VS Code 의 실행 구성은 릴리스 엔진을 실행합니다. `PlayConfiguration` 를 변경하는 경우 로컬에서 `.vscode/launch.json` 를 편집하세요.

<a id="first-run"></a>
## 첫 번째 실행

- 설치된 런처를 한 번 실행하십시오(위의 방법 중 하나): 모드와 해당 프레임워크를 활성화하고 해당 코드를 신뢰하십시오.
- "액세스 거부"로 인해 복사가 실패하는 경우 편집기를 관리자로 실행하는 대신 사용자에게 `Kenshi\mods` (속성 > 보안)에 대한 수정 권한을 부여하십시오.
