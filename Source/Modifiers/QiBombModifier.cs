using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class QiBombModifier : BombModifier {

        public QiBombModifier() {
            BombCount = 2;
        }

        public override void MonsterNotify(MonsterNotifyType notifyType) {
            if(notifyType == MonsterNotifyType.OnExplode && enabled && !IsPaused) {
                SpawnAtPlayer();
            }
        }
    }
}
