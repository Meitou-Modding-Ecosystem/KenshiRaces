<a id="animations"></a>
# アニメーション

Kenshi レースはアニメーション図書館を送り、レースのソースモデルのすべてのクリップ(Meshyアニメーションライブラリ、59クリップ)は、ゲームの男性と女性のスケールにリターゲットされ、それらのスケールのすべてのキャラクターは、人間、シェック、冬、スケール、および装備のレースをプレイすることができます。 モットーが遊ぶ Kenshi レース API ([depending-on-kenshi-races.md](depending-on-kenshi-races.md#6-play-its-animations)).

<a id="files-and-records"></a>
## ファイルと記録

- `KenshiRaces/items/animations/kenshiraces_anims_male.skeleton` そして、 `_female.skeleton`: 各がゲームの骨のビートで始まります
ビート(アニメーションファイルのトラックは骨のハンドルによって適用され、女性のハンドルの違い)は、その後、クリップは、30キーを秒で使用します。
- ANIMATION_FILE `40-KenshiRaces.mod` (`AnimationRecords.File`二人の名前。 データの段階は、それを結びつける。 `animation files` 各レース
誰の `male mesh` 下にあり `data/character/meshes/` 主人公(主人公)は、自分(じぶん)の身(み)を身(むか)め、`/items/races/`)と、 `RaceRecords.FillRace` 新たなサブレースに結びついて、依存者にも。
- 障害者 COMBAT_TECHNIQUE クリップ(`100-` へへへへ `158-KenshiRaces.mod`名称の命令、 `anim name` = クリップ:障害者ですので、戦いはそれをピックしない。
モードはモード戦闘動作として演奏する(エンジン) docs/modding.md「闘争が動く」

<a id="making-them"></a>
## 作る

`tools/AnimationExport` A とともに [動作ラボ](https://github.com/Meitou-Modding-Ecosystem/PZAKenshiMotionLab) クローン(`MOTIONLAB`デフォルト `../PZAKenshiMotionLab`)とゲームの `data/character/meshes`:

1. `python tools/AnimationExport/split.py <model.glb> <clips folder>` 原題:One Reference per Clip `Bip001` 改名の関連
ミクサモの役割は、Motion Labが読む。 後期モデルは、以前のモデルに与えられたクリップを過小評価する。 LeomaSは最後です。
2. 性別によって、 `python -m motionlab rig human_<gender>.mesh --skeleton <gender>_skeleton.skeleton -o <out> --clips idle --reference
<clip>.glb ...` (各クリップ):それらをゲームの体(約59分)にリターゲットします。
3. パイソン tools/AnimationExport/export.py <out>/human_<gender>_rigged.glb <Gender>_skeleton.skeleton
KenshiRaces/items/animations/kenshiraces_anims_<セックス>.スケルトン <クリップ>.json`: names them `kr_<clip lower case>` とファイルを書く。

すべてのクリップは、ソースクリップ(旅行コラム、メートル)が移動した場所で、直線として取り出されます。 ゲームの前向きに、すべてのスウェーデン: 根のヤウは、ゲームのアイドルが行うように、結合ポーズの -90°と一致します(**検証**, `idle_stand_normal` で、 `male_skeleton.skeleton`). 新たなキャラクターは、そのラインも必要です。 `RaceAnimations`次の無料テクニカルID(159上)とその順番はこちら。

<a id="clips"></a>
## クリップ

| クリップ | `RaceAnimations.` | テクニック | 源泉 | 秒間 | 旅行移行(m) |
| --- | --- | --- | --- | --- | --- |
| `kr_360_power_spin_jump` | `PowerSpinJump360` | `100-` | _360_パワー_スピン_ジャンプ | 3.00 | 2.5 |
| `kr_alert` | `Alert` | `101-` | 警告 | 4.00 |  |
| `kr_all_night_dance` | `AllNightDance` | `102-` | All_Night_Dance | 8.17 |  |
| `kr_angry_ground_stomp` | `AngryGroundStomp` | `103-` | Angry_Ground_Stomp | 1.37 |  |
| `kr_angry_ground_stomp_2` | `AngryGroundStomp2` | `104-` | Angry_Ground_Stomp_2 | 1.77 |  |
| `kr_archery_shot_1` | `ArcheryShot1` | `105-` | Archery_Shot_1 | 1.00 |  |
| `kr_axe_spin_attack` | `AxeSpinAttack` | `106-` | Axe_Spin_Attack | 2.50 |  |
| `kr_axe_stance` | `AxeStance` | `107-` | Axe_Stance | 6.07 | 0.6 |
| `kr_back_jump` | `BackJump` | `108-` | Back_Jump | 0.90 | 2.2 |
| `kr_ball_throw` | `BallThrow` | `109-` | Ball_throw | 2.97 | 0.1 |
| `kr_baseball_pitching` | `BaseballPitching` | `110-` | baseball_pitching | 3.97 |  |
| `kr_bicep_curl` | `BicepCurl` | `111-` | bicep_curl | 5.87 |  |
| `kr_bicycle_crunch` | `BicycleCrunch` | `112-` | bicycle_crunch | 1.10 |  |
| `kr_big_heart_gesture` | `BigHeartGesture` | `113-` | Big_Heart_Gesture | 6.17 |  |
| `kr_block1` | `Block1` | `114-` | ブロック1 | 3.47 |  |
| `kr_block2` | `Block2` | `115-` | ブロック2 | 1.60 |  |
| `kr_breakdance_1990` | `Breakdance1990` | `116-` | Breakdance_1990 | 0.50 |  |
| `kr_carry_heavy_object_walk_inplace` | `CarryHeavyObjectWalkInplace` | `117-` | Carry_Heavy_Object_Walk_inplace | 6.50 |  |
| `kr_casual_walk` | `CasualWalk` | `118-` | Casual_Walk | 4.17 |  |
| `kr_charged_spell_cast_1` | `ChargedSpellCast1` | `119-` | Charged_Spell_Cast_1 | 4.30 |  |
| `kr_circle_crunch` | `CircleCrunch` | `120-` | circle_crunch | 2.30 |  |
| `kr_climb_attempt_and_fall_4` | `ClimbAttemptAndFall4` | `121-` | Climb_Attempt_and_Fall_4 | 0.57 |  |
| `kr_climb_attempt_and_fall_5` | `ClimbAttemptAndFall5` | `122-` | Climb_Attempt_and_Fall_5 | 1.17 | 0.1 |
| `kr_climb_right_with_both_limbs` | `ClimbRightWithBothLimbs` | `123-` | Climb_Right_with_Both_Limbs | 1.17 | 0.5 |
| `kr_climb_right_with_both_limbs_inplace` | `ClimbRightWithBothLimbsInplace` | `124-` | Climb_Right_with_Both_Limbs_inplace | 1.17 |  |
| `kr_climb_stairs` | `ClimbStairs` | `125-` | Climb_Stairs | 2.90 | 0.4 |
| `kr_climb_up_rope` | `ClimbUpRope` | `126-` | Climb_Up_Rope | 2.57 | 0.2 |
| `kr_confident_strut` | `ConfidentStrut` | `127-` | Confident_Strut | 2.67 |  |
| `kr_counterstrike` | `Counterstrike` | `128-` | コントストライキ | 6.50 |  |
| `kr_fast_lightning` | `FastLightning` | `129-` | Fast_Lightning | 10.80 |  |
| `kr_female_bend_over_pick_up_inspect` | `FemaleBendOverPickUpInspect` | `130-` | Female_Bend_Over_Pick_Up_Inspect | 3.80 |  |
| `kr_flirty_strut_inplace` | `FlirtyStrutInplace` | `131-` | Flirty_Strut_inplace | 3.20 |  |
| `kr_funky_walk` | `FunkyWalk` | `132-` | Funky_Walk | 1.87 | 2.5 |
| `kr_gentlemans_bow` | `GentlemansBow` | `133-` | Gentlemans_Bow | 7.27 |  |
| `kr_jazz_hands_inplace` | `JazzHandsInplace` | `134-` | Jazz_Hands_inplace | 2.70 |  |
| `kr_jumping_jacks` | `JumpingJacks` | `135-` | jumping_jacks | 1.10 |  |
| `kr_kung_fu_punch` | `KungFuPunch` | `136-` | Kung_Fu_Punch | 7.30 | 1.6 |
| `kr_monster_walk` | `MonsterWalk` | `137-` | Monster_Walk | 4.07 |  |
| `kr_prone_reload` | `ProneReload` | `138-` | Prone_Reload | 6.40 |  |
| `kr_running` | `Running` | `139-` | 走る | 0.60 |  |
| `kr_sit_cross_legged_on_floor` | `SitCrossLeggedOnFloor` | `140-` | Sit_Cross_Legged_on_Floor | 2.50 |  |
| `kr_stand_and_chat` | `StandAndChat` | `141-` | Stand_and_Chat | 5.17 |  |
| `kr_stand_and_drink` | `StandAndDrink` | `142-` | Stand_and_Drink | 8.87 |  |
| `kr_stand_on_pole_and_balance` | `StandOnPoleAndBalance` | `143-` | Stand_on_Pole_and_Balance | 1.80 |  |
| `kr_stand_to_side_lying` | `StandToSideLying` | `144-` | Stand_To_Side_Lying | 9.67 | 1.0 |
| `kr_stand_to_sit_transition_m` | `StandToSitTransitionM` | `145-` | Stand_to_Sit_Transition_M | 4.73 | 0.4 |
| `kr_step_to_sit_transition` | `StepToSitTransition` | `146-` | Step_to_Sit_Transition | 5.67 | 1.4 |
| `kr_swim_forward` | `SwimForward` | `147-` | Swim_Forward | 4.50 | 2.1 |
| `kr_swim_idle` | `SwimIdle` | `148-` | Swim_Idle | 3.00 |  |
| `kr_sword_shout` | `SwordShout` | `149-` | Sword_Shout | 6.17 | 0.6 |
| `kr_talk_with_hands_open` | `TalkWithHandsOpen` | `150-` | Talk_with_Hands_Open | 3.97 |  |
| `kr_texting_walk` | `TextingWalk` | `151-` | Texting_Walk | 2.00 |  |
| `kr_victory_fist_pump` | `VictoryFistPump` | `152-` | Victory_Fist_Pump | 1.50 |  |
| `kr_walk_backward_with_gun_inplace` | `WalkBackwardWithGunInplace` | `153-` | Walk_Backward_with_Gun_inplace | 1.00 |  |
| `kr_walking` | `Walking` | `154-` | 歩く | 1.00 |  |
| `kr_walking_2_inplace` | `Walking2Inplace` | `155-` | walking_2_inplace | 1.17 |  |
| `kr_walking_woman` | `WalkingWoman` | `156-` | Walking_Woman | 0.97 |  |
| `kr_wall_push_jump_and_flip` | `WallPushJumpAndFlip` | `157-` | Wall_Push_Jump_and_Flip | 2.00 | 1.3 |
| `kr_wave_for_help_3` | `WaveForHelp3` | `158-` | Wave_for_Help_3 | 3.17 |  |
