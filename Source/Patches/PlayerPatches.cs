using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(Player))]
    public class PlayerPatches {

        [HarmonyPatch(nameof(Player.NextItemCheck))]
        [HarmonyPrefix]
        private static bool NextItemPatch() {
            if (BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Any()) return false;
            return true;
        }

        [HarmonyPatch(nameof(Player.PreviousItemCheck))]
        [HarmonyPrefix]
        private static bool PrevItemPatch() {
            if (BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Any()) return false;
            return true;
        }
    }
}
