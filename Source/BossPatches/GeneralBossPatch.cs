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
                Log.Error("");
                return null;
            }
        }

        protected void ResolveStateMappingEvents(StateMapping<MonsterBase.States> stateMapping, MonsterState state) {
            stateMapping.hasEnterRoutine = false;
            stateMapping.EnterCall = () => {
                try {
                    state.ResolveProxy().OnStateEnter();
                    if (state.ResolveProxy().stateEvents.StateEnterEvent != null) {
                        state.ResolveProxy().stateEvents.StateEnterEvent.Invoke();
                    }

                } catch (Exception ex) {
                    Log.Error($"ResolveStateMappingEvents exception {ex.Message} \n{ex.StackTrace}");
                }
            };
            stateMapping.hasExitRoutine = false;
            stateMapping.ExitCall = () => {
                try {
                    state.ResolveProxy().OnStateExit();
                    if (state.ResolveProxy().stateEvents.StateExitEvent != null) {
                        state.ResolveProxy().stateEvents.StateExitEvent.Invoke();
                    }

                } catch (Exception ex) {
                    Log.Error($"ExitCall exception {ex.Message} \n{ex.StackTrace}");
                }
            };
            stateMapping.Finally = () => {
                try {
                    state.ResolveProxy().OnStateFinally();
                } catch (Exception ex) {
                    Log.Error($"Finally exception {ex.Message} \n{ex.StackTrace}");
                }
            };
            stateMapping.Update = () => {
                try {
                    state.ResolveProxy().OnStateUpdate();
                } catch (Exception ex) {
                    Log.Error($"Update exception {ex.Message} \n{ex.StackTrace}");
                }
            };
            stateMapping.SpriteUpdate = () => {
                try {
                    state.ResolveProxy().OnSpriteUpdate();
                } catch (Exception ex) {
                    Log.Error($"SpriteUpdate exception {ex.Message} \n{ex.StackTrace}");
                }
            };
            stateMapping.LateUpdate = () => {
                try {
                    state.ResolveProxy().OnStateLateUpdate();
                } catch (Exception ex) {
                    Log.Error($"LateUpdate exception {ex.Message} \n{ex.StackTrace}");
                }
            };
            stateMapping.FixedUpdate = () => {
                try {
                    state.ResolveProxy().OnStateFixedUpdate();
                } catch (Exception ex) {
                    Log.Error($"FixedUpdate exception {ex.Message} \n{ex.StackTrace}");
                }
            };
            stateMapping.OnCollisionEnter = (Collision c) => {
                try {
                    state.ResolveProxy().OnStateCollisionEnter(c);
                } catch (Exception ex) {
                    Log.Error($"OnCollisionEnter exception {ex.Message} \n{ex.StackTrace}");
                }
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
