using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class QiShieldModifier : ShieldModifier {

        public override void Awake() {
            base.Awake();

            Key = "qi_shield";
        }
    }
}
