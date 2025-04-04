using BossChallengeMod.Configuration;
using BossChallengeMod.CustomMonsterStates;
using BossChallengeMod.KillCounting;
using BossChallengeMod.Modifiers;
using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace BossChallengeMod.BossPatches {
    public class RevivalChallengeBossPatch : GeneralBossPatch {
        public ResetBossStateConfiguration ResetStateConfiguration = new ResetBossStateConfiguration();

        protected ChallengeConfigurationManager challengeConfigurationManager = BossChallengeMod.Instance.ChallengeConfigurationManager;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                return challengeConfigurationManager.ChallengeConfiguration;
            }
        }

        protected bool IsModEnabled {
            get {
                if(ApplicationCore.IsInBossMemoryMode) return ConfigurationToUse.IsEnabledInMoB;
                return ConfigurationToUse.IsEnabledInNormal;
            }
        }


        private MonsterBase.States bossReviveMonsterState = BossChallengeMod.Instance.MonsterStateValuesResolver.GetState("BossRevive");
        protected string resetBossStateEventType = "RestoreBoss_enter";

        public bool UseKillCounter { get; set; } = true;
        public bool UseModifiers { get; set; } = true;
        public bool UseRecording { get; set; } = true;
        public bool UseKillCounterTracking { get; set; } = true;
        public bool UseModifierControllerTracking { get; set; } = true;
        public bool UseProximityActivation { get; set; } = false;
        public bool UseCompositeTracking { get; set; } = false;

        public MonsterBase.States InsertPlaceState { get; set; } = MonsterBase.States.LastHit;
        public ChallengeEnemyType EnemyType { get; set; } = ChallengeEnemyType.Boss;

        protected List<MonsterBase.States> ModifiedDieStates = new List<MonsterBase.States>();

        protected int MaxEnemyCycles {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.MaxBossCycles;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MaxMinibossCycles;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.MaxEnemyCycles;
                    default:
                        return 1;
                }
            }
        }

        public override void PatchMonsterPostureSystem(MonsterBase monsterBase) {
            base.PatchMonsterPostureSystem(monsterBase);

            if (IsModEnabled) {
                ModifiedDieStates.Clear();
                ModifiedDieStates.AddRange(DieStates);

                int insertIndex = monsterBase.postureSystem.DieHandleingStates.IndexOf(InsertPlaceState);
                if (insertIndex >= 0 && (MaxEnemyCycles > 1 || MaxEnemyCycles <= 0)) {
                    monsterBase.postureSystem.DieHandleingStates.Insert(insertIndex, bossReviveMonsterState);
                    ModifiedDieStates.Insert(insertIndex, bossReviveMonsterState);
                }
            }
        }

        public override IEnumerable<MonsterState> PatchMonsterStates(MonsterBase monsterBase) {
            var result = base.PatchMonsterStates(monsterBase).ToList();
            
            try {
                if(IsModEnabled) {
                    var monsterStatesRefs = (MonsterState[])monsterStatesFieldRef.GetValue(monsterBase);
                    var resetBossState = (ResetBossState)InstantiateStateObject(monsterBase.gameObject, typeof(ResetBossState), "ResetBoss", ResetStateConfiguration);
                    resetBossState.AssignChallengeConfig(ConfigurationToUse);

                    if (IsModEnabled && UseKillCounter) {
                        var mainController = InitializeMainController(monsterBase, resetBossState);

                        var killCounter = InitializeKillCounter(monsterBase, mainController);
                        var modifiersController = InitializeModifiers(monsterBase, mainController);

                        resetBossState.monsterKillCounter = killCounter;

                        if(!UseProximityActivation) {
                            BossChallengeMod.Instance.MonsterUIController.ChangeKillCounter(killCounter);
                            BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(modifiersController);
                        }

                        killCounter.CheckInit();
                    }


                    monsterStatesFieldRef.SetValue(monsterBase, monsterStatesRefs.Append(resetBossState).ToArray());
                    result.Add(resetBossState);
                }

            }
            catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }

            return result;
        }

        public override IEnumerable<RCGEventSender> CreateSenders(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = base.CreateSenders(monster, monsterStates).ToList();

            foreach (var state in monsterStates) {
                switch (state) {
                    case ResetBossState resState:
                        if(IsModEnabled) {
                            var eventType = eventTypesResolver.RequestType(resetBossStateEventType);
                            var resStateEnterSender = CreateEventSender(resState.gameObject, eventType, resState.stateEvents.StateEnterEvent);
                            result.Add(resStateEnterSender);
                        }

                        continue;
                    default:
                        continue;
                }
            }

            return result;
        }

        public override void PostfixPatch(MonsterBase monster) {
            if(IsModEnabled) {
                base.PostfixPatch(monster);
            }
        }

        public override bool CanBeApplied() {
            switch (EnemyType) {
                case ChallengeEnemyType.Boss:
                    return ConfigurationToUse.AffectBosses;
                case ChallengeEnemyType.Miniboss:
                    return ConfigurationToUse.AffectMinibosses;
                case ChallengeEnemyType.Regular:
                    return ConfigurationToUse.AffectEnemies;
                default:
                    return true;
            }
        }

        protected virtual ChallengeMonsterController InitializeMainController(MonsterBase monsterBase, ResetBossState resetBossState) {
            var controller = monsterBase.gameObject.AddComponent<ChallengeMonsterController>();
            controller.DieHandelingStates.AddRange(ModifiedDieStates);

            resetBossState.OnStateEnterInvoke += () => { controller.ProcessRevivalStateEnter(); };
            resetBossState.stateEvents.StateExitEvent.AddListener(() => { controller.OnRevivalStateExit?.Invoke(); });
            

            monsterBase.OnEngageEvent.AddListener(() => { controller.OnEngage?.Invoke(); });
            monsterBase.OnDisEngageEvent.AddListener(() => { controller.OnDisengage?.Invoke(); });
            monsterBase.OnDie.AddListener(() => { controller.ProcessDeath(); });

            return controller;
        }

        protected virtual MonsterKillCounter InitializeKillCounter(MonsterBase monsterBase, ChallengeMonsterController monsterController) {
            var killCounter = monsterBase.gameObject.AddComponent<MonsterKillCounter>();
            killCounter.EnemyType = EnemyType;          
            killCounter.CanBeTracked = UseKillCounterTracking;
            killCounter.UseProximityShow = UseProximityActivation;
            killCounter.UseRecording = UseRecording;

            monsterController.OnRevivalStateEnter += killCounter.UpdateCounter;
            monsterController.OnEngage += killCounter.OnEngage;
            monsterController.OnDisengage += killCounter.OnDisengage;
            monsterController.OnDie += killCounter.UpdateCounter;

            return killCounter;
        }

        protected virtual MonsterModifierController InitializeModifiers(MonsterBase monsterBase, ChallengeMonsterController monsterController) {
            var config = ConfigurationToUse;

            var modifierController = monsterBase.gameObject.AddComponent<MonsterModifierController>();

            //if (config.ModifiersEnabled && UseModifiers) {
                InitModifiers(monsterBase, modifierController, config);
            //}

            modifierController.EnemyType = EnemyType;
            modifierController.CanBeTracked = UseModifierControllerTracking;
            modifierController.UseProximityShow = UseProximityActivation;
            modifierController.UseCompositeTracking = UseCompositeTracking;

            monsterController.OnDie += modifierController.OnDie;
            monsterController.OnEngage += modifierController.OnEngage;
            monsterController.OnDisengage += modifierController.OnDisengage;
            monsterController.OnRevivalStateEnter += modifierController.OnRevival;

            modifierController.OnDestroyActions += () => {
                monsterController.OnDie -= modifierController.OnDie;
                monsterController.OnEngage -= modifierController.OnEngage;
                monsterController.OnDisengage -= modifierController.OnDisengage;
                monsterController.OnRevivalStateEnter -= modifierController.OnRevival;
            };

            modifierController.Init();
            modifierController.GenerateAvailableMods();

            return modifierController;
        }

        protected virtual IEnumerable<ModifierBase> InitModifiers(
            MonsterBase monsterBase,
            MonsterModifierController modifierController,
            ChallengeConfiguration config) {
            var result = new List<ModifierBase>();
            var modifiersFolder = new GameObject("Modifiers");
            modifiersFolder.transform.SetParent(monsterBase.transform, false);

            var modifiersConfigs = BossChallengeMod.Modifiers.Modifiers
                .Where(mc => !mc.IgnoredMonsters.Contains(monsterBase.name))
                .ToList();

            var sharedControllers = new Dictionary<Type, Component>();

            foreach (var modifierConfig in modifiersConfigs) {
                if(!UseModifiers) {
                    continue;
                }

                if((!modifierConfig.CreateConditionPredicate?.Invoke(modifierConfig) ?? false)) {
                    continue;
                }

                if (modifierConfig.IgnoredMonsters.Contains(monsterBase.gameObject.name)) {
                    continue;
                }

                var controllerComponent = GetOrCreateControllerComponent(monsterBase, modifierConfig, sharedControllers);

                var modifierObject = modifiersFolder.AddChildrenComponent(modifierConfig.ModifierType, modifierConfig.ObjectName);
                if (modifierObject is not ModifierBase createdModifier) {
                    GameObject.Destroy(modifierObject);

                    if (modifierConfig.ControllerConfig != null && controllerComponent != null && !modifierConfig.ControllerConfig.IsShared) {
                        GameObject.Destroy(controllerComponent);
                    }

                    Log.Error("Created modifier is not ModifierBase");
                    continue;
                }

                InitializeModifier(createdModifier, modifierConfig, controllerComponent);

                result.Add(createdModifier);

                modifierController.ModifierConfigs.Add(modifierConfig);            
            }

            return result;
        }

        protected Component? GetOrCreateControllerComponent(
            MonsterBase monsterBase,
            ModifierConfig modifierConfig,
            Dictionary<Type, Component> sharedControllers) {
            if (modifierConfig.ControllerConfig is not { } controllerConfig) return null;

            if (!controllerConfig.IsShared ||
                !sharedControllers.TryGetValue(controllerConfig.ControllerType, out var controllerComponent)) {
                controllerComponent = monsterBase.gameObject.AddComponent(controllerConfig.ControllerType);
            }

            if (controllerConfig.IsShared) {
                sharedControllers.TryAdd(controllerConfig.ControllerType, controllerComponent);
            }

            return controllerComponent;
        }

        protected void InitializeModifier(
            ModifierBase modifier,
            ModifierConfig modifierConfig,
            Component? controllerComponent) {
            modifier.Key = modifierConfig.Key;
            modifier.EnemyType = EnemyType;
            modifier.challengeConfiguration = ConfigurationToUse;

            if (controllerComponent != null) {
                modifier.SetController(controllerComponent);
            }
        }
    }

    public enum ChallengeEnemyType {
        Boss,
        Miniboss,
        Regular
    }
}
