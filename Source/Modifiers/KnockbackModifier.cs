using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class KnockbackModifier : ModifierBase {

        public override void Awake() {
            base.Awake();
            Key = "knockback";
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;
        }


        public override void NotifyDeactivation() {
            enabled = false;
        }
    }
}
