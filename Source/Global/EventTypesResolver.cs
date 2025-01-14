using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Global {
    public class EventTypesResolver {
        private Dictionary<string, RCGEventType> types = new();

        public RCGEventType RequestType(string type) {
            var targetType = types.GetValueOrDefault(type);

            if (targetType == null) {
                targetType = ScriptableObject.CreateInstance<RCGEventType>();
                targetType.name = type;
                types.Add(type, targetType);
            }

            return targetType;
        }
    }
}
