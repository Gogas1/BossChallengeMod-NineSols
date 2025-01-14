using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration {
    public class RecordEntry {
        public string Key { get; set; } = string.Empty;
        public List<BossEntry> BossesRecords = new();
    }

    public record BossEntry {
        public string Boss { get; set; } = string.Empty;
        public int BestValue { get; set; }
        public int LastValue { get; set; }
    }
}
