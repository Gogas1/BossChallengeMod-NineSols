using RCGMaker.AddressableAssets;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BossChallengeMod.UI {
    public class CurrentTalismanUIContoller : MonoBehaviour {
        private Image talismanImage = null!;

        public void Awake() {
            talismanImage = CreateImage();
            ValidateImage();
        }

        public void UpdateCurrentTalisman(Sprite sprite) {
            Player player = Player.i;

            talismanImage.sprite = sprite;
            ValidateImage();
        }

        private Image CreateImage() {
            GameObject gameObject = new GameObject("CurrentTalisman");
            gameObject.transform.SetParent(transform, false);
            var image = gameObject.AddComponent<Image>();
            image.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1f);

            return image;
        }

        private Sprite GetTalismanSprite(AbilityWrapper style, AbilityWrapper upgrade, Sprite baseSprite, Sprite upgradedSprite) {
            if (style.IsActivated) {
                return upgrade.IsActivated ? upgradedSprite : baseSprite;
            }
            return null;
        }

        private void ValidateImage() {
            talismanImage.enabled = talismanImage.sprite != null;
        }
    }
}
