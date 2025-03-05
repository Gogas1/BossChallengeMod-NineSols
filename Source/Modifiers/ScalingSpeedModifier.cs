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
                        return challengeConfiguration.BossesEnableSpeedScaling;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesEnableSpeedScaling;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesEnableSpeedScaling;
                    default:
                        return false;
                }
            }
        }
        protected float MinSpeedScalingValue {
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
        protected float MaxSpeedScalingValue {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesMaxSpeedScalingValue;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesMaxSpeedScalingValue;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesMaxSpeedScalingValue;
                    default:
                        return 1f;
                }
            }
        }
        protected int MaxSpeedScalingCycle {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesMaxSpeedScalingCycle;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesMaxSpeedScalingCycle;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesMaxSpeedScalingCycle;
                    default:
                        return 1;
                }
            }
        }

        protected bool EnableRandomSpeedScaling {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesEnableRandomSpeedScaling;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesEnableRandomSpeedScaling;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesEnableRandomSpeedScaling;
                    default:
                        return false;
                }
            }
        }
        protected int RandomSpeedScalingStartDeath {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.BossesRandomSpeedScalingStartDeath;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MinibossesRandomSpeedScalingStartDeath;
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.EnemiesRandomSpeedScalingStartDeath;
                    default:
                        return 1;
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

        public override void Awake() {
            base.Awake();
            enabled = true;
        }

        public override void NotifyDeath(int deathNumber = 0) {
            base.NotifyDeath(deathNumber);

            modifier = CalculateModifier(deathNumber);
            if (Monster != null && (EnableSpeedScaling || EnableRandomSpeedScaling)) {
                Monster.animator.SetFloat("AnimationSpeed", Monster.animator.speed * modifier);
            }
        }

        public void Update() {
            if(Monster != null && (EnableSpeedScaling || EnableRandomSpeedScaling)) {
                Monster.animator.speed = Monster.animator.speed * modifier;
            }
        }

        public override void OnDisable() {
            Monster?.animator.SetFloat("AnimationSpeed", 1);
        }

        private float CalculateModifier(int iteration) {
            var result = 1f;

            if (EnableSpeedScaling) {
                float baseScalingValue = MinSpeedScalingValue;
                float progress = (float)iteration / Mathf.Max(MaxSpeedScalingCycle, 1);
                float progressMultiplier = Mathf.Min(1, progress);
                float scalingDiff = Mathf.Abs(MaxSpeedScalingValue - MinSpeedScalingValue);
                result *= baseScalingValue + scalingDiff * progressMultiplier;
            }

            if (EnableRandomSpeedScaling && iteration >= RandomSpeedScalingStartDeath) {
                float min = MathF.Min(MinRandomSpeedScalingValue, MaxRandomSpeedScalingValue);
                float max = MathF.Max(MinRandomSpeedScalingValue, MaxRandomSpeedScalingValue);

                float value = (float)(BossChallengeMod.Random.NextDouble() * (max - min) + min);

                result *= value;
            }

            return result;
        }
    }
}
