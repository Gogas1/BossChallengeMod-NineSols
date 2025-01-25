using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class TimedShieldModifier : ShieldModifier {

        private float timer = 7f;
        private System.Random random = new System.Random();

        public override void Awake() {
            base.Awake();

            Key = "timer_shield";
        }

        private void Update() {
            if (!MonsterShieldController.IsShieldEnabled) {
                timer -= Time.deltaTime;

                if (timer <= 0) {
                    ActivateCheck();
                    timer = random.Next(5, 13);
                }
            }
        }
    }
}
