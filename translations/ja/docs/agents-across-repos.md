<a id="agents-working-across-repos"></a>
# 休憩中での作業

変換点を1つのエージェントに与え、それぞれの部分を所有し、エンジン、フレーム、モードの順序を保ちます。 変動に1つのエージェントセッションではなく、リポに1つのセッション。

<a id="give-the-agent-the-repos"></a>
## エージェントに休憩

- **クラウドセッション:** すべてのリポは、セッションやプロジェクトに付属する必要があります。 エージェントは彼らを片側にクローンし、自分の分野で働く。
それぞれの
- **PC(リモートコントロール)** クローンは、その中から [local-development.md](local-development.md). 代理人は、何ができるのか。
クラウドセッションは、例えば、 `gh repo create`.

<a id="where-each-part-goes"></a>
## 各部門はどこへ行くのか

| 変更 | リポー | PR入り |
| --- | --- | --- |
| エンジンホーク(イベント、保存フィールド、コマンド) | MeitouClient | `fork` |
| レイアウト API ・行動 | リバリーコア | `beta` |
| ロジスティクスコア(ノード、端、所有者、倉庫、輸送) | ロジスティクスコア | `beta` |
| 戦争・オーケストラ(戦争・ロジスティクス・シナリオ、ストーリー・キャラクターなし) | 戦略 | `beta` |
| フロントメリー | バストフロント | `beta` |
| モードコード、データ生成器、テスト、ドック | あのモードのリポ | `beta` |
| シェア CI、パッケージ、リリースゲート、ロックターゲット | グローバル構造 | `beta` |

<a id="order-for-a-change-that-crosses-repos"></a>
## 休憩を越える変化の命令

1. 引っ越しPR `fork`合併。 エンジンピンはアップストリームリリースタグにのみ移動するので、フレームワークとモードPRはそれを待つ必要があります。
トップページ(VlcekM/MeitouClient変化を解放する。 地元のエンジンが動き出す([local-development.md](local-development.md#change-the-engine-and-a-mod-together)).
2. フレームワークPRが変化を起こす(そしてその動きを動かす) `engine` ピンは、必要なものを持っているリリースに、合併して、 `beta`. 出版
ベータプレリリース
3. 動くPRの動き `meitou.lock.json` 初めに(そして、 `engine` エンジン・ロック(ロック・エンジン)に合併 `beta`.
4. フレームリリースが推進され、タグ付けられた後: ロックを移動するモードPR `<framework>-v<version>`.

すべてのPRはその前に結びついている。 壊れた枠組み API 変更はフレームの小さなバージョンであり、それぞれの依存者が自分のロックPRを得る。

チェーンでは、ステップ2は層ごとに繰り返されます: ロジスティック・コアの変更、その後、戦争・PRは、ロジスの構築と出版の両方を閉じます。

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
## タイムテンプレート

```text
Change: <what the player should see>
Repos: MeitouClient (<engine hook>), RivalryCore (<Rivalry Core change>), AshenHand (<mod change>)
Order: engine -> RivalryCore -> AshenHand, each PR into its integration branch, self-merge when green.
Build against unmerged changes with the local override; the final mod PR pins the published beta prerelease.
Follow the cross-repo rules. Report each PR link in order.
```
