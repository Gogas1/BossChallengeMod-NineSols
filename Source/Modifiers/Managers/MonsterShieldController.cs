using BossChallengeMod.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterShieldController : MonoBehaviour, IResettableComponent {
        protected GameObject? shieldObject;
        protected MonsterShield? shieldComponent;

        protected MonsterBase Monster { get; set; } = null!;

        private bool adjustingNeeded = false;
        private bool initNeeded = true;

        public bool IsShieldEnabled {
            get => shieldComponent?.isActiveAndEnabled ?? false;
        }

        private void Awake() {
            Monster = GetComponent<MonsterBase>();

            shieldComponent = Monster?.ShieldOnMonster ?? null;

            if (shieldComponent == null) {
                shieldObject = BossChallengeMod.Instance.ShieldProvider.GetShieldCopy();
                shieldComponent = shieldObject?.GetComponent<MonsterShield>() ?? null;
                adjustingNeeded = true;
            } else {
                shieldObject = shieldComponent.gameObject;
            }

            if (shieldObject != null && shieldComponent != null && Monster != null) {
                var buffPosObject = Monster?.monsterCore.buffPos ?? null;

                if (buffPosObject != null) {
                    shieldObject.transform.SetParent(buffPosObject.transform, false);
                } else {
                    shieldObject.transform.SetParent(Monster.transform, false);
                }
                AutoAttributeManager.AutoReferenceAllChildren(shieldObject);

                var damageReceivers = Monster.damageReceivers;
                if (damageReceivers != null && adjustingNeeded) {
                    var biggestColliderSize = damageReceivers.Select(dr => dr.GetComponent<BoxCollider2D>()).Max(bc => Math.Max(bc.size.x, bc.size.y));
                    float scale = biggestColliderSize / 58;

                    shieldObject.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }

        public void Activate() {
            if (shieldComponent != null) {
                if (initNeeded) {
                    shieldComponent.InitAndBindToMonster();
                    shieldComponent.Reset();
                    initNeeded = false;
                } else {
                    shieldComponent.Reset();
                }
            }
        }

        public void Deactivate() {
            shieldComponent?.RemoveShield();
        }

        public void ResetComponent() {
            Deactivate();
        }

        public int GetPriority() {
            return 1;
        }
    }
}
