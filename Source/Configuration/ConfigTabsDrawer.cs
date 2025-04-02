using BepInEx.Configuration;
using BossChallengeMod.Configuration.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Configuration {
    internal class ConfigTabsDrawer {
        private int _selectedTab = 0;
        public int SelectedTab {
            get { return _selectedTab; }
            set {
                if (value != _selectedTab) {
                    _selectedTab = value;

                    var selectedTabName = Tabs[_selectedTab];

                    OnSelectedTabIndexChanged?.Invoke(_selectedTab);

                    if(OnTabSelectedHandlers.TryGetValue(selectedTabName, out var actions)) {
                        actions?.Invoke();
                    }
                }
            }
        }

        public List<string> Tabs { get; set; } = new();

        public Dictionary<string, List<IField>> NestedFields { get; set; } = new();


        #region Callbacks

        public event Action<int>? OnSelectedTabIndexChanged;
        public Dictionary<string, Action> OnTabSelectedHandlers { get; set; } = new Dictionary<string, Action>();

        #endregion Callbacks

        public void AddField(string tab, IField field) {
            if (NestedFields.TryGetValue(tab, out var fields)) {
                fields.Add(field);
            } else {
                NestedFields.TryAdd(tab, new List<IField> { field });
            }
        }

        public void Draw(ConfigEntryBase entry) {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            SelectedTab = GUILayout.Toolbar(SelectedTab, Tabs.ToArray(), GUILayout.ExpandWidth(true));
            string selectedTabName = Tabs[SelectedTab];

            if(NestedFields.TryGetValue(selectedTabName, out var fields)) {
                foreach (var item in fields) {
                    item.Draw();
                }
            }

            GUILayout.EndVertical();
        }

        public ConfigurationManagerAttributes GetConfigAttributes() {
            return new ConfigurationManagerAttributes { CustomDrawer = Draw };
        }
    }
}
