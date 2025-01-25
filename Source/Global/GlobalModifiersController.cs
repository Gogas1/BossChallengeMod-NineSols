using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Global {
    public class GlobalModifiersController {
        public HashSet<object> BlockArrowVotes { get; set; } = new HashSet<object>();
        public HashSet<object> BlockTalismanVotes { get; set; } = new HashSet<object>();
        public HashSet<object> EnableQiOverloadVotes { get; set; } = new HashSet<object>();
    }
}
