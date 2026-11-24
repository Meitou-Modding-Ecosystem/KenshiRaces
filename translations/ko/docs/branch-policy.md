# 브랜치 보호 및 새 저장소 설정

조직의 모든 저장소는 공통 상시 정책을 따릅니다. [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| 브랜치 | 요건 |
| --- | --- |
| 일반 작업 브랜치 | 보호하지 않습니다. |
| `beta` | PR과 CI 통과가 필요합니다. super-maintainers는 자신의 PR을 병합할 때도 리뷰를 생략할 수 있습니다. |
| `main` / `release` | PR, CI 통과, 독립적인 super-maintainer 한 명의 승인이 필요합니다. 리뷰 예외는 없습니다. |

GitHub Actions에는 리뷰 예외가 없습니다. beta에서 팀에 부여된 예외는 리뷰에만 적용됩니다. PR과 필수 CI 검사는 언제나 필요합니다. CODEOWNERS가 추가 승인을 요구하지는 않습니다.

GitHub 템플릿은 소스를 복사합니다. 규칙 세트, 팀 권한, 환경, 비밀 값은 별도로 설정합니다. 매시 17분에 중앙 워크플로가 새 공개 저장소를 찾아 필요한 경우 팀에 쓰기 권한을 주고 정책을 적용합니다. 즉시 설정하려면 권한이 있는 GutenStructure (`Configure release branch protections`) 관리자가 main에서 조직을 대상으로 미리 보기를 실행한 후 적용해야 합니다. 보호된 병합을 사용하거나 게시하기 전에 완료하세요.

호출 측 CI에 Release readiness를 유지하고 실제 실행이 통과했는지 확인하세요. 대응하는 워크플로가 없는 필수 검사는 계속 대기합니다. 검토를 마친 안정적인 소스에서 main과 release를 만드세요. 두 브랜치를 만들기 전부터 정책의 대상입니다.

템플릿의 검토된 고정 워크플로 참조를 유지하세요. 릴리스 검사와 패키지 게시 도구는 PR로 함께 업데이트합니다. 비공개 저장소는 요금제와 보호 규칙을 별도로 확인해야 합니다. 저장소 검색 과정은 Azure 접근 권한을 부여하지 않습니다. 외부 플랫폼의 비밀 값은 작성자의 환경에만 남습니다.

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
