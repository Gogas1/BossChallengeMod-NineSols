using BepInEx;
using HarmonyLib;
using I2.Loc;
using NineSolsAPI;
using BossChallengeMod.Configuration;
using BossChallengeMod.Configuration.Repositories;
using BossChallengeMod.Global;
using BossChallengeMod.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using BossChallengeMod.ObjectProviders;
using BossChallengeMod.Preloading;
using BossChallengeMod.PatchResolver;
using BossChallengeMod.PatchResolver.Initializers;
using BossChallengeMod.Modifiers;
using System.Collections.Generic;
using BossChallengeMod.Modifiers.Managers;
using NineSolsAPI.Utils;

namespace BossChallengeMod;

[BepInDependency(NineSolsAPICore.PluginGUID)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class BossChallengeMod : BaseUnityPlugin {
    public CustomMonsterStateValuesResolver MonsterStateValuesResolver { get; private set; }
    public EventTypesResolver EventTypesResolver { get; private set; }

    public MonsterPatchResolver RegularMonstersPatchResolver { get; private set; } = null!;
    public MonsterPatchResolver BossesPatchResolver { get; private set; } = null!;

    public UIController UIController { get; private set; } = null!;
    public MonsterUIController MonsterUIController { get; private set; } = null!;
    public ChallengeConfigurationManager ChallengeConfigurationManager { get; private set; } = null!;
    public GlobalModifiersController GlobalModifiersFlags { get; private set; } = null!;
    public UIConfiguration UIConfiguration { get; private set; } = null!;

    private Preloader Preloader { get; set; } = null!;
    private BepInExModConfigurationHandler BepInExModConfigurationHandler { get; set; } = null!;

    public ShieldProvider ShieldProvider { get; private set; } = null!;
    public YanlaoGunProvider YanlaoGunProvider { get; private set; } = null!;
    public BombProvider BombShooterProvider { get; private set; } = null!;

    public static ModifiersStore Modifiers { get; private set; } = null!;

    public static BossChallengeMod Instance { get; private set; } = null!;
    public static System.Random Random { get; private set; } = null!;

    public const string PluginGUID = PluginInfo.PLUGIN_GUID;

    private int versionNotificationCounter = 3;
    private bool isToastsDisplayed;
    private bool UnloadRequested;

    private Harmony harmony = null!;

    private ModConfig _modConfig = null!;
    private bool isVersionValid = false;
    private bool isLatestPatch = false;

    protected ChallengeConfiguration ConfigurationToUse {
        get {
            return ChallengeConfigurationManager.ChallengeConfiguration;
        }
    }

    public BossChallengeMod() {
        MonsterStateValuesResolver = new CustomMonsterStateValuesResolver();
        EventTypesResolver = new EventTypesResolver();
    }

    private void Awake() {
        Instance = this;

        Log.Init(Logger);

        RCGLifeCycle.DontDestroyForever(gameObject);


        Random = new System.Random();
        Preloader = new Preloader();
        ShieldProvider = new ShieldProvider();
        YanlaoGunProvider = new YanlaoGunProvider();
        BombShooterProvider = new BombProvider();

        Modifiers = new ModifiersStore();

        AssignPreloadingTargets();
        SetupModifiers(Modifiers);

        LocalizationResolver.LoadLanguage(GetLanguageCode());

        IRecordsRepository recordsRepo = new JsonRecordsRepository();
        ChallengeConfigurationManager = new ChallengeConfigurationManager(recordsRepo);
        UIConfiguration = new UIConfiguration();
        GlobalModifiersFlags = new GlobalModifiersController();

        LocalizationManager.OnLocalizeEvent += OnLocalizationChange;
        
        BepInExModConfigurationHandler = new BepInExModConfigurationHandler(Config, ChallengeConfigurationManager, UIConfiguration);

        BepInExModConfigurationHandler.InitChallengeConfiguration();
        BepInExModConfigurationHandler.HandleChallengeConfigurationValues();

        BepInExModConfigurationHandler.InitializeUIConfiguration();
        UIController = new UIController(UIConfiguration);
        BepInExModConfigurationHandler.HandleUIConfigurationValues();
        
        MonsterUIController = new MonsterUIController();

        RegularMonstersPatchResolver = new RegularEnemiesPatchesInitializer(MonsterStateValuesResolver).MonsterPatchResolver;
        BossesPatchResolver = new BossesPatchesInitializer(MonsterStateValuesResolver).MonsterPatchResolver;

        SceneManager.sceneLoaded += OnSceneLoaded;

        harmony = Harmony.CreateAndPatchAll(typeof(BossChallengeMod).Assembly);
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded1!");
    }

    private void Start() {
        ProcessModConfig();

        if(isVersionValid) {

            if(isLatestPatch) {
                Preloader.PreloadLatest();
            }
            else {
                StartCoroutine(Preloader.PreloadOld());
            }
        }
    }

    private void ProcessModConfig() {
        _modConfig = AssemblyUtils.GetEmbeddedJson<ModConfig>($"BossChallengeMod.Resources.ModConfig.modconfig.json")!;

        if(_modConfig == null) {
            Log.Error("Mod config not found!");
            return;
        }

        var gameVer = ConfigManager.Instance.Version.Trim();

        if(gameVer.Contains(_modConfig?.LatestVersion)) {
            isLatestPatch = true;
            isVersionValid = true;
        } else if(gameVer.Contains(_modConfig.StableVersion)) {
            isVersionValid = true;
        }
    }

    private void AssignPreloadingTargets() {
        string shieldScene = "A5_S5_JieChuanHall";
        string shieldName = "Shield(Effect Receiver)_Shield Sphere Version";

        Preloader.AddPreload(shieldScene, shieldName, ShieldProvider);

        string gunScene = "A4_S4_Container_Final";
        string gunName = "ZGun FSM Object Variant";

        Preloader.AddPreload(gunScene, gunName, YanlaoGunProvider);

        string bomdShooterScene = "A1_S3_InnerHumanDisposal_Final";
        string bombShooterName = "Shooter_GrenadeUp";

        Preloader.AddPreload(bomdShooterScene, bombShooterName, BombShooterProvider);
    }

    private string GetLanguageCode() {
        string language = LocalizationManager.CurrentLanguageCode;

        switch (language) {
            case "ru":
                return "ru-ru";
            case "en-US":
                return "en-us";
            case "zh-CN":
                return "zh-cn";
            case "zh-TW":
                return "zh-tw";
            default:
                return "en-us";
        }
    }

    private void OnLocalizationChange() {
        LocalizationResolver.LoadLanguage(GetLanguageCode());
        //InitializeChallengeConfiguration();
    }

    private void OnDestroy() {
        UnloadRequested = true;
        UIController.Unload();
        LocalizationManager.OnLocalizeEvent -= OnLocalizationChange;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        harmony.UnpatchSelf();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        UIController.FixUI();
        GlobalModifiersFlags.ValidateAll();
        if (!isToastsDisplayed) {
            isToastsDisplayed = true;
            StartCoroutine(ShowToasts());
        }

        if(versionNotificationCounter > 0 && !isVersionValid) {
            if(SingletonBehaviour<GameCore>.IsAvailable()) {
                versionNotificationCounter--;
                Log.Warning($"Current version is {ConfigManager.Instance.Version.Trim()}");
                SingletonBehaviour<GameCore>.Instance.notificationUI.ShowNotification($"Boss Challenge mod version is built for different game versions: {_modConfig.StableVersion.Substring(0, 16)} or {_modConfig.LatestVersion.Substring(0, 16)}. Some features were disabled.", null, PlayerInfoPanelType.Undefined, null);
            }
        }
    }

    private IEnumerator ShowToasts() {
        yield return new WaitForSeconds(5);
        for (int i = 1; i < 5 && !UnloadRequested; i++) {
            ToastManager.Toast(LocalizationResolver.Localize($"toast_message{i}"));
            yield return new WaitForSeconds(4);
        }
    }

    private void SetupModifiers(ModifiersStore modifiersStore) {        

        modifiersStore
            .CreateModifierBuilder<SpeedModifier>("speed_temp", "SpeedModifier")
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsSpeedModifierEnabled)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<TimerModifier>("timer", "TimerModifier")
            .AddConditionPredicate(_ => ApplicationCore.IsInBossMemoryMode)
            .AddCanBeRolledConditionPredicate((_, iteration) => iteration > 0 && ConfigurationToUse.IsTimerModifierEnabled)
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                ])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<ScalingSpeedModifier>("speed_perm", "SpeedScalingModifier")
            .SetPersistance(true)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<ParryDirectDamageModifier>("parry_damage", "ParryDamageModifier")
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsParryDamageModifierEnabled)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<DamageBuildupModifier>("damage_buildup", "DamageBuildupModifier")
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsDamageBuildupModifierEnabled)
            .AddIncompatibles(["parry_damage", "qi_overload"])
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<RegenerationModifier>("regeneration", "RegenerationModifier")
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsRegenerationModifierEnabled)
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<KnockbackModifier>("knockback", "KnockbackModifier")
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsKnockbackModifierEnabled)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<RandomArrowModifier>("random_arrow", "RandomArrowModifier")
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsRandomArrowModifierEnabled && Player.i.mainAbilities.ArrowAbility.IsActivated)
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .BuildAndAdd();

        var randomTalismanCondition = (bool isEnabled) => {
            var player = Player.i;

            bool blastIsActive = (player.mainAbilities.FooExplodeAllStyle.AbilityData.IsAcquired || player.mainAbilities.FooExplodeAllStyleUpgrade.AbilityData.IsAcquired);
            bool flowIsActive = (player.mainAbilities.FooExplodeAutoStyle.AbilityData.IsAcquired || player.mainAbilities.FooExplodeAutoStyleUpgrade.AbilityData.IsAcquired);
            bool fctIsActive = (player.mainAbilities.FooExplodeConsecutiveStyle.AbilityData.IsAcquired || player.mainAbilities.FooExplodeConsecutiveStyleUpgrade.AbilityData.IsAcquired);

            return !((blastIsActive ^ flowIsActive ^ fctIsActive) && !(blastIsActive && fctIsActive && fctIsActive)) && isEnabled;
        };

        modifiersStore
            .CreateModifierBuilder<RandomTaliModifier>("random_talisman", "RandomTalismanModifier")
            .AddCanBeRolledConditionPredicate((_,_) => randomTalismanCondition(ConfigurationToUse.IsRandomTalismanModifierEnabled))
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<EnduranceModifier>("endurance", "EnduranceModifier")
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant",
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                ])
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsEnduranceModifierEnabled)
            .BuildAndAdd();

        var shieldsCondition = (bool isEnabled) => {
            return Player.i.mainAbilities.ChargedAttackAbility.IsActivated && isEnabled && isVersionValid;
        };

        modifiersStore
            .CreateModifierBuilder<QiShieldModifier>("qi_shield", "QiShieldModifer")
            .AddCanBeRolledConditionPredicate((_,_) => shieldsCondition(ConfigurationToUse.IsQiShieldModifierEnabled))
            .AddIncompatibles(["timer_shield", "distance_shield"])
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant",
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_SpearHorseMan",
                "Monster_GiantMechClaw",
                ])
            .AddCombinationModifiers(["shield_break_bomb"])
            .AddController(typeof(MonsterShieldController), true)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<TimedShieldModifier>("timer_shield", "CooldownShieldModifier")
            .AddCanBeRolledConditionPredicate((_,_) => shieldsCondition(ConfigurationToUse.IsCooldownShieldModifierEnabled))
            .AddIncompatibles(["qi_shield", "distance_shield"])
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant",
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_SpearHorseMan",
                "Monster_GiantMechClaw",
                ])
            .AddCombinationModifiers(["shield_break_bomb"])
            .AddController(typeof(MonsterShieldController), true)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<DistanceShieldModifier>("distance_shield", "DistanceShieldModifier")
            .AddCanBeRolledConditionPredicate((_,_) => shieldsCondition(ConfigurationToUse.IsDistanceShieldModifierEnabled))
            .AddIncompatibles(["timer_shield", "qi_shield"])
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant",
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_伏羲_新",
                "StealthGameMonster_SpearHorseMan",
                "Monster_GiantMechClaw",
                ])
            .AddCombinationModifiers(["shield_break_bomb"])
            .AddController(typeof(MonsterShieldController), true)
            .BuildAndAdd();        

        modifiersStore
            .CreateModifierBuilder<QiOverloadModifier>("qi_overload", "QiOverloadModifier")
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsQiOverloadModifierEnabled)
            .AddIncompatibles(["damage_buildup"])
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<YanlaoGunModifier>("ya_gun", "YanlaoGunModifier")
            .AddIncompatibles(["shield_break_bomb", "qi_bomb", "qi_overload_bomb", "qi_depletion_bomb", "cooldown_bomb"])
            .AddCanBeRolledConditionPredicate((_,_) => ConfigurationToUse.IsYanlaoGunModifierEnabled && isVersionValid)
            .AddController(typeof(MonsterYanlaoGunController), true)
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .BuildAndAdd();

        var bombCondition = (bool switchValue) => {
            return switchValue && isVersionValid;
        };

        modifiersStore
            .CreateModifierBuilder<QiBombModifier>("qi_bomb", "QiBombModifier")
            .AddCanBeRolledConditionPredicate((_,_) => bombCondition(ConfigurationToUse.IsQiBombModifierEnabled))
            .AddController(typeof(MonsterBombController), true)
            .AddIncompatibles(["ya_gun", "shield_break_bomb", "qi_overload_bomb", "qi_depletion_bomb"])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<ShieldBreakBombModifier>("shield_break_bomb", "ShieldBreakBombModifier")
            .AddCanBeRolledConditionPredicate((_,_) => bombCondition(ConfigurationToUse.IsShieldBreakBombModifierEnabled))
            .AddController(typeof(MonsterBombController), true)
            .SetIsCombination(true)
            .AddIncompatibles(["qi_bomb", "ya_gun", "qi_overload_bomb", "qi_depletion_bomb"])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<QiOverloadBombModifier>("qi_overload_bomb", "QiOverloadBombModifier")
            .AddCanBeRolledConditionPredicate((_,_) => bombCondition(ConfigurationToUse.IsQiOverloadBombModifierEnabled))
            .AddController(typeof(MonsterBombController), true)
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .AddIncompatibles(["ya_gun", "shield_break_bomb", "qi_bomb", "qi_depletion_bomb"])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<QiDepletionBombModifier>("qi_depletion_bomb", "QiDepletionBombModifier")
            .AddCanBeRolledConditionPredicate((_,_) => bombCondition(ConfigurationToUse.IsQiDepletionBombModifierEnabled))
            .AddController(typeof(MonsterBombController), true)
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .AddIncompatibles(["ya_gun", "shield_break_bomb", "qi_bomb", "qi_overload_bomb"])
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<CooldownBombModifier>("cooldown_bomb", "CooldownBomb")
            .AddCanBeRolledConditionPredicate((_,_) => bombCondition(ConfigurationToUse.IsCooldownBombModifierEnabled))
            .AddController(typeof(MonsterBombController), true)
            .AddIncompatibles(["ya_gun", "shield_break_bomb", "qi_bomb", "qi_overload_bomb", "qi_depletion_bomb"])
            .AddIgnoredMonsters([
                "StealthGameMonster_Boss_ButterFly Variant (1)",
                "StealthGameMonster_Boss_ButterFly Variant (2)",
                "StealthGameMonster_Boss_ButterFly Variant (3)",
                "StealthGameMonster_Boss_ButterFly Variant (4)",
                "StealthGameMonster_BossZombieSpear",
                "StealthGameMonster_BossZombieHammer",
                ])
            .BuildAndAdd();
    }
}