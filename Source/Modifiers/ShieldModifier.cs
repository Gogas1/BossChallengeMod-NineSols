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
        }

        public void AssignShieldController(MonsterShieldController monsterShieldController) {
            if(monsterShieldController != null) {
                MonsterShieldController = monsterShieldController;
            }
        }

        public override void NotifyActivation() {
            base.NotifyActivation();

            if (MonsterShieldController == null) {
                MonsterShieldController = gameObject.GetComponentInParent<MonsterShieldController>();
            }

            if(MonsterShieldController == null) return;

            enabled = true;
            
        }

        public override void NotifyDeactivation() {
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

        public override void CustomNotify(object message) {
            base.CustomNotify(message);
        }

        protected void ActivateCheck() {
            if (enabled && !IsPaused) {
                MonsterShieldController?.Activate();
            }
        }

        public override void SetController(Component controllerComponent) {
            if (controllerComponent is MonsterShieldController monsterShieldController) {
                MonsterShieldController = monsterShieldController;
            }
        }
    }
}
