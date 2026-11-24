<a id="animations"></a>
# 애니메이션

Kenshi 레이스 선박은 애니메이션 도서관과 비디오 : 레이스의 출처 모델의 각 클립 (Meshy애니메이션 라이브러리, 59 클립), 게임의 남성과 여성의 스케일에 대한 리트리그, 그래서 그 스케일의 모든 캐릭터를 재생할 수 있습니다, 인간, 셰이크, 겨울과 스케일뿐만 아니라 장착 경주. 모드는 그들을 통해 Kenshi 레이스 > API ([depending-on-kenshi-races.md](depending-on-kenshi-races.md#6-play-its-animations)).

<a id="files-and-records"></a>
## 파일 및 기록

- `KenshiRaces/items/animations/kenshiraces_anims_male.skeleton` 그리고 `_female.skeleton`: 각각 자신의 게임 척추의 뼈 바이트로 시작
바이트 (아니메이션 파일의 흔적은 뼈 손잡이에 의해 적용되며, 여성의 손잡이가 다릅니다), 그 다음 클립, 30 키.
- ANIMATION_FILE `40-KenshiRaces.mod` (`AnimationRecords.File`둘 다 이름을 쓴다. 데이터 단계는 그것을 연결하여 `animation files` 모든 경주에서
누구의 `male mesh` 아래에 있는 `data/character/meshes/` (그러나 그것은 그들의 몸을 잃어버리거나, 그들의 몸은 그들의 척추를 가지고 있기 때문에)`/items/races/`) 그리고 `RaceRecords.FillRace` 그것은 새로운 부족으로 연결되어 있으며, 중독자도 있습니다.
- 장애가 있는 사람 COMBAT_TECHNIQUE 클립에 의해 (`100-` 는 는 `158-KenshiRaces.mod`이름을 지정하는 경우, `anim name` = 클립 : 장애, 그래서 싸움은 그것을 픽업하지 않습니다;
모드는 모드 전투 움직임으로 연주됩니다 (엔진 docs/modding.md‘투쟁이 움직인다’는 말이다.

<a id="making-them"></a>
## 그들을 만드는

`tools/AnimationExport` A와 함께 [움직임 Lab](https://github.com/Meitou-Modding-Ecosystem/PZAKenshiMotionLab) 클로니아 (`MOTIONLAB`가짜 `../PZAKenshiMotionLab`그리고 그 게임의 `data/character/meshes`:

1. `python tools/AnimationExport/split.py <model.glb> <clips folder>` 출처 모델: 클립 당 1개의 참조 `Bip001` 재명명된 멤버들
Mixamo의 역할은 Motion Lab이 읽는 역할입니다. 나중에 모델이 한 클립을 과도하게 썼다 이전에 한 선물; 남성 LeomaS는 마지막으로 나온다.
2. 성별, `python -m motionlab rig human_<gender>.mesh --skeleton <gender>_skeleton.skeleton -o <out> --clips idle --reference
<clip>.glb ...` (모든 클립) : 게임의 몸에 그들을 뒤로 옮깁니다 (약 1 분 동안 59).
3. 피아노 tools/AnimationExport/export.py <out>/human_<gender>_rigged.glb <성>_skeleton.skeleton
KenshiRaces/items/animations/kenshiraces_anims_<성>.스켈트 <클립>.json`: names them `kr_<clip lower case>` 그리고 파일을 작성합니다.

각 클립은 출처 클립 (여행 열, 미터)이있는 곳에서, 뿌리의 지상 여행은 직선으로 옮겨집니다. 모든 게이즈가 게임의 앞쪽에 나타납니다 : 뿌리의 야우가 -90°의 묶음 포즈와 일치하며 게임의 자신의 아이들처럼 (**확인된**, `idle_stand_normal` 안에 있는 `male_skeleton.skeleton`). 새로운 클립은 또한 그 라인을 필요로합니다. `RaceAnimations`다음 무료 기술 ID (159 위), 그리고 그것의 순서 여기.

<a id="clips"></a>
## 클립

| 클립 | `RaceAnimations.` | 기술적 | 출처 | 2초 | 여행 제거 (m) |
| --- | --- | --- | --- | --- | --- |
| `kr_360_power_spin_jump` | `PowerSpinJump360` | `100-` | _360_Power_Spin_Jump에 대한 자세한 내용 | 3.00 | 2.5 |
| `kr_alert` | `Alert` | `101-` | 경고 | 4.00 |  |
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
| `kr_block1` | `Block1` | `114-` | 블록 1 | 3.47 |  |
| `kr_block2` | `Block2` | `115-` | 블록 2 | 1.60 |  |
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
| `kr_counterstrike` | `Counterstrike` | `128-` | 대항공격 | 6.50 |  |
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
| `kr_running` | `Running` | `139-` | 달리기 | 0.60 |  |
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
| `kr_walking` | `Walking` | `154-` | 걷기 | 1.00 |  |
| `kr_walking_2_inplace` | `Walking2Inplace` | `155-` | walking_2_inplace | 1.17 |  |
| `kr_walking_woman` | `WalkingWoman` | `156-` | Walking_Woman | 0.97 |  |
| `kr_wall_push_jump_and_flip` | `WallPushJumpAndFlip` | `157-` | Wall_Push_Jump_and_Flip | 2.00 | 1.3 |
| `kr_wave_for_help_3` | `WaveForHelp3` | `158-` | Wave_for_Help_3 | 3.17 |  |
