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
using BossChallengeMod.Modifiers.Managers;

namespace BossChallengeMod.BossPatches.TargetPatches {
    public class ButterflyBossPatch : RevivalChallengeBossPatch {

        public ChallengeConfiguration ChallengeConfiguration { get; set; }

        protected string resetBossStateExitEventType = "RestoreBoss_exit";

        public override IEnumerable<MonsterState> PatchMonsterStates(MonsterBase monsterBase) {
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
                        modifiersController.RollModifiers();
                        modifiersController.ApplyModifiers(killCounter.KillCounter);
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

        public override IEnumerable<RCGEventSender> CreateSenders(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = base.CreateSenders(monster, monsterStates).ToList();

            foreach (var state in monsterStates) {
                switch (state) {
                    case ResetBossState resState:
                        if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration) {
                            var eventType = eventTypesResolver.RequestType(resetBossStateExitEventType);
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

        public override IEnumerable<RCGEventReceiver> CreateReceivers(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = base.CreateReceivers(monster, monsterStates).ToList();

            var phaseOneTransition = CreatePhaseOneTransition();
            result.Add(phaseOneTransition.eventReceiver);

            return result;
        }

        public override void PatchMonsterFsmLookupStates(MonsterBase monsterBase, IEnumerable<MonsterState> states) {
            var stateToPatch = states.FirstOrDefault();

            if (stateToPatch != null && stateToPatch is ResetBossState) {
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

            var eventType = eventTypesResolver.RequestType(resetBossStateExitEventType);

            var transitionComponent = CreateTransition(
                parentStateComponent.gameObject,
                parentStateComponent,
                targetStateComponent,
                "[Patch transition] ToPhaseOne");

            var transitionReceiver = CreateEventReceiverAsComponent(transitionComponent.gameObject, eventType, [transitionComponent]);
            var receivers = transitionComponent.GetComponents<RCGEventReceiver>();
            foreach (var receiver in receivers) {
                if (receiver.eventType == default) {
                    UnityEngine.Object.Destroy(receiver);
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

            var shieldController = monsterBase.gameObject.GetComponent<MonsterShieldController>();
            if (shieldController != null) {
                GameObject.Destroy(shieldController);
            }

            if (challengeConfigurationManager.ChallengeConfiguration.EnableRestoration &&
                challengeConfigurationManager.ChallengeConfiguration.ModifiersEnabled) {

                var clones = GetClones(monsterBase);

                foreach (var clone in clones) {
                    var cloneModifiers = CreateCloneModifiers(clone);
                    controller.Modifiers.AddRange(cloneModifiers);
                }
            }

            return controller;
        }

        protected IEnumerable<ModifierBase> CreateCloneModifiers(MonsterBase monsterBase) {
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
        }

        protected override void PopulateModifierController(MonsterModifierController modifierController, ChallengeConfiguration config) {
            base.PopulateModifierController(modifierController, config);

            modifierController.ModifierConfigs.RemoveAll(m => m.Key.Contains("shield"));
        }
    }
}
