using MonsterLove.StateMachine;
using BossChallengeMod.Configuration;
using BossChallengeMod.CustomMonsterStates;
using BossChallengeMod.Extensions;
using BossChallengeMod.Global;
using BossChallengeMod.Modifiers;
using RCGFSM.Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BossChallengeMod.BossPatches {
    public class ButterflyBossPatch : RevivalChallengeBossPatch {

        public ChallengeConfiguration ChallengeConfiguration { get; set; }

        public override IEnumerable<MonsterState> PatchMonsterStates(MonsterBase monsterBase) {
            try {
                var result = new List<MonsterState>();

                if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration) {
                    var monsterStatesRefs = (MonsterState[])monsterStatesFieldRef.GetValue(monsterBase);
                    var resetBossState = (ResetBossState)InstantiateStateObject(monsterBase.gameObject, typeof(ResetBossState), "ResetBoss", ResetStateConfiguration);
                    resetBossState.AssignChallengeConfig(challengeConfigurationManager.ChallengeConfiguration);
                    resetBossState.stateEvents.StateExitEvent.AddListener(ChangeBG);
                    monsterStatesFieldRef.SetValue(monsterBase, monsterStatesRefs.Append(resetBossState).ToArray());
                    result.Add(resetBossState);

                    var clones = GetClones(monsterBase);
                    var cloneResetBossStates = new List<ResetBossState>();
                    foreach (var clone in clones) {
                        var cloneMonsterStatesRefs = (MonsterState[])monsterStatesFieldRef.GetValue(clone);
                        var cloneResetBossState = (ResetBossState)InstantiateStateObject(clone.gameObject, typeof(ResetBossState), "ResetBoss", ResetStateConfiguration);
                        cloneResetBossState.AssignChallengeConfig(challengeConfigurationManager.ChallengeConfiguration);
                        cloneResetBossStates.Add(cloneResetBossState);
                        monsterStatesFieldRef.SetValue(clone, cloneMonsterStatesRefs.Append(cloneResetBossState).ToArray());
                        cloneResetBossState.stateEvents.StateExitEvent.AddListener(ChangeBG);
                        result.Add(cloneResetBossState);
                    }

                    if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration && UseKillCounter) {
                        var killCounter = InitializeKillCounter(monsterBase);
                        killCounter.UseRecording = UseRecording;

                        Action stateEnterEventActions = () => killCounter.IncrementCounter();

                        var modifiersController = InitializeModifiers(monsterBase);

                        stateEnterEventActions += () => {
                            try {
                                modifiersController.RollModifiers();
                                modifiersController.ApplyModifiers(killCounter.KillCounter);
                            } catch (Exception e) {
                                Log.Info($"{e.Message}, {e.StackTrace}");
                            }
                        };

                        BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(modifiersController);

                        resetBossState.monsterKillCounter = killCounter;

                        resetBossState.stateEvents.StateEnterEvent.AddListener(() => stateEnterEventActions.Invoke());
                        BossChallengeMod.Instance.MonsterUIController.ChangeKillCounter(killCounter);

                        foreach (var cloneState in cloneResetBossStates) {
                            cloneState.monsterKillCounter = killCounter;
                            cloneState.stateEvents.StateEnterEvent.AddListener(() => stateEnterEventActions.Invoke());
                        }

                        killCounter.CheckLoad();
                    }

                }

                PatchGroundBreaking(monsterBase);

                return result;

            }
            catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
                throw ex;
            }
        }
        public override IEnumerable<RCGEventReceiver> CreateReceivers(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            try {
                var result = base.CreateReceivers(monster, monsterStates).ToList();

                var phaseOneTransition = CreatePhaseOneTransition();
                result.Add(phaseOneTransition.eventReceiver);

                return result;

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
                throw ex;
            }
        }

        public override void PatchMonsterFsmLookupStates(MonsterBase monsterBase, IEnumerable<MonsterState> states) {
            try {

                var stateToPatch = states.FirstOrDefault();

                if(stateToPatch != null && stateToPatch is ResetBossState) {
                    base.PatchMonsterFsmLookupStates(monsterBase, [stateToPatch]);
                }
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        public override IEnumerable<RCGEventSender> CreateSenders(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = new List<RCGEventSender>();

            foreach (var state in monsterStates) {
                switch (state) {
                    case ResetBossState resState:
                        if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration) {
                            var eventType = eventTypesResolver.RequestType(resetBossStateEventType);
                            var resStateEnterSender = CreateEventSender(resState.gameObject, eventType, resState.stateEvents.StateExitEvent);
                            result.Add(resStateEnterSender);
                        }

                        continue;
                    default:
                        continue;
                }
            }

            return result;
        }

        protected void PatchGroundBreaking(MonsterBase monster) {
            try {
                string targetEventInvokerPath = "P2_R22_Savepoint_GameLevel/EventBinder/General FSM Object_BreakFloor/FSM Animator/LogicRoot/[EnableInvoker]PushInfinityBoundary";
                var eventInvoker = GameObject.Find(targetEventInvokerPath);

                var invokerComponent = eventInvoker.GetComponent<OnEnableInvoker>();
                invokerComponent.OnEnableEvent.AddUniqueListener(TranslateCutscenes);

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        protected void ChangeBG() {
            try {

                var BGPhase3 = GameObject.Find("P2_R22_Savepoint_GameLevel/CameraCore/DockObj/OffsetObj/ShakeObj/SceneCamera/ParentToCamera/Phase3");
                var BGPhase1 = GameObject.Find("P2_R22_Savepoint_GameLevel/CameraCore/DockObj/OffsetObj/ShakeObj/SceneCamera/ParentToCamera/Phase1");

                BGPhase3.gameObject.SetActive(false);
                BGPhase1.gameObject.SetActive(true);
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        protected void TranslateCutscenes() {
            try {
                string cutscene1Path = "P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/[CutScene]AngryTimeline_Phase1_To_Phase2";
                string cutscene2Path = "P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/[CutScene]AngryTimeline_Phase2_To_Phase3";

                var cutscene1 = GameObject.Find(cutscene1Path);
                var cutscene2 = GameObject.Find(cutscene2Path);
                cutscene1.transform.Translate(0, -320, 0);
                cutscene2.transform.Translate(0, -320, 0);

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }


        protected TransitionWrapper CreatePhaseOneTransition() {
            try {
                string fromPath = "P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/--[States]/FSM/[State] BossFighting_Phase3";
                string toPath = "P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/--[States]/FSM/[State] BossFighting_Phase1";

                var parentStateComponent = GameObject.Find(fromPath).GetComponent<GeneralState>();
                var targetStateComponent = GameObject.Find(toPath).GetComponent<GeneralState>();

                var eventType = eventTypesResolver.RequestType(resetBossStateEventType);

                var transitionComponent = CreateTransition(
                    parentStateComponent.gameObject,
                    parentStateComponent,
                    targetStateComponent,
                    "[Patch transition] ToPhaseOne");

                var transitionReceiver = CreateEventReceiverAsComponent(transitionComponent.gameObject, eventType, [transitionComponent]);
                var receivers = transitionComponent.GetComponents<RCGEventReceiver>();
                foreach (var receiver in receivers) {
                    if (receiver.eventType == default) {
                        GameObject.Destroy(receiver);
                    }
                }

                return new TransitionWrapper(transitionComponent, transitionReceiver);

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
                throw ex;
            }
        }

        public IEnumerable<MonsterBase> GetClones(MonsterBase monsterBase) {
            try {

                var components = monsterBase.transform.parent.GetComponentsInChildren<StealthGameMonster>(true).ToList();
                components.Remove(monsterBase as StealthGameMonster);

                return components;
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
                throw ex;
            }
        }

        protected override MonsterModifierController InitializeModifiers(MonsterBase monsterBase) {
            try {
                var controller = base.InitializeModifiers(monsterBase);

                if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration &&
                    challengeConfigurationManager.ChallengeConfiguration.ModifiersEnabled) {

                    var clones = GetClones(monsterBase);

                    foreach (var clone in clones) {
                        var cloneModifiers = CreateCloneModifiers(clone);
                        controller.Modifiers.AddRange(cloneModifiers);
                    }
                }

                return controller;

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
                throw ex;
            }
        }

        protected IEnumerable<ModifierBase> CreateCloneModifiers(MonsterBase monsterBase) {
            try {
                var result = new List<ModifierBase>();
                var modifiersFolder = new GameObject("Modifiers");
                modifiersFolder.transform.SetParent(monsterBase.transform, false);

                var speedModifier = modifiersFolder.AddChildrenComponent<SpeedModifier>("SpeedModifier");
                result.Add(speedModifier);

                var scalingSpeedModifier = modifiersFolder.AddChildrenComponent<ScalingSpeedModifier>("SpeedScalingModifier");
                scalingSpeedModifier.challengeConfiguration = challengeConfigurationManager.ChallengeConfiguration;
                result.Add(scalingSpeedModifier);

                var parryDamageModifier = modifiersFolder.AddChildrenComponent<ParryDirectDamageModifier>("ParryDamageModifier");
                result.Add(parryDamageModifier);

                var damageBuildupModifier = modifiersFolder.AddChildrenComponent<DamageBuildupModifier>("DamageBuildupModifier");
                result.Add(parryDamageModifier);

                var knockbackModifier = modifiersFolder.AddChildrenComponent<KnockbackModifier>("KnockbackModifier");
                result.Add(knockbackModifier);

                //var knockoutModifier = modifiersFolder.AddChildrenComponent<KnockoutModifier>("KnockoutModifier");
                //result.Add(knockoutModifier);


                var enduranceModifier = modifiersFolder.AddChildrenComponent<EnduranceModifier>("EnduranceModifier");
                result.Add(enduranceModifier);

                return result;

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
                throw ex;
            }
        }
    }
}
