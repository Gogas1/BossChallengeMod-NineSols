using BossChallengeMod.BossPatches;
using BossChallengeMod.Configuration;
using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.UIElements.StylePropertyAnimationSystem;

namespace BossChallengeMod.Modifiers {
    public class ScalingSpeedModifier : ModifierBase {
        private float Modifier {
            get => linearScaling * randomScaling;
        }
        
        private float linearScaling = 1.0f;
        private float randomScaling = 1.0f;

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


        protected void HandleSpeedScalingReconfiguration<T>(T param) {
            linearScaling = CalculateLinearModifier(deathNumber);
        }

        protected void HandleRandomSpeedScalingReconfiguration<T>(T param) {
            randomScaling = CalculateRandomModifier();
        }

        protected void HandleIsEnableReconfiguration() {
            linearScaling = CalculateLinearModifier(deathNumber);
            randomScaling = CalculateRandomModifier();
        }

        public ScalingSpeedModifier() {
            enabled = false;
            
        }

        public override void Awake() {
            base.Awake();

            switch (EnemyType) {
                case ChallengeEnemyType.Boss:
                    challengeConfiguration.OnBossesIsSpeedScalingEnabledChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesMinSpeedScalingValueChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesSpeedScalingStepValueChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesSpeedStepsCapValueChanged += HandleSpeedScalingReconfiguration;

                    challengeConfiguration.OnBossesIsRandomSpeedScalingEnabledChanged += HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesMinRandomSpeedScalingValueChanged += HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesMaxRandomSpeedScalingValueChanged += HandleRandomSpeedScalingReconfiguration;
                    break;
                case ChallengeEnemyType.Miniboss:
                    challengeConfiguration.OnMinibossesIsSpeedScalingEnabledChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesMinSpeedScalingValueChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesSpeedScalingStepValueChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesSpeedStepsCapValueChanged += HandleSpeedScalingReconfiguration;

                    challengeConfiguration.OnMinibossesIsRandomSpeedScalingEnabledChanged += HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesMinRandomSpeedScalingValueChanged += HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesMaxRandomSpeedScalingValueChanged += HandleRandomSpeedScalingReconfiguration;
                    break;
                case ChallengeEnemyType.Regular:
                    challengeConfiguration.OnEnemiesIsSpeedScalingEnabledChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesMinSpeedScalingValueChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesSpeedScalingStepValueChanged += HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesSpeedStepsCapValueChanged += HandleSpeedScalingReconfiguration;

                    challengeConfiguration.OnEnemiesIsRandomSpeedScalingEnabledChanged += HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesMinRandomSpeedScalingValueChanged += HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesMaxRandomSpeedScalingValueChanged += HandleRandomSpeedScalingReconfiguration;
                    break;
            }
        }

        public override void NotifyDeath(int deathNumber = 0) {
            base.NotifyDeath(deathNumber);

            linearScaling = CalculateLinearModifier(deathNumber);
            randomScaling = CalculateRandomModifier();
            //if (Monster != null && (EnableSpeedScaling || EnableRandomSpeedScaling)) {
            //    Monster.animator.SetFloat("AnimationSpeed", Monster.animator.speed * modifier);
            //}
        }

        public override void NotifyEngage() {
            base.NotifyEngage();
            enabled = true;
            linearScaling = CalculateLinearModifier(deathNumber);
            randomScaling = CalculateRandomModifier();
        }

        public override void NotifyDisengage() {
            base.NotifyDisengage();
            enabled = false;
        }

        public void Update() {
            if(Monster != null && (EnableSpeedScaling || EnableRandomSpeedScaling) && Modifier != 1f) {
                Monster.animator.speed = Monster.animator.speed * Modifier;
            }
        }

        public override void OnDisable() {
            Monster?.animator.SetFloat("AnimationSpeed", 1);
        }

        private float CalculateLinearModifier(int iteration) {
            var result = 1f;
            if (EnableSpeedScaling) {
                int stepsToScale = iteration;
                if (ScalingStepsCap != -1) {
                    stepsToScale = Math.Min(iteration, ScalingStepsCap);
                }

                result *= InitialScalingValue + stepsToScale * ScalingStepValue;
            }

            return result;
        }

        private float CalculateRandomModifier() {
            float result = 1f;

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
                    challengeConfiguration.OnBossesIsSpeedScalingEnabledChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesMinSpeedScalingValueChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesSpeedScalingStepValueChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesSpeedStepsCapValueChanged -= HandleSpeedScalingReconfiguration;

                    challengeConfiguration.OnBossesIsRandomSpeedScalingEnabledChanged -= HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesMinRandomSpeedScalingValueChanged -= HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnBossesMaxRandomSpeedScalingValueChanged -= HandleRandomSpeedScalingReconfiguration;
                    break;
                case ChallengeEnemyType.Miniboss:
                    challengeConfiguration.OnMinibossesIsSpeedScalingEnabledChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesMinSpeedScalingValueChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesSpeedScalingStepValueChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesSpeedStepsCapValueChanged -= HandleSpeedScalingReconfiguration;

                    challengeConfiguration.OnMinibossesIsRandomSpeedScalingEnabledChanged -= HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesMinRandomSpeedScalingValueChanged -= HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnMinibossesMaxRandomSpeedScalingValueChanged -= HandleRandomSpeedScalingReconfiguration;
                    break;
                case ChallengeEnemyType.Regular:
                    challengeConfiguration.OnEnemiesIsSpeedScalingEnabledChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesMinSpeedScalingValueChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesSpeedScalingStepValueChanged -= HandleSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesSpeedStepsCapValueChanged -= HandleSpeedScalingReconfiguration;

                    challengeConfiguration.OnEnemiesIsRandomSpeedScalingEnabledChanged -= HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesMinRandomSpeedScalingValueChanged -= HandleRandomSpeedScalingReconfiguration;
                    challengeConfiguration.OnEnemiesMaxRandomSpeedScalingValueChanged -= HandleRandomSpeedScalingReconfiguration;
                    break;
            }
        }

        public override void CustomNotify(object message) {
            if (message is MonsterNotifyType notifyType) {
                if(notifyType == MonsterNotifyType.BeforeEngage) {
                    NotifyEngage();
                }
            }
        }
    }
}
