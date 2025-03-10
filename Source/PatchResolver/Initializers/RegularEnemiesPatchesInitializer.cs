﻿using BossChallengeMod.BossPatches;
using BossChallengeMod.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.PatchResolver.Initializers {
    public class RegularEnemiesPatchesInitializer : PatchesResolverInitializerBase {
        protected CustomMonsterStateValuesResolver monsterStateValuesResolver;
        protected List<string> doNotPatch = [
            "A1_S1_GameLevel/Room/A1_S1_Tutorial_Logic/StealthGameMonster_Minion_Tutorial1",
            "A1_S1_GameLevel/Room/A1_S1_Tutorial_Logic/StealthGameMonster_Minion_Tutorial2",
            "A1_S1_GameLevel/Room/GamePlayS2_1/TrapMonster_Mutant Mini",
            "A1_S1_GameLevel/Room/GamePlayS2_1/TrapMonster_Mutant Mini (3)",
            "A1_S1_GameLevel/Room/GamePlayS3_2/TrapMonster_Mutant (6)",
            "TrapMonster_BlackHole(Clone)",
            "TrapMonster_LaserAltar_Circle(Clone)",
            "TrapMonster_Altar_Health_Drop(Clone)",
            "TrapMonster_Altar_Energy_Drop(Clone)",
            "A1_S2_GameLevel/Room/Prefab/寶箱 Treasure Chests/EventBinder 小錢袋/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A3_S5_BossGouMang_GameLevel/Room/StealthGameMonster_BossZombieSpear",
            "A3_S5_BossGouMang_GameLevel/Room/StealthGameMonster_BossZombieHammer",
            "A0_S4 gameLevel/Room/軒軒野豬情境OnOff FSM/FSM Animator/LogicRoot/[On]Node/CullingGroup/SimpleCutSceneFSM_EncounterBoar (開頭介紹野豬的演出)/FSM Animator/LogicRoot/After/LootProvider/StealthGameMonster_RunningHog (1)",
            "AG_S1/Room/Prefab/GameMonster_Turret_Danger_Bullet 砲台 單向門",
            "AG_S1/Room/Prefab/Simple Binding Tool/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A7_S1/Room/Prefab/Simple Binding Tool/StealthGameMonster_Statue_Shield Variant_M",

            #region Lear
            "AG_LeeEar_S0/Room/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_SpearmanElite 長槍菁英",
            "AG_LeeEar_S0/Room/Prefab/StealthGameMonster_TutorialDummy Variant",
            "AG_LeeEar_S0/Room/Prefab/StealthGameMonster_TutorialDummy Variant (1)",
            "AG_LeeEar_S0/StealthGameMonster_ZombieSlow 天禍 爆炸 (1)",
            "AG_LeeEar_S1/Room/StealthMonster_GiantBlade_FireBlade",
            "AG_LeeEar_S1/StealthGameMonster_TutorialDummy Variant",
            "AG_LeeEar_S1/Room/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_SpearmanElite 長槍菁英",
            "AG_LeeEar_S1/StealthGameMonster_ZombieSlow 天禍 爆炸 (1)",

            "AG_LeeEar_S1/Room/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_Spearman_withShield",
            "AG_LeeEar_S1/Room/Prefab/Dragon_Snake_withShield_小心沒有Variant到",
            "AG_LeeEar_S1/Room/Prefab/Dragon_Snake_withShield_小心沒有Variant到 (1)",

            "AG_LeeEar_S1/Room/Prefab/CaveStatue_Counter Variant",
            "AG_LeeEar_S1/Room/Prefab/CaveStatue_Laser Variant",
            "AG_LeeEar_S1/Room/Prefab/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/[古代繼承] CaveStatue_Counter 站在外面",
            "AG_LeeEar_S1/Room/Prefab/[觸發框架] (1)/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/[古代繼承] CaveStatue_Laser Variant",
            "AG_LeeEar_S1/Room/Prefab/StealthGameMonster_Statue_Counter Variant (1)",
            "AG_LeeEar_S1/Room/Prefab/StealthGameMonster_LaserStatue",
            "AG_LeeEar_S1/Room/StealthGameMonster_Statue_Counter Variant_M",
            "AG_LeeEar_S1/Room/GameMonster_Turret_Danger_Bullet 砲台",
            #endregion Lear

            #region A2_S6
            "A2_S6/Room/Prefab/寶箱 Chests/EventBinder 文房四寶/StealthGameMonster_Statue_Parry Variant_M",
            "A2_S6/Room/Prefab/寶箱 Chests/EventBinder 文房四寶/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup (1)/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup (2)/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup (3)/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup (4)/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup (5)/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup (6)/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version2]/[觸發框架]砲台版 (2)/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay1/警報 On / Off 差別/General FSM Object_On And Off Switch Variant/FSM Animator/LogicRoot/[Off]Node/[Version1]/[觸發框架]砲台版 (1)/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/Gameplay2/GameMonster_AirTurrent_ReflectAndInstantKill",
            "A2_S6/Room/Prefab/Gameplay2/TrapMonster_Broken Robot mini (2)",
            "A2_S6/Room/Prefab/Gameplay4/TrapMonster_Broken Robot mini (1)",
            "A2_S6/Room/Prefab/Gameplay4/GameMonster_AirTurrent_ReflectAndInstantKill",
            "A2_S6/Room/Prefab/Gameplay4/TrapMonster_Mutant (4)",
            "A2_S6/Room/Prefab/Gameplay5/TrapMonster_Broken Robot mini (1)",
            "A2_S6/Room/Prefab/Gameplay5/TrapMonster_Mutant (3)",
            "A2_S6/Room/Prefab/Gameplay5/GameMonster_AirTurrent_ReflectAndInstantKill (1)",
            "A2_S6/Room/Prefab/Gameplay5/TrapMonster_Mutant (4)",
            "A2_S6/Room/Prefab/Gameplay6/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A2_S6/Room/Prefab/Gameplay6/StealthGameMonster_Statue_DangerJumpKickStatue_M (1)",
            "A2_S6/Room/Prefab/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S6/Room/Prefab/GameMonster_Turrent_Danger_Bullet",
            "A2_S6/Room/Prefab/GameMonster_Turrent_Danger_Bullet (2)",
            #endregion A2_S6

            #region A2_S2

            "A2_S2/Room/Prefab/寶箱 Chests/EventBinder 小錢袋/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A2_S2/Room/Prefab/寶箱 Chests/LootProvider 收金玉/Chest Runner SpiderMine Spawner 盜寶哥布林 不丟東西 (1)"

            #endregion A2_S2

            #region A2_S1

            ,"A2_S1/Room/Prefab/寶箱 Chests 右/EventBinder 中錢袋/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A2_S1/Room/Prefab/Gameplay1/[Mech]LockedByMonster/Monsters/StealthGameMonster_Statue_DangerJumpKickStatue",
            "A2_S1/Room/Prefab/Gameplay5/TrapMonster_Broken Robot",

            #endregion A2_S1

            #region A2_S3

            "A2_S3/Room/Prefab/寶箱 Chests/EventBinder 中錢袋/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A2_S3/Room/Prefab/DroneTunnel_Entrance/StealthGameMonster_Turrent Variant",
            "A2_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet (1)",
            "A2_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet",

            #endregion A2_S3

            #region A2_SG1

            "A3_SG1/Room/Prefab/EventBinder/StealthGameMonster_Statue_Shield Variant_M",
            "A3_SG1/Room/Prefab/EventBinder/StealthGameMonster_Statue_Shield Variant_M (1)",

            #endregion A2_SG1

            #region A3_S2

            "A3_S2/Room/Prefab/LockedByMonster/Monsters/StealthGameMonster_Statue_Shield Variant_M",

            #endregion A3_S2

            #region A3_S5

            "A3_S5_BossGouMang_GameLevel/Room/StealthGameMonster_BossZombieHammer",
            "A3_S5_BossGouMang_GameLevel/Room/StealthGameMonster_BossZombieSpear",
            "TrapMonster_ZombieBone(Clone)",

            #endregion A3_S5

            #region A5_S1

            "A5_S1/Room/Prefab/GameMonster_Turrent_Danger_Bullet 砲台 單向門",
            "A5_S1/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill",
            "A5_S1/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (1)",
            "A5_S1/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (2)",
            "A5_S1/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (3)",

            #endregion

            #region A1_S3

            "A1_S3_GameLevel/Room/Prefab/寶箱 Chests/EventBinder 卸力玉/StealthGameMonster_Statue_Parry Variant_M (1)",
            "A1_S3_GameLevel/Room/Prefab/寶箱 Chests/EventBinder 卸力玉/StealthGameMonster_Statue_Parry Variant_M",
            "A1_S3_GameLevel/Room/Prefab/寶箱 Chests/EventBinder 卸力玉/StealthGameMonster_Statue_DangerJumpKickStatue_M",

            #endregion A1_S3

            #region A6_S1

            "A6_S1/Room/Prefab/寶箱 Chests/StatueTreasureEventBinder 調息玉 /StealthGameMonster_Statue_Shield Variant_M",
            "A6_S1/Room/Prefab/寶箱 Chests/StatueTreasureEventBinder 調息玉 /StealthGameMonster_Statue_Parry Variant_M",

            #endregion A6_S1

            #region A4_S1

            "A4_S1/Room/Prefab/Gameplay_3/左下開電/PowerOnFSM/FSM Animator/LogicRoot/PowerControl_Obj/GameMonster_Turrent_Danger_Bullet",
            "A4_S1/Room/Prefab/埋伏陷阱Gameplay/GameMonster_Turrent_Danger_Bullet (2)",
            "A4_S1/Room/Prefab/埋伏陷阱Gameplay/GameMonster_Turrent_Danger_Bullet (3)",
            "A4_S1/Room/Prefab/GameMonster_Turrent_Danger_Bullet (1)",
            "A4_S1/Room/Prefab/GameMonster_Turrent_Danger_Bullet (2)",
            "A4_S1/Room/Prefab/GameMonster_Turrent_Danger_Bullet (3)",
            "A4_S1/Room/Prefab/Shield Giant Bot Control Provider Variant/StealthGameMonster_Shield Bot Giant (1)",

            #endregion A4_S1

            #region A4_S2

            "A4_S2/Room/Prefab/寶箱 Chests/LootProvider 大錢袋/Chest Runner SpiderMine Spawner 盜寶哥布林 丟蜘蛛雷",
            "A4_S2/Room/Prefab/寶箱 Chests/EventBinder 藥草催化器/e_Parry Variant_Mr_Statue_Parry Variant_M",
            "A4_S2/Room/Prefab/寶箱 Chests/EventBinder 藥草催化器/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A4_S2/Room/Prefab/寶箱 Chests/EventBinder 藥草催化器/StealthGameMonster_Statue_DangerJumpKickStatue_M",
            "A4_S2/Room/StealthGameMonster_Shield Totem",
            "A4_S2/Room/StealthGameMonster_Shield Totem (1)",
            "A4_S2/Room/StealthGameMonster_Shield Totem (2)",

            #endregion A4_S2

            #region A4_S4

            "A4_S4/ZGunAndDoor/Shield Giant Bot Control Provider Variant_Cutscene/StealthGameMonster_Shield Bot Giant (1)",
            "A4_S4/Room/Prefab/Room_1/Enemy/TrapMonster_Broken Robot mini (2)",
            "A4_S4/Room/Prefab/Room_3/Enemy/TrapMonster_Broken Robot mini (1)",
            "A4_S4/Room/Prefab/Room_3/Enemy/StealthGameMonster_Shield Totem",
            "A4_S4/Room/Prefab/Room_4/Enemy/StealthGameMonster_Shield Totem",

            #endregion A4_S4

            #region A5_S2 (Jail)

            "A5_S2/Room/Prefab/上半存檔點後/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/TrapMonster_Robot_Crawler (1)",
            "A5_S2/Room/Prefab/上半存檔點後/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/TrapMonster_Robot_Crawler",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/StealthGameMonster_Minion_prefab (1)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver (1)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver (3)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver (4)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario(看守的人)/StealthGameMonster_Minion_prefab_A5_S2_Low_Eyesight Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario(看守的人)/StealthGameMonster_GunBoy_A5_S2_Low_Eyesight Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab (1)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthMonster_EyeObserver (4)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthMonster_EyeObserver (5)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/1F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Dog Mrx_A5_S2_Low_Eyesight Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/2F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario(看守的人)/StealthGameMonster_Minion_prefab (4)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/2F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Dog Mrx_A5_S2_Low_Eyesight Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/2F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab (3)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/2F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab (2)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/2F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthMonster_EyeObserver (3)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/2F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab_A5_S2_Low_Eyesight Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/2F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthMonster_EyeObserver (1)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab_A5_S2_Low_Eyesight Variant (1)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_GunBoy_A5_S2_Low_Eyesight Variant Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver (1)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver (2)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/EngagingGroup/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab_A5_S2_Low_Eyesight Variant (3)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/EngagingGroup/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab_A5_S2_Low_Eyesight Variant (5)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/EngagingGroup/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Spearman_A5_S2_Low_Eyesight Variant Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthMonster_EyeObserver (3)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver (1)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthMonster_EyeObserver (2)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/EngagingGroup/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab_A5_S2_Low_Eyesight Variant (3)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/EngagingGroup/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Minion_prefab_A5_S2_Low_Eyesight Variant (5)",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/EngagingGroup/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Spearman_A5_S2_Low_Eyesight Variant Variant",
            "A5_S2/Room/Prefab/MonsterGroup_AnyEngaging_A5_S2叫典獄長 Variant/Monsters/3F Gameplay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Dog Mrx_A5_S2_Low_Eyesight Variant",
            "A5_S2/Room/Prefab/TrapMonster_Robot_Crawler (3)",
            "A5_S2/Room/Prefab/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/TrapMonster_Robot_Crawler (1)",
            "A5_S2/Room/JailBossRoom/LootProvider/Boss Fight FSM MrX/FSM Animator/LogicRoot/---Boss---/StealthGameMonster_MrX_TrueBody_Room Variant",
            "StealthGameMonster_MrX_FlyingShooter 典獄飛兵(Clone)"

            #endregion A5_S2 (Jail)

            #region A5_S3   

            ,"A5_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet (1)",
            "A5_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet 砲台 單向門 (3)",
            "A5_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet (5)",
            "A5_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet 砲台 單向門 (5)",
            "A5_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet 砲台 單向門 (6)",
            "A5_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet 砲台 單向門 (7)",
            "A5_S3/Room/Prefab/寶箱 Chests/LootProvider 大錢袋/Chest Runner SpiderMine Spawner 盜寶哥布林 丟蜘蛛雷",
            "A5_S3/Room/Prefab/StealthGameMonster_Shield Totem (2)",
            "A5_S3/Room/Prefab/StealthGameMonster_Shield Totem (3)",
            "A5_S3/Room/Prefab/GameMonster_Turrent_Danger_Bullet 砲台 單向門 (4)",


            #endregion A5_S3

            #region A5_S4

            "StealthGameMonster_ZombieGrabber(Clone)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (1)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (1)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (10)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (10)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (15)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (15)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (16)",
            "A5_S4/Room/Prefab/StealthGameMonster_ZombieGrabber (17)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (9)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (9)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (11)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (12)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (12)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (13)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (13)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (19)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (21)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (22)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieGrabber (23)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (4)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (7)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (2)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (3)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (5)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (6)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (8)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (14)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (18)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (4)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (7)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (2)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (3)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (5)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (6)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (8)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (14)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (18)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (20)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (19)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (21)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (22)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (23)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (24)",
            "A5_S4/Room/Prefab/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieGrabber (17)",
            "A5_S4/Room/GameMonster_AirTurrent_ReflectAndInstantKill",
            "A5_S4/Room/GameMonster_AirTurrent_ReflectAndInstantKill (1)",
            "A5_S4/Room/GameMonster_AirTurrent_ReflectAndInstantKill (2)",
            "A5_S4/神像Gameplay/StealthGameMonster_Statue_Shield Variant_M",
            "A5_S4/神像Gameplay/StealthGameMonster_Statue_Counter Variant_M (2)",

            #endregion A5_S4

            #region A6_S3

            "A6_S3/Room/Prefab/寶箱  Chests/LootProvider 快攻玉/Chest Runner SpiderMine Spawner 盜寶哥布林 丟蜘蛛雷",

            #endregion A6_S3

            #region A7_S2

            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part1/Period Cycle Acitvate Group (2)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part1/Period Cycle Acitvate Group (6)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (1)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part1/Period Cycle Acitvate Group (4)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (2)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part1/Period Cycle Acitvate Group (7)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (4)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part1/Period Cycle Acitvate Group (7)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (5)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part1/StealthMonster_ParrallelUniverse_FollowingGhost",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part1/StealthMonster_ParrallelUniverse_FollowingGhost (1)",

            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (7)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (7)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (1)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (6)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (7)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (6)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (2)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (6)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (3)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (6)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (4)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (8)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (6)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (8)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (5)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (8)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (6)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (10)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (5)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/Period Cycle Acitvate Group (10)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (6)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/StealthMonster_ParrallelUniverse_FollowingGhost (1)",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/StealthMonster_ParrallelUniverse_FollowingGhost (2)",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/StealthMonster_ParrallelUniverse_FollowingGhost (3)",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part2/StealthMonster_ParrallelUniverse_FollowingGhost (4)",

            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part3/Period Cycle Acitvate Group/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (10)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part3/Period Cycle Acitvate Group (2)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part3/Period Cycle Acitvate Group (1)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (11)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part3/StealthMonster_ParrallelUniverse_FollowingGhost",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part3/StealthMonster_ParrallelUniverse_FollowingGhost (1)",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part3/StealthMonster_ParrallelUniverse_FollowingGhost (2)",

            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (1)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (13)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (1)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (14)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (1)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (15)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (1)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (16)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (2)/FSMs/Period Cycle FSM_Shoot Danger Projectile Variant (12)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/StealthGameMonster_Minion_A7_LightMask Variant/[Monsters&Interactables]/StealthGameMonster_Minion_A7_LightMask",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/StealthGameMonster_Minion_A7_LightMask Variant (1)/[Monsters&Interactables]/StealthGameMonster_Minion_A7_LightMask",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (7)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (1)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (7)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/Period Cycle Acitvate Group (7)/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (2)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/StealthGameMonster_Minion_A7_LightMask Variant (2)/[Monsters&Interactables]/StealthGameMonster_Minion_A7_LightMask",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/StealthMonster_ParrallelUniverse_FollowingGhost",
            "A2_Stage_Remake/Room/Prefab/FallingTeleportTrickBackgroundProvider/Part4/StealthMonster_ParrallelUniverse_FollowingGhost (1)",

            "A7_ButterflyTest/StealthGameMonster_Minion_A7_LightMask Variant/[Monsters&Interactables]/StealthGameMonster_Minion_A7_LightMask",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (1)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (7)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (8)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (9)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (10)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (11)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (12)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (13)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (14)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase4/MovingButterfly_4/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Shooter_Black/Period Cycle FSM_Shoot Danger Projectile Variant (15)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/SequencePuzzlePhasesFSM Variant (Note請看筆記 不用開Culling)/FSM Animator/PhasesActivateNodes/Phase6/MovingButterfly_6/FSM Animator/ButterflyControlRoot/AnimatorForPath(關卡設計Override我)/Period Cycle FSM_Shoot Jump Ball Variant (2)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/Period Cycle Acitvate Group/FSMs/Period Cycle FSM_Shoot Jump Ball Variant/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/Period Cycle Acitvate Group/FSMs/Period Cycle FSM_Shoot Jump Ball Variant (1)/FSM Animator/AbstractRoot/StealthGameMonster_DummyMonster_For機關 平行宇宙 Variant",
            "A7_ButterflyTest/Room/Prefab/StealthMonster_ParrallelUniverse_FollowingGhost/StealthMonster_ParrallelUniverse_FollowingGhost",
            "A7_ButterflyTest/Room/Prefab/StealthMonster_ParrallelUniverse_FollowingGhost/StealthMonster_ParrallelUniverse_FollowingGhost (2)",
            "A7_ButterflyTest/Room/Prefab/StealthMonster_ParrallelUniverse_FollowingGhost/StealthMonster_ParrallelUniverse_FollowingGhost (3)",
            "A7_ButterflyTest/Room/Prefab/StealthMonster_ParrallelUniverse_FollowingGhost/StealthMonster_ParrallelUniverse_FollowingGhost (4)",
            "A7_ButterflyTest/Room/Prefab/StealthMonster_ParrallelUniverse_FollowingGhost/StealthMonster_ParrallelUniverse_FollowingGhost (1)",

            #endregion A7_S2

            #region AG_SG1

            "AG_SG1/Room/[Set] CloseDoorFight View&Wave (1)/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/ [0] [關門戰出場情境]GameMonster_Turret_Danger_Bullet 砲台 (2)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/GameMonster_Turret_Danger_Bullet 砲台 (2)",
            "AG_SG1/Room/[Set] CloseDoorFight View&Wave (1)/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/ [1] [關門戰出場情境]GameMonster_Turret_Danger_Bullet 砲台 (3)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/GameMonster_Turret_Danger_Bullet 砲台 (3)",

            #endregion AG_SG1

            #region A10_S3

            "A10_S3/Room/AllTreasure 寶箱 Chests/LootProvider 藥斗功率/Chest Runner SpiderMine Spawner 盜寶哥布林 丟蜘蛛雷 (1)",
            "A10_S3/Room/StealthGameMonster_Shield Totem",
            "A10_S3/Room/Enemy/StealthGameMonster_Shield Totem (2)",
            "NinjaWaterGhostElite_Phantom 殘影(Clone)",
            "StealthGameMonster_NinjaWaterGhost 殘影水鬼的影(Clone)"

            #endregion A10_S3

            #region A10_SG1 Cave 1

            ,"A10_SG1_Cave1/Room/Prefab/A10_SG1_StatueFight_and_HackButtonSet/StatueFight FSM Variant/FSM Animator/LogicRoot/ [0] [關門戰出場情境]CaveStatue_Reflect/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG1_Cave1/Room/Prefab/A10_SG1_StatueFight_and_HackButtonSet/StatueFight FSM Variant/FSM Animator/LogicRoot/ [0] [關門戰出場情境]CaveStatue_Reflect (3)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG1_Cave1/Room/Prefab/A10_SG1_StatueFight_and_HackButtonSet/StatueFight FSM Variant/FSM Animator/LogicRoot/ [0] [關門戰出場情境]CaveStatue_Reflect (5)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG1_Cave1/Room/Prefab/A10_SG1_StatueFight_and_HackButtonSet/StatueFight FSM Variant/FSM Animator/LogicRoot/ [0] [關門戰出場情境]CaveStatue_Reflect (1)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG1_Cave1/Room/Prefab/A10_SG1_StatueFight_and_HackButtonSet/StatueFight FSM Variant/FSM Animator/LogicRoot/ [0] [關門戰出場情境]CaveStatue_Reflect (2)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG1_Cave1/Room/Prefab/A10_SG1_StatueFight_and_HackButtonSet/StatueFight FSM Variant/FSM Animator/LogicRoot/ [0] [關門戰出場情境]CaveStatue_Reflect (4)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG1_Cave1/Room/Prefab/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Statue_ReflectProjectile Variant",

            #endregion A10_SG1 Cave 1

            #region A10_SG2 Cave 2

            "A10_SG2/Room/Prefab/A10_SG2_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG2/FSM Animator/LogicRoot/ [0] [關門戰出場情境]CaveStatue_Parry (1)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/[古代繼承] CaveStatue_Parry Variant",
            "A10_SG2/Room/Prefab/A10_SG2_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG2/FSM Animator/LogicRoot/ [1] [關門戰出場情境]CaveStatue_Parry/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/[古代繼承] CaveStatue_Parry Variant",
            "A10_SG2/Room/Prefab/A10_SG2_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG2/FSM Animator/LogicRoot/ [2] [關門戰出場情境]CaveStatue_Reflect/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG2/Room/Prefab/A10_SG2_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG2/FSM Animator/LogicRoot/ [3] [關門戰出場情境]CaveStatue_Counter/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/[古代繼承] CaveStatue_Counter",
            "A10_SG2/Room/Prefab/A10_SG2_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG2/FSM Animator/LogicRoot/ [4] [關門戰出場情境]CaveStatue_JumpKick/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/[古代繼承] CaveStatue_JumpKick",

            #endregion A10_SG2 Cave 2

            #region A10_SG4

            "A10_SG4/Room/Prefab/A10_SG4_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG4 Variant/FSM Animator 關卡設計/LogicRoot/ [2] [關門戰出場情境]CaveStatue_Laser Variant_2-1/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/[古代繼承] CaveStatue_Laser Variant",
            "A10_SG4/Room/Prefab/A10_SG4_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG4 Variant/FSM Animator 關卡設計/LogicRoot/ [3] [關門戰出場情境]CaveStatue_Laser Variant_2-2/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/[古代繼承] CaveStatue_Laser Variant",
            "A10_SG4/Room/Prefab/A10_SG4_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG4 Variant/FSM Animator 關卡設計/LogicRoot/ [2] [關門戰出場情境]CaveStatue_Reflect/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect",
            "A10_SG4/Room/Prefab/A10_SG4_StatueFight_And_HackButtonSet/StatueFight FSM A10_SG4 Variant/FSM Animator 關卡設計/LogicRoot/ [3] [關門戰出場情境]CaveStatue_Reflect (1)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/CaveStatue_Reflect (1)",

            #endregion A10_SG4

            #region A10_S4

            "A10_S4/Room/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/CaveStatue_Reflect",
            "A10_S4/Room/[觸發框架] (1)/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/CaveStatue_Reflect (1)",
            "StealthGameMonster_NinjaWaterGhostMiniBoss 水鬼小王 AfterImage Variant(Clone)",

            #endregion A10_S4

            #region A9_S1

            "A9_S1/Room/Prefab/GameMonster_Turret_Danger_Bullet 砲台 單向門",
            "A9_S1/Room/Prefab/GameMonster_Turret_Danger_Bullet 砲台",
            "A9_S1/Room/Prefab/Shield Giant Bot Control Provider Variant/StealthGameMonster_Shield Bot Giant (1)",
            "A9_S1/Room/Prefab/AllMonsters/[Set] CloseDoorFight View&Wave (1)/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/ [0] [關門戰出場情境]GameMonster_Turret_Danger_Bullet 砲台 (2)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/GameMonster_Turret_Danger_Bullet 砲台 (2)",
            "A9_S1/Room/Prefab/AllMonsters/[Set] CloseDoorFight View&Wave (1)/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/ [1] [關門戰出場情境]GameMonster_Turret_Danger_Bullet 砲台 (3)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/GameMonster_Turret_Danger_Bullet 砲台 (3)",
            "A9_S1/Room/Prefab/Treasure 寶箱 Chests/StatueEventBinder 大錢袋 & 中量金/StealthGameMonster_Statue_Counter Variant_M (1)",
            "A9_S1/Room/Prefab/Treasure 寶箱 Chests/StatueEventBinder 大錢袋 & 中量金/StealthGameMonster_Statue_Shield Variant_M",
            "SummonEarthSpike 地刺(Clone)",

            #endregion A9_S1

            #region A9_S2

            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (1)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (2)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (3)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (5)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (11)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (12)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (10)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (7)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (8)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (6)",
            "A9_S2/Room/Prefab/GameMonster_AirTurrent_ReflectAndInstantKill (4)",
            "A9_S2/Room/Prefab/寶箱 Chests/LootProvider 大錢袋/Chest Runner SpiderMine Spawner 盜寶哥布林 丟蜘蛛雷 (1)",
            "A9_S2/Room/Prefab/寶箱 Chests/LootProvider 鱉蠍/Chest Runner Shield Crawler Spawner 盜寶哥布林 丟金電蟲 (1)",

            #endregion A9_S2

            #region A9_S3

            "A9_S3/Room/Prefab/GameMonster_Turret_Danger_Bullet 砲台 (4)",
            "A9_S3/Room/Prefab/GameMonster_Turret_Danger_Bullet 砲台 (3)",
            "A9_S3/Room/Prefab/GameMonster_Turret_Danger_Bullet 砲台 (5)",
            "A9_S3/Room/Prefab/Treasure寶箱 Chests/LootProvider 算力元件/Chest Runner SpiderMine Spawner 盜寶哥布林 丟蜘蛛雷 (1)",

            #endregion A9_S3

            #region A9_S5

            "P2_R22_Savepoint_GameLevel/StealthGameMonster_Minion_prefab"

            #endregion A9_S5

            #region A11_S2

            ,"A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[Off]Node/Phase1_普通階段/GameplayRoot/GameMonster_Turret_Danger_Bullet 砲台 (1)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[Off]Node/Phase1_普通階段/GameplayRoot/GameMonster_Turret_Danger_Bullet 砲台 (2)",


            #endregion A11_S2
        ];

        protected List<string> minibossesPatches = [
            "A3_S2/Room/Prefab/Gameplay_8/RCGEventSharingGroup/LootProvider 貪財玉/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_NinjaWaterGhostMiniBoss 水鬼小王 榴彈",
            "AG_SG1/Room/[Set] CloseDoorFight View&Wave (1)/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/ [0] [關門戰出場情境]StealthGameMonster_Double_Axe_Fat/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_Double_Axe_Fat"
            ];

        protected List<string> pseudoMinibossesPatches = [
            "AG_SG1/Room/[Set] CloseDoorFight View&Wave (1)/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/ [0] [關門戰出場情境]StealthGameMonster_Double_Axe_Fat/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_Double_Axe_Fat"
            ];

        protected List<string> mutantsPatches = [

            #region A9_S2

            "A9_S2/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (2)",
            "A9_S2/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (1)",
            "A9_S2/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (3)",
            "A9_S2/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (10)",
            "A9_S2/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (4)",
            "A9_S2/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (13)",
            "A9_S2/Room/Prefab/StealthGameMonster_ZombieChaser_天禍 (1)",
            "A9_S2/Room/Prefab/StealthGameMonster_ZombieSlow 天禍",
            "A9_S2/Room/Prefab/寶箱 Chests/LootProvider 乘客令牌：阿守/StealthGameMonster_ZombieChaser_天禍 (2)",
            "A9_S2/Room/Prefab/寶箱 Chests/LootProvider 乘客令牌：鄒巖/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_ZombieChaser_天禍 (1)",
            "A9_S2/Room/Prefab/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_ZombieChaser_追逐天禍 (1)",

            #endregion A9_S2

            #region A9_S3

            "A9_S3/Room/Prefab/Treasure寶箱 Chests/LootProvider 乘客令牌：夕溥/StealthGameMonster_ZombieChaser_天禍 (3)",
            "A9_S3/Room/Prefab/Treasure寶箱 Chests/LootProvider 乘客令牌：央凡/StealthGameMonster_ZombieSlow 天禍 (7)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (1)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (2)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (3)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (5)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (8)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (4)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (6)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (9)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (11)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieSlow 天禍 (12)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieChaser_天禍 (2)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieChaser_天禍 (4)",
            "A9_S3/Room/Prefab/StealthGameMonster_ZombieChaser_天禍 (5)",

            #endregion A9_S3

            #region A11_S1

            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 爆炸 (2)",
            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 爆炸 (5)",
            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 爆炸 (7)",
            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 爆炸 (6)",
            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 爆炸 (3)",
            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 爆炸 (4)",
            "A11_S1/Room/StealthGameMonster_Zombie_Crawler (1)",
            "A11_S1/Room/StealthGameMonster_Zombie_Crawler (2)",
            "A11_S1/Room/MonsterGameplay_2/StealthGameMonster_ZombieSlow 天禍 (1)",
            "A11_S1/Room/MonsterGameplay_2/StealthGameMonster_ZombieSlow 天禍 (4)",
            "A11_S1/Room/StealthGameMonster_ZombieChaser_追逐天禍 (1)",
            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 爆炸 (8)",
            "A11_S1/Room/StealthGameMonster_ZombieSlow 天禍 (1)",
            "A11_S1/Room/MonsterGameplay_3/StealthGameMonster_ZombieSlow 天禍 爆炸 (7)",
            "A11_S1/Room/MonsterGameplay_3/StealthGameMonster_Zombie_Crawler (3)",
            "A11_S1/Room/MonsterGameplay_3/StealthGameMonster_ZombieSlow 天禍 (2)",
            "A11_S1/Room/EngageGroup/StealthGameMonster_ZombieChaser_追逐天禍 (1)",
            "A11_S1/Room/EngageGroup/StealthGameMonster_ZombieChaser_追逐天禍 (2)",
            "A11_S1/Room/EngageGroup/StealthGameMonster_ZombieSlow 天禍 爆炸 (8)",
            "A11_S1/Room/EngageGroup/StealthGameMonster_ZombieSlow 天禍 爆炸 (9)",
            "A11_S1/Room/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/Phase3_On/Phase2_遊戲最終階段 /EngageToken/StealthGameMonster_ZombieSlow 天禍 爆炸 (7)",
            "A11_S1/Room/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/Phase3_On/Phase2_遊戲最終階段 /EngageToken/StealthGameMonster_ZombieSlow 天禍 爆炸 (8)",
            "A11_S1/Room/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/Phase3_On/Phase2_遊戲最終階段 /EngageToken/StealthGameMonster_ZombieSlow 天禍 爆炸 (9)",
            "A11_S1/Room/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/Phase3_On/Phase2_遊戲最終階段 /StealthGameMonster_ZombieSlow 天禍 爆炸 (8)",
            "A11_S1/Room/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/Phase3_Off/Phase3_Off/LootProvider 乘客令牌：史陽月/StealthGameMonster_ZombieSlow 天禍",
            "A11_S1/Room/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/Phase3_Off/Phase3_Off/StealthGameMonster_ZombieSlow 天禍 (2)",
            "A11_S1/Room/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/Phase3_Off/Phase3_Off/StealthGameMonster_ZombieChaser_追逐天禍 爆炸 (2)",
            "A11_S1/Room/山海9000結局/Rebind/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser 小倩Variant/FSM Animator/LogicRoot/LootProvider_小倩照片/StealthGameMonster_ZombieChaser_追逐天禍 小倩",
            "A11_S1/Room/山海9000結局/Rebind/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser 小倩Variant/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",

            #endregion A11_S1

            #region A11_S2

            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_ZombieChaser_追逐天禍 (1)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieSlow 天禍 (3)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieSlow 天禍 爆炸 (2)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieSlow 天禍 爆炸 (3)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieChaser_追逐天禍 (1)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/StealthGameMonster_ZombieSlow 天禍 爆炸 (1)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_ZombieChaser_追逐天禍 (4)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_ZombieChaser_追逐天禍 (3)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_ZombieChaser_追逐天禍 (2)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_Zombie_Crawler肥蜘蛛",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (看守的人)/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieSlow 天禍 (1)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/[自然巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario /[MonsterBehaviorProvider] LevelDesign_WanderingGroup (巡邏的人)/StealthGameMonster_ZombieSlow 天禍 (2)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/EngageGroup/StealthGameMonster_ZombieChaser_追逐天禍 (1)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/General FSM Object_On And Off Switch 最終階段切換 Variant/FSM Animator/LogicRoot/[On]Node/Phase2_遊戲最終階段 /GameplayRoot/EngageGroup/StealthGameMonster_ZombieChaser_追逐天禍 (2)",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_1 /Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode_CloseDoorFight_1/Wave2/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant 2 /FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_1 /Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode_CloseDoorFight_1/Wave2/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant /FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_2/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/ [0] [關門戰出場情境]StealthGameMonster_Zombie_Crawler肥蜘蛛/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_Zombie_Crawler肥蜘蛛",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_2/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave1/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant  (1)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_2/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave3/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant /FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_2/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave3/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant  (2)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_2/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave3/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant  (3)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/Phase相關切換Gameplay----------------/[Set] CloseDoorFight View&Wave_2/Monster Wave Sequence Spawner Variant/FSM Animator/LogicRoot/MonsterRegisterNode/Wave3/A11Pod_Z衝出天禍怪FSM_Prototype_SlowExplode Variant/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_2/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_4/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant 2/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_4/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant 2 (1)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_4/A11Pod_Z衝出天禍怪FSM_Prototype_SlowExplode Variant /FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_4/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant /FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_6/ [0] [關門戰出場情境]StealthGameMonster_ZombieChaser_追逐天禍 (1)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍 (1)",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_6/ [0] [關門戰出場情境]StealthGameMonster_ZombieChaser_追逐天禍 (2)/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍 (1)",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_6/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant 2 /FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_6/A11Pod_Z衝出天禍怪FSM_Prototype_SlowExplode Variant/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_7/A11Pod_Z衝出天禍怪FSM_Prototype_SlowExplode Variant/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A11_S2/Room/Prefab/EventBinder/OldBoy FSM Object/FSM Animator/LogicRoot/MonsterRegisterNode/MonsterWaveFight_7/A11Pod_Z衝出天禍怪FSM_Prototype_SlowExplode Variant (1)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍 爆炸",
            "A11_S2/四號實驗室：天禍突變體完全體（Jump Scare）/Props/Phase2/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant 2/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Arena 實驗走道：停屍間 Z軸怪物跑出來鎖住/Background/PHASE/Phase 2/Props/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant 2/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Arena 實驗走道：停屍間 Z軸怪物跑出來鎖住/Background/PHASE/Phase 2/Props/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Arena 實驗走道：停屍間 Z軸怪物跑出來鎖住/Background/PHASE/Phase 2/Props/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant (1)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Arena 實驗走道：停屍間 Z軸怪物跑出來鎖住/Background/PHASE/Phase 2/Props/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant (2)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_S2/Arena 實驗走道：停屍間 Z軸怪物跑出來鎖住/Background/PHASE/Phase 2/Props/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant 2/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍",
            "A11_S2/Arena 實驗走道：停屍間 Z軸怪物跑出來鎖住/Background/PHASE/Phase 2/Props/A11Pod_Z衝出天禍怪FSM_Prototype_Slow Variant 2 (1)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieSlow 天禍"

            #endregion A11_S2

            #region A11_SG1

            ,"A11_SG1/Room/Prefab/LootProvider/EventBinder/General Boss Fight FSM Object Variant_Headless刑天 Variant/FSM Animator/LogicRoot/StealthGameMonster_ZombieSlow 天禍",
            "A11_SG1/Room/Prefab/LootProvider/EventBinder/General Boss Fight FSM Object Variant_Headless刑天 Variant/FSM Animator/LogicRoot/StealthGameMonster_ZombieSlow 天禍 (1)",
            "A11_SG1/Room/Prefab/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant /FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",
            "A11_SG1/Room/Prefab/A11Pod_Z衝出天禍怪FSM_Prototype_Chaser Variant  (1)/FSM Animator/LogicRoot/LevelDesign/ [0] [關門戰出場情境]StealthGameMonster/[MonsterBehaviorProvider] LevelDesign_Spawner/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_SpawnDoorProvider/StealthGameMonster_ZombieChaser_追逐天禍",

            #endregion A11_SG1
        ];

        public MonsterPatchResolver MonsterPatchResolver { get; protected set; }

        public RegularEnemiesPatchesInitializer(CustomMonsterStateValuesResolver monsterStateValuesResolver) {
            this.monsterStateValuesResolver = monsterStateValuesResolver;

            MonsterPatchResolver = CreateResolver();
        }

        public override MonsterPatchResolver CreateResolver() {
            var resolver = new MonsterPatchResolver();
            resolver.AddDefaultPatch(GetRegularMonsterPatch());
            AddNullExceptions(resolver);
            AddMinibossesPatches(resolver);
            AddPseudoMinibossesPatches(resolver);
            AddMutantsPatches(resolver);

            return resolver;
        }

        protected virtual GeneralBossPatch GetRegularMonsterPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var defaultEnemyPatch = new RevivalChallengeBossPatch();
            defaultEnemyPatch.DieStates = [
                MonsterBase.States.Dead
            ];
            defaultEnemyPatch.InsertPlaceState = MonsterBase.States.Dead;
            defaultEnemyPatch.EnemyType = ChallengeEnemyType.Regular;
            defaultEnemyPatch.UseKillCounterTracking = false;
            defaultEnemyPatch.UseModifierControllerTracking = false;
            defaultEnemyPatch.UseCompositeTracking = true;
            defaultEnemyPatch.UseProximityActivation = true;

            var resetStateConfig = defaultEnemyPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.PauseTime = 1.25f;
            resetStateConfig.Animations = [];
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];
            resetStateConfig.UseFlashing = true;

            return defaultEnemyPatch;
        }

        protected virtual void AddNullExceptions(MonsterPatchResolver patchResolver) {
            foreach (var name in doNotPatch) {
                patchResolver.AddPatch(name, null);
            }
        }

        protected virtual void AddMinibossesPatches(MonsterPatchResolver patchResolver) {
            foreach (var name in minibossesPatches) {
                patchResolver.AddPatch(name, GetMinibossBossPatch());
            }
        }

        protected virtual void AddPseudoMinibossesPatches(MonsterPatchResolver patchResolver) {
            foreach (var name in pseudoMinibossesPatches) {
                patchResolver.AddPatch(name, GetPseudoMinibossBossPatch());
            }
        }

        protected virtual void AddMutantsPatches(MonsterPatchResolver patchResolver) {
            foreach (var name in mutantsPatches) {
                patchResolver.AddPatch(name, GetMutantBossPatch());
            }
        }

        protected virtual GeneralBossPatch GetMinibossBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var defaultMinibossPatch = new RevivalChallengeBossPatch();
            defaultMinibossPatch.DieStates = [
                MonsterBase.States.LastHit,
                MonsterBase.States.Dead
            ];
            defaultMinibossPatch.EnemyType = ChallengeEnemyType.Miniboss;
            defaultMinibossPatch.UseProximityActivation = true;
            defaultMinibossPatch.UseCompositeTracking = true;

            var resetStateConfig = defaultMinibossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.Animations = [];
            resetStateConfig.PauseTime = 2f;
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];
            resetStateConfig.UseFlashing = true;

            return defaultMinibossPatch;
        }

        protected virtual GeneralBossPatch GetPseudoMinibossBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var defaultMinibossPatch = new RevivalChallengeBossPatch();
            defaultMinibossPatch.DieStates = [
                MonsterBase.States.Dead
            ];
            defaultMinibossPatch.InsertPlaceState = MonsterBase.States.Dead;
            defaultMinibossPatch.EnemyType = ChallengeEnemyType.Miniboss;
            defaultMinibossPatch.UseProximityActivation = true;
            defaultMinibossPatch.UseCompositeTracking = true;

            var resetStateConfig = defaultMinibossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.Animations = [];
            resetStateConfig.PauseTime = 2f;
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];
            resetStateConfig.UseFlashing = true;

            return defaultMinibossPatch;
        }

        protected virtual GeneralBossPatch GetMutantBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var mutantPatch = new RevivalChallengeBossPatch();
            mutantPatch.DieStates = [
                MonsterBase.States.FakeDead,
                MonsterBase.States.Dead
            ];
            mutantPatch.InsertPlaceState = MonsterBase.States.Dead;
            mutantPatch.EnemyType = ChallengeEnemyType.Regular;
            mutantPatch.UseKillCounterTracking = false;
            mutantPatch.UseModifierControllerTracking = false;
            mutantPatch.UseCompositeTracking = true;
            mutantPatch.UseProximityActivation = true;

            var resetStateConfig = mutantPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.PauseTime = 1.25f;
            resetStateConfig.Animations = [];
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];
            resetStateConfig.UseFlashing = true;

            return mutantPatch;
        }
    }
}
