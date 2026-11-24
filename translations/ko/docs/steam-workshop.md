<a id="publishing-to-the-steam-workshop"></a>
# Steam 워크샵

모드의 워크샵 항목을 한 번, 수동으로 만들 수 있습니다. ZIP. 그 후에, [워치 워치 워크 워크 워치 워](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) Windows PC에서 실행하고 일정에 따라 그 항목에 각각의 새로운 GitHub 릴리스를 업로드합니다. 모든 단계가 실행됩니다. **윈도우 PowerShell** 당신의 PC에서, 그것을 소유하는 Steam 계정에 가입.

명령은 이러한 값을 사용합니다. PowerShell 창에 한 번 설정하여 모드의 폴더 이름을 사용합니다. `release.yml` `mod-folder`2) 그리고 그것의 첫 번째 출판 버전:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. 모드를 릴리스

1. 융합 `beta` 안으로 들어가기 `main`그 다음에 열고 PR를 결합합니다. `main` 안으로 들어가기 `release` ([부문들](../README.md#branches)).
2. 릴리스 작업 흐름을 기다리십시오 `release`. 그것은 합병을 표시합니다 `<slug>-v<version>` 버전을 통해서 `meitou.json` (예를 들어
`example-mod-v0.1.0`[중고] 자유를 얻기 위해서 `<Mod>-<version>.zip` 그리고 `<Mod>-<version>.zip.sha256`. 만일 그가 움직이기 전에 실패한다면, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` 손으로 눌러서 눌러 (`-DryRun` 오직 명령을 인쇄하십시오.

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. 워크샵 워터를 설치하고 Steam에 로그인

추적하라 [설치하기](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) 워크샵 워치 가이드 설치자는 SteamCMD를 다운로드합니다 `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` 그리고 당신의 Steam 계정에 가입하십시오. 그것이 묻는다면 `Add a mod to watch now?`답글 : 답변 `n`: 항목은 아직 존재하지 않습니다. 워크샵 워치가 이미 설치되어 있는 경우 이 단계를 중단합니다.

<a id="3-prepare-the-items-files"></a>
## 3. 항목의 파일을 준비

1. 릴리스 다운로드 ZIP 그리고 그것을 포장합니다. `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` 지금은 `$Mod.dll`, `meitou.json` 그리고 `$Mod.mod`.
2. 포인트의 프리뷰 이미지를 보관하여 `$Work\preview.png`: PNG 또는 JPG1MB 미만, 평방 (512 x 512 픽셀 작동).

<a id="4-write-the-vdf"></a>
## 4. 글을 쓰세요 VDF

pfmd는 항목을 A에서 만듭니다 VDF 파일: 텍스트 파일 `"key" "value"` 쌍은 내부에 A `"workshopitem"` 블록에 대하여 이 명령은 `$Work\create.vdf` 가득 찬 길을 가져온다. 에디트 `title` 그리고 `description` 첫째는 :

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

| 키 | 가치 |
| --- | --- |
| `appid` | `233860`, KenshiSteam 앱 ID 입니다. |
| `publishedfileid` | `0` 새로운 항목을 만듭니다. 기존 항목과 같은 다른 숫자 업데이트. |
| `contentfolder` | 파일이 항목의 내용이 되게 된 폴더로 가는 완전한 경로: mod 폴더에서 ZIP하지만 부모님은 아니고. |
| `previewfile` | 전체 이미지로 가는 길입니다. |
| `visibility` | `0` 공공의 경우, `1` 친구들만 하여금, `2` 개인적인 것, `3` 녹음되지 않음 시작하여 `2` 그리고 그것은 항목의 페이지에서 변경됩니다. |
| `title` | 워크샵에 있는 항목의 이름. 그것을 시작하여 `[Meitou]`다른 Meitou 코드 모드와 마찬가지로. |
| `description` | 하나의 글자 라인. 다음 항목 페이지에 전체 설명을 삽입 (Steam의 지원) BBCode). |
| `changenote` | 항목의 첫 번째 입력 변경 메모 탭. |

빛나는 가치에 대한 규칙:

- [중고] 좌석에 좌석이 좌석을 좌석으로 좌석(`C:/Users/me/Downloads/...`). 은 을 두 배로 늘려야 한다(`C:\\Users\\me\\...`).
- 한 가치 안에 두 번의 인용문이 쓴다. `\"`.
- 파일을 유지 ASCII.

<a id="5-create-the-item"></a>
## 5. 항목을 만들기

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` 당신이 Steam에 가입하는 이름입니다, 설치자에게 주어진 바와 같이. steamcmd는 저장된 로그인을 사용합니다. 암호 또는 Steam Guard 코드를 요청하는 경우, 실행 `powershell -ExecutionPolicy Bypass -File $Watcher login`그리고 다시 명령을 내리십시오.

경기는 끝나고 `Success.`. Steam > 프로필 > 워크샵 항목을 열고 다음 번호를 복사합니다. `?id=` 그의 속에서 URL이것은 항목 ID입니다.

<a id="6-finish-the-items-page"></a>
## 6. 포인트의 페이지를 끝내기

Steam에서 항목의 페이지에서 :

1. Steam 워크샵의 법적 계약을 받아들이면 페이지가 배너를 표시합니다. 당신이 할 때까지, 당신은 단지 항목을 볼 수 있습니다.
2. 전체 설명을 작성하십시오.
3. 모드에 다른 워크샵 모드(예 : Rivalry Core)가 필요하다면, 아래에 추가 **Add/Remove 필요한 항목**. 가입자들은 다음과 같다.
또한 그것에 가입하도록 요청했습니다. 의존성에 대해서는 `meitou.json` `dependencies`, 라우터의 순서가 먼저 로드 순서에 넣습니다.
4. 에 대한 시야를 바꾸어 **공공의** 페이지가 준비되면

<a id="7-watch-the-item"></a>
## 7. 항목을 보세요

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

건조한 실행은 이미 업로드 된 것처럼 현재의 출시를보고합니다. 제거 `$Work` 한 번 할 때.

<a id="each-release-after-that"></a>
## 그 후에 각각의 자유를

단계 1에 있는 것처럼 모드를 풀어주십시오. 다음글 : 시청자의 계획된 실행은 다음을 다운로드합니다. ZIP그것을 확인하십시오 SHA-256 그리고 그것을 항목에 업로드, 출판 제목과 변경 메모로 링크. 그것은 항목의 제목, 설명, 예측 또는 가시성을 변경하지 않습니다 : 항목의 페이지에있는 것. `powershell -ExecutionPolicy Bypass -File $Watcher status` 일정, 다음 라운드 및 마지막 업로드를 표시합니다.
