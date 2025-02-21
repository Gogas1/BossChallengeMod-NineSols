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
            var builder = new ModifierConfigBuilder(ModifiersConfigs, typeof(T));
            builder.AddKey(key);
            builder.AddObjectName(objectName);
            builder.AddIncompatibles([key]);

            return builder;
        }

        public void AddModifier<T>(
            string key, 
            string objectName = "Modifier", 
            Type? monsterController = null,
            bool persistent = false) where T : ModifierBase {
            AddModifier<T>(key, [key], [], objectName, monsterController, persistent);
        }

        public void AddModifier<T>(
            string key, 
            IEnumerable<string> 
            incompatibles, 
            string objectName = "Modifier", 
            Type? monsterController = null,
            bool persistent = false) where T : ModifierBase {
            AddModifier<T>(key, incompatibles, [], objectName, monsterController, persistent);
        }

        public void AddModifier<T>(
            string key, 
            IEnumerable<string> incompatibles, 
            IEnumerable<string> ignoredMonsters, 
            string objectName = "Modifier", 
            Type? monsterController = null,
            bool persistent = false) where T : ModifierBase {
            ModifiersConfigs.Add(new ModifierConfig(key, typeof(T), incompatibles, ignoredMonsters, objectName: objectName, persistent: persistent));
        }

        

        public class ModifierConfigBuilder {
            private List<ModifierConfig> ModifiersCollectionRef;

            public ModifierConfigBuilder(List<ModifierConfig> modifiersCollectionRef, Type type) {
                ModifiersCollectionRef = modifiersCollectionRef;
                modifierType = type;
            }

            private string key;
            private string objectName;
            private bool isPersistent;
            private Type modifierType;
            private ModifierControllerConfig? modifierControllerConfig = null;
            private List<string> incompatibles = new();
            private List<string> ignoredMonsters = new();

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

            public ModifierConfigBuilder AddController(Type controllerType, bool isShared = false) {
                modifierControllerConfig = new ModifierControllerConfig(controllerType, isShared);

                return this;
            }

            public ModifierConfigBuilder SetPersistance(bool isPersistent) {
                this.isPersistent = isPersistent;

                return this;
            }

            public ModifierConfig BuildAndAdd() {
                var config = new ModifierConfig(
                        key,
                        modifierType,
                        incompatibles,
                        ignoredMonsters,
                        objectName: objectName,
                        controllerConfig: modifierControllerConfig,
                        persistent: isPersistent);

                ModifiersCollectionRef.Add(config);

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
                        persistent: isPersistent);

                return config;
            }
        }
    }
}
