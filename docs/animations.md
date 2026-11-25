# Animations

Kenshi Races ships an animation library: every clip of the races' source models (Meshy's animation library, 188 clips), retargeted onto the
game's male and female skeletons, so any character on those skeletons can play it, humans, shek, hivers and skeletons as well as the fitted
races. Mods play them through Kenshi Races' API ([depending-on-kenshi-races.md](depending-on-kenshi-races.md#6-play-its-animations)).

## Files and records

- `KenshiRaces/items/animations/kenshiraces_anims_male.skeleton` and `_female.skeleton`: each starts with its game skeleton's bones byte for
  byte (an animation file's tracks apply by bone handle, and the female rig's handles differ), then the clips, 30 keys a second.
- ANIMATION_FILE `40-KenshiRaces.mod` (`AnimationRecords.File`) names both. The data phase links it into the `animation files` of every race
  whose `male mesh` is under `data/character/meshes/` (except the Hive Queen's, which has its own skeleton) or is a fitted body
  (`/items/races/`), and `RaceRecords.FillRace` links it into new subraces, a dependent's too.
- A disabled COMBAT_TECHNIQUE per clip (`100-` to `199-` and `400-` to `487-KenshiRaces.mod`: the first models' clips in name order, then
  the spidercrab's, the ghostcrab's, the Batfolk's, the Wolfkin's, then the tieflings', the beepbot's and the Owlkind's; `200-` to `399-`
  are the introducer's and its arrival squads'), `anim name` = the clip: disabled, so no fight picks it;
  a mod plays it as a mod combat move (engine docs/modding.md, "Combat moves").

## Making them

`tools/AnimationExport` with a [Motion Lab](https://github.com/Meitou-Modding-Ecosystem/PZAKenshiMotionLab) clone (`MOTIONLAB`, default
`../PZAKenshiMotionLab`) and the game's `data/character/meshes`:

1. `python tools/AnimationExport/split.py <model.glb> <clips folder>` per source model: one reference per clip, `Bip001` joints renamed to
   the Mixamo roles the Motion Lab reads (a Mixamo rig such as `wolfman-rigged` already has them). A later model overwrites a clip an earlier one gave; the male Leoma's goes last.
   Source models: male Leoma ("Leonid Bare" and "Leonid" glbs), `leonidfemale`, `corvidmale`, `corvidfemale`, crabman, crabqueen and
   `spidercrab`, `ghostcrab`, `batman-rigged`, `batgirl-rigged`, `wolfman-rigged`, `wolfwoman-rigged`, then `tiefling-female`,
   `tieflingmale-rigged`, `beepbot-rigged` and `owlkindfemale-rigged` (clips they share with an earlier model, such as Running and Walking,
   are not taken).
2. Per gender, `python -m motionlab rig human_<gender>.mesh --skeleton <gender>_skeleton.skeleton -o <out> --clips idle --reference
   <clip>.glb ...` (every clip): retargets them onto the game's body (a few minutes for 135).
3. `python tools/AnimationExport/export.py <out>/human_<gender>_rigged.glb <gender>_skeleton.skeleton
   KenshiRaces/items/animations/kenshiraces_anims_<gender>.skeleton <clips>.json`: names them `kr_<clip lower case>` and writes the file.

Every clip plays in place: where a source clip travelled (the Travel column, metres), the root's ground travel is taken out as a straight
line, sway and bob kept. All face the game's forward: the root's yaw matches the bind pose's -90°, as the game's own idles do (**Verified**,
`idle_stand_normal` in `male_skeleton.skeleton`). A new clip also needs its line in `RaceAnimations`, with the next free technique id (488 on), and its row here. New clips can be added
without redoing the rest: retarget only them, export them, and append the export's animation chunks (what follows its copy of the game
skeleton's bones) to the end of each library file.

## Clips

| Clip | `RaceAnimations.` | Technique | Source | Seconds | Travel removed (m) |
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
| `kr_attack` | `Attack` | `159-` | Attack | 2.77 |  |
| `kr_block6` | `Block6` | `160-` | Block6 | 1.67 |  |
| `kr_block9` | `Block9` | `161-` | Block9 | 2.77 |  |
| `kr_boxing_guard_prep_straight_punch` | `BoxingGuardPrepStraightPunch` | `162-` | Boxing_Guard_Prep_Straight_Punch | 3.97 |  |
| `kr_broadsword_left` | `BroadswordLeft` | `163-` | Broadsword___Left | 2.97 | 1.1 |
| `kr_broadsword_right` | `BroadswordRight` | `164-` | Broadsword___Right | 2.97 | 0.2 |
| `kr_broadsword_thrust` | `BroadswordThrust` | `165-` | Broadsword___Thrust | 2.97 | 2.8 |
| `kr_broadsword_top` | `BroadswordTop` | `166-` | Broadsword___Top | 2.97 | 1.0 |
| `kr_broadsword_top_2` | `BroadswordTop2` | `167-` | Broadsword___Top_2 | 2.97 | 0.7 |
| `kr_charged_slash` | `ChargedSlash` | `168-` | Charged_Slash | 2.20 |  |
| `kr_mage_soell_cast_6` | `MageSoellCast6` | `169-` | mage_soell_cast_6 | 2.27 |  |
| `kr_reaping_swing` | `ReapingSwing` | `170-` | Reaping_Swing | 5.90 | 0.4 |
| `kr_right_hand_sword_slash` | `RightHandSwordSlash` | `171-` | Right_Hand_Sword_Slash | 1.50 |  |
| `kr_left_short_hook_from_guard` | `LeftShortHookFromGuard` | `172-` | Left_Short_Hook_from_Guard | 1.27 |  |
| `kr_left_slash` | `LeftSlash` | `173-` | Left_Slash | 3.17 |  |
| `kr_left_uppercut_from_guard` | `LeftUppercutFromGuard` | `174-` | Left_Uppercut_from_Guard | 1.30 |  |
| `kr_right_upper_hook_from_guard` | `RightUpperHookFromGuard` | `175-` | Right_Upper_Hook_from_Guard | 1.70 |  |
| `kr_right_uppercut_from_guard` | `RightUppercutFromGuard` | `176-` | Right_Uppercut_from_Guard | 1.00 |  |
| `kr_walk_backward_inplace` | `WalkBackwardInplace` | `177-` | Walk_Backward_inplace | 0.87 |  |
| `kr_walk_backward_with_grenade_inplace` | `WalkBackwardWithGrenadeInplace` | `178-` | Walk_Backward_with_Grenade_inplace | 1.20 |  |
| `kr_walk_fight_forward` | `WalkFightForward` | `179-` | Walk_Fight_Forward | 1.70 |  |
| `kr_walk_forward_while_shooting` | `WalkForwardWhileShooting` | `180-` | Walk_Forward_While_Shooting | 3.27 |  |
| `kr_walk_forward_with_bow_aimed` | `WalkForwardWithBowAimed` | `181-` | Walk_Forward_with_Bow_Aimed | 1.20 |  |
| `kr_walk_left_with_gun` | `WalkLeftWithGun` | `182-` | Walk_Left_with_Gun | 1.20 | 1.0 |
| `kr_walk_with_umbrella` | `WalkWithUmbrella` | `183-` | Walk_with_Umbrella | 1.30 | 1.0 |
| `kr_walk_with_umbrella_inplace` | `WalkWithUmbrellaInplace` | `184-` | Walk_with_Umbrella_inplace | 1.30 |  |
| `kr_weapon_combo` | `WeaponCombo` | `185-` | Weapon_Combo | 3.67 | 1.2 |
| `kr_weapon_combo_1` | `WeaponCombo1` | `186-` | Weapon_Combo_1 | 3.50 | 2.1 |
| `kr_weapon_combo_2` | `WeaponCombo2` | `187-` | Weapon_Combo_2 | 4.50 | 1.0 |
| `kr_agree_gesture` | `AgreeGesture` | `188-` | Agree_Gesture | 13.00 | 0.2 |
| `kr_air_squat` | `AirSquat` | `189-` | air_squat | 1.87 |  |
| `kr_climb_attempt_and_fall_2` | `ClimbAttemptAndFall2` | `190-` | Climb_Attempt_and_Fall_2 | 0.80 |  |
| `kr_climb_left_with_both_limbs` | `ClimbLeftWithBothLimbs` | `191-` | Climb_Left_with_Both_Limbs | 1.17 | 0.5 |
| `kr_collect_object` | `CollectObject` | `192-` | Collect_Object | 6.00 |  |
| `kr_confused_scratch` | `ConfusedScratch` | `193-` | Confused_Scratch | 11.50 | 0.1 |
| `kr_crouch_and_step_back` | `CrouchAndStepBack` | `194-` | Crouch_and_Step_Back | 2.67 | 2.1 |
| `kr_crouch_pull_and_throw` | `CrouchPullAndThrow` | `195-` | Crouch_Pull_and_Throw | 5.80 | 0.5 |
| `kr_discuss_while_moving` | `DiscussWhileMoving` | `196-` | Discuss_While_Moving | 8.00 | 1.4 |
| `kr_dive_down_and_land` | `DiveDownAndLand` | `197-` | Dive_Down_and_Land | 1.60 | 4.5 |
| `kr_dive_down_and_land_1` | `DiveDownAndLand1` | `198-` | Dive_Down_and_Land_1 | 0.37 | 0.5 |
| `kr_dive_down_and_land_2` | `DiveDownAndLand2` | `199-` | Dive_Down_and_Land_2 | 3.07 | 0.3 |
| `kr_elderly_shaky_walk` | `ElderlyShakyWalk` | `400-` | Elderly_Shaky_Walk | 2.80 | 0.6 |
| `kr_parkour_vault_1` | `ParkourVault1` | `401-` | Parkour_Vault_1 | 1.17 | 3.9 |
| `kr_parkour_vault_2` | `ParkourVault2` | `402-` | Parkour_Vault_2 | 1.17 | 4.7 |
| `kr_parkour_vault_3` | `ParkourVault3` | `403-` | Parkour_Vault_3 | 2.30 | 5.7 |
| `kr_parkour_vault_with_roll` | `ParkourVaultWithRoll` | `404-` | Parkour_Vault_with_Roll | 2.07 | 5.8 |
| `kr_penguin_walk` | `PenguinWalk` | `405-` | penguin_walk | 2.00 |  |
| `kr_personalized_gesture` | `PersonalizedGesture` | `406-` | Personalized_Gesture | 2.00 |  |
| `kr_wave_for_help` | `WaveForHelp` | `407-` | Wave_for_Help | 5.00 |  |
| `kr_wave_for_help_1` | `WaveForHelp1` | `408-` | Wave_for_Help_1 | 0.50 |  |
| `kr_wave_for_help_2` | `WaveForHelp2` | `409-` | Wave_for_Help_2 | 1.50 |  |
| `kr_wave_for_help_4` | `WaveForHelp4` | `410-` | Wave_for_Help_4 | 4.67 |  |
| `kr_wave_one_hand` | `WaveOneHand` | `411-` | Wave_One_Hand | 4.07 |  |
| `kr_riding_yeehaw` | `RidingYeehaw` | `412-` | Riding_Yeehaw | 2.97 | 0.1 |
| `kr_riding_calm` | `RidingCalm` | `413-` | Riding_calm | 2.97 |  |
| `kr_sidelying_reach_help` | `SideLyingReachHelp` | `414-` | SideLying_Reach_Help | 14.67 | 0.1 |
| `kr_sit_dodge` | `SitDodge` | `415-` | Sit_Dodge | 8.87 |  |
| `kr_sit_and_doze_off` | `SitAndDozeOff` | `416-` | Sit_and_Doze_Off | 17.27 |  |
| `kr_sit_and_drink` | `SitAndDrink` | `417-` | Sit_and_Drink | 11.17 |  |
| `kr_sit_on_chair_arms_crossed` | `SitOnChairArmsCrossed` | `418-` | Sit_on_Chair_Arms_Crossed | 2.37 |  |
| `kr_skill_01` | `Skill01` | `419-` | Skill_01 | 1.07 | 0.2 |
| `kr_skill_02` | `Skill02` | `420-` | Skill_02 | 1.27 | 0.2 |
| `kr_skill_03` | `Skill03` | `421-` | Skill_03 | 1.60 | 0.3 |
| `kr_skip_forward` | `SkipForward` | `422-` | Skip_Forward | 2.80 |  |
| `kr_touch_and_run` | `TouchAndRun` | `423-` | Touch_and_Run | 3.17 |  |
| `kr_triple_combo_attack` | `TripleComboAttack` | `424-` | Triple_Combo_Attack | 4.33 | 1.6 |
| `kr_two_handed_parry` | `TwoHandedParry` | `425-` | Two_Handed_Parry | 1.30 |  |
| `kr_unarmed_vault` | `UnarmedVault` | `426-` | Unarmed_Vault | 0.80 | 3.0 |
| `kr_upside_down_rope_hang` | `UpsideDownRopeHang` | `427-` | Upside_Down_Rope_Hang | 2.37 | 0.1 |
| `kr_upside_down_rope_hang_1` | `UpsideDownRopeHang1` | `428-` | Upside_Down_Rope_Hang_1 | 2.73 |  |
| `kr_upside_down_rope_hang_2` | `UpsideDownRopeHang2` | `429-` | Upside_Down_Rope_Hang_2 | 1.50 | 0.1 |
| `kr_vault_and_land` | `VaultAndLand` | `430-` | Vault_and_Land | 3.00 | 1.5 |
| `kr_vault_with_rifle` | `VaultWithRifle` | `431-` | Vault_with_Rifle | 0.80 | 2.6 |
| `kr_victory_cheer` | `VictoryCheer` | `432-` | Victory_Cheer | 9.30 |  |
| `kr_wake_up_and_look_up` | `WakeUpAndLookUp` | `433-` | Wake_Up_and_Look_Up | 3.50 |  |
| `kr_victory` | `Victory` | `434-` | victory | 8.53 |  |
| `kr_archery_aim_with_lateral_scan` | `ArcheryAimWithLateralScan` | `435-` | Archery_Aim_with_Lateral_Scan | 5.00 |  |
| `kr_archery_shot` | `ArcheryShot` | `436-` | Archery_Shot | 5.00 |  |
| `kr_archery_shot_2` | `ArcheryShot2` | `437-` | Archery_Shot_2 | 3.70 |  |
| `kr_archery_shot_3` | `ArcheryShot3` | `438-` | Archery_Shot_3 | 0.67 |  |
| `kr_arm_circle_shuffle` | `ArmCircleShuffle` | `439-` | Arm_Circle_Shuffle | 9.67 |  |
| `kr_backflip` | `Backflip` | `440-` | Backflip | 2.10 |  |
| `kr_backflip_jump` | `BackflipJump` | `441-` | Backflip_Jump | 4.07 | 0.6 |
| `kr_backflip_sweep_kick` | `BackflipSweepKick` | `442-` | Backflip_Sweep_Kick | 4.80 | 0.8 |
| `kr_backflip_and_hooks` | `BackflipAndHooks` | `443-` | Backflip_and_Hooks | 5.80 | 0.5 |
| `kr_behit_flyup` | `BeHitFlyUp` | `444-` | BeHit_FlyUp | 1.50 |  |
| `kr_carry_heavy_cannon_forward` | `CarryHeavyCannonForward` | `445-` | Carry_Heavy_Cannon_Forward | 0.90 |  |
| `kr_carry_water_bucket_walk` | `CarryWaterBucketWalk` | `446-` | Carry_Water_Bucket_Walk | 9.23 | 1.6 |
| `kr_cartographer` | `Cartographer` | `447-` | Cartographer | 2.97 |  |
| `kr_catching_breath` | `CatchingBreath` | `448-` | Catching_Breath | 7.00 | 5.3 |
| `kr_charged_ground_slam` | `ChargedGroundSlam` | `449-` | Charged_Ground_Slam | 3.00 |  |
| `kr_charged_spell_cast` | `ChargedSpellCast` | `450-` | Charged_Spell_Cast | 2.67 |  |
| `kr_charged_upward_slash` | `ChargedUpwardSlash` | `451-` | Charged_Upward_Slash | 3.17 |  |
| `kr_checkout_gesture` | `CheckoutGesture` | `452-` | Checkout_Gesture | 6.47 |  |
| `kr_chopping_vegetables` | `ChoppingVegetables` | `453-` | Chopping_Vegetables | 2.97 |  |
| `kr_cowboy_quick_draw_shooting` | `CowboyQuickDrawShooting` | `454-` | Cowboy_Quick_Draw_Shooting | 7.30 |  |
| `kr_draw_and_shoot_left` | `DrawAndShootLeft` | `455-` | Draw_and_Shoot_Left | 5.30 |  |
| `kr_draw_and_shoot_from_back` | `DrawAndShootFromBack` | `456-` | Draw_and_Shoot_from_Back | 7.80 |  |
| `kr_draw_and_shoot_from_back_1` | `DrawAndShootFromBack1` | `457-` | Draw_and_Shoot_from_Back_1 | 7.80 |  |
| `kr_draw_and_shoot_from_back_2` | `DrawAndShootFromBack2` | `458-` | Draw_and_Shoot_from_Back_2 | 5.00 |  |
| `kr_fall2` | `Fall2` | `459-` | Fall2 | 0.67 |  |
| `kr_female_bow_charge_left_hand` | `FemaleBowChargeLeftHand` | `460-` | Female_Bow_Charge_Left_Hand | 0.50 | 2.5 |
| `kr_female_bow_charge_left_hand_inplace` | `FemaleBowChargeLeftHandInplace` | `461-` | Female_Bow_Charge_Left_Hand_inplace | 0.50 |  |
| `kr_female_crouch_pick_fruit_basket_stand` | `FemaleCrouchPickFruitBasketStand` | `462-` | Female_Crouch_Pick_Fruit_Basket_Stand | 7.07 |  |
| `kr_female_crouch_pick_gun_point_forward` | `FemaleCrouchPickGunPointForward` | `463-` | Female_Crouch_Pick_Gun_Point_Forward | 2.40 | 0.7 |
| `kr_female_crouch_pick_throw_forward` | `FemaleCrouchPickThrowForward` | `464-` | Female_Crouch_Pick_Throw_Forward | 4.87 |  |
| `kr_female_crouch_pick_up_place_side` | `FemaleCrouchPickUpPlaceSide` | `465-` | Female_Crouch_Pick_Up_Place_Side | 9.53 |  |
| `kr_female_head_down_charge` | `FemaleHeadDownCharge` | `466-` | Female_Head_Down_Charge | 0.50 | 2.9 |
| `kr_female_run_forward_pick_up_right` | `FemaleRunForwardPickUpRight` | `467-` | Female_Run_Forward_Pick_Up_Right | 1.20 | 5.3 |
| `kr_female_stand_pick_fruit_basket` | `FemaleStandPickFruitBasket` | `468-` | Female_Stand_Pick_Fruit_Basket | 6.17 |  |
| `kr_female_throwing_stance_charge` | `FemaleThrowingStanceCharge` | `469-` | Female_Throwing_Stance_Charge | 0.50 | 2.8 |
| `kr_female_throwing_stance_charge_inplace` | `FemaleThrowingStanceChargeInplace` | `470-` | Female_Throwing_Stance_Charge_inplace | 0.50 |  |
| `kr_female_walk_pick_put_in_pocket` | `FemaleWalkPickPutInPocket` | `471-` | Female_Walk_Pick_Put_In_Pocket | 9.50 | 1.2 |
| `kr_rip` | `RIP` | `472-` | RIP | 2.97 |  |
| `kr_sailor_scout` | `SailorScout` | `473-` | Sailor_Scout | 2.97 | 0.1 |
| `kr_stand_up1` | `StandUp1` | `474-` | Stand_Up1 | 8.23 | 0.7 |
| `kr_stirring` | `Stirring` | `475-` | Stirring | 2.97 |  |
| `kr_sword_judgment` | `SwordJudgment` | `476-` | Sword_Judgment | 4.30 | 1.2 |
| `kr_sword_parry` | `SwordParry` | `477-` | Sword_Parry | 1.80 |  |
| `kr_sword_parry_backward` | `SwordParryBackward` | `478-` | Sword_Parry_Backward | 1.30 |  |
| `kr_sword_parry_backward_1` | `SwordParryBackward1` | `479-` | Sword_Parry_Backward_1 | 1.37 |  |
| `kr_sword_parry_backward_2` | `SwordParryBackward2` | `480-` | Sword_Parry_Backward_2 | 0.50 | 0.3 |
| `kr_sword_parry_backward_3` | `SwordParryBackward3` | `481-` | Sword_Parry_Backward_3 | 0.80 |  |
| `kr_sword_parry_backward_4` | `SwordParryBackward4` | `482-` | Sword_Parry_Backward_4 | 0.70 |  |
| `kr_sword_parry_backward_5` | `SwordParryBackward5` | `483-` | Sword_Parry_Backward_5 | 3.00 | 0.9 |
| `kr_sword_and_shield_alert_turn_left` | `SwordAndShieldAlertTurnLeft` | `484-` | Sword_and_Shield_Alert_Turn_Left | 1.30 | 0.3 |
| `kr_sword_and_shield_alert_turn_right` | `SwordAndShieldAlertTurnRight` | `485-` | Sword_and_Shield_Alert_Turn_Right | 3.30 |  |
| `kr_talk_with_right_hand_open` | `TalkWithRightHandOpen` | `486-` | Talk_with_Right_Hand_Open | 3.70 |  |
| `kr_tightrope_walk_inplace` | `TightropeWalkInplace` | `487-` | Tightrope_Walk_inplace | 5.17 |  |
