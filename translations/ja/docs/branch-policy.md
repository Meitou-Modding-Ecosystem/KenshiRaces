# ブランチ保護と新規リポジトリの設定

組織内の全リポジトリに共通の恒久ポリシーが適用されます。 [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| ブランチ | 要件 |
| --- | --- |
| 通常の作業ブランチ | 保護なし。 |
| `beta` | PRとCIの成功が必須です。super-maintainersは自分のPRをマージする場合もレビューを省略できます。 |
| `main` / `release` | PR、CIの成功、独立したsuper-maintainer 1名の承認が必要です。レビューの免除はありません。 |

GitHub Actionsにはレビュー免除がありません。betaの例外はチームのレビュー要件に限られます。PRと必須CIは常に必要です。CODEOWNERSによる追加承認は要求されません。

GitHubテンプレートがコピーするのはソースです。ルールセット、チーム権限、環境、シークレットは別途設定します。毎時17分に中央ワークフローが新しい公開リポジトリを検出し、必要に応じてチームに書き込み権限を与え、ポリシーを適用します。すぐに設定するには、権限のあるGutenStructure (`Configure release branch protections`)メンテナーがmain上で組織を対象にプレビューを実行し、その後適用します。保護付きマージの利用や公開の前に済ませます。

呼び出し元のCIにRelease readinessを残し、実際の実行が成功したことを確認してください。対応するワークフローがなければ、要求されたチェックは待機したままです。レビュー済みの安定したコードからmainとreleaseを作成します。ルールは作成前から両ブランチを対象にしています。

中央ワークフローを指すレビュー済みの固定参照を維持します。リリース判定とパッケージ公開の参照はPRで一緒に更新します。非公開リポジトリではプランと保護設定を別途検証します。検出時にAzureアクセスは付与されません。外部プラットフォームのシークレットは各作者の環境にのみ保存されます。

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
