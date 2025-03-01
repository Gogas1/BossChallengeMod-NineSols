using BossChallengeMod.BossPatches;
using BossChallengeMod.Configuration;
using BossChallengeMod.KillCounting;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(ButterflyBossFightLogic))]
    public class ButterflyBossFightLogicPatches {

        [HarmonyPatch(nameof(ButterflyBossFightLogic.SetPhase))]
        [HarmonyPostfix]
        private static void SetPhase_Postfix(ButterflyBossFightLogic __instance, int targetPhase) {
            if (targetPhase == 2) {
                GeneralBossPatch bossPatch = BossChallengeMod.Instance.BossesPatchResolver.GetPatch("P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/LogicRoot/ButterFly_BossFight_Logic/StealthGameMonster_Boss_ButterFly Variant")!;

                ChallengeConfigurationManager challengeConfigurationManager = BossChallengeMod.Instance.ChallengeConfigurationManager;
                StoryChallengeConfigurationManager storyChallengeConfigurationManager = BossChallengeMod.Instance.StoryChallengeConfigurationManager;

                ChallengeConfiguration ConfigurationToUse = ApplicationCore.IsInBossMemoryMode ? challengeConfigurationManager.ChallengeConfiguration : storyChallengeConfigurationManager.ChallengeConfiguration;

                if(ConfigurationToUse.EnableMod) {
                    var targetMonster = __instance.allMonsters[0];
                    var killCountingController = targetMonster.GetComponent<MonsterKillCounter>();

                    if (killCountingController != null && (killCountingController.KillCounter + 1 < killCountingController.MaxBossCycles || killCountingController.MaxBossCycles == -1)) {

                        targetMonster.postureSystem.DieHandleingStates.Clear();
                        targetMonster.postureSystem.DieHandleingStates.AddRange(bossPatch.DieStates);
                        targetMonster.postureSystem.GenerateCurrentDieHandleStacks();
                    }
                }
            }
        }

        [HarmonyPatch(nameof(ButterflyBossFightLogic.SetPhase))]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> SetPhase_Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();
            if (codes.Count >= 219) {
                codes[213].opcode = OpCodes.Nop;
                codes[214].opcode = OpCodes.Nop;
                codes[215].opcode = OpCodes.Nop;
                codes[216].opcode = OpCodes.Nop;
                codes[217].opcode = OpCodes.Nop;
                codes[218].opcode = OpCodes.Nop;
            }
            return instructions;
        }

        [HarmonyPatch(nameof(ButterflyBossFightLogic.ChangingPhase))]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ChangingPhase_Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();
            if (codes.Count >= 6) {
                codes[0].opcode = OpCodes.Nop;
                codes[1].opcode = OpCodes.Nop;
                codes[2].opcode = OpCodes.Nop;
                codes[3].opcode = OpCodes.Nop;
                codes[4].opcode = OpCodes.Nop;
                codes[5].opcode = OpCodes.Nop;
            }
            return instructions;
        }
    }
}
