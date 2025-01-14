using RCGFSM.Monster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.CustomActions {
    public class MonsterChangeStateActionDecorator : MonsterChangeStateAction {

        public MonsterBase.States targetStateId;

        protected override void OnStateEnterImplement() {
            base.Monster.ChangeStateIfValid(targetStateId);
            return;
        }
    }
}
