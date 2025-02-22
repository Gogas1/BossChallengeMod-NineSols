using BossChallengeMod.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Global {
    public class GlobalModifiersController {
        public HashSet<object> BlockArrowVotes { get; set; } = new HashSet<object>();
        public HashSet<object> BlockTalismanVotes { get; set; } = new HashSet<object>();
        public HashSet<object> EnableQiOverloadVotes { get; set; } = new HashSet<object>();

        public HashSet<IModifierSubscriber> PlayerGainFullQiSubscribers { get; set; } = new();
        public HashSet<IModifierSubscriber> PlayerDepletedQiSubscribers { get; set; } = new();

        public void NotifyPlayerGainFullQiSubscribers(PlayerEnergy playerEnergy) {
            foreach (var item in PlayerGainFullQiSubscribers) {
                item.NotifySubscriber(playerEnergy);
            }
        }

        public void NotifyPlayerDepletedQiSubscribers(PlayerEnergy playerEnergy) {
            foreach (var item in PlayerDepletedQiSubscribers) {
                item.NotifySubscriber(playerEnergy);
            }
        }
    }
}
