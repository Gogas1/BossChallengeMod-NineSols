using BossChallengeMod.CustomMonsterStates;
using BossChallengeMod.Global;
using RCGFSM.Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.BossPatches {
    public class EigongBossPatch : RevivalChallengeBossPatch {

        public override IEnumerable<RCGEventReceiver> CreateReceivers(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = base.CreateReceivers(monster, monsterStates).ToList();

            var phaseOneTransition = CreatePhaseOneTransition();
            result.Add(phaseOneTransition.eventReceiver);

            return result;
        }

        protected TransitionWrapper CreatePhaseOneTransition() {
            string fromPath = "GameLevel/Room/Prefab/EventBinder/General Boss Fight FSM Object Variant/--[States]/FSM/[State] BossFighting_Phase3";
            string toPath = "GameLevel/Room/Prefab/EventBinder/General Boss Fight FSM Object Variant/--[States]/FSM/[State] BossFighting_Phase1";

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
    }
}
