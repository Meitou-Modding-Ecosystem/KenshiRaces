<a id="agents-working-across-repos"></a>
# 複数のリポジトリにまたがる変更の進め方

変更が触れるすべてのリポジトリを 1 人のエージェントに与え、どのリポジトリが各部分を所有しているかを伝え、依存関係の順序 (エンジン、フレームワーク、MOD) で PR を配置します。リポジトリごとに 1 つではなく、変更ごとに 1 つのエージェント セッション。

<a id="give-the-agent-the-repos"></a>
## エージェントにリポジトリを渡します

- **クラウド セッション:**各リポジトリはセッションまたはプロジェクトにアタッチする必要があります。エージェントはそれらを並べてクローンし、それぞれの独自のブランチで動作します。
- **あなたの PC (リモート コントロール):**[local-development.md](local-development.md).エージェントは、 `gh repo create` など、クラウド セッションではできないことも実行できます。

<a id="where-each-part-goes"></a>
## 各パーツの行き先

| 変化 | リポ | PR に |
| --- | --- | --- |
| エンジンフック (イベント、保存されたフィールド、コマンド) | MeitouClient | `fork` |
| ライバルコア API または動作 | ライバルコア | `beta` |
| ロジスティックスコア (ノード、エッジ、所有者、在庫、トランスポート) | 物流コア | `beta` |
| 戦争オーケストレーション (兵站に関する戦闘シナリオ、ストーリーやキャラクターなし) | 戦争組織化 | `beta` |
| 靭皮前線の近接攻撃 | バストフロント | `beta` |
| MODのコード、データジェネレーター、テスト、ドキュメント | そのMODのリポジトリ | `beta` |
| CI 共用、パッケージング、リリースゲート、ロックターゲット | GutenStructure | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## リポジトリをまたぐ変更のオーダー

1. エンジン PR を `fork` に統合します。エンジン ピンはアップストリームのリリース タグにのみ移動するため、それを必要とするフレームワークと MOD PR は、アップストリーム (VlcekM/ MeitouClient) が変更をリリースするまで待機します。それまでは、エンジン ブランチをローカルでビルドします ([local-development.md](local-development.md#change-the-engine-and-a-mod-together) ])。
2. 変更を行う (および `engine` ピンを必要なものがあるリリースに移動する) フレームワーク PR は、 `beta` にマージされます。 CI はベータ版のプレリリースを公開します。
3. `meitou.lock.json` をそのプレリリース (および `engine` をロックのエンジン コミット) に移動する Mod PR は、 `beta` にマージされます。
4. フレームワークのリリースがプロモートされてタグ付けされた後、ロックを `<framework>-v<version>` に移動する mod PR 。

それぞれ PR その前のものをリンクします。壊れる枠組み API 変更によりフレームワークのマイナー バージョンが変更され、すべての依存リポジトリがリストされます。各依存関係は独自のロックを取得します PR.

チェーンでは、ステップ 2 がレイヤーごとにトップダウンで 1 回繰り返されます。ロジスティックス コアの変更が着地し、次にウォー オーケストレーション PR がロジスティックスが構築して独自に公開したものをロックし、次にバスト フロント PR が両方をロックします。

<a id="rules-to-paste-into-the-agents-instructions"></a>
## エージェントの指示に貼り付けるルール

```markdown
- Repos and owners: engine hooks -> MeitouClient (PR into fork); Rivalry Core -> RivalryCore (beta);
  Logistics Core -> LogisticsCore (beta); War Orchestration -> WarOrchestration (beta); Bast Front -> BastFront (beta);
  mods -> their own repo (beta); shared CI/packaging -> GutenStructure (beta).
- Layers: logistics.core -> war.orchestration -> Bast Front. A layer uses the one above only through its public API and
  a dependencies range. War Orchestration holds no story or character content.
- Land PRs in order: engine, then each framework layer top-down, then the mod. One PR per repo per step. Self-merge each
  when CI is green; link the previous PR.
- A meitou.lock.json lists every framework in the chain (Bast Front locks War Orchestration AND Logistics Core), all on
  one engine commit, each accepted by the others' dependency ranges.
- To build against unmerged framework changes, set MeitouFrameworkSources (the sibling clones' framework .csproj paths, ';'-separated)
  and MeitouEngineDir in the repo's git-ignored Meitou.user.props. Never commit Meitou.user.props.
- A lock points only at an immutable tag: <framework>-<version>-beta.<commit> or <framework>-v<version>, with the
  asset, SHA-256 and engine commit copied from that release's notes. Never the rolling <framework>-beta tag.
- Never commit inside an engine/ submodule. Pin engine/ to an upstream Meitou release tag (vX.Y.Z of VlcekM/MeitouClient), or
  with a lock, to the lock's engine commit. Never pin the fork's fork head or a nightly. Move Directory.Build.rsp and every
  gameVersion floor together, only when the mod needs the newer release (GutenStructure docs/versioning.md).
- After pulling, run tools/Update-Local.ps1 so local builds use the pinned engine.
- Don't merge a PR whose lock names a framework build that isn't published yet.
- Ask before release tags, promotions to main or release, and anything in another author's folder or repo.
- Report every PR link, in landing order.
```

<a id="prompt-template"></a>
## プロンプトテンプレート

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```

[English](../../../docs/agents-across-repos.md) · [Kenshi Races](../README.md)
