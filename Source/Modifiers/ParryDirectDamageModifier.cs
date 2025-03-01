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

        public override void NotifyActivation() {
            base.NotifyActivation();

            enabled = true;
        }

        public override void NotifyDeactivation() {
            base.NotifyDeactivation();

            enabled = false;
        }
    }
}
