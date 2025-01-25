using HarmonyLib;
using BossChallengeMod.CustomActions;
using BossChallengeMod.CustomMonsterStates;
using BossChallengeMod.Extensions;
using BossChallengeMod.Global;
using RCGFSM.Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.BossPatches.TargetPatches {
    public class FuxiBossPatch : RevivalChallengeBossPatch {

        public override IEnumerable<RCGEventReceiver> CreateReceivers(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = base.CreateReceivers(monster, monsterStates).ToList();

            var phaseOneTransition = CreatePhaseOneTransition();
            result.Add(phaseOneTransition.eventReceiver);

            string nuwaPath = "P2_R22_Savepoint_GameLevel/Room/Prefab/EventBinder (Boss Fight 相關)/General Boss Fight FSM Object_風氏兄妹/FSM Animator/LogicRoot/---Boss---/BossShowHealthArea/StealthGameMonster_新女媧 Variant";
            var nuwa = GameObject.Find(nuwaPath).GetComponent<MonsterBase>();

            if (nuwa != null) {
                MonsterChangeStateAction changeStateAction = CreateNuwaChangeStateAction(nuwa, ResetStateConfiguration.StateType);
                phaseOneTransition.eventReceiver.AddSubscriber(changeStateAction);
            }

            return result;
        }

        protected TransitionWrapper CreatePhaseOneTransition() {
            string fromPath = "P2_R22_Savepoint_GameLevel/Room/Prefab/EventBinder (Boss Fight 相關)/General Boss Fight FSM Object_風氏兄妹/--[States]/FSM/[State] BossFighting_Phase2";
            string toPath = "P2_R22_Savepoint_GameLevel/Room/Prefab/EventBinder (Boss Fight 相關)/General Boss Fight FSM Object_風氏兄妹/--[States]/FSM/[State] BossFighting_Phase1";

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
                    UnityEngine.Object.Destroy(receiver);
                }
            }

            return new TransitionWrapper(transitionComponent, transitionReceiver);
        }

        protected MonsterChangeStateAction CreateNuwaChangeStateAction(MonsterBase monster, MonsterBase.States monsterState) {
            string parentPath = "P2_R22_Savepoint_GameLevel/Room/Prefab/EventBinder (Boss Fight 相關)/General Boss Fight FSM Object_風氏兄妹/--[States]/FSM/[State] BossFighting_Phase2";
            var parentStateComponent = GameObject.Find(parentPath).GetComponent<GeneralState>();

            var changeStateAction = parentStateComponent.AddChildrenComponent<MonsterChangeStateActionDecorator>("[Action] ChangeNuwaState");
            changeStateAction.targetMonster = monster;
            changeStateAction.targetStateId = monsterState;

            return changeStateAction;
        }
    }
}
