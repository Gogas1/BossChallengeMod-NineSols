using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class ParryDirectDamageModifier : ModifierBase {

        public override void Awake() {
            base.Awake();
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation();

            enabled = false;
        }
    }
}
