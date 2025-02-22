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

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;

            if (!IsPaused) {
                _subscribers.Add(this);
            }
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation(iteration);

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
            SpawnAtPlayer();
        }

        public void OnDestroy() {
            _subscribers.Remove(this);
        }
    }
}
