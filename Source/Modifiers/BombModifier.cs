using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class BombModifier : ModifierBase {

        protected MonsterBombController? BombController = null;
        protected int BombCount = 1;
        protected float BombPlacementCooldown = 0.4f;

        public override void NotifyActivation(int iteration) {
            base.NotifyActivation(iteration);

            enabled = true;
        }

        public override void NotifyDeactivation(int iteration) {
            base.NotifyDeactivation(iteration);

            enabled = false;
        }

        protected virtual void Update() {
            var postureSystem = Monster?.postureSystem ?? null;
            if (postureSystem != null &&
                !postureSystem.IsMonsterEmptyPosture &&
                !IsPaused &&
                !Player.i.lockMoving &&
                !Player.i.freeze) {

                BombController?.DeactivateBombs();
            }
        }

        public override void SetController(Component controllerComponent) {
            if(controllerComponent is MonsterBombController bombController) {
                BombController = bombController;
            }
        }

        protected void SpawnAtPlayer() {
            if (enabled && !IsPaused) {
                StartCoroutine(BombPlacementTask(BombCount, BombPlacementCooldown));
            }
        }

        protected IEnumerator BombPlacementTask(int count, float cooldown) {
            for (int i = 0; i < count; i++) {
                BombController?.PlaceBombAtPlayer();
                yield return new WaitForSeconds(cooldown);
            }
        }
    }
}
