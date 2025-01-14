﻿using MonsterLove.StateMachine;
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
            var result = new List<MonsterState>();

            if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration) {
                var monsterStatesRefs = (MonsterState[])monsterStatesFieldRef.GetValue(monsterBase);
                var resetBossState = (ResetBossState)InstantiateStateObject(monsterBase.gameObject, typeof(ResetBossState), "ResetBoss", ResetStateConfiguration);
                resetBossState.AssignChallengeConfig(challengeConfigurationManager.ChallengeConfiguration);
                resetBossState.stateEvents.StateEnterEvent.AddListener(ChangeBG);
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
                    cloneResetBossState.stateEvents.StateEnterEvent.AddListener(ChangeBG);
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
        public override IEnumerable<RCGEventReceiver> CreateReceivers(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = base.CreateReceivers(monster, monsterStates).ToList();

            var phaseOneTransition = CreatePhaseOneTransition();
            result.Add(phaseOneTransition.eventReceiver);

            return result;
        }

        public override void PatchMonsterFsmLookupStates(MonsterBase monsterBase, IEnumerable<MonsterState> states) {
            var stateToPatch = states.FirstOrDefault();

            if(stateToPatch != null && stateToPatch is ResetBossState) {
                base.PatchMonsterFsmLookupStates(monsterBase, [stateToPatch]);
            }
        }

        protected void PatchGroundBreaking(MonsterBase monster) {
            string targetEventInvokerPath = "P2_R22_Savepoint_GameLevel/EventBinder/General FSM Object_BreakFloor/FSM Animator/LogicRoot/[EnableInvoker]PushInfinityBoundary";
            var eventInvoker = GameObject.Find(targetEventInvokerPath);

            var invokerComponent = eventInvoker.GetComponent<OnEnableInvoker>();
            invokerComponent.OnEnableEvent.AddUniqueListener(TranslateCutscenes);
        }

        protected void ChangeBG() {
            var BGPhase3 = GameObject.Find("P2_R22_Savepoint_GameLevel/CameraCore/DockObj/OffsetObj/ShakeObj/SceneCamera/ParentToCamera/Phase3");
            var BGPhase1 = GameObject.Find("P2_R22_Savepoint_GameLevel/CameraCore/DockObj/OffsetObj/ShakeObj/SceneCamera/ParentToCamera/Phase1");

            BGPhase3.gameObject.SetActive(false);
            BGPhase1.gameObject.SetActive(true);
        }

        protected void TranslateCutscenes() {
            string cutscene1Path = "P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/[CutScene]AngryTimeline_Phase1_To_Phase2";
            string cutscene2Path = "P2_R22_Savepoint_GameLevel/EventBinder/General Boss Fight FSM Object Variant/FSM Animator/[CutScene]AngryTimeline_Phase2_To_Phase3";

            var cutscene1 = GameObject.Find(cutscene1Path);
            var cutscene2 = GameObject.Find(cutscene2Path);
            cutscene1.transform.Translate(0, -320, 0);
            cutscene2.transform.Translate(0, -320, 0);
        }


        protected TransitionWrapper CreatePhaseOneTransition() {
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
        }

        public IEnumerable<MonsterBase> GetClones(MonsterBase monsterBase) {
            var components = monsterBase.transform.parent.GetComponentsInChildren<StealthGameMonster>(true).ToList();
            components.Remove(monsterBase as StealthGameMonster);

            return components;
        }

        protected override MonsterModifierController InitializeModifiers(MonsterBase monsterBase) {
            var controller = base.InitializeModifiers(monsterBase);

            if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration &&
                challengeConfigurationManager.ChallengeConfiguration.ModifiersEnabled) {

                var clones = GetClones(monsterBase);

                foreach (var clone in clones) {
                    var cloneModifiers = CreateModifiers(clone);
                    controller.Modifiers.AddRange(cloneModifiers);
                }
            }

            return controller;
        }
    }
}
