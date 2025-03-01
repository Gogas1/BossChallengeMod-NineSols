using MonoMod.Utils;
using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BossChallengeMod.Global {
    public static class LocalizationResolver {
        private static Dictionary<string, string> translationDictionary = new();
        
        private static Dictionary<string, List<Dictionary<string, string>>> outerTranslations = new();
        private static string _currentLanguage;

        internal static void LoadLanguage(string language = "en-us") {
            translationDictionary.Clear();
            translationDictionary.AddRange(LoadLanguageEmbedded(language));
            _currentLanguage = language;
        }

        private static Dictionary<string, string> LoadLanguageFile(string language) {
            string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string targetFile = Path.Combine(executingDir, $"translations_{language}.json");
            if (File.Exists(targetFile)) {
                string json = File.ReadAllText(targetFile);
                return JsonUtils.Deserialize<Dictionary<string, string>>(json) ?? [];
            }

            return [];
        }

        private static Dictionary<string, string> LoadLanguageEmbedded(string language) {
            var translations = AssemblyUtils.GetEmbeddedJson<Dictionary<string, string>>($"BossChallengeMod.Resources.Languages.translations_{language}.json");

            if (translations is not null && outerTranslations.TryGetValue(language, out var languageOuterTranslations)) {
                foreach (var translation in languageOuterTranslations.SelectMany(outerDict => outerDict)) {
                    if (!translations.TryAdd(translation.Key, translation.Value)) {
                        Log.Warning($"Could not add translation with the key {translation.Key} for the language {language}. Keys collision.");
                    }
                }
            }

            return translations ?? [];
        }

        public static void AddTranslations(string language, Dictionary<string, string> translations) {
            if (!outerTranslations.TryGetValue(language, out var languageOuterTranslations)) {
                outerTranslations.Add(language, new());
                outerTranslations[language].Add(translations);
            }
            else {
                languageOuterTranslations.Add(translations);
            }

            if(_currentLanguage == language) {
                foreach (var item in translations) {
                    translations.TryAdd(item.Key, item.Value);
                }
            }
        }

        public static string Localize(string key) {
            if(translationDictionary.TryGetValue(key, out string value)) {
                return value;
            }

            return key;
        }
    }
}
