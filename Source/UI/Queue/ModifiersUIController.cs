using BossChallengeMod.Global;
using BossChallengeMod.Helpers;
using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BossChallengeMod.UI.QueueUI {
    public class ModifiersUIController : QueueUISystem {

        private class ModifierTextItem {
            public int Id { get; }
            public string Text { get; }

            public ModifierTextItem(int id, string text) {
                Id = id;
                Text = text;
            }
        }

        private class ModifierUIItem {
            public int Id { get; }
            public TMP_Text TextObj { get; }

            public ModifierUIItem(int id, TMP_Text textObj) {
                Id = id;
                TextObj = textObj;
            }
        }

        private class ModifierTextItemComparer : IEqualityComparer<ModifierTextItem> {
            public bool Equals(ModifierTextItem x, ModifierTextItem y) {
                return x.Id == y.Id;
            }

            public int GetHashCode(ModifierTextItem obj) {
                return obj.Id;
            }
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

        private RectTransform line = null!;

        private List<ModifierTextItem> modifiers = new();
        private List<ModifierUIItem> items = new();

        public float lineDefaultLength = 200f;
        public float lineUnfoldAnimationDuration = 0.75f;
        public float lineFoldAnimationDuration = 0.75f;

        public float modifierTextAnimationShowDuration = 0.5f;
        public float modifierTextAnimationHideDuration = 0.5f;
        public float modifierTextDefaultAlpha = 0.79f;

        private float modifiersDefaultHeight = -21f;
        private float modifiersLoweredHeight = -66f;
        private float modifiersRaisingAnimationDuration = 1f;
        private float modifiersLoweringAnimationDuration = 1f;

        private ModifiersUIState currentState;
        private ModifiersUIState CurrentState {
            get { return currentState; }
            set { currentState = value; }
        }

        private List<List<ModifierTextItem>> pendingModifiersChanges = new();

        public UIControllerAnimationState UIControllerAnimationState;

        public void Awake() {
            line = CreateLine();
            line.gameObject.SetActive(false);

            OnOperationProcessed += ValidateModifiersShow;
        }

        public void SetModifiers(Dictionary<int, string> modifiers) {
            if (!modifiers.Any() && !pendingModifiersChanges.Any() && !items.Any()) {
                return;
            }

            EnqueueOperation(UnfoldLineAnimation());
            pendingModifiersChanges.Add(modifiers.Select(m => new ModifierTextItem(m.Key, m.Value)).ToList());
            EnqueueOperation(SetModifiersJob());
        }

        public void Reset() {
            EnqueueOperation(ResetModifiersJob());
        }

        private IEnumerator ResetModifiersJob() {
            modifiers.Clear();
            yield return RefreshModifiersItems();
        }

        public void RaiseModifiers(Action? callback = null) {
            StartCoroutine(AnimateModifiersRaising(callback));
        }

        public void LowerModifiers(Action? callback = null) {
            StartCoroutine(AnimateModifiersLowering(callback));
        }
        private IEnumerator SetModifiersJob() {
            var latestModifiers = pendingModifiersChanges.LastOrDefault();
            if(latestModifiers != null) {
                pendingModifiersChanges.Clear();
                var resultModifiers = latestModifiers.Union(modifiers.Intersect(latestModifiers, new ModifierTextItemComparer()), new ModifierTextItemComparer()).ToList();
                modifiers.Clear();            
                modifiers.AddRange(resultModifiers);

            }
            yield return RefreshModifiersItems();
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

            line.transform.localPosition = new Vector2(150f, 0f);

            return rectTransform;
        }

        private IEnumerator UnfoldLineAnimation() {
            if (CurrentState == ModifiersUIState.Hidden && !items.Any()) {
                CurrentState = ModifiersUIState.UnfoldingLine;

                line.gameObject.SetActive(true);

                float startingLength = line.sizeDelta.x;
                var startingProgress = startingLength / lineDefaultLength;
                float elapsedTime = lineUnfoldAnimationDuration * startingProgress;
                while (elapsedTime < lineUnfoldAnimationDuration) {
                    elapsedTime += Time.unscaledDeltaTime;
                    float progress = elapsedTime / lineUnfoldAnimationDuration;
                    float newLength = Mathf.Lerp(startingLength, lineDefaultLength, EasingFunctions.EaseOut(progress));
                    line.sizeDelta = new Vector2(newLength, line.sizeDelta.y);

                    yield return null;
                }

                line.sizeDelta = new Vector2(lineDefaultLength, line.sizeDelta.y);

                CurrentState = ModifiersUIState.LineUnfolded;
            }
        }

        private IEnumerator CollapseLineAnimation() {
            if (CurrentState == ModifiersUIState.LineUnfolded ||
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

                line.sizeDelta = new Vector2(0, line.sizeDelta.y);

                CurrentState = ModifiersUIState.Hidden;
            }
        }

        private IEnumerator RefreshModifiersItems() {
            var modifiersToRemove = items
                .Where(uiItem => !modifiers.Any(mod => mod.Id == uiItem.Id))
                .Select(uiItem => uiItem.Id)
                .ToList();

            var modifiersToCreate = modifiers
                .Where(mod => !items.Any(uiItem => uiItem.Id == mod.Id))
                .ToList();

            bool modifiersRemovedFlag = true;
            bool modifiersFixedFlag = false;
            bool modifiersCreatedFlag = true;

            if(modifiersToRemove.Any()) {
                modifiersRemovedFlag = false;
                yield return RemoveModifiersJob(modifiersToRemove, () => { modifiersRemovedFlag = true; });
            }

            yield return new WaitUntil(() => modifiersRemovedFlag);
            FixModifiersPositionsJob(() => { modifiersFixedFlag = true; });

            yield return new WaitUntil(() => modifiersFixedFlag);

            if (modifiersToCreate.Any()) {
                modifiersCreatedFlag = true;
                yield return CreateModifiersJob(modifiersToCreate, () => { modifiersCreatedFlag = true; });
            }

            yield return new WaitUntil(() => modifiersCreatedFlag);
        }

        private IEnumerator RemoveModifiersJob(List<int> idsToRemove, Action? onDone = null) {
            var itemsToRemove = items.Join(idsToRemove, i => i.Id, ir => ir, (i, ir) => i).ToList();

            if (!itemsToRemove.Any()) {
                onDone?.Invoke();
            }

            while (itemsToRemove.Any()) {
                var item = itemsToRemove.Pop();
                items.Remove(item);

                bool isLast = !itemsToRemove.Any();
                StartCoroutine(AnimateModifierHide(item.TextObj, isLast, onDone));
                if(!isLast) {
                    yield return new WaitForSecondsRealtime(0.2f);
                }
            }
        }

        private void FixModifiersPositionsJob(Action onDone) {
            if (!items.Any()) {
                onDone?.Invoke();
            }

            foreach (var item in items) {
                var calculatedPosition = CalculatePosition(item);
                StartCoroutine(MoveItem(item.TextObj.transform, calculatedPosition, item == items.Last(), onDone));
            }
        }



        private IEnumerator MoveItem(Transform transform, Vector2 targetPosition, bool isLast = false, Action? onLastDone = null) {
            Vector2 startPos = transform.localPosition;
            float progress = 0f;
            float progressPerSecond = 1.5f;

            while (progress < 1f) {
                progress += progressPerSecond * Time.unscaledDeltaTime;
                transform.localPosition = Vector3.Lerp(startPos, targetPosition, progress);
                yield return null;
            }

            transform.localPosition = targetPosition;
            if (isLast) {
                onLastDone?.Invoke();
            }
        }

        private IEnumerator CreateModifiersJob(List<ModifierTextItem> newItems, Action onDone) {
            var modifiersEnumerator = newItems.GetEnumerator();
            int counter = 1;

            if(!newItems.Any()) {
                onDone?.Invoke();
            }

            foreach (var modifier in newItems) {
                var newText = UIController.InitializeText(Vector2.zero, gameObject, LocalizationResolver.Localize(modifier.Text), true, 21);
                counter++;
                newText.alpha = 0f;
                var newItem = new ModifierUIItem(modifier.Id, newText);
                items.Add(newItem);
                newText.transform.localPosition = CalculatePosition(newItem);

                bool isLast = items.Count == modifiers.Count;
                StartCoroutine(AnimateModifierShow(newText, isLast));
                if (isLast) {
                    yield return new WaitForSecondsRealtime(0.2f);
                }
            }
        }

        private Vector2 CalculatePosition(ModifierUIItem modifierUIItem) {
            var index = items.IndexOf(modifierUIItem) + 1;
            return new Vector2(0, -31 * index);
        }

        private IEnumerator AnimateModifierShow(TMP_Text textItem, bool isLast = false, Action? onDone = null) {
            textItem.gameObject.SetActive(true);

            float startingAlpha = textItem.alpha;
            float startingProgress = startingAlpha / modifierTextDefaultAlpha;
            float elapsedTime = modifierTextAnimationShowDuration * startingProgress;
            while (elapsedTime < modifierTextAnimationShowDuration) {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / modifierTextAnimationShowDuration;
                textItem.alpha = Mathf.Lerp(startingAlpha, modifierTextDefaultAlpha, progress);

                yield return null;
            }

            if(isLast) {
                onDone?.Invoke();
            }

        }

        private IEnumerator CollapseModifiers() {
            if (CurrentState == ModifiersUIState.ModifiersUnfolded || CurrentState == ModifiersUIState.UnfoldingModifiers) {
                CurrentState = ModifiersUIState.CollapsingModifiers;

                while (items.Any()) {
                    var item = items.Pop();
                    StartCoroutine(AnimateModifierHide(item.TextObj, !items.Any()));
                    yield return new WaitForSecondsRealtime(0.2f);
                }
            }
        }

        private IEnumerator AnimateModifierHide(TMP_Text textItem, bool isLast = false, Action? onLastDone = null) {
            float startingAlpha = textItem.alpha;
            float startingProgress = 1.0f - startingAlpha / modifierTextDefaultAlpha;
            float elapsedTime = modifierTextAnimationHideDuration * startingProgress;
            while (elapsedTime < modifierTextAnimationHideDuration) {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / modifierTextAnimationHideDuration;

                textItem.alpha = Mathf.Lerp(startingAlpha, 0, progress);

                yield return null;
            }

            if (isLast) {
                onLastDone?.Invoke();
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

        private void ValidateModifiersShow() {
            if(!operationsQueue.Any() && !modifiers.Any() && CurrentState != ModifiersUIState.Hidden) {
                EnqueueOperation(CollapseModifiers());
                EnqueueOperation(CollapseLineAnimation());
            }
        }
    }
}