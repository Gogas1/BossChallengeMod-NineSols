using BossChallengeMod.BossPatches;
using BossChallengeMod.Configuration;
using BossChallengeMod.Interfaces;
using BossChallengeMod.KillCounting;
using NineSolsAPI.Utils;
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

        public List<ModifierBase> MustIncludeModifiers = new List<ModifierBase>();
        public List<ModifierBase> Modifiers = new List<ModifierBase>();

        public List<ModifierConfig> ModifierConfigs = new List<ModifierConfig>();
        public List<ModifierConfig> RollableModifiers = new List<ModifierConfig>();
        public List<ModifierConfig> Available = new List<ModifierConfig>();
        public List<ModifierConfig> Selected = new List<ModifierConfig>();

        public Dictionary<string, ModifierConfig> CombinationModifiers = new();

        private bool _isStartRolled = false;
        private bool _isDied = false;

        private ChallengeConfiguration challengeConfiguration;
        
        private PlayerSensor playerSensor = null!;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                if (ApplicationCore.IsInBossMemoryMode) return challengeConfigurationManager.ChallengeConfiguration;
                else return storyChallengeConfigurationManager.ChallengeConfiguration;
            }
        }

        protected bool EnableModifiersScaling {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesEnableModifiersScaling;                        
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesEnableModifiersScaling;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesEnableModifiersScaling;
                    default:
                        return false;
                }
            }
        }
        protected int MaxModifiersNumber {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMaxModifiersNumber;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMaxModifiersNumber; ;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMaxModifiersNumber; ;
                    default:
                        return 1;
                }
            }
        }
        protected int MaxModifiersScalingCycle {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMaxModifiersScalingCycle;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMaxModifiersScalingCycle;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMaxModifiersScalingCycle;
                    default:
                        return 1;
                }
            }
        }


        protected bool EnableRandomModifiersScaling {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesEnableRandomModifiersScaling;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesEnableRandomModifiersScaling;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesEnableRandomModifiersScaling;
                    default:
                        return false;
                }
            }
        }
        protected int RandomModifiersScalingStartDeath {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMaxModifiersNumber;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMaxModifiersNumber; ;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMaxModifiersNumber; ;
                    default:
                        return 1;
                }
            }
        }
        protected int MinRandomModifiersNumber {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMinRandomModifiersNumber;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMinRandomModifiersNumber; ;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMinRandomModifiersNumber;
                    default:
                        return 1;
                }
            }
        }
        protected int MaxRandomModifiersNumber {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMaxRandomModifiersNumber;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMaxRandomModifiersNumber; ;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMaxRandomModifiersNumber; ;
                    default:
                        return 1;
                }
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
        public ChallengeEnemyType EnemyType { get; set; }

        public MonsterModifierController() {
            challengeConfiguration = ConfigurationToUse;
            monster = GetComponent<MonsterBase>();
            monster.OnDie.AddListener(OnDieHandler);
        }

        public void Init() {
            FindModifiers();
            if (!ApplicationCore.IsInBossMemoryMode && UseProximityShow) {
                PauseAll();
            }
            SetupModifiersLists();
            modifiersNumber = CalculateModifiersNumber(0);
            AllowRepeating = challengeConfiguration.AllowRepeatModifiers;
        }

        private void Start() {
            if (UseProximityShow) {
                playerSensor = InitPlayerSensor();

            }

            if (challengeConfiguration.ModifiersStartFromDeath == 0 && !_isStartRolled) {
                try {
                    RollModifiers(0);
                    ApplyModifiers(0);
                    _isStartRolled = true;
                } catch (Exception ex) {
                    Log.Error($"{ex.Message}, {ex.StackTrace}");
                }
            }
        }

        public void GenerateAvailableMods() {
            Available.Clear();
            Available.AddRange(RollableModifiers);
        }

        public void RollModifiers(int iteration) {
            if (_isDied) {
                Selected.Clear();
                OnModifiersRoll?.Invoke();
                return;
            }

            if (iteration < challengeConfiguration.ModifiersStartFromDeath) {
                Selected.Clear();
                return;
            }

            var availabilities = new List<ModifierConfig>(Available);

            availabilities.RemoveAll(a => !a.CanBeRolledConditionPredicate?.Invoke(a, iteration) ?? false);

            if (!AllowRepeating) {
                availabilities = availabilities.Except(Selected).ToList();
            }
            Selected.Clear();

            if (availabilities.Any()) {
                for (int i = 0; i < modifiersNumber && availabilities.Any(); i++) {
                    try {
                        var selected = availabilities[BossChallengeMod.Random.Next(0, availabilities.Count)];

                        foreach (var cm in selected.CombinationModifiers) {
                            if (CombinationModifiers.TryGetValue(cm, out var combinationModifier)) {
                                availabilities.Add(combinationModifier);
                            }
                        }

                        Selected.Add(selected);
                        var selectedIncompatibles = new HashSet<string>(Selected.SelectMany(s => s.Incompatibles));
                        var selectedKeys = new HashSet<string>(Selected.Select(s => s.Key));

                        availabilities.RemoveAll(am =>
                            selectedIncompatibles.Contains(am.Key) ||
                            am.Incompatibles.Any(ai => selectedKeys.Contains(ai))
                        );

                    } catch (Exception ex) {
                        Log.Error($"{ex.Message}, {ex.StackTrace}");
                    }
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
            Modifiers.AddRange(MustIncludeModifiers);
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
            playerSensor.gameObject.SetActive(false);
            _isDied = true;
            OnModifiersRoll?.Invoke();
        }

        private void NotifyModifiers(int iteration) {
            try {
                var selectedKeys = new HashSet<string>(Selected.Select(m => m.Key));
                foreach (var modifier in Modifiers) {
                    try {
                        if (selectedKeys.Contains(modifier.Key)) {
                            modifier.NotifyActivation(iteration);
                        } else {
                            modifier.NotifyDeactivation(iteration);
                        }
                    } catch (Exception ex) {
                        Log.Error($"Exception occured while applying {modifier.name}: {ex.Message}, {ex.StackTrace}");
                    }
                }
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
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

            if (EnableModifiersScaling) {
                int baseScalingValue = 1;
                float progress = MaxModifiersScalingCycle > 0 ? (float)Math.Max(iteration, 0) / MathF.Max(MaxModifiersScalingCycle, 1) : 1;
                float progressMultiplier = MathF.Min(1, progress);
                int scalingDiff = Math.Abs(MaxModifiersNumber);
                result += Math.Max(baseScalingValue, (int)Math.Round(scalingDiff * progressMultiplier, MidpointRounding.AwayFromZero));
            }

            if (EnableRandomModifiersScaling && iteration >= RandomModifiersScalingStartDeath) {
                int value = BossChallengeMod.Random.Next(MinRandomModifiersNumber, MaxRandomModifiersNumber + 1);

                result += value;
            } else {
                result = Math.Max(1, result);
            }

            return result;
        }

        public void ResetComponent() {
            playerSensor?.gameObject.SetActive(false);

            _isDied = false;

            if(UseCompositeTracking || UseProximityShow) {
                BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
            }

            challengeConfiguration = ConfigurationToUse;
            FindModifiers();
            if (!ApplicationCore.IsInBossMemoryMode && UseProximityShow) {
                PauseAll();
            }

            SetupModifiersLists();

            modifiersNumber = CalculateModifiersNumber(0);
            AllowRepeating = challengeConfiguration.AllowRepeatModifiers;

            if (challengeConfiguration.ModifiersStartFromDeath == 0 && !_isStartRolled) {
                RollModifiers(0);
                ApplyModifiers(0);
                _isStartRolled = true;
            }

            playerSensor?.gameObject.SetActive(true);
        }

        private void SetupModifiersLists() {
            RollableModifiers.Clear();
            RollableModifiers.AddRange(ModifierConfigs.Where(mc => !mc.IsPersistentModifier && !mc.IsCombinationModifier));

            CombinationModifiers = ModifierConfigs.Where(mc => mc.IsCombinationModifier).ToDictionary(mc => mc.Key, mc => mc);
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
                else if(UseProximityShow) {
                    BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(this);
                }
            });
            sensor.PlayerExitEvent.AddListener(() => {
                if (UseCompositeTracking) {
                    PauseAll();
                    BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
                } else if(UseProximityShow) {
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
