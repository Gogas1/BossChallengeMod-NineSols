using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Patches {
    [HarmonyPatch(typeof(PlayerWeaponState))]
    public class PlayerWeaponStatePatches {
        [HarmonyPatch(nameof(PlayerWeaponState.OnStateExit))]
        [HarmonyPostfix]
        private static void OnStateExit_Postfix() {
            if (BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Any()) {
                int arrowsNum = Player.i.weaponDataCollection.AcquiredCount;
                int variantsNum = UnityEngine.Random.Range(1, arrowsNum);
                for (int i = 0; i < variantsNum; i++) {
                    Player.i.weaponDataCollection.Next();
                }
            }
        }
    }
}
