namespace KenshiRaces;

/// <summary>
/// The animation library (docs/animations.md): every clip of the bodies' source models, retargeted onto the game's male and female skeletons and
/// played in place. Each is a disabled COMBAT_TECHNIQUE whose <c>anim name</c> is the clip, so a mod plays it as a mod combat move
/// (<c>ModContext.AddCombatMove</c>, <see cref="AnimationRecords.Technique"/>); the clips are in the ANIMATION_FILE
/// <see cref="AnimationRecords.File"/>, linked to every race on the game's human skeletons. Generated from the export's clip list; ids
/// <c>100-</c> to <c>158-KenshiRaces.mod</c> in name order, new clips taking the next free id.
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
        WalkBackwardWithGunInplace, Walking, Walking2Inplace, WalkingWoman, WallPushJumpAndFlip, WaveForHelp3
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
