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

            Key = "ya_gun";

        }

        private void Start() {

            YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
        }

        private void Update() {
            if(enabled && Monster != null) {
                var distanceDifference = Vector2.Distance(Player.i.transform.position, Monster.transform.position);
                if ((Monster?.postureSystem.IsMonsterEmptyPosture ?? true) || distanceDifference >= MaxDistance) {
                    YanlaoGunController?.StopGun();
                }
                else {
                    YanlaoGunController?.StartGun();
                }
            }
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);
            if (YanlaoGunController == null) {
                YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
            }

            enabled = keys.Contains(Key);

            if (enabled) {
                YanlaoGunController?.StartGun();
            } else {
                YanlaoGunController?.StopGun();
            }

        }
    }
}
