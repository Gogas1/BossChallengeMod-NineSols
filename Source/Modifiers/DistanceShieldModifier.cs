using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class DistanceShieldModifier : ShieldModifier {

        protected float distanceTreshhold = 100;

        public override void Awake() {
            base.Awake();

            Key = "distance_shield";            
        }

        private void Update() {
            if (!MonsterShieldController.IsShieldEnabled && Monster != null) {
                var distanceDifference = Player.i.transform.position.x - Monster.transform.position.x;
                if (distanceDifference >= distanceTreshhold) {
                    ActivateCheck();
                }
            }
        }
    }
}
