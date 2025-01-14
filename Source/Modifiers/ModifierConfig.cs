using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class ModifierConfig {
        public string Key { get; set; } = string.Empty;
        public List<string> Incompatibles { get; } = new List<string>();
    }
}
