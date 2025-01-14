using NineSolsAPI;
using BossChallengeMod.Global;
using BossChallengeMod.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BossChallengeMod.UI {
    public class KillCounterController : MonoBehaviour {
        private TMP_FontAsset font = null!;
        private LocalizationResolver localizer = BossChallengeMod.Instance.LocalizationResolver;

        private bool needToShow;
        private bool needToShowExpanded;
        public UIControllerAnimationState UIControllerAnimationState { get; set; }

        public TMP_Text MainCounterText = null!;
        public TMP_Text HightScoreText = null!;
        public TMP_Text LastAttemptText = null!;

        public float labelSpacing = 10f;
        public float animationChangeDuration = 1.0f;

        public float labelTextAnimationShowDuration = 1.0f;
        public float labelTextAnimationHideDuration = 1.0f;
        public float labelTextDefaultAlpha = 1.0f;

        public float expandedTextAnimationShowDuration = 1.0f;
        public float expandedTextAnimationHideDuration = 1.0f;
        public float expandedTextDefaultAlpha = 1.0f;

        public void Awake() {
            font = LoadFont()!;

            MainCounterText = UIController.InitializeText(Vector2.zero, gameObject, $"{localizer.Localize("kill_counter")} {0}" , false, font: font);
            HightScoreText = UIController.InitializeText(new Vector2(0f, -36), gameObject, $"{localizer.Localize("kill_counter_hs")} {0}", false, 20, font);
            LastAttemptText = UIController.InitializeText(new Vector2(0f, -56), gameObject, $"{localizer.Localize("kill_counter_last")} {0}", false, 20, font);
            labelTextDefaultAlpha = MainCounterText.alpha;
            expandedTextDefaultAlpha = HightScoreText.alpha;
            MainCounterText.alpha = 0;
        }

        public void ChangeNumber(int number) {
            MainCounterText.text = $"{localizer.Localize("kill_counter")} {number.ToString()}";
        }        

        public void Show() {
            needToShow = true;
            StartCoroutine(AnimateTextShow(
                    MainCounterText,
                    needToShow,
                    labelTextDefaultAlpha,
                    labelTextAnimationShowDuration,
                    EasingFunctions.Linear));
        }

        public void Hide(Action? callback = null) {
            needToShow = false;
            StartCoroutine(AnimateTextHide(
                    MainCounterText,
                    needToShow,
                    labelTextDefaultAlpha,
                    labelTextAnimationHideDuration,
                    EasingFunctions.Linear,
                    callback));
        }

        public void UpdateHighScore(int number) {
            HightScoreText.text = $"{localizer.Localize("kill_counter_hs")} {number.ToString()}";
        }
        public void UpdatePrevResult(int number) {
            LastAttemptText.text = $"{localizer.Localize("kill_counter_last")} {number.ToString()}";
        }

        public void ShowExtraText(Action? callback = null) {
            if(UIControllerAnimationState == UIControllerAnimationState.ModifiersLowered) {
                needToShowExpanded = true;
                StartCoroutine(AnimateTextShow(
                        HightScoreText,
                        needToShowExpanded,
                        expandedTextDefaultAlpha,
                        expandedTextAnimationShowDuration,
                        EasingFunctions.Linear,
                        callback: callback));
                StartCoroutine(AnimateTextShow(
                        LastAttemptText,
                        needToShowExpanded,
                        expandedTextDefaultAlpha,
                        expandedTextAnimationShowDuration,
                        EasingFunctions.Linear));
            }
        }

        public void HideExtraText(Action? callback = null) {
            if(UIControllerAnimationState == UIControllerAnimationState.KillCounterExpanded) {
                needToShowExpanded = false;
                StartCoroutine(AnimateTextHide(
                        HightScoreText,
                        needToShowExpanded,
                        expandedTextDefaultAlpha,
                        expandedTextAnimationHideDuration,
                        EasingFunctions.Linear,
                        callback: callback));
                StartCoroutine(AnimateTextHide(
                        LastAttemptText,
                        needToShowExpanded,
                        expandedTextDefaultAlpha,
                        expandedTextAnimationHideDuration,
                        EasingFunctions.Linear));
            }
        }

        private IEnumerator AnimateTextShow(
            TMP_Text text,
            bool flag,
            float defaultAlpha,
            float showDuration,
            Func<float, float> easingFunction,
            Action? callback = null) {
            if(flag) {
                text.gameObject.SetActive(true);
                float startingAlpha = text.alpha;
                float startingProgress = startingAlpha / defaultAlpha;
                float elapsedTime = showDuration * startingProgress;
                while (flag && elapsedTime < showDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    float progress = elapsedTime / showDuration;
                    text.alpha = Mathf.Lerp(startingAlpha, defaultAlpha, easingFunction(progress));

                    yield return null;
                }

                if (flag) {
                    callback?.Invoke();
                }
            }
        }

        private IEnumerator AnimateTextHide(
            TMP_Text text,
            bool flag,
            float defaultAlpha,
            float hideDuration,
            Func<float, float> easingFunction,
            Action? callback = null) {
            if (!flag) {
                float startingAlpha = text.alpha;
                float startingProgress = 1.0f - startingAlpha / defaultAlpha;
                float elapsedTime = hideDuration * startingProgress;
                while (!flag && elapsedTime < hideDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    float progress = elapsedTime / hideDuration;

                    text.alpha = Mathf.Lerp(startingAlpha, 0, easingFunction(progress));

                    yield return null;
                }

                if (!flag) {
                    text.gameObject.SetActive(false);
                    callback?.Invoke();
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
    }
}
