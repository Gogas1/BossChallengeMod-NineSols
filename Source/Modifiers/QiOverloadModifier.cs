using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class QiOverloadModifier : ModifierBase {
        public override void Awake() {
            base.Awake();
            Key = "qi_overload";
        }

        public override void NotifyActivation(IEnumerable<string> keys, int iteration) {
            base.NotifyActivation(keys, iteration);

            enabled = keys.Contains(Key);

            if (enabled) {
                BossChallengeMod.Instance.GlobalModifiersFlags.EnableQiOverloadVotes.Add(this);
            } else {
                BossChallengeMod.Instance.GlobalModifiersFlags.EnableQiOverloadVotes.Remove(this);
            }
        }

        public void OnDestroy() {
            BossChallengeMod.Instance.GlobalModifiersFlags.EnableQiOverloadVotes.Remove(this);
        }
    }
}
