using BossChallengeMod.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod {
    public class ChallengeMonsterController : MonoBehaviour, IResettableComponent {

        private int _killCounter;
        public int KillCounter { get; private set; }

        private MonsterBase _monster;

        public Action? OnRevivalStateEnter;
        public Action? OnRevivalStateExit;
        
        public Action? OnDie;

        public Action? OnEngage;
        public Action? OnDisengage;

        public List<MonsterBase.States> DieHandelingStates = new List<MonsterBase.States>();

        private void Awake() {
            _monster = GetComponent<MonsterBase>();
        }

        public void ProcessRevivalStateEnter() {
            KillCounter++;

            OnRevivalStateEnter?.Invoke();
        }

        public void ProcessDeath() {
            KillCounter++;
            OnDie?.Invoke();
        }

        public int GetPriority() {
            return -1;
        }

        public void ResetComponent() {
            KillCounter = 0;
            _monster.postureSystem.DieHandleingStates.Clear();
            _monster.postureSystem.DieHandleingStates.AddRange(DieHandelingStates);
            _monster.postureSystem.GenerateCurrentDieHandleStacks();   
        }
    }
}
