using BossChallengeMod.Modifiers.Managers;
using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class ShieldModifier : ModifierBase {

        protected MonsterShieldController MonsterShieldController = null!;

        public override void Awake() {
            base.Awake();
            Key = "shield";

            MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
        }

        public void AssignShieldController(MonsterShieldController monsterShieldController) {
            if(monsterShieldController != null) {
                MonsterShieldController = monsterShieldController;
            }
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            if (MonsterShieldController == null) {
                MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
            }

            if(MonsterShieldController == null) return;

            enabled = keys.Contains(Key);
            if(!enabled && MonsterShieldController.IsShieldEnabled) {
                MonsterShieldController?.Deactivate();
            }
        }

        public override void MonsterNotify(MonsterNotifyType notifyType) {
            base.MonsterNotify(notifyType);

            ActivateCheck();
        }

        protected void ActivateCheck() {
            if (enabled) {
                MonsterShieldController?.Activate();
            }
        }
    }
}
