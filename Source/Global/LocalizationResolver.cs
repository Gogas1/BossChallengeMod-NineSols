using MonoMod.Utils;
using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BossChallengeMod.Global {
    public class LocalizationResolver {
        private Dictionary<string, string> translationDictionary = new();

        public void LoadLanguage(string language = "en-us") {
            translationDictionary.Clear();
            translationDictionary.AddRange(LoadLanguageEmbedded(language));
        }

        private Dictionary<string, string> LoadLanguageFile(string language) {
            string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string targetFile = Path.Combine(executingDir, $"translations_{language}.json");
            if (File.Exists(targetFile)) {
                string json = File.ReadAllText(targetFile);
                return JsonUtils.Deserialize<Dictionary<string, string>>(json) ?? [];
            }

            return [];
        }

        private Dictionary<string, string> LoadLanguageEmbedded(string language) {
            var translations = AssemblyUtils.GetEmbeddedJson<Dictionary<string, string>>($"Resources.Languages.translations_{language}.json");
            return translations ?? [];
        }

        public string Localize(string key) {
            if(translationDictionary.TryGetValue(key, out string value)) {
                return value;
            }

            return key;
        }
    }
}
