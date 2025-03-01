using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterBombController : MonoBehaviour {
        private GameObject? shooterObject;
        private FxPlayer? shooterComponent;

        private MonsterBase Monster { get; set; } = null!;

        private List<GameObject> spawnedBomds = new List<GameObject>();

        private void Awake() {
            Monster = GetComponent<MonsterBase>();

            shooterObject = BossChallengeMod.Instance.BombShooterProvider.GetBombShooterCopy();

            if (shooterObject == null || Monster == null) {
                return;
            }

            shooterComponent = shooterObject.GetComponent<FxPlayer>();

            if(shooterComponent == null) {
                return;
            }

            shooterObject.transform.SetParent(Monster.transform);
            AutoAttributeManager.AutoReferenceAllChildren(shooterObject);
        }

        public void DeactivateBombs() {
            foreach (var item in spawnedBomds) {
                GameObject.Destroy(item);
            }
        }

        public void PlaceBombAtPlayer() {
            var playerPos = Player.i.transform.position;
            spawnedBomds.RemoveAllNull();
            var spawnedBomb = shooterComponent?.PlayCustomAt(new Vector3(playerPos.x, playerPos.y + 20, playerPos.z));
            if (spawnedBomb != null) {
                spawnedBomds.Add(spawnedBomb);
            }
        }

        public void PlaceBombAt(Vector3 place) {
            spawnedBomds.RemoveAllNull();
            var spawnedBomb = shooterComponent?.PlayCustomAt(place);
            if (spawnedBomb != null) {
                spawnedBomds.Add(spawnedBomb);
            }
        }
    }
}
