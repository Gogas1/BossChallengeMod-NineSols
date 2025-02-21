using BossChallengeMod.Modifiers.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class YanlaoGunModifier : ModifierBase {

        protected MonsterYanlaoGunController? YanlaoGunController;
        public float MaxDistance { get; set; } = 800f;

        public override void Awake() {
            base.Awake();
        }

        private void Start() {

            YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
        }

        private void Update() {
            if(YanlaoGunController == null) return;

            var postureSystem = Monster?.postureSystem ?? null;
            if (postureSystem != null && 
                (postureSystem.IsMonsterEmptyPosture || Player.i.lockMoving) && 
                YanlaoGunController.IsRunning) {
                YanlaoGunController.StopGun();
            }

            if(postureSystem != null && 
                !postureSystem.IsMonsterEmptyPosture && 
                !YanlaoGunController.IsRunning && 
                !IsPaused &&
                !Player.i.lockMoving) {
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
                YanlaoGunController?.StartGun();
            }
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation();

            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            enabled = false;

            if (YanlaoGunController?.IsRunning ?? false) {                
                YanlaoGunController?.StopGun();
            }
        }

        public override void NotifyPause() {
            base.NotifyPause();

            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            if (YanlaoGunController?.isActiveAndEnabled ?? false) {
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
    }
}
