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
        protected StoryChallengeConfigurationManager storyChallengeConfigurationManager = BossChallengeMod.Instance.StoryChallengeConfigurationManager;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                if(ApplicationCore.IsInBossMemoryMode) return challengeConfigurationManager.ChallengeConfiguration;
                else return storyChallengeConfigurationManager.ChallengeConfiguration;
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

        public override void PatchMonsterPostureSystem(MonsterBase monsterBase) {
            base.PatchMonsterPostureSystem(monsterBase);

            if (ConfigurationToUse.EnableMod) {
                int insertIndex = monsterBase.postureSystem.DieHandleingStates.IndexOf(InsertPlaceState);
                if (insertIndex >= 0) {
                    monsterBase.postureSystem.DieHandleingStates.Insert(insertIndex, bossReviveMonsterState);
                }
            }
        }

        public override IEnumerable<MonsterState> PatchMonsterStates(MonsterBase monsterBase) {
            var result = base.PatchMonsterStates(monsterBase).ToList();
            
            try {
                if(ConfigurationToUse.EnableMod) {
                    var monsterStatesRefs = (MonsterState[])monsterStatesFieldRef.GetValue(monsterBase);
                    var resetBossState = (ResetBossState)InstantiateStateObject(monsterBase.gameObject, typeof(ResetBossState), "ResetBoss", ResetStateConfiguration);
                    resetBossState.AssignChallengeConfig(ConfigurationToUse);

                    if (ConfigurationToUse.EnableMod && UseKillCounter) {
                        var killCounter = InitializeKillCounter(monsterBase);
                        killCounter.UseRecording = UseRecording;

                        Action stateEnterEventActions = () => {
                            killCounter.IncrementCounter();
                        };

                        var modifiersController = InitializeModifiers(monsterBase);

                        stateEnterEventActions += () => {
                            modifiersController.RollModifiers(killCounter.KillCounter);
                            modifiersController.ApplyModifiers(killCounter.KillCounter);
                        };


                        resetBossState.monsterKillCounter = killCounter;

                        resetBossState.stateEvents.StateEnterEvent.AddListener(() => stateEnterEventActions.Invoke());

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
                        if(ConfigurationToUse.EnableMod) {
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
            if(ConfigurationToUse.EnableMod) {
                base.PostfixPatch(monster);
            }
        }

        public override bool CanBeApplied() {
            switch (EnemyType) {
                case ChallengeEnemyType.Boss:
                    return ConfigurationToUse.AffectBosses;
                case ChallengeEnemyType.Miniboss:
                    return ConfigurationToUse.AffectMiniBosses;
                case ChallengeEnemyType.Regular:
                    return ConfigurationToUse.AffectRegularEnemies;
                default:
                    return true;
            }
        }

        protected virtual MonsterKillCounter InitializeKillCounter(MonsterBase monsterBase) {
            var killCounter = monsterBase.gameObject.AddComponent<MonsterKillCounter>();
            killCounter.EnemyType = EnemyType;          
            killCounter.CanBeTracked = UseKillCounterTracking;
            killCounter.UseProximityShow = UseProximityActivation;

            return killCounter;
        }

        protected virtual MonsterModifierController InitializeModifiers(MonsterBase monsterBase) {
            var config = ConfigurationToUse;

            var modifiers = CreateModifiers(monsterBase);
            var shieldController = monsterBase.gameObject.AddComponent<MonsterShieldController>();
            var yanlaoGunController = monsterBase.gameObject.AddComponent<MonsterYanlaoGunController>();
            var modifierController = monsterBase.gameObject.AddComponent<MonsterModifierController>();

            if (config.ModifiersEnabled && UseModifiers) {
                PopulateModifierController(modifierController, config);
            }

            modifierController.EnemyType = EnemyType;
            modifierController.CanBeTracked = UseModifierControllerTracking;
            modifierController.UseProximityShow = UseProximityActivation;
            modifierController.UseCompositeTracking = UseCompositeTracking;
            modifierController.Init();
            modifierController.GenerateAvailableMods();

            return modifierController;
        }

        protected virtual void PopulateModifierController(MonsterModifierController modifierController, ChallengeConfiguration config) {
            if (config.SpeedModifierEnabled) {
                var speedModifier = new Modifiers.ModifierConfig() {
                    Key = "speed_temp",
                };
                speedModifier.Incompatibles.Add(speedModifier.Key);
                modifierController.ModifierConfigs.Add(speedModifier);
            }

            if (config.TimerModifierEnabled) {
                var timerModifier = new Modifiers.ModifierConfig() {
                    Key = "timer",
                };
                timerModifier.Incompatibles.Add(timerModifier.Key);
                timerModifier.Incompatibles.Add("regeneration");
                modifierController.ModifierConfigs.Add(timerModifier);
            }

            if (config.ParryDirectDamageModifierEnabled) {
                var parryDamageModifier = new Modifiers.ModifierConfig() {
                    Key = "parry_damage",
                };
                parryDamageModifier.Incompatibles.Add(parryDamageModifier.Key);
                modifierController.ModifierConfigs.Add(parryDamageModifier);
            }

            if (config.DamageBuildupModifierEnabled) {
                var damageBuildupModifier = new Modifiers.ModifierConfig() {
                    Key = "damage_buildup",
                };
                damageBuildupModifier.Incompatibles.Add(damageBuildupModifier.Key);
                modifierController.ModifierConfigs.Add(damageBuildupModifier);
            }

            if (config.RegenerationModifierEnabled) {
                var regenerationModifier = new Modifiers.ModifierConfig() {
                    Key = "regeneration",
                };
                regenerationModifier.Incompatibles.Add(regenerationModifier.Key);
                regenerationModifier.Incompatibles.AddRange(["timer"]);
                modifierController.ModifierConfigs.Add(regenerationModifier);
            }

            if (config.KnockbackModifierEnabled) {
                var knockbackModifier = new ModifierConfig() {
                    Key = "knockback"
                };
                knockbackModifier.Incompatibles.Add(knockbackModifier.Key);
                modifierController.ModifierConfigs.Add(knockbackModifier);
            }

            //if (config.KnockoutModifierEnabled) {
            //    var knockoutModifier = new ModifierConfig() {
            //        Key = "knockout"
            //    };
            //    knockoutModifier.Incompatibles.Add(knockoutModifier.Key);
            //    modifierController.ModifierConfigs.Add(knockoutModifier);
            //}

            if (config.RandomArrowModifierEnabled) {
                var arrowModifier = new ModifierConfig() {
                    Key = "random_arrow"
                };
                arrowModifier.Incompatibles.Add(arrowModifier.Key);
                modifierController.ModifierConfigs.Add(arrowModifier);
            }

            if (config.RandomTalismanModifierEnabled) {
                var talismanModifier = new ModifierConfig() {
                    Key = "random_talisman"
                };
                talismanModifier.Incompatibles.Add(talismanModifier.Key);
                modifierController.ModifierConfigs.Add(talismanModifier);
            }

            if (config.EnduranceModifierEnabled) {
                var enduranceModifier = new ModifierConfig() {
                    Key = "endurance"
                };
                enduranceModifier.Incompatibles.Add(enduranceModifier.Key);
                modifierController.ModifierConfigs.Add(enduranceModifier);
            }

            if (config.QiShieldModifierEnabled) {
                var qiShieldModifier = new ModifierConfig() {
                    Key = "qi_shield"
                };
                qiShieldModifier.Incompatibles.Add(qiShieldModifier.Key);
                qiShieldModifier.Incompatibles.AddRange(["timer_shield", "distance_shield"]);
                modifierController.ModifierConfigs.Add(qiShieldModifier);
            }

            if (config.TimedShieldModifierEnabled) {
                var impactShieldModifier = new ModifierConfig() {
                    Key = "timer_shield"
                };
                impactShieldModifier.Incompatibles.Add(impactShieldModifier.Key);
                impactShieldModifier.Incompatibles.AddRange(["qi_shield", "distance_shield"]);
                modifierController.ModifierConfigs.Add(impactShieldModifier);
            }

            if (config.QiOverloadModifierEnabled) {
                var qiOverloadModifier = new ModifierConfig() {
                    Key = "qi_overload"
                };
                qiOverloadModifier.Incompatibles.Add(qiOverloadModifier.Key);
                modifierController.ModifierConfigs.Add(qiOverloadModifier);
            }

            if (config.DistanceShieldModifierEnabled) {
                var distanceShieldModifier = new ModifierConfig() {
                    Key = "distance_shield"
                };
                distanceShieldModifier.Incompatibles.Add(distanceShieldModifier.Key);
                distanceShieldModifier.Incompatibles.AddRange(["qi_shield", "timer_shield"]);
                modifierController.ModifierConfigs.Add(distanceShieldModifier);
            }

            if(config.YanlaoGunModifierEnabled) {
                var yanlaoModifier = new ModifierConfig() {
                    Key = "ya_gun"
                };
                yanlaoModifier.Incompatibles.Add(yanlaoModifier.Key);
                modifierController.ModifierConfigs.Add(yanlaoModifier);
            }
        }

        protected virtual IEnumerable<ModifierBase> CreateModifiers(MonsterBase monsterBase) {
            var result = new List<ModifierBase>();
            var modifiersFolder = new GameObject("Modifiers");
            modifiersFolder.transform.SetParent(monsterBase.transform, false);

            var speedModifier = modifiersFolder.AddChildrenComponent<SpeedModifier>("SpeedModifier");
            result.Add(speedModifier);

            var scalingSpeedModifier = modifiersFolder.AddChildrenComponent<ScalingSpeedModifier>("SpeedScalingModifier");
            scalingSpeedModifier.EnemyType = EnemyType;
            scalingSpeedModifier.challengeConfiguration = ConfigurationToUse;
            result.Add(scalingSpeedModifier);

            var timerModifier = modifiersFolder.AddChildrenComponent<TimerModifier>("TimerModifier");
            result.Add(timerModifier);

            var parryDamageModifier = modifiersFolder.AddChildrenComponent<ParryDirectDamageModifier>("ParryDamageModifier");
            result.Add(parryDamageModifier);

            var damageBuildupModifier = modifiersFolder.AddChildrenComponent<DamageBuildupModifier>("DamageBuildupModifier");
            result.Add(parryDamageModifier);

            var regenModifier = modifiersFolder.AddChildrenComponent<RegenerationModifier>("RegenerationModifier");
            result.Add(regenModifier);

            var knockbackModifier = modifiersFolder.AddChildrenComponent<KnockbackModifier>("KnockbackModifier");
            result.Add(knockbackModifier);

            //var knockoutModifier = modifiersFolder.AddChildrenComponent<KnockoutModifier>("KnockoutModifier");
            //result.Add(knockoutModifier);

            var arrowModifier = modifiersFolder.AddChildrenComponent<RandomArrowModifier>("RandomArrowModifier");
            result.Add(arrowModifier);

            var talismanModifier = modifiersFolder.AddChildrenComponent<RandomTaliModifier>("RandomTalismanModifier");
            result.Add(talismanModifier);

            var enduranceModifier = modifiersFolder.AddChildrenComponent<EnduranceModifier>("EnduranceModifier");
            result.Add(enduranceModifier);

            var qiShieldModifier = modifiersFolder.AddChildrenComponent<QiShieldModifier>("QiShieldModifer");
            result.Add(qiShieldModifier);

            var impactShieldModifier = modifiersFolder.AddChildrenComponent<TimedShieldModifier>("ImpactShieldModifier");
            result.Add(impactShieldModifier);

            var qiOverloadModifier = modifiersFolder.AddChildrenComponent<QiOverloadModifier>("QiOverloadModifier");
            result.Add(qiOverloadModifier);

            var distanceShieldModifier = modifiersFolder.AddChildrenComponent<DistanceShieldModifier>("DistanceShieldModifier");
            result.Add(distanceShieldModifier);

            var yanlaoGunModifier = modifiersFolder.AddChildrenComponent<YanlaoGunModifier>("YanlaoGunModifier");
            result.Add(yanlaoGunModifier);

            return result;
        }
    }

    public enum ChallengeEnemyType {
        Boss,
        Miniboss,
        Regular
    }
}
