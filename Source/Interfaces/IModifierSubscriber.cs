using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Interfaces {
    public interface IModifierSubscriber {

        public void NotifySubscriber(object args);

    }
}
