using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Preloading {
    public interface IPreloadTarget {
        void Set(GameObject? preloaded);
    }
}
