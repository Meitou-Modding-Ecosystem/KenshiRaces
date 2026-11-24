<a id="permanent-branch-policy-and-repository-onboarding"></a>
# 永久分支政策和存储库的登机

所有 Meitou-Modding-Ecosystem 存储库使用 [中央永久政策](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md):

| 分支 | 要求 |
| --- | --- |
| 常规分支 | 不受保护 |
| 贝塔 | PR + 通过 CI; 超级维持人可以通过评论,包括自我调整 |
| 主 / 释放 | PR + 通过 CI 加以一个独立的超级维持人批准; 没有评论过渡 |

GitHub 活动没有评论过渡。 Beta 的团队例外仅适用于评论:强制性 PR/CI 安全没有过渡。 CODEOWNERS 没有额外的批准要求。

GitHub 模板复制来源,而不是分支规则,团队许可,环境或秘密。 GutenStructure的配置分支保护,每小时在17分钟内发现新的机关存储器,如果缺乏,则将其分配到团队存储器中。 要立即设置一个新功能,请授权的GutenStructure维护器预览,然后将该工作流应用于主要的目标组织。 在依靠保护合并或出版之前,做这件事。 保持在呼叫器的准备,并检查实际的通行;没有匹配的工作流的预期将永远等待。

创建 main/release 从审查的稳定来源,当准备好;政策针对它们之前,它们存在。 保持这个模板的不可改变的中央工作流,更新发布门和包装出版商通过PR。 个人休息需要分开 plan/protection 验证; 未经发现的 Azure 访问。 外部平台的秘密仍然存在于作者所拥有的环境中,并且从这个模板中永远不会复制。
