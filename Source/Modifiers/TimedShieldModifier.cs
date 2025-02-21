using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class TimedShieldModifier : ShieldModifier {

        private float timer = 7f;

        public override void Awake() {
            base.Awake();
        }

        private void Update() {
            if (!MonsterShieldController.IsShieldEnabled) {
                timer -= Time.deltaTime;

                if (timer <= 0) {
                    ActivateCheck();
                    timer = BossChallengeMod.Random.Next(5, 13);
                }
            }
        }

        public override void MonsterNotify(MonsterNotifyType notifyType) {
            
        }
    }
}
