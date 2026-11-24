<a id="animations"></a>
# 애니메이션

Kenshi Races 는 애니메이션 라이브러리를 제공합니다. 종족의 소스 모델( Meshy 의 애니메이션 라이브러리, 59개 클립)의 모든 클립은 게임의 남성 및 여성 스켈레톤에 대상이 변경되므로 해당 스켈레톤의 모든 캐릭터, 인간, 셰크, 하이브 및 스켈레톤은 물론 장착된 종족까지 재생할 수 있습니다. 모드에서는 Kenshi Races ' API ([dependent-on-kenshi-races.md](depending-on-kenshi-races.md#6-play-its-animations) )를 통해 재생합니다.

<a id="files-and-records"></a>
## 파일 및 기록

- `KenshiRaces/items/animations/kenshiraces_anims_male.skeleton` 및 `_female.skeleton` : 각각 게임 스켈레톤의 뼈대 바이트로 시작합니다(애니메이션 파일의 트랙은 뼈대 핸들에 의해 적용되며 여성 리그의 핸들은 다름). 그런 다음 클립(초당 30개 키).
- ANIMATION_FILE  `40-KenshiRaces.mod` ( `AnimationRecords.File` )는 둘 다 이름을 지정합니다. 데이터 단계에서는 `male mesh` 가 `data/character/meshes/` 아래에 있거나(자체 골격이 있는 하이브 여왕 제외) 또는 장착된 신체( `/items/races/` )인 모든 종족의 `animation files` 에 연결하고, `RaceRecords.FillRace` 는 이를 종속 종족인 새로운 하위 종족에도 연결합니다.
- 클립당 비활성화된 COMBAT_TECHNIQUE ( `100-` ~ `158-KenshiRaces.mod` , 이름 순서), `anim name` = 클립: 비활성화되었으므로 전투에서 이를 선택하지 않습니다. 모드는 이를 모드 전투 동작으로 재생합니다(engine docs/modding.md, "전투 동작").

<a id="making-them"></a>
## 만들기

[ Motion Lab ](https://github.com/Meitou-Modding-Ecosystem/PZAKenshiMotionLab) 클론( `MOTIONLAB` , 기본값 `../PZAKenshiMotionLab` ) 및 게임의 `data/character/meshes` 가 있는 `tools/AnimationExport` :

1. 소스 모델당 `python tools/AnimationExport/split.py <model.glb> <clips folder>` : 클립당 하나의 참조, `Bip001` 조인트는 Motion Lab 가 읽는 Mixamo 역할로 이름이 변경되었습니다. 이후 모델은 이전 모델이 제공한 클립을 덮어씁니다. 수컷 Leoma 가 마지막에 갑니다.
2. 성별에 따라 `python -m motionlab rig human_<gender>.mesh --skeleton <gender>_skeleton.skeleton -o <out> --clips idle --reference <clip>.glb ...` (모든 클립): 게임 본문에 대상을 변경합니다(59개의 경우 약 1분).
3. `python tools/AnimationExport/export.py <out>/human_<gender>_rigged.glb <gender>_skeleton.skeleton KenshiRaces/items/animations/kenshiraces_anims_<gender>.skeleton <clips>.json` : `kr_<clip lower case>` 라는 이름을 지정하고 파일을 씁니다.

모든 클립은 제자리에서 재생됩니다. 소스 클립이 이동한 곳(여행 열, 미터)에서 루트의 지면 이동은 직선으로 가져와 흔들리고 밥을 유지합니다. 모두 게임의 앞쪽을 향합니다. 루트의 요는 게임 자체의 유휴 상태와 마찬가지로 바인드 포즈의 -90°와 일치합니다(**인증됨**, `male_skeleton.skeleton` 의 `idle_stand_normal` ). 새 클립에는 다음 무료 기술 ID(159 켜짐)가 있는 `RaceAnimations` 의 라인과 여기에 해당 행이 필요합니다.

<a id="clips"></a>
## 클립

| 클립 | `RaceAnimations.` | 기술 | 원천 | 초 | 제거된 이동량(m) |
| --- | --- | --- | --- | --- | --- |
| `kr_360_power_spin_jump` | `PowerSpinJump360` | `100-` | _360_Power_Spin_Jump | 3.00 | 2.5 |
| `kr_alert` | `Alert` | `101-` | Alert | 4.00 |  |
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
| `kr_block1` | `Block1` | `114-` | Block1 | 3.47 |  |
| `kr_block2` | `Block2` | `115-` | Block2 | 1.60 |  |
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
| `kr_counterstrike` | `Counterstrike` | `128-` | Counterstrike | 6.50 |  |
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
| `kr_running` | `Running` | `139-` | Running | 0.60 |  |
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
| `kr_walking` | `Walking` | `154-` | Walking | 1.00 |  |
| `kr_walking_2_inplace` | `Walking2Inplace` | `155-` | walking_2_inplace | 1.17 |  |
| `kr_walking_woman` | `WalkingWoman` | `156-` | Walking_Woman | 0.97 |  |
| `kr_wall_push_jump_and_flip` | `WallPushJumpAndFlip` | `157-` | Wall_Push_Jump_and_Flip | 2.00 | 1.3 |
| `kr_wave_for_help_3` | `WaveForHelp3` | `158-` | Wave_for_Help_3 | 3.17 |  |
