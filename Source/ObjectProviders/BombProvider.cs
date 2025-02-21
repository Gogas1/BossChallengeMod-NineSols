using BossChallengeMod.Preloading;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.ObjectProviders {
    public class BombProvider : IPreloadTarget {
        private GameObject? bombShooterCopy = null!;

        public void Set(GameObject? preloaded) {
            bombShooterCopy = preloaded;
        }

        public GameObject? GetBombShooterCopy() {
            var result = GameObject.Instantiate(bombShooterCopy);
            AutoAttributeManager.AutoReferenceAllChildren(result);
            return result;
        }
    }
}
