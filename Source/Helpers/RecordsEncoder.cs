using BossChallengeMod.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BossChallengeMod.Helpers {
    public static class RecordsEncoder {
        public static string EncodeToBase64(ChallengeConfiguration config) {
            var binaryData = EncodeChallengeConfiguration(config);
            return Convert.ToBase64String(binaryData);
        }

        public static ChallengeConfiguration DecodeFromBase64(string base64String) {
            var binaryData = Convert.FromBase64String(base64String);
            return DecodeChallengeConfiguration(binaryData);
        }

        public static byte[] EncodeChallengeConfiguration(ChallengeConfiguration config) {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms)) {
                writer.Write(config.MaxCycles);
                
                writer.Write(config.EnableSpeedScaling);
                writer.Write(config.MinSpeedScalingValue);
                writer.Write(config.MaxSpeedScalingValue);
                writer.Write(config.MaxSpeedScalingCycle);

                writer.Write(config.EnableModifiersScaling);
                writer.Write(config.MaxModifiersNumber);
                writer.Write(config.MaxModifiersScalingCycle);

                writer.Write(config.ModifiersEnabled);
                writer.Write(config.AllowRepeatModifiers);
                writer.Write(config.SpeedModifierEnabled);
                writer.Write(config.TimerModifierEnabled);
                writer.Write(config.ParryDirectDamageModifierEnabled);
                writer.Write(config.DamageBuildupModifierEnabled);
                writer.Write(config.RegenerationModifierEnabled);
                writer.Write(config.KnockbackModifierEnabled);
                writer.Write(config.RandomArrowModifierEnabled);
                writer.Write(config.RandomTalismanModifierEnabled);
                writer.Write(config.EnduranceModifierEnabled);

                return ms.ToArray();
            }
        }

        public static ChallengeConfiguration DecodeChallengeConfiguration(byte[] data) {
            using (var ms = new MemoryStream(data))
            using (var reader = new BinaryReader(ms)) {
                return new ChallengeConfiguration {
                    MaxCycles = reader.ReadInt32(),

                    EnableSpeedScaling = reader.ReadBoolean(),
                    MinSpeedScalingValue = reader.ReadSingle(),
                    MaxSpeedScalingValue = reader.ReadSingle(),
                    MaxSpeedScalingCycle = reader.ReadInt32(),

                    EnableModifiersScaling = reader.ReadBoolean(),
                    MaxModifiersNumber = reader.ReadInt32(),
                    MaxModifiersScalingCycle = reader.ReadInt32(),

                    ModifiersEnabled = reader.ReadBoolean(),
                    AllowRepeatModifiers = reader.ReadBoolean(),
                    SpeedModifierEnabled = reader.ReadBoolean(),
                    TimerModifierEnabled = reader.ReadBoolean(),
                    ParryDirectDamageModifierEnabled = reader.ReadBoolean(),
                    DamageBuildupModifierEnabled = reader.ReadBoolean(),
                    RegenerationModifierEnabled = reader.ReadBoolean(),
                    KnockbackModifierEnabled = reader.ReadBoolean(),
                    RandomArrowModifierEnabled = reader.ReadBoolean(),
                    RandomTalismanModifierEnabled = reader.ReadBoolean(),
                    EnduranceModifierEnabled = reader.ReadBoolean(),
                };
            }
        }
    }
}
