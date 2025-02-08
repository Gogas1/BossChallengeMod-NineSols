using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossChallengeMod.BossPatches;
using BossChallengeMod.Interfaces;
using BossChallengeMod.Modifiers;
using HarmonyLib;
using NineSolsAPI.Utils;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(MonsterBase))]
    public class MonsterBasePatches {

        [HarmonyPatch("CheckInit")]
        [HarmonyPrefix]
        private static void CheckInit_Prefix(MonsterBase __instance, out IEnumerable<MonsterState> __state) {
            Log.Info($"Prefix Patching {ObjectUtils.ObjectPath(__instance.gameObject)}");
            __state = new List<MonsterState>();

            try {
                bool skipped = true;

                if (__instance.tag == "Boss") {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.BossesPatchResolver.GetPatch(ObjectUtils.ObjectPath(__instance.gameObject));

                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch.PatchMonsterPostureSystem(__instance);
                        __state = bossPatch.PatchMonsterStates(__instance);
                        var senders = bossPatch.CreateSenders(__instance, __state);
                        var receivers = bossPatch.CreateReceivers(__instance, __state);
                        bossPatch.ProcessEventHandlers(receivers, senders);
                    }

                    skipped = false;
                }

                if (__instance.CompareTag("Enemy") || __instance.CompareTag("Untagged")) {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.RegularMonstersPatchResolver.GetPatch(ObjectUtils.ObjectPath(__instance.gameObject));
                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch.PatchMonsterPostureSystem(__instance);
                        __state = bossPatch.PatchMonsterStates(__instance);
                        var senders = bossPatch.CreateSenders(__instance, __state);
                        var receivers = bossPatch.CreateReceivers(__instance, __state);
                        bossPatch.ProcessEventHandlers(receivers, senders);
                    } else {
                        Log.Info($"Prefix Patching. Null patched");
                    }

                    skipped = false;
                }

                if (skipped) {
                    Log.Warning($"Prefix Patching. The monster was skipped?");
                }
            } catch (Exception e) {
                Log.Error($"Prefix Patching. {e.Message}, {e.StackTrace}");
            }
        }

        [HarmonyPatch("CheckInit")]
        [HarmonyPostfix]
        private static void CheckInit_Postfix(MonsterBase __instance, IEnumerable<MonsterState> __state) {
            Log.Info($"Postfix Patching {ObjectUtils.ObjectPath(__instance.gameObject)}");

            try {
                bool skipped = true;

                if (__instance.tag == "Boss") {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.BossesPatchResolver.GetPatch(ObjectUtils.ObjectPath(__instance.gameObject));

                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch?.PatchMonsterFsmLookupStates(__instance, __state);
                        bossPatch?.PostfixPatch(__instance);
                    } else {
                        Log.Info($"Postfix Patching. Null patched");
                    }

                    skipped = false;
                }

                if (__instance.CompareTag("Enemy") || __instance.CompareTag("Untagged")) {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.RegularMonstersPatchResolver.GetPatch(__instance.name);

                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch?.PatchMonsterFsmLookupStates(__instance, __state);
                        bossPatch?.PostfixPatch(__instance);
                    } else {
                        Log.Info($"Postfix Patching. Null patched");
                    }

                    skipped = false;
                } else {
                    Log.Info($"Postfix Patching. Null patched");
                }

                if (skipped) {
                    Log.Warning($"Postfix Patching. The monster was skipped?");
                }
            } catch (Exception e) {
                Log.Error($"Postfix Patching. {e.Message}, {e.StackTrace}");
            }
        }

        [HarmonyPatch(nameof(MonsterBase.LevelReset))]
        [HarmonyPostfix]
        private static void LevelReset_Postifx(MonsterBase __instance) {
            var resettables = __instance.gameObject.GetComponentsInChildren<IResettableComponent>().ToList();
            resettables.OrderByDescending(r => r.GetPriority());

            foreach (var resettable in resettables) {
                resettable.ResetComponent();
            }
        }

        [HarmonyPatch(typeof(MonsterBase), nameof(MonsterBase.HurtInterruptCheck))]
        [HarmonyPrefix]
        private static bool HurtInterruptCheck_Prefix(MonsterBase __instance) {
            if (__instance.name.Contains("Boss_ButterFly")) {
                return true;
            }

            var modifiers = __instance.GetComponentsInChildren<ModifierBase>();
            if (modifiers != null) {
                if (modifiers.FirstOrDefault(m => m.Key == "endurance")?.enabled ?? false) {
                    return false;
                }
            }

            return true;
        }

        [HarmonyPatch(typeof(MonsterBase), "OnExplode")]
        [HarmonyPostfix]
        private static void OnExplode_Postfix(MonsterBase __instance) {
            var modifiers = __instance.GetComponentsInChildren<ModifierBase>();
            if (modifiers != null) {
                foreach (ModifierBase modifier in modifiers) {
                    modifier.MonsterNotify(MonsterNotifyType.OnExplode);
                }
            }
        }
    }
}
