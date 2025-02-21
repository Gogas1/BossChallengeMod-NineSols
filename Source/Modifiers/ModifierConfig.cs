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
        public Type ModifierType { get; set; }
        public ModifierControllerConfig? ControllerConfig { get; private set; }
        public List<string> Incompatibles { get; } = new List<string>();
        public List<string> IgnoredMonsters { get; } = new List<string>();

        public ModifierConfig() {
            
        }

        public ModifierConfig(
            string key, 
            Type modifierType, 
            string objectName = "Modifier",
            ModifierControllerConfig? controllerConfig = null,
            bool persistent = false) {

            Key = key;
            ModifierType = modifierType;
            ObjectName = objectName;
            ControllerConfig = controllerConfig;
            IsPersistentModifier = persistent;
        }

        public ModifierConfig(
            string key, 
            Type modifierType, 
            IEnumerable<string> incompatibles, 
            string objectName = "Modifiers",
            ModifierControllerConfig? controllerConfig = null,
            bool persistent = false) : this(key, modifierType, objectName, controllerConfig, persistent) {
            Incompatibles = incompatibles.ToList();            
        }

        public ModifierConfig(
            string key,
            Type modifierType, 
            IEnumerable<string> incompatibles, 
            IEnumerable<string> ignoredMonsters, 
            string objectName = "Modifiers",
            ModifierControllerConfig? controllerConfig = null,
            bool persistent = false) : this(key, modifierType, incompatibles, objectName, controllerConfig, persistent) {
            IgnoredMonsters = ignoredMonsters.ToList();
        }

        public override int GetHashCode() {
            return HashCode.Combine(Key.GetHashCode(), Incompatibles.GetHashCode(), IgnoredMonsters.GetHashCode());
        }
    }
}
