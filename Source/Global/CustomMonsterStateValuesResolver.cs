using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Global {
    public class CustomMonsterStateValuesResolver {
        private Dictionary<string, MonsterBase.States> statesDictionary = new();
        private List<int> originalValuesCast = Enum.GetValues(typeof(MonsterBase.States)).Cast<int>().ToList();

        public MonsterBase.States GetState(string name) {
            if (statesDictionary.TryGetValue(name, out MonsterBase.States state)) {
                return state;
            } else {
                var maxOriginalValue = originalValuesCast.Any() ? originalValuesCast.Max() : default;
                var maxDictionaryValue = statesDictionary.Any() ? statesDictionary.Values.Max(s => (int)s) : default;

                MonsterBase.States newState = (MonsterBase.States)(Math.Max(maxOriginalValue, maxDictionaryValue) + 1);
                statesDictionary.Add(name, newState);

                return newState;
            }
        }
    }
}
