using HarmonyLib;
using BossChallengeMod.Configuration;
using BossChallengeMod.Global;
using BossChallengeMod.KillCounting;
using RCGMaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BossChallengeMod.CustomMonsterStates {
    public class ResetBossState : MonsterState {

        protected ChallengeConfiguration challengeConfig;
        public MonsterKillCounter monsterKillCounter = null!;

        public Queue<string> AnimationsQueue;
        public string[] Animations { get; set; }
        public string[] TargetDamageReceivers { get; set; }
        public MonsterBase.States StateType { get; set; }

        public ResetBossState() {

            TargetDamageReceivers = ["Attack", "Foo", "JumpKick"];
            Animations = [];

            AnimationsQueue = new Queue<string>();
            exitState = MonsterBase.States.Engaging;
            stateEvents = new StateEvents() {
                StateEnterEvent = new UnityEvent(),
                StateExitEvent = new UnityEvent(),
            };
        }

        public override void OnStateEnter() {
            if (monsterKillCounter != null && challengeConfig.MaxBossCycles == monsterKillCounter.KillCounter + 1) {
                EffectHitData effectHitData = new EffectHitData();
                effectHitData.Override(Player.i.normalAttackDealer, monster.postureSystem.decreasePostureReceiver, null);
                monster.HittedByPlayerDecreasePosture(effectHitData);

                return;
            }

            ResetAnimationQueue();
            monster.UnFreeze();
            monster.HurtClearHintFxs();
            monster.monsterCore.DisablePushAway();
            SwitchDamageReceivers(false);
            for (int i = 0; i < base.monster.attackSensors.Length; i++) {
                if (base.monster.attackSensors[i] != null) {
                    base.monster.attackSensors[i].ClearQueue();
                }
            }
            base.monster.VelX = 0f;
            if (AnimationsQueue.Any()) {
                PlayAnimation(AnimationsQueue.Dequeue(), false);
            } else {
                End();
            }

            ResetInitialAttacks(monster.monsterCore.attackSequenceMoodule);
            ResetMustEnqAttacks(monster);
            ResetSequenceManagersAttacks(monster);
        }

        private void End() {
            if (monster.fsm.HasState(exitState)) {
                monster.ChangeStateIfValid(exitState);
            }
        }

        public override void OnStateExit() {
            if (monsterKillCounter != null && challengeConfig.MaxBossCycles == monsterKillCounter.KillCounter)
                return;

            monster.PhaseIndex = 0;
            monster.animator.SetInteger(ResetBossState.PhaseIndexHash, base.monster.PhaseIndex);
            monster.postureSystem.RestorePosture();
            SwitchDamageReceivers(true);
            monster.monsterCore.EnablePushAway();
            monster.postureSystem.GenerateCurrentDieHandleStacks();
        }

        public override void AnimationEvent(AnimationEvents.AnimationEvent e) {
            if (e == AnimationEvents.AnimationEvent.Done) {
                if (AnimationsQueue.Any()) {
                    PlayAnimation(AnimationsQueue.Dequeue(), true);
                } else {
                    End();
                }
            }
        }

        public override MonsterBase.States GetStateType() {
            if (this.stateTypeScriptable != null) {
                return this.stateTypeScriptable.overrideStateType;
            }

            return StateType;
        }

        public void AssignMonster(MonsterBase monster) {
            _monster = monster;
        }

        public void AssignChallengeConfig(ChallengeConfiguration config) {
            challengeConfig = config;
        }

        private void ResetAnimationQueue() {
            AnimationsQueue.Clear();
            foreach (var item in Animations) {
                AnimationsQueue.Enqueue(item);
            }
        }

        private void ResetInitialAttacks(AttackSequenceModule attackSequenceModule) {
            var phasesAttacks = GetPhaseSequences(attackSequenceModule);
            if (phasesAttacks == null) {
                return;
            }

            foreach (var item in phasesAttacks) {
                item.EnterLevelReset();
            }
        }

        private void ResetMustEnqAttacks(MonsterBase monsterBase) {
            foreach (var attackSensor in monsterBase.attackSensors) {
                foreach (var item in attackSensor.AttackWeightPhaseList) {
                    item.EnterLevelReset();
                }
            }
        }

        private void ResetSequenceManagersAttacks(MonsterBase monsterBase) {
            Transform parent = monsterBase.transform.parent;
            var sequenceManagers = parent.gameObject.GetComponentsInChildren<EventSequenceManager>(true);
            foreach (var item in sequenceManagers) {
                item.setting.EnterLevelReset();
            }
        }

        private MonsterStateSequenceWeight[]? GetPhaseSequences(AttackSequenceModule attackSequenceModule) {
            var phasesSequencesRef = AccessTools.Field(typeof(AttackSequenceModule), "SequenceForDifferentPhase");
            if (phasesSequencesRef != null) {
                var phasesSequences = (MonsterStateSequenceWeight[])phasesSequencesRef.GetValue(attackSequenceModule);
                return phasesSequences;
            }

            return null;
        }

        private void SwitchDamageReceivers(bool state) {
            var damageReceivers = monster.damageReceivers;
            if (damageReceivers == null) return;

            foreach (var effect in damageReceivers
                         .SelectMany(item => item.GetComponentsInChildren<EffectReceiver>(true))
                         .Where(effect => TargetDamageReceivers.Contains(effect.gameObject.name))) {
                effect.gameObject.SetActive(state);
            }
        }

        private static readonly int PhaseIndexHash = Animator.StringToHash("PhaseIndex");
    }
}
