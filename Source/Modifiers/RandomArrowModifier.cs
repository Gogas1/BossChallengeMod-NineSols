using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class RandomArrowModifier : ModifierBase {
        private HashSet<object> _blockArrowVotes = BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes;

        public RandomArrowModifier() {
            Key = "random_arrow";
        }

        public override void Awake() {
            base.Awake();
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;
            if(!IsPaused) {
                _blockArrowVotes.Add(this);
            }
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation();

            enabled = false;
            _blockArrowVotes.Remove(this);
        }

        public override void NotifyPause() {
            base.NotifyPause();

            if (enabled) {
                _blockArrowVotes.Remove(this);
            }
        }

        public override void NotifyResume() {
            base.NotifyResume();

            if (enabled) {
                _blockArrowVotes.Add(this);
            }
        }

        public void OnDestroy() {
            _blockArrowVotes.Remove(this);
        }
    }
}
