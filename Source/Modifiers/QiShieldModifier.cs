using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class QiShieldModifier : ShieldModifier {

        public override void Awake() {
            base.Awake();
        }

        public override void MonsterNotify(object message) {
            if (message is not MonsterNotifyType notifyType) {
                return;
            }

            if (notifyType == MonsterNotifyType.OnExplode) {
                base.MonsterNotify(notifyType);
            }
        }
    }
}
