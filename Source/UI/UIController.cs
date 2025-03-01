using NineSolsAPI;
using RCGMaker.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BossChallengeMod.Configuration;

namespace BossChallengeMod.UI {
    public class UIController {
        private KillCounterController bossCounterTextController;
        private QueueUI.ModifiersUIController modifiersController;
        private TimerController timerController;
        private CurrentTalismanUIContoller talismanUIController;

        public Action? OnModifiersRaisedAnimationDone;

        private UIConfiguration _configuration;

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

        private RectTransform rightPanel;
        private RectTransform bottomPanel;
        private RectTransform bottomLeftPanel;

        public UIController(UIConfiguration configuration) {
            rightPanel = CreateRightPanelObject();
            bottomPanel = CreateBottomPanelObject();
            bottomLeftPanel = CreateBottomLeftPanelObject();

            bossCounterTextController = rightPanel.gameObject.AddChildrenComponent<KillCounterController>("KillCounter");
            modifiersController = CreateModifiersControllerGUI();
            timerController = bottomPanel.gameObject.AddChildrenComponent<TimerController>("TimerUI");
            talismanUIController = bottomLeftPanel.gameObject.AddChildrenComponent<CurrentTalismanUIContoller>("TalismanUI");
            _configuration = configuration;
            ResolveConfigurationEvents(_configuration);
        }

        private void RestoreUI() {
            rightPanel = CreateRightPanelObject();
            bottomPanel = CreateBottomPanelObject();
            bottomLeftPanel = CreateBottomLeftPanelObject();

            bossCounterTextController = rightPanel.gameObject.AddChildrenComponent<KillCounterController>("KillCounter");
            modifiersController = CreateModifiersControllerGUI();
            timerController = bottomPanel.gameObject.AddChildrenComponent<TimerController>("TimerUI");
            talismanUIController = bottomLeftPanel.gameObject.AddChildrenComponent<CurrentTalismanUIContoller>("TalismanUI");
        }

        public RectTransform CreateRightPanelObject() {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;

            GameObject parentObject = new GameObject("BossChallenge_RightPanelUI");
            parentObject.transform.SetParent(canvas, false);
            RectTransform rectTransform = parentObject.AddComponent<RectTransform>();

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float coordsX, coordsY;
            CalculateRightPanelPosition(canvasRect, out coordsX, out coordsY);

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0, 0f);
            rectTransform.anchoredPosition = new Vector2(coordsX, coordsY);
            rectTransform.sizeDelta = new Vector2(300, 200);

            return rectTransform;
        }

        private void CalculateRightPanelPosition(RectTransform canvasRect, out float coordsX, out float coordsY) {
            float width = canvasRect.rect.width;
            float height = canvasRect.rect.height;

            CalculateRightPanelPosition(width, height, out coordsX, out coordsY);
        }

        private void CalculateRightPanelPosition(float width, float height, out float coordsX, out float coordsY) {
            coordsX = width - width / 9f;
            coordsY = height - height / 4.5f;
        }

        public RectTransform CreateBottomPanelObject() {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;

            GameObject parentObject = new GameObject("BossChallenge_BottomPanelUI");
            parentObject.transform.SetParent(canvas, false);
            RectTransform rectTransform = parentObject.AddComponent<RectTransform>();

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float coordsX, coordsY;
            CalculateBottomPanelPosition(canvasRect, out coordsX, out coordsY);

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0, 0f);
            rectTransform.anchoredPosition = new Vector2(coordsX, coordsY);
            rectTransform.sizeDelta = new Vector2(100, 100);

            return rectTransform;
        }

        private void CalculateBottomPanelPosition(RectTransform canvasRect, out float coordsX, out float coordsY) {
            float width = canvasRect.rect.width;
            float height = canvasRect.rect.height;

            CalculateBottomPanelPosition(width, height, out coordsX, out coordsY);
        }

        private void CalculateBottomPanelPosition(float width, float height, out float coordsX, out float coordsY) {
            coordsX = width - width / 2f;
            coordsY = height / 10f;
        }

        public RectTransform CreateBottomLeftPanelObject() {
            var canvas = NineSolsAPICore.FullscreenCanvas.transform;

            GameObject parentObject = new GameObject("BossChallenge_BottomLeftPanelUI");
            parentObject.transform.SetParent(canvas, false);
            RectTransform rectTransform = parentObject.AddComponent<RectTransform>();

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float coordsX, coordsY;
            CalculateBottomLeftPanelPosition(canvasRect, out coordsX, out coordsY);

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0, 0f);
            rectTransform.anchoredPosition = new Vector2(coordsX, coordsY);
            rectTransform.sizeDelta = new Vector2(100, 100);

            // Return the created GameObject
            return rectTransform;
        }

        private void CalculateBottomLeftPanelPosition(RectTransform canvasRect, out float coordsX, out float coordsY) {
            float width = canvasRect.rect.width;
            float height = canvasRect.rect.height;

            CalculateBottomLeftPanelPosition(width, height, out coordsX, out coordsY);
        }

        private void CalculateBottomLeftPanelPosition(float width, float height, out float coordsX, out float coordsY) {
            coordsX = width / 13.71f;
            coordsY = height / 4.15f;
        }

        private QueueUI.ModifiersUIController CreateModifiersControllerGUI() {
            var modifiers = rightPanel.gameObject.AddChildrenComponent<QueueUI.ModifiersUIController>("ModifiersUI");
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

            RectTransform rectTransform = textComponent.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 50);

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

        //public void UpdateModifiers(IEnumerable<string> modifiers) {
        //    modifiersController.SetModifiers(modifiers);
        //}

        public void UpdateModifiers(Dictionary<int, string> modifiers) {
            modifiersController.SetModifiers(modifiers);
        }
        public void HideModifiers() {
            modifiersController.Reset();
        }

        public void ShowText() {
            bossCounterTextController.Show();
        }

        public void HideText(Action? callback = null) {
            bossCounterTextController.Hide(callback);
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
            UnsubscribeConfigurationEvents(_configuration);
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

        private void ResolveConfigurationEvents(UIConfiguration configuration) {
            configuration.OnCounterUIEnabledChanged += ToggleCounterUI;
            configuration.OnUseCustomCounterPositionChanged += ToggleCustomCounterUIPosition;
            configuration.OnCounterXPosChanged += ChangeCounterXPosition;
            configuration.OnCounterYPosChanged += ChangeCounterYPosition;
            configuration.OnCounterUIScaleChanged += ChangeCounterScale;

            configuration.OnEnableTimerUIChanged += ToggleTimerUI;
            configuration.OnUseCustomTimerPositionChanged += ToggleCustomTimerUIPosition;
            configuration.OnTimerXPosChanged += ChangeTimerXPosition;
            configuration.OnTimerYPosChanged += ChangeTimerYPosition;
            configuration.OnTimerUIScaleChanged += ChangeTimerScale;

            configuration.OnEnableTalismanModeUIChanged += ToggleTalismanModeUI;
            configuration.OnUseCustomTalismanModePositionChanged += ToggleCustomTalismanModeUIPosition;
            configuration.OnTalismanModeXPosChanged += ChangeTalismanModeXPosition;
            configuration.OnTalismanModeYPosChanged += ChangeTalismanModeYPosition;
            configuration.OnTalismanUIScaleChanged += ChangeTalismanScale;
        }

        private void UnsubscribeConfigurationEvents(UIConfiguration configuration) {
            configuration.OnCounterUIEnabledChanged -= ToggleCounterUI;
            configuration.OnUseCustomCounterPositionChanged -= ToggleCustomCounterUIPosition;
            configuration.OnCounterXPosChanged -= ChangeCounterXPosition;
            configuration.OnCounterYPosChanged -= ChangeCounterYPosition;
            configuration.OnCounterUIScaleChanged -= ChangeCounterScale;

            configuration.OnEnableTimerUIChanged -= ToggleTimerUI;
            configuration.OnUseCustomTimerPositionChanged -= ToggleCustomTimerUIPosition;
            configuration.OnTimerXPosChanged -= ChangeTimerXPosition;
            configuration.OnTimerYPosChanged -= ChangeTimerYPosition;
            configuration.OnTimerUIScaleChanged -= ChangeTimerScale;

            configuration.OnEnableTalismanModeUIChanged -= ToggleTalismanModeUI;
            configuration.OnUseCustomTalismanModePositionChanged -= ToggleCustomTalismanModeUIPosition;
            configuration.OnTalismanModeXPosChanged -= ChangeTalismanModeXPosition;
            configuration.OnTalismanModeYPosChanged -= ChangeTalismanModeYPosition;
            configuration.OnTalismanUIScaleChanged -= ChangeTalismanScale;
        }

        public void ToggleCounterUI(bool toggle) {
            rightPanel.gameObject.SetActive(toggle);
        }
        public void ToggleCustomCounterUIPosition(bool toggle) {
            if(rightPanel != null) {
                if (toggle) {
                    float posX = _configuration.CounterXPos;
                    float posY = _configuration.CounterYPos;

                    rightPanel.anchoredPosition = new Vector3(posX, posY);

                } else {
                    var canvas = NineSolsAPICore.FullscreenCanvas.transform;
                    RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                    float posX, posY;
                    CalculateRightPanelPosition(canvasRect, out posX, out posY);
                    rightPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeCounterXPosition(float posX) {
            if(rightPanel != null) {
                if (_configuration.UseCustomCounterPosition) {
                    float posY = _configuration.CounterYPos;

                    rightPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeCounterYPosition(float posY) {
            if (rightPanel != null) {
                if (_configuration.UseCustomCounterPosition) {
                    float posX = _configuration.CounterXPos;

                    rightPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeCounterScale(float scale) {
            if (rightPanel != null) {
                rightPanel.localScale = new Vector3(scale, scale);
            }
        }


        public void ToggleTimerUI(bool toggle) {
            timerController.gameObject.SetActive(toggle);
        }
        public void ToggleCustomTimerUIPosition(bool toggle) {
            if (bottomPanel != null) {
                if (toggle) {
                    float posX = _configuration.CounterXPos;
                    float posY = _configuration.CounterYPos;

                    bottomPanel.transform.position = new Vector3(posX, posY);

                } else {
                    var canvas = NineSolsAPICore.FullscreenCanvas.transform;
                    RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                    float posX, posY;
                    CalculateBottomPanelPosition(canvasRect, out posX, out posY);
                    bottomPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeTimerXPosition(float posX) {
            if (bottomPanel != null) {
                if (_configuration.UseCustomTimerPosition) {
                    float posY = _configuration.TimerYPos;

                    bottomPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeTimerYPosition(float posY) {
            if (bottomPanel != null) {
                if (_configuration.UseCustomTimerPosition) {
                    float posX = _configuration.TimerXPos;

                    bottomPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeTimerScale(float scale) {
            if (bottomPanel != null) {
                bottomPanel.localScale = new Vector3(scale, scale);
            }
        }


        public void ToggleTalismanModeUI(bool toggle) {
            bottomLeftPanel.gameObject.SetActive(toggle);
        }
        public void ToggleCustomTalismanModeUIPosition(bool toggle) {
            if (bottomLeftPanel != null) {
                if (toggle) {
                    float posX = _configuration.TalismanModeXPos;
                    float posY = _configuration.TalismanModeYPos;

                    bottomLeftPanel.transform.position = new Vector3(posX, posY);

                } else {
                    var canvas = NineSolsAPICore.FullscreenCanvas.transform;
                    RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                    float posX, posY;
                    CalculateBottomLeftPanelPosition(canvasRect, out posX, out posY);
                    bottomLeftPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeTalismanModeXPosition(float posX) {
            if (bottomLeftPanel != null) {
                if (_configuration.UseCustomTalismanModePosition) {
                    float posY = _configuration.TalismanModeYPos;

                    bottomLeftPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeTalismanModeYPosition(float posY) {
            if (bottomLeftPanel != null) {
                if (_configuration.UseCustomTalismanModePosition) {
                    float posX = _configuration.TalismanModeXPos;

                    bottomLeftPanel.anchoredPosition = new Vector3(posX, posY);
                }
            }
        }
        public void ChangeTalismanScale(float scale) {
            if (bottomLeftPanel != null) {
                bottomLeftPanel.localScale = new Vector3(scale, scale);
            }
        }

        public void RecalculatePositions(int width, int height) {
            if (!_configuration.UseCustomCounterPosition) {
                float posX, posY;
                CalculateRightPanelPosition(width, height, out posX, out posY);
                rightPanel.anchoredPosition = new Vector3(posX, posY);
            }

            if (!_configuration.UseCustomTimerPosition) {
                float posX, posY;
                CalculateBottomPanelPosition(width, height, out posX, out posY);
                bottomPanel.anchoredPosition = new Vector3(posX, posY);
            }

            if(!_configuration.UseCustomTalismanModePosition) {
                float posX, posY;
                CalculateBottomLeftPanelPosition(width, height, out posX, out posY);
                bottomLeftPanel.anchoredPosition = new Vector3(posX, posY);
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
