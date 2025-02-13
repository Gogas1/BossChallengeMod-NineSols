using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class SpeedModifier : ModifierBase {

        public SpeedModifier() {
            Key = "speed_temp";
        }

        public override void Awake() {
            base.Awake();
        }

        public override void NotifyActivation(int iteration) {
            enabled = true;
        }

        public override void NotifyDeactivation(int iteration) {
            enabled = false;
        }

        public override void OnEnable() {

        }

        public void Update() {
            if (Monster != null && !IsPaused) {
                Monster.animator.speed = Monster.animator.speed * 1.2f;
            }
        }

        public override void OnDisable() {

        }
    }
}
