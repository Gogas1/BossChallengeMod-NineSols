using BossChallengeMod.Modifiers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(MonsterShield))]
    public class MonsterShieldPatches {

        [HarmonyPatch("BreakShield")]
        [HarmonyPostfix]
        private static void BreakShield_Postfix(MonsterShield __instance) {
            var modifiers = __instance.gameObject.GetComponentInParent<MonsterBase>()?.GetComponentsInChildren<ModifierBase>() ?? null;

            if (modifiers != null) {
                foreach (ModifierBase modifier in modifiers) {
                    modifier.MonsterNotify(MonsterNotifyType.OnShieldBroken);
                }
            }
        }
    }
}
