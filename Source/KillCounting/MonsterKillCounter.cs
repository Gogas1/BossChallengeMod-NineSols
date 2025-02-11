using BossChallengeMod.Configuration;
using BossChallengeMod.Interfaces;
using BossChallengeMod.Modifiers;
using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BossChallengeMod.KillCounting {
    public class MonsterKillCounter : MonoBehaviour, IResettableComponent {
        private ChallengeConfigurationManager challengeConfigurationManager = BossChallengeMod.Instance.ChallengeConfigurationManager;
        private StoryChallengeConfigurationManager storyChallengeConfigurationManager = BossChallengeMod.Instance.StoryChallengeConfigurationManager;
        private MonsterBase monster = null!;
        private ChallengeConfiguration challengeConfiguration;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                if (ApplicationCore.IsInBossMemoryMode) return challengeConfigurationManager.ChallengeConfiguration;
                else return storyChallengeConfigurationManager.ChallengeConfiguration;
            }
        }

        protected bool CanRecord {
            get {
                return UseRecording && ApplicationCore.IsInBossMemoryMode;
            }
        }        


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

        private float showDistance = 500f;
        public float ShowDistance 
        { 
            get => showDistance;
        }

        public ChallengeEnemyType EnemyType { get; set; }
        public bool UseRecording { get; set; }
        public bool CanBeTracked { get; set; }
        public int MaxBossCycles { get; set; } = -1;
        public bool UseProximityShow { get; set; }
        public Action? OnUpdate { get; set; }
        public Action? OnDestroyActions { get; set; }

        PlayerSensor playerSensor = null!;
        public MonsterKillCounter() {            
            monster = GetComponent<MonsterBase>();
            challengeConfiguration = ConfigurationToUse;
        }

        private bool PlayerEnterCheck(Collider2D other) {
            if (!other.CompareTag("Player")) {
                return false;
            }
            if (!(other.GetComponent<Actor>() is Player)) {
                return false;
            }

            return true;
        }

        private void Start() {
            if (UseProximityShow) {
                playerSensor = InitPlayerSensor();
            }
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
            switch (EnemyType) {
                case ChallengeEnemyType.Regular:
                    if (challengeConfiguration.RandomizeEnemyCyclesNumber) {
                        MaxBossCycles = BossChallengeMod.Random.Next(challengeConfiguration.MinRandomEnemyCycles, challengeConfiguration.MaxRandomEnemyCycles + 1);
                    } else {
                        MaxBossCycles = challengeConfiguration.MaxEnemyCycles;
                    }

                    break;
                case ChallengeEnemyType.Miniboss:
                    if (challengeConfiguration.RandomizeMiniBossCyclesNumber) {
                        MaxBossCycles = BossChallengeMod.Random.Next(challengeConfiguration.MinRandomMiniBossCycles, challengeConfiguration.MaxRandomMiniBossCycles + 1);
                    } else {
                        MaxBossCycles = challengeConfiguration.MaxMinibossCycles;
                    }

                    break;
                case ChallengeEnemyType.Boss:
                    if (challengeConfiguration.RandomizeBossCyclesNumber) {
                        MaxBossCycles = BossChallengeMod.Random.Next(challengeConfiguration.MinRandomBossCycles, challengeConfiguration.MaxRandomBossCycles + 1);
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

        private PlayerSensor InitPlayerSensor() {
            CircleCollider2D showTrigger;
            PlayerSensor sensor;

            var sensorFolder = new GameObject("ChallengePlayerSensorKillCounter");
            sensorFolder.transform.SetParent(transform, false);

            showTrigger = sensorFolder.AddComponent<CircleCollider2D>();
            showTrigger.isTrigger = true;
            showTrigger.radius = ShowDistance;

            sensor = sensorFolder.AddComponent<PlayerSensor>();
            sensor.PlayerEnterEvent = new UnityEngine.Events.UnityEvent();
            sensor.PlayerExitEvent = new UnityEngine.Events.UnityEvent();
            sensor.PlayerStayEvent = new UnityEngine.Events.UnityEvent();

            AutoAttributeManager.AutoReference(sensorFolder);
            sensor.Awake();
            sensor.EnterLevelReset();

            sensor.PlayerEnterEvent.AddListener(() => {
                BossChallengeMod.Instance.MonsterUIController.ChangeKillCounter(this);
            });
            sensor.PlayerExitEvent.AddListener(() => {
                BossChallengeMod.Instance.MonsterUIController.ChangeKillCounter(null);
            });

            return sensor;
        }
    }
    public enum ChallengeEnemyType {
        Boss,
        Miniboss,
        Regular
    }
}
