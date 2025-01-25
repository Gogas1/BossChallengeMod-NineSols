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

        public override void Awake() {
            base.Awake();
            Key = "damage_buildup";
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);
            
            started = enabled = keys.Contains(Key);
            if (started) {
                StartCoroutine(StartBuildup());
            }
        }

        public IEnumerator StartBuildup() {
            while (started) {
                if (!Monster!.postureSystem.IsMonsterEmptyPosture) {
                    var playerHealth = Player.i.health;

                    if(playerHealth.currentValue - damage > 1f) {
                        playerHealth.currentValue = playerHealth.currentValue - damage;

                        playerHealth.CurrentInternalInjury += Mathf.Min(damage, playerHealth.maxHealth.Value - (playerHealth.currentValue - playerHealth.CurrentInternalInjury));
                        playerHealth.ResetRecoverableTime();
                    }
                }

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
