using BossChallengeMod.BossPatches;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.PatchResolver {
    public class MonsterPatchResolver {
        private GeneralBossPatch? _defaultPatch;
        private Dictionary<string, GeneralBossPatch?> _patches = new();

        public void AddPatch(string name, GeneralBossPatch? patch) {
            _patches.TryAdd(name, patch);
        }

        public void AddDefaultPatch(GeneralBossPatch? generalBossPatch) {
            _defaultPatch = generalBossPatch;
        }

        public GeneralBossPatch? GetPatch(string name) {
            if(_patches.TryGetValue(name, out var patch)) {
                return patch;
            }

            return _defaultPatch;
        }
    }
}
