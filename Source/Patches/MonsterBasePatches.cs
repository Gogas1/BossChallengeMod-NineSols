using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossChallengeMod.BossPatches;
using BossChallengeMod.Interfaces;
using BossChallengeMod.KillCounting;
using BossChallengeMod.Modifiers;
using BossChallengeMod.Modifiers.Managers;
using HarmonyLib;
using NineSolsAPI.Utils;
using UnityEngine.Events;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(MonsterBase))]
    public class MonsterBasePatches {

        [HarmonyPatch("CheckInit")]
        [HarmonyPrefix]
        private static void CheckInit_Prefix(MonsterBase __instance, out IEnumerable<MonsterState> __state) {
            __state = new List<MonsterState>();

            try {

                if (__instance.tag == "Boss") {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.BossesPatchResolver.GetPatch(ObjectUtils.ObjectPath(__instance.gameObject));
                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch.PatchMonsterPostureSystem(__instance);
                        __state = bossPatch.PatchMonsterStates(__instance);
                        var senders = bossPatch.CreateSenders(__instance, __state);
                        var receivers = bossPatch.CreateReceivers(__instance, __state);
                        bossPatch.ProcessEventHandlers(receivers, senders);
                    }

                }

                if (__instance.CompareTag("Enemy") || __instance.CompareTag("Untagged")) {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.RegularMonstersPatchResolver.GetPatch(ObjectUtils.ObjectPath(__instance.gameObject));
                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch.PatchMonsterPostureSystem(__instance);
                        __state = bossPatch.PatchMonsterStates(__instance);
                        var senders = bossPatch.CreateSenders(__instance, __state);
                        var receivers = bossPatch.CreateReceivers(__instance, __state);
                        bossPatch.ProcessEventHandlers(receivers, senders);
                    }
                }
            } catch (Exception e) {
                Log.Error($"Prefix Patching. {e.Message}, {e.StackTrace}");
            }
        }

        [HarmonyPatch("CheckInit")]
        [HarmonyPostfix]
        private static void CheckInit_Postfix(MonsterBase __instance, IEnumerable<MonsterState> __state) {

            try {

                if (__instance.tag == "Boss") {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.BossesPatchResolver.GetPatch(ObjectUtils.ObjectPath(__instance.gameObject));

                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch?.PatchMonsterFsmLookupStates(__instance, __state);
                        bossPatch?.PostfixPatch(__instance);
                    }

                }

                if (__instance.CompareTag("Enemy") || __instance.CompareTag("Untagged")) {
                    GeneralBossPatch? bossPatch = BossChallengeMod.Instance.RegularMonstersPatchResolver.GetPatch(ObjectUtils.ObjectPath(__instance.gameObject));

                    if (bossPatch != null && bossPatch.CanBeApplied()) {
                        bossPatch?.PatchMonsterFsmLookupStates(__instance, __state);
                        bossPatch?.PostfixPatch(__instance);
                    } 

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

            var enduranceModifier = __instance.GetComponentInChildren<EnduranceModifier>();
            if (enduranceModifier != null) {
                if (enduranceModifier.enabled && !enduranceModifier.IsPaused) {
                    return false;
                }
            }

            return true;
        }

        [HarmonyPatch("OnExplode")]
        [HarmonyPostfix]
        private static void OnExplode_Postfix(MonsterBase __instance) {
            try {
                var modifiersController = __instance.GetComponent<MonsterModifierController>();
                if (modifiersController != null) {
                    modifiersController.CustomNotify(MonsterNotifyType.OnExplode);
                }

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        [HarmonyPatch(nameof(MonsterBase.ExitLevelAndDestroy))]
        [HarmonyPrefix]
        private static void OnDestroy_Prefix(MonsterBase __instance) {
            var modifiersController = __instance.GetComponent<MonsterModifierController>();
            if (modifiersController != null) {
                modifiersController.OnDestroing();
            }

            var killCounterController = __instance.GetComponent<MonsterKillCounter>();
            if (killCounterController != null) {
                killCounterController.OnDestroing();
            }
        }

        [HarmonyPatch(nameof(MonsterBase.ChangeStateIfValid), new Type[] { typeof(MonsterBase.States) })]
        [HarmonyPostfix]
        private static void ChangeStateIfValid_Postfix(MonsterBase __instance, MonsterBase.States targetState) {
            if(targetState == MonsterBase.States.LastHit) {
                UnityEvent onDie = __instance.OnDie;
                if (onDie != null) {
                    onDie.Invoke();
                }
            }
        }
    }
}
