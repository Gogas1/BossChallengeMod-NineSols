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
        private ChallengeMonsterController monsterController = null!;
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
        private bool _isFirstEngage = true;

        private ChallengeConfiguration challengeConfiguration;
        
        private PlayerSensor playerSensor = null!;

        protected ChallengeConfiguration ConfigurationToUse {
            get {
                return challengeConfigurationManager.ChallengeConfiguration;
            }
        }

        protected bool EnableModifiersScaling {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesIsModifiersScalingEnabled;                        
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesIsModifiersScalingEnabled;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesIsModifiersScalingEnabled;
                    default:
                        return false;
                }
            }
        }

        protected float InitialScalingValue {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMinModifiersNumber;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMinModifiersNumber;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMinModifiersNumber;
                    default:
                        return 1;
                }
            }
        }

        protected float ScalingStep {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesModifiersScalingStepValue;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesModifiersScalingStepValue;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesModifiersScalingStepValue;
                    default:
                        return 1;
                }
            }
        }
        protected int StepsCap {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesModifiersStepsCapValue;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesModifiersStepsCapValue;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesModifiersStepsCapValue;
                    default:
                        return 1;
                }
            }
        }


        protected bool EnableRandomModifiersScaling {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesIsRandomModifiersScalingEnabled;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesIsRandomModifiersScalingEnabled;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesIsRandomModifiersScalingEnabled;
                    default:
                        return false;
                }
            }
        }
        protected int MinRandomModifiersNumber {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMinRandomModifiersScalingValue;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMinRandomModifiersScalingValue; ;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMinRandomModifiersScalingValue;
                    default:
                        return 1;
                }
            }
        }
        protected int MaxRandomModifiersNumber {
            get {
                switch (EnemyType) {
                    case ChallengeEnemyType.Boss:
                        return ConfigurationToUse.BossesMaxRandomModifiersScalingValue;
                    case ChallengeEnemyType.Miniboss:
                        return ConfigurationToUse.MinibossesMaxRandomModifiersScalingValue; ;
                    case ChallengeEnemyType.Regular:
                        return ConfigurationToUse.EnemiesMaxRandomModifiersScalingValue; ;
                    default:
                        return 1;
                }
            }
        }

        public Action? OnModifiersChange;
        public Action? OnDestroyActions;
        public bool AllowRepeating { get; set; }

        public bool CanBeTracked { get; set; }
        public bool UseCompositeTracking { get; set; }
        public bool UseProximityShow { get; set; }
        public bool RollOnStart { get; set; }
        private float showDistance = 400f;
        public float ShowDistance {
            get => showDistance;
        }
        public ChallengeEnemyType EnemyType { get; set; }

        public MonsterModifierController() {
            challengeConfiguration = ConfigurationToUse;
            monster = GetComponent<MonsterBase>();
            monsterController = GetComponent<ChallengeMonsterController>();
        }

        public void Init() {
            FindModifiers();
            if (!ApplicationCore.IsInBossMemoryMode && UseProximityShow) {
                PauseAll();
            }
            SetupModifiersLists();
            modifiersNumber = CalculateModifiersNumber(0);
            AllowRepeating = challengeConfiguration.IsRepeatingEnabled;
        }

        private void Start() {
            if(RollOnStart) {
                ForceRollBeforeEngage();
            }
        }

        public void OnRevival() {
            if (ConfigurationToUse.ModifiersStartDeath <= monsterController.KillCounter) {
                NotifyModifiersOnDeath();
                RollModifiers(monsterController.KillCounter);
                ApplyModifiers();
            }
        }

        public void OnDie() {
            NotifyModifiersOnDeath();
            Selected.Clear();
            ApplyModifiers();
        }

        public void ForceRollBeforeEngage() {
            if (_isFirstEngage && ConfigurationToUse.ModifiersStartDeath <= monsterController.KillCounter) {
                RollModifiers(monsterController.KillCounter);
                ApplyModifiers();

                _isFirstEngage = false;
            }
        }

        public void OnEngage() {
            if(_isFirstEngage && ConfigurationToUse.ModifiersStartDeath <= monsterController.KillCounter) {
                RollModifiers(monsterController.KillCounter);
                ApplyModifiers();

                _isFirstEngage = false;
            }

            NotifyEngage();

            if (UseCompositeTracking) {
                ResumeAll();
                BossChallengeMod.Instance.MonsterUIController.AddCompositeModifierController(this);
            } else if (UseProximityShow) {
                BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(this);
            }
        }

        public void NotifyEngage() {
            foreach (var item in Modifiers) {
                item.NotifyEngage();
            }
        }

        public void NotifyDisengage() {
            foreach (var item in Modifiers) {
                item.NotifyDisengage();
            }
        }

        public void OnDisengage() {
            NotifyDisengage();

            if (UseCompositeTracking) {
                PauseAll();
                BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
            } else if (UseProximityShow) {
                BossChallengeMod.Instance.MonsterUIController.ChangeModifiersController(null);
            }
        }


        public void GenerateAvailableMods() {
            Available.Clear();
            Available.AddRange(RollableModifiers);
        }

        public void CustomNotify(object message) {
            foreach (var item in Modifiers) {
                item.CustomNotify(message);
            }
        }

        public void RollModifiers(int iteration) {
            int modifiersNum = CalculateModifiersNumber(iteration);

            if (_isDied) {
                Selected.Clear();
                OnModifiersChange?.Invoke();
                return;
            }

            if (iteration < challengeConfiguration.ModifiersStartDeath) {
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
                for (int i = 0; i < modifiersNum && availabilities.Any(); i++) {
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

            OnModifiersChange?.Invoke();
        }

        public void ApplyModifiers() {
            if (_isDied) {
                return;
            }

            try {

                NotifyModifiers();

            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        public void FindModifiers() {
            Modifiers.Clear();
            Modifiers.AddRange(gameObject.GetComponentsInChildren<ModifierBase>());
            Modifiers.AddRange(MustIncludeModifiers);
        }

        public void OnDestroing() {
            OnDestroyActions?.Invoke();

            if (UseCompositeTracking) {
                BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
            }
        }

        private void OnDieHandler() {
            Selected.Clear();
            ApplyModifiers();

            if (UseCompositeTracking) {
                BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
            }

            playerSensor.gameObject.SetActive(false);
            _isDied = true;
            OnModifiersChange?.Invoke();
        }

        private void NotifyModifiers() {
            try {
                var selectedKeys = new HashSet<string>(Selected.Select(m => m.Key));
                foreach (var modifier in Modifiers) {
                    try {
                        if (selectedKeys.Contains(modifier.Key)) {
                            modifier.NotifyActivation();
                        } else {
                            modifier.NotifyDeactivation();
                        }
                    } catch (Exception ex) {
                        Log.Error($"Exception occured while applying {modifier.name}: {ex.Message}, {ex.StackTrace}");
                    }
                }
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        private void NotifyModifiersOnDeath() {
            try {
                int death = monsterController.KillCounter;
                foreach (var item in Modifiers) {
                    item.NotifyDeath(death);
                }
            } catch (Exception ex) {
                Log.Error($"{ex.Message}, {ex.StackTrace}");
            }
        }

        private int CalculateModifiersNumber(int iteration) {
            var result = 0;

            if(!EnableModifiersScaling && !EnableRandomModifiersScaling) {
                return 1;
            }

            if (EnableModifiersScaling) {
                int value = 0;

                int deathsToScale = Math.Max(0, iteration - ConfigurationToUse.ModifiersStartDeath);
                int stepsToScale = deathsToScale;

                if(StepsCap != -1) {
                    stepsToScale = Math.Min(deathsToScale, StepsCap);
                }

                value = (int)Math.Floor(InitialScalingValue + stepsToScale * ScalingStep);

                result += value;
            }

            if (EnableRandomModifiersScaling) {
                int value = BossChallengeMod.Random.Next(MinRandomModifiersNumber, MaxRandomModifiersNumber + 1);

                result += value;
            }

            result = Math.Max(0, result);

            return result;
        }

        public void ResetComponent() {
            playerSensor?.gameObject.SetActive(false);

            _isDied = false;
            _isStartRolled = false;
            _isFirstEngage = true;

            //if(UseCompositeTracking || UseProximityShow) {
            //    BossChallengeMod.Instance.MonsterUIController.RemoveCompositeModifierController(this);
            //}

            challengeConfiguration = ConfigurationToUse;
            FindModifiers();
            if (!ApplicationCore.IsInBossMemoryMode && UseProximityShow) {
                PauseAll();
            }

            SetupModifiersLists();

            modifiersNumber = CalculateModifiersNumber(0);
            AllowRepeating = challengeConfiguration.IsRepeatingEnabled;

            //if (challengeConfiguration.ModifiersStartFromDeath == 0 && !_isStartRolled) {
            //    RollModifiers(0);
            //    ApplyModifiers();
            //    _isStartRolled = true;
            //}

            playerSensor?.gameObject.SetActive(true);

            Selected.Clear();
            ApplyModifiers();
            OnDie();
        }

        private void SetupModifiersLists() {
            RollableModifiers.Clear();
            RollableModifiers.AddRange(ModifierConfigs.Where(mc => !mc.IsPersistentModifier && !mc.IsCombinationModifier));

            CombinationModifiers.Clear();
            foreach (var item in ModifierConfigs.Where(mc => mc.IsCombinationModifier)) {
                CombinationModifiers.TryAdd(item.Key, item);
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

        public int GetPriority() {
            return 0;
        }
    }
}
