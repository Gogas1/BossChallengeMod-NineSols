using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration {
    public class ChallengeConfiguration {
        #region Modifiers

        private bool _isModifiersEnabled;
        public event Action<bool>? OnIsModifiersEnabledChanged;
        public bool IsModifiersEnabled {
            get => _isModifiersEnabled;
            set {
                if (_isModifiersEnabled != value) {
                    _isModifiersEnabled = value;
                    OnIsModifiersEnabledChanged?.Invoke(_isModifiersEnabled);
                }
            }
        }

        private int _modifiersStartDeath;
        public event Action<int>? OnModifiersStartDeathChanged;
        public int ModifiersStartDeath {
            get => _modifiersStartDeath;
            set {
                if (_modifiersStartDeath != value) {
                    _modifiersStartDeath = value;
                    OnModifiersStartDeathChanged?.Invoke(_modifiersStartDeath);
                }
            }
        }

        private bool _isRepeatingEnabled;
        public event Action<bool>? OnIsRepeatingEnabledChanged;
        public bool IsRepeatingEnabled {
            get => _isRepeatingEnabled;
            set {
                if (_isRepeatingEnabled != value) {
                    _isRepeatingEnabled = value;
                    OnIsRepeatingEnabledChanged?.Invoke(_isRepeatingEnabled);
                }
            }
        }

        private bool _isSpeedModifierEnabled;
        public event Action<bool>? OnIsSpeedModifierEnabledChanged;
        public bool IsSpeedModifierEnabled {
            get => _isSpeedModifierEnabled;
            set {
                if (_isSpeedModifierEnabled != value) {
                    _isSpeedModifierEnabled = value;
                    OnIsSpeedModifierEnabledChanged?.Invoke(_isSpeedModifierEnabled);
                }
            }
        }

        private bool _isTimerModifierEnabled;
        public event Action<bool>? OnIsTimerModifierEnabledChanged;
        public bool IsTimerModifierEnabled {
            get => _isTimerModifierEnabled;
            set {
                if (_isTimerModifierEnabled != value) {
                    _isTimerModifierEnabled = value;
                    OnIsTimerModifierEnabledChanged?.Invoke(_isTimerModifierEnabled);
                }
            }
        }

        private bool _isParryDamageModifierEnabled;
        public event Action<bool>? OnIsParryDamageModifierEnabledChanged;
        public bool IsParryDamageModifierEnabled {
            get => _isParryDamageModifierEnabled;
            set {
                if (_isParryDamageModifierEnabled != value) {
                    _isParryDamageModifierEnabled = value;
                    OnIsParryDamageModifierEnabledChanged?.Invoke(_isParryDamageModifierEnabled);
                }
            }
        }

        private bool _isDamageBuildupModifierEnabled;
        public event Action<bool>? OnIsDamageBuildupModifierEnabledChanged;
        public bool IsDamageBuildupModifierEnabled {
            get => _isDamageBuildupModifierEnabled;
            set {
                if (_isDamageBuildupModifierEnabled != value) {
                    _isDamageBuildupModifierEnabled = value;
                    OnIsDamageBuildupModifierEnabledChanged?.Invoke(_isDamageBuildupModifierEnabled);
                }
            }
        }

        private bool _isRegenerationModifierEnabled;
        public event Action<bool>? OnIsRegenerationModifierEnabledChanged;
        public bool IsRegenerationModifierEnabled {
            get => _isRegenerationModifierEnabled;
            set {
                if (_isRegenerationModifierEnabled != value) {
                    _isRegenerationModifierEnabled = value;
                    OnIsRegenerationModifierEnabledChanged?.Invoke(_isRegenerationModifierEnabled);
                }
            }
        }

        private bool _isKnockbackModifierEnabled;
        public event Action<bool>? OnIsKnockbackModifierEnabledChanged;
        public bool IsKnockbackModifierEnabled {
            get => _isKnockbackModifierEnabled;
            set {
                if (_isKnockbackModifierEnabled != value) {
                    _isKnockbackModifierEnabled = value;
                    OnIsKnockbackModifierEnabledChanged?.Invoke(_isKnockbackModifierEnabled);
                }
            }
        }

        private bool _isRandomArrowModifierEnabled;
        public event Action<bool>? OnIsRandomArrowModifierEnabledChanged;
        public bool IsRandomArrowModifierEnabled {
            get => _isRandomArrowModifierEnabled;
            set {
                if (_isRandomArrowModifierEnabled != value) {
                    _isRandomArrowModifierEnabled = value;
                    OnIsRandomArrowModifierEnabledChanged?.Invoke(_isRandomArrowModifierEnabled);
                }
            }
        }

        private bool _isRandomTalismanModifierEnabled;
        public event Action<bool>? OnIsRandomTalismanModifierEnabledChanged;
        public bool IsRandomTalismanModifierEnabled {
            get => _isRandomTalismanModifierEnabled;
            set {
                if (_isRandomTalismanModifierEnabled != value) {
                    _isRandomTalismanModifierEnabled = value;
                    OnIsRandomTalismanModifierEnabledChanged?.Invoke(_isRandomTalismanModifierEnabled);
                }
            }
        }

        private bool _isEnduranceModifierEnabled;
        public event Action<bool>? OnIsEnduranceModifierEnabledChanged;
        public bool IsEnduranceModifierEnabled {
            get => _isEnduranceModifierEnabled;
            set {
                if (_isEnduranceModifierEnabled != value) {
                    _isEnduranceModifierEnabled = value;
                    OnIsEnduranceModifierEnabledChanged?.Invoke(_isEnduranceModifierEnabled);
                }
            }
        }

        private bool _isQiShieldModifierEnabled;
        public event Action<bool>? OnIsQiShieldModifierEnabledChanged;
        public bool IsQiShieldModifierEnabled {
            get => _isQiShieldModifierEnabled;
            set {
                if (_isQiShieldModifierEnabled != value) {
                    _isQiShieldModifierEnabled = value;
                    OnIsQiShieldModifierEnabledChanged?.Invoke(_isQiShieldModifierEnabled);
                }
            }
        }

        private bool _isCooldownShieldModifierEnabled;
        public event Action<bool>? OnIsCooldownShieldModifierEnabledChanged;
        public bool IsCooldownShieldModifierEnabled {
            get => _isCooldownShieldModifierEnabled;
            set {
                if (_isCooldownShieldModifierEnabled != value) {
                    _isCooldownShieldModifierEnabled = value;
                    OnIsCooldownShieldModifierEnabledChanged?.Invoke(_isCooldownShieldModifierEnabled);
                }
            }
        }

        private bool _isQiOverloadModifierEnabled;
        public event Action<bool>? OnIsQiOverloadModifierEnabledChanged;
        public bool IsQiOverloadModifierEnabled {
            get => _isQiOverloadModifierEnabled;
            set {
                if (_isQiOverloadModifierEnabled != value) {
                    _isQiOverloadModifierEnabled = value;
                    OnIsQiOverloadModifierEnabledChanged?.Invoke(_isQiOverloadModifierEnabled);
                }
            }
        }

        private bool _isDistanceShieldModifierEnabled;
        public event Action<bool>? OnIsDistanceShieldModifierEnabledChanged;
        public bool IsDistanceShieldModifierEnabled {
            get => _isDistanceShieldModifierEnabled;
            set {
                if (_isDistanceShieldModifierEnabled != value) {
                    _isDistanceShieldModifierEnabled = value;
                    OnIsDistanceShieldModifierEnabledChanged?.Invoke(_isDistanceShieldModifierEnabled);
                }
            }
        }

        private bool _isYanlaoGunModifierEnabled;
        public event Action<bool>? OnIsYanlaoGunModifierEnabledChanged;
        public bool IsYanlaoGunModifierEnabled {
            get => _isYanlaoGunModifierEnabled;
            set {
                if (_isYanlaoGunModifierEnabled != value) {
                    _isYanlaoGunModifierEnabled = value;
                    OnIsYanlaoGunModifierEnabledChanged?.Invoke(_isYanlaoGunModifierEnabled);
                }
            }
        }

        private bool _isQiBombModifierEnabled;
        public event Action<bool>? OnIsQiBombModifierEnabledChanged;
        public bool IsQiBombModifierEnabled {
            get => _isQiBombModifierEnabled;
            set {
                if (_isQiBombModifierEnabled != value) {
                    _isQiBombModifierEnabled = value;
                    OnIsQiBombModifierEnabledChanged?.Invoke(_isQiBombModifierEnabled);
                }
            }
        }

        private bool _isShieldBreakBombModifierEnabled;
        public event Action<bool>? OnIsShieldBreakBombModifierEnabledChanged;
        public bool IsShieldBreakBombModifierEnabled {
            get => _isShieldBreakBombModifierEnabled;
            set {
                if (_isShieldBreakBombModifierEnabled != value) {
                    _isShieldBreakBombModifierEnabled = value;
                    OnIsShieldBreakBombModifierEnabledChanged?.Invoke(_isShieldBreakBombModifierEnabled);
                }
            }
        }

        private bool _isQiOverloadBombModifierEnabled;
        public event Action<bool>? OnIsQiOverloadBombModifierEnabledChanged;
        public bool IsQiOverloadBombModifierEnabled {
            get => _isQiOverloadBombModifierEnabled;
            set {
                if (_isQiOverloadBombModifierEnabled != value) {
                    _isQiOverloadBombModifierEnabled = value;
                    OnIsQiOverloadBombModifierEnabledChanged?.Invoke(_isQiOverloadBombModifierEnabled);
                }
            }
        }

        private bool _isQiDepletionBombModifierEnabled;
        public event Action<bool>? OnIsQiDepletionBombModifierEnabledChanged;
        public bool IsQiDepletionBombModifierEnabled {
            get => _isQiDepletionBombModifierEnabled;
            set {
                if (_isQiDepletionBombModifierEnabled != value) {
                    _isQiDepletionBombModifierEnabled = value;
                    OnIsQiDepletionBombModifierEnabledChanged?.Invoke(_isQiDepletionBombModifierEnabled);
                }
            }
        }

        private bool _isCooldownBombModifierEnabled;
        public event Action<bool>? OnIsCooldownBombModifierEnabledChanged;
        public bool IsCooldownBombModifierEnabled {
            get => _isCooldownBombModifierEnabled;
            set {
                if (_isCooldownBombModifierEnabled != value) {
                    _isCooldownBombModifierEnabled = value;
                    OnIsCooldownBombModifierEnabledChanged?.Invoke(_isCooldownBombModifierEnabled);
                }
            }
        }

        #endregion Modifiers

        #region Bosses

        private bool _affectBosses;
        public event Action<bool>? OnAffectBossesChanged;
        public bool AffectBosses {
            get => _affectBosses;
            set {
                if (_affectBosses != value) {
                    _affectBosses = value;
                    OnAffectBossesChanged?.Invoke(_affectBosses);
                }
            }
        }

        private int _maxBossCycles;
        public event Action<int>? OnMaxBossCyclesChanged;
        public int MaxBossCycles {
            get => _maxBossCycles;
            set {
                if (_maxBossCycles != value) {
                    _maxBossCycles = value;
                    OnMaxBossCyclesChanged?.Invoke(_maxBossCycles);
                }
            }
        }

        private bool _bossesIsSpeedScalingEnabled;
        public event Action<bool>? OnBossesIsSpeedScalingEnabledChanged;
        public bool BossesIsSpeedScalingEnabled {
            get => _bossesIsSpeedScalingEnabled;
            set {
                if (_bossesIsSpeedScalingEnabled != value) {
                    _bossesIsSpeedScalingEnabled = value;
                    OnBossesIsSpeedScalingEnabledChanged?.Invoke(_bossesIsSpeedScalingEnabled);
                }
            }
        }

        private float _bossesMinSpeedScalingValue;
        public event Action<float>? OnBossesMinSpeedScalingValueChanged;
        public float BossesMinSpeedScalingValue {
            get => _bossesMinSpeedScalingValue;
            set {
                if (_bossesMinSpeedScalingValue != value) {
                    _bossesMinSpeedScalingValue = value;
                    OnBossesMinSpeedScalingValueChanged?.Invoke(_bossesMinSpeedScalingValue);
                }
            }
        }

        private float _bossesSpeedScalingStepValue;
        public event Action<float>? OnBossesSpeedScalingStepValueChanged;
        public float BossesSpeedScalingStepValue {
            get => _bossesSpeedScalingStepValue;
            set {
                if (_bossesSpeedScalingStepValue != value) {
                    _bossesSpeedScalingStepValue = value;
                    OnBossesSpeedScalingStepValueChanged?.Invoke(_bossesSpeedScalingStepValue);
                }
            }
        }

        private int _bossesSpeedStepsCapValue;
        public event Action<int>? OnBossesSpeedStepsCapValueChanged;
        public int BossesSpeedStepsCapValue {
            get => _bossesSpeedStepsCapValue;
            set {
                if (_bossesSpeedStepsCapValue != value) {
                    _bossesSpeedStepsCapValue = value;
                    OnBossesSpeedStepsCapValueChanged?.Invoke(_bossesSpeedStepsCapValue);
                }
            }
        }

        private bool _bossesIsModifiersScalingEnabled;
        public event Action<bool>? OnBossesIsModifiersScalingEnabledChanged;
        public bool BossesIsModifiersScalingEnabled {
            get => _bossesIsModifiersScalingEnabled;
            set {
                if (_bossesIsModifiersScalingEnabled != value) {
                    _bossesIsModifiersScalingEnabled = value;
                    OnBossesIsModifiersScalingEnabledChanged?.Invoke(_bossesIsModifiersScalingEnabled);
                }
            }
        }

        private float _bossesMinModifiersNumber;
        public event Action<float>? OnBossesMinModifiersNumberChanged;
        public float BossesMinModifiersNumber {
            get => _bossesMinModifiersNumber;
            set {
                if (_bossesMinModifiersNumber != value) {
                    _bossesMinModifiersNumber = value;
                    OnBossesMinModifiersNumberChanged?.Invoke(_bossesMinModifiersNumber);
                }
            }
        }

        private float _bossesModifiersScalingStepValue;
        public event Action<float>? OnBossesModifiersScalingStepValueChanged;
        public float BossesModifiersScalingStepValue {
            get => _bossesModifiersScalingStepValue;
            set {
                if (_bossesModifiersScalingStepValue != value) {
                    _bossesModifiersScalingStepValue = value;
                    OnBossesModifiersScalingStepValueChanged?.Invoke(_bossesModifiersScalingStepValue);
                }
            }
        }

        private int _bossesModifiersStepsCapValue;
        public event Action<int>? OnBossesModifiersStepsCapValueChanged;
        public int BossesModifiersStepsCapValue {
            get => _bossesModifiersStepsCapValue;
            set {
                if (_bossesModifiersStepsCapValue != value) {
                    _bossesModifiersStepsCapValue = value;
                    OnBossesModifiersStepsCapValueChanged?.Invoke(_bossesModifiersStepsCapValue);
                }
            }
        }

        private bool _bossesIsRandomSpeedScalingEnabled;
        public event Action<bool>? OnBossesIsRandomSpeedScalingEnabledChanged;
        public bool BossesIsRandomSpeedScalingEnabled {
            get => _bossesIsRandomSpeedScalingEnabled;
            set {
                if (_bossesIsRandomSpeedScalingEnabled != value) {
                    _bossesIsRandomSpeedScalingEnabled = value;
                    OnBossesIsRandomSpeedScalingEnabledChanged?.Invoke(_bossesIsRandomSpeedScalingEnabled);
                }
            }
        }

        private float _bossesMinRandomSpeedScalingValue;
        public event Action<float>? OnBossesMinRandomSpeedScalingValueChanged;
        public float BossesMinRandomSpeedScalingValue {
            get => _bossesMinRandomSpeedScalingValue;
            set {
                if (_bossesMinRandomSpeedScalingValue != value) {
                    _bossesMinRandomSpeedScalingValue = value;
                    OnBossesMinRandomSpeedScalingValueChanged?.Invoke(_bossesMinRandomSpeedScalingValue);
                }
            }
        }

        private float _bossesMaxRandomSpeedScalingValue;
        public event Action<float>? OnBossesMaxRandomSpeedScalingValueChanged;
        public float BossesMaxRandomSpeedScalingValue {
            get => _bossesMaxRandomSpeedScalingValue;
            set {
                if (_bossesMaxRandomSpeedScalingValue != value) {
                    _bossesMaxRandomSpeedScalingValue = value;
                    OnBossesMaxRandomSpeedScalingValueChanged?.Invoke(_bossesMaxRandomSpeedScalingValue);
                }
            }
        }

        private bool _bossesIsRandomModifiersScalingEnabled;
        public event Action<bool>? OnBossesIsRandomModifiersScalingEnabledChanged;
        public bool BossesIsRandomModifiersScalingEnabled {
            get => _bossesIsRandomModifiersScalingEnabled;
            set {
                if (_bossesIsRandomModifiersScalingEnabled != value) {
                    _bossesIsRandomModifiersScalingEnabled = value;
                    OnBossesIsRandomModifiersScalingEnabledChanged?.Invoke(_bossesIsRandomModifiersScalingEnabled);
                }
            }
        }

        private int _bossesMinRandomModifiersScalingValue;
        public event Action<int>? OnBossesMinRandomModifiersScalingValueChanged;
        public int BossesMinRandomModifiersScalingValue {
            get => _bossesMinRandomModifiersScalingValue;
            set {
                if (_bossesMinRandomModifiersScalingValue != value) {
                    _bossesMinRandomModifiersScalingValue = value;
                    OnBossesMinRandomModifiersScalingValueChanged?.Invoke(_bossesMinRandomModifiersScalingValue);
                }
            }
        }

        private int _bossesMaxRandomModifiersScalingValue;
        public event Action<int>? OnBossesMaxRandomModifiersScalingValueChanged;
        public int BossesMaxRandomModifiersScalingValue {
            get => _bossesMaxRandomModifiersScalingValue;
            set {
                if (_bossesMaxRandomModifiersScalingValue != value) {
                    _bossesMaxRandomModifiersScalingValue = value;
                    OnBossesMaxRandomModifiersScalingValueChanged?.Invoke(_bossesMaxRandomModifiersScalingValue);
                }
            }
        }

        #endregion Bosses

        #region Minibosses

        private bool _affectMinibosses;
        public event Action<bool>? OnAffectMinibossesChanged;
        public bool AffectMinibosses {
            get => _affectMinibosses;
            set {
                if (_affectMinibosses != value) {
                    _affectMinibosses = value;
                    OnAffectMinibossesChanged?.Invoke(_affectMinibosses);
                }
            }
        }

        private int _maxMinibossCycles;
        public event Action<int>? OnMaxMinibossCyclesChanged;
        public int MaxMinibossCycles {
            get => _maxMinibossCycles;
            set {
                if (_maxMinibossCycles != value) {
                    _maxMinibossCycles = value;
                    OnMaxMinibossCyclesChanged?.Invoke(_maxMinibossCycles);
                }
            }
        }

        private bool _minibossesIsSpeedScalingEnabled;
        public event Action<bool>? OnMinibossesIsSpeedScalingEnabledChanged;
        public bool MinibossesIsSpeedScalingEnabled {
            get => _minibossesIsSpeedScalingEnabled;
            set {
                if (_minibossesIsSpeedScalingEnabled != value) {
                    _minibossesIsSpeedScalingEnabled = value;
                    OnMinibossesIsSpeedScalingEnabledChanged?.Invoke(_minibossesIsSpeedScalingEnabled);
                }
            }
        }

        private float _minibossesMinSpeedScalingValue;
        public event Action<float>? OnMinibossesMinSpeedScalingValueChanged;
        public float MinibossesMinSpeedScalingValue {
            get => _minibossesMinSpeedScalingValue;
            set {
                if (_minibossesMinSpeedScalingValue != value) {
                    _minibossesMinSpeedScalingValue = value;
                    OnMinibossesMinSpeedScalingValueChanged?.Invoke(_minibossesMinSpeedScalingValue);
                }
            }
        }

        private float _minibossesSpeedScalingStepValue;
        public event Action<float>? OnMinibossesSpeedScalingStepValueChanged;
        public float MinibossesSpeedScalingStepValue {
            get => _minibossesSpeedScalingStepValue;
            set {
                if (_minibossesSpeedScalingStepValue != value) {
                    _minibossesSpeedScalingStepValue = value;
                    OnMinibossesSpeedScalingStepValueChanged?.Invoke(_minibossesSpeedScalingStepValue);
                }
            }
        }

        private int _minibossesSpeedStepsCapValue;
        public event Action<int>? OnMinibossesSpeedStepsCapValueChanged;
        public int MinibossesSpeedStepsCapValue {
            get => _minibossesSpeedStepsCapValue;
            set {
                if (_minibossesSpeedStepsCapValue != value) {
                    _minibossesSpeedStepsCapValue = value;
                    OnMinibossesSpeedStepsCapValueChanged?.Invoke(_minibossesSpeedStepsCapValue);
                }
            }
        }

        private bool _minibossesIsModifiersScalingEnabled;
        public event Action<bool>? OnMinibossesIsModifiersScalingEnabledChanged;
        public bool MinibossesIsModifiersScalingEnabled {
            get => _minibossesIsModifiersScalingEnabled;
            set {
                if (_minibossesIsModifiersScalingEnabled != value) {
                    _minibossesIsModifiersScalingEnabled = value;
                    OnMinibossesIsModifiersScalingEnabledChanged?.Invoke(_minibossesIsModifiersScalingEnabled);
                }
            }
        }

        private float _minibossesMinModifiersNumber;
        public event Action<float>? OnMinibossesMinModifiersNumberChanged;
        public float MinibossesMinModifiersNumber {
            get => _minibossesMinModifiersNumber;
            set {
                if (_minibossesMinModifiersNumber != value) {
                    _minibossesMinModifiersNumber = value;
                    OnMinibossesMinModifiersNumberChanged?.Invoke(_minibossesMinModifiersNumber);
                }
            }
        }

        private float _minibossesModifiersScalingStepValue;
        public event Action<float>? OnMinibossesModifiersScalingStepValueChanged;
        public float MinibossesModifiersScalingStepValue {
            get => _minibossesModifiersScalingStepValue;
            set {
                if (_minibossesModifiersScalingStepValue != value) {
                    _minibossesModifiersScalingStepValue = value;
                    OnMinibossesModifiersScalingStepValueChanged?.Invoke(_minibossesModifiersScalingStepValue);
                }
            }
        }

        private int _minibossesModifiersStepsCapValue;
        public event Action<int>? OnMinibossesModifiersStepsCapValueChanged;
        public int MinibossesModifiersStepsCapValue {
            get => _minibossesModifiersStepsCapValue;
            set {
                if (_minibossesModifiersStepsCapValue != value) {
                    _minibossesModifiersStepsCapValue = value;
                    OnMinibossesModifiersStepsCapValueChanged?.Invoke(_minibossesModifiersStepsCapValue);
                }
            }
        }

        private bool _minibossesIsRandomSpeedScalingEnabled;
        public event Action<bool>? OnMinibossesIsRandomSpeedScalingEnabledChanged;
        public bool MinibossesIsRandomSpeedScalingEnabled {
            get => _minibossesIsRandomSpeedScalingEnabled;
            set {
                if (_minibossesIsRandomSpeedScalingEnabled != value) {
                    _minibossesIsRandomSpeedScalingEnabled = value;
                    OnMinibossesIsRandomSpeedScalingEnabledChanged?.Invoke(_minibossesIsRandomSpeedScalingEnabled);
                }
            }
        }

        private float _minibossesMinRandomSpeedScalingValue;
        public event Action<float>? OnMinibossesMinRandomSpeedScalingValueChanged;
        public float MinibossesMinRandomSpeedScalingValue {
            get => _minibossesMinRandomSpeedScalingValue;
            set {
                if (_minibossesMinRandomSpeedScalingValue != value) {
                    _minibossesMinRandomSpeedScalingValue = value;
                    OnMinibossesMinRandomSpeedScalingValueChanged?.Invoke(_minibossesMinRandomSpeedScalingValue);
                }
            }
        }

        private float _minibossesMaxRandomSpeedScalingValue;
        public event Action<float>? OnMinibossesMaxRandomSpeedScalingValueChanged;
        public float MinibossesMaxRandomSpeedScalingValue {
            get => _minibossesMaxRandomSpeedScalingValue;
            set {
                if (_minibossesMaxRandomSpeedScalingValue != value) {
                    _minibossesMaxRandomSpeedScalingValue = value;
                    OnMinibossesMaxRandomSpeedScalingValueChanged?.Invoke(_minibossesMaxRandomSpeedScalingValue);
                }
            }
        }

        private bool _minibossesIsRandomModifiersScalingEnabled;
        public event Action<bool>? OnMinibossesIsRandomModifiersScalingEnabledChanged;
        public bool MinibossesIsRandomModifiersScalingEnabled {
            get => _minibossesIsRandomModifiersScalingEnabled;
            set {
                if (_minibossesIsRandomModifiersScalingEnabled != value) {
                    _minibossesIsRandomModifiersScalingEnabled = value;
                    OnMinibossesIsRandomModifiersScalingEnabledChanged?.Invoke(_minibossesIsRandomModifiersScalingEnabled);
                }
            }
        }

        private int _minibossesMinRandomModifiersScalingValue;
        public event Action<int>? OnMinibossesMinRandomModifiersScalingValueChanged;
        public int MinibossesMinRandomModifiersScalingValue {
            get => _minibossesMinRandomModifiersScalingValue;
            set {
                if (_minibossesMinRandomModifiersScalingValue != value) {
                    _minibossesMinRandomModifiersScalingValue = value;
                    OnMinibossesMinRandomModifiersScalingValueChanged?.Invoke(_minibossesMinRandomModifiersScalingValue);
                }
            }
        }

        private int _minibossesMaxRandomModifiersScalingValue;
        public event Action<int>? OnMinibossesMaxRandomModifiersScalingValueChanged;
        public int MinibossesMaxRandomModifiersScalingValue {
            get => _minibossesMaxRandomModifiersScalingValue;
            set {
                if (_minibossesMaxRandomModifiersScalingValue != value) {
                    _minibossesMaxRandomModifiersScalingValue = value;
                    OnMinibossesMaxRandomModifiersScalingValueChanged?.Invoke(_minibossesMaxRandomModifiersScalingValue);
                }
            }
        }

        #endregion Minibosses

        #region Enemies

        private bool _affectEnemies;
        public event Action<bool>? OnAffectEnemiesChanged;
        public bool AffectEnemies {
            get => _affectEnemies;
            set {
                if (_affectEnemies != value) {
                    _affectEnemies = value;
                    OnAffectEnemiesChanged?.Invoke(_affectEnemies);
                }
            }
        }

        private int _maxEnemyCycles;
        public event Action<int>? OnMaxEnemyCyclesChanged;
        public int MaxEnemyCycles {
            get => _maxEnemyCycles;
            set {
                if (_maxEnemyCycles != value) {
                    _maxEnemyCycles = value;
                    OnMaxEnemyCyclesChanged?.Invoke(_maxEnemyCycles);
                }
            }
        }

        private bool _enemiesIsSpeedScalingEnabled;
        public event Action<bool>? OnEnemiesIsSpeedScalingEnabledChanged;
        public bool EnemiesIsSpeedScalingEnabled {
            get => _enemiesIsSpeedScalingEnabled;
            set {
                if (_enemiesIsSpeedScalingEnabled != value) {
                    _enemiesIsSpeedScalingEnabled = value;
                    OnEnemiesIsSpeedScalingEnabledChanged?.Invoke(_enemiesIsSpeedScalingEnabled);
                }
            }
        }

        private float _enemiesMinSpeedScalingValue;
        public event Action<float>? OnEnemiesMinSpeedScalingValueChanged;
        public float EnemiesMinSpeedScalingValue {
            get => _enemiesMinSpeedScalingValue;
            set {
                if (_enemiesMinSpeedScalingValue != value) {
                    _enemiesMinSpeedScalingValue = value;
                    OnEnemiesMinSpeedScalingValueChanged?.Invoke(_enemiesMinSpeedScalingValue);
                }
            }
        }

        private float _enemiesSpeedScalingStepValue;
        public event Action<float>? OnEnemiesSpeedScalingStepValueChanged;
        public float EnemiesSpeedScalingStepValue {
            get => _enemiesSpeedScalingStepValue;
            set {
                if (_enemiesSpeedScalingStepValue != value) {
                    _enemiesSpeedScalingStepValue = value;
                    OnEnemiesSpeedScalingStepValueChanged?.Invoke(_enemiesSpeedScalingStepValue);
                }
            }
        }

        private int _enemiesSpeedStepsCapValue;
        public event Action<int>? OnEnemiesSpeedStepsCapValueChanged;
        public int EnemiesSpeedStepsCapValue {
            get => _enemiesSpeedStepsCapValue;
            set {
                if (_enemiesSpeedStepsCapValue != value) {
                    _enemiesSpeedStepsCapValue = value;
                    OnEnemiesSpeedStepsCapValueChanged?.Invoke(_enemiesSpeedStepsCapValue);
                }
            }
        }

        private bool _enemiesIsModifiersScalingEnabled;
        public event Action<bool>? OnEnemiesIsModifiersScalingEnabledChanged;
        public bool EnemiesIsModifiersScalingEnabled {
            get => _enemiesIsModifiersScalingEnabled;
            set {
                if (_enemiesIsModifiersScalingEnabled != value) {
                    _enemiesIsModifiersScalingEnabled = value;
                    OnEnemiesIsModifiersScalingEnabledChanged?.Invoke(_enemiesIsModifiersScalingEnabled);
                }
            }
        }

        private float _enemiesMinModifiersNumber;
        public event Action<float>? OnEnemiesMinModifiersNumberChanged;
        public float EnemiesMinModifiersNumber {
            get => _enemiesMinModifiersNumber;
            set {
                if (_enemiesMinModifiersNumber != value) {
                    _enemiesMinModifiersNumber = value;
                    OnEnemiesMinModifiersNumberChanged?.Invoke(_enemiesMinModifiersNumber);
                }
            }
        }

        private float _enemiesModifiersScalingStepValue;
        public event Action<float>? OnEnemiesModifiersScalingStepValueChanged;
        public float EnemiesModifiersScalingStepValue {
            get => _enemiesModifiersScalingStepValue;
            set {
                if (_enemiesModifiersScalingStepValue != value) {
                    _enemiesModifiersScalingStepValue = value;
                    OnEnemiesModifiersScalingStepValueChanged?.Invoke(_enemiesModifiersScalingStepValue);
                }
            }
        }

        private int _enemiesModifiersStepsCapValue;
        public event Action<int>? OnEnemiesModifiersStepsCapValueChanged;
        public int EnemiesModifiersStepsCapValue {
            get => _enemiesModifiersStepsCapValue;
            set {
                if (_enemiesModifiersStepsCapValue != value) {
                    _enemiesModifiersStepsCapValue = value;
                    OnEnemiesModifiersStepsCapValueChanged?.Invoke(_enemiesModifiersStepsCapValue);
                }
            }
        }

        private bool _enemiesIsRandomSpeedScalingEnabled;
        public event Action<bool>? OnEnemiesIsRandomSpeedScalingEnabledChanged;
        public bool EnemiesIsRandomSpeedScalingEnabled {
            get => _enemiesIsRandomSpeedScalingEnabled;
            set {
                if (_enemiesIsRandomSpeedScalingEnabled != value) {
                    _enemiesIsRandomSpeedScalingEnabled = value;
                    OnEnemiesIsRandomSpeedScalingEnabledChanged?.Invoke(_enemiesIsRandomSpeedScalingEnabled);
                }
            }
        }

        private float _enemiesMinRandomSpeedScalingValue;
        public event Action<float>? OnEnemiesMinRandomSpeedScalingValueChanged;
        public float EnemiesMinRandomSpeedScalingValue {
            get => _enemiesMinRandomSpeedScalingValue;
            set {
                if (_enemiesMinRandomSpeedScalingValue != value) {
                    _enemiesMinRandomSpeedScalingValue = value;
                    OnEnemiesMinRandomSpeedScalingValueChanged?.Invoke(_enemiesMinRandomSpeedScalingValue);
                }
            }
        }

        private float _enemiesMaxRandomSpeedScalingValue;
        public event Action<float>? OnEnemiesMaxRandomSpeedScalingValueChanged;
        public float EnemiesMaxRandomSpeedScalingValue {
            get => _enemiesMaxRandomSpeedScalingValue;
            set {
                if (_enemiesMaxRandomSpeedScalingValue != value) {
                    _enemiesMaxRandomSpeedScalingValue = value;
                    OnEnemiesMaxRandomSpeedScalingValueChanged?.Invoke(_enemiesMaxRandomSpeedScalingValue);
                }
            }
        }

        private bool _enemiesIsRandomModifiersScalingEnabled;
        public event Action<bool>? OnEnemiesIsRandomModifiersScalingEnabledChanged;
        public bool EnemiesIsRandomModifiersScalingEnabled {
            get => _enemiesIsRandomModifiersScalingEnabled;
            set {
                if (_enemiesIsRandomModifiersScalingEnabled != value) {
                    _enemiesIsRandomModifiersScalingEnabled = value;
                    OnEnemiesIsRandomModifiersScalingEnabledChanged?.Invoke(_enemiesIsRandomModifiersScalingEnabled);
                }
            }
        }

        private int _enemiesMinRandomModifiersScalingValue;
        public event Action<int>? OnEnemiesMinRandomModifiersScalingValueChanged;
        public int EnemiesMinRandomModifiersScalingValue {
            get => _enemiesMinRandomModifiersScalingValue;
            set {
                if (_enemiesMinRandomModifiersScalingValue != value) {
                    _enemiesMinRandomModifiersScalingValue = value;
                    OnEnemiesMinRandomModifiersScalingValueChanged?.Invoke(_enemiesMinRandomModifiersScalingValue);
                }
            }
        }

        private int _enemiesMaxRandomModifiersScalingValue;
        public event Action<int>? OnEnemiesMaxRandomModifiersScalingValueChanged;
        public int EnemiesMaxRandomModifiersScalingValue {
            get => _enemiesMaxRandomModifiersScalingValue;
            set {
                if (_enemiesMaxRandomModifiersScalingValue != value) {
                    _enemiesMaxRandomModifiersScalingValue = value;
                    OnEnemiesMaxRandomModifiersScalingValueChanged?.Invoke(_enemiesMaxRandomModifiersScalingValue);
                }
            }
        }

        #endregion Enemies
    }
}
