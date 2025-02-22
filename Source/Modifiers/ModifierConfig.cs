using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Modifiers {
    public record ModifierControllerConfig {
        public Type ControllerType { get; set; }
        public bool IsShared { get; set; }

        public ModifierControllerConfig(Type type, bool shared) {
            ControllerType = type;
            IsShared = shared;
        }
    }

    public class ModifierConfig {
        

        public string Key { get; set; } = string.Empty;
        public string ObjectName { get; set; } = string.Empty;
        public bool IsPersistentModifier { get; set; }
        public bool IsCombinationModifier { get; set; }
        public Type ModifierType { get; set; }
        public ModifierControllerConfig? ControllerConfig { get; private set; }
        public List<string> Incompatibles { get; } = new List<string>();
        public List<string> IgnoredMonsters { get; } = new List<string>();
        public List<string> CombinationModifiers { get; } = new List<string>();
        public Predicate<ModifierConfig>? CreateConditionPredicate { get; set; }

        public ModifierConfig() {
            
        }

        public ModifierConfig(
            string key, 
            Type modifierType, 
            string objectName = "Modifier",
            ModifierControllerConfig? controllerConfig = null,
            bool persistent = false,
            Predicate<ModifierConfig>? conditionPredicate = null,
            IEnumerable<string>? combinationModifiers = null,
            bool isCombinationModifier = false) {

            Key = key;
            ModifierType = modifierType;
            ObjectName = objectName;
            ControllerConfig = controllerConfig;
            IsPersistentModifier = persistent;
            IsCombinationModifier = isCombinationModifier;
            CreateConditionPredicate = conditionPredicate;

            if (combinationModifiers != null) {
                CombinationModifiers.AddRange(combinationModifiers);
            }
        }

        public ModifierConfig(
            string key, 
            Type modifierType, 
            IEnumerable<string> incompatibles, 
            string objectName = "Modifiers",
            ModifierControllerConfig? controllerConfig = null,
            bool persistent = false,
            Predicate<ModifierConfig>? conditionPredicate = null,
            IEnumerable<string>? combinationModifiers = null,
            bool isCombinationModifier = false) : this(key, modifierType, objectName, controllerConfig, persistent, conditionPredicate, combinationModifiers, isCombinationModifier) {
            Incompatibles = incompatibles.ToList();

        }

        public ModifierConfig(
            string key,
            Type modifierType, 
            IEnumerable<string> incompatibles, 
            IEnumerable<string> ignoredMonsters, 
            string objectName = "Modifiers",
            ModifierControllerConfig? controllerConfig = null,
            bool persistent = false,
            Predicate<ModifierConfig>? conditionPredicate = null,
            IEnumerable<string>? combinationModifiers = null,
            bool isCombinationModifier = false) : this(key, modifierType, incompatibles, objectName, controllerConfig, persistent, conditionPredicate, combinationModifiers, isCombinationModifier) {
            IgnoredMonsters = ignoredMonsters.ToList();
        }

        public override int GetHashCode() {
            return HashCode.Combine(Key.GetHashCode(), Incompatibles.GetHashCode(), IgnoredMonsters.GetHashCode());
        }
    }
}
