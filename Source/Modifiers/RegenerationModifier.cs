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
        public float delay = 0.04f;

        protected Coroutine? regenerationCoroutine;

        protected bool canHeal = false;
        protected bool coroutineRunning = false;

        public override void Awake() {
            try {
                base.Awake();
                Monster?.postureSystem.OnPostureDecrease.AddListener(PauseRegeneration);

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        public override void NotifyActivation(int iteration) {
            try {
                base.NotifyActivation(iteration);

                enabled = true;
                regenerationPool = CalculateTotalHp() / 2;

                StartRegenerationCoroutine();
            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        public override void NotifyDeactivation(int iteration) {
            try {
                base.NotifyDeactivation();

                enabled = false;
                canHeal = enabled && regenerationPool > 0 && !IsPaused && pausedFor <= 0;

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }

        public void Update() {
            try {
                if (pausedFor > 0) {
                    pausedFor = Mathf.Max(0, pausedFor - Time.deltaTime);
                }

                canHeal = enabled && regenerationPool > 0 && !IsPaused && pausedFor <= 0;

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
            coroutineRunning = true;
            while (canHeal && postureSystem != null) {
                if (!postureSystem.IsMonsterEmptyPosture && postureSystem.MaxPostureValue > postureSystem.CurrentHealthValue) {

                    int hpToHeal = Mathf.Min((int)(postureSystem.MaxPostureValue - postureSystem.CurrentHealthValue), heal);
                    postureSystem.GainPosture(hpToHeal);
                    regenerationPool -= hpToHeal;
                }

                yield return new WaitForSeconds(delay);
            }
            coroutineRunning = false;
        }

        protected void StartRegenerationCoroutine() {
            if (enabled && gameObject.activeSelf && !IsPaused) {
                if (regenerationCoroutine != null) {
                    StopCoroutine(regenerationCoroutine);
                    coroutineRunning = false;
                }

                regenerationCoroutine = StartCoroutine(StartRegeneration());
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

        public override void OnEnable() {
            try {
                base.OnEnable();

                if (!coroutineRunning) {
                    regenerationPool = CalculateTotalHp() / 2;
                    StartRegenerationCoroutine();
                }

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
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

        public override void NotifyPause() {
            base.NotifyPause();
        }

        public override void NotifyResume() {
            try {
                base.NotifyResume();

                if (!coroutineRunning) {
                    StartRegenerationCoroutine();
                }

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }
    }
}
