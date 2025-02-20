using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class DamageBuildupModifier : ModifierBase {

        private bool started;
        public float damage = 0.025f;
        public float delay = 0.006f;
        public float damagePerSecond = 3.8f;

        public DamageBuildupModifier() {
            Key = "damage_buildup";
        }

        public override void Awake() {
            base.Awake();
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation(iteration);

            enabled = false;
        }

        private void Update() {
            if (!Monster!.postureSystem.IsMonsterEmptyPosture && 
                !IsPaused && 
                !Player.i.lockMoving &&
                !Player.i.freeze) {
                var playerHealth = Player.i.health;
                float damagePerFrame = damagePerSecond * Time.deltaTime;

                if (playerHealth.currentValue - damagePerFrame > 1f) {
                    playerHealth.currentValue -= damagePerFrame;

                    playerHealth.CurrentInternalInjury += Mathf.Min(
                        damagePerFrame,
                        playerHealth.maxHealth.Value - (playerHealth.currentValue - playerHealth.CurrentInternalInjury)
                    );
                    playerHealth.ResetRecoverableTime();
                }
            }
        }
    }
}
