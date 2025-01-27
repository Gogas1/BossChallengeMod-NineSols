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
                var eventBinder = gunObject.GetComponent<RCGArgEventBinder>();
                var playerSensor = gunObject.GetComponentInChildren<PlayerSensor>();
                var playerSensorEventSender = playerSensor?.gameObject.GetComponent<RCGEventSender>() ?? null;

                if (fsmContext == null || 
                    stateMachineOwner == null ||
                    eventBinder == null ||
                    playerSensor == null ||
                    playerSensor == null ||
                    playerSensorEventSender == null) {
                    Log.Error($"SetupGun something is null: {fsmContext}, {stateMachineOwner}, {eventBinder}, {playerSensor}, {playerSensorEventSender}");
                    return;
                }

                fsmContext.EnterLevelAwake();
                stateMachineOwner.ResetFSM();
                playerSensor.Awake();
                playerSensor.EnterLevelReset();
                eventBinder.EnterLevelAwakeReverse();
                playerSensor.PlayerEnterEvent.AddListener(playerSensorEventSender.Send);

                var enemyExplosion = gunObject.transform.Find("FSM Animator/LogicRoot/A4_S4_ZGunLogic/Aimer/Explosion/ExplosionForEnemy");

                if (enemyExplosion == null) {
                    Log.Error("Enemy explosion is null");
                    return;
                }

                enemyExplosion.gameObject.SetActive(false);
                
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }
    }
}
