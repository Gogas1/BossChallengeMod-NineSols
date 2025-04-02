using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Configuration.Fields {
    public class BooleanField : ILabeledField<bool> {
        private float maxWidth = 0;
        private bool value;

        private string _label = "";
        private string? _tooltip = null;

        public string Label {
            get => _label;
            set => _label = value;
        }

        public string? Tooltip {
            get => _tooltip;
            set => _tooltip = value;
        }

        public event Action<object?>? ValueChanged;
        public event Action<bool>? FieldValueChanged;

        public void AddValueChangeHandler(Action<bool> handler) {
            FieldValueChanged += handler;
        }

        public void AddValueChangeHandler(Action<object?> handler) {
            ValueChanged += handler;
        }

        public void Draw() {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            if (!string.IsNullOrEmpty(_label)) {
                GUILayout.Label(new GUIContent(_label, _tooltip), GetLabelOptions());
            }

            bool result = GUILayout.Toggle(value, value ? "Enabled" : "Disabled", GetFieldOptions());

            if (result != value) {
                ValueChanged?.Invoke(value);
                FieldValueChanged?.Invoke(value);
                value = result;
            }

            GUILayout.EndHorizontal();
        }

        public void SetLabel(string label) {
            _label = label;
        }

        public void SetMaxWidth(float maxWidth) {
            this.maxWidth = maxWidth;
        }

        public void SetTooltip(string tooltip) {
            _tooltip = tooltip;
        }

        protected GUILayoutOption[] GetLabelOptions() {
            List<GUILayoutOption> options = new List<GUILayoutOption> { GUILayout.ExpandWidth(true) };

            float halfMaxWidth = this.maxWidth / 2;
            options.Add(GUILayout.MaxWidth(halfMaxWidth));

            return options.ToArray();
        }

        protected GUILayoutOption[] GetFieldOptions() {
            List<GUILayoutOption> options = new List<GUILayoutOption> { GUILayout.ExpandWidth(true) };

            float fieldMaxWidth = string.IsNullOrEmpty(_label) ? this.maxWidth : this.maxWidth / 2;
            options.Add(GUILayout.MaxWidth(fieldMaxWidth));

            return options.ToArray();
        }
    }
}
