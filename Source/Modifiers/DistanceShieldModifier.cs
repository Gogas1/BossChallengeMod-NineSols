using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class DistanceShieldModifier : ShieldModifier {

        protected float distanceTreshhold = 225f;

        public override void Awake() {
            base.Awake();
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
