using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.PatchResolver.Initializers {
    public class RegularEnemiesPatchesInitializer : PatchesResolverInitializerBase {

        protected MonsterPatchResolver monsterPatchResolver;

        public RegularEnemiesPatchesInitializer() {
            monsterPatchResolver = CreateResolver();
        }

        public override MonsterPatchResolver CreateResolver() {
            var resolver = monsterPatchResolver;
        }
    }
}
