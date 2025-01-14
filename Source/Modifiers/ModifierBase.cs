using BossChallengeMod.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class ModifierBase : MonoBehaviour {
        public string Key { get; protected set; } = string.Empty;
        public MonsterBase? Monster;
        public ChallengeConfiguration challengeConfiguration;

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

        public virtual void Notify(IEnumerable<string> keys, int iteration) {

        }
    }
}
