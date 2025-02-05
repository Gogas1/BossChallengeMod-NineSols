using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.PatchResolver.Initializers {
    public class PatchesResolverInitializerBase {
        public virtual MonsterPatchResolver CreateResolver() {
            return new MonsterPatchResolver();
        }
    }
}
