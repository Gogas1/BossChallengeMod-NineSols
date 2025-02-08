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
            "A1_S1_GameLevel/Room/GamePlayS3_2/TrapMonster_Mutant (6)"
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
            defaultEnemyPatch.EnemyType = KillCounting.ChallengeEnemyType.Regular;
            defaultEnemyPatch.UseKillCounterTracking = false;
            defaultEnemyPatch.UseModifierControllerTracking = false;

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
