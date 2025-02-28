using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class RandomArrowModifier : ModifierBase {
        private HashSet<object> _blockArrowVotes = BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes;


        public override void Awake() {
            base.Awake();
        }

        public override void NotifyActivation() {
            base.NotifyActivation();

            enabled = true;
            if(!IsPaused) {
                _blockArrowVotes.Add(this);
            }
        }

        public override void NotifyDeactivation() {
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
