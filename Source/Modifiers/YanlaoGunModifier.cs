using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class YanlaoGunModifier : ModifierBase {

        protected MonsterYanlaoGunController? YanlaoGunController;

        public override void Awake() {
            base.Awake();

            Key = "ya_gun";

            YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            enabled = keys.Contains(Key);

            if(enabled) {
                YanlaoGunController?.StartGun();
            }
            else {
                YanlaoGunController?.StopGun();
            }
        }

    }
}
