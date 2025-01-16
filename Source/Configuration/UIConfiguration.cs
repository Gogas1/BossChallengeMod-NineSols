using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration {
    public class UIConfiguration {
        private bool _counterUIEnabled;
        public bool CounterUIEnabled {
            get => _counterUIEnabled;
            set {
                if (_counterUIEnabled != value) {
                    _counterUIEnabled = value;
                    OnCounterUIEnabledChanged?.Invoke(value);
                }
            }
        }
        public event Action<bool>? OnCounterUIEnabledChanged;

        private bool _useCustomCounterPosition;
        public bool UseCustomCounterPosition {
            get => _useCustomCounterPosition;
            set {
                if (_useCustomCounterPosition != value) {
                    _useCustomCounterPosition = value;
                    OnUseCustomCounterPositionChanged?.Invoke(value);
                }
            }
        }
        public event Action<bool>? OnUseCustomCounterPositionChanged;

        private float _counterXPos;
        public float CounterXPos {
            get => _counterXPos;
            set {
                if (_counterXPos != value) {
                    _counterXPos = value;
                    OnCounterXPosChanged?.Invoke(value);
                }
            }
        }
        public event Action<float>? OnCounterXPosChanged;

        private float _counterYPos;
        public float CounterYPos {
            get => _counterYPos;
            set {
                if (_counterYPos != value) {
                    _counterYPos = value;
                    OnCounterYPosChanged?.Invoke(value);
                }
            }
        }
        public event Action<float>? OnCounterYPosChanged;

        private float counterUIScale;
        public float CounterUIScale { 
            get => counterUIScale;
            set {
                counterUIScale = value;
                OnCounterUIScaleChanged?.Invoke(value);
            }
        }
        public event Action<float>? OnCounterUIScaleChanged;


        private bool _enableTimerUI;
        public bool TimerUIEnabled {
            get => _enableTimerUI;
            set {
                if (_enableTimerUI != value) {
                    _enableTimerUI = value;
                    OnEnableTimerUIChanged?.Invoke(value);
                }
            }
        }
        public event Action<bool>? OnEnableTimerUIChanged;

        private bool _useCustomTimerPosition;
        public bool UseCustomTimerPosition {
            get => _useCustomTimerPosition;
            set {
                if (_useCustomTimerPosition != value) {
                    _useCustomTimerPosition = value;
                    OnUseCustomTimerPositionChanged?.Invoke(value);
                }
            }
        }
        public event Action<bool>? OnUseCustomTimerPositionChanged;

        private float _timerXPos;
        public float TimerXPos {
            get => _timerXPos;
            set {
                if (_timerXPos != value) {
                    _timerXPos = value;
                    OnTimerXPosChanged?.Invoke(value);
                }
            }
        }
        public event Action<float>? OnTimerXPosChanged;

        private float _timerYPos;
        public float TimerYPos {
            get => _timerYPos;
            set {
                if (_timerYPos != value) {
                    _timerYPos = value;
                    OnTimerYPosChanged?.Invoke(value);
                }
            }
        }
        public event Action<float>? OnTimerYPosChanged;

        private float timerUIScale;
        public float TimerUIScale {
            get => timerUIScale;
            set {
                timerUIScale = value;
                OnTimerUIScaleChanged?.Invoke(value);
            }
        }
        public event Action<float>? OnTimerUIScaleChanged;


        private bool _enableTalismanModeUI;
        public bool TalismanModeUIEnabled {
            get => _enableTalismanModeUI;
            set {
                if (_enableTalismanModeUI != value) {
                    _enableTalismanModeUI = value;
                    OnEnableTalismanModeUIChanged?.Invoke(value);
                }
            }
        }
        public event Action<bool>? OnEnableTalismanModeUIChanged;

        private bool _useCustomTalismanModePosition;
        public bool UseCustomTalismanModePosition {
            get => _useCustomTalismanModePosition;
            set {
                if (_useCustomTalismanModePosition != value) {
                    _useCustomTalismanModePosition = value;
                    OnUseCustomTalismanModePositionChanged?.Invoke(value);
                }
            }
        }
        public event Action<bool>? OnUseCustomTalismanModePositionChanged;

        private float _talismanModeXPos;
        public float TalismanModeXPos {
            get => _talismanModeXPos;
            set {
                if (_talismanModeXPos != value) {
                    _talismanModeXPos = value;
                    OnTalismanModeXPosChanged?.Invoke(value);
                }
            }
        }
        public event Action<float>? OnTalismanModeXPosChanged;

        private float _talismanModeYPos;

        public float TalismanModeYPos {
            get => _talismanModeYPos;
            set {
                if (_talismanModeYPos != value) {
                    _talismanModeYPos = value;
                    OnTalismanModeYPosChanged?.Invoke(value);
                }
            }
        }
        public event Action<float>? OnTalismanModeYPosChanged;

        private float talismanUIScale;
        public float TalismanUIScale {
            get => talismanUIScale;
            set {
                talismanUIScale = value;
                OnTalismanUIScaleChanged?.Invoke(value);
            }
        }
        public event Action<float>? OnTalismanUIScaleChanged;
    }
}
