using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class QiOverloadModifier : ModifierBase {
        private HashSet<object> _enableQiOverloadVotes = BossChallengeMod.Instance.GlobalModifiersFlags.EnableQiOverloadVotes;

        public override void Awake() {
            base.Awake();
            Key = "qi_overload";
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;
            _enableQiOverloadVotes.Add(this);
        }

        public override void NotifyDeactivation() {
            base.NotifyDeactivation();

            enabled = false;
            _enableQiOverloadVotes.Remove(this);
        }

        public override void NotifyPause() {
            base.NotifyPause();

            if (enabled) {
                _enableQiOverloadVotes.Remove(this);
            }
        }

        public override void NotifyResume() {
            base.NotifyResume();

            if (enabled) {
                _enableQiOverloadVotes.Add(this);
            }
        }

        public void OnDestroy() {
            _enableQiOverloadVotes.Remove(this);
        }
    }
}
