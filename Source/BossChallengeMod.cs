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
using static BossChallengeMod.Modifiers.ModifiersStore;
using BossChallengeMod.Modifiers.Managers;

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
    public StoryChallengeConfigurationManager StoryChallengeConfigurationManager { get; private set; } = null!;
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

    private bool isToastsDisplayed;
    private bool UnloadRequested;

    private Harmony harmony = null!;

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
        StoryChallengeConfigurationManager = new StoryChallengeConfigurationManager();
        UIConfiguration = new UIConfiguration();
        GlobalModifiersFlags = new GlobalModifiersController();

        LocalizationManager.OnLocalizeEvent += OnLocalizationChange;
        
        BepInExModConfigurationHandler = new BepInExModConfigurationHandler(Config, ChallengeConfigurationManager, UIConfiguration, StoryChallengeConfigurationManager);

        BepInExModConfigurationHandler.InitChallengeConfiguration();
        BepInExModConfigurationHandler.HandleConfigurationValues();

        BepInExModConfigurationHandler.InitStoryChallengeConfiguration();
        BepInExModConfigurationHandler.HandleStoryChallengeConfigurationValues();

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
        StartCoroutine(Preloader.Preload());
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
        if (!isToastsDisplayed) {
            isToastsDisplayed = true;
            StartCoroutine(ShowToasts());
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
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<TimerModifier>("timer", "TimerModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<ScalingSpeedModifier>("speed_perm", "SpeedScalingModifier")
            .SetPersistance(true)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<ParryDirectDamageModifier>("parry_damage", "ParryDamageModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<DamageBuildupModifier>("damage_buildup", "DamageBuildupModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<RegenerationModifier>("regeneration", "RegenerationModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<KnockbackModifier>("knockback", "KnockbackModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<RandomArrowModifier>("random_arrow", "RandomArrowModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<RandomTaliModifier>("random_talisman", "RandomTalismanModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<EnduranceModifier>("endurance", "EnduranceModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<QiShieldModifier>("qi_shield", "QiShieldModifer")
            .AddIncompatibles(["timer_shield", "distance_shield"])
            .AddController(typeof(MonsterShieldController), true)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<TimedShieldModifier>("timer_shield", "CooldownShieldModifier")
            .AddIncompatibles(["qi_shield", "distance_shield"])
            .AddController(typeof(MonsterShieldController), true)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<DistanceShieldModifier>("distance_shield", "DistanceShieldModifier")
            .AddIncompatibles(["timer_shield", "qi_shield"])
            .AddController(typeof(MonsterShieldController), true)
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<QiOverloadModifier>("qi_overload", "QiOverloadModifier")
            .BuildAndAdd();

        modifiersStore
            .CreateModifierBuilder<YanlaoGunModifier>("ya_gun", "YanlaoGunModifier")
            .AddController(typeof(MonsterYanlaoGunController), true)
            .BuildAndAdd();
    }
}