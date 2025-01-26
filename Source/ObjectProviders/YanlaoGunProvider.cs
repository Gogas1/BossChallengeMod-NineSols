using BossChallengeMod.Preloading;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.ObjectProviders {
    public class YanlaoGunProvider : IPreloadTarget {
        private GameObject? gunCopy;

        public void Set(GameObject? preloaded) {
            gunCopy = preloaded;
        }

        public GameObject? GetGunCopy() {
            var result = GameObject.Instantiate(gunCopy);
            AutoAttributeManager.AutoReferenceAllChildren(result);
            SetupGun(result);
            return result;
        }

        private void SetupGun(GameObject? gunObject) {
            try {
                if (gunObject == null) {
                    return;
                }

                var fsmContext = gunObject.GetComponentInChildren<GeneralFSMContext>();
                var stateMachineOwner = gunObject.GetComponent<StateMachineOwner>();

                if (fsmContext == null || stateMachineOwner == null) {
                    return;
                }

                fsmContext.EnterLevelAwake();
                stateMachineOwner.ResetFSM();

                
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }
    }
}
