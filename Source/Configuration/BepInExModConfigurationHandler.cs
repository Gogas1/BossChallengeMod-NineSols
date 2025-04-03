using BepInEx.Configuration;
using BossChallengeMod.Configuration.Fields;
using BossChallengeMod.Configuration.Holders;
using BossChallengeMod.Global;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Configuration {
    public class BepInExModConfigurationHandler {

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

        private ReworkedConfigsHolder configHolder = new();

        private ConfigFile Config = null!;
        private ChallengeConfigurationManager ChallengeConfigurationManager = null!;
        private UIConfiguration UIConfiguration = null!;



        public BepInExModConfigurationHandler(
            ConfigFile config,
            ChallengeConfigurationManager challengeConfigurationManager,
            UIConfiguration uIConfiguration,
            StoryChallengeConfigurationManager storyChallengeConfigurationManager) {

            Config = config;
            ChallengeConfigurationManager = challengeConfigurationManager;
            UIConfiguration = uIConfiguration;
        }

        public float CalculateRightColumnWidth() {
            var width = Mathf.Min(Screen.width, 650);
            var height = Screen.height < 560 ? Screen.height : Screen.height - 100;
            var offsetX = Mathf.RoundToInt((Screen.width - width) / 2f);
            var offsetY = Mathf.RoundToInt((Screen.height - height) / 2f);

            var LeftColumnWidth = Mathf.RoundToInt(width / 2.5f);
            var RightColumnWidth = (int)width - LeftColumnWidth - 115;

            return RightColumnWidth;
        }

        public void InitChallengeConfiguration() {
            InitGeneralConfig();
            InitModifiersConfig();
            InitBossesConfig();
            InitMinibossesConfig();
            InitEnemiesConfig();
        }

        private void InitGeneralConfig() {
            configHolder.IsMoBEnabled = Config.Bind(
                "1. General",
                "mob enabled",
                true,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_general_mob_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_general_mob_enabled_name"),
                        Order = 0,
                    }));
            configHolder.IsMoBEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsEnabledInMoB = configHolder.IsMoBEnabled.Value;
            };

            configHolder.IsNormalEnabled = Config.Bind(
                "1. General",
                "regular enabled",
                true,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_general_regular_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_general_regular_enabled_name"),
                        Order = 1,
                    }));
            configHolder.IsNormalEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsEnabledInNormal = configHolder.IsNormalEnabled.Value;
            };
        }

        private void InitModifiersConfig() {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            #region Hidden Config

            configHolder.ModifiersStartDeathValue = Config.Bind(
                "2. Modifiers",
                "modifiers start death",
                1,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.ModifiersStartDeathValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.ModifiersStartDeath = configHolder.ModifiersStartDeathValue.Value;
            };

            configHolder.IsModifiersRepeatingEnabled = Config.Bind(
                "2. Modifiers",
                "modifiers repeat enabled",
                false,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsModifiersRepeatingEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsRepeatingEnabled = configHolder.IsModifiersRepeatingEnabled.Value;
            };

            configHolder.IsSpeedModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier speed",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsSpeedModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsSpeedModifierEnabled = configHolder.IsSpeedModifierEnabled.Value;
            };

            configHolder.IsTimerModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier timer",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsTimerModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsTimerModifierEnabled = configHolder.IsTimerModifierEnabled.Value;
            };

            configHolder.IsParryDamageModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier parry damage",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsParryDamageModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsParryDamageModifierEnabled = configHolder.IsParryDamageModifierEnabled.Value;
            };

            configHolder.IsDamageBuildupModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier damage buildup",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsDamageBuildupModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsDamageBuildupModifierEnabled = configHolder.IsDamageBuildupModifierEnabled.Value;
            };

            configHolder.IsRegenerationModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier regeneration",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsRegenerationModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsRegenerationModifierEnabled = configHolder.IsRegenerationModifierEnabled.Value;
            };

            configHolder.IsKnockbackModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier knockback",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsKnockbackModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsKnockbackModifierEnabled = configHolder.IsKnockbackModifierEnabled.Value;
            };

            configHolder.IsRandomArrowModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier random arrow",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsRandomArrowModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsRandomArrowModifierEnabled = configHolder.IsRandomArrowModifierEnabled.Value;
            };

            configHolder.IsRandomTalismanModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier random talisman",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsRandomTalismanModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsRandomTalismanModifierEnabled = configHolder.IsRandomTalismanModifierEnabled.Value;
            };

            configHolder.IsEnduranceModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier endurance",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsEnduranceModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsEnduranceModifierEnabled = configHolder.IsEnduranceModifierEnabled.Value;
            };

            configHolder.IsQiShieldModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier qi shield",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsQiShieldModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsQiShieldModifierEnabled = configHolder.IsQiShieldModifierEnabled.Value;
            };

            configHolder.IsCooldownShieldModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier cooldown shield",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsCooldownShieldModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsCooldownShieldModifierEnabled = configHolder.IsCooldownShieldModifierEnabled.Value;
            };

            configHolder.IsQiOverloadModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier qi overload",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsQiOverloadModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsQiOverloadModifierEnabled = configHolder.IsQiOverloadModifierEnabled.Value;
            };

            configHolder.IsDistanceShieldModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier distance shield",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsDistanceShieldModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsDistanceShieldModifierEnabled = configHolder.IsDistanceShieldModifierEnabled.Value;
            };

            configHolder.IsYanlaoGunModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier yanlao gun",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsYanlaoGunModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsYanlaoGunModifierEnabled = configHolder.IsYanlaoGunModifierEnabled.Value;
            };

            configHolder.IsQiBombModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier qi bomb",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsQiBombModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsQiBombModifierEnabled = configHolder.IsQiBombModifierEnabled.Value;
            };

            configHolder.IsShieldBreakBombModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier shield bomb",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsShieldBreakBombModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsShieldBreakBombModifierEnabled = configHolder.IsShieldBreakBombModifierEnabled.Value;
            };

            configHolder.IsQiOverloadBombModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier qi overload bomb",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsQiOverloadBombModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsQiOverloadBombModifierEnabled = configHolder.IsQiOverloadBombModifierEnabled.Value;
            };

            configHolder.IsQiDepletionBombModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier qi depletion bomb",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsQiDepletionBombModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsQiDepletionBombModifierEnabled = configHolder.IsQiDepletionBombModifierEnabled.Value;
            };

            configHolder.IsCooldownBombModifierEnabled = Config.Bind(
                "2. Modifiers",
                "modifier coolldown bomb",
                true,
                new ConfigDescription("", null, new ConfigurationManagerAttributes { Browsable = false, }));
            configHolder.IsCooldownBombModifierEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsCooldownBombModifierEnabled = configHolder.IsCooldownBombModifierEnabled.Value;
            };
            #endregion Hidden Config

            var modifiersTabsDrawer = new ConfigTabsDrawer();
            modifiersTabsDrawer.Tabs.AddRange([disabledText, enabledText]);

            IntField modifiersStartDeathField = new IntField(
                            LocalizationResolver.Localize("config_modifiers_start_death_name"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_start_death_desc"),
                            configHolder.ModifiersStartDeathValue.Value);
            modifiersTabsDrawer.AddField(1, modifiersStartDeathField);

            BooleanField modifiersRepeatSwitch = new BooleanField(
                            LocalizationResolver.Localize("config_modifiers_modifiers_repeat_enabled_name"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_modifiers_repeat_enabled_desc"),
                            configHolder.IsModifiersRepeatingEnabled.Value);
            modifiersTabsDrawer.AddField(1, modifiersRepeatSwitch);

            #region Modifiers switches init
            //Speed
            BooleanField speedModSwitch = new BooleanField(
                            LocalizationResolver.Localize("speed_temp"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_speed_enabled_description"),
                            configHolder.IsSpeedModifierEnabled.Value);
            speedModSwitch.AddValueChangeHandler(arg => { configHolder.IsSpeedModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, speedModSwitch);

            //Timer
            BooleanField timerModSwitch = new BooleanField(
                            LocalizationResolver.Localize("timer"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_timer_enabled_description"),
                            configHolder.IsTimerModifierEnabled.Value);
            timerModSwitch.AddValueChangeHandler(arg => { configHolder.IsTimerModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, timerModSwitch);

            //Parry damage
            BooleanField parryDamageModSwitch = new BooleanField(
                            LocalizationResolver.Localize("parry_damage"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_parry_damage_enabled_description"),
                            configHolder.IsParryDamageModifierEnabled.Value);
            parryDamageModSwitch.AddValueChangeHandler(arg => { configHolder.IsParryDamageModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, parryDamageModSwitch);

            //Damage buildup
            BooleanField damageBuildupModSwitch = new BooleanField(
                            LocalizationResolver.Localize("damage_buildup"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_internal_damage_enabled_description"),
                            configHolder.IsDamageBuildupModifierEnabled.Value);
            damageBuildupModSwitch.AddValueChangeHandler(arg => { configHolder.IsDamageBuildupModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, damageBuildupModSwitch);

            //Regeneration
            BooleanField regenerationModSwitch = new BooleanField(
                            LocalizationResolver.Localize("regeneration"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_regeneration_enabled_description"),
                            configHolder.IsRegenerationModifierEnabled.Value);
            regenerationModSwitch.AddValueChangeHandler(arg => { configHolder.IsRegenerationModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, regenerationModSwitch);

            //Knockback
            BooleanField knockbackModSwitch = new BooleanField(
                            LocalizationResolver.Localize("knockback"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_knockback_enabled_description"),
                            configHolder.IsKnockbackModifierEnabled.Value);
            knockbackModSwitch.AddValueChangeHandler(arg => { configHolder.IsKnockbackModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, knockbackModSwitch);

            //Random arrow
            BooleanField randomArrowModSwitch = new BooleanField(
                            LocalizationResolver.Localize("random_arrow"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_random_arrow_enabled_description"),
                            configHolder.IsRandomArrowModifierEnabled.Value);
            randomArrowModSwitch.AddValueChangeHandler(arg => { configHolder.IsRandomArrowModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, randomArrowModSwitch);

            //Random talisman
            BooleanField randomTaliModSwitch = new BooleanField(
                            LocalizationResolver.Localize("random_talisman"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_random_talisman_enabled_description"),
                            configHolder.IsRandomTalismanModifierEnabled.Value);
            randomTaliModSwitch.AddValueChangeHandler(arg => { configHolder.IsRandomTalismanModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, randomTaliModSwitch);

            //Endurance
            BooleanField enduranceModSwitch = new BooleanField(
                            LocalizationResolver.Localize("endurance"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_endurance_enabled_description"),
                            configHolder.IsEnduranceModifierEnabled.Value);
            enduranceModSwitch.AddValueChangeHandler(arg => { configHolder.IsEnduranceModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, enduranceModSwitch);

            //Qi-Shield
            BooleanField qiShieldModSwitch = new BooleanField(
                            LocalizationResolver.Localize("qi_shield"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_qi_shield_enabled_description"),
                            configHolder.IsQiShieldModifierEnabled.Value);
            qiShieldModSwitch.AddValueChangeHandler(arg => { configHolder.IsQiShieldModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, qiShieldModSwitch);

            //Coolldown shield
            BooleanField cooldownShieldModSwitch = new BooleanField(
                            LocalizationResolver.Localize("timer_shield"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_cooldown_shield_enabled_description"),
                            configHolder.IsCooldownShieldModifierEnabled.Value);
            cooldownShieldModSwitch.AddValueChangeHandler(arg => { configHolder.IsCooldownShieldModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, cooldownShieldModSwitch);

            //Qi-Overload
            BooleanField qiOverloadModSwitch = new BooleanField(
                            LocalizationResolver.Localize("qi_overload"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_qi_overload_enabled_description"),
                            configHolder.IsQiOverloadModifierEnabled.Value);
            qiOverloadModSwitch.AddValueChangeHandler(arg => { configHolder.IsQiOverloadModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, qiOverloadModSwitch);

            //Distance shield
            BooleanField distanceShieldModSwitch = new BooleanField(
                            LocalizationResolver.Localize("distance_shield"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_distance_shield_enabled_description"),
                            configHolder.IsDistanceShieldModifierEnabled.Value);
            distanceShieldModSwitch.AddValueChangeHandler(arg => { configHolder.IsDistanceShieldModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, distanceShieldModSwitch);

            //Yanlao Gun
            BooleanField yaGunModSwitch = new BooleanField(
                            LocalizationResolver.Localize("ya_gun"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_yanlao_gun_enabled_description"),
                            configHolder.IsYanlaoGunModifierEnabled.Value);
            yaGunModSwitch.AddValueChangeHandler(arg => { configHolder.IsYanlaoGunModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, yaGunModSwitch);

            //Qi-Bomb
            BooleanField qiBombModSwitch = new BooleanField(
                            LocalizationResolver.Localize("qi_bomb"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_qi_bomb_enabled_description"),
                            configHolder.IsQiBombModifierEnabled.Value);
            qiBombModSwitch.AddValueChangeHandler(arg => { configHolder.IsQiBombModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, qiBombModSwitch);

            //Shield break bomb
            BooleanField shieldBreakBombModSwitch = new BooleanField(
                            LocalizationResolver.Localize("shield_break_bomb"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_shield_break_bomb_enabled_description"),
                            configHolder.IsShieldBreakBombModifierEnabled.Value);
            shieldBreakBombModSwitch.AddValueChangeHandler(arg => { configHolder.IsShieldBreakBombModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, shieldBreakBombModSwitch);

            //Qi Overload bomb
            BooleanField qiOverloadBombModSwitch = new BooleanField(
                            LocalizationResolver.Localize("qi_overload_bomb"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_qi_overload_bomb_enabled_description"),
                            configHolder.IsQiOverloadBombModifierEnabled.Value);
            qiOverloadBombModSwitch.AddValueChangeHandler(arg => { configHolder.IsQiOverloadBombModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, qiOverloadBombModSwitch);

            //Qi Depletion bomb
            BooleanField qiDepletionBombModSwitch = new BooleanField(
                            LocalizationResolver.Localize("qi_depletion_bomb"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_qi_depletion_bomb_enabled_description"),
                            configHolder.IsQiDepletionBombModifierEnabled.Value);
            qiDepletionBombModSwitch.AddValueChangeHandler(arg => { configHolder.IsQiDepletionBombModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, qiDepletionBombModSwitch);

            //Cooldown bomb
            BooleanField cooldownBombModSwitch = new BooleanField(
                            LocalizationResolver.Localize("cooldown_bomb"),
                            CalculateRightColumnWidth,
                            LocalizationResolver.Localize("config_modifiers_cooldown_bomb_enabled_description"),
                            configHolder.IsCooldownBombModifierEnabled.Value);
            cooldownBombModSwitch.AddValueChangeHandler(arg => { configHolder.IsCooldownBombModifierEnabled.Value = arg; });
            modifiersTabsDrawer.AddField(1, cooldownBombModSwitch);
            #endregion Modifiers switches init

            configHolder.IsModifiersEnabled = Config.Bind(
                "2. Modifiers",
                "modifiers enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_modifiers_modifiers_enabled_name"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_modifiers_modifiers_enabled_desc"),
                        Order = 0,
                    }, 
                    modifiersTabsDrawer.GetConfigAttributes()));
            configHolder.IsModifiersEnabled.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.IsModifiersEnabled = configHolder.IsModifiersEnabled.Value;
            };

            modifiersTabsDrawer.SelectedTab = configHolder.IsModifiersEnabled.Value ? 1 : 0;
            modifiersTabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.IsModifiersEnabled.Value = true;
            });
            modifiersTabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.IsModifiersEnabled.Value = false;
            });
        }

        #region Boss config
        private void InitBossesConfig() {
            string sectionName = "3. Bosses config";            

            configHolder.AffectBosses = Config.Bind(
                sectionName,
                "bosses affected",
                true,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_affect_bosses_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_affect_bosses_name"),
                        Order = 0,
                    }));
            configHolder.AffectBosses.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.AffectBosses = configHolder.AffectBosses.Value;
            };

            configHolder.MaxBossCycles = Config.Bind(
                sectionName,
                "bosses deaths",
                -1,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_boss_deaths_number_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_boss_deaths_number_name"),
                        Order = 1,
                    }));
            configHolder.MaxBossCycles.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MaxBossCycles = configHolder.MaxBossCycles.Value;
            };

            SetupBossSpeedScaling(sectionName);
            SetupBossModifiersScaling(sectionName);
            SetupBossRandomSpeedScaling(sectionName);
            SetupBossRandomModifiersScaling(sectionName);
        }

        private void SetupBossSpeedScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.BossesMinSpeedScalingValue = Config.Bind(
                sectionName,
                "boss speed scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesMinSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesMinSpeedScalingValue = configHolder.BossesMinSpeedScalingValue.Value;
            };

            configHolder.BossesSpeedScalingStepValue = Config.Bind(
                sectionName,
                "boss speed scaling step value",
                0.06f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesSpeedScalingStepValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesSpeedScalingStepValue = configHolder.BossesSpeedScalingStepValue.Value;
            };

            configHolder.BossesSpeedStepsCapValue = Config.Bind(
                sectionName,
                "boss speed scaling steps cap",
                5,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesSpeedStepsCapValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesSpeedStepsCapValue = configHolder.BossesSpeedStepsCapValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_boss_speed_scaling_minspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_speed_scaling_minspeed_desc"),
                configHolder.BossesMinSpeedScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.BossesMinSpeedScalingValue.Value = arg;
            });

            var stepValueField = new FloatField(
                LocalizationResolver.Localize("config_boss_speed_scaling_step_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_speed_scaling_step_desc"),
                configHolder.BossesSpeedScalingStepValue.Value);
            stepValueField.AddValueChangeHandler(arg => {
                configHolder.BossesSpeedScalingStepValue.Value = arg;
            });

            var stepsCapField = new IntField(
                LocalizationResolver.Localize("config_boss_speed_scaling_steps_cap_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_speed_scaling_steps_cap_desc"),
                configHolder.BossesSpeedStepsCapValue.Value);
            stepsCapField.AddValueChangeHandler(arg => {
                configHolder.BossesSpeedStepsCapValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, stepValueField);
            tabsDrawer.AddField(1, stepsCapField);

            configHolder.BossesIsSpeedScalingEnabled = Config.Bind(
                sectionName,
                "boss speed scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_boss_speed_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_boss_speed_scaling_enabled_name"),
                        Order = 2
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.BossesIsSpeedScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.BossesIsSpeedScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.BossesIsSpeedScalingEnabled.Value = false;
            });
        }

        private void SetupBossModifiersScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.BossesMinModifiersNumber = Config.Bind(
                sectionName,
                "boss modifiers scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesMinModifiersNumber.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesMinModifiersNumber = configHolder.BossesMinModifiersNumber.Value;
            };

            configHolder.BossesModifiersScalingStepValue = Config.Bind(
                sectionName,
                "boss modifiers scaling step value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesSpeedScalingStepValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesModifiersScalingStepValue = configHolder.BossesModifiersScalingStepValue.Value;
            };

            configHolder.BossesModifiersStepsCapValue = Config.Bind(
                sectionName,
                "boss modifiers scaling steps cap",
                2,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesSpeedStepsCapValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesModifiersStepsCapValue = configHolder.BossesModifiersStepsCapValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_boss_modifiers_scaling_min_number_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_modifiers_scaling_min_number_desc"),
                configHolder.BossesMinModifiersNumber.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.BossesMinModifiersNumber.Value = arg;
            });

            var stepValueField = new FloatField(
                LocalizationResolver.Localize("config_boss_modifiers_scaling_step_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_modifiers_scaling_step_desc"),
                configHolder.BossesModifiersScalingStepValue.Value);
            stepValueField.AddValueChangeHandler(arg => {
                configHolder.BossesModifiersScalingStepValue.Value = arg;
            });

            var stepsCapField = new IntField(
                LocalizationResolver.Localize("config_boss_modifiers_scaling_steps_cap_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_modifiers_scaling_steps_cap_desc"),
                configHolder.BossesModifiersStepsCapValue.Value);
            stepsCapField.AddValueChangeHandler(arg => {
                configHolder.BossesModifiersStepsCapValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, stepValueField);
            tabsDrawer.AddField(1, stepsCapField);

            configHolder.BossesIsModifiersScalingEnabled = Config.Bind(
                sectionName,
                "boss modifiers scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_boss_modifiers_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_boss_modifiers_scaling_enabled_name"),
                        Order = 3
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.BossesIsModifiersScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.BossesIsModifiersScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.BossesIsModifiersScalingEnabled.Value = false;
            });
        }
        
        private void SetupBossRandomSpeedScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.BossesMinRandomSpeedScalingValue = Config.Bind(
                sectionName,
                "boss random speed scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesMinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesMinRandomSpeedScalingValue = configHolder.BossesMinRandomSpeedScalingValue.Value;
            };

            configHolder.BossesMaxRandomSpeedScalingValue = Config.Bind(
                sectionName,
                "boss random speed scaling max value",
                1.5f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesMaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesMaxRandomSpeedScalingValue = configHolder.BossesMaxRandomSpeedScalingValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_boss_rand_speed_scaling_minspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_rand_speed_scaling_minspeed_desc"),
                configHolder.BossesMinRandomSpeedScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.BossesMinRandomSpeedScalingValue.Value = arg;
            });

            var maxValueField = new FloatField(
                LocalizationResolver.Localize("config_boss_rand_speed_scaling_maxspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_rand_speed_scaling_maxspeed_desc"),
                configHolder.BossesMaxRandomSpeedScalingValue.Value);
            maxValueField.AddValueChangeHandler(arg => {
                configHolder.BossesMaxRandomSpeedScalingValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, maxValueField);

            configHolder.BossesIsRandomSpeedScalingEnabled = Config.Bind(
                sectionName,
                "boss random speed scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_boss_rand_speed_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_boss_rand_speed_scaling_enabled_name"),
                        Order = 4
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.BossesIsRandomSpeedScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.BossesIsRandomSpeedScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.BossesIsRandomSpeedScalingEnabled.Value = false;
            });
        }

        private void SetupBossRandomModifiersScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.BossesMinRandomModifiersScalingValue = Config.Bind(
                sectionName,
                "boss random modifiers scaling min value",
                1,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesMinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesMinRandomModifiersScalingValue = configHolder.BossesMinRandomModifiersScalingValue.Value;
            };

            configHolder.BossesMaxRandomModifiersScalingValue = Config.Bind(
                sectionName,
                "boss random modifiers scaling max value",
                3,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.BossesMaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.BossesMaxRandomModifiersScalingValue = configHolder.BossesMaxRandomModifiersScalingValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new IntField(
                LocalizationResolver.Localize("config_boss_rand_modifiers_scaling_min_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_rand_modifiers_scaling_min_desc"),
                configHolder.BossesMinRandomModifiersScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.BossesMinRandomModifiersScalingValue.Value = arg;
            });

            var maxValueField = new IntField(
                LocalizationResolver.Localize("config_boss_rand_modifiers_scaling_max_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_boss_rand_modifiers_scaling_max_desc"),
                configHolder.BossesMaxRandomModifiersScalingValue.Value);
            maxValueField.AddValueChangeHandler(arg => {
                configHolder.BossesMaxRandomModifiersScalingValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, maxValueField);

            configHolder.BossesIsRandomModifiersScalingEnabled = Config.Bind(
                sectionName,
                "boss random modifiers scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_boss_rand_modifiers_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_boss_rand_modifiers_scaling_enabled_name"),
                        Order = 5
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.BossesIsRandomModifiersScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.BossesIsRandomModifiersScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.BossesIsRandomModifiersScalingEnabled.Value = false;
            });
        }
        #endregion Boss config

        #region Miniboss config
        private void InitMinibossesConfig() {
            string sectionName = "3. Minibosses config";

            configHolder.AffectMinibosses = Config.Bind(
                sectionName,
                "minibosses affected",
                true,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_affect_minibosses_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_affect_minibosses_name"),
                        Order = 0,
                    }));
            configHolder.AffectMinibosses.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.AffectMinibosses = configHolder.AffectMinibosses.Value;
            };

            configHolder.MaxMinibossCycles = Config.Bind(
                sectionName,
                "minibosses deaths",
                -1,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_miniboss_deaths_number_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_miniboss_deaths_number_name"),
                        Order = 1,
                    }));
            configHolder.MaxMinibossCycles.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MaxMinibossCycles = configHolder.MaxMinibossCycles.Value;
            };

            SetupMinibossSpeedScaling(sectionName);
            SetupMinibossModifiersScaling(sectionName);
            SetupMinibossRandomSpeedScaling(sectionName);
            SetupMinibossRandomModifiersScaling(sectionName);
        }

        private void SetupMinibossSpeedScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.MinibossesMinSpeedScalingValue = Config.Bind(
                sectionName,
                "miniboss speed scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesMinSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesMinSpeedScalingValue = configHolder.MinibossesMinSpeedScalingValue.Value;
            };

            configHolder.MinibossesSpeedScalingStepValue = Config.Bind(
                sectionName,
                "miniboss speed scaling step value",
                0.06f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesSpeedScalingStepValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesSpeedScalingStepValue = configHolder.MinibossesSpeedScalingStepValue.Value;
            };

            configHolder.MinibossesSpeedStepsCapValue = Config.Bind(
                sectionName,
                "miniboss speed scaling steps cap",
                5,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesSpeedStepsCapValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesSpeedStepsCapValue = configHolder.MinibossesSpeedStepsCapValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_miniboss_speed_scaling_minspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_speed_scaling_minspeed_desc"),
                configHolder.MinibossesMinSpeedScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesMinSpeedScalingValue.Value = arg;
            });

            var stepValueField = new FloatField(
                LocalizationResolver.Localize("config_miniboss_speed_scaling_step_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_speed_scaling_step_desc"),
                configHolder.MinibossesSpeedScalingStepValue.Value);
            stepValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesSpeedScalingStepValue.Value = arg;
            });

            var stepsCapField = new IntField(
                LocalizationResolver.Localize("config_miniboss_speed_scaling_steps_cap_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_speed_scaling_steps_cap_desc"),
                configHolder.MinibossesSpeedStepsCapValue.Value);
            stepsCapField.AddValueChangeHandler(arg => {
                configHolder.MinibossesSpeedStepsCapValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, stepValueField);
            tabsDrawer.AddField(1, stepsCapField);

            configHolder.MinibossesIsSpeedScalingEnabled = Config.Bind(
                sectionName,
                "miniboss speed scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_miniboss_speed_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_miniboss_speed_scaling_enabled_name"),
                        Order = 2
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.MinibossesIsSpeedScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.MinibossesIsSpeedScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.MinibossesIsSpeedScalingEnabled.Value = false;
            });
        }

        private void SetupMinibossModifiersScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.MinibossesMinModifiersNumber = Config.Bind(
                sectionName,
                "miniboss modifiers scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesMinModifiersNumber.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesMinModifiersNumber = configHolder.MinibossesMinModifiersNumber.Value;
            };

            configHolder.MinibossesModifiersScalingStepValue = Config.Bind(
                sectionName,
                "miniboss modifiers scaling step value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesSpeedScalingStepValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesModifiersScalingStepValue = configHolder.MinibossesModifiersScalingStepValue.Value;
            };

            configHolder.MinibossesModifiersStepsCapValue = Config.Bind(
                sectionName,
                "miniboss modifiers scaling steps cap",
                2,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesSpeedStepsCapValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesModifiersStepsCapValue = configHolder.MinibossesModifiersStepsCapValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_miniboss_modifiers_scaling_min_number_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_modifiers_scaling_min_number_desc"),
                configHolder.MinibossesMinModifiersNumber.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesMinModifiersNumber.Value = arg;
            });

            var stepValueField = new FloatField(
                LocalizationResolver.Localize("config_miniboss_modifiers_scaling_step_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_modifiers_scaling_step_desc"),
                configHolder.MinibossesModifiersScalingStepValue.Value);
            stepValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesModifiersScalingStepValue.Value = arg;
            });

            var stepsCapField = new IntField(
                LocalizationResolver.Localize("config_miniboss_modifiers_scaling_steps_cap_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_modifiers_scaling_steps_cap_desc"),
                configHolder.MinibossesModifiersStepsCapValue.Value);
            stepsCapField.AddValueChangeHandler(arg => {
                configHolder.MinibossesModifiersStepsCapValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, stepValueField);
            tabsDrawer.AddField(1, stepsCapField);

            configHolder.MinibossesIsModifiersScalingEnabled = Config.Bind(
                sectionName,
                "miniboss modifiers scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_miniboss_modifiers_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_miniboss_modifiers_scaling_enabled_name"),
                        Order = 3
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.MinibossesIsModifiersScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.MinibossesIsModifiersScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.MinibossesIsModifiersScalingEnabled.Value = false;
            });
        }

        private void SetupMinibossRandomSpeedScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.MinibossesMinRandomSpeedScalingValue = Config.Bind(
                sectionName,
                "miniboss random speed scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesMinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesMinRandomSpeedScalingValue = configHolder.MinibossesMinRandomSpeedScalingValue.Value;
            };

            configHolder.MinibossesMaxRandomSpeedScalingValue = Config.Bind(
                sectionName,
                "miniboss random speed scaling max value",
                1.5f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesMaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesMaxRandomSpeedScalingValue = configHolder.MinibossesMaxRandomSpeedScalingValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_miniboss_rand_speed_scaling_minspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_rand_speed_scaling_minspeed_desc"),
                configHolder.MinibossesMinRandomSpeedScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesMinRandomSpeedScalingValue.Value = arg;
            });

            var maxValueField = new FloatField(
                LocalizationResolver.Localize("config_miniboss_rand_speed_scaling_maxspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_rand_speed_scaling_maxspeed_desc"),
                configHolder.MinibossesMaxRandomSpeedScalingValue.Value);
            maxValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesMaxRandomSpeedScalingValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, maxValueField);

            configHolder.MinibossesIsRandomSpeedScalingEnabled = Config.Bind(
                sectionName,
                "miniboss random speed scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_miniboss_rand_speed_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_miniboss_rand_speed_scaling_enabled_name"),
                        Order = 4
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.MinibossesIsRandomSpeedScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.MinibossesIsRandomSpeedScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.MinibossesIsRandomSpeedScalingEnabled.Value = false;
            });
        }

        private void SetupMinibossRandomModifiersScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.MinibossesMinRandomModifiersScalingValue = Config.Bind(
                sectionName,
                "miniboss random modifiers scaling min value",
                1,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesMinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesMinRandomModifiersScalingValue = configHolder.MinibossesMinRandomModifiersScalingValue.Value;
            };

            configHolder.MinibossesMaxRandomModifiersScalingValue = Config.Bind(
                sectionName,
                "miniboss random modifiers scaling max value",
                3,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.MinibossesMaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MinibossesMaxRandomModifiersScalingValue = configHolder.MinibossesMaxRandomModifiersScalingValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new IntField(
                LocalizationResolver.Localize("config_miniboss_rand_modifiers_scaling_min_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_rand_modifiers_scaling_min_desc"),
                configHolder.MinibossesMinRandomModifiersScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesMinRandomModifiersScalingValue.Value = arg;
            });

            var maxValueField = new IntField(
                LocalizationResolver.Localize("config_miniboss_rand_modifiers_scaling_max_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_miniboss_rand_modifiers_scaling_max_desc"),
                configHolder.MinibossesMaxRandomModifiersScalingValue.Value);
            maxValueField.AddValueChangeHandler(arg => {
                configHolder.MinibossesMaxRandomModifiersScalingValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, maxValueField);

            configHolder.MinibossesIsRandomModifiersScalingEnabled = Config.Bind(
                sectionName,
                "miniboss random modifiers scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_miniboss_rand_modifiers_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_miniboss_rand_modifiers_scaling_enabled_name"),
                        Order = 5
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.MinibossesIsRandomModifiersScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.MinibossesIsRandomModifiersScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.MinibossesIsRandomModifiersScalingEnabled.Value = false;
            });
        }
        #endregion Miniboss config

        #region Enemy config
        private void InitEnemiesConfig() {
            string sectionName = "3. Enemies config";

            configHolder.AffectEnemies = Config.Bind(
                sectionName,
                "enemies affected",
                true,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_affect_enemies_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_affect_enemies_name"),
                        Order = 0,
                    }));
            configHolder.AffectEnemies.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.AffectEnemies = configHolder.AffectEnemies.Value;
            };

            configHolder.MaxEnemyCycles = Config.Bind(
                sectionName,
                "enemies deaths",
                -1,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_enemy_deaths_number_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_enemy_deaths_number_name"),
                        Order = 1,
                    }));
            configHolder.MaxEnemyCycles.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.MaxEnemyCycles = configHolder.MaxEnemyCycles.Value;
            };

            SetupEnemySpeedScaling(sectionName);
            SetupEnemyModifiersScaling(sectionName);
            SetupEnemyRandomSpeedScaling(sectionName);
            SetupEnemyRandomModifiersScaling(sectionName);
        }

        private void SetupEnemySpeedScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.EnemiesMinSpeedScalingValue = Config.Bind(
                sectionName,
                "enemy speed scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesMinSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesMinSpeedScalingValue = configHolder.EnemiesMinSpeedScalingValue.Value;
            };

            configHolder.EnemiesSpeedScalingStepValue = Config.Bind(
                sectionName,
                "enemy speed scaling step value",
                0.06f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesSpeedScalingStepValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesSpeedScalingStepValue = configHolder.EnemiesSpeedScalingStepValue.Value;
            };

            configHolder.EnemiesSpeedStepsCapValue = Config.Bind(
                sectionName,
                "enemy speed scaling steps cap",
                5,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesSpeedStepsCapValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesSpeedStepsCapValue = configHolder.EnemiesSpeedStepsCapValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_enemy_speed_scaling_minspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_speed_scaling_minspeed_desc"),
                configHolder.EnemiesMinSpeedScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesMinSpeedScalingValue.Value = arg;
            });

            var stepValueField = new FloatField(
                LocalizationResolver.Localize("config_enemy_speed_scaling_step_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_speed_scaling_step_desc"),
                configHolder.EnemiesSpeedScalingStepValue.Value);
            stepValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesSpeedScalingStepValue.Value = arg;
            });

            var stepsCapField = new IntField(
                LocalizationResolver.Localize("config_enemy_speed_scaling_steps_cap_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_speed_scaling_steps_cap_desc"),
                configHolder.EnemiesSpeedStepsCapValue.Value);
            stepsCapField.AddValueChangeHandler(arg => {
                configHolder.EnemiesSpeedStepsCapValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, stepValueField);
            tabsDrawer.AddField(1, stepsCapField);

            configHolder.EnemiesIsSpeedScalingEnabled = Config.Bind(
                sectionName,
                "enemy speed scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_enemy_speed_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_enemy_speed_scaling_enabled_name"),
                        Order = 2
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.EnemiesIsSpeedScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.EnemiesIsSpeedScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.EnemiesIsSpeedScalingEnabled.Value = false;
            });
        }

        private void SetupEnemyModifiersScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.EnemiesMinModifiersNumber = Config.Bind(
                sectionName,
                "enemy modifiers scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesMinModifiersNumber.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesMinModifiersNumber = configHolder.EnemiesMinModifiersNumber.Value;
            };

            configHolder.EnemiesModifiersScalingStepValue = Config.Bind(
                sectionName,
                "enemy modifiers scaling step value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesSpeedScalingStepValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesModifiersScalingStepValue = configHolder.EnemiesModifiersScalingStepValue.Value;
            };

            configHolder.EnemiesModifiersStepsCapValue = Config.Bind(
                sectionName,
                "enemy modifiers scaling steps cap",
                2,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesSpeedStepsCapValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesModifiersStepsCapValue = configHolder.EnemiesModifiersStepsCapValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_enemy_modifiers_scaling_min_number_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_modifiers_scaling_min_number_desc"),
                configHolder.EnemiesMinModifiersNumber.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesMinModifiersNumber.Value = arg;
            });

            var stepValueField = new FloatField(
                LocalizationResolver.Localize("config_enemy_modifiers_scaling_step_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_modifiers_scaling_step_desc"),
                configHolder.EnemiesModifiersScalingStepValue.Value);
            stepValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesModifiersScalingStepValue.Value = arg;
            });

            var stepsCapField = new IntField(
                LocalizationResolver.Localize("config_enemy_modifiers_scaling_steps_cap_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_modifiers_scaling_steps_cap_desc"),
                configHolder.EnemiesModifiersStepsCapValue.Value);
            stepsCapField.AddValueChangeHandler(arg => {
                configHolder.EnemiesModifiersStepsCapValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, stepValueField);
            tabsDrawer.AddField(1, stepsCapField);

            configHolder.EnemiesIsModifiersScalingEnabled = Config.Bind(
                sectionName,
                "enemy modifiers scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_enemy_modifiers_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_enemy_modifiers_scaling_enabled_name"),
                        Order = 3
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.EnemiesIsModifiersScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.EnemiesIsModifiersScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.EnemiesIsModifiersScalingEnabled.Value = false;
            });
        }

        private void SetupEnemyRandomSpeedScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.EnemiesMinRandomSpeedScalingValue = Config.Bind(
                sectionName,
                "enemy random speed scaling min value",
                1f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesMinRandomSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesMinRandomSpeedScalingValue = configHolder.EnemiesMinRandomSpeedScalingValue.Value;
            };

            configHolder.EnemiesMaxRandomSpeedScalingValue = Config.Bind(
                sectionName,
                "enemy random speed scaling max value",
                1.5f,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesMaxRandomSpeedScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesMaxRandomSpeedScalingValue = configHolder.EnemiesMaxRandomSpeedScalingValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new FloatField(
                LocalizationResolver.Localize("config_enemy_rand_speed_scaling_minspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_rand_speed_scaling_minspeed_desc"),
                configHolder.EnemiesMinRandomSpeedScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesMinRandomSpeedScalingValue.Value = arg;
            });

            var maxValueField = new FloatField(
                LocalizationResolver.Localize("config_enemy_rand_speed_scaling_maxspeed_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_rand_speed_scaling_maxspeed_desc"),
                configHolder.EnemiesMaxRandomSpeedScalingValue.Value);
            maxValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesMaxRandomSpeedScalingValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, maxValueField);

            configHolder.EnemiesIsRandomSpeedScalingEnabled = Config.Bind(
                sectionName,
                "enemy random speed scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_enemy_rand_speed_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_enemy_rand_speed_scaling_enabled_name"),
                        Order = 4
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.EnemiesIsRandomSpeedScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.EnemiesIsRandomSpeedScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.EnemiesIsRandomSpeedScalingEnabled.Value = false;
            });
        }

        private void SetupEnemyRandomModifiersScaling(string sectionName) {
            string enabledText = LocalizationResolver.Localize("label_enabled");
            string disabledText = LocalizationResolver.Localize("label_disabled");

            configHolder.EnemiesMinRandomModifiersScalingValue = Config.Bind(
                sectionName,
                "enemy random modifiers scaling min value",
                1,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesMinRandomModifiersScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesMinRandomModifiersScalingValue = configHolder.EnemiesMinRandomModifiersScalingValue.Value;
            };

            configHolder.EnemiesMaxRandomModifiersScalingValue = Config.Bind(
                sectionName,
                "enemy random modifiers scaling max value",
                3,
                new ConfigDescription(
                    "",
                    null,
                    new ConfigurationManagerAttributes {
                        Browsable = false,
                    }));
            configHolder.EnemiesMaxRandomModifiersScalingValue.SettingChanged += (_, _) => {
                ChallengeConfigurationManager.ChallengeConfiguration.EnemiesMaxRandomModifiersScalingValue = configHolder.EnemiesMaxRandomModifiersScalingValue.Value;
            };


            var tabsDrawer = new ConfigTabsDrawer();
            tabsDrawer.Tabs = [disabledText, enabledText];

            var minValueField = new IntField(
                LocalizationResolver.Localize("config_enemy_rand_modifiers_scaling_min_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_rand_modifiers_scaling_min_desc"),
                configHolder.EnemiesMinRandomModifiersScalingValue.Value);
            minValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesMinRandomModifiersScalingValue.Value = arg;
            });

            var maxValueField = new IntField(
                LocalizationResolver.Localize("config_enemy_rand_modifiers_scaling_max_name"),
                CalculateRightColumnWidth,
                LocalizationResolver.Localize("config_enemy_rand_modifiers_scaling_max_desc"),
                configHolder.EnemiesMaxRandomModifiersScalingValue.Value);
            maxValueField.AddValueChangeHandler(arg => {
                configHolder.EnemiesMaxRandomModifiersScalingValue.Value = arg;
            });

            tabsDrawer.AddField(1, minValueField);
            tabsDrawer.AddField(1, maxValueField);

            configHolder.EnemiesIsRandomModifiersScalingEnabled = Config.Bind(
                sectionName,
                "enemy random modifiers scaling enabled",
                false,
                new ConfigDescription(
                    LocalizationResolver.Localize("config_enemy_rand_modifiers_scaling_enabled_desc"),
                    null,
                    new ConfigurationManagerAttributes {
                        DispName = LocalizationResolver.Localize("config_enemy_rand_modifiers_scaling_enabled_name"),
                        Order = 5
                    },
                    tabsDrawer.GetConfigAttributes()
                ));
            tabsDrawer.SelectedTab = configHolder.EnemiesIsRandomModifiersScalingEnabled.Value ? 1 : 0;
            tabsDrawer.OnTabSelectedHandlers.TryAdd(enabledText, () => {
                configHolder.EnemiesIsRandomModifiersScalingEnabled.Value = true;
            });
            tabsDrawer.OnTabSelectedHandlers.TryAdd(disabledText, () => {
                configHolder.EnemiesIsRandomModifiersScalingEnabled.Value = false;
            });
        }

        #endregion Enemy config

        public void HandleChallengeConfigurationValues() {
            var config = ChallengeConfigurationManager.ChallengeConfiguration;

            config.IsEnabledInMoB = configHolder.IsMoBEnabled.Value;
            config.IsEnabledInNormal = configHolder.IsNormalEnabled.Value;

            config.IsModifiersEnabled = configHolder.IsModifiersEnabled.Value;
            config.ModifiersStartDeath = configHolder.ModifiersStartDeathValue.Value;
            config.IsRepeatingEnabled = configHolder.IsModifiersRepeatingEnabled.Value;
            
            config.IsSpeedModifierEnabled = configHolder.IsSpeedModifierEnabled.Value;
            config.IsTimerModifierEnabled = configHolder.IsTimerModifierEnabled.Value;
            config.IsParryDamageModifierEnabled = configHolder.IsParryDamageModifierEnabled.Value;
            config.IsDamageBuildupModifierEnabled = configHolder.IsDamageBuildupModifierEnabled.Value;
            config.IsRegenerationModifierEnabled = configHolder.IsRegenerationModifierEnabled.Value;
            config.IsKnockbackModifierEnabled = configHolder.IsKnockbackModifierEnabled.Value;
            config.IsRandomArrowModifierEnabled = configHolder.IsRandomArrowModifierEnabled.Value;
            config.IsRandomTalismanModifierEnabled = configHolder.IsRandomTalismanModifierEnabled.Value;
            config.IsEnduranceModifierEnabled = configHolder.IsEnduranceModifierEnabled.Value;
            config.IsQiShieldModifierEnabled = configHolder.IsQiShieldModifierEnabled.Value;
            config.IsCooldownShieldModifierEnabled = configHolder.IsCooldownShieldModifierEnabled.Value;
            config.IsQiOverloadModifierEnabled = configHolder.IsQiOverloadModifierEnabled.Value;
            config.IsDistanceShieldModifierEnabled = configHolder.IsDistanceShieldModifierEnabled.Value;
            config.IsYanlaoGunModifierEnabled = configHolder.IsYanlaoGunModifierEnabled.Value;
            config.IsQiBombModifierEnabled = configHolder.IsQiBombModifierEnabled.Value;
            config.IsShieldBreakBombModifierEnabled = configHolder.IsShieldBreakBombModifierEnabled.Value;
            config.IsQiOverloadBombModifierEnabled = configHolder.IsQiOverloadBombModifierEnabled.Value;
            config.IsQiDepletionBombModifierEnabled = configHolder.IsQiDepletionBombModifierEnabled.Value;
            config.IsCooldownBombModifierEnabled = configHolder.IsCooldownBombModifierEnabled.Value;

            #region Bosses
            config.AffectBosses = configHolder.AffectBosses.Value;
            config.MaxBossCycles = configHolder.MaxBossCycles.Value;

            config.BossesIsSpeedScalingEnabled = configHolder.BossesIsSpeedScalingEnabled.Value;
            config.BossesMinSpeedScalingValue = configHolder.BossesMinSpeedScalingValue.Value;
            config.BossesSpeedScalingStepValue = configHolder.BossesSpeedScalingStepValue.Value;
            config.BossesSpeedStepsCapValue = configHolder.BossesSpeedStepsCapValue.Value;

            config.BossesIsModifiersScalingEnabled = configHolder.BossesIsModifiersScalingEnabled.Value;
            config.BossesMinModifiersNumber = configHolder.BossesMinModifiersNumber.Value;
            config.BossesModifiersScalingStepValue = configHolder.BossesModifiersScalingStepValue.Value;
            config.BossesModifiersStepsCapValue = configHolder.BossesModifiersStepsCapValue.Value;

            config.BossesIsRandomSpeedScalingEnabled = configHolder.BossesIsRandomSpeedScalingEnabled.Value;
            config.BossesMinRandomSpeedScalingValue = configHolder.BossesMinRandomSpeedScalingValue.Value;
            config.BossesMaxRandomSpeedScalingValue = configHolder.BossesMaxRandomSpeedScalingValue.Value;
            
            config.BossesIsRandomModifiersScalingEnabled = configHolder.BossesIsRandomModifiersScalingEnabled.Value;
            config.BossesMinRandomModifiersScalingValue = configHolder.BossesMinRandomModifiersScalingValue.Value;
            config.BossesMaxRandomModifiersScalingValue = configHolder.BossesMaxRandomModifiersScalingValue.Value;
            #endregion Bosses

            #region Minibosses
            config.AffectMinibosses = configHolder.AffectMinibosses.Value;
            config.MaxMinibossCycles = configHolder.MaxMinibossCycles.Value;

            config.MinibossesIsSpeedScalingEnabled = configHolder.MinibossesIsSpeedScalingEnabled.Value;
            config.MinibossesMinSpeedScalingValue = configHolder.MinibossesMinSpeedScalingValue.Value;
            config.MinibossesSpeedScalingStepValue = configHolder.MinibossesSpeedScalingStepValue.Value;
            config.MinibossesSpeedStepsCapValue = configHolder.MinibossesSpeedStepsCapValue.Value;

            config.MinibossesIsModifiersScalingEnabled = configHolder.MinibossesIsModifiersScalingEnabled.Value;
            config.MinibossesMinModifiersNumber = configHolder.MinibossesMinModifiersNumber.Value;
            config.MinibossesModifiersScalingStepValue = configHolder.MinibossesModifiersScalingStepValue.Value;
            config.MinibossesModifiersStepsCapValue = configHolder.MinibossesModifiersStepsCapValue.Value;

            config.MinibossesIsRandomSpeedScalingEnabled = configHolder.MinibossesIsRandomSpeedScalingEnabled.Value;
            config.MinibossesMinRandomSpeedScalingValue = configHolder.MinibossesMinRandomSpeedScalingValue.Value;
            config.MinibossesMaxRandomSpeedScalingValue = configHolder.MinibossesMaxRandomSpeedScalingValue.Value;

            config.MinibossesIsRandomModifiersScalingEnabled = configHolder.MinibossesIsRandomModifiersScalingEnabled.Value;
            config.MinibossesMinRandomModifiersScalingValue = configHolder.MinibossesMinRandomModifiersScalingValue.Value;
            config.MinibossesMaxRandomModifiersScalingValue = configHolder.MinibossesMaxRandomModifiersScalingValue.Value;
            #endregion Minibosses

            #region Enemies
            config.AffectEnemies = configHolder.AffectEnemies.Value;
            config.MaxEnemyCycles = configHolder.MaxEnemyCycles.Value;

            config.EnemiesIsSpeedScalingEnabled = configHolder.EnemiesIsSpeedScalingEnabled.Value;
            config.EnemiesMinSpeedScalingValue = configHolder.EnemiesMinSpeedScalingValue.Value;
            config.EnemiesSpeedScalingStepValue = configHolder.EnemiesSpeedScalingStepValue.Value;
            config.EnemiesSpeedStepsCapValue = configHolder.EnemiesSpeedStepsCapValue.Value;

            config.EnemiesIsModifiersScalingEnabled = configHolder.EnemiesIsModifiersScalingEnabled.Value;
            config.EnemiesMinModifiersNumber = configHolder.EnemiesMinModifiersNumber.Value;
            config.EnemiesModifiersScalingStepValue = configHolder.EnemiesModifiersScalingStepValue.Value;
            config.EnemiesModifiersStepsCapValue = configHolder.EnemiesModifiersStepsCapValue.Value;

            config.EnemiesIsRandomSpeedScalingEnabled = configHolder.EnemiesIsRandomSpeedScalingEnabled.Value;
            config.EnemiesMinRandomSpeedScalingValue = configHolder.EnemiesMinRandomSpeedScalingValue.Value;
            config.EnemiesMaxRandomSpeedScalingValue = configHolder.EnemiesMaxRandomSpeedScalingValue.Value;

            config.EnemiesIsRandomModifiersScalingEnabled = configHolder.EnemiesIsRandomModifiersScalingEnabled.Value;
            config.EnemiesMinRandomModifiersScalingValue = configHolder.EnemiesMinRandomModifiersScalingValue.Value;
            config.EnemiesMaxRandomModifiersScalingValue = configHolder.EnemiesMaxRandomModifiersScalingValue.Value;
            #endregion Enemies
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
