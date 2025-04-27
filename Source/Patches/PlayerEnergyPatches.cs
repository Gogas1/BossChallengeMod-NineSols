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
        private static void Gain_Prefix(PlayerEnergy __instance, float amount, out bool __state) {
            __state = false;

            try {
                if (__instance == Player.i.chiContainer) {
                    if (BossChallengeMod.Instance.GlobalModifiersFlags.EnableQiOverloadVotes.Any()) {
                        int extraCharges = Math.Max(0, (int)(__instance.Value + amount - __instance.MaxValue));

                        if (extraCharges > 0) {
                            Player.i.health.ReceiveRecoverableDamage(Player.i.health.maxHealth.Value / 10);
                            if (Player.i.health.currentValue <= Player.i.health.maxHealth.Value / 10) {
                                overloadCounter += extraCharges;
                                if (overloadCounter >= overloadThreshold) {
                                    __state = true;
                                    overloadCounter = 0;
                                }
                            }

                        } else {
                            overloadCounter = 0;
                        }
                    }

                    if (__instance.Value < __instance.MaxValue && __instance.Value + amount >= __instance.MaxValue) {
                        BossChallengeMod.Instance.GlobalModifiersFlags.NotifyPlayerGainFullQiSubscribers(__instance);
                    }
                }
            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        [HarmonyPatch(nameof(PlayerEnergy.Gain))]
        [HarmonyPostfix]
        private static void Gain_Postfix(PlayerEnergy __instance, float amount, bool __state) {
            if (__instance == Player.i.chiContainer) {
                if(__state) {
                    __instance.Clear();
                }
            }
        }

        [HarmonyPatch(nameof(PlayerEnergy.Consume))]
        [HarmonyPostfix]
        private static void Consume_Postfix(PlayerEnergy __instance) {
            try {
                if (__instance == Player.i.chiContainer) {
                    overloadCounter = 0;

                    if (__instance.Value <= 0) {
                        BossChallengeMod.Instance.GlobalModifiersFlags.NotifyPlayerDepletedQiSubscribers(__instance);
                    }
                }
            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
            
        }
    }
}
