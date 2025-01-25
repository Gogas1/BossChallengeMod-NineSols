using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class RandomArrowModifier : ModifierBase {
        public override void Awake() {
            base.Awake();
            Key = "random_arrow";
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            enabled = keys.Contains(Key);
            
            if(enabled) {
                BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Add(this);
            }
            else {
                BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Remove(this);
            }
        }

        public void OnDestroy() {
            BossChallengeMod.Instance.GlobalModifiersFlags.BlockArrowVotes.Remove(this);
        }
    }
}
