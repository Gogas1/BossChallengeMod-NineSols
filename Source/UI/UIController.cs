using HarmonyLib;
using NineSolsAPI;
using RCGMaker.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRule;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using ClipperLibClone;
using System.Collections;
using BossChallengeMod.Helpers;
using UnityEngine.SceneManagement;

namespace BossChallengeMod.UI {
    public class UIController {
        private KillCounterController bossCounterTextController;
        private ModifiersUIController modifiersController;
        private TimerController timerController;
        private CurrentTalismanUIContoller talismanUIController;

        public Action? OnModifiersRaisedAnimationDone;

        private UIControllerAnimationState animationState = UIControllerAnimationState.KillCounterCollapsed;
        public UIControllerAnimationState CurrentAnimationState {
            get => animationState;
            private set {
                animationState = value;
                modifiersController.UIControllerAnimationState = value;
                bossCounterTextController.UIControllerAnimationState = value;
                switch (value) {
                    case UIControllerAnimationState.KillCounterCollapsed:
                        break;
                    case UIControllerAnimationState.ModifiersLowered:
                        bossCounterTextController.ShowExtraText(() => {
                            CurrentAnimationState = UIControllerAnimationState.KillCounterExpanded;
                        });
                        break;
                    case UIControllerAnimationState.KillCounterExpanded:
                        break;
                    case UIControllerAnimationState.KillCounterExpandedTextHidden:
                        modifiersController.RaiseModifiers(() => {
                            CurrentAnimationState = UIControllerAnimationState.KillCounterCollapsed;
                            OnModifiersRaisedAnimationDone?.Invoke();
                        });
                        break;
                }
            }
        }

        private GameObject rightPanel;
        private GameObject bottomPanel;
        private GameObject bottomLeftPanel;

        public UIController() {
            rightPanel = CreateRightPanelObject();
            bottomPanel = CreateBottomPanelObject();
            bottomLeftPanel = CreateBottomLeftPanelObject();

            bossCounterTextController = rightPanel.AddChildrenComponent<KillCounterController>("KillCounter");
            modifiersController = CreateModifiersControllerGUI();
            timerController = bottomPanel.AddChildrenComponent<TimerController>("TimerUI");
            talismanUIController = bottomLeftPanel.AddChildrenComponent<CurrentTalismanUIContoller>("TalismanUI");
        }

        private void RestoreUI() {
            rightPanel = CreateRightPanelObject();
            bottomPanel = CreateBottomPanelObject();
            bossCounterTextController = rightPanel.AddChildrenComponent<KillCounterController>("KillCounter");
            modifiersController = CreateModifiersControllerGUI();
            timerController = bottomPanel.AddChildrenComponent<TimerController>("TimerUI");
        }

        public GameObject CreateRightPanelObject() {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;
            GameObject parentObject = new GameObject("BossChallenge_RightPanelUI");
            parentObject.transform.parent = canvas.transform;

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float width = canvasRect.rect.width;
            float height = canvasRect.rect.height;

            float coordsX = width - width / 10f;
            float coordsY = height - height / 4.5f;

            parentObject.transform.position = new Vector3(coordsX, coordsY);

            return parentObject;
        }

        public GameObject CreateBottomPanelObject() {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;
            GameObject parentObject = new GameObject("BossChallenge_BottomPanelUI");
            parentObject.transform.parent = canvas.transform;

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float width = canvasRect.rect.width;
            float height = canvasRect.rect.height;

            float coordsX = width - width / 2f;
            float coordsY = height / 10f;

            parentObject.transform.position = new Vector3(coordsX, coordsY);

            return parentObject;
        }

        public GameObject CreateBottomLeftPanelObject() {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;
            GameObject parentObject = new GameObject("BossChallenge_BottomLeftPanelUI");
            parentObject.transform.parent = canvas.transform;

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float width = canvasRect.rect.width;
            float height = canvasRect.rect.height;

            float coordsX = width / 13.71f;
            float coordsY = height / 4.15f;

            parentObject.transform.position = new Vector3(coordsX, coordsY);

            return parentObject;
        }

        private ModifiersUIController CreateModifiersControllerGUI() {
            var modifiers = rightPanel.AddChildrenComponent<ModifiersUIController>("ModifiersUI");
            modifiers.transform.localPosition = new Vector3(0, -21f);
            return modifiers;
        }

        public static TMP_Text InitializeText(
            Vector2 localPosition, 
            GameObject parent, 
            string text = "Test", 
            bool active = false,
            int fontSize = 32, 
            TMP_FontAsset? font = null,
            TextAlignmentOptions alignment = TextAlignmentOptions.TopRight) 
        {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;
            var textComponent = canvas.gameObject.AddChildrenComponent<TextMeshProUGUI>("BossCounter_Text");

            textComponent.fontSize = fontSize;

            if(font != null)
                textComponent.font = font;

            textComponent.color = new Color(0.941f, 0.862f, 0.588f, 0.790f);
            textComponent.alignment = alignment;
            textComponent.text = text;

            textComponent.transform.localPosition = localPosition;
            textComponent.transform.SetParent(parent.transform, false);

            textComponent.SetActive(active);

            return textComponent;
        }        
        
        public void UpdateTalisman(Sprite sprite) {
            talismanUIController.UpdateCurrentTalisman(sprite);
        }

        public void UpdateBossCounterNumber(int number) {
            if(rightPanel == null ||
                bottomPanel == null ||
                timerController == null) {
                RestoreUI();
            }

            bossCounterTextController.ChangeNumber(number);
            ShowText();
        }

        public void UpdateModifiers(IEnumerable<string> modifiers) {
            modifiersController.SetModifiers(modifiers);
            modifiersController.Show();
        }

        public void ShowText() {
            bossCounterTextController.Show();
        }

        public void HideText(Action? callback = null) {
            bossCounterTextController.Hide(callback);
        }

        public void HideModifiers() {
            modifiersController.Reset();
        }

        public void ShowExpandedKillNumbersText() {
            modifiersController.LowerModifiers(() => {
                CurrentAnimationState = UIControllerAnimationState.ModifiersLowered;
            });
        }

        public void HideExpandedKillNumbersText(Action? callback = null) {
            bossCounterTextController.HideExtraText(() => {
                CurrentAnimationState = UIControllerAnimationState.KillCounterExpandedTextHidden;
                callback?.Invoke();
            });
        }

        public void UpdateBossRecordValues(int highScore, int prevResult) {
            if (rightPanel == null ||
                bottomPanel == null ||
                timerController == null) {
                RestoreUI();
            }

            bossCounterTextController.UpdateHighScore(highScore);
            bossCounterTextController.UpdatePrevResult(prevResult);
        }

        public void ShowTimer() {
            timerController.Show();
        }

        public void HideTimer() {
            timerController.Hide();
        }

        public void UpdateTimer(int milliseconds) {
            timerController.UpdateTimer(milliseconds);
        }

        private TMP_FontAsset? LoadFont() {
            TMP_FontAsset[] allFonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
            foreach (TMP_FontAsset font in allFonts) {
                if (font.name == "LiberationSans SDF") return font;                
            }

            return null;
        }

        public void Unload() {
            if(rightPanel != null) GameObject.Destroy(rightPanel);
            if (bottomPanel != null) GameObject.Destroy(bottomPanel);
            if (bottomLeftPanel != null) GameObject.Destroy(bottomLeftPanel);
            if (modifiersController != null) GameObject.Destroy(modifiersController);
            if (OnModifiersRaisedAnimationDone != null) OnModifiersRaisedAnimationDone = null;
        }

        public void FixUI() {
            if (rightPanel == null ||
                bottomPanel == null ||
                timerController == null) {
                RestoreUI();
            }
        }
    }

    public enum UIControllerAnimationState {
        KillCounterCollapsed,
        ModifiersLowered,
        KillCounterExpanded,
        KillCounterExpandedTextHidden,
    }
}
