using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class RegenerationModifier : ModifierBase {
        private int regenerationPool;
        private bool started;
        private float pausedFor = 0f;

        public float pauseTime = 1f;
        public int heal = 1;
        public float delay = 0.04f;


        public override void Awake() {
            base.Awake();
            Key = "regeneration";
            Monster?.postureSystem.OnPostureDecrease.AddListener(PauseRegeneration);
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            started = enabled = keys.Contains(Key);

            if (started) {
                regenerationPool = CalculateTotalHp() / 2;
                StartCoroutine(StartRegeneration());
            }
        }

        public void Update() {
            if(pausedFor > 0) {
                pausedFor = Mathf.Max(0, pausedFor - Time.deltaTime);
            }
        }

        private void PauseRegeneration() {
            pausedFor = pauseTime;
        }

        private IEnumerator StartRegeneration() {
            var postureSystem = Monster?.postureSystem ?? null;
            while (started && regenerationPool > 0 && postureSystem != null) {
                if (!postureSystem.IsMonsterEmptyPosture && postureSystem.MaxPostureValue > postureSystem.CurrentHealthValue && pausedFor <= 0f) {

                    int hpToHeal = Mathf.Min((int)(postureSystem.MaxPostureValue - postureSystem.CurrentHealthValue), heal);
                    postureSystem.GainPosture(hpToHeal);
                    regenerationPool -= hpToHeal;
                }

                yield return new WaitForSeconds(delay);
            }
        }

        private int CalculateTotalHp() {
            int totalHP = default;
            if(Monster != null) {                 
                var monsterStat = Monster.monsterStat;
                totalHP = (int)monsterStat.HealthValue;

                if (monsterStat.PhaseCount > 1) {
                    totalHP += (int)monsterStat.HealthValue * (int)monsterStat.Phase2HealthRatio;
                }

                if (monsterStat.PhaseCount > 2) {
                    totalHP += (int)monsterStat.HealthValue * (int)monsterStat.Phase3HealthRatio;
                }
            }

            return totalHP;
        }

        public void OnDestroy() {
            Monster?.postureSystem.OnPostureDecrease.RemoveListener(PauseRegeneration);
            started = false;
        }
    }
}
