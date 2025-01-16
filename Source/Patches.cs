using HarmonyLib;
using BossChallengeMod.BossPatches;
using BossChallengeMod.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection.Emit;

namespace BossChallengeMod;

[HarmonyPatch]
public class Patches {

    [HarmonyPatch(typeof(MonsterBase), "CheckInit")]
    [HarmonyPrefix]
    public static void CheckInit_Prefix(MonsterBase __instance, out IEnumerable<MonsterState> __state) {
        __state = new List<MonsterState>();
        if (__instance.tag == "Boss") {
            GeneralBossPatch? bossPatch = null;

            if (!BossChallengeMod.Instance.BossPatches.TryGetValue(__instance.name, out bossPatch)) {
                bossPatch = BossChallengeMod.Instance.BossPatches["Default"];
            }

            if (bossPatch != null) {
                bossPatch.PatchMonsterPostureSystem(__instance);
                __state = bossPatch.PatchMonsterStates(__instance);
                var senders = bossPatch.CreateSenders(__instance, __state);
                var receivers = bossPatch.CreateReceivers(__instance, __state);
                bossPatch.ProcessEventHandlers(receivers, senders);
            }
        }
    }

    [HarmonyPatch(typeof(MonsterBase), "CheckInit")]
    [HarmonyPostfix]
    public static void CheckInit_Postfix(MonsterBase __instance, IEnumerable<MonsterState> __state) {
        if (__instance.tag == "Boss") {
            GeneralBossPatch? bossPatch = null;

            if (!BossChallengeMod.Instance.BossPatches.TryGetValue(__instance.name, out bossPatch)) {
                bossPatch = BossChallengeMod.Instance.BossPatches["Default"];
            }

            bossPatch?.PatchMonsterFsmLookupStates(__instance, __state);
            bossPatch?.PostfixPatch(__instance);
        }
    }

    [HarmonyPatch(typeof(ButterflyBossFightLogic), nameof(ButterflyBossFightLogic.SetPhase))]
    [HarmonyPostfix]
    public static void SetPhase_Postfix(ButterflyBossFightLogic __instance, int targetPhase) {
        if(targetPhase == 2) {
            GeneralBossPatch bossPatch = BossChallengeMod.Instance.BossPatches["StealthGameMonster_Boss_ButterFly Variant"];

            //if(bossPatch is ButterflyBossPatch butterflyBossPatch) {
            //    if(butterflyBossPatch.ChallengeConfiguration.EnableRestoration) {
                    var targetMonster = __instance.allMonsters[0];
                    targetMonster.postureSystem.DieHandleingStates.Clear();
                    targetMonster.postureSystem.DieHandleingStates.AddRange(bossPatch.DieStates);
                    targetMonster.postureSystem.GenerateCurrentDieHandleStacks();
            //    }
            //}
        }
    }

    [HarmonyPatch(typeof(ButterflyBossFightLogic), nameof(ButterflyBossFightLogic.SetPhase))]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> SetPhaseTranspiler(IEnumerable<CodeInstruction> instructions) {
        var codes = instructions.ToList();
        if(codes.Count >= 219) {
            codes[213].opcode = OpCodes.Nop;
            codes[214].opcode = OpCodes.Nop;
            codes[215].opcode = OpCodes.Nop;
            codes[216].opcode = OpCodes.Nop;
            codes[217].opcode = OpCodes.Nop;
            codes[218].opcode = OpCodes.Nop;
        }
        return instructions;
    }

    [HarmonyPatch(typeof(ButterflyBossFightLogic), nameof(ButterflyBossFightLogic.ChangingPhase))]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> ChangingPhaseTranspiler(IEnumerable<CodeInstruction> instructions) {
        var codes = instructions.ToList();
        if (codes.Count >= 6) {
            codes[0].opcode = OpCodes.Nop;
            codes[1].opcode = OpCodes.Nop;
            codes[2].opcode = OpCodes.Nop;
            codes[3].opcode = OpCodes.Nop;
            codes[4].opcode = OpCodes.Nop;
            codes[5].opcode = OpCodes.Nop;
        }
        return instructions;
    }

    [HarmonyPatch(typeof(PlayerParryState), "NonAccurateParry")]
    [HarmonyPostfix]
    public static void ParryDamageModifier_Patch(PlayerParryState __instance, ref EffectHitData hitData, ref ParryParam param, ref DamageDealer bindDamage) {
        if (bindDamage.Owner == null) return;
        var modifiers = bindDamage.Owner.GetComponentsInChildren<ModifierBase>();
        if (modifiers != null) {
            if (modifiers.FirstOrDefault(m => m.Key == "parry_damage")?.enabled ?? false) {
                var playerHealth = Player.i.health;
                playerHealth.CurrentInternalInjury = 0;
                playerHealth.ResetRecoverableTime();
            }

            if (modifiers.FirstOrDefault(m => m.Key == "damage_buildup")?.enabled ?? false) {
                var playerHealth = Player.i.health;
                var damageAmount = bindDamage.DamageAmount;
                playerHealth.RecoverInternalInjury(damageAmount / 2);
            }
        }

    }

    [HarmonyPatch(typeof(PlayerParryState), nameof(PlayerParryState.Parried))]
    [HarmonyPrefix]
    public static void ParryMethodPatch(ref ParryParam param, ref DamageDealer bindDamage) {
        if(bindDamage.Owner == null) return;
        var modifiers = bindDamage.Owner.GetComponentsInChildren<ModifierBase>();
        if (modifiers != null) {
            if (modifiers.FirstOrDefault(m => m.Key == "knockback")?.enabled ?? false) {
                param.knockBackValue = param.knockBackValue * 2;
            }
        }
    }

    //[HarmonyPatch(typeof(Player), "OnTakeDamage")]
    //[HarmonyPrefix]
    //public static void PlayerTakeDamagePatch(ref DamageDealer damageDealer) {
    //    var modifiers = damageDealer.Owner.GetComponentsInChildren<ModifierBase>();
    //    if (modifiers != null) {
    //        if (modifiers.FirstOrDefault(m => m.Key == "stun")?.enabled ?? false) {
    //            var param = damageDealer.bindParriable.param;
    //            if(param.knockBackType != KnockBackType.Custom) {
    //                damageDealer.bindParriable.param.knockBackType = KnockBackType.Large;
    //            }
    //        }
    //    }
    //}

    [HarmonyPatch(typeof(Player), nameof(Player.NextItemCheck))]
    [HarmonyPrefix]
    public static bool NextItemPatch() {
        if (BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Any()) return false;
        return true;
    }

    [HarmonyPatch(typeof(Player), nameof(Player.PreviousItemCheck))]
    [HarmonyPrefix]
    public static bool PrevItemPatch() {
        if (BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Any()) return false;
        return true;
    }

    [HarmonyPatch(typeof(PlayerWeaponState), nameof(PlayerWeaponState.OnStateExit))]
    [HarmonyPostfix]
    public static void WeaponCheckPatch() {
        if(BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Any()) {
            System.Random random = new System.Random();
            int arrowsNum = Player.i.weaponDataCollection.AcquiredCount;
            int variantsNum = random.Next(1, arrowsNum);
            for (int i = 0; i < variantsNum; i++) {
                Player.i.weaponDataCollection.Next();
            }
        }
    }

    [HarmonyPatch(typeof(FooManager), "ExplodeWithDealer")]
    [HarmonyPostfix]
    public static void FooExplodePatch() {
        System.Random random = new System.Random();
        if (BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes.Any()) {
            string path = "GameCore(Clone)/RCG LifeCycle/UIManager/GameplayUICamera/UI-Canvas/[Tab] MenuTab/CursorProvider/Menu Vertical Layout/Panels/PlayerStatus Panel/Description Provider/LeftPart/PlayerStatusSelectableButton_ControlStyle";
            GameObject talismanSelectorGO = GameObject.Find(path);

            var selectorComp = talismanSelectorGO.GetComponent<CollectionRotateSelectorButton>();
            if (selectorComp != null) {
                var collection = selectorComp.collection;
                int talismansNum = collection.AcquiredCount;
                int variantsNum = random.Next(1, talismansNum);
                for (int i = 0; i < variantsNum; i++) {
                    collection.Next();
                }

                selectorComp.UpdateView();
                BossChallengeMod.Instance.UIController.UpdateTalisman(selectorComp.image.sprite);
            }

        }
    }

    [HarmonyPatch(typeof(MonsterBase), nameof(MonsterBase.HurtInterruptCheck))]
    [HarmonyPrefix]
    public static bool HurtInterruptPatch(MonsterBase __instance) {
        if(__instance.name.Contains("Boss_ButterFly")) {
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

    [HarmonyPatch(typeof(PlayerFooExplodeState), nameof(PlayerFooExplodeState.AnimationEvent))]
    [HarmonyPostfix]
    public static void FooExplodeAnimationEventPatch() {
        if (Player.i.mainAbilities.FooExplodeAutoStyle.IsActivated) {
            Player.i.ChangeState(PlayerStateType.Normal, false);
        }
    }

    [HarmonyPatch(typeof(CollectionRotateSelectorButton), "SubmitImplementation")]
    [HarmonyPrefix]
    public static bool FooSelectionPatchPrefix(CollectionRotateSelectorButton __instance) {
        if (__instance.gameObject.name == "PlayerStatusSelectableButton_ControlStyle" && BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes.Any()) {
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(CollectionRotateSelectorButton), "SubmitImplementation")]
    [HarmonyPostfix]
    public static void FooSelectionPatchPostfix(CollectionRotateSelectorButton __instance) {
        if (__instance.gameObject.name == "PlayerStatusSelectableButton_ControlStyle") {
            BossChallengeMod.Instance.UIController.UpdateTalisman(__instance.image.sprite);
        }
    }
}