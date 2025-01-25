﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class ParryDirectDamageModifier : ModifierBase {

        public override void Awake() {
            base.Awake();
            Key = "parry_damage";
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            enabled = keys.Contains(Key);
        }
    }
}
