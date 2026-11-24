# Kenshi Races

[English](../../README.md) · [Français](../fr/README.md) · [Deutsch](../de/README.md) · [Español](../es/README.md) · [Italiano](../it/README.md) · [Português do Brasil](../pt-BR/README.md) · [Русский](../ru/README.md) · 日本語 · [한국어](../ko/README.md) · [简体中文](../zh-CN/README.md)

Kenshi Racesは[MeitouClient](https://github.com/Meitou-Modding-Ecosystem/MeitouClient)のキャラクターエディターに3種のプレイアブル種族を追加します。LeomaとCrevanには男女のボディがあり、Crabfolkには`crabman`と`crabqueen`の2種類があります。いずれもゲームの人間用スケルトンを使用します。同じスケルトンを使うすべての種族で利用できる59本のアニメーションと、ほかのMODが独自の種族や亜種を追加するためのフレームワークも含まれています。 GPL-3.0-or-later.

## ガイド

- [種族とレコード、ゲーム内でのテスト](docs/kenshi-races.md)
- [別のMODでフレームワークを使い、亜種を追加する](docs/depending-on-kenshi-races.md)
- [59本のアニメーションとその制作手順](docs/animations.md)
- [GLB形式のボディをゲームのスケルトンに合わせる](docs/race-import.md)
- [Visual Studio、VS Code、ターミナルからビルドして起動する](docs/play.md)
- [ローカルリポジトリのセットアップと更新](docs/local-development.md)
- [複数のリポジトリにまたがる変更の進め方](docs/agents-across-repos.md)
- [Meitouの新しいリリースとの互換性を確認する](docs/refcheck.md)
- [Steamワークショップに公開する](docs/steam-workshop.md)
- [FCSを使わずにGLBモデルを建築物として取り込む](docs/structures.md)
- [ポリゴン数、寸法、テクスチャを調整する](docs/model-prep.md)
- [ブランチ保護とレビューのルール](docs/branch-policy.md)
- [リポジトリのルールとエージェント用の手順](../../CLAUDE.md) · [AGENTS.md](../../AGENTS.md) · [kenshi-races-dependent](../../.claude/skills/kenshi-races-dependent/SKILL.md) · [meitou-engine-release](../../.claude/skills/meitou-engine-release/SKILL.md)

<a id="numbers"></a>
## 共有ID

タスク種別と会話コードの番号は、読み込まれたすべてのMODで共有します。Kenshi Racesは番号を予約していません。 (`AddGoalType`, `AddJobType`, `AddDialogueCondition`, `AddDialogueEffect`). [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/numbers.md)

<a id="layout"></a>
## リポジトリの構成

- `engine/` — ビルド対象のコミットに固定したMeitouClientサブモジュールです。プルするたびに下のコマンドで更新してください。 [Update-Local.ps1](docs/local-development.md#keep-clones-current)

  `powershell -ExecutionPolicy Bypass -File tools\Update-Local.ps1 -Clean -Build`

- `KenshiRaces/`, `meitou.json`, `KenshiRaces/items/races/`, `KenshiRaces.Tests/` — MOD本体とマニフェストです。適合済みのボディはリポジトリに含まれています。テストには合成データを使うため、ゲームのインストールは不要です。
- `<Mod>.Data/` → `<Mod>.mod`, `<Mod>/Structures/` — 任意のプロジェクトで、インストール済みのゲームからMODデータを生成したり、配置時にGLBモデルを建築物として取り込んだりできます。 [StructureImport](docs/structures.md)
- `meitou.lock.json` — フレームワークのバージョンを固定します。更新ワークフローが新しい版へ移行し、CIの成功後にマージします。 [Framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) · [Framework update](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-update.md)
- `Play/` — ワンクリックでビルド、配置、起動します。
- `tools/` — 同期、エンジン参照の検査、建築物のインポート、ボディの適合、テクスチャの調整に使うツールです。 [Update-Local.ps1](docs/local-development.md#keep-clones-current) · [refcheck.sh / RefCheck](docs/refcheck.md) · [StructureImport](docs/structures.md) · [RaceImport](docs/race-import.md) · [model-info.py / prepare-model.py](docs/model-prep.md)
- `Directory.Build.props` — エンジンのビルド設定を取り込みます。コンパイラの警告はエラーとして扱われます。
- `Directory.Build.rsp` — エンジン参照のバージョンをサポート対象の最も古い版に合わせます。実行中のエンジンより新しい版を参照するMODは読み込めません。サブモジュールには既定のアセンブリバージョンが使われます。 (`gameVersion`, `1.0.0.0`).  Meitouの新しいリリースとの互換性は毎日検証されます。 [Versioning](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/versioning.md) · [Meitou compatibility](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/meitou-compat.md)

<a id="branches"></a>
## ブランチ

PRの宛先はbetaです。ビルドとテストの成功が必要です。ゲーム内で確認後、betaからmainへ昇格できます。製品版はrelease宛てのPRで公開します。releaseへのマージ時、バージョンタグが未作成ならタグを付けてZIPを公開します。手動でのタグ作成には予備のスクリプトを使えます。 (`<slug>-v<version>`, `.github/workflows/release.yml`, `tools\Tag-Release.ps1`). [Beta / main / release](docs/branch-policy.md)
