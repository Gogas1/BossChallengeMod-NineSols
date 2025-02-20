using BossChallengeMod.CustomMonsterStates;
using BossChallengeMod.Global;
using RCGFSM.Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.BossPatches.TargetPatches {
    public class EigongBossPatch : RevivalChallengeBossPatch {

        public override void PatchMonsterPostureSystem(MonsterBase monsterBase) {
            base.PatchMonsterPostureSystem(monsterBase);

            var flag = SaveManager.Instance.allFlags.FlagDict["e78958a13315eb9418325caf25da9d4dScriptableDataBool"];
            if (flag != null && flag is ScriptableDataBool boolFlag) {
                if(!boolFlag.CurrentValue) {
                    var postureSystem = monsterBase.postureSystem;
                    postureSystem.DieHandleingStates.Remove(MonsterBase.States.FooStunEnter);
                }
            }
        }
    }
}
