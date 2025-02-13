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
        public ShieldModifier() {
            Key = "shield";
        }
        public override void Awake() {
            base.Awake();

            MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
        }

        public void AssignShieldController(MonsterShieldController monsterShieldController) {
            if(monsterShieldController != null) {
                MonsterShieldController = monsterShieldController;
            }
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            if (MonsterShieldController == null) {
                MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
            }

            if(MonsterShieldController == null) return;

            enabled = true;
            
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation();

            enabled = false;

            if (MonsterShieldController == null) {
                MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
            }

            if (MonsterShieldController?.IsShieldEnabled ?? false) {
                MonsterShieldController?.Deactivate();
            }
        }

        public override void NotifyPause() {
            base.NotifyPause();

            if (MonsterShieldController == null) {
                MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
            }

            if (MonsterShieldController?.IsShieldEnabled ?? false) {
                MonsterShieldController?.Deactivate();
            }
        }

        public override void MonsterNotify(MonsterNotifyType notifyType) {
            base.MonsterNotify(notifyType);

            if (MonsterShieldController == null) {
                MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
            }

            ActivateCheck();
        }

        protected void ActivateCheck() {
            if (enabled && !IsPaused) {
                MonsterShieldController?.Activate();
            }
        }
    }
}
