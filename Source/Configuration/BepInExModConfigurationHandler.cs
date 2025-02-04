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
                "3.M Yanlago Gun modiifer",
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

            config.EnableSpeedScaling = isSpeedScalingEnabled.Value;
            config.MinSpeedScalingValue = minSpeedScalingValue.Value;
            config.MaxSpeedScalingValue = maxSpeedScalingValue.Value;
            config.MaxSpeedScalingCycle = maxSpeedScalingCycleValue.Value;

            config.EnableModifiersScaling = isModifiersScalingEnabled.Value;
            config.MaxModifiersNumber = maxModifiersNumber.Value;
            config.MaxModifiersScalingCycle = maxModifiersNumberScalingValue.Value;

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
                1,
                LocalizationResolver.Localize("config_cycles_number_description"));
            _storyConfigs.MaxBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxBossCycles = _storyConfigs.MaxBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxMinibossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.2.2 Miniboss deaths number",
                1,
                LocalizationResolver.Localize("config_story_miniboss_cycles_number_description"));
            _storyConfigs.MaxMinibossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxMinibossCycles = _storyConfigs.MaxMinibossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxEnemyCycles = Config.Bind(
                "5. Story Challenge General",
                "5.2.3 Regular enemy deaths number",
                1,
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
                "5.4.1 Randomize boss death number",
                false,
                LocalizationResolver.Localize("config_boss_deaths_randomized_enabled_description"));
            _storyConfigs.IsBossCyclesNumberRandomized.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomizeBossCyclesNumber = _storyConfigs.IsBossCyclesNumberRandomized.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.4.2 Min boss deaths random number",
                1,
                LocalizationResolver.Localize("config_boss_deaths_randomized_min_description"));
            _storyConfigs.MinRandomBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomBossCycles = _storyConfigs.MinRandomBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.4.3 Max boss deaths random number",
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
                "5.5.1 Randomize miniboss death number",
                false,
                LocalizationResolver.Localize("config_story_miniboss_deaths_randomized_description"));
            _storyConfigs.IsMiniBossCyclesNumberRandomized.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomizeMiniBossCyclesNumber = _storyConfigs.IsMiniBossCyclesNumberRandomized.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomMiniBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.5.2 Min miniboss deaths random number",
                1,
                LocalizationResolver.Localize("config_story_miniboss_deaths_randomized_min_description"));
            _storyConfigs.MinRandomMiniBossCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomMiniBossCycles = _storyConfigs.MinRandomMiniBossCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomMiniBossCycles = Config.Bind(
                "5. Story Challenge General",
                "5.5.3 Max miniboss deaths random number",
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
                "5.6.1 Randomize regular enemy death number",
                false,
                LocalizationResolver.Localize("config_story_enemy_deaths_randomized_description"));
            _storyConfigs.IsEnemyCyclesNumberRandomized.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomizeEnemyCyclesNumber = _storyConfigs.IsEnemyCyclesNumberRandomized.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomEnemyCycles = Config.Bind(
                "5. Story Challenge General",
                "5.6.2 Min regular enemy deaths random number",
                1,
                LocalizationResolver.Localize("config_story_enemy_deaths_randomized_min_description"));
            _storyConfigs.MinRandomEnemyCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomEnemyCycles = _storyConfigs.MinRandomEnemyCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomEnemyCycles = Config.Bind(
                "5. Story Challenge General",
                "5.6.3 Max regular enemy deaths random number",
                3,
                LocalizationResolver.Localize("config_story_enemy_deaths_randomized_max_description"));
            _storyConfigs.MaxRandomEnemyCycles.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomEnemyCycles = _storyConfigs.MaxRandomEnemyCycles.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            #endregion General

            #region Scaling

            //Speed scaling
            _storyConfigs.IsSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1 Enable Speed Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_enabled_description"));
            _storyConfigs.IsSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableSpeedScaling = _storyConfigs.IsSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.1 Scaling: Initial Speed",
                1.0f,
                LocalizationResolver.Localize("config_scaling_minspeed_description"));
            _storyConfigs.MinSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinSpeedScalingValue = _storyConfigs.MinSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.2 Scaling: Maximum Speed",
                1.35f,
                LocalizationResolver.Localize("config_scaling_maxspeed_description"));
            _storyConfigs.MaxSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.MaxSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxSpeedScalingCycleValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.1.3 Maximum Speed Scaling After Deaths",
                5,
                LocalizationResolver.Localize("config_scaling_scaling_cycle_description"));
            _storyConfigs.MaxSpeedScalingCycleValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingCycle = _storyConfigs.MaxSpeedScalingCycleValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Modifiers scaling
            _storyConfigs.IsModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2 Enable Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_scaling_modifiers_enabled_description"));
            _storyConfigs.IsModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableModifiersScaling = _storyConfigs.IsModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxModifiersNumber = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.1 Scaling: Maximum Modifiers Number",
                3,
                LocalizationResolver.Localize("config_scaling_maxmodifiers_description"));
            _storyConfigs.MaxModifiersNumber.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersNumber = _storyConfigs.MaxModifiersNumber.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxModifiersNumberScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.2.2 Maximum Modifiers Number Scaling After Deaths",
                3,
                LocalizationResolver.Localize("config_scaling_modifiers_scaling_cycle_description"));
            _storyConfigs.MaxModifiersNumberScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxModifiersScalingCycle = _storyConfigs.MaxModifiersNumberScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random speed scaling
            _storyConfigs.IsRandomSpeedScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3 Enable Random Speed Scaling",
                false,
                LocalizationResolver.Localize("config_rand_speed_scaling_enabled_description"));
            _storyConfigs.IsRandomSpeedScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomSpeedScaling = _storyConfigs.IsRandomSpeedScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.StartRandomSpeedScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.1 Random Speed Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_speed_scaling_start_death_description"));
            _storyConfigs.StartRandomSpeedScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomSpeedScalingStartDeath = _storyConfigs.StartRandomSpeedScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.2 Random Scaling: Minimal Speed",
                1.0f,
                LocalizationResolver.Localize("config_rand_speed_scaling_minspeed_description"));
            _storyConfigs.MinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomSpeedScalingValue = _storyConfigs.MinRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomSpeedScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.3.3 Random Scaling: Maximum Speed",
                1.5f,
                LocalizationResolver.Localize("config_rand_speed_scaling_maxspeed_description"));
            _storyConfigs.MaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxSpeedScalingValue = _storyConfigs.MaxRandomSpeedScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            //Random modifiers scaling
            _storyConfigs.IsRandomModifiersScalingEnabled = Config.Bind(
                "6. Story Challenge Scaling",
                "6.4 Enable Random Modifiers Scaling",
                false,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_enabled_description"));
            _storyConfigs.IsRandomModifiersScalingEnabled.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRandomModifiersScaling = _storyConfigs.IsRandomModifiersScalingEnabled.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.StartRandomModifiersScalingDeath = Config.Bind(
                "6. Story Challenge Scaling",
                "6.4.1 Random Modifiers Scaling After Deaths",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_start_death_description"));
            _storyConfigs.StartRandomModifiersScalingDeath.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.RandomModifiersScalingStartDeath = _storyConfigs.StartRandomModifiersScalingDeath.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MinRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.4.2 Random Scaling: Min Modifiers Number",
                1,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_min_description"));
            _storyConfigs.MinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MinRandomModifiersNumber = _storyConfigs.MinRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

            _storyConfigs.MaxRandomModifiersScalingValue = Config.Bind(
                "6. Story Challenge Scaling",
                "6.4.3 Random Scaling: Max Modifiers Number",
                4,
                LocalizationResolver.Localize("config_rand_modifiers_scaling_max_description"));
            _storyConfigs.MaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                var config = StoryChallengeConfigurationManager.ChallengeConfiguration;
                config.MaxRandomModifiersNumber = _storyConfigs.MaxRandomModifiersScalingValue.Value;
                StoryChallengeConfigurationManager.ChallengeConfiguration = config;
            };

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
                "7.M Yanlago Gun modiifer",
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

            config.EnableSpeedScaling = _storyConfigs.IsSpeedScalingEnabled.Value;
            config.MinSpeedScalingValue = _storyConfigs.MinSpeedScalingValue.Value;
            config.MaxSpeedScalingValue = _storyConfigs.MaxSpeedScalingValue.Value;
            config.MaxSpeedScalingCycle = _storyConfigs.MaxSpeedScalingCycleValue.Value;

            config.EnableModifiersScaling = _storyConfigs.IsModifiersScalingEnabled.Value;
            config.MaxModifiersNumber = _storyConfigs.MaxModifiersNumber.Value;
            config.MaxModifiersScalingCycle = _storyConfigs.MaxModifiersNumberScalingValue.Value;

            config.EnableRandomSpeedScaling = _storyConfigs.IsRandomSpeedScalingEnabled.Value;
            config.RandomSpeedScalingStartDeath = _storyConfigs.StartRandomSpeedScalingDeath.Value;
            config.MinRandomSpeedScalingValue = _storyConfigs.MinRandomSpeedScalingValue.Value;
            config.MaxRandomSpeedScalingValue = _storyConfigs.MaxRandomSpeedScalingValue.Value;

            config.EnableRandomModifiersScaling = _storyConfigs.IsRandomSpeedScalingEnabled.Value;
            config.RandomModifiersScalingStartDeath = _storyConfigs.StartRandomSpeedScalingDeath.Value;
            config.MinRandomModifiersNumber = _storyConfigs.MinRandomModifiersScalingValue.Value;
            config.MaxRandomModifiersNumber = _storyConfigs.MaxRandomModifiersScalingValue.Value;

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

            StoryChallengeConfigurationManager.ChallengeConfiguration = config;
        }
    }
}
