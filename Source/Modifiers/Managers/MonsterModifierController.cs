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

        private bool _isStarted = false;
        private bool _isDied = false;

        private ChallengeConfiguration challengeConfiguration;
        private PlayerSensor playerSensor = null!;
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
        public bool UseProximityShow { get; set; }
        private float showDistance = 400f;
        public float ShowDistance {
            get => showDistance;
        }


        public MonsterModifierController() {
            challengeConfiguration = ConfigurationToUse;
            monster = GetComponent<MonsterBase>();
            monster.OnDie.AddListener(OnDieHandler);
        }

        public void Awake() {
            FindModifiers();
            if (!ApplicationCore.IsInBossMemoryMode) {
                PauseAll();
            }
            modifiersNumber = CalculateModifiersNumber(0);
            AllowRepeating = challengeConfiguration.AllowRepeatModifiers;
        }

        private void Start() {
            if (UseProximityShow) {
                playerSensor = InitPlayerSensor();

            }

            if (challengeConfiguration.ModifiersStartFromDeath == 0) {
                RollModifiers(0);
                ApplyModifiers(0);
            }
            _isStarted = true;
        }

        public void GenerateAvailableMods() {
            Available.Clear();
            Available.AddRange(ModifierConfigs);
        }

        public void RollModifiers(int iteration) {
            if (_isDied) {
                return;
            }

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
                    var selected = availablilities[BossChallengeMod.Random.Next(0, availablilities.Count)];
                    availablilities.RemoveAll(am => selected.Incompatibles.Select(i => i).Contains(am.Key));
                    Selected.Add(selected);
                }
            }

            OnModifiersRoll?.Invoke();
        }

        public void ApplyModifiers(int iteration) {
            if (_isDied) {
                return;
            }

            try {

                NotifyModifiers(iteration);
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
            monster.OnDie.RemoveListener(OnDieHandler);
            if (UseCompositeTracking) {
                BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
            }
        }

        private void OnDieHandler() {
            Selected.Clear();
            ApplyModifiers(0);

            BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
            _isDied = true;
        }

        private void NotifyModifiers(int iteration) {
            var selectedKeys = new HashSet<string>(Selected.Select(m => m.Key));
            foreach (var modifier in Modifiers) {
                try {
                    if (selectedKeys.Contains(modifier.Key)) {
                        modifier.NotifyActivation(iteration);
                    } else {
                        modifier.NotifyDeactivation();
                    }
                } catch (Exception ex) {
                    Log.Error($"Exception occured while applying {modifier.name}: {ex.Message}, {ex.StackTrace}");
                }
            }
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
                int value = BossChallengeMod.Random.Next(challengeConfiguration.MinRandomModifiersNumber, challengeConfiguration.MaxRandomModifiersNumber + 1);

                result += value;
            } else {
                result = Math.Max(1, result);
            }

            return result;
        }

        public void ResetComponent() {
            _isStarted = false;
            challengeConfiguration = ConfigurationToUse;
            FindModifiers();
            if (!ApplicationCore.IsInBossMemoryMode) {
                PauseAll();
            }
            modifiersNumber = CalculateModifiersNumber(1);
            AllowRepeating = challengeConfiguration.AllowRepeatModifiers;

            if (challengeConfiguration.ModifiersStartFromDeath == 0 && _isStarted) {
                RollModifiers(0);
                ApplyModifiers(0);
            }
        }

        private void PauseAll() {
            foreach (var item in Modifiers) {
                try {
                    item.NotifyPause();

                } catch (Exception ex) {
                    Log.Error($"{ex.Message}, {ex.StackTrace}");
                }
            }
        }

        private void ResumeAll() {
            foreach (var item in Modifiers) {
                item.NotifyResume();
            }
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
                    ResumeAll();
                    BossChallengeMod.Instance.MonsterUIController.AddCompositeModifierController(this);
                }
                else {
                    BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(this);
                }
            });
            sensor.PlayerExitEvent.AddListener(() => {
                if (UseCompositeTracking) {
                    PauseAll();
                    BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
                } else {
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
