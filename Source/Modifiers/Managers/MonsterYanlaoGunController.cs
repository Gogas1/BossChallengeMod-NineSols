using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterYanlaoGunController : MonoBehaviour {
        protected GameObject? gunObject;



        private void Awake() {
            gunObject = BossChallengeMod.Instance.YanlaoGunProvider.GetGunCopy();
        }

        public void StartGun() {

        }

        public void StopGun() {

        }
    }
}
