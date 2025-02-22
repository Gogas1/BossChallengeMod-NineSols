using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class CooldownBombModifier : BombModifier {
        private float timer = 9f;

        protected override void Update() {
            base.Update();

            var postureSystem = Monster?.postureSystem ?? null;
            if (postureSystem != null &&
                !postureSystem.IsMonsterEmptyPosture &&
                !IsPaused &&
                !Player.i.lockMoving &&
                !Player.i.freeze) {

                timer -= Time.deltaTime;

                if (timer <= 0) {
                    var playerCenterPos = new Vector2(Player.i.transform.position.x, Player.i.transform.position.y + 20f);
                    playerCenterPos = playerCenterPos + Player.i.Velocity;

                    BombController?.PlaceBombAt(playerCenterPos);

                    timer = BossChallengeMod.Random.Next(9, 13);
                }
            }
        }
    }
}
