using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class ShieldModifier : ModifierBase {

        protected GameObject? shieldObject;
        protected MonsterShield? shieldComponent;

        private bool adjustingNeeded = false;
        private bool initNeeded = true;

        public override void Awake() {
            base.Awake();
            Key = "shield";

            shieldComponent = Monster?.ShieldOnMonster ?? null;

            if(shieldComponent == null) {
                shieldObject = BossChallengeMod.Instance.ShieldProvider.GetShieldCopy();
                shieldComponent = shieldObject?.GetComponent<MonsterShield>() ?? null;
                adjustingNeeded = true;
            }
            else {
                shieldObject = shieldComponent.gameObject;
            }

            if (shieldObject != null && shieldComponent != null && Monster != null) {
                var buffPosObject = Monster?.monsterCore.buffPos ?? null;

                if (buffPosObject != null) {
                    shieldObject.transform.SetParent(buffPosObject.transform, false);
                }
                else {
                    shieldObject.transform.SetParent(Monster.transform, false);
                }
                AutoAttributeManager.AutoReference(shieldObject);

                var damageReceivers = Monster.damageReceivers;
                if (damageReceivers != null && adjustingNeeded) {
                    var biggestColliderSize = damageReceivers.Select(dr => dr.GetComponent<BoxCollider2D>()).Max(bc => Math.Max(bc.size.x, bc.size.y));
                    float scale = biggestColliderSize / 58;

                    shieldObject.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            enabled = keys.Contains(Key);
            if(!enabled) {
                shieldComponent.Break();
            }
        }

        public override void MonsterNotify() {
            base.MonsterNotify();

            if (enabled && shieldComponent != null) {
                if(initNeeded) {
                    shieldComponent.InitAndBindToMonster();
                    initNeeded = false;
                }
                else {
                    shieldComponent.Reset();
                }
            }
        }
    }
}
