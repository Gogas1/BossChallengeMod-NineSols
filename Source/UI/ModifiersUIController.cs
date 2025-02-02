using ClipperLibClone;
using BossChallengeMod.Global;
using BossChallengeMod.Helpers;
using BossChallengeMod.Modifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BossChallengeMod.UI {
    public class ModifiersUIController : MonoBehaviour {
        private RectTransform line = null!;
        private List<string> modifiers = new List<string>();
        private List<TMP_Text> items = new List<TMP_Text>();

        private LocalizationResolver localizer = BossChallengeMod.Instance.LocalizationResolver;

        public float lineDefaultLength = 200f;
        public float lineUnfoldAnimationDuration = 0.75f;
        public float lineFoldAnimationDuration = 0.75f;

        public float modifierTextAnimationShowDuration = 1.0f;
        public float modifierTextAnimationHideDuration = 1.0f;
        public float modifierTextDefaultAlpha = 0.79f;

        private float modifiersDefaultHeight = -21f;
        private float modifiersLoweredHeight = -66f;
        private float modifiersRaisingAnimationDuration = 1f;
        private float modifiersLoweringAnimationDuration = 1f;

        private bool forceHide;

        public UIControllerAnimationState UIControllerAnimationState;

        private ModifiersUIState currentState;
        private ModifiersUIState CurrentState {
            get => currentState;
            set {
                switch (value) {
                    case ModifiersUIState.Hidden:
                        currentState = value;
                        break;
                    case ModifiersUIState.UnfoldingLine:
                        currentState = value;
                        break;
                    case ModifiersUIState.LineUnfolded:
                        currentState = value;
                        if (modifiers.Any()) {
                            StartCoroutine(UnfoldModifiers());
                        }
                        else {
                            StartCoroutine(CollapseLineAnimation());
                        }
                        break;
                    case ModifiersUIState.UnfoldingModifiers:
                        currentState = value;
                        break;
                    case ModifiersUIState.ModifiersUnfolded:
                        currentState = value;
                        break;
                    case ModifiersUIState.CollapsingModifiers:
                        currentState = value;
                        break;
                    case ModifiersUIState.ModifiersCollapsed:
                        currentState = value;
                        if (modifiers.Any() && !forceHide) {
                            StartCoroutine(UnfoldModifiers());
                        } else {
                            StartCoroutine(CollapseLineAnimation());
                        }
                        break;
                    case ModifiersUIState.CollapsingLine:
                        currentState = value;
                        break;
                    default:
                        currentState = value;
                        break;
                }
            }
        }


        public void Awake() {
            line = CreateLine();
            line.gameObject.SetActive(false);
        }

        public void Show(bool force = false) {
            if (!modifiers.Any() && !force) 
                return;            

            forceHide = false;
            switch (CurrentState) {
                case ModifiersUIState.Hidden:
                    StartCoroutine(UnfoldLineAnimation());
                    break;
                case ModifiersUIState.UnfoldingLine:
                    break;
                case ModifiersUIState.LineUnfolded:
                    break;
                case ModifiersUIState.UnfoldingModifiers:
                    break;
                case ModifiersUIState.ModifiersUnfolded:
                    break;
                case ModifiersUIState.CollapsingModifiers:
                    break;
                case ModifiersUIState.ModifiersCollapsed:
                    break;
                case ModifiersUIState.CollapsingLine:
                    StartCoroutine(UnfoldLineAnimation());
                    break;
                default:
                    break;
            }
        }

        public void Hide(bool force = false) {
            forceHide = force;
            switch (CurrentState) {
                case ModifiersUIState.Hidden:
                    break;
                case ModifiersUIState.UnfoldingLine:
                    CurrentState = ModifiersUIState.CollapsingLine;
                    break;
                case ModifiersUIState.LineUnfolded:
                    StartCoroutine(CollapseLineAnimation());
                    break;
                case ModifiersUIState.UnfoldingModifiers:
                    CurrentState = ModifiersUIState.CollapsingModifiers;
                    break;
                case ModifiersUIState.ModifiersUnfolded:
                    StartCoroutine(CollapseModifiers());
                    break;
                case ModifiersUIState.CollapsingModifiers:
                    break;
                case ModifiersUIState.ModifiersCollapsed:
                    break;
                case ModifiersUIState.CollapsingLine:
                    break;
                default:
                    break;
            }
        }

        public void SetModifiers(IEnumerable<string> modifiers) {
            this.modifiers.Clear();
            this.modifiers.AddRange(modifiers);
            Hide();
        }

        public void Reset() {
            SetModifiers([]);
        }

        public void RaiseModifiers(Action? callback = null) {
            StartCoroutine(AnimateModifiersRaising(callback));
        }

        public void LowerModifiers(Action? callback = null) {
            StartCoroutine(AnimateModifiersLowering(callback));
        }

        private IEnumerator UnfoldModifiers() {
            if(CurrentState == ModifiersUIState.LineUnfolded || 
                CurrentState == ModifiersUIState.ModifiersCollapsed || 
                CurrentState == ModifiersUIState.CollapsingModifiers) {

                CurrentState = ModifiersUIState.UnfoldingModifiers;

                var modifiersQueue = new Queue<string>(modifiers);
                int counter = 1;

                while(items.Any()) {
                    yield return new WaitForSecondsRealtime(0.1f);
                }

                while (modifiersQueue.Any() && CurrentState == ModifiersUIState.UnfoldingModifiers) {
                    var modifier = modifiersQueue.Dequeue();
                    var newText = UIController.InitializeText(new Vector2(0, -31 * counter), gameObject, localizer.Localize(modifier), true, 21);
                    counter++;
                    newText.alpha = 0f;
                    items.Add(newText);
                    StartCoroutine(AnimateModifierShow(newText, !modifiersQueue.Any()));
                    yield return new WaitForSecondsRealtime(0.33f);
                }
            }
        }

        private IEnumerator CollapseModifiers() {
            if(CurrentState == ModifiersUIState.ModifiersUnfolded || CurrentState == ModifiersUIState.UnfoldingModifiers) {
                CurrentState = ModifiersUIState.CollapsingModifiers;

                while (items.Any()) {
                    var modifier = items.Pop();
                    StartCoroutine(AnimateModifierHide(modifier, !items.Any()));
                    yield return new WaitForSecondsRealtime(0.33f);
                }
            }
        }

        private IEnumerator UnfoldLineAnimation() {
            if(CurrentState == ModifiersUIState.Hidden || CurrentState == ModifiersUIState.CollapsingLine) {
                CurrentState = ModifiersUIState.UnfoldingLine;

                line.gameObject.SetActive(true);

                float startingLength = line.sizeDelta.x;
                var startingProgress = startingLength / lineDefaultLength;
                float elapsedTime = lineUnfoldAnimationDuration * startingProgress;
                while (CurrentState == ModifiersUIState.UnfoldingLine && elapsedTime < lineUnfoldAnimationDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    float progress = elapsedTime / lineUnfoldAnimationDuration;
                    float newLength = Mathf.Lerp(startingLength, lineDefaultLength, EasingFunctions.EaseOut(progress));
                    line.sizeDelta = new Vector2(newLength, line.sizeDelta.y);
                    
                    yield return null;
                }

                if(CurrentState == ModifiersUIState.UnfoldingLine) {
                    CurrentState = ModifiersUIState.LineUnfolded;
                }
            }
        }

        private IEnumerator CollapseLineAnimation() {
            if (CurrentState == ModifiersUIState.LineUnfolded || 
                CurrentState == ModifiersUIState.UnfoldingLine ||
                CurrentState == ModifiersUIState.ModifiersCollapsed) {
                CurrentState = ModifiersUIState.CollapsingLine;

                float startingLength = line.sizeDelta.x;
                var startingProgress = 1.0f - startingLength / lineDefaultLength;
                float elapsedTime = lineFoldAnimationDuration * startingProgress;
                while (CurrentState == ModifiersUIState.CollapsingLine && elapsedTime < lineFoldAnimationDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    float progress = elapsedTime / lineFoldAnimationDuration;
                    float newLength = Mathf.Lerp(startingLength, 0, EasingFunctions.EaseOut(progress));
                    line.sizeDelta = new Vector2(newLength, line.sizeDelta.y);

                    yield return null;
                }

                if(CurrentState == ModifiersUIState.CollapsingLine) {
                    CurrentState = ModifiersUIState.Hidden;
                }
            }
        }

        private IEnumerator AnimateModifierShow(TMP_Text textItem, bool last = false) {
            if (CurrentState == ModifiersUIState.UnfoldingModifiers) {
                textItem.gameObject.SetActive(true);

                float startingAlpha = textItem.alpha;
                float startingProgress = startingAlpha / modifierTextDefaultAlpha;
                float elapsedTime = modifierTextAnimationShowDuration * startingProgress;
                while (CurrentState == ModifiersUIState.UnfoldingModifiers && elapsedTime < modifierTextAnimationShowDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    float progress = elapsedTime / modifierTextAnimationShowDuration;
                    textItem.alpha = Mathf.Lerp(startingAlpha, modifierTextDefaultAlpha, progress);

                    yield return null;
                }

                if(CurrentState == ModifiersUIState.UnfoldingModifiers && last) {
                    CurrentState = ModifiersUIState.ModifiersUnfolded;
                }
            }
        }

        private IEnumerator AnimateModifierHide(TMP_Text textItem, bool last = false) {
            float startingAlpha = textItem.alpha;
            float startingProgress = 1.0f - startingAlpha / modifierTextDefaultAlpha;
            float elapsedTime = modifierTextAnimationHideDuration * startingProgress;
            while (elapsedTime < modifierTextAnimationHideDuration) {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / modifierTextAnimationHideDuration;

                textItem.alpha = Mathf.Lerp(startingAlpha, 0, progress);

                yield return null;
            }

            if(CurrentState == ModifiersUIState.CollapsingModifiers && last) {
                CurrentState = ModifiersUIState.ModifiersCollapsed;
            }

            GameObject.Destroy(textItem.gameObject);
        }

        private IEnumerator AnimateModifiersLowering(Action? callback = null) {
            if (UIControllerAnimationState == UIControllerAnimationState.KillCounterCollapsed) {
                float elapsedTime = 0f;
                while (elapsedTime < modifiersLoweringAnimationDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    var progress = elapsedTime / modifiersLoweringAnimationDuration;
                    var newPosition = new Vector3(
                        transform.localPosition.x,
                        Mathf.Lerp(modifiersDefaultHeight, modifiersLoweredHeight, EasingFunctions.EaseOut(progress)));

                    transform.localPosition = newPosition;

                    yield return null;
                }

                if (UIControllerAnimationState == UIControllerAnimationState.KillCounterCollapsed) {
                    callback?.Invoke();
                }
            }
        }

        private IEnumerator AnimateModifiersRaising(Action? callback = null) {
            if (UIControllerAnimationState == UIControllerAnimationState.KillCounterExpandedTextHidden) {
                float elapsedTime = 0f;
                while (elapsedTime < modifiersRaisingAnimationDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    var progress = elapsedTime / modifiersRaisingAnimationDuration;
                    var newPosition = new Vector3(
                        transform.localPosition.x,
                        Mathf.Lerp(modifiersLoweredHeight, modifiersDefaultHeight, EasingFunctions.EaseOut(progress)));

                    transform.localPosition = newPosition;

                    yield return null;
                }

                if (UIControllerAnimationState == UIControllerAnimationState.KillCounterExpandedTextHidden) {
                    callback?.Invoke();
                }
            }
        }

        private RectTransform CreateLine() {
            Color lineColor = new Color(0.941f, 0.862f, 0.588f, 0.790f);
            float lineWidth = 1f;
            float lineLength = 0f;

            GameObject line = new GameObject("HorizontalLine");
            line.transform.SetParent(transform, false);

            Image lineImage = line.AddComponent<Image>();
            lineImage.color = lineColor;

            RectTransform rectTransform = line.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(lineLength, lineWidth);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(1f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;

            line.transform.localPosition = new Vector2(100f, 0f);

            return rectTransform;
        }

        

        private enum ModifiersUIState {
            Hidden,
            UnfoldingLine,
            LineUnfolded,
            UnfoldingModifiers,
            ModifiersUnfolded,
            CollapsingModifiers,
            ModifiersCollapsed,
            CollapsingLine,
        }
    }
}
