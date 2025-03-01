using BossChallengeMod.Interfaces;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterYanlaoGunController : MonoBehaviour, IResettableComponent {
        protected GameObject? GunObject { get; private set; }

        protected GeneralState? StatePaused;
        protected GeneralState? StateStart;

        protected List<GeneralState> StatesRunning = new List<GeneralState>();

        protected GeneralFSMContext? gunFsmContext;

        protected string pausedStateName = "[State] PlayerInSafeZone";
        protected string startStateName = "[State] Follow";

        protected readonly FieldInfo gunMaxSpeedFieldRef = AccessTools.Field(typeof(A4_S4_ZGunLogic), "maxSpeed");

        public bool IsRunning {
            get {
                if (gunFsmContext == null) return false;

                return StatesRunning.Contains(gunFsmContext.currentStateType);
            }
        }

        private void Awake() {
            GunObject = BossChallengeMod.Instance.YanlaoGunProvider.GetGunCopy();

            if (GunObject == null) return;
            
            gunFsmContext = GunObject.GetComponentInChildren<GeneralFSMContext>();            

            StatePaused = gunFsmContext.transform.Find(pausedStateName)?.GetComponent<GeneralState>() ?? null;
            StateStart = gunFsmContext.transform.Find(startStateName)?.GetComponent<GeneralState>() ?? null;

            if(gunFsmContext.States.Count() >= 8) {
                StatesRunning.AddRange(new ArraySegment<GeneralState>(gunFsmContext.States, 2, 6));
            }
        }

        public void StartGun() {
            if(gunFsmContext == null || StateStart == null || IsRunning) return;

            gunFsmContext.ChangeState(StateStart);
        }

        public void StopGun() {
            if (gunFsmContext == null || StatePaused == null || !IsRunning) return;
            gunFsmContext.ChangeState(StatePaused);
        }

        protected void TuneTheGun(GameObject? gunObject) {
            if (gunObject == null) return;

            var gunComponent = gunObject?.GetComponentInChildren<A4_S4_ZGunLogic>() ?? null;

            if (gunComponent == null) return;

            gunMaxSpeedFieldRef.SetValue(gunComponent, 300f);
            gunComponent.NormalLerpingSpeed = 2f;
        }

        public int GetPriority() {
            return 1;
        }

        public void ResetComponent() {
            StopGun();
        }
    }
}
