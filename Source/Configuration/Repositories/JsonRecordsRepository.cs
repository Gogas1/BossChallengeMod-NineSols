﻿using NineSolsAPI.Utils;
using BossChallengeMod.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossChallengeMod.Configuration.Repositories {
    public class JsonRecordsRepository : IRecordsRepository {
        private readonly string fileName = "BossReviveChallengeRecords.json";

        public async Task<RecordEntry?> GetRecordForKeyAsync(string key) {
            if (!File.Exists(fileName)) {
                await File.WriteAllTextAsync(fileName, "[]");
            }

            string json = await File.ReadAllTextAsync(fileName);
            List<RecordEntry>? records = JsonUtils.Deserialize<List<RecordEntry>>(json) ?? new List<RecordEntry>();

            return records.FirstOrDefault(r => r.Key == key);
        }

        public async Task SaveBossRecordForKeyAsync(string key, BossEntry bossEntry) {
            List<RecordEntry> records;

            if (!File.Exists(fileName)) {
                records = new List<RecordEntry>();
                await File.WriteAllTextAsync(fileName, "[]");
            } else {
                string json = await File.ReadAllTextAsync(fileName);
                records = JsonUtils.Deserialize<List<RecordEntry>>(json) ?? new List<RecordEntry>();
            }

            string configurationKey = key.ToLower();
            var targetRecord = records.FirstOrDefault(r => r.Key == configurationKey) ?? new RecordEntry { Key = configurationKey };

            if (!records.Contains(targetRecord)) {
                records.Add(targetRecord);
            }

            var targetBossEntry = targetRecord.BossesRecords.FirstOrDefault(br => br.Boss == bossEntry.Boss);
            if (targetBossEntry == null) {
                targetRecord.BossesRecords.Add(bossEntry);
            } else {
                targetBossEntry.BestValue = bossEntry.BestValue;
                targetBossEntry.LastValue = bossEntry.LastValue;
            }

            string updatedJson = JsonUtils.Serialize(records);
            await File.WriteAllTextAsync(fileName, updatedJson);
        }
    }
}
