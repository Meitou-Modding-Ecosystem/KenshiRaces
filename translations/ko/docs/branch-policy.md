<a id="permanent-branch-policy-and-repository-onboarding"></a>
# 지속적인 지점 정책 및 리포티토리 탑승

모든 Meitou-Modding-Ecosystem 저장소는 [중앙 지속적인 정책](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md):

| 부문 | 요구사항 |
| --- | --- |
| 일반적인 부문 | 보호되지 않은 |
| 베타 | PR + CI를 통과; 슈퍼 유지 보수자는 자체 조합을 포함하여 검토를 통과 할 수 있습니다 |
| 주요 / 릴리스 | PR + 통과하는 CI+ 하나의 독립적 인 슈퍼 유지 보수 승인; 검토 바이파스 없음 |

GitHub 액션은 리뷰 바이파스를 가지고 있지 않습니다. 베타 팀 예외는 리뷰에만 적용됩니다: 의무 PR/CI 안전은 바이파스가 없습니다. CODEOWNERS 추가 승인 요구 사항이 없습니다.

GitHub 템플릿은 사본, 지점 규칙, 팀 허가, 환경 또는 비밀이 아닙니다. GutenStructure의 Configure Release Branch는 17 분에 새로운 공공 기관을 발견하고, 실패하면 팀 기관에 액세스 할 수 있습니다. 즉시 새로운 리포를 설정하려면 권한이있는 GutenStructure 유지 보수를 요청하고 주요 목표 조직에 해당 작업을 적용합니다. 보호 된 조합이나 출판을 신뢰하기 전에 이것을하십시오. CI에서 출시 준비를 유지하고 실제 통과를 확인하십시오; 일치하지 않는 작업 흐름은 영원히 기다립니다.

창조하라 main/release 검토 된 안정적인 출처에서 준비; 정책은 존재하기 전에 그들을 대상으로합니다. 이 템플릿의 변함없는 검토 된 중앙 작업 흐름을 유지하고 PR를 통해 출시 포트와 패키지 출판물을 업데이트합니다. 개인 휴식은 별도로 필요합니다 plan/protection 유효성; 발견에 의해 Azure 액세스가 허용되지 않습니다. 외부 플랫폼 비밀은 저자 소유 환경에 남아 있으며이 템플릿에서 복사되지 않습니다.
