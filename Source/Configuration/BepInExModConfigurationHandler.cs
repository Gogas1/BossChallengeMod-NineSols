using BepInEx.Configuration;
using BossChallengeMod.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration {
    public class BepInExModConfigurationHandler {

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

        private ConfigFile Config = null!;
        private ChallengeConfigurationManager ChallengeConfigurationManager = null!;
        private UIConfiguration UIConfiguration = null!;

        public BepInExModConfigurationHandler(
            ConfigFile config, 
            ChallengeConfigurationManager challengeConfigurationManager, 
            UIConfiguration uIConfiguration) {
            Config = config;
            ChallengeConfigurationManager = challengeConfigurationManager;
            UIConfiguration = uIConfiguration;
        }

        public void InitChallengeConfiguration() {
            isCyclingEnabled = Config.Bind(
            "1. General",
            "1.1 Enable Boss Revival",
            true,
            LocalizationResolver.Localize("config_cycling_enabled_description"));
            isCyclingEnabled.SettingChanged += (_, _) => {
                var config = ChallengeConfigurationManager.ChallengeConfiguration;
                config.EnableRestoration = isCyclingEnabled.Value;
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
                config.MaxCycles = maxCycles.Value;
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

            //isKnockoutModifierEnabled = Config.Bind(
            //    "3. Modifiers",
            //    "3.M Knockout modiifer",
            //    true,
            //    LocalizationResolver.Localize("config_modifiers_knockout_enabled_description"));
            //isKnockoutModifierEnabled.SettingChanged += (_, _) => {
            //    var config = ChallengeConfigurationManager.ChallengeConfiguration;
            //    config.KnockoutModifierEnabled = isKnockoutModifierEnabled.Value;
            //    ChallengeConfigurationManager.ChallengeConfiguration = config;
            //};

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
            config.EnableRestoration = isCyclingEnabled.Value;
            config.UseSingleRecordKey = useSingleRecordsKey.Value;
            config.MaxCycles = maxCycles.Value;

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
    }
}
