using BossChallengeMod.Modifiers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Patches {
    [HarmonyPatch(typeof(PlayerParryState))]
    public class PlayerParryStatePatches {
        [HarmonyPatch("NonAccurateParry")]
        [HarmonyPostfix]
        private static void NonAccurateParry_Postfix(PlayerParryState __instance, ref EffectHitData hitData, ref ParryParam param, ref DamageDealer bindDamage) {
            if (bindDamage.Owner == null) return;
            var modifiers = bindDamage.Owner.GetComponentsInChildren<ModifierBase>();
            if (modifiers != null) {
                var parryDaamageModifier = modifiers.FirstOrDefault(m => m.Key == "parry_damage");
                if (parryDaamageModifier != null) {
                    Log.Info($"{parryDaamageModifier}, {parryDaamageModifier.enabled}, {parryDaamageModifier.IsPaused}");
                }
                if ((parryDaamageModifier?.enabled ?? false) && (!parryDaamageModifier?.IsPaused ?? false)) {
                    var playerHealth = Player.i.health;
                    playerHealth.CurrentInternalInjury = 0;
                    playerHealth.ResetRecoverableTime();
                }

                var damageBuildupModifier = modifiers.FirstOrDefault(m => m.Key == "damage_buildup");
                if ((damageBuildupModifier?.enabled ?? false) && (!damageBuildupModifier?.IsPaused ?? false)) {
                    var playerHealth = Player.i.health;
                    var damageAmount = bindDamage.DamageAmount;
                    playerHealth.RecoverInternalInjury(damageAmount / 2);
                }
            }

        }

        [HarmonyPatch(nameof(PlayerParryState.Parried))]
        [HarmonyPrefix]
        private static void Parried_Prefix(ref ParryParam param, ref DamageDealer bindDamage) {
            if (bindDamage.Owner == null) return;
            var modifiers = bindDamage.Owner.GetComponentsInChildren<ModifierBase>();
            if (modifiers != null) {
                var knockbackModifier = modifiers.FirstOrDefault(m => m.Key == "knockback");
                if ((knockbackModifier?.enabled ?? false) && (!knockbackModifier?.IsPaused ?? false)) {
                    param.knockBackValue = param.knockBackValue * 1.5f;
                }
            }
        }
    }
}
