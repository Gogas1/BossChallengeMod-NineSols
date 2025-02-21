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
        public MonsterBase? Monster;
        public ChallengeConfiguration challengeConfiguration;
        public ChallengeEnemyType EnemyType { get; set; }

        public virtual void Awake() {
            Monster = GetComponentInParent<MonsterBase>();
            DisableComponent();
        }

        public virtual void OnEnable() {

        }

        public virtual void OnDisable() {

        }

        public void DisableComponent() {
            enabled = false;
        }

        public virtual void NotifyActivation(int iteration) {

        }

        public virtual void NotifyDeactivation() {

        }

        public virtual void NotifyDeactivation(int iteration = 0) {

        }

        public virtual void NotifyPause() {
            IsPaused = true;
        }

        public virtual void NotifyResume() {
            IsPaused = false;
        }

        public virtual void MonsterNotify(MonsterNotifyType notifyType) {

        }

        public virtual void SetController(Component controllerComponent) {
            throw new NotImplementedException();
        }
    }
}
