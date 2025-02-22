using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class ShieldBreakBombModifier : BombModifier {

        public ShieldBreakBombModifier() {
            BombCount = 2;
        }

        public override void MonsterNotify(MonsterNotifyType notifyType) {
            if (notifyType == MonsterNotifyType.OnShieldBroken && enabled && !IsPaused) {
                SpawnAtPlayer();
            }
        }
    }
}
