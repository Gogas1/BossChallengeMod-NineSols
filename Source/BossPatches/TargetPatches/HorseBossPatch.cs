using BossChallengeMod.Configuration;
using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.BossPatches.TargetPatches {
    public class HorseBossPatch : RevivalChallengeBossPatch {

        protected override MonsterModifierController InitializeModifiers(MonsterBase monsterBase) {
            var result = base.InitializeModifiers(monsterBase);

            var shieldController = monsterBase.gameObject.GetComponent<MonsterShieldController>();
            if (shieldController != null) {
                GameObject.Destroy(shieldController);
            }

            return result;
        }

        protected override void PopulateModifierController(MonsterModifierController modifierController, ChallengeConfiguration config) {
            base.PopulateModifierController(modifierController, config);

            modifierController.ModifierConfigs.RemoveAll(m => m.Key.Contains("shield"));
        }
    }
}
