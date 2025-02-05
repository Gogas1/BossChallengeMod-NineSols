using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Patches {
    [HarmonyPatch(typeof(CollectionRotateSelectorButton))]
    public class CollectionRotateSelectorButtonPatches {
        [HarmonyPatch("SubmitImplementation")]
        [HarmonyPrefix]
        private static bool SubmitImplementation_Prefix(CollectionRotateSelectorButton __instance) {
            if (__instance.gameObject.name == "PlayerStatusSelectableButton_ControlStyle" && BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes.Any()) {
                return false;
            }

            if (__instance.gameObject.name == "PlayerStatusSelectableButton_Arrow" && BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Any()) {
                return false;
            }

            return true;
        }

        [HarmonyPatch("SubmitImplementation")]
        [HarmonyPostfix]
        private static void SubmitImplementation_Postfix(CollectionRotateSelectorButton __instance) {
            if (__instance.gameObject.name == "PlayerStatusSelectableButton_ControlStyle") {
                BossChallengeMod.Instance.UIController.UpdateTalisman(__instance.image.sprite);
            }
        }
    }
}
