using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod {
    public class ChallengeMonsterController : MonoBehaviour {

        private int _killCounter;
        public int KillCounter { get; private set; }

        public Action? OnRevivalStateEnter;
        public Action? OnRevivalStateExit;
        
        public Action? OnDie;

        public Action? OnEngage;
        public Action? OnDisengage;

        public void ProcessRevivalStateEnter() {
            KillCounter++;

            OnRevivalStateEnter?.Invoke();
        }

        public void ProcessDeath() {
            KillCounter++;
            OnDie?.Invoke();
        }
    }
}
