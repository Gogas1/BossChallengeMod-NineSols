using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(PlayerEnergy))]
    public class PlayerEnergyPatches {

        private static int overloadCounter = 0;
        private static int overloadThreshold = 6;
        [HarmonyPatch(nameof(PlayerEnergy.Gain))]
        [HarmonyPrefix]
        private static bool Gain_Prefix(PlayerEnergy __instance, float amount) {
            if (__instance == Player.i.chiContainer) {
                if (BossChallengeMod.Instance.GlobalModifiersFlags.EnableQiOverloadVotes.Any()) {
                    int extraCharges = Math.Max(0, (int)(__instance.Value + amount - __instance.MaxValue));

                    if (extraCharges > 0) {
                        Player.i.health.ReceiveRecoverableDamage(Player.i.health.maxHealth.Value / 10);
                        if (Player.i.health.currentValue <= Player.i.health.maxHealth.Value / 10) {
                            overloadCounter += extraCharges;
                            if (overloadCounter >= overloadThreshold) {
                                __instance.Clear();
                                overloadCounter = 0;
                                return false;
                            }
                        }

                    } else {
                        overloadCounter = 0;
                    }
                }
            }

            return true;
        }

        [HarmonyPatch(nameof(PlayerEnergy.Consume))]
        [HarmonyPostfix]
        private static void Consume_Postfix(PlayerEnergy __instance) {
            overloadCounter = 0;
        }
    }
}
