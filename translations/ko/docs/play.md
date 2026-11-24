<a id="build-and-run-locally"></a>
# 현지적으로 건설하고 실행하십시오.

건물은 `Play` 엔진을 구축하고,이 리포에서 각 모드를 복사하고, 모드와 프레임을 복사합니다. `meitou.lock.json` 게임의 모드 폴더에 이름을 넣고 게임이 시작됩니다. 당신이 좋아하는 모든 도구를 사용하십시오 : Visual Studio, VS 코드 또는 터미널 모두 같은 구조를 실행합니다.

세 사람은 모두 필요하다.NET 11 SDK 그리고 그것의 하위 모듈을 가진 클론 (`git clone --recurse-submodules <repo URL>`또는 `git submodule update --init --recursive` 기존의 클론에 대한 그들이 없으면 건물은 멈추게 된다. `MEITOU001`그 명령을 부르며

<a id="visual-studio"></a>
## 시각 스튜디오

1. 리포의 개방 `.slnx`.
2. 세트 `Play` Visual Studio가 그것을 선택하지 않은 경우, 그것은 스타트업 프로젝트로 설정됩니다 (오른쪽 클릭 > 스타트업 프로그램으로 설정).
3. 시작 버튼 옆에 있는 내려가기에서 프로필을 선택한 다음 F5 (debug) 또는 Ctrl+F5를 누릅니다.

| 프로파일 | 경주 | 그것을 사용하여 |
| --- | --- | --- |
| 엔진 + 모드 (Motor + Mods) | `MeitouClient.exe` 이 의 에서 만들어진 `engine` 하위 모듈 | 테스트 및 당신의 모드를 탐색; 모드 코드 작업에 대한 파괴점 |
| Meitou Launcher (설치된) | 메이투에서 설치된 라우터가 출시되며, `%LOCALAPPDATA%\MeitouLauncher\current\meitou-launcher.exe` | 모드 목록이나 로드 명령을 변경하고 새로운 모드 코드를 신뢰하십시오. |

<a id="vs-code"></a>
## VS 코드

1. C# DevKit 확장 프로그램을 설치하십시오 (VS 코드는 폴더를 열 때 제공됩니다. `.vscode/extensions.json`).
2. Repo의 뿌리 폴더를 열어 (File > Open Folder)
3. 실행 및 데뷔 (Ctrl + Shift + D), 선택 **엔진 + 모드 (Motor + Mods)**F5를 눌러주세요. 그것은 먼저 구축하고 배치하고, 그 다음에 엔진을 시작합니다.
Debugger; 모드 코드 작업의 붕괴점.
4. 설치된 라우너: 터미널 > 실행 작업 > **Meitou Launcher (설치된)**. 그것은 건설하고 배치하고, 그 다음에 발사기를 열어줍니다.

Terminal > Run Build Task (Ctrl+Shift+B)는 단지 구축 및 배포합니다.

<a id="without-an-editor"></a>
## 편집자가 없으면

오른쪽 클릭 `tools\Build.ps1` > **PowerShell을 사용하여 실행**. 건설 및 배치 `dotnet build` (그들이 사라지면 먼저 하위 모듈을 확인) 그리고 창을 열어 결과를 읽을 수 있습니다. `-Test` 또한 테스트를 진행하고 있습니다. `-Run` 그리고 엔진이 시작됩니다.

<a id="terminal"></a>
## 터미널

RPO의 뿌리 폴더에서:

```powershell
dotnet build
dotnet test
dotnet run --project Play
```

- `dotnet build` 그리고 `dotnet test` 모든 프로젝트를 구축하고 테스트합니다.
- `dotnet run --project Play` 엔진을 구축하고, 배치하고, 시작합니다 (플레이 (엔진 + 모드) 프로필).
- 대신 설치된 라우너를 사용하려면 엔진을 시작하지 않고 배치하고 라우너가 열립니다.

```powershell
dotnet build Play
& "$env:LOCALAPPDATA\MeitouLauncher\current\meitou-launcher.exe"
```

<a id="what-a-play-build-does"></a>
## 어떤 게임을 만들고 있는지

- 모드를 찾으십시오 : 각 상단 수준의 폴더를 보유 `meitou.json` 그리고 `<Folder>/<Folder>.csproj`. 새로운 모드는 변화를 필요로하지 않습니다. `Play`.
- 엔진과 각 모드를 구축 **릴리스**, 당신이 선택한 구성에 상관없이 : 디버그 엔진은 던지기를 구축 `TaskStart.CheckStarts`
한 번 모드는 캐릭터의 작업을 시작합니다. 모드 코드의 분할점 (Release symbols are deployed)은 여전히 작동합니다.
- 각 모드의 복사본 `<Mod>.dll`, `<Mod>.pdb` 그리고 `meitou.json` 는 는 `<Kenshi>\mods\<Mod>\`. 게임을 먼저 닫으십시오 : 실행 게임은 DLL를 닫습니다.
- 각 프레임 워크에 대한 복사 `meitou.lock.json` 다운로드된 폴더에서 `<Kenshi>\mods\<Framework>\`또는 건설된 프레임 워크를 위해서
클로나에 대한 정보([local-development.md](local-development.md)그들의 생산성은 구축된다.
- 데이터 생성기 (Data Generator)`<Mod>.Data/<Mod>.Data.csproj`은 은 `<generator> <Kenshi folder> <output .mod>`* 그것을 얻는 `<Mod>.mod`
설치된 게임에서 생성된; 실패는 건설 경고이며 모드는 그것없이 배치됩니다. 다른 모든 모드는 텅 비어있다. `<Mod>.mod` 존재하지 않는 경우; 존재하는 경우는 결코 대체되지 않습니다.
- A와 함께 하는 모드 `Structures` 폴더는 그것을 얻는다 `.glb` 그것은 그것의 폴더에 수입 된 모델, 그것의 기록에 합류 `.mod`
([structures.md](structures.md)).
- 모드 폴더가 존재하지 않을 때, 아무것도 복사되지 않으며 건물은 그렇게 말합니다.
- 라우너의 플레이 버튼은 그것이 설치 한 출시를 시작하고,이 리포의 엔진이 아닙니다. 그것을 닫고 "Play (engine + mods)"를 실행하여 귀하의
건설을 하다

<a id="settings"></a>
## 설정을

단점 : 게임 `C:\Program Files (x86)\Steam\steamapps\common\Kenshi`모델 번호:mods `<game>\mods`. 그들을 바꾸어라 `KENSHI_PATH` 환경 변수 (엔진은 또한 그것을 읽고, 그래서 둘 다 동의), 또는 기계에서 git 무시 `Meitou.user.props` RPO의 뿌리 폴더에서:

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

엔진은 스스로 게임을 찾을 수 있습니다 (Steam); `KenshiDir` 단지 안에 앉아서 `Meitou.user.props` 그것에 전달되지 않았다. VS 코드의 출시 구성은 Release 엔진을 실행합니다; ♰ `.vscode/launch.json` 현지어로 변경할 때 `PlayConfiguration`.

<a id="first-run"></a>
## 첫 번째 라운드

- 설치된 라우터를 한 번 실행하십시오 (위의 모든 방법) 모드와 그 프레임을 활성화하고 코드를 신뢰합니다.
- 복사본이 " 접근 거부"로 실패하면 사용자에게 변경 권리를 부여합니다. `Kenshi\mods` (자유 > 보안) 대신 당신의
관리자로서의 편집자
