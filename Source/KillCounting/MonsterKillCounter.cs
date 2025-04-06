using BossChallengeMod.BossPatches;
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
        private ChallengeMonsterController monsterController = null!;
        private MonsterBase monster = null!;
        private ChallengeConfiguration challengeConfiguration;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                return challengeConfigurationManager.ChallengeConfiguration;
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
        public int MaxBossCycles {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Regular:
                        return challengeConfiguration.MaxEnemyCycles;
                    case ChallengeEnemyType.Miniboss:
                        return challengeConfiguration.MaxMinibossCycles;
                    case ChallengeEnemyType.Boss:
                        return challengeConfiguration.MaxBossCycles;
                    default:
                        return -1;
                }
            }
        }

        public MonsterBase.States MonsterResetState;
        public ChallengeEnemyType EnemyType { get; set; }
        public bool UseRecording { get; set; }
        public bool CanBeTracked { get; set; }
        public bool UseProximityShow { get; set; }
        public Action? OnUpdate { get; set; }
        public Action? OnDestroyActions { get; set; }

        PlayerSensor playerSensor = null!;
        public MonsterKillCounter() {            
            monster = GetComponent<MonsterBase>();
            challengeConfiguration = ConfigurationToUse;
            monsterController = GetComponent<ChallengeMonsterController>();
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

        }

        public void CheckInit() {
            KillCounter = monsterController.KillCounter;
            if (CanRecord) {
                var bossEntry = Task.Run<BossEntry>(() => challengeConfigurationManager.GetRecordForBoss(monster, challengeConfiguration)).GetAwaiter().GetResult();
                BestCount = bossEntry.BestValue;
                LastCount = bossEntry.LastValue;
            }
        }

        public void UpdateCounter() {
            KillCounter = monsterController.KillCounter;
            if(KillCounter > BestCount) {
                BestCount = KillCounter;
            }
            if (CanRecord) {
                StartCoroutine(challengeConfigurationManager.SaveRecordForBoss(monster, challengeConfiguration, BestCount, KillCounter, true));
            }
        }

        public void OnDie() {
            OnDestroyActions?.Invoke();
        }

        public void OnDestroing() {
            OnDestroyActions?.Invoke();
        }

        public void OnEngage() {
            if(CanBeTracked && UseProximityShow) {
                BossChallengeMod.Instance.MonsterUIController.ChangeKillCounter(this);
            }
        }

        public void OnDisengage() {
            if (CanBeTracked && UseProximityShow) {
                BossChallengeMod.Instance.MonsterUIController.ChangeKillCounter(null);
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
    
}
