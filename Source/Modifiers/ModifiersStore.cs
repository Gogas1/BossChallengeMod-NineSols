using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public class ModifiersStore {
        protected List<ModifierConfig> ModifiersConfigs = new List<ModifierConfig>();

        public List<ModifierConfig> Modifiers {
            get {
                return ModifiersConfigs.ToList();
            }
        }

        public ModifierConfigBuilder CreateModifierBuilder<T>(string key, string objectName) {
            var builder = new ModifierConfigBuilder(typeof(T), this);
            builder.AddKey(key);
            builder.AddObjectName(objectName);
            builder.AddIncompatibles([key]);

            return builder;
        }

        public void AddModifierConfig(ModifierConfig config) {
            if (ModifiersConfigs.Any(mc => mc.Key == config.Key)) {
                Log.Error($"Modifier with the key \"{config.Key}\" already added");
                return;
            }

            ModifiersConfigs.Add(config);
        }

        public class ModifierConfigBuilder {
            private ModifiersStore modifiersStore;

            public ModifierConfigBuilder(Type type, ModifiersStore modifiersStore) {
                modifierType = type;
                this.modifiersStore = modifiersStore;
            }

            private string key;
            private string objectName;
            private bool isPersistent;
            private bool isCombination;
            private Type modifierType;
            private ModifierControllerConfig? modifierControllerConfig = null;
            private List<string> incompatibles = new();
            private List<string> ignoredMonsters = new();
            private List<string> combinationModifiers = new();
            private Func<ModifierConfig, bool>? conditionPredicate = null;
            private Func<ModifierConfig, int, bool>? canBeRolledConditionPredicate = null;

            public ModifierConfigBuilder AddKey(string key) {
                this.key = key;

                return this;
            }

            public ModifierConfigBuilder AddObjectName(string name) {
                objectName = name;

                return this;
            }

            public ModifierConfigBuilder AddIncompatibles(IEnumerable<string> incompatibles) {
                this.incompatibles.AddRange(incompatibles);

                return this;
            }

            public ModifierConfigBuilder AddIgnoredMonsters(IEnumerable<string> ignoredMonsters) {
                this.ignoredMonsters.AddRange(ignoredMonsters);

                return this;
            }

            public ModifierConfigBuilder AddCombinationModifiers(IEnumerable<string> configs) {
                combinationModifiers.AddRange(configs);

                return this;
            }

            public ModifierConfigBuilder AddController(Type controllerType, bool isShared = false) {
                modifierControllerConfig = new ModifierControllerConfig(controllerType, isShared);

                return this;
            }

            public ModifierConfigBuilder SetPersistance(bool isPersistent) {
                this.isPersistent = isPersistent;

                return this;
            }

            public ModifierConfigBuilder SetIsCombination(bool isCombination) {
                this.isCombination = isCombination;

                return this;
            }

            public ModifierConfigBuilder AddConditionPredicate(Func<ModifierConfig, bool> predicate) {
                conditionPredicate = predicate;

                return this;
            }

            public ModifierConfigBuilder AddCanBeRolledConditionPredicate(Func<ModifierConfig, int, bool> predicate) {
                this.canBeRolledConditionPredicate = predicate;

                return this;
            }

            public ModifierConfig BuildAndAdd() {
                var config = Build();

                modifiersStore.AddModifierConfig(config);

                return config;
            }

            public ModifierConfig Build() {
                var config = new ModifierConfig(
                        key,
                        modifierType,
                        incompatibles,
                        ignoredMonsters,
                        objectName: objectName,
                        controllerConfig: modifierControllerConfig,
                        persistent: isPersistent,
                        conditionPredicate: conditionPredicate,
                        canBeRolledConditionPredicate: canBeRolledConditionPredicate,
                        combinationModifiers: combinationModifiers,
                        isCombinationModifier: isCombination);

                return config;
            }
        }
    }
}
