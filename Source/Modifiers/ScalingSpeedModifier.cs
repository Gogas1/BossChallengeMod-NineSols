using BossChallengeMod.BossPatches;
using BossChallengeMod.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class ScalingSpeedModifier : ModifierBase {
        private float modifier = 1.0f;
        

        protected bool EnableSpeedScaling {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesIsSpeedScalingEnabled;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesIsSpeedScalingEnabled;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesIsSpeedScalingEnabled;
                    default:
                        return false;
                }
            }
        }
        protected float InitialScalingValue {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesMinSpeedScalingValue;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesMinSpeedScalingValue;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesMinSpeedScalingValue;
                    default:
                        return 1f;
                }
            }
        }
        protected float ScalingStepValue {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesSpeedScalingStepValue;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesSpeedScalingStepValue;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesSpeedScalingStepValue;
                    default:
                        return 1f;
                }
            }
        }
        protected int ScalingStepsCap {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesSpeedStepsCapValue;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesSpeedStepsCapValue;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesSpeedStepsCapValue;
                    default:
                        return 1;
                }
            }
        }

        protected bool EnableRandomSpeedScaling {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesIsRandomSpeedScalingEnabled;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesIsRandomSpeedScalingEnabled;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesIsRandomSpeedScalingEnabled;
                    default:
                        return false;
                }
            }
        }
        protected float MinRandomSpeedScalingValue {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesMinRandomSpeedScalingValue;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesMinRandomSpeedScalingValue;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesMinRandomSpeedScalingValue;
                    default:
                        return 1f;
                }
            }
        }
        protected float MaxRandomSpeedScalingValue {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesMaxRandomSpeedScalingValue;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesMaxRandomSpeedScalingValue;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesMaxRandomSpeedScalingValue;
                    default:
                        return 1f;
                }
            }
        }

        protected void HandleSpeedScalingReconfiguration() {
            modifier = CalculateModifier(deathNumber);
        }

        public override void Awake() {
            base.Awake();
            enabled = true;

            switch (EnemyType) {
                case ChallengeEnemyType.Boss:
                    challengeConfiguration.OnBossesIsSpeedScalingEnabledChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesMinSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesSpeedScalingStepValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesSpeedStepsCapValueChanged += _ => HandleSpeedScalingReconfiguration();

                    challengeConfiguration.OnBossesIsRandomSpeedScalingEnabledChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesMinRandomSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesMaxRandomSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    break;
                case ChallengeEnemyType.Miniboss:
                    challengeConfiguration.OnMinibossesIsSpeedScalingEnabledChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesMinSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesSpeedScalingStepValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesSpeedStepsCapValueChanged += _ => HandleSpeedScalingReconfiguration();

                    challengeConfiguration.OnMinibossesIsRandomSpeedScalingEnabledChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesMinRandomSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesMaxRandomSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    break;
                case ChallengeEnemyType.Regular:
                    challengeConfiguration.OnEnemiesIsSpeedScalingEnabledChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesMinSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesSpeedScalingStepValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesSpeedStepsCapValueChanged += _ => HandleSpeedScalingReconfiguration();

                    challengeConfiguration.OnEnemiesIsRandomSpeedScalingEnabledChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesMinRandomSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesMaxRandomSpeedScalingValueChanged += _ => HandleSpeedScalingReconfiguration();
                    break;
            }
        }

        public override void NotifyDeath(int deathNumber = 0) {
            base.NotifyDeath(deathNumber);

            modifier = CalculateModifier(deathNumber);
            //if (Monster != null && (EnableSpeedScaling || EnableRandomSpeedScaling)) {
            //    Monster.animator.SetFloat("AnimationSpeed", Monster.animator.speed * modifier);
            //}
        }

        public override void NotifyEngage() {
            base.NotifyEngage();
            enabled = true;
            modifier = CalculateModifier(deathNumber);
        }

        public override void NotifyDisengage() {
            base.NotifyDisengage();
            enabled = false;
        }

        public void Update() {
            if(Monster != null && (EnableSpeedScaling || EnableRandomSpeedScaling) && modifier != 1f) {
                Monster.animator.speed = Monster.animator.speed * modifier;
            }
        }

        public override void OnDisable() {
            Monster?.animator.SetFloat("AnimationSpeed", 1);
        }

        private float CalculateModifier(int iteration) {
            var result = 1f;

            if (EnableSpeedScaling) {
                int stepsToScale = iteration;
                if (ScalingStepsCap != -1) {
                    stepsToScale = Math.Min(iteration, ScalingStepsCap);
                }

                result *= InitialScalingValue + stepsToScale * ScalingStepValue;
            }

            if (EnableRandomSpeedScaling) {
                float min = MathF.Min(MinRandomSpeedScalingValue, MaxRandomSpeedScalingValue);
                float max = MathF.Max(MinRandomSpeedScalingValue, MaxRandomSpeedScalingValue);

                float value = (float)(BossChallengeMod.Random.NextDouble() * (max - min) + min);

                result *= value;
            }

            return result;
        }

        public override void NotifyDestroing() {
            base.NotifyDestroing();

            switch (EnemyType) {
                case ChallengeEnemyType.Boss:
                    challengeConfiguration.OnBossesIsSpeedScalingEnabledChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesMinSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesSpeedScalingStepValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesSpeedStepsCapValueChanged -= _ => HandleSpeedScalingReconfiguration();

                    challengeConfiguration.OnBossesIsRandomSpeedScalingEnabledChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesMinRandomSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnBossesMaxRandomSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    break;
                case ChallengeEnemyType.Miniboss:
                    challengeConfiguration.OnMinibossesIsSpeedScalingEnabledChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesMinSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesSpeedScalingStepValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesSpeedStepsCapValueChanged -= _ => HandleSpeedScalingReconfiguration();

                    challengeConfiguration.OnMinibossesIsRandomSpeedScalingEnabledChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesMinRandomSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnMinibossesMaxRandomSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    break;
                case ChallengeEnemyType.Regular:
                    challengeConfiguration.OnEnemiesIsSpeedScalingEnabledChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesMinSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesSpeedScalingStepValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesSpeedStepsCapValueChanged -= _ => HandleSpeedScalingReconfiguration();

                    challengeConfiguration.OnEnemiesIsRandomSpeedScalingEnabledChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesMinRandomSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    challengeConfiguration.OnEnemiesMaxRandomSpeedScalingValueChanged -= _ => HandleSpeedScalingReconfiguration();
                    break;
            }
        }
    }
}
