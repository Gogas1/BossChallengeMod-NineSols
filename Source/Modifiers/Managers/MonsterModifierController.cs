using BossChallengeMod.Configuration;
using BossChallengeMod.Interfaces;
using BossChallengeMod.KillCounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace BossChallengeMod.Modifiers.Managers {
    public class MonsterModifierController : MonoBehaviour, IResettableComponent {
        private ChallengeConfigurationManager challengeConfigurationManager = BossChallengeMod.Instance.ChallengeConfigurationManager;
        private StoryChallengeConfigurationManager storyChallengeConfigurationManager = BossChallengeMod.Instance.StoryChallengeConfigurationManager;
        private MonsterBase monster = null!;
        private int modifiersNumber = 1;

        public List<ModifierBase> Modifiers = new List<ModifierBase>();

        public List<ModifierConfig> ModifierConfigs = new List<ModifierConfig>();
        public List<ModifierConfig> Available = new List<ModifierConfig>();
        public List<ModifierConfig> Selected = new List<ModifierConfig>();

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

        public bool CanBeTracked { get; set; }
        public bool UseCompositeTracking { get; set; }
        private float showDistance = 500f;
        public float ShowDistance {
            get => showDistance;
        }


        public MonsterModifierController() {
            challengeConfiguration = ConfigurationToUse;
            monster = GetComponent<MonsterBase>();
        }

        public void Awake() {
            FindModifiers();
            modifiersNumber = CalculateModifiersNumber(0);
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

            if(!Player.i.mainAbilities.ChargedAttackAbility.IsActivated) {
                availablilities.RemoveAll(m => m.Key.Contains("shield"));
            }

            if(!ApplicationCore.IsInBossMemoryMode) {
                availablilities.RemoveAll(m => m.Key == "timer");
            }

            if (!AllowRepeating) {
                availablilities = availablilities.Except(Selected).ToList();
            }
            Selected.Clear();

            if (availablilities.Any()) {
                for (int i = 0; i < modifiersNumber && availablilities.Any(); i++) {
                    var selected = availablilities[UnityEngine.Random.Range(0, availablilities.Count)];
                    availablilities.RemoveAll(am => selected.Incompatibles.Select(i => i).Contains(am.Key));
                    Selected.Add(selected);
                }
            }

            OnModifiersRoll?.Invoke();
        }

        public void ApplyModifiers(int iteration) {
            try {

                var selectedKeys = new HashSet<string>(Selected.Select(m => m.Key));
                foreach (var modifier in Modifiers) {
                    if (selectedKeys.Contains(modifier.Key)) {
                        modifier.NotifyActivation(iteration);
                    } else {
                        modifier.NotifyDeactivation();
                    }
                }
                modifiersNumber = CalculateModifiersNumber(iteration + 1);

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        public void FindModifiers() {
            Modifiers.Clear();
            Modifiers.AddRange(gameObject.GetComponentsInChildren<ModifierBase>());
        }

        public void OnDestroy() {
            OnDestroyActions?.Invoke();
        }

        private void PauseModifiers() {
            foreach (var modifier in Modifiers) {
                modifier.NotifyPause();
            }
        }

        private void ResumeModifiers() {
            foreach (var modifier in Modifiers) {
                modifier.NotifyResume();
            }
        }

        private int CalculateModifiersNumber(int iteration) {
            var result = 0;

            if (challengeConfiguration.EnableModifiersScaling) {
                int baseScalingValue = 1;
                float progress = Math.Max(iteration, 1) / MathF.Max(challengeConfiguration.MaxModifiersScalingCycle, 1);
                float progressMultiplier = MathF.Min(1, progress);
                int scalingDiff = Math.Abs(challengeConfiguration.MaxModifiersNumber - 1);
                result += baseScalingValue + (int)(scalingDiff * progressMultiplier);
            }

            if (challengeConfiguration.EnableRandomModifiersScaling && iteration >= challengeConfiguration.RandomModifiersScalingStartDeath) {
                int value = UnityEngine.Random.Range(challengeConfiguration.MinRandomModifiersNumber, challengeConfiguration.MaxRandomModifiersNumber + 1);

                result += value;
            } else {
                result = Math.Max(1, result);
            }

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

        private PlayerSensor InitPlayerSensor() {
            CircleCollider2D showTrigger;
            PlayerSensor sensor;

            var sensorFolder = new GameObject("ChallengePlayerSensorModifiersController");
            sensorFolder.transform.SetParent(transform, false);

            showTrigger = sensorFolder.AddComponent<CircleCollider2D>();
            showTrigger.isTrigger = true;
            showTrigger.radius = ShowDistance;

            sensor = sensorFolder.AddComponent<PlayerSensor>();
            sensor.PlayerEnterEvent = new UnityEngine.Events.UnityEvent();
            sensor.PlayerExitEvent = new UnityEngine.Events.UnityEvent();
            sensor.PlayerStayEvent = new UnityEngine.Events.UnityEvent();

            AutoAttributeManager.AutoReference(sensorFolder);
            sensor.Awake();
            sensor.EnterLevelReset();

            sensor.PlayerEnterEvent.AddListener(() => {
                if(UseCompositeTracking) {

                }
                else {
                    BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(this);
                }
            });
            sensor.PlayerExitEvent.AddListener(() => {
                if (UseCompositeTracking) {

                }
                else {
                    BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(null);
                }
            });

            return sensor;
        }

        public int GetPriority() {
            return 0;
        }
    }
}
