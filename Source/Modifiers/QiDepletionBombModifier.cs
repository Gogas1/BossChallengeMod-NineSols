using BossChallengeMod.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BossChallengeMod.Modifiers {
    internal class QiDepletionBombModifier : BombModifier, IModifierSubscriber {

        private HashSet<IModifierSubscriber> _subscribers = BossChallengeMod.Instance.GlobalModifiersFlags.PlayerDepletedQiSubscribers;

        public QiDepletionBombModifier() {
            BombCount = 2;
        }

        public override void NotifyActivation() {
            base.NotifyActivation();

            enabled = true;

            if (!IsPaused) {
                _subscribers.Add(this);
            }
        }

        public override void NotifyDeactivation() {
            base.NotifyDeactivation();

            enabled = false;
            _subscribers.Remove(this);
        }

        public override void NotifyPause() {
            base.NotifyPause();

            if (enabled) {
                _subscribers.Remove(this);
            }
        }

        public override void NotifyResume() {
            base.NotifyResume();

            if (enabled) {
                _subscribers.Add(this);
            }
        }

        public void NotifySubscriber(object args) {
            if(gameObject.activeInHierarchy) {
                SpawnAtPlayer();
            }
        }

        public override void NotifyDestroing() {
            _subscribers.Remove(this);
        }
    }
}
