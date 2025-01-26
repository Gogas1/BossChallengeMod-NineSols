using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterYanlaoGunController : MonoBehaviour {
        protected GameObject? gunObject;

        protected GeneralState? StatePaused;
        protected GeneralState? StateStart;

        protected GeneralFSMContext? gunFsmContext;

        protected string pausedStateName = "";
        protected string startStateName = "";

        private void Awake() {
            gunObject = BossChallengeMod.Instance.YanlaoGunProvider.GetGunCopy();

            if (gunObject == null) return;
            
            gunFsmContext = gunObject.GetComponentInChildren<GeneralFSMContext>();            

            StatePaused = gunFsmContext.transform.Find(pausedStateName)?.GetComponent<GeneralState>() ?? null;
            StateStart = gunFsmContext.transform.Find(startStateName)?.GetComponent<GeneralState>() ?? null;
        }

        public void StartGun() {
            if(gunFsmContext == null || StateStart == null) return;

            gunFsmContext.ChangeState(StateStart);
        }

        public void StopGun() {
            if (gunFsmContext == null || StatePaused == null) return;

            gunFsmContext.ChangeState(StatePaused);
        }
    }
}
