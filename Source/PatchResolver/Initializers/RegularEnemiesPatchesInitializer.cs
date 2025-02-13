using BossChallengeMod.BossPatches;
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
            "A0_S4 gameLevel/Room/軒軒野豬情境OnOff FSM/FSM Animator/LogicRoot/[On]Node/CullingGroup/SimpleCutSceneFSM_EncounterBoar (開頭介紹野豬的演出)/FSM Animator/LogicRoot/After/LootProvider/StealthGameMonster_RunningHog (1)"
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

            return defaultEnemyPatch;
        }

        protected virtual void AddNullExceptions(MonsterPatchResolver patchResolver) {
            foreach (var name in doNotPatch) {
                patchResolver.AddPatch(name, null);
            }
        }
    }
}
