using BossChallengeMod.Modifiers.Managers;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class YanlaoGunModifier : ModifierBase {

        protected MonsterYanlaoGunController? YanlaoGunController;
        public float MaxDistance { get; set; } = 800f;
        protected bool isDelaying = false;

        public override void Awake() {
            base.Awake();
        }

        private void Update() {
            if(YanlaoGunController == null) return;

            var postureSystem = Monster?.postureSystem ?? null;
            if (postureSystem != null && 
                (postureSystem.IsMonsterEmptyPosture || (!Monster?.enabled ?? true) || Player.i.lockMoving || Player.i.freeze) && 
                YanlaoGunController.IsRunning) {
                YanlaoGunController.StopGun();
            }

            if(postureSystem != null && 
                !postureSystem.IsMonsterEmptyPosture && 
                !YanlaoGunController.IsRunning &&
                (Monster?.enabled ?? false) &&
                !IsPaused &&
                !Player.i.lockMoving &&
                !Player.i.freeze &&
                !isDelaying) {
                YanlaoGunController.StartGun();
            }
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);
            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            enabled = true;

            if(!YanlaoGunController.IsRunning && !IsPaused) {
                StartCoroutine(StartWithDelay(5));
            }
        }

        protected IEnumerator StartWithDelay(float delay) {
            isDelaying = true;
            yield return new WaitForSeconds(delay);

            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            var postureSystem = Monster?.postureSystem ?? null;
            if (postureSystem != null &&
                !postureSystem.IsMonsterEmptyPosture &&
                !YanlaoGunController.IsRunning &&
                (Monster?.enabled ?? false) &&
                !IsPaused &&
                !Player.i.lockMoving &&
                !Player.i.freeze &&
                !isDelaying) {
                YanlaoGunController.StartGun();
            }
            isDelaying = false;
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation();

            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            if (YanlaoGunController?.IsRunning ?? false) {                
                YanlaoGunController?.StopGun();
            }

            enabled = false;

        }

        public override void NotifyPause() {
            base.NotifyPause();

            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            if (YanlaoGunController?.IsRunning ?? false) {
                YanlaoGunController?.StopGun();
            }
        }

        public override void NotifyResume() {
            base.NotifyResume();

            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            if (!YanlaoGunController.IsRunning && enabled) {
                YanlaoGunController?.StartGun();
            }
        }

        public override void SetController(Component controllerComponent) {
            if(controllerComponent is MonsterYanlaoGunController gunController) {
                YanlaoGunController = gunController;
            }
        }

        public override void OnDisable() {
            base.OnDisable();
            YanlaoGunController?.StopGun();

        }
    }
}
