using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class RandomTaliModifier : ModifierBase {
        private HashSet<object> _blockTalismanVotes = BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes;


        public override void Awake() {
            base.Awake();
        }

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;
            if(!IsPaused) {
                _blockTalismanVotes.Add(this);
            }
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation();

            enabled = false;
            _blockTalismanVotes.Remove(this);
        }

        public override void NotifyPause() {
            base.NotifyPause();

            if (enabled) {
                _blockTalismanVotes.Remove(this);
            }
        }

        public override void NotifyResume() {
            base.NotifyResume();

            if (enabled) {
                _blockTalismanVotes.Add(this);
            }
        }

        public void OnDestroy() {
            _blockTalismanVotes.Remove(this);
        }
    }
}
