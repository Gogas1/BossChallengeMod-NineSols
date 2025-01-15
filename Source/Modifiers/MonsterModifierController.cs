using BossChallengeMod.Configuration;
using BossChallengeMod.KillCounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Modifiers {
    public class MonsterModifierController : MonoBehaviour {
        public List<ModifierBase> Modifiers = new List<ModifierBase>();

        public List<ModifierConfig> ModifierConfigs = new List<ModifierConfig>();
        public List<ModifierConfig> Available = new List<ModifierConfig>();
        public List<ModifierConfig> Selected = new List<ModifierConfig>();

        private readonly System.Random random;
        private readonly ChallengeConfiguration challengeConfiguration = BossChallengeMod.Instance.ChallengeConfigurationManager.ChallengeConfiguration;

        public Action? OnModifiersRoll;
        public Action? OnDestroyActions;
        public bool AllowRepeating { get; set; }

        private int modifiersNumber = 1;

        public MonsterModifierController() {
            random = new System.Random();
        }

        public void Awake() {
            FindModifiers();
            modifiersNumber = CalculateModifiersNumber(1);
            AllowRepeating = challengeConfiguration.AllowRepeatModifiers;
        }

        public void GenerateAvailableMods() {
            Available.Clear();
            Available.AddRange(ModifierConfigs);
        }

        public void RollModifiers() {
            var availablilities = new List<ModifierConfig>(Available);

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
            foreach (var modifier in Modifiers) {
                modifier.Notify(Selected.Select(m => m.Key), iteration);
            }
            modifiersNumber = CalculateModifiersNumber(iteration + 1);
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
    }
}
