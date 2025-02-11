using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using I2.Loc;
using NineSolsAPI;
using BossChallengeMod.BossPatches;
using BossChallengeMod.Configuration;
using BossChallengeMod.Configuration.Repositories;
using BossChallengeMod.Global;
using BossChallengeMod.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using BossChallengeMod.ObjectProviders;
using System.IO;
using BossChallengeMod.Preloading;
using BossChallengeMod.BossPatches.TargetPatches;
using BossChallengeMod.PatchResolver;
using BossChallengeMod.PatchResolver.Initializers;

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

        AssignPreloadingTargets();

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
}