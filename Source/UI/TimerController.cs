using NineSolsAPI;
using BossChallengeMod.Helpers;
using RCGMaker.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace BossChallengeMod.UI {
    public class TimerController : MonoBehaviour {
        private TMP_Text timerText = null!;
        private TMP_FontAsset font = null!;

        private float animationShowDuration = 0.75f;
        private float animationHideDuration = 0.75f;
        private float hiddenYPosition;
        private float visibleYPosition;

        private UIAnimationStates state = UIAnimationStates.Hide;

        public void Awake() {
            font = LoadFont()!;
            timerText = UIController.InitializeText(
                Vector2.zero,
                gameObject,
                "00:00:000",
                font: font,
                alignment: TextAlignmentOptions.Center);
            visibleYPosition = GetVisiblePosition();
            hiddenYPosition = GetHiddenPosition();
            timerText.transform.localPosition = new Vector3(timerText.transform.localPosition.x, hiddenYPosition);
        }

        public void Show() {
            state = UIAnimationStates.Show;
            StartCoroutine(AnimateTimerShow());
        }

        public void Hide() {
            state = UIAnimationStates.Hide;
            StartCoroutine(AnimateTimerHide());
        }

        public void UpdateTimer(int milliseconds) {
            timerText.text = FormatMilliseconds(milliseconds);
        }

        private string FormatMilliseconds(int milliseconds) {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);

            string formattedTime = string.Format("{0:D2}:{1:D2}.{2:D3}",
            timeSpan.Minutes,
            timeSpan.Seconds,
            timeSpan.Milliseconds);

            return formattedTime;
        }

        private IEnumerator AnimateTimerShow() {
            if(state == UIAnimationStates.Show) {
                timerText.gameObject.SetActive(true);
                float elapsedTime = 0f;
                while (elapsedTime < animationShowDuration && state == UIAnimationStates.Show) {
                    elapsedTime += Time.unscaledDeltaTime;
                    var progress = elapsedTime / animationShowDuration;
                    float newPosition = Mathf.Lerp(hiddenYPosition, visibleYPosition, EasingFunctions.EaseOut(progress));
                    timerText.transform.localPosition = new Vector3(timerText.transform.localPosition.x, newPosition);

                    yield return null;
                }
            }
        }

        private IEnumerator AnimateTimerHide() {
            if (state == UIAnimationStates.Hide) {
                float elapsedTime = 0f;
                while (elapsedTime < animationHideDuration && state == UIAnimationStates.Hide) {
                    elapsedTime += Time.unscaledDeltaTime;
                    var progress = elapsedTime / animationShowDuration;
                    float newPosition = Mathf.Lerp(visibleYPosition, hiddenYPosition, EasingFunctions.EaseOut(progress));
                    timerText.transform.localPosition = new Vector3(timerText.transform.localPosition.x, newPosition);
                    
                    yield return null;
                }

                if (state == UIAnimationStates.Hide) {
                    timerText.gameObject.SetActive(false);
                }
            }
        }

        private TMP_FontAsset? LoadFont() {
            TMP_FontAsset[] allFonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
            foreach (TMP_FontAsset font in allFonts) {
                if (font.name == "LiberationSans SDF") return font;
            }

            return null;
        }

        private float GetVisiblePosition() {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float height = canvasRect.rect.height;
            float coordsX = height * 0.03f;

            return coordsX;
        }

        private float GetHiddenPosition() {
            float coordsY = -transform.position.y - timerText.preferredHeight;

            return coordsY;
        }

        private enum UIAnimationStates {
            Show,
            Hide
        }
    }
}
