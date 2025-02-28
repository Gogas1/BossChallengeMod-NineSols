using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class SpeedModifier : ModifierBase {

        public override void Awake() {
            base.Awake();
        }

        public override void NotifyActivation() {
            enabled = true;
        }

        public override void NotifyDeactivation() {
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
