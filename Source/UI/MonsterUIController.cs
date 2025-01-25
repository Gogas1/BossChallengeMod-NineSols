using BossChallengeMod.KillCounting;
using BossChallengeMod.Modifiers.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

namespace BossChallengeMod.UI {
    public class MonsterUIController {
        private MonsterKillCounter? trackedKillCounter = null;
        private MonsterModifierController? trackedModifiersController = null;

        private UIController UIController = BossChallengeMod.Instance.UIController;

        private bool isHidingKillCounter = false;
        private bool isHidingRecordsText = false;

        public MonsterUIController() {
            UIController.OnModifiersRaisedAnimationDone += () => {
                IsHidingRecordsText = false;
            };
            SceneManager.sceneUnloaded += ValidateOnSceneUnload;
        }

        public bool IsHidingKillCounter { 
            get => isHidingKillCounter;
            set {
                isHidingKillCounter = value;
                if (trackedKillCounter != null && !isHidingKillCounter) {
                    UpdateKillCounterUI();
                }
            }
        }
        public bool IsHidingRecordsText { 
            get => isHidingRecordsText;
            set { 
                isHidingRecordsText = value;
                if (trackedKillCounter != null && !isHidingRecordsText) {
                    UpdateKillCounterUI();
                }
            }
        }

        public void ChangeKillCounter(MonsterKillCounter? monsterKillCounter) {
            if(trackedKillCounter != null) {
                trackedKillCounter.OnUpdate -= UpdateKillCounterUI;
                trackedKillCounter.OnDestroyActions -= ResetKillCounter;
            }

            trackedKillCounter = monsterKillCounter;

            if (trackedKillCounter != null) {
                trackedKillCounter.OnUpdate += UpdateKillCounterUI;
                trackedKillCounter.OnDestroyActions += ResetKillCounter;
            }

            UpdateKillCounterUI();
        }

        public void ChangeModifiersController(MonsterModifierController? monsterModifierController) {
            if (trackedModifiersController != null) {
                trackedModifiersController.OnModifiersRoll -= UpdateModifierUI;
                trackedModifiersController.OnDestroyActions -= ResetModifierController;
            }

            trackedModifiersController = monsterModifierController;

            if (trackedModifiersController != null) {
                trackedModifiersController.OnModifiersRoll += UpdateModifierUI;
                trackedModifiersController.OnDestroyActions += ResetModifierController;
            }

            UpdateModifierUI();
        }

        private void UpdateKillCounterUI() {
            if (trackedKillCounter == null) {
                IsHidingKillCounter = true;
                UIController.HideText(() => IsHidingKillCounter = false);

                if(UIController.CurrentAnimationState != UIControllerAnimationState.KillCounterCollapsed) {
                    IsHidingRecordsText = true;
                    UIController.HideExpandedKillNumbersText();
                }
                return;
            }

            if (!isHidingKillCounter) {
                UIController.UpdateBossCounterNumber(trackedKillCounter.KillCounter);
                UIController.ShowText();
            }

            if (!isHidingRecordsText && (trackedKillCounter.BestCount > 0 || trackedKillCounter.LastCount > 0)) {
                UIController.UpdateBossRecordValues(trackedKillCounter.BestCount, trackedKillCounter.LastCount);
                UIController.ShowExpandedKillNumbersText();
            }
        }

        private void UpdateModifierUI() {
            if (trackedModifiersController == null) {
                UIController.HideModifiers();
                return;
            }

            if (trackedModifiersController.Selected.Any()) {
                UIController.UpdateModifiers(trackedModifiersController.Selected.Select(m => m.Key));
            }
            else {
                UIController.HideModifiers();
            }
        }

        public void ResetKillCounter() {
            ChangeKillCounter(null);
        }

        public void ResetModifierController() {
            ChangeModifiersController(null);
        }

        public void ValidateOnSceneUnload(Scene scene) {
            UpdateKillCounterUI();
            UpdateModifierUI();
        }

        public void Unload() {
            SceneManager.sceneUnloaded -= ValidateOnSceneUnload;
        }
    }
}
