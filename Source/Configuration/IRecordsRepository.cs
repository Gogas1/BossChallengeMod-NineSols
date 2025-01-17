using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BossChallengeMod.Configuration {
    public interface IRecordsRepository {
        RecordEntry? GetRecordForConfiguration(ChallengeConfiguration configuration);
        Task<RecordEntry?> GetRecordForConfigurationAsync(ChallengeConfiguration configuration);
        Task<RecordEntry?> GetRecordForKeyAsync(string key);

        void SaveBossRecordForConfiguration(ChallengeConfiguration configuration, BossEntry bossEntry);
        Task SaveBossRecordForConfigurationAsync(ChallengeConfiguration configuration, BossEntry bossEntry);

        Task SaveBossRecordForKeyAsync(string key, BossEntry bossEntry);
    }
}
