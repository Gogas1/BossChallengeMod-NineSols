using BossChallengeMod.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossChallengeMod.Configuration.Repositories {
    public class RecordsMemoryRepository : IRecordsRepository {
        private List<RecordEntry> RecordEntries = new List<RecordEntry>();

        public RecordEntry? GetRecordForConfiguration(ChallengeConfiguration configuration) {
            var configurationString = RecordsEncoder.EncodeToBase64(configuration);
            return RecordEntries.FirstOrDefault(re => re.Key == configurationString);
        }

        public async Task<RecordEntry?> GetRecordForConfigurationAsync(ChallengeConfiguration configuration) {
            return await Task.Run(() => GetRecordForConfiguration(configuration));
        }

        public Task<RecordEntry?> GetRecordForKeyAsync(string key) {
            throw new NotImplementedException();
        }

        public void SaveBossRecordForConfiguration(ChallengeConfiguration configuration, BossEntry bossEntry) {
            throw new NotImplementedException();
        }

        public Task SaveBossRecordForConfigurationAsync(ChallengeConfiguration configuration, BossEntry bossEntry) {
            throw new NotImplementedException();
        }

        public Task SaveBossRecordForKeyAsync(string key, BossEntry bossEntry) {
            throw new NotImplementedException();
        }
    }
}
