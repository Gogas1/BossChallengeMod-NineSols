using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossChallengeMod.Configuration {
    public class ChallengeConfigurationManager {
        private readonly IRecordsRepository _recordsRepository;
        private readonly Queue<Action> taskQueue = new Queue<Action>();
        private bool isInProcess = false;

        public ChallengeConfiguration ChallengeConfiguration { get; set; } = new();

        public ChallengeConfigurationManager(IRecordsRepository recordsRepository) {
            _recordsRepository = recordsRepository;
        }

        public async Task<BossEntry> GetRecordForBoss(MonsterBase monsterBase) {
            return await GetRecordForBoss(monsterBase, ChallengeConfiguration);
        }

        public async Task<BossEntry> GetRecordForBoss(MonsterBase monsterBase, ChallengeConfiguration challengeConfiguration) {
            var targetRecordEntry = await _recordsRepository.GetRecordForConfigurationAsync(challengeConfiguration);
            if (targetRecordEntry == null) {
                return new BossEntry() {
                    Boss = monsterBase.gameObject.name,
                    BestValue = 0,
                    LastValue = 0,
                };
            }

            var targetBossEntry = targetRecordEntry.BossesRecords.FirstOrDefault(br => br.Boss == monsterBase.gameObject.name) ??
                new BossEntry() {
                    Boss = monsterBase.gameObject.name,
                    BestValue = 0,
                    LastValue = 0,
                };

            return targetBossEntry;
        }

        public IEnumerator SaveRecordForBoss(MonsterBase monsterBase, int bestValue, int lastValue) {
            yield return SaveRecordForBoss(monsterBase, ChallengeConfiguration, bestValue, lastValue);
        }

        public IEnumerator SaveRecordForBoss(MonsterBase monsterBase, ChallengeConfiguration challengeConfiguration, int bestValue, int lastValue) {
            taskQueue.Enqueue(() => {
                _recordsRepository.SaveBossRecordForConfigurationAsync(
                    challengeConfiguration,
                    new BossEntry() {
                        Boss = monsterBase.gameObject.name,
                        BestValue = bestValue,
                        LastValue = lastValue
                    });
            });

            if (!isInProcess) {
                isInProcess = true;
                yield return ProcessQueue();
            }
        }

        private IEnumerator ProcessQueue() {
            while (taskQueue.Count > 0) {
                Action action = taskQueue.Dequeue();
                Task writeTask = Task.Run(action);

                while (!writeTask.IsCompleted) {
                    yield return null;
                }

                if (writeTask.Exception != null) {
                    Log.Error($"Failed to write file: {writeTask.Exception.InnerException.Message}");
                }
            }

            isInProcess = false;
        }
    }
}
