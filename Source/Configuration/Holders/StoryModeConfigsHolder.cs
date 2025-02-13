using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration.Holders {
    public class StoryModeConfigsHolder {

        #region General
        public ConfigEntry<bool> IsModEnabled { get; set; } = null!;
        public ConfigEntry<int> MaxBossCycles { get; set; } = null!;
        public ConfigEntry<int> MaxMinibossCycles { get; set; } = null!;
        public ConfigEntry<int> MaxEnemyCycles { get; set; } = null!;

        public ConfigEntry<bool> AffectBosses { get; set; } = null!;
        public ConfigEntry<bool> AffectMiniBosses { get; set; } = null!;
        public ConfigEntry<bool> AffectEnemies { get; set; } = null!;

        public ConfigEntry<bool> IsBossCyclesNumberRandomized { get; set; } = null!;
        public ConfigEntry<int> MaxRandomBossCycles { get; set; } = null!;
        public ConfigEntry<int> MinRandomBossCycles { get; set; } = null!;

        public ConfigEntry<bool> IsMiniBossCyclesNumberRandomized { get; set; } = null!;
        public ConfigEntry<int> MinRandomMiniBossCycles { get; set; } = null!;
        public ConfigEntry<int> MaxRandomMiniBossCycles { get; set; } = null!;

        public ConfigEntry<bool> IsEnemyCyclesNumberRandomized { get; set; } = null!;
        public ConfigEntry<int> MinRandomEnemyCycles { get; set; } = null!;
        public ConfigEntry<int> MaxRandomEnemyCycles { get; set; } = null!;

        #endregion General

        #region Scaling

        #region Bosses scaling
        public ConfigEntry<bool> BossesIsSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> BossesMinSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> BossesMaxSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<int> BossesMaxSpeedScalingCycleValue { get; set; } = null!;

        public ConfigEntry<bool> BossesIsModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> BossesMaxModifiersNumber { get; set; } = null!;
        public ConfigEntry<int> BossesMaxModifiersNumberScalingValue { get; set; } = null!;

        public ConfigEntry<bool> BossesIsRandomSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> BossesStartRandomSpeedScalingDeath { get; set; } = null!;
        public ConfigEntry<float> BossesMinRandomSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> BossesMaxRandomSpeedScalingValue { get; set; } = null!;

        public ConfigEntry<bool> BossesIsRandomModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> BossesStartRandomModifiersScalingDeath { get; set; } = null!;
        public ConfigEntry<int> BossesMinRandomModifiersScalingValue { get; set; } = null!;
        public ConfigEntry<int> BossesMaxRandomModifiersScalingValue { get; set; } = null!;
        #endregion Bosses scaling

        #region Minibosses scaling
        public ConfigEntry<bool> MinibossesIsSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> MinibossesMinSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> MinibossesMaxSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<int> MinibossesMaxSpeedScalingCycleValue { get; set; } = null!;

        public ConfigEntry<bool> MinibossesIsModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> MinibossesMaxModifiersNumber { get; set; } = null!;
        public ConfigEntry<int> MinibossesMaxModifiersNumberScalingValue { get; set; } = null!;

        public ConfigEntry<bool> MinibossesIsRandomSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> MinibossesStartRandomSpeedScalingDeath { get; set; } = null!;
        public ConfigEntry<float> MinibossesMinRandomSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> MinibossesMaxRandomSpeedScalingValue { get; set; } = null!;

        public ConfigEntry<bool> MinibossesIsRandomModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> MinibossesStartRandomModifiersScalingDeath { get; set; } = null!;
        public ConfigEntry<int> MinibossesMinRandomModifiersScalingValue { get; set; } = null!;
        public ConfigEntry<int> MinibossesMaxRandomModifiersScalingValue { get; set; } = null!;
        #endregion Minibosses scaling

        #region Enemy scaling
        public ConfigEntry<bool> EnemiesIsSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> EnemiesMinSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> EnemiesMaxSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<int> EnemiesMaxSpeedScalingCycleValue { get; set; } = null!;

        public ConfigEntry<bool> EnemiesIsModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> EnemiesMaxModifiersNumber { get; set; } = null!;
        public ConfigEntry<int> EnemiesMaxModifiersNumberScalingValue { get; set; } = null!;

        public ConfigEntry<bool> EnemiesIsRandomSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> EnemiesStartRandomSpeedScalingDeath { get; set; } = null!;
        public ConfigEntry<float> EnemiesMinRandomSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> EnemiesMaxRandomSpeedScalingValue { get; set; } = null!;

        public ConfigEntry<bool> EnemiesIsRandomModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> EnemiesStartRandomModifiersScalingDeath { get; set; } = null!;
        public ConfigEntry<int> EnemiesMinRandomModifiersScalingValue { get; set; } = null!;
        public ConfigEntry<int> EnemiesMaxRandomModifiersScalingValue { get; set; } = null!;
        #endregion Enemy scaling

        #endregion Scaling

        public ConfigEntry<bool> IsModifiersEnabled { get; set; } = null!;
        public ConfigEntry<int> ModifiersStartDeathValue { get; set; } = null!;
        public ConfigEntry<bool> IsModifiersRepeatingEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsSpeedModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsTimerModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsParryDamageModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsDamageBuildupModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsRegenerationModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsKnockbackModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsRandomArrowModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsRandomTalismanModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsEnduranceModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsQiShieldModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsTimedShieldModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsQiOverloadModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsDistanceShieldModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsYanlaoGunModifierEnabled { get; set; } = null!;
    }
}
