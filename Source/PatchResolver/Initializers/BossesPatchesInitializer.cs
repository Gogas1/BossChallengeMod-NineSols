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

        public BossesPatchesInitializer(CustomMonsterStateValuesResolver monsterStateValuesResolver) {
            this.monsterStateValuesResolver = monsterStateValuesResolver;

            MonsterPatchResolver = CreateResolver();
        }
        public override MonsterPatchResolver CreateResolver() {
            var resolver = new MonsterPatchResolver();
            resolver.AddDefaultPatch(GetDefaultBossPatch());
            resolver.AddPatch("StealthGameMonster_SpearHorseMan", GetHorseBossPatch());
            resolver.AddPatch("StealthGameMonster_伏羲_新", GetFuxiBossPatch());
            resolver.AddPatch("StealthGameMonster_新女媧 Variant", GetNuwaBossPatch());
            resolver.AddPatch("StealthGameMonster_GouMang Variant", GetGoumangBossPatch());
            resolver.AddPatch("Monster_GiantMechClaw", GetClawBossPatch());

            var butterflyPatch = GetButterflyBossPatch();
            resolver.AddPatch("StealthGameMonster_Boss_ButterFly Variant", butterflyPatch);
            resolver.AddPatch("StealthGameMonster_Boss_ButterFly Variant (1)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("StealthGameMonster_Boss_ButterFly Variant (2)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("StealthGameMonster_Boss_ButterFly Variant (3)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("StealthGameMonster_Boss_ButterFly Variant (4)", new RevivalChallengeBossClonePatch());
            resolver.AddPatch("Boss_Yi Gung", GetEigongBossPatch());

            resolver.AddPatch(
                "A1_S2_GameLevel/Room/Prefab/Gameplay5/EventBinder/LootProvider/General Boss Fight FSM ObjectA1_S2_大劍兵/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_Samurai_General_Boss Variant",
                GetMinibossBossPatch());

            return resolver;
        }

        private GeneralBossPatch GetMinibossBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var defaultBossPatch = new RevivalChallengeBossPatch();
            defaultBossPatch.DieStates = [
                MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];
            defaultBossPatch.EnemyType = KillCounting.ChallengeEnemyType.Miniboss;

            var resetStateConfig = defaultBossPatch.ResetStateConfiguration;
            resetStateConfig.ExitState = MonsterBase.States.Engaging;
            resetStateConfig.Animations = [];
            resetStateConfig.PauseTime = 2f;
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return defaultBossPatch;
        }

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
        private GeneralBossPatch GetClawBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var clawBossPatch = new ClawBossPatch();
            clawBossPatch.DieStates = [
                MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
            ];

            var resetStateConfig = clawBossPatch.ResetStateConfiguration;
            resetStateConfig.Animations = ["PostureBreak"];
            resetStateConfig.ExitState = MonsterBase.States.Attack7;
            resetStateConfig.StateType = bossReviveMonsterState;
            resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

            return clawBossPatch;
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

            var eigongBossPatch = new RevivalChallengeBossPatch();
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
        private GeneralBossPatch GetHorseBossPatch() {
            var bossReviveMonsterState = monsterStateValuesResolver.GetState("BossRevive");

            var defaultBossPatch = new HorseBossPatch();
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
    }
}
