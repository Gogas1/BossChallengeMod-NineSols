using BossChallengeMod.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class ScalingSpeedModifier : ModifierBase {
        private float modifier = 1.0f;

        public override void Awake() {
            base.Awake();
            Key = "speed_perm";
            enabled = true;
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            modifier = CalculateModifier(iteration);
            if (Monster != null && (challengeConfiguration.EnableSpeedScaling || challengeConfiguration.EnableRandomSpeedScaling)) {
                Monster.animator.SetFloat("AnimationSpeed", Monster.animator.speed * modifier);
            }
        }

        public void Update() {
            if(Monster != null && 
                (challengeConfiguration.EnableSpeedScaling || challengeConfiguration.EnableRandomSpeedScaling) && 
                Monster.animator.speed < Monster.animator.speed * modifier) {
                Monster.animator.SetFloat("AnimationSpeed", Monster.animator.speed * modifier);
            }
        }

        public override void OnDisable() {

        }

        private float CalculateModifier(int iteration) {
            var result = 1f;

            if (challengeConfiguration.EnableSpeedScaling) {
                float baseScalingValue = challengeConfiguration.MinSpeedScalingValue;
                float progress = (float)iteration / Mathf.Max(challengeConfiguration.MaxSpeedScalingCycle, 1);
                float progressMultiplier = Mathf.Min(1, progress);
                float scalingDiff = Mathf.Abs(challengeConfiguration.MaxSpeedScalingValue - challengeConfiguration.MinSpeedScalingValue);
                result *= baseScalingValue + scalingDiff * progressMultiplier;
            }

            if (challengeConfiguration.EnableRandomSpeedScaling && iteration >= challengeConfiguration.RandomSpeedScalingStartDeath) {
                float value = UnityEngine.Random.Range(
                    MathF.Min(challengeConfiguration.MinRandomSpeedScalingValue, challengeConfiguration.MaxRandomSpeedScalingValue),
                    MathF.Max(challengeConfiguration.MinRandomSpeedScalingValue, challengeConfiguration.MaxRandomSpeedScalingValue));

                result *= value;
            }

            return result;
        }
    }
}
