<a id="publishing-to-the-steam-workshop"></a>
# Steam 창작마당에 게시

ZIP 릴리스에서 직접 모드의 창작마당 항목을 한 번 생성합니다. 그 후 [Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) 는 Windows PC 에서 실행되고 각각의 새로운 GitHub 릴리스를 일정에 따라 해당 항목에 업로드합니다. 모든 단계는 항목을 소유할 Steam 계정으로 로그인한 PC 의 **Windows PowerShell**에서 실행됩니다.

명령은 이러한 값을 사용합니다. 모드의 폴더 이름(예: `release.yml`  `mod-folder` ) 및 첫 번째 릴리스 버전을 사용하여 PowerShell 창당 한 번 설정합니다.

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. 모드 출시

1. `beta` 를 `main` 로 병합한 다음 `main` 의 PR 를 열고 `release` ([분기](../README.md#branches) )로 병합합니다.
2. `release` 의 릴리스 워크플로를 기다립니다. 병합 `<slug>-v<version>` 를 `meitou.json` 의 버전(예: `example-mod-v0.1.0` )으로 태그 지정하고 `<Mod>-<version>.zip` 및 `<Mod>-<version>.zip.sha256` 를 사용하여 릴리스를 생성합니다. 태그를 지정하기 전에 해당 실행이 실패하면 `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` 는 태그를 수동으로 푸시합니다( `-DryRun` 는 명령만 인쇄합니다).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Workshop Watcher를 설치하고 Steam에 로그인하세요.

Workshop Watcher 가이드의 [](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) 설치]를 따르세요. 설치 프로그램은 steamcmd를 `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` 에 다운로드하고 Steam 계정에 로그인합니다. `Add a mod to watch now?` 를 물으면 `n` 라고 ​​대답하세요. 항목이 아직 존재하지 않습니다. Workshop Watcher가 이미 설치되어 있으면 이 단계를 건너뛰세요.

<a id="3-prepare-the-items-files"></a>
## 3. 아이템의 파일을 준비하세요

1. ZIP 릴리스를 다운로드하고 `$Work` 에 압축을 풉니다.

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` 는 이제 `$Mod.dll` , `meitou.json` 및 `$Mod.mod` 를 보유합니다.
2. 항목의 미리보기 이미지를 `$Work\preview.png` : PNG 또는 JPG , 1 MB , 정사각형(512 x 512 픽셀 작동)으로 저장합니다.

<a id="4-write-the-vdf"></a>
## 4. VDF 를 작성하세요

steamcmd는 VDF 파일( `"workshopitem"` 블록 내부에 있는 `"key" "value"` 쌍의 텍스트 파일)에서 항목을 생성합니다. 이 명령은 경로가 채워진 `$Work\create.vdf` 를 작성합니다. 먼저 `title` 및 `description` 를 편집합니다.

```powershell
$Dir = $Work.Replace('\', '/')
@"
"workshopitem"
{
    "appid"           "233860"
    "publishedfileid" "0"
    "contentfolder"   "$Dir/$Mod"
    "previewfile"     "$Dir/preview.png"
    "visibility"      "2"
    "title"           "[Meitou] My Mod"
    "description"     "What the mod does. Requires MeitouClient."
    "changenote"      "$Mod $Version"
}
"@ | Set-Content "$Work\create.vdf" -Encoding ASCII
```

| 열쇠 | 값 |
| --- | --- |
| `appid` | `233860` , Kenshi 의 스팀 앱 ID . |
| `publishedfileid` | `0` 가 새 항목을 생성합니다. 다른 번호는 기존 항목을 업데이트합니다. |
| `contentfolder` | 파일이 항목의 콘텐츠가 되는 폴더의 전체 경로: 상위 폴더가 아닌 ZIP 의 mod 폴더입니다. |
| `previewfile` | 미리보기 이미지의 전체 경로입니다. |
| `visibility` | `0` 공개, `1` 친구 전용, `2` 비공개, `3` 미등록. `2` 로 시작하여 항목 페이지에서 변경하세요. |
| `title` | 창작마당에 있는 아이템 이름입니다. 다른 Meitou 코드 모드처럼 `[Meitou]` 로 시작하세요. |
| `description` | 한 줄의 텍스트입니다. 나중에 항목 페이지에 전체 설명을 작성하세요(Steam의 BBCode 지원). |
| `changenote` | 항목 변경 메모 탭의 첫 번째 항목입니다. |

값에 대한 규칙:

- 슬래시( `C:/Users/me/Downloads/...` )를 사용하여 경로를 작성합니다. 백슬래시는 두 번 사용해야 합니다( `C:\\Users\\me\\...` ).
- 값 내부의 큰따옴표는 `\"` 로 작성됩니다.
- ASCII 파일을 유지하십시오.

<a id="5-create-the-item"></a>
## 5. 아이템 생성

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` 는 설치 프로그램에 부여된 Steam에 로그인하는 데 사용하는 이름입니다. steamcmd는 저장된 로그인을 사용합니다. 비밀번호나 Steam Guard 코드를 묻는 경우 `powershell -ExecutionPolicy Bypass -File $Watcher login` 를 실행한 다음 명령을 다시 실행하세요.

실행은 `Success.` 로 끝납니다. Steam > 프로필 > 창작마당 아이템을 열고 새 아이템을 열고 URL 에서 `?id=` 뒤의 번호를 복사하세요. 즉, 아이템 ID 입니다.

<a id="6-finish-the-items-page"></a>
## 6. 항목 페이지를 완료합니다.

Steam의 항목 페이지에서:

1. 페이지에 배너가 표시되면 Steam 창작마당 법적 계약에 동의하세요. 그러기 전까지는 본인만 해당 항목을 볼 수 있습니다.
2. 전체 설명을 작성하세요.
3. 모드에 다른 워크샵 모드(예: Rivalry Core)가 필요한 경우 **필요한 항목 추가/제거**에 추가하세요. 그러면 구독자에게도 구독하라는 메시지가 표시됩니다. `meitou.json`  `dependencies` 의 종속성을 사용하면 실행기의 배열이 로드 순서에서 첫 번째로 배치됩니다.
4. 페이지가 준비되면 공개 상태를 **공개**로 변경합니다.

<a id="7-watch-the-item"></a>
## 7. 아이템 보기

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

테스트 실행에서는 현재 릴리스가 이미 업로드된 것으로 보고합니다. 일단 `$Work` 를 제거하십시오.

<a id="each-release-after-that"></a>
## 이후 출시될 때마다

1단계에서와 같이 모드를 해제합니다. 감시자의 다음 예정된 실행은 ZIP 를 다운로드하고 SHA -256을 확인한 후 릴리스 제목과 링크를 변경 노트로 사용하여 항목에 업로드합니다. 항목의 제목, 설명, 미리보기 또는 가시성은 변경되지 않습니다. 항목 페이지에서 변경하십시오. `powershell -ExecutionPolicy Bypass -File $Watcher status` 에는 일정, 다음 실행 및 마지막 업로드가 표시됩니다.
