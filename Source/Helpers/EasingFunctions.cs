using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Helpers {
    public static class EasingFunctions {
        public static float Linear(float t) {
            return t;
        }
        public static float EaseIn(float t) {
            return t * t;
        }
        public static float EaseOut(float t) {
            return t * (2f - t);
        }
        public static float EaseInOut(float t) {
            return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
        }
    }
}
