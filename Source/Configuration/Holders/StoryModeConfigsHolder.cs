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
        public ConfigEntry<bool> IsSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> MinSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> MaxSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<int> MaxSpeedScalingCycleValue { get; set; } = null!;

        public ConfigEntry<bool> IsModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> MaxModifiersNumber { get; set; } = null!;
        public ConfigEntry<int> MaxModifiersNumberScalingValue { get; set; } = null!;

        public ConfigEntry<bool> IsRandomSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> StartRandomSpeedScalingDeath { get; set; } = null!;
        public ConfigEntry<float> MinRandomSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> MaxRandomSpeedScalingValue { get; set; } = null!;

        public ConfigEntry<bool> IsRandomModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> StartRandomModifiersScalingDeath { get; set; } = null!;
        public ConfigEntry<int> MinRandomModifiersScalingValue { get; set; } = null!;
        public ConfigEntry<int> MaxRandomModifiersScalingValue { get; set; } = null!;

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
