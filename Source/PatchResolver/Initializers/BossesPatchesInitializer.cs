using BossChallengeMod.BossPatches.TargetPatches;
using BossChallengeMod.BossPatches;
using BossChallengeMod.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.PatchResolver.Initializers {
    public class BossesPatchesInitializer : PatchesResolverInitializerBase {
        protected CustomMonsterStateValuesResolver monsterStateValuesResolver;
        public MonsterPatchResolver MonsterPatchResolver { get; protected set; }
        protected List<string> doNotPatch = [
            "AG_LeeEar_S0/Room/李耳教學大禮包 格擋/[FSM] 李耳教學 戰鬥+結束拿能力/FSM Animator/LogicRoot/StealthGameMonster_Boss_Lear",
            "AG_LeeEar_S1/Room/李耳教學大禮包/[FSM] 李耳教學 戰鬥+結束拿能力/FSM Animator/LogicRoot/StealthGameMonster_Boss_Lear",
            "AG_LeeEar_S1/Room/StealthMonster_GiantBlade_FireBlade",
            "AG_LeeEar_S1/StealthGameMonster_TutorialDummy Variant",
            "A5_S1/Room/FlashKill Binding/werw/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_Boss_JieChuan",
            "A5_S2/Room/Prefab/EventBinder(摸過存擋點)/存擋點的節點開關/FSM Animator/LogicRoot/[On]Node/[觸發框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_MrX Variant(Chase) (2)",
            "A5_S2/Room/Prefab/EventBinder(摸過存擋點)/存擋點的節點開關/FSM Animator/LogicRoot/[Off]Node/DisEngageGroup(典獄長邏輯)/MrX Central Controller/Switcher/典獄長初登場/GamePlay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_MrX Variant(Chase)",
            "A5_S2/Room/Prefab/EventBinder(摸過存擋點)/存擋點的節點開關/FSM Animator/LogicRoot/[Off]Node/DisEngageGroup(典獄長邏輯)/MrX Central Controller/Switcher/左邊中間典獄長窄走廊出現情境/GamePlay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_MrX Variant(Chase)",
            "A5_S2/Room/Prefab/EventBinder(摸過存擋點)/存擋點的節點開關/FSM Animator/LogicRoot/[Off]Node/DisEngageGroup(典獄長邏輯)/MrX Central Controller/Switcher/典獄長回頭堵玩家情境(中間往右)/GamePlay/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_MrX Variant(Chase)",
            "A5_S2/Room/Prefab/[觸發巡邏框架]/[MonsterBehaviorProvider] LevelDesign_CullingAndResetGroup/[MonsterBehaviorProvider] LevelDesign_Init_Scenario (巡邏的人)/[MonsterBehaviorProvider] LevelDesign_WanderingGroup/[MonsterBehaviorProvider] LevelDesign_PlayerSensor/StealthGameMonster_MrX Variant(Chase)",
            "A5_S2/A5_S2_BossFight_Logic/StealthGameMonster_MrX Variant(BossFight)",
            "A5_S2/A5_S2_BossFight_Logic/StealthGameMonster_MrX_TrueBody_Room Variant",
            "A5_S2/Room/JailBossRoom/LootProvider/Boss Fight FSM MrX/FSM Animator/LogicRoot/AnimatorControlNode(Note)/StealthGameMonster_MrX Variant",
            "_depre_StealthGameMonster_伏羲 Variant(Clone)",
            ];

        protected List<string> minibossesPatches = [
            "A1_S2_GameLevel/Room/Prefab/Gameplay5/EventBinder/LootProvider/General Boss Fight FSM ObjectA1_S2_大劍兵/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_Samurai_General_Boss Variant",
            "A2_S6/Room/Prefab/寶箱 Chests/MiniBossFight 大錢袋/LootProvider 大錢袋/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthMonster_GiantBlade_FireBlade",
            "A2_S2/Room/Prefab/寶箱 Chests/EventBinder_開啟橋後觸發Boss Fight 算力元件/LootProvider 算力元件/General Boss Fight FSM Object Variant Variant/FSM Animator/LogicRoot/---Boss---/StealthGameMonster_SpearmanMiniBoss 長槍小王基本 Variant",
            "A3_S2/Room/Prefab/Gameplay_8/RCGEventSharingGroup/LootProvider 貪財玉/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_NinjaWaterGhostMiniBoss 水鬼小王 榴彈",
            "A1_S3_GameLevel/Room/Prefab/EventBinder/LootProvider 藥草催化器/General Boss Fight FSM Object_A1_S3_水鬼/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_NinjaWaterGhostMiniBoss 水鬼小王",
            "A6_S1/Room/Prefab/寶箱 Chests/LootProvider 中錢袋/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/---Boss---/StealthMonster_GiantBladeWithDropTrap",
            "A6_S3/Room/LootProvider/General Boss Fight FSM Object Variant 藥草催化器/FSM Animator/LogicRoot/---Boss---/[傳送範圍]/1_StealthMonster_連擊怪_礦坑版",
            "A4_S2/Room/Prefab/寶箱 Chests/LootProvider 風火環/General Boss Fight FSM Object_鳳凰飛兵小王 Variant/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthMonster_Flying Teleport Wizard_MiniBoss_弱化",
            "A4_S3/Room/EventBinder 玄鐵 戰神原型機圖示/刑天 Boss Fight FSM Object Variant/FSM Animator/LogicRoot/---Boss---/StealthGameMonster_ShingTen Variant",
            "A5_S4/Room/LootProvider/General Boss Fight FSM Object_長槍MiniBoss 改 爆劍玉/FSM Animator/LogicRoot/---Boss---/StealthGameMonster_SpearmanMiniBoss 長槍小王完整版 (1)",
            "GameLevel/Room/Prefab/LootProvider 玄鐵/EvenBinder/General Boss Fight FSM A0_S9 GiantBladeFire/FSM Animator/LogicRoot/---Boss---/StealthMonster_GiantBlade_FireBlade_A0_S9",
            "A2_Stage_Remake/Room/Prefab/[EventBinder]BossFight/General Boss Fight FSM Object_鳳凰飛兵小王 Variant (1)/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthMonster_Flying Teleport Wizard_MiniBoss 幻象區平行宇宙版 Variant",
            "A10_S4/Room/MiniBossLearDoorEventBinder/General Boss Fight FSM Object_殘像Miniboss/FSM Animator/LogicRoot/---Boss---/LootProvider/[傳送範圍]/StealthGameMonster_NinjaWaterGhostMiniBoss 水鬼小王 殘影Phantom Summoner",
            "A9_S1/Room/Prefab/WaterFallEventBinder/LootProvider 弓箭強度 玄鐵/General Boss Fight FSM Object_A9_S1_大錘兵 (1)/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/Hammer FatGuy 大錘兵",
            "A11_SG1/Room/Prefab/LootProvider/EventBinder/General Boss Fight FSM Object Variant_Headless刑天 Variant/FSM Animator/LogicRoot/---Boss---/StealthGameMonster_ShingTen Variant"
            ];

        public BossesPatchesInitializer(CustomMonsterStateValuesResolver monsterStateValuesResolver) {
            this.monsterStateValuesResolver = monsterStateValuesResolver;

            MonsterPatchResolver = CreateResolver();
        }
        public override MonsterPatchResolver CreateResolver() {
            var resolver = new MonsterPatchResolver();
            resolver.AddDefaultPatch(GetDefaultBossPatch());
            //resolver.AddPatch("A2_S5_ BossHorseman_GameLevel/Room/StealthGameMonster_SpearHorseMan", GetHorseBossPatch());
            resolver.AddPatch("P2_R22_Savepoint_GameLevel/Room/Prefab/EventBinder (Boss Fight 相關)/General Boss Fight FSM Object_風氏兄妹/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_伏羲_新", GetFuxiBossPatch());
            resolver.AddPatch("P2_R22_Savepoint_GameLevel/Room/Prefab/EventBinder (Boss Fight 相關)/General Boss Fight FSM Object_風氏兄妹/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_新女媧 Variant", GetNuwaBossPatch());
            resolver.AddPatch("A3_S5_BossGouMang_GameLevel/Room/StealthGameMonster_GouMang Variant", GetGoumangBossPatch());
            resolver.AddPatch("A4_S5/MechClaw Game Play/Monster_GiantMechClaw", GetClawBossPatch());

            var butterflyPatch = GetButterflyBossPatch();
            resolver.AddPatch("P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/ButterFly_BossFight_Logic/StealthGameMonster_Boss_ButterFly Variant", butterflyPatch);
            resolver.AddPatch("P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/ButterFly_BossFight_Logic/StealthGameMonster_Boss_ButterFly Variant (1)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/ButterFly_BossFight_Logic/StealthGameMonster_Boss_ButterFly Variant (2)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/ButterFly_BossFight_Logic/StealthGameMonster_Boss_ButterFly Variant (3)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/ButterFly_BossFight_Logic/StealthGameMonster_Boss_ButterFly Variant (4)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("GameLevel/Room/Prefab/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/---Boss---/Boss_Yi Gung", GetEigongBossPatch());

            AddMinibossesPatches(resolver);
            AddNullExceptions(resolver);

            return resolver;
        }

        private GeneralBossPatch GetMinibossBossPatch() {
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

        #region Bosses
        private FuxiBossPatch GetFuxiBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var fuxiBossPatch = new FuxiBossPatch();
            fuxiBossPatch.DieStates = [
                MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];

            var resetStateConfig = fuxiBossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.Animations = ["PostureBreak"];
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return fuxiBossPatch;
        }
        private GeneralBossPatch GetNuwaBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var nuwaBossPatch = new RevivalChallengeBossPatch();
            nuwaBossPatch.DieStates = [
                MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];
            nuwaBossPatch.UseKillCounter = false;
            nuwaBossPatch.UseModifiers = false;
            nuwaBossPatch.UseRecording = false;

            var resetStateConfig = nuwaBossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return nuwaBossPatch;
        }
        private GeneralBossPatch GetGoumangBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var goumangBossPatch = new GoumangBossPatch();
            goumangBossPatch.DieStates = [
                MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];

            var resetStateConfig = goumangBossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Attack2;
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return goumangBossPatch;
        }
        private ButterflyBossPatch GetButterflyBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var butterBossPatch = new ButterflyBossPatch();
            butterBossPatch.DieStates = [
                bossReviveMonsterState,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];

            var resetStateConfig = butterBossPatch.ResetStateConfiguration;
            resetStateConfig.Animations = ["Hurt"];
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return butterBossPatch;
        }
        private GeneralBossPatch GetEigongBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var eigongBossPatch = new EigongBossPatch();
            eigongBossPatch.DieStates = [
                MonsterBase.States.BossAngry,
            MonsterBase.States.FooStunEnter,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];

            var resetStateConfig = eigongBossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.Animations = ["BossAngry"];
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return eigongBossPatch;
        }

        private GeneralBossPatch GetClawBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var clawBossPatch = new RevivalChallengeBossPatch();
            clawBossPatch.DieStates = [
                MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];

            var resetStateConfig = clawBossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Attack7;
            resetStateConfig.Animations = ["PostureBreak"];
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return clawBossPatch;
        }

        private GeneralBossPatch GetDefaultBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var defaultBossPatch = new RevivalChallengeBossPatch();
            defaultBossPatch.DieStates = [
                MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];            

            var resetStateConfig = defaultBossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.Animations = ["PostureBreak"];
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return defaultBossPatch;
        }
        #endregion Bosses

        protected virtual void AddMinibossesPatches(MonsterPatchResolver patchResolver) {
            foreach (var name in minibossesPatches) {
                patchResolver.AddPatch(name, GetMinibossBossPatch());
            }
        }

        protected virtual void AddNullExceptions(MonsterPatchResolver patchResolver) {
            foreach (var name in doNotPatch) {
                patchResolver.AddPatch(name, null);
            }
        }
    }
}
