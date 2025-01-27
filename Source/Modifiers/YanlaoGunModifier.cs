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

        public override void Awake() {
            base.Awake();

            Key = "ya_gun";

        }

        private void Start() {

            YanlaoGunController = GetComponentInParent<MonsterYanlaoGunController>();
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
