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

namespace BossChallengeMod;

[BepInDependency(NineSolsAPICore.PluginGUID)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class BossChallengeMod : BaseUnityPlugin {
    public CustomMonsterStateValuesResolver MonsterStateValuesResolver { get; private set; }
    public EventTypesResolver EventTypesResolver { get; private set; }

    public Dictionary<string, GeneralBossPatch> BossPatches { get; private set; } = new Dictionary<string, GeneralBossPatch>();

    public UIController UIController { get; private set; } = null!;
    public MonsterUIController MonsterUIController { get; private set; } = null!;
    public ChallengeConfigurationManager ChallengeConfigurationManager { get; private set; } = null!;
    public GlobalModifiersController GlobalModifiersFlags { get; private set; } = null!;
    public UIConfiguration UIConfiguration { get; private set; } = null!;

    private Preloader Preloader { get; set; } = null!;
    private BepInExModConfigurationHandler BepInExModConfigurationHandler { get; set; } = null!;

    public ShieldProvider ShieldProvider { get; private set; } = null!;
    public YanlaoGunProvider YanlaoGunProvider { get; private set; } = null!;

    public static BossChallengeMod Instance { get; private set; } = null!;

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

        Preloader = new Preloader();
        ShieldProvider = new ShieldProvider();
        YanlaoGunProvider = new YanlaoGunProvider();

        AssignPreloadingTargets();

        LocalizationResolver.LoadLanguage(GetLanguageCode());

        IRecordsRepository recordsRepo = new JsonRecordsRepository();
        ChallengeConfigurationManager = new ChallengeConfigurationManager(recordsRepo);
        UIConfiguration = new UIConfiguration();
        GlobalModifiersFlags = new GlobalModifiersController();

        LocalizationManager.OnLocalizeEvent += OnLocalizationChange;
        
        BepInExModConfigurationHandler = new BepInExModConfigurationHandler(Config, ChallengeConfigurationManager, UIConfiguration);

        BepInExModConfigurationHandler.InitChallengeConfiguration();
        BepInExModConfigurationHandler.HandleConfigurationValues();

        BepInExModConfigurationHandler.InitializeUIConfiguration();
        UIController = new UIController(UIConfiguration);
        BepInExModConfigurationHandler.HandleUIConfigurationValues();
        
        MonsterUIController = new MonsterUIController();

        InitializePatches();

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

    private void InitializePatches() {
        BossPatches.Add("StealthGameMonster_SpearHorseMan", GetHorseBossPatch());
        BossPatches.Add("StealthGameMonster_伏羲_新", GetFuxiBossPatch());
        BossPatches.Add("StealthGameMonster_新女媧 Variant", GetNuwaBossPatch());
        BossPatches.Add("StealthGameMonster_GouMang Variant", GetGoumangBossPatch());
        BossPatches.Add("Monster_GiantMechClaw", GetClawBossPatch());

        var butterflyPatch = GetButterflyBossPatch();
        BossPatches.Add("StealthGameMonster_Boss_ButterFly Variant", butterflyPatch);
        BossPatches.Add("StealthGameMonster_Boss_ButterFly Variant (1)", new RevivalChallengeBossClonePatch());
        BossPatches.Add("StealthGameMonster_Boss_ButterFly Variant (2)", new RevivalChallengeBossClonePatch());
        BossPatches.Add("StealthGameMonster_Boss_ButterFly Variant (3)", new RevivalChallengeBossClonePatch());
        BossPatches.Add("StealthGameMonster_Boss_ButterFly Variant (4)", new RevivalChallengeBossClonePatch());
        BossPatches.Add("Boss_Yi Gung", GetEigongBossPatch());
        BossPatches.Add("Default", GetDefaultBossPatch());
    }

    private FuxiBossPatch GetFuxiBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var fuxiBossPatch = new FuxiBossPatch();
        fuxiBossPatch.DieStates = [
            MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];

        var resetStateConfig = fuxiBossPatch.ResetStateConfiguration;
        resetStateConfig.ExitState = MonsterBase.States.Engaging;
        resetStateConfig.Animations = ["PostureBreak"];
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return fuxiBossPatch;
    }
    private GeneralBossPatch GetNuwaBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var nuwaBossPatch = new RevivalChallengeBossPatch();
        nuwaBossPatch.DieStates = [
            MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];
        nuwaBossPatch.UseKillCounter = false;
        nuwaBossPatch.UseModifiers = false;
        nuwaBossPatch.UseRecording = false;

        var resetStateConfig = nuwaBossPatch.ResetStateConfiguration;
        resetStateConfig.ExitState = MonsterBase.States.Engaging;
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return nuwaBossPatch;
    }
    private GeneralBossPatch GetGoumangBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var goumangBossPatch = new GoumangBossPatch();
        goumangBossPatch.DieStates = [
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];

        var resetStateConfig = goumangBossPatch.ResetStateConfiguration;
        resetStateConfig.ExitState = MonsterBase.States.Attack2;
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return goumangBossPatch;
    }
    private GeneralBossPatch GetClawBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var clawBossPatch = new ClawBossPatch();
        clawBossPatch.DieStates = [
            MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];

        var resetStateConfig = clawBossPatch.ResetStateConfiguration;
        resetStateConfig.Animations = ["PostureBreak"];
        resetStateConfig.ExitState = MonsterBase.States.Attack7;
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return clawBossPatch;
    }
    private ButterflyBossPatch GetButterflyBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var butterBossPatch = new ButterflyBossPatch();
        butterBossPatch.DieStates = [
            bossReviveMonsterState,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];

        var resetStateConfig = butterBossPatch.ResetStateConfiguration;
        resetStateConfig.Animations = ["Hurt"];
        resetStateConfig.ExitState = MonsterBase.States.Engaging;
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return butterBossPatch;
    }
    private GeneralBossPatch GetEigongBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var eigongBossPatch = new RevivalChallengeBossPatch();
        eigongBossPatch.DieStates = [
            MonsterBase.States.BossAngry,
            MonsterBase.States.FooStunEnter,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];

        var resetStateConfig = eigongBossPatch.ResetStateConfiguration;
        resetStateConfig.ExitState = MonsterBase.States.Engaging;
        resetStateConfig.Animations = ["BossAngry"];
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return eigongBossPatch;
    }
    private GeneralBossPatch GetHorseBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var defaultBossPatch = new HorseBossPatch();
        defaultBossPatch.DieStates = [
            MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];

        var resetStateConfig = defaultBossPatch.ResetStateConfiguration;
        resetStateConfig.ExitState = MonsterBase.States.Engaging;
        resetStateConfig.Animations = ["PostureBreak"];
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return defaultBossPatch;
    }
    private GeneralBossPatch GetDefaultBossPatch() {
        var bossReviveMonsterState = MonsterStateValuesResolver.GetState("BossRevive");

        var defaultBossPatch = new RevivalChallengeBossPatch();
        defaultBossPatch.DieStates = [
            MonsterBase.States.BossAngry,
            MonsterBase.States.LastHit,
            MonsterBase.States.Dead
        ];

        var resetStateConfig = defaultBossPatch.ResetStateConfiguration;
        resetStateConfig.ExitState = MonsterBase.States.Engaging;
        resetStateConfig.Animations = ["PostureBreak"];
        resetStateConfig.StateType = bossReviveMonsterState;
        resetStateConfig.TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];

        return defaultBossPatch;
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