using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.BossPatches.TargetPatches {
    public class GoumangBossPatch : RevivalChallengeBossPatch {

        protected override MonsterModifierController InitializeModifiers(MonsterBase monsterBase, ChallengeMonsterController monsterController) {
            var controller = base.InitializeModifiers(monsterBase, monsterController);

            if (IsModEnabled && ConfigurationToUse.IsModifiersEnabled) {
                controller.RollOnStart = true;

                var spearZombiePath = "A3_S5_BossGouMang_GameLevel/Room/StealthGameMonster_BossZombieSpear";
                var hammerZombiePath = "A3_S5_BossGouMang_GameLevel/Room/StealthGameMonster_BossZombieHammer";

                var spearZombie = GameObject.Find(spearZombiePath).GetComponent<StealthGameMonster>();
                var hammerZombie = GameObject.Find(hammerZombiePath).GetComponent<StealthGameMonster>();

                var spearZombieModifiers = InitModifiers(spearZombie, controller, ConfigurationToUse);
                var hammerZombieModifiers = InitModifiers(hammerZombie, controller, ConfigurationToUse);

                controller.MustIncludeModifiers.AddRange(spearZombieModifiers);
                controller.MustIncludeModifiers.AddRange(hammerZombieModifiers);
            }

            return controller;
        }
    }
}
