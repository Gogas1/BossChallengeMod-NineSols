using BossChallengeMod.Configuration;
using BossChallengeMod.Modifiers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossChallengeMod.KillCounting {
    public class MonsterKillCounter : MonoBehaviour {
        private ChallengeConfigurationManager challengeConfigurationManager = BossChallengeMod.Instance.ChallengeConfigurationManager;
        private ChallengeConfiguration challengeConfiguration = BossChallengeMod.Instance.ChallengeConfigurationManager.ChallengeConfiguration;
        private MonsterBase monster = null!;

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

        public Action? OnUpdate;
        public Action? OnDestroyActions;

        public MonsterKillCounter() {            
            monster = GetComponent<MonsterBase>();
        }

        public bool CheckLoad() {
            if (UseRecording) {
                var bossEntry = Task.Run<BossEntry>(() => challengeConfigurationManager.GetRecordForBoss(monster, challengeConfiguration)).GetAwaiter().GetResult();
                BestCount = bossEntry.BestValue;
                LastCount = bossEntry.LastValue;
            }

            return UseRecording;
        }

        public void IncrementCounter() {
            KillCounter++;
            if(KillCounter > BestCount) {
                BestCount = KillCounter;
            }
            if (UseRecording) {
                StartCoroutine(challengeConfigurationManager.SaveRecordForBoss(monster, challengeConfiguration, BestCount, KillCounter));
            }
        }

        public void OnDestroy() {
            OnDestroyActions?.Invoke();
        }
    }
}
