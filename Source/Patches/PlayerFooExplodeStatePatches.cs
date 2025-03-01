using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Patches {
    [HarmonyPatch(typeof(PlayerFooExplodeState))]
    public class PlayerFooExplodeStatePatches {

        [HarmonyPatch(nameof(PlayerFooExplodeState.AnimationEvent))]
        [HarmonyPostfix]
        private static void AnimationEvent_Postfix() {
            try {
                if (Player.i.mainAbilities.FooExplodeAutoStyle.IsActivated) {
                    Player.i.ChangeState(PlayerStateType.Normal, false);
                }

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }
    }
}
