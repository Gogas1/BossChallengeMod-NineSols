using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BossChallengeMod.Configuration {
    public interface IRecordsRepository {
        Task<RecordEntry?> GetRecordForKeyAsync(string key);
        Task SaveBossRecordForKeyAsync(string key, BossEntry bossEntry);
    }
}
