using InControl;
using BossChallengeMod.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class TimerModifier : ModifierBase {
        private bool start = false;
        private int time = 10000;

        private UIController UIController = BossChallengeMod.Instance.UIController;

        private List<int> attempts = new List<int>();
        private int stopwatch;
        private bool startStopwatch;

        protected Coroutine? timerCoroutine;

        public bool ForcePause { get; set; } = false;

        public override void Awake() {
            base.Awake();
            enabled = true;
            startStopwatch = true;
        }

        public void Update() {
            if (startStopwatch && (!Monster?.postureSystem.IsMonsterEmptyPosture ?? false)) {
                stopwatch += (int)(Time.deltaTime * 1000);
            }
        }

        public override void NotifyActivation(int iteration) {
            if (Monster != null && iteration != 0) {
                if (timerCoroutine != null) {
                    StopCoroutine(timerCoroutine);
                }

                attempts.Add(stopwatch);
                stopwatch = 0;
                start = true;
                if (start) {
                    time = (int)CalculateTime(attempts.ToArray(), iteration);
                    StartCoroutine(StartTimer());
                }
            }            
        }

        public override void NotifyDeactivation(int iteration) {
            attempts.Add(stopwatch);
            stopwatch = 0;
            start = false;
        }

        public IEnumerator StartTimer() {
            UIController.ShowTimer();
            int remainingTime = time;
            UIController.UpdateTimer(remainingTime);

            while (remainingTime > 0 && start) {
                if(!Monster!.postureSystem.IsMonsterEmptyPosture && !ForcePause && !IsPaused) {
                    remainingTime -= (int)(Time.deltaTime * 1000);
                }
                UIController.UpdateTimer(remainingTime >= 0 ? remainingTime : 0);
                yield return null;
            }

            if(remainingTime <= 0) {
                Player.i.Suicide();
            }

            yield return new WaitForSeconds(2);

            if(!enabled) {
                UIController.HideTimer();
            }
        }

        public void OnDestroy() {
            start = false;
            UIController.HideTimer();
        }

        private double CalculateTime(int[] previousTimes, int attempts, double kMax = 5.0, double kMin = 2.0, int threshold = 10) {
            if (previousTimes == null || previousTimes.Length == 0)
                throw new ArgumentException("Previous times array cannot be null or empty.");
            if (attempts <= 0)
                throw new ArgumentException("Attempts must be greater than zero.");

            double mean = previousTimes.Average();
            double stdDev = Math.Sqrt(previousTimes.Select(t => Math.Pow(t - mean, 2)).Average());
            if (stdDev == 0) {
                stdDev = mean * 0.1;
            }

            double k = Math.Max(kMax - ((kMax - kMin) / threshold) * attempts, kMin);
            double strictTimer = mean + (k * stdDev);

            return strictTimer;
        }
    }
}
