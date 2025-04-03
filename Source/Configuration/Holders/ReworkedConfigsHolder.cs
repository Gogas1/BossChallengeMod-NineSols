using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Configuration.Holders {
    public class ReworkedConfigsHolder {
        #region General
        public ConfigEntry<bool> IsMoBEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsNormalEnabled { get; set; } = null!;

        #endregion General

        #region Modifiers 

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
        public ConfigEntry<bool> IsCooldownShieldModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsQiOverloadModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsDistanceShieldModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsYanlaoGunModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsQiBombModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsShieldBreakBombModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsQiOverloadBombModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsQiDepletionBombModifierEnabled { get; set; } = null!;
        public ConfigEntry<bool> IsCooldownBombModifierEnabled { get; set; } = null!;

        #endregion Modifiers

        #region Bosses

        public ConfigEntry<bool> AffectBosses { get; set; } = null!;
        public ConfigEntry<int> MaxBossCycles { get; set; } = null!;

        public ConfigEntry<bool> BossesIsSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> BossesMinSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> BossesSpeedScalingStepValue { get; set; } = null!;
        public ConfigEntry<int> BossesSpeedStepsCapValue { get; set; } = null!;

        public ConfigEntry<bool> BossesIsModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> BossesMinModifiersNumber { get; set; } = null!;
        public ConfigEntry<float> BossesModifiersScalingStepValue { get; set; } = null!;
        public ConfigEntry<int> BossesModifiersStepsCapValue { get; set; } = null!;

        public ConfigEntry<bool> BossesIsRandomSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> BossesMinRandomSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> BossesMaxRandomSpeedScalingValue { get; set; } = null!;

        public ConfigEntry<bool> BossesIsRandomModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> BossesMinRandomModifiersScalingValue { get; set; } = null!;
        public ConfigEntry<int> BossesMaxRandomModifiersScalingValue { get; set; } = null!;

        #endregion Bosses

        #region Minibosses

        public ConfigEntry<bool> AffectMinibosses { get; set; } = null!;
        public ConfigEntry<int> MaxMinibossCycles { get; set; } = null!;

        public ConfigEntry<bool> MinibossesIsSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> MinibossesMinSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> MinibossesSpeedScalingStepValue { get; set; } = null!;
        public ConfigEntry<int> MinibossesSpeedStepsCapValue { get; set; } = null!;

        public ConfigEntry<bool> MinibossesIsModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> MinibossesMinModifiersNumber { get; set; } = null!;
        public ConfigEntry<float> MinibossesModifiersScalingStepValue { get; set; } = null!;
        public ConfigEntry<int> MinibossesModifiersStepsCapValue { get; set; } = null!;

        public ConfigEntry<bool> MinibossesIsRandomSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> MinibossesMinRandomSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> MinibossesMaxRandomSpeedScalingValue { get; set; } = null!;

        public ConfigEntry<bool> MinibossesIsRandomModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> MinibossesMinRandomModifiersScalingValue { get; set; } = null!;
        public ConfigEntry<int> MinibossesMaxRandomModifiersScalingValue { get; set; } = null!;

        #endregion Minibosses

        #region Enemy

        public ConfigEntry<bool> AffectEnemies { get; set; } = null!;
        public ConfigEntry<int> MaxEnemyCycles { get; set; } = null!;

        public ConfigEntry<bool> EnemiesIsSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> EnemiesMinSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> EnemiesSpeedScalingStepValue { get; set; } = null!;
        public ConfigEntry<int> EnemiesSpeedStepsCapValue { get; set; } = null!;

        public ConfigEntry<bool> EnemiesIsModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> EnemiesMinModifiersNumber { get; set; } = null!;
        public ConfigEntry<float> EnemiesModifiersScalingStepValue { get; set; } = null!;
        public ConfigEntry<int> EnemiesModifiersStepsCapValue { get; set; } = null!;

        public ConfigEntry<bool> EnemiesIsRandomSpeedScalingEnabled { get; set; } = null!;
        public ConfigEntry<float> EnemiesMinRandomSpeedScalingValue { get; set; } = null!;
        public ConfigEntry<float> EnemiesMaxRandomSpeedScalingValue { get; set; } = null!;

        public ConfigEntry<bool> EnemiesIsRandomModifiersScalingEnabled { get; set; } = null!;
        public ConfigEntry<int> EnemiesMinRandomModifiersScalingValue { get; set; } = null!;
        public ConfigEntry<int> EnemiesMaxRandomModifiersScalingValue { get; set; } = null!;

        #endregion Enemy
    }
}
