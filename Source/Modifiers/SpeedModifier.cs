using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class SpeedModifier : ModifierBase {

        public override void Awake() {
            base.Awake();
            Key = "speed_temp";
        }

        public override void Notify(IEnumerable<string> keys, int iteration) {
            enabled = keys.Contains(Key);
        }

        public override void OnEnable() {

        }

        public void Update() {
            if (Monster != null) {
                Monster.animator.speed = Monster.animator.speed * 1.25f;
            }
        }

        public override void OnDisable() {

        }
    }
}
