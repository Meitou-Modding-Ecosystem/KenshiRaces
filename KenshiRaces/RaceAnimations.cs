namespace KenshiRaces;

/// <summary>
/// The animation library (docs/animations.md): every clip of the bodies' source models, retargeted onto the game's male and female skeletons and
/// played in place. Each is a disabled COMBAT_TECHNIQUE whose <c>anim name</c> is the clip, so a mod plays it as a mod combat move
/// (<c>ModContext.AddCombatMove</c>, <see cref="AnimationRecords.Technique"/>); the clips are in the ANIMATION_FILE
/// <see cref="AnimationRecords.File"/>, linked to every race on the game's human skeletons. Generated from the export's clip list; ids
/// <c>100-</c> to <c>158-KenshiRaces.mod</c> in name order for the first models' 59 clips, then <c>159-</c> to <c>171-</c> for the
/// spidercrab's 13, <c>172-</c> to <c>187-</c> for the ghostcrab's 16, <c>188-</c> to <c>199-</c> and <c>400-</c> to <c>411-</c> for the
/// Batfolk's 24, <c>412-</c> to <c>434-</c> for the Wolfkin's 23 (moved off <c>200-</c> to <c>234-</c>, the introducer's records), <c>435-</c>
/// to <c>487-</c> in name order for the tieflings', the beepbot's and the Owlkind's 53, <c>488-</c> and <c>489-</c> for the chibot's 2, each new
/// clip taking the next free id.
/// </summary>
public static class RaceAnimations
{
    /// <summary>
    /// Meshy's <c>_360_Power_Spin_Jump</c>, 3.00 s.
    /// </summary>
    public static readonly RaceAnimation PowerSpinJump360 = new("kr_360_power_spin_jump", "_360_Power_Spin_Jump", 3f, Technique(100));

    /// <summary>
    /// Meshy's <c>Alert</c>, 4.00 s.
    /// </summary>
    public static readonly RaceAnimation Alert = new("kr_alert", "Alert", 4f, Technique(101));

    /// <summary>
    /// Meshy's <c>All_Night_Dance</c>, 8.17 s.
    /// </summary>
    public static readonly RaceAnimation AllNightDance = new("kr_all_night_dance", "All_Night_Dance", 8.167f, Technique(102));

    /// <summary>
    /// Meshy's <c>Angry_Ground_Stomp</c>, 1.37 s.
    /// </summary>
    public static readonly RaceAnimation AngryGroundStomp = new("kr_angry_ground_stomp", "Angry_Ground_Stomp", 1.367f, Technique(103));

    /// <summary>
    /// Meshy's <c>Angry_Ground_Stomp_2</c>, 1.77 s.
    /// </summary>
    public static readonly RaceAnimation AngryGroundStomp2 = new("kr_angry_ground_stomp_2", "Angry_Ground_Stomp_2", 1.767f, Technique(104));

    /// <summary>
    /// Meshy's <c>Archery_Shot_1</c>, 1.00 s.
    /// </summary>
    public static readonly RaceAnimation ArcheryShot1 = new("kr_archery_shot_1", "Archery_Shot_1", 1f, Technique(105));

    /// <summary>
    /// Meshy's <c>Axe_Spin_Attack</c>, 2.50 s.
    /// </summary>
    public static readonly RaceAnimation AxeSpinAttack = new("kr_axe_spin_attack", "Axe_Spin_Attack", 2.5f, Technique(106));

    /// <summary>
    /// Meshy's <c>Axe_Stance</c>, 6.07 s.
    /// </summary>
    public static readonly RaceAnimation AxeStance = new("kr_axe_stance", "Axe_Stance", 6.067f, Technique(107));

    /// <summary>
    /// Meshy's <c>Back_Jump</c>, 0.90 s.
    /// </summary>
    public static readonly RaceAnimation BackJump = new("kr_back_jump", "Back_Jump", 0.9f, Technique(108));

    /// <summary>
    /// Meshy's <c>Ball_throw</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation BallThrow = new("kr_ball_throw", "Ball_throw", 2.967f, Technique(109));

    /// <summary>
    /// Meshy's <c>baseball_pitching</c>, 3.97 s.
    /// </summary>
    public static readonly RaceAnimation BaseballPitching = new("kr_baseball_pitching", "baseball_pitching", 3.967f, Technique(110));

    /// <summary>
    /// Meshy's <c>bicep_curl</c>, 5.87 s.
    /// </summary>
    public static readonly RaceAnimation BicepCurl = new("kr_bicep_curl", "bicep_curl", 5.867f, Technique(111));

    /// <summary>
    /// Meshy's <c>bicycle_crunch</c>, 1.10 s.
    /// </summary>
    public static readonly RaceAnimation BicycleCrunch = new("kr_bicycle_crunch", "bicycle_crunch", 1.1f, Technique(112));

    /// <summary>
    /// Meshy's <c>Big_Heart_Gesture</c>, 6.17 s.
    /// </summary>
    public static readonly RaceAnimation BigHeartGesture = new("kr_big_heart_gesture", "Big_Heart_Gesture", 6.167f, Technique(113));

    /// <summary>
    /// Meshy's <c>Block1</c>, 3.47 s.
    /// </summary>
    public static readonly RaceAnimation Block1 = new("kr_block1", "Block1", 3.467f, Technique(114));

    /// <summary>
    /// Meshy's <c>Block2</c>, 1.60 s.
    /// </summary>
    public static readonly RaceAnimation Block2 = new("kr_block2", "Block2", 1.6f, Technique(115));

    /// <summary>
    /// Meshy's <c>Breakdance_1990</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation Breakdance1990 = new("kr_breakdance_1990", "Breakdance_1990", 0.5f, Technique(116));

    /// <summary>
    /// Meshy's <c>Carry_Heavy_Object_Walk_inplace</c>, 6.50 s.
    /// </summary>
    public static readonly RaceAnimation CarryHeavyObjectWalkInplace = new("kr_carry_heavy_object_walk_inplace", "Carry_Heavy_Object_Walk_inplace", 6.5f, Technique(117));

    /// <summary>
    /// Meshy's <c>Casual_Walk</c>, 4.17 s.
    /// </summary>
    public static readonly RaceAnimation CasualWalk = new("kr_casual_walk", "Casual_Walk", 4.167f, Technique(118));

    /// <summary>
    /// Meshy's <c>Charged_Spell_Cast_1</c>, 4.30 s.
    /// </summary>
    public static readonly RaceAnimation ChargedSpellCast1 = new("kr_charged_spell_cast_1", "Charged_Spell_Cast_1", 4.3f, Technique(119));

    /// <summary>
    /// Meshy's <c>circle_crunch</c>, 2.30 s.
    /// </summary>
    public static readonly RaceAnimation CircleCrunch = new("kr_circle_crunch", "circle_crunch", 2.3f, Technique(120));

    /// <summary>
    /// Meshy's <c>Climb_Attempt_and_Fall_4</c>, 0.57 s.
    /// </summary>
    public static readonly RaceAnimation ClimbAttemptAndFall4 = new("kr_climb_attempt_and_fall_4", "Climb_Attempt_and_Fall_4", 0.5667f, Technique(121));

    /// <summary>
    /// Meshy's <c>Climb_Attempt_and_Fall_5</c>, 1.17 s.
    /// </summary>
    public static readonly RaceAnimation ClimbAttemptAndFall5 = new("kr_climb_attempt_and_fall_5", "Climb_Attempt_and_Fall_5", 1.167f, Technique(122));

    /// <summary>
    /// Meshy's <c>Climb_Right_with_Both_Limbs</c>, 1.17 s.
    /// </summary>
    public static readonly RaceAnimation ClimbRightWithBothLimbs = new("kr_climb_right_with_both_limbs", "Climb_Right_with_Both_Limbs", 1.167f, Technique(123));

    /// <summary>
    /// Meshy's <c>Climb_Right_with_Both_Limbs_inplace</c>, 1.17 s.
    /// </summary>
    public static readonly RaceAnimation ClimbRightWithBothLimbsInplace = new("kr_climb_right_with_both_limbs_inplace", "Climb_Right_with_Both_Limbs_inplace", 1.167f, Technique(124));

    /// <summary>
    /// Meshy's <c>Climb_Stairs</c>, 2.90 s.
    /// </summary>
    public static readonly RaceAnimation ClimbStairs = new("kr_climb_stairs", "Climb_Stairs", 2.9f, Technique(125));

    /// <summary>
    /// Meshy's <c>Climb_Up_Rope</c>, 2.57 s.
    /// </summary>
    public static readonly RaceAnimation ClimbUpRope = new("kr_climb_up_rope", "Climb_Up_Rope", 2.567f, Technique(126));

    /// <summary>
    /// Meshy's <c>Confident_Strut</c>, 2.67 s.
    /// </summary>
    public static readonly RaceAnimation ConfidentStrut = new("kr_confident_strut", "Confident_Strut", 2.667f, Technique(127));

    /// <summary>
    /// Meshy's <c>Counterstrike</c>, 6.50 s.
    /// </summary>
    public static readonly RaceAnimation Counterstrike = new("kr_counterstrike", "Counterstrike", 6.5f, Technique(128));

    /// <summary>
    /// Meshy's <c>Fast_Lightning</c>, 10.80 s.
    /// </summary>
    public static readonly RaceAnimation FastLightning = new("kr_fast_lightning", "Fast_Lightning", 10.8f, Technique(129));

    /// <summary>
    /// Meshy's <c>Female_Bend_Over_Pick_Up_Inspect</c>, 3.80 s.
    /// </summary>
    public static readonly RaceAnimation FemaleBendOverPickUpInspect = new("kr_female_bend_over_pick_up_inspect", "Female_Bend_Over_Pick_Up_Inspect", 3.8f, Technique(130));

    /// <summary>
    /// Meshy's <c>Flirty_Strut_inplace</c>, 3.20 s.
    /// </summary>
    public static readonly RaceAnimation FlirtyStrutInplace = new("kr_flirty_strut_inplace", "Flirty_Strut_inplace", 3.2f, Technique(131));

    /// <summary>
    /// Meshy's <c>Funky_Walk</c>, 1.87 s.
    /// </summary>
    public static readonly RaceAnimation FunkyWalk = new("kr_funky_walk", "Funky_Walk", 1.867f, Technique(132));

    /// <summary>
    /// Meshy's <c>Gentlemans_Bow</c>, 7.27 s.
    /// </summary>
    public static readonly RaceAnimation GentlemansBow = new("kr_gentlemans_bow", "Gentlemans_Bow", 7.267f, Technique(133));

    /// <summary>
    /// Meshy's <c>Jazz_Hands_inplace</c>, 2.70 s.
    /// </summary>
    public static readonly RaceAnimation JazzHandsInplace = new("kr_jazz_hands_inplace", "Jazz_Hands_inplace", 2.7f, Technique(134));

    /// <summary>
    /// Meshy's <c>jumping_jacks</c>, 1.10 s.
    /// </summary>
    public static readonly RaceAnimation JumpingJacks = new("kr_jumping_jacks", "jumping_jacks", 1.1f, Technique(135));

    /// <summary>
    /// Meshy's <c>Kung_Fu_Punch</c>, 7.30 s.
    /// </summary>
    public static readonly RaceAnimation KungFuPunch = new("kr_kung_fu_punch", "Kung_Fu_Punch", 7.3f, Technique(136));

    /// <summary>
    /// Meshy's <c>Monster_Walk</c>, 4.07 s.
    /// </summary>
    public static readonly RaceAnimation MonsterWalk = new("kr_monster_walk", "Monster_Walk", 4.067f, Technique(137));

    /// <summary>
    /// Meshy's <c>Prone_Reload</c>, 6.40 s.
    /// </summary>
    public static readonly RaceAnimation ProneReload = new("kr_prone_reload", "Prone_Reload", 6.4f, Technique(138));

    /// <summary>
    /// Meshy's <c>Running</c>, 0.60 s.
    /// </summary>
    public static readonly RaceAnimation Running = new("kr_running", "Running", 0.6f, Technique(139));

    /// <summary>
    /// Meshy's <c>Sit_Cross_Legged_on_Floor</c>, 2.50 s.
    /// </summary>
    public static readonly RaceAnimation SitCrossLeggedOnFloor = new("kr_sit_cross_legged_on_floor", "Sit_Cross_Legged_on_Floor", 2.5f, Technique(140));

    /// <summary>
    /// Meshy's <c>Stand_and_Chat</c>, 5.17 s.
    /// </summary>
    public static readonly RaceAnimation StandAndChat = new("kr_stand_and_chat", "Stand_and_Chat", 5.167f, Technique(141));

    /// <summary>
    /// Meshy's <c>Stand_and_Drink</c>, 8.87 s.
    /// </summary>
    public static readonly RaceAnimation StandAndDrink = new("kr_stand_and_drink", "Stand_and_Drink", 8.867f, Technique(142));

    /// <summary>
    /// Meshy's <c>Stand_on_Pole_and_Balance</c>, 1.80 s.
    /// </summary>
    public static readonly RaceAnimation StandOnPoleAndBalance = new("kr_stand_on_pole_and_balance", "Stand_on_Pole_and_Balance", 1.8f, Technique(143));

    /// <summary>
    /// Meshy's <c>Stand_To_Side_Lying</c>, 9.67 s.
    /// </summary>
    public static readonly RaceAnimation StandToSideLying = new("kr_stand_to_side_lying", "Stand_To_Side_Lying", 9.667f, Technique(144));

    /// <summary>
    /// Meshy's <c>Stand_to_Sit_Transition_M</c>, 4.73 s.
    /// </summary>
    public static readonly RaceAnimation StandToSitTransitionM = new("kr_stand_to_sit_transition_m", "Stand_to_Sit_Transition_M", 4.733f, Technique(145));

    /// <summary>
    /// Meshy's <c>Step_to_Sit_Transition</c>, 5.67 s.
    /// </summary>
    public static readonly RaceAnimation StepToSitTransition = new("kr_step_to_sit_transition", "Step_to_Sit_Transition", 5.667f, Technique(146));

    /// <summary>
    /// Meshy's <c>Swim_Forward</c>, 4.50 s.
    /// </summary>
    public static readonly RaceAnimation SwimForward = new("kr_swim_forward", "Swim_Forward", 4.5f, Technique(147));

    /// <summary>
    /// Meshy's <c>Swim_Idle</c>, 3.00 s.
    /// </summary>
    public static readonly RaceAnimation SwimIdle = new("kr_swim_idle", "Swim_Idle", 3f, Technique(148));

    /// <summary>
    /// Meshy's <c>Sword_Shout</c>, 6.17 s.
    /// </summary>
    public static readonly RaceAnimation SwordShout = new("kr_sword_shout", "Sword_Shout", 6.167f, Technique(149));

    /// <summary>
    /// Meshy's <c>Talk_with_Hands_Open</c>, 3.97 s.
    /// </summary>
    public static readonly RaceAnimation TalkWithHandsOpen = new("kr_talk_with_hands_open", "Talk_with_Hands_Open", 3.967f, Technique(150));

    /// <summary>
    /// Meshy's <c>Texting_Walk</c>, 2.00 s.
    /// </summary>
    public static readonly RaceAnimation TextingWalk = new("kr_texting_walk", "Texting_Walk", 2f, Technique(151));

    /// <summary>
    /// Meshy's <c>Victory_Fist_Pump</c>, 1.50 s.
    /// </summary>
    public static readonly RaceAnimation VictoryFistPump = new("kr_victory_fist_pump", "Victory_Fist_Pump", 1.5f, Technique(152));

    /// <summary>
    /// Meshy's <c>Walk_Backward_with_Gun_inplace</c>, 1.00 s.
    /// </summary>
    public static readonly RaceAnimation WalkBackwardWithGunInplace = new("kr_walk_backward_with_gun_inplace", "Walk_Backward_with_Gun_inplace", 1f, Technique(153));

    /// <summary>
    /// Meshy's <c>Walking</c>, 1.00 s.
    /// </summary>
    public static readonly RaceAnimation Walking = new("kr_walking", "Walking", 1f, Technique(154));

    /// <summary>
    /// Meshy's <c>walking_2_inplace</c>, 1.17 s.
    /// </summary>
    public static readonly RaceAnimation Walking2Inplace = new("kr_walking_2_inplace", "walking_2_inplace", 1.167f, Technique(155));

    /// <summary>
    /// Meshy's <c>Walking_Woman</c>, 0.97 s.
    /// </summary>
    public static readonly RaceAnimation WalkingWoman = new("kr_walking_woman", "Walking_Woman", 0.9667f, Technique(156));

    /// <summary>
    /// Meshy's <c>Wall_Push_Jump_and_Flip</c>, 2.00 s.
    /// </summary>
    public static readonly RaceAnimation WallPushJumpAndFlip = new("kr_wall_push_jump_and_flip", "Wall_Push_Jump_and_Flip", 2f, Technique(157));

    /// <summary>
    /// Meshy's <c>Wave_for_Help_3</c>, 3.17 s.
    /// </summary>
    public static readonly RaceAnimation WaveForHelp3 = new("kr_wave_for_help_3", "Wave_for_Help_3", 3.167f, Technique(158));

    /// <summary>
    /// Meshy's <c>Attack</c>, 2.77 s.
    /// </summary>
    public static readonly RaceAnimation Attack = new("kr_attack", "Attack", 2.767f, Technique(159));

    /// <summary>
    /// Meshy's <c>Block6</c>, 1.67 s.
    /// </summary>
    public static readonly RaceAnimation Block6 = new("kr_block6", "Block6", 1.667f, Technique(160));

    /// <summary>
    /// Meshy's <c>Block9</c>, 2.77 s.
    /// </summary>
    public static readonly RaceAnimation Block9 = new("kr_block9", "Block9", 2.767f, Technique(161));

    /// <summary>
    /// Meshy's <c>Boxing_Guard_Prep_Straight_Punch</c>, 3.97 s.
    /// </summary>
    public static readonly RaceAnimation BoxingGuardPrepStraightPunch = new("kr_boxing_guard_prep_straight_punch", "Boxing_Guard_Prep_Straight_Punch", 3.967f, Technique(162));

    /// <summary>
    /// Meshy's <c>Broadsword___Left</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation BroadswordLeft = new("kr_broadsword_left", "Broadsword___Left", 2.967f, Technique(163));

    /// <summary>
    /// Meshy's <c>Broadsword___Right</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation BroadswordRight = new("kr_broadsword_right", "Broadsword___Right", 2.967f, Technique(164));

    /// <summary>
    /// Meshy's <c>Broadsword___Thrust</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation BroadswordThrust = new("kr_broadsword_thrust", "Broadsword___Thrust", 2.967f, Technique(165));

    /// <summary>
    /// Meshy's <c>Broadsword___Top</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation BroadswordTop = new("kr_broadsword_top", "Broadsword___Top", 2.967f, Technique(166));

    /// <summary>
    /// Meshy's <c>Broadsword___Top_2</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation BroadswordTop2 = new("kr_broadsword_top_2", "Broadsword___Top_2", 2.967f, Technique(167));

    /// <summary>
    /// Meshy's <c>Charged_Slash</c>, 2.20 s.
    /// </summary>
    public static readonly RaceAnimation ChargedSlash = new("kr_charged_slash", "Charged_Slash", 2.2f, Technique(168));

    /// <summary>
    /// Meshy's <c>mage_soell_cast_6</c>, 2.27 s.
    /// </summary>
    public static readonly RaceAnimation MageSoellCast6 = new("kr_mage_soell_cast_6", "mage_soell_cast_6", 2.267f, Technique(169));

    /// <summary>
    /// Meshy's <c>Reaping_Swing</c>, 5.90 s.
    /// </summary>
    public static readonly RaceAnimation ReapingSwing = new("kr_reaping_swing", "Reaping_Swing", 5.9f, Technique(170));

    /// <summary>
    /// Meshy's <c>Right_Hand_Sword_Slash</c>, 1.50 s.
    /// </summary>
    public static readonly RaceAnimation RightHandSwordSlash = new("kr_right_hand_sword_slash", "Right_Hand_Sword_Slash", 1.5f, Technique(171));

    /// <summary>
    /// Meshy's <c>Left_Short_Hook_from_Guard</c>, 1.27 s.
    /// </summary>
    public static readonly RaceAnimation LeftShortHookFromGuard = new("kr_left_short_hook_from_guard", "Left_Short_Hook_from_Guard", 1.267f, Technique(172));

    /// <summary>
    /// Meshy's <c>Left_Slash</c>, 3.17 s.
    /// </summary>
    public static readonly RaceAnimation LeftSlash = new("kr_left_slash", "Left_Slash", 3.167f, Technique(173));

    /// <summary>
    /// Meshy's <c>Left_Uppercut_from_Guard</c>, 1.30 s.
    /// </summary>
    public static readonly RaceAnimation LeftUppercutFromGuard = new("kr_left_uppercut_from_guard", "Left_Uppercut_from_Guard", 1.3f, Technique(174));

    /// <summary>
    /// Meshy's <c>Right_Upper_Hook_from_Guard</c>, 1.70 s.
    /// </summary>
    public static readonly RaceAnimation RightUpperHookFromGuard = new("kr_right_upper_hook_from_guard", "Right_Upper_Hook_from_Guard", 1.7f, Technique(175));

    /// <summary>
    /// Meshy's <c>Right_Uppercut_from_Guard</c>, 1.00 s.
    /// </summary>
    public static readonly RaceAnimation RightUppercutFromGuard = new("kr_right_uppercut_from_guard", "Right_Uppercut_from_Guard", 1f, Technique(176));

    /// <summary>
    /// Meshy's <c>Walk_Backward_inplace</c>, 0.87 s.
    /// </summary>
    public static readonly RaceAnimation WalkBackwardInplace = new("kr_walk_backward_inplace", "Walk_Backward_inplace", 0.8667f, Technique(177));

    /// <summary>
    /// Meshy's <c>Walk_Backward_with_Grenade_inplace</c>, 1.20 s.
    /// </summary>
    public static readonly RaceAnimation WalkBackwardWithGrenadeInplace = new("kr_walk_backward_with_grenade_inplace", "Walk_Backward_with_Grenade_inplace", 1.2f, Technique(178));

    /// <summary>
    /// Meshy's <c>Walk_Fight_Forward</c>, 1.70 s.
    /// </summary>
    public static readonly RaceAnimation WalkFightForward = new("kr_walk_fight_forward", "Walk_Fight_Forward", 1.7f, Technique(179));

    /// <summary>
    /// Meshy's <c>Walk_Forward_While_Shooting</c>, 3.27 s.
    /// </summary>
    public static readonly RaceAnimation WalkForwardWhileShooting = new("kr_walk_forward_while_shooting", "Walk_Forward_While_Shooting", 3.267f, Technique(180));

    /// <summary>
    /// Meshy's <c>Walk_Forward_with_Bow_Aimed</c>, 1.20 s.
    /// </summary>
    public static readonly RaceAnimation WalkForwardWithBowAimed = new("kr_walk_forward_with_bow_aimed", "Walk_Forward_with_Bow_Aimed", 1.2f, Technique(181));

    /// <summary>
    /// Meshy's <c>Walk_Left_with_Gun</c>, 1.20 s.
    /// </summary>
    public static readonly RaceAnimation WalkLeftWithGun = new("kr_walk_left_with_gun", "Walk_Left_with_Gun", 1.2f, Technique(182));

    /// <summary>
    /// Meshy's <c>Walk_with_Umbrella</c>, 1.30 s.
    /// </summary>
    public static readonly RaceAnimation WalkWithUmbrella = new("kr_walk_with_umbrella", "Walk_with_Umbrella", 1.3f, Technique(183));

    /// <summary>
    /// Meshy's <c>Walk_with_Umbrella_inplace</c>, 1.30 s.
    /// </summary>
    public static readonly RaceAnimation WalkWithUmbrellaInplace = new("kr_walk_with_umbrella_inplace", "Walk_with_Umbrella_inplace", 1.3f, Technique(184));

    /// <summary>
    /// Meshy's <c>Weapon_Combo</c>, 3.67 s.
    /// </summary>
    public static readonly RaceAnimation WeaponCombo = new("kr_weapon_combo", "Weapon_Combo", 3.667f, Technique(185));

    /// <summary>
    /// Meshy's <c>Weapon_Combo_1</c>, 3.50 s.
    /// </summary>
    public static readonly RaceAnimation WeaponCombo1 = new("kr_weapon_combo_1", "Weapon_Combo_1", 3.5f, Technique(186));

    /// <summary>
    /// Meshy's <c>Weapon_Combo_2</c>, 4.50 s.
    /// </summary>
    public static readonly RaceAnimation WeaponCombo2 = new("kr_weapon_combo_2", "Weapon_Combo_2", 4.5f, Technique(187));

    /// <summary>
    /// Meshy's <c>Agree_Gesture</c>, 13.00 s.
    /// </summary>
    public static readonly RaceAnimation AgreeGesture = new("kr_agree_gesture", "Agree_Gesture", 13f, Technique(188));

    /// <summary>
    /// Meshy's <c>air_squat</c>, 1.87 s.
    /// </summary>
    public static readonly RaceAnimation AirSquat = new("kr_air_squat", "air_squat", 1.867f, Technique(189));

    /// <summary>
    /// Meshy's <c>Climb_Attempt_and_Fall_2</c>, 0.80 s.
    /// </summary>
    public static readonly RaceAnimation ClimbAttemptAndFall2 = new("kr_climb_attempt_and_fall_2", "Climb_Attempt_and_Fall_2", 0.8f, Technique(190));

    /// <summary>
    /// Meshy's <c>Climb_Left_with_Both_Limbs</c>, 1.17 s.
    /// </summary>
    public static readonly RaceAnimation ClimbLeftWithBothLimbs = new("kr_climb_left_with_both_limbs", "Climb_Left_with_Both_Limbs", 1.167f, Technique(191));

    /// <summary>
    /// Meshy's <c>Collect_Object</c>, 6.00 s.
    /// </summary>
    public static readonly RaceAnimation CollectObject = new("kr_collect_object", "Collect_Object", 6f, Technique(192));

    /// <summary>
    /// Meshy's <c>Confused_Scratch</c>, 11.50 s.
    /// </summary>
    public static readonly RaceAnimation ConfusedScratch = new("kr_confused_scratch", "Confused_Scratch", 11.5f, Technique(193));

    /// <summary>
    /// Meshy's <c>Crouch_and_Step_Back</c>, 2.67 s.
    /// </summary>
    public static readonly RaceAnimation CrouchAndStepBack = new("kr_crouch_and_step_back", "Crouch_and_Step_Back", 2.667f, Technique(194));

    /// <summary>
    /// Meshy's <c>Crouch_Pull_and_Throw</c>, 5.80 s.
    /// </summary>
    public static readonly RaceAnimation CrouchPullAndThrow = new("kr_crouch_pull_and_throw", "Crouch_Pull_and_Throw", 5.8f, Technique(195));

    /// <summary>
    /// Meshy's <c>Discuss_While_Moving</c>, 8.00 s.
    /// </summary>
    public static readonly RaceAnimation DiscussWhileMoving = new("kr_discuss_while_moving", "Discuss_While_Moving", 8f, Technique(196));

    /// <summary>
    /// Meshy's <c>Dive_Down_and_Land</c>, 1.60 s.
    /// </summary>
    public static readonly RaceAnimation DiveDownAndLand = new("kr_dive_down_and_land", "Dive_Down_and_Land", 1.6f, Technique(197));

    /// <summary>
    /// Meshy's <c>Dive_Down_and_Land_1</c>, 0.37 s.
    /// </summary>
    public static readonly RaceAnimation DiveDownAndLand1 = new("kr_dive_down_and_land_1", "Dive_Down_and_Land_1", 0.367f, Technique(198));

    /// <summary>
    /// Meshy's <c>Dive_Down_and_Land_2</c>, 3.07 s.
    /// </summary>
    public static readonly RaceAnimation DiveDownAndLand2 = new("kr_dive_down_and_land_2", "Dive_Down_and_Land_2", 3.067f, Technique(199));

    /// <summary>
    /// Meshy's <c>Elderly_Shaky_Walk</c>, 2.80 s.
    /// </summary>
    public static readonly RaceAnimation ElderlyShakyWalk = new("kr_elderly_shaky_walk", "Elderly_Shaky_Walk", 2.8f, Technique(400));

    /// <summary>
    /// Meshy's <c>Parkour_Vault_1</c>, 1.17 s.
    /// </summary>
    public static readonly RaceAnimation ParkourVault1 = new("kr_parkour_vault_1", "Parkour_Vault_1", 1.167f, Technique(401));

    /// <summary>
    /// Meshy's <c>Parkour_Vault_2</c>, 1.17 s.
    /// </summary>
    public static readonly RaceAnimation ParkourVault2 = new("kr_parkour_vault_2", "Parkour_Vault_2", 1.167f, Technique(402));

    /// <summary>
    /// Meshy's <c>Parkour_Vault_3</c>, 2.30 s.
    /// </summary>
    public static readonly RaceAnimation ParkourVault3 = new("kr_parkour_vault_3", "Parkour_Vault_3", 2.3f, Technique(403));

    /// <summary>
    /// Meshy's <c>Parkour_Vault_with_Roll</c>, 2.07 s.
    /// </summary>
    public static readonly RaceAnimation ParkourVaultWithRoll = new("kr_parkour_vault_with_roll", "Parkour_Vault_with_Roll", 2.067f, Technique(404));

    /// <summary>
    /// Meshy's <c>penguin_walk</c>, 2.00 s.
    /// </summary>
    public static readonly RaceAnimation PenguinWalk = new("kr_penguin_walk", "penguin_walk", 2f, Technique(405));

    /// <summary>
    /// Meshy's <c>Personalized_Gesture</c>, 2.00 s.
    /// </summary>
    public static readonly RaceAnimation PersonalizedGesture = new("kr_personalized_gesture", "Personalized_Gesture", 2f, Technique(406));

    /// <summary>
    /// Meshy's <c>Wave_for_Help</c>, 5.00 s.
    /// </summary>
    public static readonly RaceAnimation WaveForHelp = new("kr_wave_for_help", "Wave_for_Help", 5f, Technique(407));

    /// <summary>
    /// Meshy's <c>Wave_for_Help_1</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation WaveForHelp1 = new("kr_wave_for_help_1", "Wave_for_Help_1", 0.5f, Technique(408));

    /// <summary>
    /// Meshy's <c>Wave_for_Help_2</c>, 1.50 s.
    /// </summary>
    public static readonly RaceAnimation WaveForHelp2 = new("kr_wave_for_help_2", "Wave_for_Help_2", 1.5f, Technique(409));

    /// <summary>
    /// Meshy's <c>Wave_for_Help_4</c>, 4.67 s.
    /// </summary>
    public static readonly RaceAnimation WaveForHelp4 = new("kr_wave_for_help_4", "Wave_for_Help_4", 4.667f, Technique(410));

    /// <summary>
    /// Meshy's <c>Wave_One_Hand</c>, 4.07 s.
    /// </summary>
    public static readonly RaceAnimation WaveOneHand = new("kr_wave_one_hand", "Wave_One_Hand", 4.067f, Technique(411));

    /// <summary>
    /// Meshy's <c>Riding_Yeehaw</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation RidingYeehaw = new("kr_riding_yeehaw", "Riding_Yeehaw", 2.967f, Technique(412));

    /// <summary>
    /// Meshy's <c>Riding_calm</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation RidingCalm = new("kr_riding_calm", "Riding_calm", 2.967f, Technique(413));

    /// <summary>
    /// Meshy's <c>SideLying_Reach_Help</c>, 14.67 s.
    /// </summary>
    public static readonly RaceAnimation SideLyingReachHelp = new("kr_sidelying_reach_help", "SideLying_Reach_Help", 14.667f, Technique(414));

    /// <summary>
    /// Meshy's <c>Sit_Dodge</c>, 8.87 s.
    /// </summary>
    public static readonly RaceAnimation SitDodge = new("kr_sit_dodge", "Sit_Dodge", 8.867f, Technique(415));

    /// <summary>
    /// Meshy's <c>Sit_and_Doze_Off</c>, 17.27 s.
    /// </summary>
    public static readonly RaceAnimation SitAndDozeOff = new("kr_sit_and_doze_off", "Sit_and_Doze_Off", 17.267f, Technique(416));

    /// <summary>
    /// Meshy's <c>Sit_and_Drink</c>, 11.17 s.
    /// </summary>
    public static readonly RaceAnimation SitAndDrink = new("kr_sit_and_drink", "Sit_and_Drink", 11.167f, Technique(417));

    /// <summary>
    /// Meshy's <c>Sit_on_Chair_Arms_Crossed</c>, 2.37 s.
    /// </summary>
    public static readonly RaceAnimation SitOnChairArmsCrossed = new("kr_sit_on_chair_arms_crossed", "Sit_on_Chair_Arms_Crossed", 2.367f, Technique(418));

    /// <summary>
    /// Meshy's <c>Skill_01</c>, 1.07 s.
    /// </summary>
    public static readonly RaceAnimation Skill01 = new("kr_skill_01", "Skill_01", 1.067f, Technique(419));

    /// <summary>
    /// Meshy's <c>Skill_02</c>, 1.27 s.
    /// </summary>
    public static readonly RaceAnimation Skill02 = new("kr_skill_02", "Skill_02", 1.267f, Technique(420));

    /// <summary>
    /// Meshy's <c>Skill_03</c>, 1.60 s.
    /// </summary>
    public static readonly RaceAnimation Skill03 = new("kr_skill_03", "Skill_03", 1.6f, Technique(421));

    /// <summary>
    /// Meshy's <c>Skip_Forward</c>, 2.80 s.
    /// </summary>
    public static readonly RaceAnimation SkipForward = new("kr_skip_forward", "Skip_Forward", 2.8f, Technique(422));

    /// <summary>
    /// Meshy's <c>Touch_and_Run</c>, 3.17 s.
    /// </summary>
    public static readonly RaceAnimation TouchAndRun = new("kr_touch_and_run", "Touch_and_Run", 3.167f, Technique(423));

    /// <summary>
    /// Meshy's <c>Triple_Combo_Attack</c>, 4.33 s.
    /// </summary>
    public static readonly RaceAnimation TripleComboAttack = new("kr_triple_combo_attack", "Triple_Combo_Attack", 4.333f, Technique(424));

    /// <summary>
    /// Meshy's <c>Two_Handed_Parry</c>, 1.30 s.
    /// </summary>
    public static readonly RaceAnimation TwoHandedParry = new("kr_two_handed_parry", "Two_Handed_Parry", 1.3f, Technique(425));

    /// <summary>
    /// Meshy's <c>Unarmed_Vault</c>, 0.80 s.
    /// </summary>
    public static readonly RaceAnimation UnarmedVault = new("kr_unarmed_vault", "Unarmed_Vault", 0.8f, Technique(426));

    /// <summary>
    /// Meshy's <c>Upside_Down_Rope_Hang</c>, 2.37 s.
    /// </summary>
    public static readonly RaceAnimation UpsideDownRopeHang = new("kr_upside_down_rope_hang", "Upside_Down_Rope_Hang", 2.367f, Technique(427));

    /// <summary>
    /// Meshy's <c>Upside_Down_Rope_Hang_1</c>, 2.73 s.
    /// </summary>
    public static readonly RaceAnimation UpsideDownRopeHang1 = new("kr_upside_down_rope_hang_1", "Upside_Down_Rope_Hang_1", 2.733f, Technique(428));

    /// <summary>
    /// Meshy's <c>Upside_Down_Rope_Hang_2</c>, 1.50 s.
    /// </summary>
    public static readonly RaceAnimation UpsideDownRopeHang2 = new("kr_upside_down_rope_hang_2", "Upside_Down_Rope_Hang_2", 1.5f, Technique(429));

    /// <summary>
    /// Meshy's <c>Vault_and_Land</c>, 3.00 s.
    /// </summary>
    public static readonly RaceAnimation VaultAndLand = new("kr_vault_and_land", "Vault_and_Land", 3f, Technique(430));

    /// <summary>
    /// Meshy's <c>Vault_with_Rifle</c>, 0.80 s.
    /// </summary>
    public static readonly RaceAnimation VaultWithRifle = new("kr_vault_with_rifle", "Vault_with_Rifle", 0.8f, Technique(431));

    /// <summary>
    /// Meshy's <c>Victory_Cheer</c>, 9.30 s.
    /// </summary>
    public static readonly RaceAnimation VictoryCheer = new("kr_victory_cheer", "Victory_Cheer", 9.3f, Technique(432));

    /// <summary>
    /// Meshy's <c>Wake_Up_and_Look_Up</c>, 3.50 s.
    /// </summary>
    public static readonly RaceAnimation WakeUpAndLookUp = new("kr_wake_up_and_look_up", "Wake_Up_and_Look_Up", 3.5f, Technique(433));

    /// <summary>
    /// Meshy's <c>victory</c>, 8.53 s.
    /// </summary>
    public static readonly RaceAnimation Victory = new("kr_victory", "victory", 8.533f, Technique(434));

    /// <summary>
    /// Meshy's <c>Archery_Aim_with_Lateral_Scan</c>, 5.00 s.
    /// </summary>
    public static readonly RaceAnimation ArcheryAimWithLateralScan = new("kr_archery_aim_with_lateral_scan", "Archery_Aim_with_Lateral_Scan", 5f, Technique(435));

    /// <summary>
    /// Meshy's <c>Archery_Shot</c>, 5.00 s.
    /// </summary>
    public static readonly RaceAnimation ArcheryShot = new("kr_archery_shot", "Archery_Shot", 5f, Technique(436));

    /// <summary>
    /// Meshy's <c>Archery_Shot_2</c>, 3.70 s.
    /// </summary>
    public static readonly RaceAnimation ArcheryShot2 = new("kr_archery_shot_2", "Archery_Shot_2", 3.7f, Technique(437));

    /// <summary>
    /// Meshy's <c>Archery_Shot_3</c>, 0.67 s.
    /// </summary>
    public static readonly RaceAnimation ArcheryShot3 = new("kr_archery_shot_3", "Archery_Shot_3", 0.667f, Technique(438));

    /// <summary>
    /// Meshy's <c>Arm_Circle_Shuffle</c>, 9.67 s.
    /// </summary>
    public static readonly RaceAnimation ArmCircleShuffle = new("kr_arm_circle_shuffle", "Arm_Circle_Shuffle", 9.667f, Technique(439));

    /// <summary>
    /// Meshy's <c>Backflip</c>, 2.10 s.
    /// </summary>
    public static readonly RaceAnimation Backflip = new("kr_backflip", "Backflip", 2.1f, Technique(440));

    /// <summary>
    /// Meshy's <c>Backflip_Jump</c>, 4.07 s.
    /// </summary>
    public static readonly RaceAnimation BackflipJump = new("kr_backflip_jump", "Backflip_Jump", 4.067f, Technique(441));

    /// <summary>
    /// Meshy's <c>Backflip_Sweep_Kick</c>, 4.80 s.
    /// </summary>
    public static readonly RaceAnimation BackflipSweepKick = new("kr_backflip_sweep_kick", "Backflip_Sweep_Kick", 4.8f, Technique(442));

    /// <summary>
    /// Meshy's <c>Backflip_and_Hooks</c>, 5.80 s.
    /// </summary>
    public static readonly RaceAnimation BackflipAndHooks = new("kr_backflip_and_hooks", "Backflip_and_Hooks", 5.8f, Technique(443));

    /// <summary>
    /// Meshy's <c>BeHit_FlyUp</c>, 1.50 s.
    /// </summary>
    public static readonly RaceAnimation BeHitFlyUp = new("kr_behit_flyup", "BeHit_FlyUp", 1.5f, Technique(444));

    /// <summary>
    /// Meshy's <c>Carry_Heavy_Cannon_Forward</c>, 0.90 s.
    /// </summary>
    public static readonly RaceAnimation CarryHeavyCannonForward = new("kr_carry_heavy_cannon_forward", "Carry_Heavy_Cannon_Forward", 0.9f, Technique(445));

    /// <summary>
    /// Meshy's <c>Carry_Water_Bucket_Walk</c>, 9.23 s.
    /// </summary>
    public static readonly RaceAnimation CarryWaterBucketWalk = new("kr_carry_water_bucket_walk", "Carry_Water_Bucket_Walk", 9.233f, Technique(446));

    /// <summary>
    /// Meshy's <c>Cartographer</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation Cartographer = new("kr_cartographer", "Cartographer", 2.967f, Technique(447));

    /// <summary>
    /// Meshy's <c>Catching_Breath</c>, 7.00 s.
    /// </summary>
    public static readonly RaceAnimation CatchingBreath = new("kr_catching_breath", "Catching_Breath", 7f, Technique(448));

    /// <summary>
    /// Meshy's <c>Charged_Ground_Slam</c>, 3.00 s.
    /// </summary>
    public static readonly RaceAnimation ChargedGroundSlam = new("kr_charged_ground_slam", "Charged_Ground_Slam", 3f, Technique(449));

    /// <summary>
    /// Meshy's <c>Charged_Spell_Cast</c>, 2.67 s.
    /// </summary>
    public static readonly RaceAnimation ChargedSpellCast = new("kr_charged_spell_cast", "Charged_Spell_Cast", 2.667f, Technique(450));

    /// <summary>
    /// Meshy's <c>Charged_Upward_Slash</c>, 3.17 s.
    /// </summary>
    public static readonly RaceAnimation ChargedUpwardSlash = new("kr_charged_upward_slash", "Charged_Upward_Slash", 3.167f, Technique(451));

    /// <summary>
    /// Meshy's <c>Checkout_Gesture</c>, 6.47 s.
    /// </summary>
    public static readonly RaceAnimation CheckoutGesture = new("kr_checkout_gesture", "Checkout_Gesture", 6.467f, Technique(452));

    /// <summary>
    /// Meshy's <c>Chopping_Vegetables</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation ChoppingVegetables = new("kr_chopping_vegetables", "Chopping_Vegetables", 2.967f, Technique(453));

    /// <summary>
    /// Meshy's <c>Cowboy_Quick_Draw_Shooting</c>, 7.30 s.
    /// </summary>
    public static readonly RaceAnimation CowboyQuickDrawShooting = new("kr_cowboy_quick_draw_shooting", "Cowboy_Quick_Draw_Shooting", 7.3f, Technique(454));

    /// <summary>
    /// Meshy's <c>Draw_and_Shoot_Left</c>, 5.30 s.
    /// </summary>
    public static readonly RaceAnimation DrawAndShootLeft = new("kr_draw_and_shoot_left", "Draw_and_Shoot_Left", 5.3f, Technique(455));

    /// <summary>
    /// Meshy's <c>Draw_and_Shoot_from_Back</c>, 7.80 s.
    /// </summary>
    public static readonly RaceAnimation DrawAndShootFromBack = new("kr_draw_and_shoot_from_back", "Draw_and_Shoot_from_Back", 7.8f, Technique(456));

    /// <summary>
    /// Meshy's <c>Draw_and_Shoot_from_Back_1</c>, 7.80 s.
    /// </summary>
    public static readonly RaceAnimation DrawAndShootFromBack1 = new("kr_draw_and_shoot_from_back_1", "Draw_and_Shoot_from_Back_1", 7.8f, Technique(457));

    /// <summary>
    /// Meshy's <c>Draw_and_Shoot_from_Back_2</c>, 5.00 s.
    /// </summary>
    public static readonly RaceAnimation DrawAndShootFromBack2 = new("kr_draw_and_shoot_from_back_2", "Draw_and_Shoot_from_Back_2", 5f, Technique(458));

    /// <summary>
    /// Meshy's <c>Fall2</c>, 0.67 s.
    /// </summary>
    public static readonly RaceAnimation Fall2 = new("kr_fall2", "Fall2", 0.667f, Technique(459));

    /// <summary>
    /// Meshy's <c>Female_Bow_Charge_Left_Hand</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation FemaleBowChargeLeftHand = new("kr_female_bow_charge_left_hand", "Female_Bow_Charge_Left_Hand", 0.5f, Technique(460));

    /// <summary>
    /// Meshy's <c>Female_Bow_Charge_Left_Hand_inplace</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation FemaleBowChargeLeftHandInplace = new("kr_female_bow_charge_left_hand_inplace", "Female_Bow_Charge_Left_Hand_inplace", 0.5f, Technique(461));

    /// <summary>
    /// Meshy's <c>Female_Crouch_Pick_Fruit_Basket_Stand</c>, 7.07 s.
    /// </summary>
    public static readonly RaceAnimation FemaleCrouchPickFruitBasketStand = new("kr_female_crouch_pick_fruit_basket_stand", "Female_Crouch_Pick_Fruit_Basket_Stand", 7.067f, Technique(462));

    /// <summary>
    /// Meshy's <c>Female_Crouch_Pick_Gun_Point_Forward</c>, 2.40 s.
    /// </summary>
    public static readonly RaceAnimation FemaleCrouchPickGunPointForward = new("kr_female_crouch_pick_gun_point_forward", "Female_Crouch_Pick_Gun_Point_Forward", 2.4f, Technique(463));

    /// <summary>
    /// Meshy's <c>Female_Crouch_Pick_Throw_Forward</c>, 4.87 s.
    /// </summary>
    public static readonly RaceAnimation FemaleCrouchPickThrowForward = new("kr_female_crouch_pick_throw_forward", "Female_Crouch_Pick_Throw_Forward", 4.867f, Technique(464));

    /// <summary>
    /// Meshy's <c>Female_Crouch_Pick_Up_Place_Side</c>, 9.53 s.
    /// </summary>
    public static readonly RaceAnimation FemaleCrouchPickUpPlaceSide = new("kr_female_crouch_pick_up_place_side", "Female_Crouch_Pick_Up_Place_Side", 9.533f, Technique(465));

    /// <summary>
    /// Meshy's <c>Female_Head_Down_Charge</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation FemaleHeadDownCharge = new("kr_female_head_down_charge", "Female_Head_Down_Charge", 0.5f, Technique(466));

    /// <summary>
    /// Meshy's <c>Female_Run_Forward_Pick_Up_Right</c>, 1.20 s.
    /// </summary>
    public static readonly RaceAnimation FemaleRunForwardPickUpRight = new("kr_female_run_forward_pick_up_right", "Female_Run_Forward_Pick_Up_Right", 1.2f, Technique(467));

    /// <summary>
    /// Meshy's <c>Female_Stand_Pick_Fruit_Basket</c>, 6.17 s.
    /// </summary>
    public static readonly RaceAnimation FemaleStandPickFruitBasket = new("kr_female_stand_pick_fruit_basket", "Female_Stand_Pick_Fruit_Basket", 6.167f, Technique(468));

    /// <summary>
    /// Meshy's <c>Female_Throwing_Stance_Charge</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation FemaleThrowingStanceCharge = new("kr_female_throwing_stance_charge", "Female_Throwing_Stance_Charge", 0.5f, Technique(469));

    /// <summary>
    /// Meshy's <c>Female_Throwing_Stance_Charge_inplace</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation FemaleThrowingStanceChargeInplace = new("kr_female_throwing_stance_charge_inplace", "Female_Throwing_Stance_Charge_inplace", 0.5f, Technique(470));

    /// <summary>
    /// Meshy's <c>Female_Walk_Pick_Put_In_Pocket</c>, 9.50 s.
    /// </summary>
    public static readonly RaceAnimation FemaleWalkPickPutInPocket = new("kr_female_walk_pick_put_in_pocket", "Female_Walk_Pick_Put_In_Pocket", 9.5f, Technique(471));

    /// <summary>
    /// Meshy's <c>RIP</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation RIP = new("kr_rip", "RIP", 2.967f, Technique(472));

    /// <summary>
    /// Meshy's <c>Sailor_Scout</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation SailorScout = new("kr_sailor_scout", "Sailor_Scout", 2.967f, Technique(473));

    /// <summary>
    /// Meshy's <c>Stand_Up1</c>, 8.23 s.
    /// </summary>
    public static readonly RaceAnimation StandUp1 = new("kr_stand_up1", "Stand_Up1", 8.233f, Technique(474));

    /// <summary>
    /// Meshy's <c>Stirring</c>, 2.97 s.
    /// </summary>
    public static readonly RaceAnimation Stirring = new("kr_stirring", "Stirring", 2.967f, Technique(475));

    /// <summary>
    /// Meshy's <c>Sword_Judgment</c>, 4.30 s.
    /// </summary>
    public static readonly RaceAnimation SwordJudgment = new("kr_sword_judgment", "Sword_Judgment", 4.3f, Technique(476));

    /// <summary>
    /// Meshy's <c>Sword_Parry</c>, 1.80 s.
    /// </summary>
    public static readonly RaceAnimation SwordParry = new("kr_sword_parry", "Sword_Parry", 1.8f, Technique(477));

    /// <summary>
    /// Meshy's <c>Sword_Parry_Backward</c>, 1.30 s.
    /// </summary>
    public static readonly RaceAnimation SwordParryBackward = new("kr_sword_parry_backward", "Sword_Parry_Backward", 1.3f, Technique(478));

    /// <summary>
    /// Meshy's <c>Sword_Parry_Backward_1</c>, 1.37 s.
    /// </summary>
    public static readonly RaceAnimation SwordParryBackward1 = new("kr_sword_parry_backward_1", "Sword_Parry_Backward_1", 1.367f, Technique(479));

    /// <summary>
    /// Meshy's <c>Sword_Parry_Backward_2</c>, 0.50 s.
    /// </summary>
    public static readonly RaceAnimation SwordParryBackward2 = new("kr_sword_parry_backward_2", "Sword_Parry_Backward_2", 0.5f, Technique(480));

    /// <summary>
    /// Meshy's <c>Sword_Parry_Backward_3</c>, 0.80 s.
    /// </summary>
    public static readonly RaceAnimation SwordParryBackward3 = new("kr_sword_parry_backward_3", "Sword_Parry_Backward_3", 0.8f, Technique(481));

    /// <summary>
    /// Meshy's <c>Sword_Parry_Backward_4</c>, 0.70 s.
    /// </summary>
    public static readonly RaceAnimation SwordParryBackward4 = new("kr_sword_parry_backward_4", "Sword_Parry_Backward_4", 0.7f, Technique(482));

    /// <summary>
    /// Meshy's <c>Sword_Parry_Backward_5</c>, 3.00 s.
    /// </summary>
    public static readonly RaceAnimation SwordParryBackward5 = new("kr_sword_parry_backward_5", "Sword_Parry_Backward_5", 3f, Technique(483));

    /// <summary>
    /// Meshy's <c>Sword_and_Shield_Alert_Turn_Left</c>, 1.30 s.
    /// </summary>
    public static readonly RaceAnimation SwordAndShieldAlertTurnLeft = new("kr_sword_and_shield_alert_turn_left", "Sword_and_Shield_Alert_Turn_Left", 1.3f, Technique(484));

    /// <summary>
    /// Meshy's <c>Sword_and_Shield_Alert_Turn_Right</c>, 3.30 s.
    /// </summary>
    public static readonly RaceAnimation SwordAndShieldAlertTurnRight = new("kr_sword_and_shield_alert_turn_right", "Sword_and_Shield_Alert_Turn_Right", 3.3f, Technique(485));

    /// <summary>
    /// Meshy's <c>Talk_with_Right_Hand_Open</c>, 3.70 s.
    /// </summary>
    public static readonly RaceAnimation TalkWithRightHandOpen = new("kr_talk_with_right_hand_open", "Talk_with_Right_Hand_Open", 3.7f, Technique(486));

    /// <summary>
    /// Meshy's <c>Tightrope_Walk_inplace</c>, 5.17 s.
    /// </summary>
    public static readonly RaceAnimation TightropeWalkInplace = new("kr_tightrope_walk_inplace", "Tightrope_Walk_inplace", 5.167f, Technique(487));

    /// <summary>
    /// Meshy's <c>Husbandry_Training</c>, 4.97 s.
    /// </summary>
    public static readonly RaceAnimation HusbandryTraining = new("kr_husbandry_training", "Husbandry_Training", 4.967f, Technique(488));

    /// <summary>
    /// Meshy's <c>Husbandry_Training_Advanced</c>, 4.97 s.
    /// </summary>
    public static readonly RaceAnimation HusbandryTrainingAdvanced = new("kr_husbandry_training_advanced", "Husbandry_Training_Advanced", 4.967f,
        Technique(489));

    /// <summary>
    /// Every clip, in id order.
    /// </summary>
    public static IReadOnlyList<RaceAnimation> All { get; } =
    [
        PowerSpinJump360, Alert, AllNightDance, AngryGroundStomp, AngryGroundStomp2, ArcheryShot1, AxeSpinAttack, AxeStance, BackJump,
        BallThrow, BaseballPitching, BicepCurl, BicycleCrunch, BigHeartGesture, Block1, Block2, Breakdance1990,
        CarryHeavyObjectWalkInplace, CasualWalk, ChargedSpellCast1, CircleCrunch, ClimbAttemptAndFall4, ClimbAttemptAndFall5,
        ClimbRightWithBothLimbs, ClimbRightWithBothLimbsInplace, ClimbStairs, ClimbUpRope, ConfidentStrut, Counterstrike, FastLightning,
        FemaleBendOverPickUpInspect, FlirtyStrutInplace, FunkyWalk, GentlemansBow, JazzHandsInplace, JumpingJacks, KungFuPunch,
        MonsterWalk, ProneReload, Running, SitCrossLeggedOnFloor, StandAndChat, StandAndDrink, StandOnPoleAndBalance, StandToSideLying,
        StandToSitTransitionM, StepToSitTransition, SwimForward, SwimIdle, SwordShout, TalkWithHandsOpen, TextingWalk, VictoryFistPump,
        WalkBackwardWithGunInplace, Walking, Walking2Inplace, WalkingWoman, WallPushJumpAndFlip, WaveForHelp3, Attack, Block6, Block9,
        BoxingGuardPrepStraightPunch, BroadswordLeft, BroadswordRight, BroadswordThrust, BroadswordTop, BroadswordTop2,
        ChargedSlash, MageSoellCast6, ReapingSwing, RightHandSwordSlash,
        LeftShortHookFromGuard, LeftSlash, LeftUppercutFromGuard, RightUpperHookFromGuard, RightUppercutFromGuard, WalkBackwardInplace,
        WalkBackwardWithGrenadeInplace, WalkFightForward, WalkForwardWhileShooting, WalkForwardWithBowAimed, WalkLeftWithGun,
        WalkWithUmbrella, WalkWithUmbrellaInplace, WeaponCombo, WeaponCombo1, WeaponCombo2,
        AgreeGesture, AirSquat, ClimbAttemptAndFall2, ClimbLeftWithBothLimbs, CollectObject, ConfusedScratch, CrouchAndStepBack, CrouchPullAndThrow, DiscussWhileMoving, DiveDownAndLand, DiveDownAndLand1, DiveDownAndLand2, ElderlyShakyWalk, ParkourVault1, ParkourVault2, ParkourVault3, ParkourVaultWithRoll, PenguinWalk, PersonalizedGesture,
        WaveForHelp, WaveForHelp1, WaveForHelp2, WaveForHelp4, WaveOneHand,
        RidingYeehaw, RidingCalm, SideLyingReachHelp, SitDodge, SitAndDozeOff, SitAndDrink, SitOnChairArmsCrossed, Skill01, Skill02,
        Skill03, SkipForward, TouchAndRun, TripleComboAttack, TwoHandedParry, UnarmedVault, UpsideDownRopeHang, UpsideDownRopeHang1,
        UpsideDownRopeHang2, VaultAndLand, VaultWithRifle, VictoryCheer, WakeUpAndLookUp, Victory,
        ArcheryAimWithLateralScan, ArcheryShot, ArcheryShot2, ArcheryShot3, ArmCircleShuffle, Backflip, BackflipJump, BackflipSweepKick,
        BackflipAndHooks, BeHitFlyUp, CarryHeavyCannonForward, CarryWaterBucketWalk, Cartographer, CatchingBreath, ChargedGroundSlam, ChargedSpellCast,
        ChargedUpwardSlash, CheckoutGesture, ChoppingVegetables, CowboyQuickDrawShooting, DrawAndShootLeft, DrawAndShootFromBack, DrawAndShootFromBack1, DrawAndShootFromBack2,
        Fall2, FemaleBowChargeLeftHand, FemaleBowChargeLeftHandInplace, FemaleCrouchPickFruitBasketStand, FemaleCrouchPickGunPointForward, FemaleCrouchPickThrowForward, FemaleCrouchPickUpPlaceSide, FemaleHeadDownCharge,
        FemaleRunForwardPickUpRight, FemaleStandPickFruitBasket, FemaleThrowingStanceCharge, FemaleThrowingStanceChargeInplace, FemaleWalkPickPutInPocket, RIP, SailorScout, StandUp1,
        Stirring, SwordJudgment, SwordParry, SwordParryBackward, SwordParryBackward1, SwordParryBackward2, SwordParryBackward3, SwordParryBackward4,
        SwordParryBackward5, SwordAndShieldAlertTurnLeft, SwordAndShieldAlertTurnRight, TalkWithRightHandOpen, TightropeWalkInplace,
        HusbandryTraining, HusbandryTrainingAdvanced
    ];

    /// <summary>
    /// The clip of that name (<c>kr_ball_throw</c>), null for none.
    /// </summary>
    public static RaceAnimation? Named(string name) => All.FirstOrDefault(a => a.Name == name);

    /// <summary>
    /// A clip's COMBAT_TECHNIQUE id.
    /// </summary>
    static string Technique(int number) => $"{number}-{Races.FileName}";
}
