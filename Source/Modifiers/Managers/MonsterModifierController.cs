using BossChallengeMod.Configuration;
using BossChallengeMod.Interfaces;
using BossChallengeMod.KillCounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterModifierController : MonoBehaviour, IResettableComponent {
        private ChallengeConfigurationManager challengeConfigurationManager = BossChallengeMod.Instance.ChallengeConfigurationManager;
        private StoryChallengeConfigurationManager storyChallengeConfigurationManager = BossChallengeMod.Instance.StoryChallengeConfigurationManager;

        public List<ModifierBase> Modifiers = new List<ModifierBase>();

        public List<ModifierConfig> ModifierConfigs = new List<ModifierConfig>();
        public List<ModifierConfig> Available = new List<ModifierConfig>();
        public List<ModifierConfig> Selected = new List<ModifierConfig>();

        private readonly System.Random random;
        private ChallengeConfiguration challengeConfiguration;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                if (ApplicationCore.IsInBossMemoryMode) return challengeConfigurationManager.ChallengeConfiguration;
                else return storyChallengeConfigurationManager.ChallengeConfiguration;
            }
        }

        public Action? OnModifiersRoll;
        public Action? OnDestroyActions;
        public bool AllowRepeating { get; set; }

        private int modifiersNumber = 1;

        public MonsterModifierController() {
            random = new System.Random();
            challengeConfiguration = ConfigurationToUse;
        }

        public void Awake() {
            FindModifiers();
            modifiersNumber = CalculateModifiersNumber(1);
            AllowRepeating = challengeConfiguration.AllowRepeatModifiers;
        }

        private void Start() {
            if (challengeConfiguration.ModifiersStartFromDeath == 0) {
                RollModifiers(0);
                ApplyModifiers(0);
            }
        }

        public void GenerateAvailableMods() {
            Available.Clear();
            Available.AddRange(ModifierConfigs);
        }

        public void RollModifiers(int iteration) {
            if (iteration < challengeConfiguration.ModifiersStartFromDeath) {
                Selected.Clear();
                return;
            }

            var availablilities = new List<ModifierConfig>(Available);

            if(iteration == 0) {
                availablilities.RemoveAll(m => m.Key == "timer");
            }

            if (!AllowRepeating) {
                availablilities = availablilities.Except(Selected).ToList();
            }
            Selected.Clear();

            if (availablilities.Any()) {
                for (int i = 0; i < modifiersNumber && availablilities.Any(); i++) {
                    var selected = availablilities[random.Next(0, availablilities.Count)];
                    availablilities.RemoveAll(am => selected.Incompatibles.Select(i => i).Contains(am.Key));
                    Selected.Add(selected);
                }
            }

            OnModifiersRoll?.Invoke();
        }

        public void ApplyModifiers(int iteration) {
            try {
                foreach (var modifier in Modifiers) {
                    modifier.NotifyActivation(Selected.Select(m => m.Key), iteration);
                }
                modifiersNumber = CalculateModifiersNumber(iteration + 1);

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        public void FindModifiers() {
            Modifiers.AddRange(GetComponentsInChildren<ModifierBase>());
        }

        public void OnDestroy() {
            OnDestroyActions?.Invoke();
        }

        private int CalculateModifiersNumber(int iteration) {
            if (!challengeConfiguration.EnableModifiersScaling) return 1;

            int baseScalingValue = 1;
            float progress = iteration / MathF.Max(challengeConfiguration.MaxModifiersScalingCycle, 1);
            float progressMultiplier = MathF.Min(1, progress);
            int scalingDiff = Math.Abs(challengeConfiguration.MaxModifiersNumber - 1);
            var result = baseScalingValue + (int)(scalingDiff * progressMultiplier);
            return result;
        }

        public void ResetComponent() {
            challengeConfiguration = ConfigurationToUse;
            FindModifiers();
            modifiersNumber = CalculateModifiersNumber(1);
            AllowRepeating = challengeConfiguration.AllowRepeatModifiers;

            RollModifiers(0);
            ApplyModifiers(0);
        }

        public int GetPriority() {
            return 0;
        }
    }
}
