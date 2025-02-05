using BossChallengeMod.Configuration;
using BossChallengeMod.Interfaces;
using BossChallengeMod.Modifiers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossChallengeMod.KillCounting {
    public class MonsterKillCounter : MonoBehaviour, IResettableComponent {
        private ChallengeConfigurationManager challengeConfigurationManager = BossChallengeMod.Instance.ChallengeConfigurationManager;
        private StoryChallengeConfigurationManager storyChallengeConfigurationManager = BossChallengeMod.Instance.StoryChallengeConfigurationManager;
        private MonsterBase monster = null!;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                if (ApplicationCore.IsInBossMemoryMode) return challengeConfigurationManager.ChallengeConfiguration;
                else return storyChallengeConfigurationManager.ChallengeConfiguration;
            }
        }

        protected bool CanRecord {
            get {
                return UseRecording && !ApplicationCore.IsInBossMemoryMode;
            }
        }        

        private ChallengeConfiguration challengeConfiguration;
        public ChallengeEnemyType EnemyType { get; set; }

        private int killCounter;
        public int KillCounter { 
            get => killCounter; 
            private set { 
                killCounter = value;
                OnUpdate?.Invoke();
            }
        }

        private int bestCount;
        public int BestCount { 
            get => bestCount;
            private set { 
                bestCount = value;
                OnUpdate?.Invoke();
            }
        }

        private int lastCount;
        public int LastCount {
            get => lastCount;
            private set { 
                lastCount = value;
                OnUpdate?.Invoke();
            }
        }
        public bool UseRecording { get; set; }
        public bool CanBeTracked { get; set; }

        public int MaxBossCycles { get; set; } = -1;

        public Action? OnUpdate { get; set; }
        public Action? OnDestroyActions { get; set; }

        public MonsterKillCounter() {            
            monster = GetComponent<MonsterBase>();
            challengeConfiguration = ConfigurationToUse;
        }

        public void CheckInit() {
            if (CanRecord) {
                var bossEntry = Task.Run<BossEntry>(() => challengeConfigurationManager.GetRecordForBoss(monster, challengeConfiguration)).GetAwaiter().GetResult();
                BestCount = bossEntry.BestValue;
                LastCount = bossEntry.LastValue;
            }

            CalculateMaxCycles();
        }

        public void IncrementCounter() {
            KillCounter++;
            if(KillCounter > BestCount) {
                BestCount = KillCounter;
            }
            if (CanRecord) {
                StartCoroutine(challengeConfigurationManager.SaveRecordForBoss(monster, challengeConfiguration, BestCount, KillCounter, challengeConfiguration.UseSingleRecordKey));
            }
        }

        public void OnDestroy() {
            OnDestroyActions?.Invoke();
        }

        private void CalculateMaxCycles() {
            var random = new System.Random();

            switch (EnemyType) {
                case ChallengeEnemyType.Regular:
                    if (challengeConfiguration.RandomizeEnemyCyclesNumber) {
                        MaxBossCycles = random.Next(challengeConfiguration.MinRandomEnemyCycles, challengeConfiguration.MaxRandomEnemyCycles + 1);
                    } else {
                        MaxBossCycles = challengeConfiguration.MaxEnemyCycles;
                    }

                    break;
                case ChallengeEnemyType.Miniboss:
                    if (challengeConfiguration.RandomizeMiniBossCyclesNumber) {
                        MaxBossCycles = random.Next(challengeConfiguration.MinRandomMiniBossCycles, challengeConfiguration.MaxRandomMiniBossCycles + 1);
                    } else {
                        MaxBossCycles = challengeConfiguration.MaxMinibossCycles;
                    }

                    break;
                case ChallengeEnemyType.Boss:
                    if (challengeConfiguration.RandomizeBossCyclesNumber) {
                        MaxBossCycles = random.Next(challengeConfiguration.MinRandomBossCycles, challengeConfiguration.MaxRandomBossCycles + 1);
                    } else {
                        MaxBossCycles = challengeConfiguration.MaxBossCycles;
                    }

                    break;
            }
        }

        public void ResetComponent() {
            challengeConfiguration = ConfigurationToUse;
            KillCounter = 0;
            CheckInit();
            OnUpdate?.Invoke();
        }

        public int GetPriority() {
            return 0;
        }
    }
    public enum ChallengeEnemyType {
        Boss,
        Miniboss,
        Regular
    }
}
