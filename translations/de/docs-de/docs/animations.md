<a id="animations"></a>
# Animationen

Kenshi Races liefert eine Animationsbibliothek: Jeder Clip der Quellmodelle der Rassen (Animationsbibliothek von Meshy , 59 Clips), neu ausgerichtet auf die männlichen und weiblichen Skelette des Spiels, sodass jeder Charakter auf diesen Skeletten es spielen kann, Menschen, Shek, Hivers und Skelette sowie die angepassten Rassen. Mods spielen sie über Kenshi Races ' API ([abhängig von kenshi-races.md](depending-on-kenshi-races.md#6-play-its-animations) ).

<a id="files-and-records"></a>
## Dateien und Aufzeichnungen

- `KenshiRaces/items/animations/kenshiraces_anims_male.skeleton` und `_female.skeleton` : Jeder beginnt Byte für Byte mit den Knochen des Spielskeletts (die Spuren einer Animationsdatei werden durch den Knochengriff bestimmt, und die Griffe des weiblichen Rigs unterscheiden sich), dann mit den Clips, 30 Tasten pro Sekunde.
- ANIMATION_FILE  `40-KenshiRaces.mod` ( `AnimationRecords.File` ) benennt beide. Die Datenphase verknüpft es mit dem `animation files` jeder Rasse, deren `male mesh` unter `data/character/meshes/` steht (mit Ausnahme der Hive Queen, die ihr eigenes Skelett hat) oder ein angepasster Körper ist ( `/items/races/` ), und `RaceRecords.FillRace` verknüpft es mit neuen Unterrassen, auch einem abhängigen.
- Ein deaktivierter COMBAT_TECHNIQUE pro Clip ( `100-` bis `158-KenshiRaces.mod` , Namensreihenfolge), `anim name` = der Clip: deaktiviert, daher wird er von keinem Kampf ausgewählt; Ein Mod spielt es als Mod-Kampfzug (engine docs/modding.md, „Combat Moves“).

<a id="making-them"></a>
## Wir machen sie

`tools/AnimationExport` mit einem [ Motion Lab ](https://github.com/Meitou-Modding-Ecosystem/PZAKenshiMotionLab) -Klon ( `MOTIONLAB` , Standard `../PZAKenshiMotionLab` ) und dem `data/character/meshes` des Spiels:

1. `python tools/AnimationExport/split.py <model.glb> <clips folder>` pro Quellmodell: eine Referenz pro Clip, `Bip001` -Verbindungen wurden in die Mixamo-Rollen umbenannt, die der Motion Lab liest. Ein späteres Modell überschreibt einen Clip, den ein früheres Modell lieferte; der männliche Leoma kommt als Letzter.
2. Für jedes Geschlecht richtet `python -m motionlab rig human_<gender>.mesh --skeleton <gender>_skeleton.skeleton -o <out> --clips idle --reference <clip>.glb ...` (jeder Clip) sie erneut auf den Körper des Spiels aus (ungefähr eine Minute für 59).
3. `python tools/AnimationExport/export.py <out>/human_<gender>_rigged.glb <gender>_skeleton.skeleton KenshiRaces/items/animations/kenshiraces_anims_<gender>.skeleton <clips>.json` : Benennt sie als `kr_<clip lower case>` und schreibt die Datei.

Jeder Clip wird an der richtigen Stelle abgespielt: Dort, wo ein Quellclip zurückgelegt hat (Spalte „Reise“, Meter), wird die Bodenbewegung der Wurzel als gerade Linie herausgenommen, wobei Schwankung und Pendelbewegung beibehalten werden. Alle blicken in die Vorwärtsrichtung des Spiels: Das Gieren der Wurzel stimmt mit den -90° der Bindungshaltung überein, wie es auch bei den Leerläufen des Spiels der Fall ist (**Verifiziert**, `idle_stand_normal` in `male_skeleton.skeleton` ). Ein neuer Clip benötigt außerdem seine Zeile in `RaceAnimations` mit der nächsten freien Technik-ID (159 auf) und seine Zeile hier.

<a id="clips"></a>
## Clips

| Clip | `RaceAnimations.` | Technik | Quelle | Sekunden | Weg entfernt (m) |
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
