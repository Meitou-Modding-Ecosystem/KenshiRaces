# 分支保护与新仓库配置

组织内的所有仓库均遵循统一的长期政策。 [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| 分支 | 要求 |
| --- | --- |
| 普通工作分支 | 不设保护。 |
| `beta` | 必须提交 PR 并通过 CI。super-maintainers 可豁免审查，也可在 CI 通过后自行合并自己的 PR。 |
| `main` / `release` | 必须提交 PR、通过 CI，并获得一名独立 super-maintainer 的批准。不得豁免审查。 |

GitHub Actions 没有豁免审查的权限。beta 分支的团队例外仅适用于审查，PR 和必需的 CI 检查始终不可省略。CODEOWNERS 不会增加额外的批准要求。

GitHub 模板复制源代码。规则集、团队权限、环境和机密需要另行配置。中央工作流每小时第17分钟发现新的组织公共仓库，必要时为团队授予写权限，并应用政策。如需立即配置，由获授权的 GutenStructure (`Configure release branch protections`) 维护者在 main 上以组织为目标先预览再应用。依赖受保护的合并或发布前应完成此步骤。

在调用方 CI 中保留 Release readiness 并确认实际运行成功。如果没有对应的工作流，必需检查会一直等待。准备就绪后，从经过审查的稳定代码创建 main 和 release。这两条分支在创建前就已纳入规则适用范围。

保留模板中经过审查且不可变的中央工作流引用。通过同一 PR 一起更新发布检查与软件包发布流程。私有仓库需单独验证套餐及保护设置。自动发现不会授予 Azure 访问权。外部平台机密始终保留在各作者的环境中。

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
