using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Interfaces {
    public interface IResettableComponent {
        int GetPriority();
        void ResetComponent();
    }
}
