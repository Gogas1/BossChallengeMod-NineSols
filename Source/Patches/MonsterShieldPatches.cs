using BossChallengeMod.Modifiers;
using BossChallengeMod.Modifiers.Managers;
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
            var modifiersController = __instance.gameObject.GetComponentInParent<MonsterBase>().GetComponent<MonsterModifierController>();
            if (modifiersController != null) {
                modifiersController.CustomNotify(MonsterNotifyType.OnShieldBroken);
            }
        }
    }
}
