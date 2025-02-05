using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(ResolutionSetting))]
    public class ResolutionSettingPatches {

        [HarmonyPatch("OnChanged")]
        [HarmonyPostfix]
        public static void OnChanged_Postfix(ref Resolution resolution) {
            BossChallengeMod.Instance.UIController?.RecalculatePositions(resolution.width, resolution.height);
        }
    }
}
