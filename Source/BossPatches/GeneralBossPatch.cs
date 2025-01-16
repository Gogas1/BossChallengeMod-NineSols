using HarmonyLib;
using MonsterLove.StateMachine;
using BossChallengeMod.CustomMonsterStates;
using BossChallengeMod.Global;
using BossChallengeMod.Modifiers;
using BossChallengeMod.UI;
using BossChallengeMod.CustomActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using BossChallengeMod.CustomMonsterStates.Configuration;

namespace BossChallengeMod.BossPatches {
    public class GeneralBossPatch {
        public List<MonsterBase.States> DieStates = new List<MonsterBase.States>();

        protected EventTypesResolver eventTypesResolver = BossChallengeMod.Instance.EventTypesResolver;

        protected readonly FieldInfo monsterStatesFieldRef = AccessTools.Field(typeof(MonsterBase), "monsterStatesRefs");
        protected readonly FieldInfo stateLookupsFieldRef = AccessTools.Field(typeof(StateMachine<MonsterBase.States>), "stateLookup");

        protected readonly FieldInfo eventBinderSendersArrayRef = AccessTools.Field(typeof(RCGArgEventBinder), "_senderRefs");
        protected readonly FieldInfo eventBinderReceiversArrayRef = AccessTools.Field(typeof(RCGArgEventBinder), "_receiverRefs");

        protected readonly FieldInfo transitionReceiverIReceiversRef = AccessTools.Field(typeof(RCGEventReceiver), "iReceivers");

        protected readonly FieldInfo transitionCullingGroupRef = AccessTools.Field(typeof(RCGEventReceiveTransition), "_cullingGroup");
        protected readonly FieldInfo transitionParentRef = AccessTools.Field(typeof(RCGEventReceiveTransition), "parentState");


        public virtual void PatchMonsterPostureSystem(MonsterBase monsterBase) {
            var postureSystem = monsterBase.postureSystem;
            postureSystem.DieHandleingStates.Clear();
            postureSystem.DieHandleingStates.AddRange(DieStates);
        }

        public virtual IEnumerable<MonsterState> PatchMonsterStates(MonsterBase monsterBase) 
        {
            return new List<MonsterState>();
        }


        public virtual IEnumerable<RCGEventSender> CreateSenders(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = new List<RCGEventSender>();

            return result;
        }


        public virtual IEnumerable<RCGEventReceiver> CreateReceivers(MonsterBase monster, IEnumerable<MonsterState> monsterStates) {
            var result = new List<RCGEventReceiver>();
            
            return result;
        }


        public virtual void ProcessEventHandlers(IEnumerable<RCGEventReceiver> receivers, IEnumerable<RCGEventSender> senders) {
            Bind(senders, receivers);
        }


        public virtual void PatchMonsterFsmLookupStates(MonsterBase monsterBase, IEnumerable<MonsterState> states) 
        {
            var stateMachine = monsterBase.fsm;
            var lookUpDictionary = (Dictionary<MonsterBase.States, StateMapping<MonsterBase.States>>)stateLookupsFieldRef.GetValue(stateMachine);

            foreach (var state in states) {
                var stateType = state.GetStateType();

                if (lookUpDictionary.ContainsKey(stateType)) {
                    Log.Error($"{stateType} of {state} of name {state.gameObject.name} is registered in the state machine");
                    continue;
                }

                StateMapping<MonsterBase.States> stateMapping = new StateMapping<MonsterBase.States>(stateType);
                ResolveStateMappingEvents(stateMapping, state);

                lookUpDictionary.Add(stateType, stateMapping);
            }
        }


        public virtual void PostfixPatch(MonsterBase monster) {

        }       

        protected MonsterState InstantiateStateObject(GameObject monsteGameObject, Type type, string name, StateConfiguration stateConfiguration) {
            GameObject parent = monsteGameObject.transform.Find("States").gameObject;
            var component = parent.AddChildrenComponent(type, name);

            if (component is ResetBossState monsterState) {
                stateConfiguration.ConfigureComponent(component);
                return monsterState;
            } 
            else {
                GameObject.Destroy(component);
                return null;
            }
        }

        protected void ResolveStateMappingEvents(StateMapping<MonsterBase.States> stateMapping, MonsterState state) {
            stateMapping.hasEnterRoutine = false;
            stateMapping.EnterCall = () => {
                state.ResolveProxy().OnStateEnter();
                if (state.ResolveProxy().stateEvents.StateEnterEvent != null) {
                    state.ResolveProxy().stateEvents.StateEnterEvent.Invoke();
                }
            };
            stateMapping.hasExitRoutine = false;
            stateMapping.ExitCall = () => {
                state.ResolveProxy().OnStateExit();
                if (state.ResolveProxy().stateEvents.StateExitEvent != null) {
                    state.ResolveProxy().stateEvents.StateExitEvent.Invoke();
                }
            };
            stateMapping.Finally = () => {
                state.ResolveProxy().OnStateFinally();
            };
            stateMapping.Update = () => {
                state.ResolveProxy().OnStateUpdate();
            };
            stateMapping.SpriteUpdate = () => {
                state.ResolveProxy().OnSpriteUpdate();
            };
            stateMapping.LateUpdate = () => {
                state.ResolveProxy().OnStateLateUpdate();
            };
            stateMapping.FixedUpdate = () => {
                state.ResolveProxy().OnStateFixedUpdate();
            };
            stateMapping.OnCollisionEnter = (Collision c) => {
                state.ResolveProxy().OnStateCollisionEnter(c);
            };
        }


        protected void BindSendAndRec(RCGEventSender sender, RCGEventReceiver receiver) {
            if (sender.eventType == receiver.eventType) {
                sender.bindReceivers.Add(receiver);
                receiver.SenderCandidates.Add(sender);
            }
        }


        protected void Bind(IEnumerable<RCGEventSender> senders, IEnumerable<RCGEventReceiver> receivers) {
            foreach (RCGEventSender sender in senders) {
                foreach (RCGEventReceiver receiver in receivers) {
                    BindSendAndRec(sender, receiver);
                }
            }
        }


        protected RCGEventSender CreateEventSender(GameObject parent, RCGEventType eventType, UnityEvent invoker, string? name = null) {
            var sender = parent.AddChildrenComponent<RCGEventSender>(name ?? "[Sender] newEventSender");
            sender.eventType = eventType;
            invoker.AddListener(() => {
                sender.Send();
            });
            return sender;
        }


        protected RCGEventReceiver CreateEventReceiver(GameObject parent, RCGEventType eventType, IEnumerable<IRCGArgEventReceiver> subscribers, string name = "[Receiver] newEventReceiver") {
            var receiver = parent.AddChildrenComponent<RCGEventReceiver>(name);
            receiver.eventType = eventType;
            var receiverSubscribers = new List<IRCGArgEventReceiver>((IRCGArgEventReceiver[])transitionReceiverIReceiversRef.GetValue(receiver));
            receiverSubscribers.AddRange(subscribers);
            transitionReceiverIReceiversRef.SetValue(receiver, receiverSubscribers.ToArray());

            return receiver;
        }


        protected RCGEventReceiver CreateEventReceiverAsComponent(GameObject parent, RCGEventType eventType, IEnumerable<IRCGArgEventReceiver> subscribers) {
            var receiver = parent.AddComponent<RCGEventReceiver>();
            receiver.eventType = eventType;
            var receiverSubscribers = new List<IRCGArgEventReceiver>(((IRCGArgEventReceiver[])transitionReceiverIReceiversRef.GetValue(receiver)) ?? []);
            receiverSubscribers.AddRange(subscribers);
            transitionReceiverIReceiversRef.SetValue(receiver, receiverSubscribers.ToArray());
            
            return receiver;
        }

        protected RCGEventReceiveTransition CreateTransition(GameObject parent, GeneralState stateFrom, GeneralState stateTo, string name = "[Patch Transition] New transition") {
            var transitionEventTransition = parent.AddChildrenComponent<RCGEventReceiveTransition>(name);

            var cullingGroup = stateTo.Context.fsmOwner.GetComponent<RCGCullingGroup>();
            if (cullingGroup != null) {
                transitionCullingGroupRef.SetValue(transitionEventTransition, cullingGroup);
            }
            transitionParentRef.SetValue(transitionEventTransition, stateFrom);

            transitionEventTransition.target = stateTo;

            return transitionEventTransition;
        }
    }
}
