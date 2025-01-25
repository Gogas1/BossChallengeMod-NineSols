using BossChallengeMod.Preloading;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.ObjectProviders {
    public class ShieldProvider : IPreloadTarget {
        private GameObject? shieldCopy = null;

        public void Set(GameObject? preloaded) {
            shieldCopy = preloaded;
        }

        public GameObject? GetShieldCopy() {
            return GameObject.Instantiate(shieldCopy);
        }
    }
}
