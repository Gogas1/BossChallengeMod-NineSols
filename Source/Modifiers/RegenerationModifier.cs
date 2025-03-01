using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class RegenerationModifier : ModifierBase {
        private int regenerationPool;
        private float pausedFor = 0f;

        public float pauseTime = 1f;
        public int heal = 1;
        public float delay = 0.03f;

        protected float healReadyTimer = 0f;

        protected bool canHeal;

        public override void Awake() {
            try {
                base.Awake();
                Monster?.postureSystem.OnPostureDecrease.AddListener(PauseRegeneration);

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        public override void NotifyActivation() {
            try {
                base.NotifyActivation();

                enabled = true;
                regenerationPool = CalculateTotalHp() / 2;

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        public override void NotifyDeactivation() {
            try {
                base.NotifyDeactivation();

                enabled = false;

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        public void Update() {
            try {
                if (pausedFor > 0) {
                    pausedFor = Mathf.Max(0, pausedFor - Time.deltaTime);
                }

                var postureSystem = Monster?.postureSystem ?? null;
                canHeal = enabled &&
                    regenerationPool > 0 &&
                    !IsPaused &&
                    pausedFor <= 0 &&
                    postureSystem != null &&
                    !postureSystem.IsMonsterEmptyPosture &&
                    postureSystem.MaxPostureValue > postureSystem.CurrentHealthValue;

                if (canHeal) {
                    healReadyTimer += Time.deltaTime;
                    var healsNumber = (int)(healReadyTimer / delay);

                    if (healsNumber > 0) {
                        healReadyTimer = 0;

                        int hpToHeal = Mathf.Min((int)(postureSystem!.MaxPostureValue - postureSystem.CurrentHealthValue), heal * healsNumber);
                        postureSystem.GainPosture(hpToHeal);
                        regenerationPool -= hpToHeal;
                    }
                }

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        private void PauseRegeneration() {
            try {
                pausedFor = pauseTime;

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        private IEnumerator StartRegeneration() {
            var postureSystem = Monster?.postureSystem ?? null;
            while (canHeal && postureSystem != null) {
                if (!postureSystem.IsMonsterEmptyPosture && postureSystem.MaxPostureValue > postureSystem.CurrentHealthValue) {

                    int hpToHeal = Mathf.Min((int)(postureSystem.MaxPostureValue - postureSystem.CurrentHealthValue), heal);
                    postureSystem.GainPosture(hpToHeal);
                    regenerationPool -= hpToHeal;
                }

                yield return new WaitForSeconds(delay);
            }
        }

        private int CalculateTotalHp() {
            try {
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

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
                return 0;
            }
        }

        public void OnDestroy() {
            try {
                enabled = false;
                Monster?.postureSystem.OnPostureDecrease.RemoveListener(PauseRegeneration);

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }
    }
}
