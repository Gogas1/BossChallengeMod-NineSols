using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterYanlaoGunController : MonoBehaviour {
        protected GameObject? GunObject { get; private set; }

        protected GeneralState? StatePaused;
        protected GeneralState? StateStart;

        protected GeneralFSMContext? gunFsmContext;

        protected string pausedStateName = "[State] PlayerInSafeZone";
        protected string startStateName = "[State] Follow";

        protected readonly FieldInfo gunMaxSpeedFieldRef = AccessTools.Field(typeof(A4_S4_ZGunLogic), "maxSpeed");

        private void Awake() {
            GunObject = BossChallengeMod.Instance.YanlaoGunProvider.GetGunCopy();

            if (GunObject == null) return;
            
            gunFsmContext = GunObject.GetComponentInChildren<GeneralFSMContext>();            

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

        protected void TuneTheGun(GameObject? gunObject) {
            if (gunObject == null) return;

            var gunComponent = gunObject?.GetComponentInChildren<A4_S4_ZGunLogic>() ?? null;

            if (gunComponent == null) return;

            gunMaxSpeedFieldRef.SetValue(gunComponent, 300f);
            gunComponent.NormalLerpingSpeed = 2f;
        }
    }
}
