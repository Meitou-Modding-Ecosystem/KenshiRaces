<a id="permanent-branch-policy-and-repository-onboarding"></a>
# 永久分支政策和存储库加入

所有 Meitou -Modding-Ecosystem 存储库都使用 [中央永久策略](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md) ：

| 分支 | 要求 |
| --- | --- |
| 普通分支机构 | 无保护 |
| 贝塔 | PR + 传递 CI ；超级维护者可能会绕过审查，包括自我合并 |
| 主要/发布 | PR +通过 CI +一名独立超级维护者认可；没有审核绕过 |

GitHub Actions 没有审核绕过。 Beta 的团队例外仅适用于审核：强制 PR / CI 安全无旁路。 CODEOWNERS 不增加额外的批准要求。

GitHub 模板复制源代码，而不是分支规则集、团队权限、环境或机密。 GutenStructure 的配置发布分支保护每小时在第 17 分钟发现新的公共组织存储库，分配团队存储库写入权限（如果丢失），并应用策略。要立即配置新的存储库，请要求授权的 GutenStructure 维护人员预览，然后将该工作流程应用于主要目标组织。在依赖受保护的合并或发布之前执行此操作。将 Release readiness 保留在调用者 CI 中并验证实际的传递运行；没有匹配工作流程的预期检查将永远等待。

准备好后，从经过审查的稳定源创建主/版本；政策甚至在它们存在之前就针对它们。保持此模板的不可变的已审核中央工作流程引脚，通过 PR 一起更新发布门和包发布者。私人回购需要单独的计划/保护验证；发现不会授予任何 Azure 访问权限。外部平台机密保留在作者拥有的环境中，并且永远不会从此模板复制。
