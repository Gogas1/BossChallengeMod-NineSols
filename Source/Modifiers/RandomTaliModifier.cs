using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class RandomTaliModifier : ModifierBase {
        public override void Awake() {
            base.Awake();
            Key = "random_talisman";
        }

        public override void Notify(IEnumerable<string> keys, int iteration) {
            base.Notify(keys, iteration);

            enabled = keys.Contains(Key);

            if (enabled) {
                BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes.Add(this);
            } else {
                BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes.Remove(this);
            }
        }

        public void OnDestroy() {
            BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes.Remove(this);
        }
    }
}
