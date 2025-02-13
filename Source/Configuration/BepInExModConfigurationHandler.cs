using BepInEx.Configuration;
using BossChallengeMod.Configuration.Holders;
using BossChallengeMod.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration {
    public class BepInExModConfigurationHandler {

        #region Challenge configs
        private ConfigEntry<bool> isCyclingEnabled = null!;
        private ConfigEntry<bool> useSingleRecordsKey = null!;
        private ConfigEntry<int> maxCycles = null!;

        private ConfigEntry<bool> isSpeedScalingEnabled = null!;
        private ConfigEntry<float> minSpeedScalingValue = null!;
        private ConfigEntry<float> maxSpeedScalingValue = null!;
        private ConfigEntry<int> maxSpeedScalingCycleValue = null!;

        private ConfigEntry<bool> isModifiersScalingEnabled = null!;
        private ConfigEntry<int> maxModifiersNumber = null!;
        private ConfigEntry<int> maxModifiersNumberScalingValue = null!;

        private ConfigEntry<bool> IsRandomSpeedScalingEnabled { get; set; } = null!;
        private ConfigEntry<int> StartRandomSpeedScalingDeath { get; set; } = null!;
        private ConfigEntry<float> MinRandomSpeedScalingValue { get; set; } = null!;
        private ConfigEntry<float> MaxRandomSpeedScalingValue { get; set; } = null!;

        private ConfigEntry<bool> IsRandomModifiersScalingEnabled { get; set; } = null!;
        private ConfigEntry<int> StartRandomModifiersScalingDeath { get; set; } = null!;
        private ConfigEntry<int> MinRandomModifiersScalingValue { get; set; } = null!;
        private ConfigEntry<int> MaxRandomModifiersScalingValue { get; set; } = null!;


        private ConfigEntry<bool> isModifiersEnabled = null!;
        private ConfigEntry<int> modifiersStartDeathValue = null!;
        private ConfigEntry<bool> isModifiersRepeatingEnabled = null!;
        private ConfigEntry<bool> isSpeedModifierEnabled = null!;
        private ConfigEntry<bool> isTimerModifierEnabled = null!;
        private ConfigEntry<bool> isParryDamageModifierEnabled = null!;
        private ConfigEntry<bool> isDamageBuildupModifierEnabled = null!;
        private ConfigEntry<bool> isRegenerationModifierEnabled = null!;
        private ConfigEntry<bool> isKnockbackModifierEnabled = null!;
        private ConfigEntry<bool> isRandomArrowModifierEnabled = null!;
        private ConfigEntry<bool> isRandomTalismanModifierEnabled = null!;
        private ConfigEntry<bool> isEnduranceModifierEnabled = null!;
        private ConfigEntry<bool> isQiShieldModifierEnabled = null!;
        private ConfigEntry<bool> isTimedShieldModifierEnabled = null!;
        private ConfigEntry<bool> isQiOverloadModifierEnabled = null!;
        private ConfigEntry<bool> isDistanceShieldModifierEnabled = null!;
        private ConfigEntry<bool> isYanlaoGunModifierEnabled = null!;

        #endregion Challenge configs

        #region UI configs
        private ConfigEntry<bool> isCounterUIEnabled = null!;
        private ConfigEntry<bool> useCustomCounterPosition = null!;
        private ConfigEntry<float> counterCustomXPosition = null!;
        private ConfigEntry<float> counterCustomYPosition = null!;
        private ConfigEntry<float> counterScale = null!;

        private ConfigEntry<bool> isTimerUIEnabled = null!;
        private ConfigEntry<bool> useCustomTimerPosition = null!;
        private ConfigEntry<float> timerCustomXPosition = null!;
        private ConfigEntry<float> timerCustomYPosition = null!;
        private ConfigEntry<float> timerScale = null!;

        private ConfigEntry<bool> isTalismanModeUIEnabled = null!;
        private ConfigEntry<bool> useCustomTalismanModePosition = null!;
        private ConfigEntry<float> talismanModeCustomXPosition = null!;
        private ConfigEntry<float> talismanModeCustomYPosition = null!;
        private ConfigEntry<float> talismanModeScale = null!;

        #endregion UI configs

        #region Story challenge configs

        private StoryModeConfigsHolder _storyConfigs = new();

        #endregion Story challenge configs

        private ConfigFile Config = null!;
        private ChallengeConfigurationManager ChallengeConfigurationManager = null!;
        private StoryChallengeConfigurationManager StoryChallengeConfigurationManager = null!;
        private UIConfiguration UIConfiguration = null!;

        public BepInExModConfigurationHandler(
            ConfigFile config,
            ChallengeConfigurationManager challengeConfigurationManager,
            UIConfiguration uIConfiguration,
            StoryChallengeConfigurationManager storyChallengeConfigurationManager) {
            Config = config;
            ChallengeConfigurationManager = challengeConfigurationManager;
            UIConfiguration = uIConfiguration;
            StoryChallengeConfigurationManager = storyChallengeConfigurationManager;
        }

        public void InitChallengeConfiguration() {
            Config.Bind("0. INFO: Challenge settings are applied at the beginning of the battle or when loading the location (so at the moment of enemy initialization). To apply the changed settings in the middle of the battle, start the battle again (death, reloading the location, re-entering from the lobby)",
                "0. Yes",
                false);

            isCyclingEnabled = Config.Bind(
            "1. General",
            "1.1 Enable Boss Revival",
            true,
            LocalizationResolver.Localize("config_cycling_enabled_description"));
            isCyclingEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableMod = isCyclingEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            useSingleRecordsKey = Config.Bind(
                "1. General",
                "1.2 Record regardless of configuration",
                false,
                LocalizationResolver.Localize("config_single_recording_enabled_description"));
            useSingleRecordsKey.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.UseSingleRecordKey = useSingleRecordsKey.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            maxCycles = Config.Bind(
                "1. General",
                "1.3 Boss deaths number",
                -1,
                LocalizationResolver.Localize("config_cycles_number_description"));
            maxCycles.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxBossCycles = maxCycles.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isSpeedScalingEnabled = Config.Bind(
                "2. Scaling",
                "2.1 Enable Speed Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_enabled_description"));
            isSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableSpeedScaling = isSpeedScalingEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            minSpeedScalingValue = Config.Bind(
                "2. Scaling",
                "2.1.1 Scaling: Initial Speed",
                1.0f,
                LocalizationResolver.Localize("config_scaling_minspeed_description"));
            minSpeedScalingValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MinSpeedScalingValue = minSpeedScalingValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            maxSpeedScalingValue = Config.Bind(
                "2. Scaling",
                "2.1.2 Scaling: Maximum Speed",
                1.35f,
                LocalizationResolver.Localize("config_scaling_maxspeed_description"));
            maxSpeedScalingValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = maxSpeedScalingValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            maxSpeedScalingCycleValue = Config.Bind(
                "2. Scaling",
                "2.1.3 Maximum Speed Scaling After Deaths",
                5,
                LocalizationResolver.Localize("config_scaling_scaling_cycle_description"));
            maxSpeedScalingCycleValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingCycle = maxSpeedScalingCycleValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isModifiersScalingEnabled = Config.Bind(
                "2. Scaling",
                "2.2 Enable Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_modifiers_enabled_description"));
            isModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableModifiersScaling = isModifiersScalingEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            maxModifiersNumber = Config.Bind(
                "2. Scaling",
                "2.2.1 Scaling: Maximum Modifiers Number",
                3,
                LocalizationResolver.Localize("config_scaling_maxmodifiers_description"));
            maxModifiersNumber.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersNumber = maxModifiersNumber.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            maxModifiersNumberScalingValue = Config.Bind(
                "2. Scaling",
                "2.2.2 Maximum Modifiers Number Scaling After Deaths",
                3,
                LocalizationResolver.Localize("config_scaling_modifiers_scaling_cycle_description"));
            maxModifiersNumberScalingValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersScalingCycle = maxModifiersNumberScalingValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            IsRandomSpeedScalingEnabled = Config.Bind(
                "2. Scaling",
                "2.3 Enable Random Speed Scaling",
                false,
                LocalizationResolver.Localize("config_rand_speed_scaling_enabled_description"));
            IsRandomSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomSpeedScaling = IsRandomSpeedScalingEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            StartRandomSpeedScalingDeath = Config.Bind(
                "2. Scaling",
                "2.3.1 Random Speed Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_speed_scaling_start_death_description"));
            StartRandomSpeedScalingDeath.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomSpeedScalingStartDeath = StartRandomSpeedScalingDeath.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            MinRandomSpeedScalingValue = Config.Bind(
                "2. Scaling",
                "2.3.2 Random Scaling: Minimal Speed",
                1.0f,
                LocalizationResolver.Localize("config_rand_speed_scaling_minspeed_description"));
            MinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomSpeedScalingValue = MinRandomSpeedScalingValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            MaxRandomSpeedScalingValue = Config.Bind(
                "2. Scaling",
                "2.3.3 Random Scaling: Maximum Speed",
                1.5f,
                LocalizationResolver.Localize("config_rand_speed_scaling_maxspeed_description"));
            MaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomSpeedScalingValue = MaxRandomSpeedScalingValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random modifiers scaling
            IsRandomModifiersScalingEnabled = Config.Bind(
                "2. Scaling",
                "2.4 Enable Random Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_enabled_description"));
            IsRandomModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomModifiersScaling = IsRandomModifiersScalingEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            StartRandomModifiersScalingDeath = Config.Bind(
                "2. Scaling",
                "2.4.1 Random Modifiers Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_start_death_description"));
            StartRandomModifiersScalingDeath.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomModifiersScalingStartDeath = StartRandomModifiersScalingDeath.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            MinRandomModifiersScalingValue = Config.Bind(
                "2. Scaling",
                "2.4.2 Random Scaling: Min Modifiers Number",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_min_description"));
            MinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomModifiersNumber = MinRandomModifiersScalingValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            MaxRandomModifiersScalingValue = Config.Bind(
                "2. Scaling",
                "2.4.3 Random Scaling: Max Modifiers Number",
                4,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_max_description"));
            MaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomModifiersNumber = MaxRandomModifiersScalingValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isModifiersEnabled = Config.Bind(
                "3. Modifiers",
                "3.1 Enable Modifiers",
                false,
                LocalizationResolver.Localize("config_modifiers_enabled_description"));
            isModifiersEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.ModifiersEnabled = isModifiersEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            modifiersStartDeathValue = Config.Bind(
                "3. Modifiers",
                "3.2 Modifiers Start Death",
                1,
                LocalizationResolver.Localize("config_modifiers_start_death_description"));
            modifiersStartDeathValue.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.ModifiersStartFromDeath = modifiersStartDeathValue.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isModifiersRepeatingEnabled = Config.Bind(
                "3. Modifiers",
                "3.3 Enable Modifiers Repeating",
                false,
                LocalizationResolver.Localize("config_repeating_enabled_description"));
            isModifiersRepeatingEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.ModifiersEnabled = isModifiersRepeatingEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isSpeedModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Speed Modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_speed_enabled_description"));
            isSpeedModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.SpeedModifierEnabled = isSpeedModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isTimerModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Timer Modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_timer_enabled_description"));
            isTimerModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.TimerModifierEnabled = isTimerModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isParryDamageModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Precice parry only modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_parry_damage_enabled_description"));
            isParryDamageModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.ParryDirectDamageModifierEnabled = isParryDamageModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isDamageBuildupModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Internal damage buildup modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_internal_damage_enabled_description"));
            isDamageBuildupModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.DamageBuildupModifierEnabled = isDamageBuildupModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isRegenerationModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Regeneration modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_regeneration_enabled_description"));
            isRegenerationModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.RegenerationModifierEnabled = isRegenerationModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isKnockbackModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Knockback modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_knockback_enabled_description"));
            isKnockbackModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.KnockbackModifierEnabled = isKnockbackModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isRandomArrowModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Random arrow modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_random_arrow_enabled_description"));
            isRandomArrowModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomArrowModifierEnabled = isRandomArrowModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isRandomTalismanModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Random talisman modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_random_talisman_enabled_description"));
            isRandomTalismanModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomTalismanModifierEnabled = isRandomTalismanModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isEnduranceModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Endurance modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_endurance_enabled_description"));
            isEnduranceModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.EnduranceModifierEnabled = isEnduranceModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isQiShieldModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Shield: Qi Shield modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_qi_shield_enabled_description"));
            isQiShieldModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.QiShieldModifierEnabled = isQiShieldModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isTimedShieldModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Shield: Cooldown Shield modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_cooldown_shield_enabled_description"));
            isTimedShieldModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.TimedShieldModifierEnabled = isTimedShieldModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isQiOverloadModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Qi Overload modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_qi_overload_enabled_description"));
            isQiOverloadModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.QiOverloadModifierEnabled = isQiOverloadModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isDistanceShieldModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Shield: Distance Shield modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_distance_shield_enabled_description"));
            isDistanceShieldModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.DistanceShieldModifierEnabled = isDistanceShieldModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            isYanlaoGunModifierEnabled = Config.Bind(
                "3. Modifiers",
                "3.M Yanlaos Assistance modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_yanlao_gun_enabled_description"));
            isYanlaoGunModifierEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.YanlaoGunModifierEnabled = isYanlaoGunModifierEnabled.Value;
                ChallengeConfigurationManager.ChallengeConfiguration = config;
            };
        }

        public void HandleConfigurationValues() {
            var config = ChallengeConfigurationManager.ChallengeConfiguration;
            config.EnableMod = isCyclingEnabled.Value;
            config.UseSingleRecordKey = useSingleRecordsKey.Value;
            config.MaxBossCycles = maxCycles.Value;

            config.AffectBosses = true;
            config.AffectMiniBosses = true;
            config.AffectRegularEnemies = true;

            #region Scaling
            config.BossesEnableSpeedScaling = config.MinibossesEnableSpeedScaling = config.EnemiesEnableSpeedScaling = isSpeedScalingEnabled.Value;
            config.BossesMinSpeedScalingValue = config.MinibossesMinSpeedScalingValue = config.EnemiesMinSpeedScalingValue = minSpeedScalingValue.Value;
            config.BossesMaxSpeedScalingValue = config.MinibossesMaxSpeedScalingValue = config.BossesMaxSpeedScalingValue = maxSpeedScalingValue.Value;
            config.BossesMaxSpeedScalingCycle = config.MinibossesMaxSpeedScalingCycle = config.EnemiesMaxSpeedScalingCycle = maxSpeedScalingCycleValue.Value;

            config.BossesEnableModifiersScaling = config.MinibossesEnableModifiersScaling = config.EnemiesEnableModifiersScaling = isModifiersScalingEnabled.Value;
            config.BossesMaxModifiersNumber = config.MinibossesMaxModifiersNumber = config.EnemiesMaxModifiersNumber = maxModifiersNumber.Value;
            config.BossesMaxModifiersScalingCycle = config.MinibossesMaxModifiersScalingCycle = config.EnemiesMaxModifiersScalingCycle = maxModifiersNumberScalingValue.Value;

            config.BossesEnableRandomSpeedScaling = config.MinibossesEnableRandomSpeedScaling = config.EnemiesEnableRandomSpeedScaling = IsRandomSpeedScalingEnabled.Value;
            config.BossesRandomSpeedScalingStartDeath = config.MinibossesRandomSpeedScalingStartDeath = config.EnemiesRandomSpeedScalingStartDeath = StartRandomSpeedScalingDeath.Value;
            config.BossesMinRandomSpeedScalingValue = config.MinibossesMinRandomSpeedScalingValue = config.EnemiesMinRandomSpeedScalingValue = MinRandomSpeedScalingValue.Value;

            config.BossesMaxRandomSpeedScalingValue = config.MinibossesMaxRandomSpeedScalingValue = config.EnemiesMaxRandomSpeedScalingValue = MaxRandomSpeedScalingValue.Value;
            config.BossesEnableRandomModifiersScaling = config.MinibossesEnableRandomModifiersScaling = config.EnemiesEnableRandomModifiersScaling = IsRandomModifiersScalingEnabled.Value;
            config.BossesRandomModifiersScalingStartDeath = config.MinibossesRandomModifiersScalingStartDeath = config.EnemiesRandomModifiersScalingStartDeath = StartRandomModifiersScalingDeath.Value;
            config.BossesMinRandomModifiersNumber = config.MinibossesMinRandomModifiersNumber = config.EnemiesMinRandomModifiersNumber = MinRandomModifiersScalingValue.Value;
            config.BossesMaxRandomModifiersNumber = config.MinibossesMaxRandomModifiersNumber = config.EnemiesMaxRandomModifiersNumber = MaxRandomModifiersScalingValue.Value;
            #endregion Scaling

            config.ModifiersEnabled = isModifiersEnabled.Value;
            config.AllowRepeatModifiers = isModifiersRepeatingEnabled.Value;
            config.SpeedModifierEnabled = isSpeedModifierEnabled.Value;
            config.TimerModifierEnabled = isTimerModifierEnabled.Value;
            config.ParryDirectDamageModifierEnabled = isParryDamageModifierEnabled.Value;
            config.DamageBuildupModifierEnabled = isDamageBuildupModifierEnabled.Value;
            config.RegenerationModifierEnabled = isRegenerationModifierEnabled.Value;
            config.KnockbackModifierEnabled = isKnockbackModifierEnabled.Value;
            //config.KnockoutModifierEnabled = isKnockoutModifierEnabled.Value;
            config.RandomArrowModifierEnabled = isRandomArrowModifierEnabled.Value;
            config.RandomTalismanModifierEnabled = isRandomTalismanModifierEnabled.Value;
            config.EnduranceModifierEnabled = isEnduranceModifierEnabled.Value;
            config.QiShieldModifierEnabled = isQiShieldModifierEnabled.Value;
            config.TimedShieldModifierEnabled = isTimedShieldModifierEnabled.Value;
            config.QiOverloadModifierEnabled = isQiOverloadModifierEnabled.Value;
            config.DistanceShieldModifierEnabled = isDistanceShieldModifierEnabled.Value;
            config.YanlaoGunModifierEnabled = isYanlaoGunModifierEnabled.Value;
            config.ModifiersStartFromDeath = modifiersStartDeathValue.Value;

            ChallengeConfigurationManager.ChallengeConfiguration = config;
        }

        public void InitializeUIConfiguration() {
            isCounterUIEnabled = Config.Bind(
                "4. UI",
                "4.1.1 Right panel(killer counter and modifiers list) UI enabled",
                true,
                LocalizationResolver.Localize("config_ui_counter_enabled_description"));
            isCounterUIEnabled.SettingChanged += (_, _) => { UIConfiguration.CounterUIEnabled = isCounterUIEnabled.Value; };

            useCustomCounterPosition = Config.Bind(
                "4. UI",
                "4.1.2 Use custom right panel position",
                false,
                LocalizationResolver.Localize("config_ui_counter_custom_description"));
            useCustomCounterPosition.SettingChanged += (_, _) => { UIConfiguration.UseCustomCounterPosition = useCustomCounterPosition.Value; };

            counterCustomXPosition = Config.Bind(
                "4. UI",
                "4.1.3 Custom right panel X position",
                0f,
                LocalizationResolver.Localize("config_ui_counter_custom_x_description"));
            counterCustomXPosition.SettingChanged += (_, _) => { UIConfiguration.CounterXPos = counterCustomXPosition.Value; };

            counterCustomYPosition = Config.Bind(
                "4. UI",
                "4.1.4 Custom right panel Y position",
                0f,
                LocalizationResolver.Localize("config_ui_counter_custom_y_description"));
            counterCustomYPosition.SettingChanged += (_, _) => { UIConfiguration.CounterYPos = counterCustomYPosition.Value; };

            counterScale = Config.Bind(
                "4. UI",
                "4.1.5 Right panel UI scale",
                1f,
                LocalizationResolver.Localize("config_ui_counter_scale_description"));
            counterScale.SettingChanged += (_, _) => { UIConfiguration.CounterUIScale = counterScale.Value; };


            isTimerUIEnabled = Config.Bind(
                "4. UI",
                "4.2.1 Timer UI enabled",
                true,
                LocalizationResolver.Localize("config_ui_timer_enabled_description"));
            isTimerUIEnabled.SettingChanged += (_, _) => { UIConfiguration.TimerUIEnabled = isTimerUIEnabled.Value; };

            useCustomTimerPosition = Config.Bind(
                "4. UI",
                "4.2.2 Use custom timer position",
                false,
                LocalizationResolver.Localize("config_ui_timer_custom_description"));
            useCustomTimerPosition.SettingChanged += (_, _) => { UIConfiguration.UseCustomTimerPosition = useCustomTimerPosition.Value; };

            timerCustomXPosition = Config.Bind(
                "4. UI",
                "4.2.3 Custom timer X position",
                0f,
                LocalizationResolver.Localize("config_ui_timer_custom_x_description"));
            timerCustomXPosition.SettingChanged += (_, _) => { UIConfiguration.TimerXPos = timerCustomXPosition.Value; };

            timerCustomYPosition = Config.Bind(
                "4. UI",
                "4.2.4 Custom timer Y position",
                0f,
                LocalizationResolver.Localize("config_ui_timer_custom_y_description"));
            timerCustomYPosition.SettingChanged += (_, _) => { UIConfiguration.TimerYPos = timerCustomYPosition.Value; };

            timerScale = Config.Bind(
                "4. UI",
                "4.2.5 Timer UI scale",
                1f,
                LocalizationResolver.Localize("config_ui_timer_scale_description"));
            timerScale.SettingChanged += (_, _) => { UIConfiguration.TimerUIScale = timerScale.Value; };


            isTalismanModeUIEnabled = Config.Bind(
                "4. UI",
                "4.3.1 Talisman Mode UI enabled",
                true,
                LocalizationResolver.Localize("config_ui_talisman_mode_enabled_description"));
            isTalismanModeUIEnabled.SettingChanged += (_, _) => { UIConfiguration.TalismanModeUIEnabled = isTalismanModeUIEnabled.Value; };

            useCustomTalismanModePosition = Config.Bind(
                "4. UI",
                "4.3.2 Use custom talisman mode position",
                false,
                LocalizationResolver.Localize("config_ui_talisman_mode_custom_description"));
            useCustomTalismanModePosition.SettingChanged += (_, _) => { UIConfiguration.UseCustomTalismanModePosition = useCustomTalismanModePosition.Value; };

            talismanModeCustomXPosition = Config.Bind(
                "4. UI",
                "4.3.3 Custom talisman mode X position",
                0f,
                LocalizationResolver.Localize("config_ui_talisman_mode_custom_x_description"));
            talismanModeCustomXPosition.SettingChanged += (_, _) => { UIConfiguration.TalismanModeXPos = talismanModeCustomXPosition.Value; };

            talismanModeCustomYPosition = Config.Bind(
                "4. UI",
                "4.3.4 Custom talisman mode Y position",
                0f,
                LocalizationResolver.Localize("config_ui_talisman_mode_custom_y_description"));
            talismanModeCustomYPosition.SettingChanged += (_, _) => { UIConfiguration.TalismanModeYPos = talismanModeCustomYPosition.Value; };

            talismanModeScale = Config.Bind(
                "4. UI",
                "4.3.5 Talisman mode UI scale",
                1f,
                LocalizationResolver.Localize("config_ui_talisman_mode_scale_description"));
            talismanModeScale.SettingChanged += (_, _) => { UIConfiguration.TalismanUIScale = talismanModeScale.Value; };
        }

        public void HandleUIConfigurationValues() {
            UIConfiguration.CounterUIEnabled = isCounterUIEnabled.Value;
            UIConfiguration.UseCustomCounterPosition = useCustomCounterPosition.Value;
            UIConfiguration.CounterXPos = counterCustomXPosition.Value;
            UIConfiguration.CounterYPos = counterCustomYPosition.Value;
            UIConfiguration.CounterUIScale = counterScale.Value;

            UIConfiguration.TimerUIEnabled = isTimerUIEnabled.Value;
            UIConfiguration.UseCustomTimerPosition = useCustomTimerPosition.Value;
            UIConfiguration.TimerXPos = timerCustomXPosition.Value;
            UIConfiguration.TimerYPos = timerCustomYPosition.Value;
            UIConfiguration.TimerUIScale = timerScale.Value;

            UIConfiguration.TalismanModeUIEnabled = isTalismanModeUIEnabled.Value;
            UIConfiguration.UseCustomTalismanModePosition = useCustomTalismanModePosition.Value;
            UIConfiguration.TalismanModeXPos = talismanModeCustomXPosition.Value;
            UIConfiguration.TalismanModeYPos = talismanModeCustomYPosition.Value;
            UIConfiguration.TalismanUIScale = talismanModeScale.Value;
        }

        public void InitStoryChallengeConfiguration() {
            #region General
            //Mod enabled
            _storyConfigs.IsModEnabled = Config.Bind(
                "5. Story Challenge General",
                "5.1 Enable Mod",
                false,
                LocalizationResolver.Localize("config_story_enabled_description"));
            _storyConfigs.IsModEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableMod = _storyConfigs.IsModEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Max Cycles
            _storyConfigs.MaxBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.2.1 Boss deaths number",
                2,
                LocalizationResolver.Localize("config_cycles_number_description"));
            _storyConfigs.MaxBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxBossCycles = _storyConfigs.MaxBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxMinibossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.2.2 Miniboss deaths number",
                2,
                LocalizationResolver.Localize("config_story_miniboss_cycles_number_description"));
            _storyConfigs.MaxMinibossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxMinibossCycles = _storyConfigs.MaxMinibossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxEnemyCycles = Config.Bind(
                "5. Story Challenge General",
                "5.2.3 Regular enemy deaths number",
                2,
                LocalizationResolver.Localize("config_story_enemy_cycles_number_description"));
            _storyConfigs.MaxEnemyCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxEnemyCycles = _storyConfigs.MaxEnemyCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Affect monsters
            _storyConfigs.AffectBosses = Config.Bind(
                "5. Story Challenge General",
                "5.3.1 Affect bosses",
                true,
                LocalizationResolver.Localize("config_story_affect_bosses_description"));
            _storyConfigs.AffectBosses.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.AffectBosses = _storyConfigs.AffectBosses.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.AffectMiniBosses = Config.Bind(
                "5. Story Challenge General",
                "5.3.2 Affect minibosses",
                true,
                LocalizationResolver.Localize("config_story_affect_minibosses_description"));
            _storyConfigs.AffectMiniBosses.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.AffectMiniBosses = _storyConfigs.AffectMiniBosses.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.AffectEnemies = Config.Bind(
                "5. Story Challenge General",
                "5.3.3 Affect regular enemies",
                true,
                LocalizationResolver.Localize("config_story_affect_enemies_description"));
            _storyConfigs.AffectEnemies.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.AffectRegularEnemies = _storyConfigs.AffectEnemies.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Randomize boss cycles
            _storyConfigs.IsBossCyclesNumberRandomized = Config.Bind(
                "5. Story Challenge General",
                "5.4 Randomize boss death number",
                false,
                LocalizationResolver.Localize("config_boss_deaths_randomized_enabled_description"));
            _storyConfigs.IsBossCyclesNumberRandomized.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomizeBossCyclesNumber = _storyConfigs.IsBossCyclesNumberRandomized.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.4.1 Min boss deaths random number",
                1,
                LocalizationResolver.Localize("config_boss_deaths_randomized_min_description"));
            _storyConfigs.MinRandomBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomBossCycles = _storyConfigs.MinRandomBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.4.2 Max boss deaths random number",
                3,
                LocalizationResolver.Localize("config_boss_deaths_randomized_max_description"));
            _storyConfigs.MaxRandomBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomBossCycles = _storyConfigs.MaxRandomBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Randomize miniboss cycles
            _storyConfigs.IsMiniBossCyclesNumberRandomized = Config.Bind(
                "5. Story Challenge General",
                "5.5 Randomize miniboss death number",
                false,
                LocalizationResolver.Localize("config_story_miniboss_deaths_randomized_description"));
            _storyConfigs.IsMiniBossCyclesNumberRandomized.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomizeMiniBossCyclesNumber = _storyConfigs.IsMiniBossCyclesNumberRandomized.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomMiniBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.5.1 Min miniboss deaths random number",
                1,
                LocalizationResolver.Localize("config_story_miniboss_deaths_randomized_min_description"));
            _storyConfigs.MinRandomMiniBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomMiniBossCycles = _storyConfigs.MinRandomMiniBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomMiniBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.5.2 Max miniboss deaths random number",
                3,
                LocalizationResolver.Localize("config_story_miniboss_deaths_randomized_max_description"));
            _storyConfigs.MaxRandomMiniBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomMiniBossCycles = _storyConfigs.MaxRandomMiniBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Randomize enemy cycles
            _storyConfigs.IsEnemyCyclesNumberRandomized = Config.Bind(
                "5. Story Challenge General",
                "5.6 Randomize regular enemy death number",
                false,
                LocalizationResolver.Localize("config_story_enemy_deaths_randomized_description"));
            _storyConfigs.IsEnemyCyclesNumberRandomized.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomizeEnemyCyclesNumber = _storyConfigs.IsEnemyCyclesNumberRandomized.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomEnemyCycles = Config.Bind(
                "5. Story Challenge General",
                "5.6.1 Min regular enemy deaths random number",
                1,
                LocalizationResolver.Localize("config_story_enemy_deaths_randomized_min_description"));
            _storyConfigs.MinRandomEnemyCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomEnemyCycles = _storyConfigs.MinRandomEnemyCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomEnemyCycles = Config.Bind(
                "5. Story Challenge General",
                "5.6.2 Max regular enemy deaths random number",
                3,
                LocalizationResolver.Localize("config_story_enemy_deaths_randomized_max_description"));
            _storyConfigs.MaxRandomEnemyCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomEnemyCycles = _storyConfigs.MaxRandomEnemyCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            #endregion General

            #region Scaling

            #region Bosses scaling
            //Speed scaling
            _storyConfigs.BossesIsSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.1 Enable Bosses Speed Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_enabled_description"));
            _storyConfigs.BossesIsSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableSpeedScaling = _storyConfigs.BossesIsSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMinSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.1.1 Bosses Scaling: Initial Speed",
                1.0f,
                LocalizationResolver.Localize("config_scaling_minspeed_description"));
            _storyConfigs.BossesMinSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinSpeedScalingValue = _storyConfigs.BossesMinSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMaxSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.1.2 Bosses Scaling: Maximum Speed",
                1.35f,
                LocalizationResolver.Localize("config_scaling_maxspeed_description"));
            _storyConfigs.BossesMaxSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.BossesMaxSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMaxSpeedScalingCycleValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.1.3 Bosses Maximum Speed Scaling After Deaths",
                5,
                LocalizationResolver.Localize("config_scaling_scaling_cycle_description"));
            _storyConfigs.BossesMaxSpeedScalingCycleValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingCycle = _storyConfigs.BossesMaxSpeedScalingCycleValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Modifiers scaling
            _storyConfigs.BossesIsModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.2 Enable Bosses Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_modifiers_enabled_description"));
            _storyConfigs.BossesIsModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableModifiersScaling = _storyConfigs.BossesIsModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMaxModifiersNumber = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.2.1 Bosses Scaling: Maximum Modifiers Number",
                3,
                LocalizationResolver.Localize("config_scaling_maxmodifiers_description"));
            _storyConfigs.BossesMaxModifiersNumber.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersNumber = _storyConfigs.BossesMaxModifiersNumber.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMaxModifiersNumberScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.2.2 Bosses Maximum Modifiers Number Scaling After Deaths",
                3,
                LocalizationResolver.Localize("config_scaling_modifiers_scaling_cycle_description"));
            _storyConfigs.BossesMaxModifiersNumberScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersScalingCycle = _storyConfigs.BossesMaxModifiersNumberScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random speed scaling
            _storyConfigs.BossesIsRandomSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.3 Enable Bosses Random Speed Scaling",
                false,
                LocalizationResolver.Localize("config_rand_speed_scaling_enabled_description"));
            _storyConfigs.BossesIsRandomSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomSpeedScaling = _storyConfigs.BossesIsRandomSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesStartRandomSpeedScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.3.1 Bosses Random Speed Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_speed_scaling_start_death_description"));
            _storyConfigs.BossesStartRandomSpeedScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomSpeedScalingStartDeath = _storyConfigs.BossesStartRandomSpeedScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMinRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.3.2 Bosses Random Scaling: Minimal Speed",
                1.0f,
                LocalizationResolver.Localize("config_rand_speed_scaling_minspeed_description"));
            _storyConfigs.BossesMinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomSpeedScalingValue = _storyConfigs.BossesMinRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMaxRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.3.3 Bosses Random Scaling: Maximum Speed",
                1.5f,
                LocalizationResolver.Localize("config_rand_speed_scaling_maxspeed_description"));
            _storyConfigs.BossesMaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.BossesMaxRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random modifiers scaling
            _storyConfigs.BossesIsRandomModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.4 Enable Bosses Random Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_enabled_description"));
            _storyConfigs.BossesIsRandomModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomModifiersScaling = _storyConfigs.BossesIsRandomModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesStartRandomModifiersScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.4.1 Bosses Random Modifiers Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_start_death_description"));
            _storyConfigs.BossesStartRandomModifiersScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomModifiersScalingStartDeath = _storyConfigs.BossesStartRandomModifiersScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMinRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.4.2 Bosses Random Scaling: Min Modifiers Number",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_min_description"));
            _storyConfigs.BossesMinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomModifiersNumber = _storyConfigs.BossesMinRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.BossesMaxRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.4.3 Bosses Random Scaling: Max Modifiers Number",
                4,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_max_description"));
            _storyConfigs.BossesMaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomModifiersNumber = _storyConfigs.BossesMaxRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            #endregion Bosses scaling

            #region Minibosses scaling
            //Speed scaling
            _storyConfigs.MinibossesIsSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.1 Enable Minibosses Speed Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_enabled_description"));
            _storyConfigs.MinibossesIsSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableSpeedScaling = _storyConfigs.MinibossesIsSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMinSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.1.1 Minibosses Scaling: Initial Speed",
                1.0f,
                LocalizationResolver.Localize("config_scaling_minspeed_description"));
            _storyConfigs.MinibossesMinSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinSpeedScalingValue = _storyConfigs.MinibossesMinSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMaxSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.1.2 Minibosses Scaling: Maximum Speed",
                1.35f,
                LocalizationResolver.Localize("config_scaling_maxspeed_description"));
            _storyConfigs.MinibossesMaxSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.MinibossesMaxSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMaxSpeedScalingCycleValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.1.3 Minibosses Maximum Speed Scaling After Deaths",
                5,
                LocalizationResolver.Localize("config_scaling_scaling_cycle_description"));
            _storyConfigs.MinibossesMaxSpeedScalingCycleValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingCycle = _storyConfigs.MinibossesMaxSpeedScalingCycleValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Modifiers scaling
            _storyConfigs.MinibossesIsModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.2 Enable Minibosses Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_modifiers_enabled_description"));
            _storyConfigs.MinibossesIsModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableModifiersScaling = _storyConfigs.MinibossesIsModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMaxModifiersNumber = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.2.1 Minibosses Scaling: Maximum Modifiers Number",
                3,
                LocalizationResolver.Localize("config_scaling_maxmodifiers_description"));
            _storyConfigs.MinibossesMaxModifiersNumber.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersNumber = _storyConfigs.MinibossesMaxModifiersNumber.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMaxModifiersNumberScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.2.2 Minibosses Maximum Modifiers Number Scaling After Deaths",
                3,
                LocalizationResolver.Localize("config_scaling_modifiers_scaling_cycle_description"));
            _storyConfigs.MinibossesMaxModifiersNumberScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersScalingCycle = _storyConfigs.MinibossesMaxModifiersNumberScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random speed scaling
            _storyConfigs.MinibossesIsRandomSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.2.3 Enable Minibosses Random Speed Scaling",
                false,
                LocalizationResolver.Localize("config_rand_speed_scaling_enabled_description"));
            _storyConfigs.MinibossesIsRandomSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomSpeedScaling = _storyConfigs.MinibossesIsRandomSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesStartRandomSpeedScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.3.1 Minibosses Random Speed Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_speed_scaling_start_death_description"));
            _storyConfigs.MinibossesStartRandomSpeedScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomSpeedScalingStartDeath = _storyConfigs.MinibossesStartRandomSpeedScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMinRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.3.2 Minibosses Random Scaling: Minimal Speed",
                1.0f,
                LocalizationResolver.Localize("config_rand_speed_scaling_minspeed_description"));
            _storyConfigs.MinibossesMinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomSpeedScalingValue = _storyConfigs.MinibossesMinRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMaxRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.3.3 Minibosses Random Scaling: Maximum Speed",
                1.5f,
                LocalizationResolver.Localize("config_rand_speed_scaling_maxspeed_description"));
            _storyConfigs.MinibossesMaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.MinibossesMaxRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random modifiers scaling
            _storyConfigs.MinibossesIsRandomModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.4 Enable Minibosses Random Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_enabled_description"));
            _storyConfigs.MinibossesIsRandomModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomModifiersScaling = _storyConfigs.MinibossesIsRandomModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesStartRandomModifiersScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.4.1 Minibosses Random Modifiers Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_start_death_description"));
            _storyConfigs.MinibossesStartRandomModifiersScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomModifiersScalingStartDeath = _storyConfigs.MinibossesStartRandomModifiersScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMinRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.4.2 Minibosses Random Scaling: Min Modifiers Number",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_min_description"));
            _storyConfigs.MinibossesMinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomModifiersNumber = _storyConfigs.MinibossesMinRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinibossesMaxRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.4.3 Minibosses Random Scaling: Max Modifiers Number",
                4,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_max_description"));
            _storyConfigs.MinibossesMaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomModifiersNumber = _storyConfigs.MinibossesMaxRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            #endregion Minibosses scaling

            #region Enemies scaling
            //Speed scaling
            _storyConfigs.EnemiesIsSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.1 Enable Enemies Speed Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_enabled_description"));
            _storyConfigs.EnemiesIsSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableSpeedScaling = _storyConfigs.EnemiesIsSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMinSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.1.1 Enemies Scaling: Initial Speed",
                1.0f,
                LocalizationResolver.Localize("config_scaling_minspeed_description"));
            _storyConfigs.EnemiesMinSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinSpeedScalingValue = _storyConfigs.EnemiesMinSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMaxSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.1.2 Enemies Scaling: Maximum Speed",
                1.35f,
                LocalizationResolver.Localize("config_scaling_maxspeed_description"));
            _storyConfigs.EnemiesMaxSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.EnemiesMaxSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMaxSpeedScalingCycleValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.1.3 Enemies Maximum Speed Scaling After Deaths",
                5,
                LocalizationResolver.Localize("config_scaling_scaling_cycle_description"));
            _storyConfigs.EnemiesMaxSpeedScalingCycleValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingCycle = _storyConfigs.EnemiesMaxSpeedScalingCycleValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Modifiers scaling
            _storyConfigs.EnemiesIsModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.2 Enable Enemies Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_modifiers_enabled_description"));
            _storyConfigs.EnemiesIsModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableModifiersScaling = _storyConfigs.EnemiesIsModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMaxModifiersNumber = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.2.1 Enemies Scaling: Maximum Modifiers Number",
                3,
                LocalizationResolver.Localize("config_scaling_maxmodifiers_description"));
            _storyConfigs.EnemiesMaxModifiersNumber.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersNumber = _storyConfigs.EnemiesMaxModifiersNumber.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMaxModifiersNumberScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.2.2 Enemies Maximum Modifiers Number Scaling After Deaths",
                3,
                LocalizationResolver.Localize("config_scaling_modifiers_scaling_cycle_description"));
            _storyConfigs.EnemiesMaxModifiersNumberScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersScalingCycle = _storyConfigs.EnemiesMaxModifiersNumberScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random speed scaling
            _storyConfigs.EnemiesIsRandomSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.3 Enable Enemies Random Speed Scaling",
                false,
                LocalizationResolver.Localize("config_rand_speed_scaling_enabled_description"));
            _storyConfigs.EnemiesIsRandomSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomSpeedScaling = _storyConfigs.EnemiesIsRandomSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesStartRandomSpeedScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.3.1 Enemies Random Speed Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_speed_scaling_start_death_description"));
            _storyConfigs.EnemiesStartRandomSpeedScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomSpeedScalingStartDeath = _storyConfigs.EnemiesStartRandomSpeedScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMinRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.3.2 Enemies Random Scaling: Minimal Speed",
                1.0f,
                LocalizationResolver.Localize("config_rand_speed_scaling_minspeed_description"));
            _storyConfigs.EnemiesMinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomSpeedScalingValue = _storyConfigs.EnemiesMinRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMaxRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.3.3 Enemies Random Scaling: Maximum Speed",
                1.5f,
                LocalizationResolver.Localize("config_rand_speed_scaling_maxspeed_description"));
            _storyConfigs.EnemiesMaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.EnemiesMaxRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random modifiers scaling
            _storyConfigs.EnemiesIsRandomModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.4 Enable Enemies Random Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_enabled_description"));
            _storyConfigs.EnemiesIsRandomModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomModifiersScaling = _storyConfigs.EnemiesIsRandomModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesStartRandomModifiersScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.4.1 Enemies Random Modifiers Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_start_death_description"));
            _storyConfigs.EnemiesStartRandomModifiersScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomModifiersScalingStartDeath = _storyConfigs.EnemiesStartRandomModifiersScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMinRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.4.2 Enemies Random Scaling: Min Modifiers Number",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_min_description"));
            _storyConfigs.EnemiesMinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomModifiersNumber = _storyConfigs.EnemiesMinRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.EnemiesMaxRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.4.3 Enemies Random Scaling: Max Modifiers Number",
                4,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_max_description"));
            _storyConfigs.EnemiesMaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomModifiersNumber = _storyConfigs.EnemiesMaxRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            #endregion Enemies scaling

            #endregion Scaling

            #region Modifiers

            _storyConfigs.IsModifiersEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.1 Enable Modifiers",
                false,
                LocalizationResolver.Localize("config_modifiers_enabled_description"));
            _storyConfigs.IsModifiersEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.ModifiersEnabled = _storyConfigs.IsModifiersEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.ModifiersStartDeathValue = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.2 Modifiers Start Death",
                1,
                LocalizationResolver.Localize("config_modifiers_start_death_description"));
            _storyConfigs.ModifiersStartDeathValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.ModifiersStartFromDeath = _storyConfigs.ModifiersStartDeathValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsModifiersRepeatingEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.3 Enable Modifiers Repeating",
                false,
                LocalizationResolver.Localize("config_repeating_enabled_description"));
            _storyConfigs.IsModifiersRepeatingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.ModifiersEnabled = _storyConfigs.IsModifiersRepeatingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsSpeedModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Speed Modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_speed_enabled_description"));
            _storyConfigs.IsSpeedModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.SpeedModifierEnabled = _storyConfigs.IsSpeedModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsTimerModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Timer Modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_timer_enabled_description"));
            _storyConfigs.IsTimerModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.TimerModifierEnabled = _storyConfigs.IsTimerModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsParryDamageModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Precice parry only modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_parry_damage_enabled_description"));
            _storyConfigs.IsParryDamageModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.ParryDirectDamageModifierEnabled = _storyConfigs.IsParryDamageModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsDamageBuildupModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Internal damage buildup modifier",
                true,
                LocalizationResolver.Localize("config_modifiers_internal_damage_enabled_description"));
            _storyConfigs.IsDamageBuildupModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.DamageBuildupModifierEnabled = _storyConfigs.IsDamageBuildupModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsRegenerationModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Regeneration modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_regeneration_enabled_description"));
            _storyConfigs.IsRegenerationModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RegenerationModifierEnabled = _storyConfigs.IsRegenerationModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsKnockbackModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Knockback modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_knockback_enabled_description"));
            _storyConfigs.IsKnockbackModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.KnockbackModifierEnabled = _storyConfigs.IsKnockbackModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsRandomArrowModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Random arrow modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_random_arrow_enabled_description"));
            _storyConfigs.IsRandomArrowModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomArrowModifierEnabled = _storyConfigs.IsRandomArrowModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsRandomTalismanModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Random talisman modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_random_talisman_enabled_description"));
            _storyConfigs.IsRandomTalismanModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomTalismanModifierEnabled = _storyConfigs.IsRandomTalismanModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsEnduranceModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Endurance modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_endurance_enabled_description"));
            _storyConfigs.IsEnduranceModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnduranceModifierEnabled = _storyConfigs.IsEnduranceModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsQiShieldModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Shield: Qi Shield modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_qi_shield_enabled_description"));
            _storyConfigs.IsQiShieldModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.QiShieldModifierEnabled = _storyConfigs.IsQiShieldModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsTimedShieldModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Shield: Cooldown Shield modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_cooldown_shield_enabled_description"));
            _storyConfigs.IsTimedShieldModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.TimedShieldModifierEnabled = _storyConfigs.IsTimedShieldModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsQiOverloadModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Qi Overload modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_qi_overload_enabled_description"));
            _storyConfigs.IsQiOverloadModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.QiOverloadModifierEnabled = _storyConfigs.IsQiOverloadModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsDistanceShieldModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Shield: Distance Shield modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_distance_shield_enabled_description"));
            _storyConfigs.IsDistanceShieldModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.DistanceShieldModifierEnabled = _storyConfigs.IsDistanceShieldModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.IsYanlaoGunModifierEnabled = Config.Bind(
                "7. Story Challenge Modifiers",
                "7.M Yanlaos Gun modiifer",
                true,
                LocalizationResolver.Localize("config_modifiers_yanlao_gun_enabled_description"));
            _storyConfigs.IsYanlaoGunModifierEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.YanlaoGunModifierEnabled = _storyConfigs.IsYanlaoGunModifierEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            #endregion Modifiers
        }

        public void HandleStoryChallengeConfigurationValues() {
            var config = StoryChallengeConfigurationManager.ChallengeConfiguration;

            config.EnableMod = _storyConfigs.IsModEnabled.Value;
            config.MaxBossCycles = _storyConfigs.MaxBossCycles.Value;
            config.MaxMinibossCycles = _storyConfigs.MaxMinibossCycles.Value;
            config.MaxEnemyCycles = _storyConfigs.MaxEnemyCycles.Value;

            config.AffectBosses = _storyConfigs.AffectBosses.Value;
            config.AffectMiniBosses = _storyConfigs.AffectMiniBosses.Value;
            config.AffectRegularEnemies = _storyConfigs.AffectEnemies.Value;

            config.RandomizeBossCyclesNumber = _storyConfigs.IsBossCyclesNumberRandomized.Value;
            config.MinRandomBossCycles = _storyConfigs.MinRandomBossCycles.Value;
            config.MaxRandomBossCycles = _storyConfigs.MaxRandomBossCycles.Value;

            config.RandomizeMiniBossCyclesNumber = _storyConfigs.IsMiniBossCyclesNumberRandomized.Value;
            config.MinRandomMiniBossCycles = _storyConfigs.MinRandomMiniBossCycles.Value;
            config.MaxRandomMiniBossCycles = _storyConfigs.MaxRandomMiniBossCycles.Value;

            config.RandomizeEnemyCyclesNumber = _storyConfigs.IsEnemyCyclesNumberRandomized.Value;
            config.MinRandomEnemyCycles = _storyConfigs.MinRandomEnemyCycles.Value;
            config.MaxRandomEnemyCycles = _storyConfigs.MaxRandomEnemyCycles.Value;

            #region Bosses scaling
            config.BossesEnableSpeedScaling = _storyConfigs.BossesIsSpeedScalingEnabled.Value;
            config.BossesMinSpeedScalingValue = _storyConfigs.BossesMinSpeedScalingValue.Value;
            config.BossesMaxSpeedScalingValue = _storyConfigs.BossesMaxSpeedScalingValue.Value;
            config.BossesMaxSpeedScalingCycle = _storyConfigs.BossesMaxSpeedScalingCycleValue.Value;

            config.BossesEnableModifiersScaling = _storyConfigs.BossesIsModifiersScalingEnabled.Value;
            config.BossesMaxModifiersNumber = _storyConfigs.BossesMaxModifiersNumber.Value;
            config.BossesMaxModifiersScalingCycle = _storyConfigs.BossesMaxModifiersNumberScalingValue.Value;

            config.BossesEnableRandomSpeedScaling = _storyConfigs.BossesIsRandomSpeedScalingEnabled.Value;
            config.BossesRandomSpeedScalingStartDeath = _storyConfigs.BossesStartRandomSpeedScalingDeath.Value;
            config.BossesMinRandomSpeedScalingValue = _storyConfigs.BossesMinRandomSpeedScalingValue.Value;
            config.BossesMaxRandomSpeedScalingValue = _storyConfigs.BossesMaxRandomSpeedScalingValue.Value;

            config.BossesEnableRandomModifiersScaling = _storyConfigs.BossesIsRandomSpeedScalingEnabled.Value;
            config.BossesRandomModifiersScalingStartDeath = _storyConfigs.BossesStartRandomSpeedScalingDeath.Value;
            config.BossesMinRandomModifiersNumber = _storyConfigs.BossesMinRandomModifiersScalingValue.Value;
            config.BossesMaxRandomModifiersNumber = _storyConfigs.BossesMaxRandomModifiersScalingValue.Value;
            #endregion Bosses scaling

            #region Minibosses scaling
            config.MinibossesEnableSpeedScaling = _storyConfigs.MinibossesIsSpeedScalingEnabled.Value;
            config.MinibossesMinSpeedScalingValue = _storyConfigs.MinibossesMinSpeedScalingValue.Value;
            config.MinibossesMaxSpeedScalingValue = _storyConfigs.MinibossesMaxSpeedScalingValue.Value;
            config.MinibossesMaxSpeedScalingCycle = _storyConfigs.MinibossesMaxSpeedScalingCycleValue.Value;

            config.MinibossesEnableModifiersScaling = _storyConfigs.MinibossesIsModifiersScalingEnabled.Value;
            config.MinibossesMaxModifiersNumber = _storyConfigs.MinibossesMaxModifiersNumber.Value;
            config.MinibossesMaxModifiersScalingCycle = _storyConfigs.MinibossesMaxModifiersNumberScalingValue.Value;

            config.MinibossesEnableRandomSpeedScaling = _storyConfigs.MinibossesIsRandomSpeedScalingEnabled.Value;
            config.MinibossesRandomSpeedScalingStartDeath = _storyConfigs.MinibossesStartRandomSpeedScalingDeath.Value;
            config.MinibossesMinRandomSpeedScalingValue = _storyConfigs.MinibossesMinRandomSpeedScalingValue.Value;
            config.MinibossesMaxRandomSpeedScalingValue = _storyConfigs.MinibossesMaxRandomSpeedScalingValue.Value;

            config.MinibossesEnableRandomModifiersScaling = _storyConfigs.MinibossesIsRandomSpeedScalingEnabled.Value;
            config.MinibossesRandomModifiersScalingStartDeath = _storyConfigs.MinibossesStartRandomSpeedScalingDeath.Value;
            config.MinibossesMinRandomModifiersNumber = _storyConfigs.MinibossesMinRandomModifiersScalingValue.Value;
            config.MinibossesMaxRandomModifiersNumber = _storyConfigs.MinibossesMaxRandomModifiersScalingValue.Value;
            #endregion Minibosses scaling

            #region Enemies scaling
            config.EnemiesEnableSpeedScaling = _storyConfigs.EnemiesIsSpeedScalingEnabled.Value;
            config.EnemiesMinSpeedScalingValue = _storyConfigs.EnemiesMinSpeedScalingValue.Value;
            config.EnemiesMaxSpeedScalingValue = _storyConfigs.EnemiesMaxSpeedScalingValue.Value;
            config.EnemiesMaxSpeedScalingCycle = _storyConfigs.EnemiesMaxSpeedScalingCycleValue.Value;

            config.EnemiesEnableModifiersScaling = _storyConfigs.EnemiesIsModifiersScalingEnabled.Value;
            config.EnemiesMaxModifiersNumber = _storyConfigs.EnemiesMaxModifiersNumber.Value;
            config.EnemiesMaxModifiersScalingCycle = _storyConfigs.EnemiesMaxModifiersNumberScalingValue.Value;

            config.EnemiesEnableRandomSpeedScaling = _storyConfigs.EnemiesIsRandomSpeedScalingEnabled.Value;
            config.EnemiesRandomSpeedScalingStartDeath = _storyConfigs.EnemiesStartRandomSpeedScalingDeath.Value;
            config.EnemiesMinRandomSpeedScalingValue = _storyConfigs.EnemiesMinRandomSpeedScalingValue.Value;
            config.EnemiesMaxRandomSpeedScalingValue = _storyConfigs.EnemiesMaxRandomSpeedScalingValue.Value;

            config.EnemiesEnableRandomModifiersScaling = _storyConfigs.EnemiesIsRandomSpeedScalingEnabled.Value;
            config.EnemiesRandomModifiersScalingStartDeath = _storyConfigs.EnemiesStartRandomSpeedScalingDeath.Value;
            config.EnemiesMinRandomModifiersNumber = _storyConfigs.EnemiesMinRandomModifiersScalingValue.Value;
            config.EnemiesMaxRandomModifiersNumber = _storyConfigs.EnemiesMaxRandomModifiersScalingValue.Value;
            #endregion Enemies scaling

            config.ModifiersEnabled = _storyConfigs.IsModifiersEnabled.Value;
            config.AllowRepeatModifiers = _storyConfigs.IsModifiersRepeatingEnabled.Value;
            config.SpeedModifierEnabled = _storyConfigs.IsSpeedModifierEnabled.Value;
            config.TimerModifierEnabled = _storyConfigs.IsTimerModifierEnabled.Value;
            config.ParryDirectDamageModifierEnabled = _storyConfigs.IsParryDamageModifierEnabled.Value;
            config.DamageBuildupModifierEnabled = _storyConfigs.IsDamageBuildupModifierEnabled.Value;
            config.RegenerationModifierEnabled = _storyConfigs.IsRegenerationModifierEnabled.Value;
            config.KnockbackModifierEnabled = _storyConfigs.IsKnockbackModifierEnabled.Value;            
            config.RandomArrowModifierEnabled = _storyConfigs.IsRandomArrowModifierEnabled.Value;
            config.RandomTalismanModifierEnabled = _storyConfigs.IsRandomTalismanModifierEnabled.Value;
            config.EnduranceModifierEnabled = _storyConfigs.IsEnduranceModifierEnabled.Value;
            config.QiShieldModifierEnabled = _storyConfigs.IsQiShieldModifierEnabled.Value;
            config.TimedShieldModifierEnabled = _storyConfigs.IsTimedShieldModifierEnabled.Value;
            config.QiOverloadModifierEnabled = _storyConfigs.IsQiOverloadModifierEnabled.Value;
            config.DistanceShieldModifierEnabled = _storyConfigs.IsDistanceShieldModifierEnabled.Value;
            config.YanlaoGunModifierEnabled = _storyConfigs.IsYanlaoGunModifierEnabled.Value;
            config.ModifiersStartFromDeath = _storyConfigs.ModifiersStartDeathValue.Value;

            StoryChallengeConfigurationManager.ChallengeConfiguration = config;
        }
    }
}
