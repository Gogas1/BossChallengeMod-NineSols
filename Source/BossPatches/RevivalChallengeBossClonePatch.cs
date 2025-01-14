using MonsterLove.StateMachine;
using BossChallengeMod.CustomMonsterStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.BossPatches {
    public class RevivalChallengeBossClonePatch : GeneralBossPatch {

        public override void PatchMonsterFsmLookupStates(MonsterBase monsterBase, IEnumerable<MonsterState> states) {
            var stateMachine = monsterBase.fsm;
            var lookUpDictionary = (Dictionary<MonsterBase.States, StateMapping<MonsterBase.States>>)stateLookupsFieldRef.GetValue(stateMachine);

            var resetBossState = monsterBase.GetComponentsInChildren<ResetBossState>();

            foreach (var state in resetBossState) {
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

    }
}
