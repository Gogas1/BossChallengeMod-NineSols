using HarmonyLib;
using BossChallengeMod.BossPatches;
using BossChallengeMod.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection.Emit;
using NineSolsAPI;

namespace BossChallengeMod.Patches;

[HarmonyPatch]
public class Patches {

}

public enum MonsterNotifyType {
    Generic,
    OnExplode,
    OnChargeAttack
}