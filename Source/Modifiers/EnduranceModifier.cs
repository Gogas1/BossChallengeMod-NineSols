using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class EnduranceModifier : ModifierBase {
        public override void Awake() {
            base.Awake();
            Key = "endurance";
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            enabled = keys.Contains(Key);
        }
    }
}
