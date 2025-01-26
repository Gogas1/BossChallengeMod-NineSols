using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class DistanceShieldModifier : ShieldModifier {

        protected float distanceTreshhold = 275;

        public override void Awake() {
            base.Awake();

            Key = "distance_shield";            
        }

        private void Update() {
            if (!MonsterShieldController.IsShieldEnabled && Monster != null) {
                var distanceDifference = Vector2.Distance(Player.i.transform.position, Monster.transform.position);
                if (distanceDifference >= distanceTreshhold) {
                    ActivateCheck();
                }
            }
        }

        public override void MonsterNotify(MonsterNotifyType notifyType) {
            
        }
    }
}
