<a id="animations"></a>
# Animazione

Kenshi Race ha una libreria di animazione e di animazione: ogni clip dei modelli sorgente delle razze (MeshyLa libreria di animazione, 59 clip), rivolto ai skeletti maschili e femminili del gioco, in modo che qualsiasi personaggio su questi skeletti possa giocare, gli esseri umani, gli inverni e gli skeletti nonché le corse adatte. I modi li giocano attraverso Kenshi Racchi API ([depending-on-kenshi-races.md](depending-on-kenshi-races.md#6-play-its-animations)).

<a id="files-and-records"></a>
## File e registri

- `KenshiRaces/items/animations/kenshiraces_anims_male.skeleton` e `_female.skeleton`: ciascuno inizia con il suo game skeleton's bone byte per
Bytes (le tracce di un file di animazione sono applicate con la maniglia, e le maniglie della femminile differiscono), poi i clip, 30 chiavi al secondo.
- ANIMATION_FILE `40-KenshiRaces.mod` (`AnimationRecords.File`I nomi di entrambi. La fase dei dati lo collega al `animation files` di ogni corsa
di cui `male mesh` è sotto `data/character/meshes/` (a meno che la Regina Hive, che ha il proprio scheletro) o è un corpo adatto (`/items/races/`) e `RaceRecords.FillRace` L'unisce in nuove subrasie, anche di un dipendente.
- Un disabile COMBAT_TECHNIQUE per il clip (`100-` per `158-KenshiRaces.mod`Il nome della Commissione, `anim name` = il clip: disabile, quindi nessuna battaglia lo picchia;
un mod lo riproduce come un movimento di combattimento mod (motore docs/modding.md“Il combattimento si muove”.

<a id="making-them"></a>
## Fare loro

`tools/AnimationExport` Con A [Il laboratorio](https://github.com/Meitou-Modding-Ecosystem/PZAKenshiMotionLab) Il clan (`MOTIONLAB`Il default `../PZAKenshiMotionLab`e il gioco `data/character/meshes`:

1. `python tools/AnimationExport/split.py <model.glb> <clips folder>` per modello di fonte: una riferimento per clip, `Bip001` Le associazioni rinnovate a
I ruoli di Mixamo sono i luoghi che il Motion Lab legge. Un modello successivo sovracrita un clip un precedente dato; il maschio LeomaE’ l’ultimo.
2. Per sesso, `python -m motionlab rig human_<gender>.mesh --skeleton <gender>_skeleton.skeleton -o <out> --clips idle --referenza
<clip>.glb ...` (ogni clip): li ritorna sul corpo del gioco (circa un minuto per 59).
3. di Python tools/AnimationExport/export.py <out>/human_<gender>_rigged.glb <gender>/skeleton.skeleton
KenshiRaces/items/animations/kenshiraces_anims_<gender>.skeleton <clips>.json`: names them `kr_<clicca il caso inferiore>` e scrive il file.

Ogni clip gioca in posizione: dove un clip di origine si muove (la colonna di viaggio), il viaggio di terra è preso come linea retta, e il bob è tenuto. Il gioco si svolgerà con la corona di ipoti e il bingo di -90°, mentre i propri idoli del gioco (**Verificato**, `idle_stand_normal` In `male_skeleton.skeleton`). Un nuovo clip ha anche bisogno della sua linea `RaceAnimations`, con il prossimo ID tecnico gratuito (159 su), e la sua sequenza qui.

<a id="clips"></a>
## Clipi

| Clip | `RaceAnimations.` | Tecnica | La fonte | Seconda | Viaggio rimosso (m) |
| --- | --- | --- | --- | --- | --- |
| `kr_360_power_spin_jump` | `PowerSpinJump360` | `100-` | _360_Power_Spin_Raggiunta | 3.00 | 2.5 |
| `kr_alert` | `Alert` | `101-` | Allarme | 4.00 |  |
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
| `kr_block1` | `Block1` | `114-` | Il blocco 1 | 3.47 |  |
| `kr_block2` | `Block2` | `115-` | Il blocco 2 | 1.60 |  |
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
| `kr_counterstrike` | `Counterstrike` | `128-` | Controversa | 6.50 |  |
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
| `kr_running` | `Running` | `139-` | corsa | 0.60 |  |
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
| `kr_walking` | `Walking` | `154-` | camminare | 1.00 |  |
| `kr_walking_2_inplace` | `Walking2Inplace` | `155-` | walking_2_inplace | 1.17 |  |
| `kr_walking_woman` | `WalkingWoman` | `156-` | Walking_Woman | 0.97 |  |
| `kr_wall_push_jump_and_flip` | `WallPushJumpAndFlip` | `157-` | Wall_Push_Jump_and_Flip | 2.00 | 1.3 |
| `kr_wave_for_help_3` | `WaveForHelp3` | `158-` | Wave_for_Help_3 | 3.17 |  |
