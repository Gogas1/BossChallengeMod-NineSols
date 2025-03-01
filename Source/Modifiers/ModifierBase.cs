using BossChallengeMod.BossPatches;
using BossChallengeMod.Configuration;
using BossChallengeMod.Patches;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class ModifierBase : MonoBehaviour {
        public bool IsPaused { get; protected set; }
        public string Key { get; set; } = string.Empty;
        public MonsterBase? Monster { get; protected set; }
        public ChallengeConfiguration challengeConfiguration { get;  set; }
        public ChallengeEnemyType EnemyType { get; set; }

        protected int deathNumber;

        public ModifierBase() {
            DisableComponent();
        }

        public virtual void Awake() {
            Monster = GetComponentInParent<MonsterBase>();
        }

        public virtual void OnEnable() {

        }

        public virtual void OnDisable() {

        }

        public void DisableComponent() {
            enabled = false;
        }

        public virtual void NotifyActivation() {

        }

        public virtual void NotifyDeactivation() {

        }

        public virtual void NotifyDeath(int deathNumber = 0) {
            this.deathNumber = deathNumber;
        }

        public virtual void NotifyPause() {
            IsPaused = true;
        }

        public virtual void NotifyResume() {
            IsPaused = false;
        }

        public virtual void CustomNotify(object message) {

        }

        public virtual void SetController(Component controllerComponent) {
            throw new NotImplementedException();
        }
    }
}
